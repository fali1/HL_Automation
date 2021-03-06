﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Drawing;                         // for taking screenshot
using System.Drawing.Imaging;                 // for taking screenshot
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;           // for Actions API
using OpenQA.Selenium.Interactions.Internal;  // for Actions API
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.PageObjects;
using System.Windows;
using System.Linq;
using System.Collections;


namespace HL_Breadth
{
    [TestFixture]
    public class f_Breadth_Hnp : HL_Base_Class
    {

      //  private IWebDriver driver;

        private StringBuilder verificationErrors;

        private string baseURL;

        private bool acceptNextAlert = true;

        public string login_name = "admin"; //used in login and session manager testcases

        public string login_pwd = "admin";

        public string welcome_username = "Welcome admin"; //used in login testcase to verify 'Welcome user' label after login

        public string browser = "Mozilla"; //used in session manager according to browser(firefox,chrome,IE)

        public string driver_type;

     //   string browser_type;

     //   string browser_name;

        string user_label;

        string trimmed_user_label;

        string create_directory_path = @".\Screenshots_Testcase_Results";

    
        string create_directory_path_with_time;



        [TestFixtureSetUp]

        public void SetupTest()
        {

            // driver = new ChromeDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch chrome browser


            // driver = new InternetExplorerDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch IE browser

            // driver = new SafariDriver();// launch safari browser

            // driver = new FirefoxDriver();// launch firefox browser

            //     System.Diagnostics.Debugger.Launch();// launch debugger

            string[] lines_local = read_from_file("login_credentials"); // return all the data in the form of array

            browser_name = get_browser();// get browser name ( firefox , safari , chrome , internetexplorer )
            Console.WriteLine("Browser Name got from xml file:" + " " + browser_name);

            switch (browser_name)
            {
                case "firefox":
                    var profile = new FirefoxProfile();
                    profile.SetPreference("dom.forms.number", false);
                    driver = new FirefoxDriver(profile);// launch firefox browser
                    break;

                case "safari":
                    driver = new SafariDriver();// launch safari browser
                    break;

                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.AddArguments("test-type");
                    driver = new ChromeDriver(@".\drivers", options);
                    break;

                case "internetexplorer":
                    driver = new InternetExplorerDriver(@".\drivers"); // launch IE browser
                    break;
            }

            DateTime todaydatetime = DateTime.Now;          // Use current time
            string format = "yyyy_MM_dd_hh_mm_ss";          // Use this format Year_Month_Date_Hour_Minute_Second => 2014_04_21_02_35_09
            Console.WriteLine(todaydatetime.ToString(format));

            /*    Console.WriteLine(time.ToString("U"));    // output U =>   Friday, February 27, 2009 8:12:22 PM
                  Console.WriteLine(time.ToString("G"));    // output G =>   2/27/2009 12:12:22 PM
            
              M       display one-digit month number          
              d       display one-digit day of the MONTH      
              h       display one-digit hour on 12-hour scale 
              mm      display two-digit minutes
              yy      display two-digit year                  
             */

            create_directory_path_with_time = create_directory_path + todaydatetime.ToString(format);
            Console.WriteLine(create_directory_path_with_time);
            if (!Directory.Exists(create_directory_path_with_time))
            {
                Directory.CreateDirectory(create_directory_path_with_time);
            }

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(25000));//wait for request 

            //driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 30, 15));

            driver_type = driver.GetType().ToString();// get driver type ( firefox , safari , chrome , internetexplorer )

            Console.WriteLine("Driver Type:" + " " + driver_type);

            string[] lines_url = read_url_from_file(@"url");

            baseURL = lines_url[0]; //url of application

            driver.Navigate().GoToUrl(baseURL);

            driver.Manage().Window.Maximize();//maximize browser

            login_name = lines_local[0]; //used in login and session manager testcases

            login_pwd = lines_local[1];

            driver.FindElement(By.Id("username")).Clear();

            driver.FindElement(By.Id("username")).SendKeys(login_name);

            driver.FindElement(By.Id("password")).Clear();

            driver.FindElement(By.Id("password")).SendKeys(login_pwd);

            driver.FindElement(By.CssSelector("a.c_btn_large1.login_button")).Click();// user login button

            Thread.Sleep(3000);

            takescreenshot("login");

            Console.WriteLine("User label:" + "*" + driver.FindElement(By.XPath("//li[@class='user_name']")).Text + "*");

            user_label = driver.FindElement(By.XPath("//li[@class='user_name']")).Text.ToString();

            trimmed_user_label = user_label.TrimEnd();

            Console.WriteLine("User label Trimmed at the end:" + "*" + trimmed_user_label + "*");

            Assert.AreEqual(trimmed_user_label, "Welcome admin");

            verificationErrors = new StringBuilder();
        }



        [Test]
        public void a_Hnp_Configuration()
        {
            string hnp_msngr_name = "hnp_two_way_messenger";
            string hnp_carr_name = "hnp_two_way_carrier";

            // Example #2 
            // Read each line of the file into a string array. Each element 
            // of the array is one line of the file. 
            
            string[] lines = System.IO.File.ReadAllLines(@".\push_notifications.txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of push_notifications.txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            // HNP Messenger
            WaitForChrome(5000, browser_name);

            driver.FindElement(By.XPath("//a[text()='Sys Admin']")).Click();
            Thread.Sleep(2000);
            WaitForChrome(5000,browser_name);

            Actions action = new Actions(driver);

            driver.FindElement(By.XPath("//a[text()='Messengers']")).Click();
            action.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(2000);

            WaitForChrome(5000,browser_name);

            driver.FindElement(By.XPath("//li[text()='Hiplink']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='HNP 2 Way']")).Click();

            driver.FindElement(By.Id("btnMsngr")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("txtMessangerName")).Clear();
            driver.FindElement(By.Id("txtMessangerName")).SendKeys(hnp_msngr_name);

            driver.FindElement(By.Id("btnSaveMsngr")).Click();
            Thread.Sleep(3000);
            takescreenshot("hnp_messenger_created");



            // HNP Carrier

            driver.FindElement(By.XPath("//a[text()='Sys Admin']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Carriers']")).Click();
            action.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(2000);

            WaitForChrome(5000, browser_name);

            driver.FindElement(By.XPath("//li[text()='Hiplink']")).Click();

            driver.FindElement(By.XPath("//li[text()='HNP 2 Way']")).Click();

            driver.FindElement(By.Id("btnaddcarrier")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("carrierName")).Clear();
            driver.FindElement(By.Id("carrierName")).SendKeys(hnp_carr_name);

            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);
            takescreenshot("hnp_carrier_created");


            // Configuration of HNP

            check_driver_type(driver_type, "settings", "Configuration", "Settings");

            Assert.AreEqual("HNP Configuration", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            driver.FindElement(By.LinkText("Edit")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//span[text()='Enable Settings']")).Click();

            driver.FindElement(By.XPath("//span[contains(text(),'Enable Emergency Mode')]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='"+hnp_carr_name+"']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Push Notifications']")).Click();
            WaitForChrome(5000,browser_name);

            driver.FindElement(By.XPath("//span[text()='Enable APNS (Apple Push Notification Service)']")).Click();

            driver.FindElement(By.Id("sslCertificate")).Clear();
            driver.FindElement(By.Id("sslCertificate")).SendKeys(lines[0]);
            
            driver.FindElement(By.Id("privateKey")).Clear();
            driver.FindElement(By.Id("privateKey")).SendKeys(lines[1]);

            driver.FindElement(By.Id("privateKeyPassword")).Clear();
            driver.FindElement(By.Id("privateKeyPassword")).SendKeys(lines[2]);

            driver.FindElement(By.XPath("//span[text()='Enable GCMS (Google Cloud Messaging Service)']")).Click();

            driver.FindElement(By.Id("serverId")).Clear();
            driver.FindElement(By.Id("serverId")).SendKeys(lines[3]);

            driver.FindElement(By.Id("serverKey")).Clear();
            driver.FindElement(By.Id("serverKey")).SendKeys(lines[4]);

            takescreenshot("push_notifications_settings");


            driver.FindElement(By.LinkText("Save")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("btnOk")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//a[text()='General Settings']")).Click();

            Assert.AreEqual(true,driver.FindElement(By.XPath("(//span[@class='c_f_info_text'])[1]")).Text.Equals("Yes"));
            Assert.AreEqual(true,driver.FindElement(By.XPath("(//span[@class='c_f_info_text'])[4]")).Text.Equals("Yes"));

            takescreenshot("hnp_configured");

                
            Actions hnp = new Actions(driver);

            
            //hnp.MoveToElement(driver.FindElement(By.Id("settings"))).Perform();

            
            driver.FindElement(By.Id("settings")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.LinkText("General Policy")).Click(); //opening General Policy page
            action.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(2000);
                
            Assert.AreEqual("General Policy", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            driver.FindElement(By.XPath("//b[text()='Enable General Policy']")).Click();
            
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();    // Configure permissions
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//a[text()='System Configuration']")).Click(); // System configuration

            driver.FindElement(By.XPath("(//a[@class='selector'])[9]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[text()='Yes'])[8]")).Click();

            driver.FindElement(By.XPath("//a[text()='Session Configuration']")).Click(); // Session configuration

            driver.FindElement(By.XPath("(//a[@class='selector'])[14]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[text()='Yes'])[12]")).Click();

            driver.FindElement(By.XPath("//a[text()='Messaging Configuration']")).Click(); // Messaging configuration

            driver.FindElement(By.XPath("(//a[@class='selector'])[15]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//li[text()='Yes'])[13]")).Click();

            driver.FindElement(By.XPath("//a[text()='Alert Configuration']")).Click(); // Alert configuration

            driver.FindElement(By.XPath("(//a[@class='selector'])[21]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//li[text()='01'])[4]")).Click();

            driver.FindElement(By.XPath("//a[text()='File Transfer Configuration']")).Click(); // File Transfer configuration

            driver.FindElement(By.XPath("(//a[@class='selector'])[31]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//li[text()='01'])[9]")).Click();


            driver.FindElement(By.LinkText("Save")).Click();
            Thread.Sleep(1000);
            takescreenshot("hnp_general_policy_configured");


            hnp.MoveToElement(driver.FindElement(By.Id("settings"))).Perform();
            Thread.Sleep(1000);


            driver.FindElement(By.LinkText("Activation")).Click(); //opening Activation page
            action.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(2000);

            Assert.AreEqual("Activation", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);
            Thread.Sleep(2000);

                /*--------------------------------------------------*/

                // Start/Restart HNP manager Service

                driver.FindElement(By.Id("administration")).Click();

                driver.FindElement(By.LinkText("Services")).Click();

                Thread.Sleep(2000);
                WaitForChrome(5000,browser_name);


                if (IsElementPresent(By.XPath("//*[@id='item_17']/td[2]/a[@class='action service_action_play']"))) //HNP Manager's play button
                {

                    driver.FindElement(By.XPath("//*[@id='item_17']/td[2]/a[@class='action service_action_play']")).Click();
                    WaitForChrome(5000, browser_name);
                    Thread.Sleep(2000);


                    if (IsElementPresent(By.XPath("//*[@id='item_17']/td[3]/a[@class='service_action_refresh']"))) //HNP manager restart button
                    {

                        driver.FindElement(By.XPath("//*[@id='item_17']/td[3]/a[@class='service_action_refresh']")).Click();
                        WaitForChrome(5000, browser_name);
                        Thread.Sleep(2000);

                        Console.WriteLine("HNP Manager  Services restarted");

                        takescreenshot("hnp_service");

                    }

                    else
                    {

                        Assert.Fail("HNP Manager  Services restart service button not found");

                    }

                }

                else
                {

                    Assert.Fail("HNP Manager Services play service button not found");

                }

        }




        public void takescreenshot(string suffix)
        {

            string image_name = suffix;

            Screenshot Shot = ((ITakesScreenshot)driver).GetScreenshot();

            Shot.SaveAsFile(create_directory_path_with_time + "\\" + image_name + ".png", System.Drawing.Imaging.ImageFormat.Png);

        }

     

        public string random_alphanum(string alphanumeric)
        {

            Random r = new Random();
            string random_alpha = alphanumeric + r.Next(1, 1000);

            return random_alpha;


        }

        [TestFixtureTearDown]
        public void TeardownTest()
        {
            try
            {
                WaitForChrome(5000, browser_name);
                driver.FindElement(By.XPath("//a[text()='Logout']")).Click();
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            //  Assert.AreEqual("", verificationErrors.ToString());
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine(driver.FindElement(by));
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}

