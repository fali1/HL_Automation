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


namespace HL_Smoke
{
    [TestFixture]
    public class k_Smoke_Help_About_Session_License_key : HL_Base_Class
    {

     //   private IWebDriver driver;

        private StringBuilder verificationErrors;

        private string baseURL;

        private bool acceptNextAlert = true;

        public string login_name = "admin"; //used in login and session manager testcases

        public string login_pwd = "admin";

        public string welcome_username = "Welcome admin"; //used in login testcase to verify 'Welcome user' label after login

        public string browser = "Mozilla"; //used in session manager according to browser(firefox,chrome,IE)

        public string driver_type;

     //   string browser_type;

        string user_label;

        string trimmed_user_label;

        string create_directory_path = @".\Screenshots_Help_Testcase";

        string create_directory_path_with_time;





        [TestFixtureSetUp]

        public void SetupTest()
        {

            // driver = new ChromeDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch chrome browser


            // driver = new InternetExplorerDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch IE browser

            // driver = new SafariDriver();// launch safari browser

            // driver = new FirefoxDriver();// launch firefox browser

            //   System.Diagnostics.Debugger.Launch();// launch debugger

            string[] lines_local = read_from_file("login_credentials"); // return all the data in the form of array

            browser_name = get_browser();// get browser name ( firefox , safari , chrome , internetexplorer )
            Console.WriteLine("Browser Name got from xml file:" + " " + browser_name);

            switch (browser_name)
            {
                case "firefox":
                    driver = new FirefoxDriver();// launch firefox browser
                    break;

                case "safari":
                    driver = new SafariDriver();// launch safari browser
                    break;

                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.AddArguments("test-type");
                    driver = new ChromeDriver(@".\drivers",options);
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

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
            

            driver_type = driver.GetType().ToString();// get driver type ( firefox , safari , chrome , internetexplorer )

            Console.WriteLine("Driver Type:" + " " + driver_type);
            string[] line_url = read_url_from_file("url"); //url of application

            baseURL = line_url[0];

            driver.Navigate().GoToUrl(baseURL);

            driver.Manage().Window.Maximize();//maximize browser

            login_name = lines_local[0]; //used in login and session manager testcases

            login_pwd = lines_local[1];

            driver.FindElement(By.Id("username")).Clear();

            driver.FindElement(By.Id("username")).SendKeys(login_name);

            driver.FindElement(By.Id("password")).Clear();

            driver.FindElement(By.Id("password")).SendKeys(login_pwd);

            driver.FindElement(By.CssSelector("a.c_btn_large1.login_button")).Click();// user login button

            WaitForElementToExist("entityTitle", driver);

            Thread.Sleep(3000);

            takescreenshot("login");

            Console.WriteLine("User label:" + "*" + driver.FindElement(By.XPath("//li[@class='user_name']")).Text + "*");

            user_label = driver.FindElement(By.XPath("//li[@class='user_name']")).Text.ToString();

            trimmed_user_label = user_label.TrimEnd();

            Console.WriteLine("User label Trimmed at the end:" + "*" + trimmed_user_label + "*");

            Assert.AreEqual(trimmed_user_label, "Welcome "+login_name);

            verificationErrors = new StringBuilder();
        }



        [Test]
        public void a_User_Session()
        {

            check_driver_type(driver_type, "administration", "Sessions Manager", "Sys Admin");

            Assert.AreEqual("Sessions Manager", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            Console.WriteLine(driver.FindElement(By.XPath("//div[@class='tab_block tab_session tab_active']")).Text);

            if (!(driver.FindElement(By.XPath("//div[@class='tab_block tab_session tab_active']")).Text.Contains(login_name)))
            {
                Assert.Fail("Session Manager Failed ...");
            }

            else
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Session Manager Passed ...    ^^^^^^^^^^^^^^^^^^^^^");
                
            }

        }


        [Test]
        public void b_License_Page()
        {

            check_driver_type(driver_type, "administration", "Install Licence", "Sys Admin");

            Assert.AreEqual("Install Licence", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@class='license_table']/ul/li[@class='product_name']")).Text + "*");

            if (!(driver.FindElement(By.XPath("//div[@class='license_table']/ul/li[@class='product_name']")).Text.Contains("HipLink") &&
                driver.FindElement(By.XPath("//div[@class='license_table']/ul/li[@class='version']")).Text.Contains("5.0") &&
                driver.FindElement(By.XPath("//div[@class='license_table']/ul/li[4]")).Text.Contains("Full")))
            {
                Assert.Fail("License page Failed...");
            }
            else
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^  License page Passed...  ^^^^^^^^^^^^^^^^^^^");
            }
        }



        [Test]
        public void c_About_Page()
        {

            check_driver_type(driver_type, "administration", "About Hiplink", "Sys Admin");

            Assert.AreEqual("About", driver.FindElement(By.XPath("//div[@class='main_container pg_about']/h1")).Text);


            if (!(driver.FindElement(By.Id("lblProductName")).Text.Equals("HipLink") &&
                driver.FindElement(By.XPath("//i[@class='product_name_sec']")).Text.Contains("Hiplink Alert Notification Solutions")))
            {
                Assert.Fail("About page Failed...");
            }
            else
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^  About page Passed...  ^^^^^^^^^^^^^^^^^^^");
            }
        }



        [Test]
        public void d_Help()
        {
            ICollection<string> windowids = null;
            IEnumerator<String> iter = null;
            String mainWindowId = null;
            String curWindow = null;

            Thread.Sleep(1000);

            mainWindowId = driver.CurrentWindowHandle;
            Console.WriteLine("Main window handle: " + mainWindowId);//main window id

            // The below step would use whatever element you need to use to 
            // open the new window. 
            driver.FindElement(By.LinkText("Help")).Click();

            //    Assert.AreEqual("Help", driver.FindElement(By.XPath("//div[@class='main_container pg_help']/h1")).Text);

            //the above should open a new tab on current browser window BUT Selenium will open it as a new browser window

            Thread.Sleep(25);

            takescreenshot("help");

            windowids = null;
            windowids = driver.WindowHandles; // returns an ID for every opened window
            iter = windowids.GetEnumerator(); ;  // iterate through open browser and print out their ids. One id only for now.
            Console.WriteLine("List Window IDs. There should be 2 id now");
            Console.WriteLine("=========================================");

            for (int i = 0; i < windowids.Count; i++)
            {
                Console.WriteLine(windowids.ElementAt(i));
                if (i != 0)
                {

                    driver.SwitchTo().Window(windowids.ElementAt(i)).Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5L));

                    Console.WriteLine(driver.FindElement(By.XPath("//div[@class='main_container pg_help']")).Text);
                    Console.WriteLine(driver.FindElement(By.XPath("//li[@class='product_name']")).Text);


                    if (driver.FindElement(By.XPath("//div[@class='main_container pg_help']")).Text.Contains("Help") &&

                        driver.FindElement(By.XPath("//li[@class='product_name']")).Text.Contains("Manual"))
                    {

                        // Do some operations in the new window and close it 
                        driver.Close();

                        Console.WriteLine("^^^^^^^^^^^^^^^  Help Passed ... ^^^^^^^^^^^^^^^^");
                    }

                    else
                    {
                        Assert.Fail("Help Failed ...");
                    }


                }

                else
                {
                    Console.WriteLine("We are at main window right now! ");
                }
            }
            Thread.Sleep(3000);

            // This switches to the window by name. You could also search for 
            // the newly opened window handle and switch using that. 
            // Code that does this is left as an exercise for you to complete on your own. 


            // Switch "focus" back to the original window. 
            //   driver.SwitchTo().Window(originalHandle);

            //-------------------------------------------------------------------      



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


