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
using System.Diagnostics;
using System.ComponentModel;

namespace HL_Breadth
{
    [TestFixture]
    public class d_Breadth_Send_Panels : HL_Base_Class
    {

    //    private IWebDriver driver;

        private StringBuilder verificationErrors;

        private string baseURL;

        private bool acceptNextAlert = true;

        public string login_name = "admin"; //used in login and session manager testcases

        public string login_pwd = "admin";

        public string welcome_username = "Welcome admin"; //used in login testcase to verify 'Welcome user' label after login

        public string browser = "Mozilla"; //used in session manager according to browser(firefox,chrome,IE)

        public string driver_type;

   //     string browser_type;

    //    string browser_name;

        string user_label;

        string trimmed_user_label;

        string create_directory_path = @".\Screenshots_Testcase_Results";
        string create_directory_path_with_time;
        string carrier_name = "smtp_carrier";



        [TestFixtureSetUp]

        public void SetupTest()
        {

            // driver = new ChromeDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch chrome browser


            // driver = new InternetExplorerDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch IE browser

            // driver = new SafariDriver();// launch safari browser

            // driver = new FirefoxDriver();// launch firefox browser

            // System.Diagnostics.Debugger.Launch();// launch debugger

             string[] lines_local = read_from_file("login_credentials"); // return all the data in the form of array

             browser_name = get_browser();// get browser name ( firefox , safari , chrome , internetexplorer )
             Console.WriteLine("Browser Name got from xml file:" + " " + browser_name);

             switch (browser_name)
             {
                 case "firefox":
                     var profile = new FirefoxProfile();
                    profile.SetPreference("dom.forms.number",false);
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
            WaitForElementToExist("entityTitle", driver);

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
        public void a_Services_Settings_Panel()
        {

            check_driver_type(driver_type, "administration", "Services", "Sys Admin");

            Assert.AreEqual("Services", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            //hover_func("administration", "Services");

            driver.FindElement(By.XPath("(//a[text()='Stop All'])[1]")).Click();
            Thread.Sleep(2000);

            // for messenger services panel 

            if (IsElementPresent(By.XPath("(//a[@title='Play'])[1]"))) //messenger service start button
            {
                driver.FindElement(By.XPath("(//a[@title='Play'])[1]")).Click();
                Thread.Sleep(2000);

                if (IsElementPresent(By.XPath("(//a[@title='Restart'])[1]"))) //messenger service restart button
                {
                    driver.FindElement(By.XPath("(//a[@title='Restart'])[1]")).Click();
                    Thread.Sleep(2000);

                    if (IsElementPresent(By.XPath("(//a[@title='Stop'])[1]"))) //messenger service stop button
                    {
                        driver.FindElement(By.XPath("(//a[@title='Stop'])[1]")).Click();
                        Thread.Sleep(2000);

                        if (IsElementPresent(By.XPath("(//a[@title='Play'])[1]"))) //messenger service start button
                        {
                            driver.FindElement(By.XPath("(//a[@title='Play'])[1]")).Click();
                            Thread.Sleep(2000);

                            takescreenshot("Services_Passed");
                            Console.WriteLine("Messenger Services Passed");
                        }

                        else
                        {
                            takescreenshot("Services_Failed");
                            Assert.Fail("Messenger Services Start service button not found");
                        }
                    }

                    else
                    {
                        takescreenshot("Services_Failed");
                        Assert.Fail("Messenger Services Stop service button not found");
                    }
                }

                else
                {
                    takescreenshot("Services_Failed");
                    Assert.Fail("Messenger Services Restart service button not found");
                }
            }

            else
            {
                takescreenshot("Services_Failed");
                Assert.Fail("Messenger Services Start service button not found");
            }


            //table[@id='tblgateway']/tbody/tr[1]/td[2]/a[@class='action service_action_play']


            // for gateway services 

            Thread.Sleep(2000);

            /*
            // for system services panel 


            if (IsElementPresent(By.XPath("//table[@id='tblsystem']/tbody/tr[1]/td[2]/a[@class='action service_action_stop']"))) //messenger service stop button
            {
                driver.FindElement(By.XPath("//table[@id='tblsystem']/tbody/tr[1]/td[2]/a[@class='action service_action_stop']")).Click();
                Thread.Sleep(2000);

                if (IsElementPresent(By.XPath("//table[@id='tblsystem']/tbody/tr[1]/td[2]/a[@class='action service_action_play']"))) //messenger service start button
                {

                    driver.FindElement(By.XPath("//table[@id='tblsystem']/tbody/tr[1]/td[2]/a[@class='action service_action_play']")).Click();
                    Thread.Sleep(2000);


                    if (IsElementPresent(By.XPath("//table[@id='tblsystem']/tbody/tr[1]/td[3]/a[@class='service_action_refresh']"))) //messenger service restart button
                    {

                        driver.FindElement(By.XPath("//table[@id='tblsystem']/tbody/tr[1]/td[3]/a[@class='service_action_refresh']")).Click();
                        Thread.Sleep(2000);

                        takescreenshot("System_Services_Passed");

                        Console.WriteLine("System Services Passed");
                    }
                    else
                    {
                        takescreenshot("System_Services_Failed");
                        Assert.Fail("System Services Restart service button not found");
                    }

                }
                else
                {
                    takescreenshot("System_Services_Failed");
                    Assert.Fail("System Services Start service button not found");
                }

            }
            else
            {
                takescreenshot("System_Services_Failed");
                Assert.Fail("System Services Stop service button not found");
            }
             */


            Thread.Sleep(2000);

        }




        [Test]
        public void b_Primary_Send_Panel()
        {

          //  string receiver_name = "receiver_smtp";
            string primary_message = "Test Automation Message";
            string response_action_name = "Test Response Action";

            ArrayList array_list = new ArrayList();


            /////////////////////////// Adding receiver ///////////////////////////////


            string receiver_name = "receiver_smtp_send";
            
            string receiver_description = "Receiver Description";
            
            string receiver_pin = "testm703@gmail.com";
            
            string receiver_emailaddress = "email@address.com";

            string department_name = "Default";


            //check_driver_type(driver_type, "recipients", "Receivers", "Recipients");

            driver.FindElement(By.Id("recipients")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("Receivers")).Click();
            Thread.Sleep(2000);

            Assert.AreEqual("Receivers", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

            driver.FindElement(By.XPath("//a[text()='Add Reciever']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(receiver_name);

            driver.FindElement(By.Id("txtADesc")).Clear();
            driver.FindElement(By.Id("txtADesc")).SendKeys(receiver_description);

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'" + department_name + "')])")).Click();// selecting department
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'" + carrier_name + "')])")).Click();// selecting carrier

            driver.FindElement(By.Id("txtPrimaryPin")).Clear();
            driver.FindElement(By.Id("txtPrimaryPin")).SendKeys(receiver_pin);

            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);

            takescreenshot("Receiver");

            ///////////////////////////////////////////////////////////////////////////





            check_driver_type(driver_type, "send", "Primary Send", "Send");

            Assert.AreEqual("Primary Send", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text);

            driver.FindElement(By.Id("tt_interactive_filters")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//ul[@id='recipientLetterTool']/li[text()='R']")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("Count:" + driver.FindElements(By.XPath("//ul[@class='group_recipient_list']/li/span[@class='reciever g_r_l_name']")).Count + "*");

            for (int i = 0; i < driver.FindElements(By.XPath("//ul[@class='group_recipient_list']/li/span[@class='reciever g_r_l_name']")).Count; i++)
            {
                int j = i + 1;
                array_list.Add(driver.FindElement(By.XPath("(//ul[@class='group_recipient_list']/li/span[@class='reciever g_r_l_name'])["+ j +"]")).Text);
                
                Console.WriteLine("array_list_text" + array_list[i]);

                Assert.AreEqual(true , driver.FindElement(By.XPath("(//ul[@class='group_recipient_list']/li/span[@class='reciever g_r_l_name'])[" + j + "]")).Text.StartsWith("r"));

            }


            driver.FindElement(By.XPath("//b[@title='All']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//*[contains(text(),'" + receiver_name + "')]")).Click();

            driver.FindElement(By.Id("moveItemRight")).Click();
            //  driver.FindElement(By.XPath("//ul[@id='ulMembers']/li[2]/span")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).Clear();

            driver.FindElement(By.Id("txtAreaMessage")).SendKeys(primary_message);

            driver.FindElement(By.Id("priorityCheck")).Click();

            driver.FindElement(By.Id("incTimeStampCheck")).Click();

            driver.FindElement(By.Id("incSenderNameCheck")).Click();

            driver.FindElement(By.Id("incMsgIdCheck")).Click();

            driver.FindElement(By.Id("incEmail")).Click();

            driver.FindElement(By.XPath("//div[@id='testing']/b")).Click();//opening Advanced Messaging Parameters

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();// opening severity dropdown
            WaitForChrome(5000,browser_name);

            driver.FindElement(By.XPath("//li[text()='Important']")).Click();// selecting severity as Important

            driver.FindElement(By.XPath("//b[text()='Expire After']")).Click();// checking 'Expire After' checkbox

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//li[text()='02']")).Click();// selecting '02'

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[2]/div[3]/div/ul/li[3]")).Click();// selecting '02'

            driver.FindElement(By.XPath("//div[@id='response2wayPanel']/div[1]/b")).Click();// opening 2way Responses section
            WaitForChrome(5000,browser_name);

            driver.FindElement(By.Id("txtRespName")).Clear();

            driver.FindElement(By.Id("txtRespName")).SendKeys(response_action_name);// typing Response Action name

            driver.FindElement(By.Id("btnAddAction")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath(".//*[@id='sideBars']/div[3]/div/div[1]/b")).Click();  // opening Attachment section

            Thread.Sleep(4500);

            string[] imageurl = read_from_file("image_url");
            //uploading attachment
            IWebElement fileInput = driver.FindElement(By.XPath("//input[@type='file']"));
            fileInput.SendKeys(imageurl[0]);
            Thread.Sleep(4500);



            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            if (driver.FindElement(By.XPath("//div[@class='popup_message']")).Displayed)
            {
                if (driver.FindElement(By.Id("statusMessage")).Text.Contains("Created a total of 1 separate message(s)"))
                {

                    takescreenshot("Primary_Send_Passed");
                    driver.FindElement(By.Id("btnToMessage")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//label[@class='response_wait_notify_sender']")).Click();//span[contains(text(),'Send message to HNP Emergency User')]

                    driver.FindElement(By.Id("txtAreaMessage")).Clear();

                    driver.FindElement(By.Id("txtAreaMessage")).SendKeys(primary_message);

                    driver.FindElement(By.Id("btnSend")).Click();
                    Thread.Sleep(2000);

                    
                    if (driver.FindElement(By.XPath("//div[@class='popup_message']")).Displayed)
                    {
                        if (driver.FindElement(By.Id("statusMessage")).Text.Contains("Created a total of 1 separate message(s)"))
                        {

                            driver.FindElement(By.Id("btnToMessage")).Click();
                            Thread.Sleep(2000);

                        }
                    }
                    else
                    {
                        takescreenshot("Primary_Send_Failed");
                        Assert.Fail(" Primary Send Panel Faled ... ");
                    }
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^  Primary Send Panel Passed ... ^^^^^^^^^^^^^^^^^^^^^");

                }
                else
                {
                    takescreenshot("Primary_Send_Failed");
                    Assert.Fail(" Primary Send Panel Faled ... ");
                }
            }

            else
            {
                takescreenshot("Primary_Send_Failed");
                Assert.Fail(" Primary Send Panel Failed ... ");
            }

        }



        [Test]
        public void c_Quick_Send_Panel()
        {
            string pin_number = "testm703@gmail.com";
            string carrier_name = "smtp_carrier";
            string quick_message = "test message";

            check_driver_type(driver_type, "send", "Quick Send", "Send");

            Assert.AreEqual("Quick Send", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text);

            driver.FindElement(By.XPath("//div[@class='data_row_col1']/input")).Clear();

            driver.FindElement(By.XPath("//div[@class='data_row_col1']/input")).SendKeys(pin_number);

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'" + carrier_name + "')])")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).Clear();

            driver.FindElement(By.Id("txtAreaMessage")).SendKeys(quick_message);

            driver.FindElement(By.Id("priorityCheck")).Click();

            driver.FindElement(By.Id("incTimeStampCheck")).Click();

            driver.FindElement(By.Id("incSenderNameCheck")).Click();

            driver.FindElement(By.Id("incMsgIdCheck")).Click();

            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            takescreenshot("Quick_Send");


            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); //return to message button
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnReset")).Click();
            Thread.Sleep(2000);

            Assert.AreEqual(true, driver.FindElement(By.Id("txtAreaMessage")).Text.Equals("")); // Visible Works
            
            /*

            if (driver.FindElement(By.XPath("//div[@class='popup_message']")).Displayed)
            {
                if (driver.FindElement(By.Id("statusMessage")).Text.Contains("Created a total of 1 separate message(s)"))
                {

                    takescreenshot("Quick_Send_Passed");
                    driver.FindElement(By.Id("btnToMessage")).Click();
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^  Quick Send Panel Passed ... ^^^^^^^^^^^^^^^^^^^^^");

                }
                else
                {
                    takescreenshot("Quick_Send_Failed");
                    Assert.Fail(" Quick Send Panel Failed ...Mess ");
                }
            }

            else
            {
                takescreenshot("Quick_Send_Failed");
                Assert.Fail(" Quick Send Panel Failed ... ");
            }
            
             */


        }



        [Test]
        public void d_Escalation_Send_Panel()
        {
            string receiver_name = "receiver_smtp";
            string msg = "hello escalation send panel";

            check_driver_type(driver_type, "send", "Escalation Send", "Send");

            Assert.AreEqual("Escalation Send", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text); // verifying page title

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click(); //cycles drop down
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//div[@class='recipient_select_top_row']/div[2]/div/ul/li[text()='2']")).Click(); //selecting number of cycles 
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//*[contains(text(),'" + receiver_name + "')]")).Click(); //select receiver to send message

            driver.FindElement(By.Id("moveItemRight")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).Clear();

            driver.FindElement(By.Id("txtAreaMessage")).SendKeys(msg);

            driver.FindElement(By.Id("priorityCheck")).Click(); //priority checkbox

            driver.FindElement(By.Id("incTimeStampCheck")).Click(); //timestamp checkbox

            driver.FindElement(By.Id("incSenderNameCheck")).Click(); //sender name checkbox

            driver.FindElement(By.Id("incMsgIdCheck")).Click(); //msg id checkbox


            driver.FindElement(By.XPath("//div[@id='testing']/b")).Click();//opening Advanced Messaging Parameters

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();// opening severity dropdown
            WaitForChrome(5000, browser_name);

            driver.FindElement(By.XPath("//li[text()='Important']")).Click();// selecting severity as Important

            driver.FindElement(By.XPath("//b[text()='Expire After']")).Click();// checking 'Expire After' checkbox

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//li[text()='02']")).Click();// selecting '02'

            driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[2]/div[3]/div/ul/li[3]")).Click();// selecting '02'

            Console.WriteLine("recipients:"+driver.FindElement(By.Id("selRecipientArea")).Text);
            

            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(3000);

            takescreenshot("Escalation_Send_Panel");

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); //return to message button
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnReset")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("recipients:" + driver.FindElement(By.Id("selRecipientArea")).Text);

            Assert.AreEqual(true, driver.FindElement(By.Id("selRecipientArea")).Text.Equals("")); // Visible Works
            Assert.AreEqual(true, driver.FindElement(By.Id("txtAreaMessage")).Text.Equals("")); // Visible Works
            
                        
           


        }



        [Test]
        public void e_Fax_Send_Panel()
        {
            string receiver_name = "receiver_fax";
            string msg = "hello fax receiver";

            string receiver_pin = "123456789";
            string receiver_description = "Receiver Description";
            string receiver_emailaddress = "email@address.com";

            string department_name = "Default";

            check_driver_type(driver_type, "recipients", "Receivers", "Recipients");

            Assert.AreEqual("Receivers", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.XPath("//a[text()='Add Reciever']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(receiver_name);

            driver.FindElement(By.Id("txtADesc")).Clear();
            driver.FindElement(By.Id("txtADesc")).SendKeys(receiver_description);

            driver.FindElement(By.Id("txtEmailAdrs")).Clear();
            driver.FindElement(By.Id("txtEmailAdrs")).SendKeys(receiver_emailaddress);
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//label/b[text()='Email CC']")).Click();
            driver.FindElement(By.XPath("//label/b[text()='Email Failover']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'Default')])")).Click();// selecting department

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'Fax')])")).Click();// selecting receiver type

            driver.FindElement(By.Id("txtPrimaryPin")).Clear();
            driver.FindElement(By.Id("txtPrimaryPin")).SendKeys(receiver_pin);

            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);


            check_driver_type(driver_type, "send", "Fax Send", "Send");

            Assert.AreEqual("Fax Send", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text);

            driver.FindElement(By.XPath("//*[contains(text(),'" + receiver_name + "')]")).Click(); //select receiver to send message

            driver.FindElement(By.Id("moveItemRight")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).Clear();

            driver.FindElement(By.Id("txtAreaMessage")).SendKeys(msg);

            driver.FindElement(By.Id("incMsgIdCheck")).Click(); // include message id checkbox


            driver.FindElement(By.Id("btnSend")).Click();
            WaitForChrome(5000,browser_name);
            Thread.Sleep(2000);

            takescreenshot("Fax_Send_Panel");

            //driver.FindElement(By.Id("btnToMessage")).Displayed

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); //return to message button
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnReset")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("recipients:" + driver.FindElement(By.Id("selRecipientArea")).Text);

            Assert.AreEqual(true, driver.FindElement(By.Id("selRecipientArea")).Text.Equals("")); // Visible Works
            Assert.AreEqual(true, driver.FindElement(By.Id("txtAreaMessage")).Text.Equals("")); // Visible Works
            
            Console.WriteLine("Fax_Send_Panel Passed...");

        }



        [Test]
        public void f_Voice_Send_Panel()
        {
            string receiver_name_voice = "receiver_voice";
            string msg = "hello voice receiver";

            string receiver_pin = "123456789";
            string receiver_description = "Receiver Description";
            string receiver_emailaddress = "email@address.com";

            string department_name_default = "Default";

            check_driver_type(driver_type, "recipients", "Receivers", "Recipients");

            Assert.AreEqual("Receivers", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.XPath(".//div[@class='filter_panel']/a[text()='Add Reciever']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(receiver_name_voice);

            driver.FindElement(By.Id("txtADesc")).Clear();
            driver.FindElement(By.Id("txtADesc")).SendKeys(receiver_description);

            driver.FindElement(By.Id("txtEmailAdrs")).Clear();
            driver.FindElement(By.Id("txtEmailAdrs")).SendKeys(receiver_emailaddress);
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//label/b[text()='Email CC']")).Click();
            driver.FindElement(By.XPath("//label/b[text()='Email Failover']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'" + department_name_default + "')])")).Click();// selecting department

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),':Voice')])")).Click();// selecting carrier

            driver.FindElement(By.Id("txtPrimaryPin")).Clear();
            driver.FindElement(By.Id("txtPrimaryPin")).SendKeys(receiver_pin);

            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);

            check_driver_type(driver_type, "send", "Voice Send", "Send");

            Assert.AreEqual("Voice Send", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text); //verifying page title

            driver.FindElement(By.XPath("//*[contains(text(),'" + receiver_name_voice + "')]")).Click(); //select receiver to send message

            driver.FindElement(By.Id("moveItemRight")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).Clear();

            driver.FindElement(By.Id("txtAreaMessage")).SendKeys(msg);

            driver.FindElement(By.XPath("//b[text()='Include message ID']")).Click(); // include message id checkbox

            driver.FindElement(By.XPath("//b[text()='Send sound file first']")).Click(); // send sound file first checkbox

            driver.FindElement(By.XPath("//div[@class='accor_header']/b")).Click();
            Thread.Sleep(2000);

            //uploading attachment
            IWebElement fileInput = driver.FindElement(By.XPath("//input[@type='file']"));
            fileInput.SendKeys(@"C:\Users\fali\Downloads\beep-01a.wav");
            Thread.Sleep(4500);

            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            takescreenshot("Voice_Send_Panel");

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); // return to message button
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnReset")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("recipients:" + driver.FindElement(By.Id("selRecipientArea")).Text);

            Assert.AreEqual(true, driver.FindElement(By.Id("selRecipientArea")).Text.Equals("")); // Visible Works
            Assert.AreEqual(true, driver.FindElement(By.Id("txtAreaMessage")).Text.Equals("")); // Visible Works
            

            Console.WriteLine("Voice_Send_Panel Passed...");

        }

        [Test]
        public void g_Quota_Send_Panel()
        {
            check_driver_type(driver_type, "send", "Quota Send", "Send");


            Assert.AreEqual("Quota Send", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text); //verifying page title

            driver.FindElement(By.XPath("//span[text()='receiver_smtp']")).Click();
            driver.FindElement(By.Id("moveItemRight")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//span[text()='receiver_smtp_send']")).Click();
            driver.FindElement(By.Id("moveItemRight")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("txtQuota")).Clear();
            driver.FindElement(By.Id("txtQuota")).SendKeys("1");
            Thread.Sleep(1000);

            driver.FindElement(By.Id("txtRespWait")).Clear();
            driver.FindElement(By.Id("txtRespWait")).SendKeys("1");

            driver.FindElement(By.XPath("//b[text()='Notify sender']")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).Clear();
            driver.FindElement(By.Id("txtAreaMessage")).SendKeys("Test quota");

            driver.FindElement(By.Id("priorityCheck")).Click(); //priority checkbox

            driver.FindElement(By.Id("incTimeStampCheck")).Click(); //timestamp checkbox

            driver.FindElement(By.Id("incSenderNameCheck")).Click(); //sender name checkbox

            driver.FindElement(By.Id("incMsgIdCheck")).Click(); //msg id checkbox


            driver.FindElement(By.XPath("//div[@id='testing']/b")).Click();//opening Advanced Messaging Parameters

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();// opening severity dropdown
            WaitForChrome(5000, browser_name);

            driver.FindElement(By.XPath("//li[text()='Important']")).Click();// selecting severity as Important

            driver.FindElement(By.XPath("//b[text()='Expire After']")).Click();// checking 'Expire After' checkbox

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//li[text()='02']")).Click();// selecting '02'

            driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[2]/div[3]/div/ul/li[3]")).Click();// selecting '02'

            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            takescreenshot("Quota_Send_Panel");

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); // return to message button
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnReset")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("recipients:" + driver.FindElement(By.Id("selRecipientArea")).Text);

            Assert.AreEqual(true, driver.FindElement(By.Id("selRecipientArea")).Text.Equals("")); // Visible Works
            Assert.AreEqual(true, driver.FindElement(By.Id("txtAreaMessage")).Text.Equals("")); // Visible Works
            

            Console.WriteLine("Quota_Send_Panel Passed...");
        }


        [Test]
        public void h_Attribute_Send_Panel()
        {

            check_driver_type(driver_type, "send", "Attribute Send", "Send");

            Assert.AreEqual("Attribute Send", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text); //verifying page title

            driver.FindElement(By.XPath("//b[text()='A1']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//span[text()='receiver_smtp']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("moveItemRight")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).SendKeys("Test attribute");

            driver.FindElement(By.Id("priorityCheck")).Click(); //priority checkbox

            driver.FindElement(By.Id("incTimeStampCheck")).Click(); //timestamp checkbox

            driver.FindElement(By.Id("incSenderNameCheck")).Click(); //sender name checkbox

            driver.FindElement(By.Id("incMsgIdCheck")).Click(); //msg id checkbox


            driver.FindElement(By.XPath("//div[@id='testing']/b")).Click();//opening Advanced Messaging Parameters

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();// opening severity dropdown

            driver.FindElement(By.XPath("//li[text()='Important']")).Click();// selecting severity as Important

            driver.FindElement(By.XPath("//b[text()='Expire After']")).Click();// checking 'Expire After' checkbox

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//li[text()='02']")).Click();// selecting '02'

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[2]/div[3]/div/ul/li[3]")).Click();// selecting '02'


            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            takescreenshot("Attribute_Send_Panel");

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); // return to message button
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnReset")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("recipients:" + driver.FindElement(By.Id("selRecipientArea")).Text);

            Assert.AreEqual(true, driver.FindElement(By.Id("selRecipientArea")).Text.Equals("")); // Visible Works
            Assert.AreEqual(true, driver.FindElement(By.Id("txtAreaMessage")).Text.Equals("")); // Visible Works
            

            Console.WriteLine("Attribute_Send_Panel Passed...");
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

