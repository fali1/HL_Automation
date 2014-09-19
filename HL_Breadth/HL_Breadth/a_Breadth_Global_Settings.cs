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

namespace HL_Breadth
{
    [TestFixture]
    public class a_Breadth_Global_Settings
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
        public void Global_Settings_Panel()
        {
            check_driver_type(driver_type, "administration", "Global Settings", "Sys Admin");

            Assert.AreEqual("Global Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            //  hover_func("administration", "Global Settings");

          
            //Common section (Display)
            driver.FindElement(By.XPath("(//a[text()='Common'])[1]")).Click();

            driver.FindElement(By.Id("btnEditMain")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtnumOfRecordReportPage")).Clear();
            driver.FindElement(By.Id("txtnumOfRecordReportPage")).SendKeys("12");


            //Receiver section
            driver.FindElement(By.LinkText("Receiver")).Click();


            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();//Detail Receiver/User Display
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[1]/div/ul/li[text()='Yes']")).Click();

            /*
            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();//Enable Receiver First Name
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[2]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click();//Enable Receiver Last Name
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[3]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[7]")).Click();//Enable Receiver Security Code
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[4]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[8]")).Click();//Enable Receiver Status
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[5]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[9]")).Click();//Allow Receiver Login
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[6]/div/ul/li[text()='Yes']")).Click();

            Thread.Sleep(2000);
            

            // scroll down the screen until "Receiver Logon via Assigned Owner" dropdown is displayed

            IWebElement element = driver.FindElement(By.XPath("(//a[@class='selector'])[10]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(500);

            driver.FindElement(By.XPath("(//a[@class='selector'])[10]")).Click();//Receiver Logon via Assigned Owner
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[7]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.Id("txtemailSubject")).Clear();
            driver.FindElement(By.Id("txtemailSubject")).SendKeys("Failed over email subject");//Failed Over Email Subject 

            // scroll down the screen until "Failed Over Email Message" textbox is displayed

            IWebElement element12 = driver.FindElement(By.Id("txtemailMsg"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element12);
            Thread.Sleep(500);

            driver.FindElement(By.Id("txtemailMsg")).Clear();
            driver.FindElement(By.Id("txtemailMsg")).SendKeys("failed over email message");//Failed Over Email Message


            // driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[12]/div/a[2]")).SendKeys(OpenQA.Selenium.Keys.Tab);

            // scroll down the screen until "Notify Admin when receiver changes his/her own schedule" dropdown is displayed

            IWebElement element123 = driver.FindElement(By.XPath("(//a[@class='selector'])[11]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element123);
            Thread.Sleep(500);

            driver.FindElement(By.XPath("(//a[@class='selector'])[11]")).Click();//Notify Admin when receiver changes his/her own schedule
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[11]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[12]")).Click();//Notify Receiver on covering other receiver
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[12]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//b[text()='Receiver to send Test Message']")).Click();

            driver.FindElement(By.Id("txtAenableReceiver")).Clear();
            driver.FindElement(By.Id("txtAenableReceiver")).SendKeys("Receiver to send test message");
             */

            // driver will scrol down the screen untill the required element is in view... 

            IWebElement element1 = driver.FindElement(By.XPath("//b[text()='Enable Receiver Attributes']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("var $myLabel = $(arguments[0]), $myParent = $myLabel.parents('.common_scrollbar'); $myParent.mCustomScrollbar('scrollTo', 'label[for=' + $myLabel.siblings('input').attr('id') + ']');", element1);
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//b[text()='Enable Receiver Attributes']")).Click();

            driver.FindElement(By.XPath("//li[text()='Add New']")).Click();

            driver.FindElement(By.Id("attrib_")).SendKeys("A1");

            
            // Recipient User


            driver.FindElement(By.LinkText("Recipient User")).Click();

            driver.FindElement(By.Id("receipentDeviceName")).Clear();

            driver.FindElement(By.Id("receipentDeviceName")).SendKeys("Recipient_user_name");



            //Common section

            driver.FindElement(By.XPath("(//a[text()='Common'])[2]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[14]")).Click();//Enable time-stamp on all messages
            driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[1]/div/ul/li[text()='Yes']")).Click();

            /*  driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[2]/div/a[2]")).Click();//Enable sender name on all messages
              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[2]/div/ul/li[text()='Yes']")).Click();

              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[3]/div/a[2]")).Click();//Put sender name at the beginning of message
              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[3]/div/ul/li[text()='Yes']")).Click();

              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[4]/div/a[2]")).Click();//Include message ID in the message automatically
              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[4]/div/ul/li[text()='Yes']")).Click();

              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[8]/div/a[2]")).Click();//Enable Authorization Code for all messages
              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[8]/div/ul/li[text()='Yes']")).Click();

              // scroll down the screen until "Maximum Resend messages to keep for a User(10-1000)" textbox is displayed

              IWebElement element13 = driver.FindElement(By.Id("txtemailMsg"));
              ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element13);
              Thread.Sleep(500);

              driver.FindElement(By.Id("txtmaxResendMsg")).Clear();//Maximum Resend messages to keep for a User(10-1000)
              driver.FindElement(By.Id("txtmaxResendMsg")).SendKeys("100");

              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[9]/div/a[2]")).Click();//User Description as Signature
              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[9]/div/ul/li[text()='Yes']")).Click();

              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[10]/div/a[2]")).Click();//Enable Send Subject Field
              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[10]/div/ul/li[text()='Yes']")).Click();

              driver.FindElement(By.Id("txtdefaultSmtpSub")).Clear();//Default SMTP Subject
              driver.FindElement(By.Id("txtdefaultSmtpSub")).SendKeys("Default smtp message");

              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[12]/div/a[2]")).Click();//Enable Confidential Messaging
              driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[12]/div/ul/li[text()='Yes']")).Click();

             */

            //Department section
          
            driver.FindElement(By.LinkText("Departments")).Click();



            driver.FindElement(By.XPath("//b[text()='Maximum Receivers Allowed in a Department']")).Click();//Maximum Receivers Allowed in a Department

            driver.FindElement(By.Id("txtMaxRecieverDPT")).Clear();
            driver.FindElement(By.Id("txtMaxRecieverDPT")).SendKeys("100");//Maximum Receivers Allowed in a Department

            driver.FindElement(By.XPath("(//a[@class='selector'])[24]")).Click();//Count Recipient Groups as Members
            driver.FindElement(By.XPath("//div[@id='departmentsEdit']/fieldset[2]/div/ul/li[text()='Yes']")).Click();

            //Session section
            driver.FindElement(By.LinkText("Session")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[25]")).Click();//Temporary session
            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset/div/ul/li[text()='No']")).Click();

            /*    driver.FindElement(By.Id("txtsessionTimeout")).Clear();//Session Timeout (minutes)
                driver.FindElement(By.Id("txtsessionTimeout")).SendKeys("40");

                driver.FindElement(By.Id("txtuserPasswordExpire")).Clear();//User password expires after (days)
                driver.FindElement(By.Id("txtuserPasswordExpire")).SendKeys("10");

                driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[4]/div/a[2]")).Click();//Minimum user password length in characters
                driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[4]/div/ul/li[4]")).Click();

                driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[5]/div/a[2]")).Click();//User password needs at least a numeric, an alphabetic and a special char
                driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[5]/div/ul/li[text()='No']")).Click();
             */

            driver.FindElement(By.Id("btnSave")).Click();

            driver.FindElement(By.Id("btnOk")).Click();

            //Common section (Display)

            driver.FindElement(By.XPath("(//a[text()='Common'])[1]")).Click();

            if (driver.FindElement(By.Id("lblnumOfRecordReportPage")).Text.Equals("12"))
            {
                driver.FindElement(By.LinkText("Receiver")).Click();

                if (driver.FindElement(By.Id("lbldetailDisplay")).Text.Equals("Yes"))
                {
                    driver.FindElement(By.LinkText("Recipient User")).Click();

                    if (driver.FindElement(By.Id("lblRespTemp")).Text.Equals("Recipient_user_name"))
                    {

                        //Common section (Message Sending)
                        driver.FindElement(By.XPath("(//a[text()='Common'])[2]")).Click();

                        if (driver.FindElement(By.Id("lblselenableTimeStamp")).Text.Equals("Yes"))
                        {
                            driver.FindElement(By.LinkText("Departments")).Click();

                            if (driver.FindElement(By.Id("lblMaxRecieverDPT")).Text.Equals("100"))
                            {
                                driver.FindElement(By.LinkText("Session")).Click();

                                if (driver.FindElement(By.Id("lbltemparerySession")).Text.Equals("No"))
                                {
                                    Console.WriteLine("Global Settings Passed...");
                                }
                                else
                                {
                                    Assert.Fail("Session section is not updated");

                                }

                            }

                            else
                            {
                                Assert.Fail("Departments section is not updated");

                            }

                        }

                        else
                        {
                            Assert.Fail("Common section (Message Sending) is not updated");

                        }

                    }

                    else
                    {
                        Assert.Fail("Recipient User section is not updated");

                    }

                }

                else
                {
                    Assert.Fail("Receiver section is not updated");

                }
            }

            else
            {
                Assert.Fail("Common section (Display) is not updated");
            }




            //Common section (Display)
            driver.FindElement(By.XPath("(//a[text()='Common'])[1]")).Click();

            driver.FindElement(By.Id("btnEditMain")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtnumOfRecordReportPage")).Clear();
            driver.FindElement(By.Id("txtnumOfRecordReportPage")).SendKeys("20");


            //Receiver section
            driver.FindElement(By.LinkText("Receiver")).Click();


            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();//Detail Receiver/User Display
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[1]/div/ul/li[text()='No']")).Click();

      /*      // driver will scrol down the screen untill the required element is in view... 

            IWebElement element_update = driver.FindElement(By.XPath("//b[text()='Enable Receiver Attributes']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("var $myLabel = $(arguments[0]), $myParent = $myLabel.parents('.common_scrollbar'); $myParent.mCustomScrollbar('scrollTo', 'label[for=' + $myLabel.siblings('input').attr('id') + ']');", element_update);
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//b[text()='Enable Receiver Attributes']")).Click();

            driver.FindElement(By.XPath("//li[text()='Add New']")).Click();

            driver.FindElement(By.Id("attrib_")).SendKeys("B1");

            */

            // Recipient User


            driver.FindElement(By.LinkText("Recipient User")).Click();

            driver.FindElement(By.Id("receipentDeviceName")).Clear();

            driver.FindElement(By.Id("receipentDeviceName")).SendKeys("Recipient_user_name_updated");



            //Common section

            driver.FindElement(By.XPath("(//a[text()='Common'])[2]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[14]")).Click();//Enable time-stamp on all messages
            driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[1]/div/ul/li[text()='No']")).Click();

            //Department section

            driver.FindElement(By.LinkText("Departments")).Click();

          //  driver.FindElement(By.XPath("//b[text()='Maximum Receivers Allowed in a Department']")).Click();//Maximum Receivers Allowed in a Department

            driver.FindElement(By.Id("txtMaxRecieverDPT")).Clear();
            driver.FindElement(By.Id("txtMaxRecieverDPT")).SendKeys("200");//Maximum Receivers Allowed in a Department

            driver.FindElement(By.XPath("(//a[@class='selector'])[24]")).Click();//Count Recipient Groups as Members
            driver.FindElement(By.XPath("//div[@id='departmentsEdit']/fieldset[2]/div/ul/li[text()='Yes']")).Click();

            //Session section
            driver.FindElement(By.LinkText("Session")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[25]")).Click();//Temporary session
            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset/div/ul/li[text()='Yes']")).Click();


            driver.FindElement(By.Id("btnSave")).Click();

            driver.FindElement(By.Id("btnOk")).Click();
            Thread.Sleep(2000);

            //Common section (Display)

            driver.FindElement(By.XPath("(//a[text()='Common'])[1]")).Click();

            if (driver.FindElement(By.Id("lblnumOfRecordReportPage")).Text.Equals("20"))
            {
                driver.FindElement(By.LinkText("Receiver")).Click();

                if (driver.FindElement(By.Id("lbldetailDisplay")).Text.Equals("No"))
                {
                    driver.FindElement(By.LinkText("Recipient User")).Click();

                    if (driver.FindElement(By.Id("lblRespTemp")).Text.Equals("Recipient_user_name_updated"))
                    {

                        //Common section (Message Sending)
                        driver.FindElement(By.XPath("(//a[text()='Common'])[2]")).Click();

                        if (driver.FindElement(By.Id("lblselenableTimeStamp")).Text.Equals("No"))
                        {
                            driver.FindElement(By.LinkText("Departments")).Click();

                            if (driver.FindElement(By.Id("lblMaxRecieverDPT")).Text.Equals("200"))
                            {
                                driver.FindElement(By.LinkText("Session")).Click();

                                if (driver.FindElement(By.Id("lbltemparerySession")).Text.Equals("Yes"))
                                {
                                    Console.WriteLine("Global Settings Passed...");
                                }
                                else
                                {
                                    Assert.Fail("Session section is not updated");

                                }

                            }

                            else
                            {
                                Assert.Fail("Departments section is not updated");

                            }

                        }

                        else
                        {
                            Assert.Fail("Common section (Message Sending) is not updated");

                        }

                    }

                    else
                    {
                        Assert.Fail("Recipient User section is not updated");

                    }

                }

                else
                {
                    Assert.Fail("Receiver section is not updated");

                }
            }

            else
            {
                Assert.Fail("Common section (Display) is not updated");
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

        public string[] read_from_file(string file_name)
        {
            // Read each line of the file into a string array. Each element 
            // of the array is one line of the file. 

            string[] lines = System.IO.File.ReadAllLines(@".\" + file_name + ".txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of " + file_name + ".txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\n" + line);
            }

            return lines;
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

