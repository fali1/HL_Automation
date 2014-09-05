using System;
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
    public class j_Smoke_Global_search
    {

        private IWebDriver driver;

        private StringBuilder verificationErrors;

        private string baseURL;

        private bool acceptNextAlert = true;

        public string login_name = "admin"; //used in login and session manager testcases

        public string login_pwd = "admin";

        public string welcome_username = "Welcome admin"; //used in login testcase to verify 'Welcome user' label after login

        public string browser = "Mozilla"; //used in session manager according to browser(firefox,chrome,IE)

        public string driver_type;

        string browser_type;

        string browser_name;

        string user_label;

        string trimmed_user_label;

        string create_directory_path = @".\Screenshots_Testcase_Results";

        string create_directory_path_directory = @"C:\Program Files (x86)\Hiplink Software\HipLink\new_directory";

        int test_result_exist = 0;

        string create_directory_path_with_time;

        string new_dir = "new_directory";

        string messenger_name = "smtp_messenger";

        string carrier_name = "smtp_carrier";



        [TestFixtureSetUp]

        public void SetupTest()
        {

            // driver = new ChromeDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch chrome browser


            // driver = new InternetExplorerDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch IE browser

            // driver = new SafariDriver();// launch safari browser

            // driver = new FirefoxDriver();// launch firefox browser

            //     System.Diagnostics.Debugger.Launch();// launch debugger

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
                    driver = new ChromeDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch chrome browser
                    break;

                case "internetexplorer":
                    driver = new InternetExplorerDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch IE browser
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


            // Read each line of the file into a string array. Each element 
            // of the array is one line of the file. 

            string[] lines = System.IO.File.ReadAllLines(@".\url.txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of url.txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\n" + line);
            }


            baseURL = lines[0]; //url of application

            driver.Navigate().GoToUrl(baseURL);

            driver.Manage().Window.Maximize();//maximize browser

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
        public void Global_Search_Panel()
        {
            string user = "fahad";
            string department = "Default";
            string receiver = "receiver_smtp";
            string user_group = "new_user_group";
            string receiver_group = "Broadcast_Group3";

            // search for user

            driver.FindElement(By.Id("btnSearchOptions")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[text()='User']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("txtSearch")).Click();
            driver.FindElement(By.Id("txtSearch")).Clear();
            driver.FindElement(By.Id("txtSearch")).SendKeys(user);
            driver.FindElement(By.Id("btnSearchGo")).Click();
            Thread.Sleep(2000);
            if (driver.FindElement(By.XPath("(//div[@class='result-row-title'])[1]")).Text.Contains(user))
            {
                Console.WriteLine("User record fetched ...");
                driver.FindElement(By.Id("btnClose")).Click();
                //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }
            else
            {
                Assert.Fail("User record not fetched ...");
            }

            // search for Department

            driver.FindElement(By.Id("btnSearchOptions")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[text()='Department']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("txtSearch")).Click();
            driver.FindElement(By.Id("txtSearch")).Clear();
            driver.FindElement(By.Id("txtSearch")).SendKeys(department);
            driver.FindElement(By.Id("btnSearchGo")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Department")).Click();
            Thread.Sleep(2000);
            if (driver.FindElement(By.XPath("(//div[@class='result-row-title'])[2]")).Text.Contains(department))
            {
                Console.WriteLine("Department record fetched ...");
                driver.FindElement(By.Id("btnOKSearch")).Click();
                //   driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }
            else
            {
                Assert.Fail("Department record not fetched ...");
            }

            // search for Receiver

            driver.FindElement(By.Id("btnSearchOptions")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[text()='Receiver']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("txtSearch")).Click();
            driver.FindElement(By.Id("txtSearch")).Clear();
            driver.FindElement(By.Id("txtSearch")).SendKeys(receiver);
            driver.FindElement(By.Id("btnSearchGo")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Receiver")).Click();
            Thread.Sleep(2000);
            if (driver.FindElement(By.XPath("(//div[@class='result-row-title'])[2]")).Text.Contains(receiver))
            {
                Console.WriteLine("Receiver record fetched ...");
                driver.FindElement(By.Id("btnClose")).Click();
                //   driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }
            else
            {
                Assert.Fail("Receiver record not fetched ...");
            }

            // search for User Group

            driver.FindElement(By.Id("btnSearchOptions")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[text()='User Group']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("txtSearch")).Click();
            driver.FindElement(By.Id("txtSearch")).Clear();
            driver.FindElement(By.Id("txtSearch")).SendKeys(user_group);
            driver.FindElement(By.Id("btnSearchGo")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("User Group")).Click();
            Thread.Sleep(2000);
            if (driver.FindElement(By.XPath("(//div[@class='result-row-title'])[2]")).Text.Contains(user_group))
            {
                Console.WriteLine("User Group record fetched ...");
                driver.FindElement(By.Id("btnOKSearch")).Click();
                //    driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }
            else
            {
                Assert.Fail("User Group record not fetched ...");
            }


            // search for Receiver Group

            driver.FindElement(By.Id("btnSearchOptions")).Click();
            Thread.Sleep(2000);
                   driver.FindElement(By.XPath("//span[text()='Receiver Groups']")).Click();
                   Thread.Sleep(2000);
                   driver.FindElement(By.Id("txtSearch")).Click();
                   driver.FindElement(By.Id("txtSearch")).Clear();
                   driver.FindElement(By.Id("txtSearch")).SendKeys(receiver_group);
                   driver.FindElement(By.Id("btnSearchGo")).Click();
                   Thread.Sleep(2000);
                   driver.FindElement(By.LinkText("Receiver Groups")).Click();
                   Thread.Sleep(2000);
                   if (driver.FindElement(By.XPath("(//div[@class='result-row-title'])[2]")).Text.Contains(receiver_group))
                   {
                       Console.WriteLine("User Group record fetched ...");
                       driver.FindElement(By.Id("btnClose")).Click();
                   }
                   else
                   {
                       Assert.Fail("User Group record not fetched ...");
                   }
                   



        }
        
        
        
        
        
        
        
        
        
        public void check_driver_type(string drivertype, string id_name, string link_text, string a_text) //drivertype= browser , id_name = landing page , link_text = panel(e.g Add user page) 
        {

            Thread.Sleep(2000);

            if (drivertype.ToString() == "OpenQA.Selenium.Safari.SafariDriver") //for safari
            {

                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);

                driver.FindElement(By.XPath(".//*[@id='" + id_name + "']/a")).Click(); //goto landing for particular ID
                Thread.Sleep(2000);



                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='" + link_text + "']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);



            }

            else if (drivertype.ToString() == "OpenQA.Selenium.Chrome.ChromeDriver" || drivertype.ToString() == "OpenQA.Selenium.Firefox.FirefoxDriver") //for firefox and chrome
            {

                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);

                //a[contains(text(),'On-Duty')])[2]

                driver.FindElement(By.XPath("//li[@id='" + id_name + "']/a[text()='" + a_text + "']")).Click(); //goto landing for particular ID
                Thread.Sleep(2000);



                Actions a1c = new Actions(driver);
                Thread.Sleep(2000);

                a1c.MoveToElement(driver.FindElement(By.XPath("//div[@class='footer']"))).Perform();
                Thread.Sleep(3000);



                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='" + link_text + "']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);

                /*
                if (link_text.Equals("Escalation"))
                {
                    driver.FindElement(By.XPath("(//a[contains(text(),'"+link_text+"')])[4]")).Click();
                    Thread.Sleep(2000);
                }
                else
                {
                    driver.FindElement(By.XPath("(//a[text()='" + link_text + "'])[2]")).Click();
                    Thread.Sleep(2000);
                }*/



                driver.FindElement(By.XPath(".//*[@id='" + id_name + "']/a")).Click(); //goto landing for particular ID
                Thread.Sleep(2000);

                hover_func(id_name, link_text, a_text);
                Thread.Sleep(2000);

            }

            else // for IE
            {

                // drivertype.ToString() == "OpenQA.Selenium.IE.InternetExplorerDriver"



                hover_func(id_name, link_text, a_text);
                Thread.Sleep(2000);
            }

        }



        public void hover_func(string id_name, string link_text, string a_text)
        {

            //------ Hover functionality and click ------

            var hoveritem = driver.FindElement(By.Id(id_name));

            Actions action1 = new Actions(driver); //simply my webdriver
            Thread.Sleep(2000);

            action1.MoveToElement(hoveritem).Perform(); //move to list element that needs to be hovered

            Thread.Sleep(3000);

            driver.FindElement(By.XPath("(//a[text()='" + link_text + "'])[1]")).Click();
            Thread.Sleep(3000);


            //------ Focus out the mouse to disappear hovered dialog ------

            Actions action2 = new Actions(driver);
            Thread.Sleep(2000);

            action2.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(3000);

        }




        public void takescreenshot(string suffix)
        {

            string image_name = suffix;

            Screenshot Shot = ((ITakesScreenshot)driver).GetScreenshot();

            Shot.SaveAsFile(create_directory_path_with_time + "\\" + image_name + ".png", System.Drawing.Imaging.ImageFormat.Png);

        }

        public string get_browser() // Get browser name from Browsers.xml file
        {

            using (XmlTextReader reader = new XmlTextReader(@".\Browsers.xml"))
            {

                while (reader.Read())
                {

                    if (reader.IsStartElement())
                    {

                        if (reader.Name == "browser")
                        {

                            browser_type = reader.ReadString(); //read browser name under <browser> tag
                            Console.WriteLine("browser: " + browser_type);
                            break;

                        }

                    }

                }

            }

            return browser_type;
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



