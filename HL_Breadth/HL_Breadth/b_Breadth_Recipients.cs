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
    public class b_Breadth_Recipients : HL_Base_Class
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

        string create_directory_path_with_time;

        string create_directory_path = @".\Screenshots_Recipients_Testcase";

        // DIRECTORY VARIABLES

      //  string create_directory_path_directory = @"C:\HipLink\new_directory";

      //  string create_directory_path_directory_2 = @"C:\HipLink\new_directory_2";

        string new_dir = "new_directory";

        string new_dir_2 = "new_new_directory";

        string new_dir_2_edited = "edited_new_directory_2";

        // MESSENGER VARIABLES

        string messenger_name = "smtp_messenger";

        string messenger_name_2 = "smtp_new_messenger";

        string messenger_name_2_edited = "edited_smtp_messenger_2";


        // CARRIER VARIABLES

        string carrier_name = "smtp_carrier";

        string carrier_name_2 = "smtp_new_carrier";

        string carrier_name_2_edited = "edited_smtp_carrier_2";



        [TestFixtureSetUp]

        public void SetupTest()
        {

            // driver = new ChromeDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch chrome browser


            // driver = new InternetExplorerDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch IE browser

            // driver = new SafariDriver();// launch safari browser

            // driver = new FirefoxDriver();// launch firefox browser

              System.Diagnostics.Debugger.Launch();// launch debugger

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

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(25000));//wait for request 

            driver_type = driver.GetType().ToString();// get driver type ( firefox , safari , chrome , internetexplorer )

            Console.WriteLine("Driver Type:" + " " + driver_type);

            baseURL = "http://localhost:8000/";
        // baseURL = "http://192.168.5.209:8001/";

            driver.Navigate().GoToUrl(baseURL + "/HipLink5UI-Work/index.html#login");

            driver.Manage().Window.Maximize();//maximize browser

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
        public void a_Add_Edit_Delete_Directory_Settings_Panel()
        {
            string[] lines_local = read_from_file("directory_path"); // return all the data in the form of array

            string create_directory_path_directory = lines_local[0];

            string dir_path = lines_local[0];

            string create_directory_path_directory_2 = lines_local[1];

            if (!Directory.Exists(create_directory_path_directory))
            {
                Directory.CreateDirectory(create_directory_path_directory);
            }

            if (!Directory.Exists(create_directory_path_directory_2))
            {
                Directory.CreateDirectory(create_directory_path_directory_2);
            }


            check_driver_type(driver_type, "administration", "Directories & Queues", "Sys Admin");

            Assert.AreEqual("Directories & Queues", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

          // ADDED FIRST RECORD

            driver.FindElement(By.Id("btnAddQue")).Click();

            driver.FindElement(By.Id("txtqueName")).Clear();

            driver.FindElement(By.Id("txtqueName")).SendKeys(new_dir); // directory name

            driver.FindElement(By.Id("txtquePath")).Clear();

            driver.FindElement(By.Id("txtquePath")).SendKeys(create_directory_path_directory); // directory path

            driver.FindElement(By.LinkText("OK")).Click();
            Thread.Sleep(2000);

          
            // VERIFYING FIRST RECORD

            if (!(driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(new_dir) &&

                driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(create_directory_path_directory)))
            
            {
                takescreenshot("Directory_Failed");
                Assert.Fail("Add Directory Failed...");
            }

            else
            
            {
                // ADDED SECOND RECORD

                driver.FindElement(By.Id("btnAddQue")).Click();

                driver.FindElement(By.Id("txtqueName")).Clear();

                driver.FindElement(By.Id("txtqueName")).SendKeys(new_dir_2); // directory name

                driver.FindElement(By.Id("txtquePath")).Clear();

                driver.FindElement(By.Id("txtquePath")).SendKeys(create_directory_path_directory_2); // directory path

                driver.FindElement(By.LinkText("OK")).Click();
                Thread.Sleep(2000);

                // VERIFYING SECOND RECORD

                if (!(driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(new_dir_2) &&

               driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(create_directory_path_directory_2)))
                
                {

                    takescreenshot("Second_Directory_Failed");

                    Assert.Fail("Second Directory Failed...");

                }

                else
                
                {
                    // EDITING SECOND RECORD

                    driver.FindElement(By.XPath("(//a[@class='row_action_edit'])[3]")).Click();

                    driver.FindElement(By.Id("txtqueName")).Clear();

                    driver.FindElement(By.Id("txtqueName")).SendKeys(new_dir_2_edited);

                    driver.FindElement(By.LinkText("OK")).Click();
                    Thread.Sleep(2000);

                    // VERIFYING EDITED SECOND RECORD

                    if(!(driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(new_dir_2_edited)))
                    {
                        takescreenshot("Second_Directory_Edited_Failed");

                        Assert.Fail("Second Directory Edited Failed...");
                    }
                    else
                    {
                        // DELETING SECOND RECORD

                        driver.FindElement(By.XPath("(//a[@class='row_action_delete'])[3]")).Click();
                        Thread.Sleep(2000);

                        // GETTING ROW COUNT AND DISPLAY  IT
                        IWebElement element = driver.FindElement(By.XPath("//div[@id='lightGridDiv']/table/tbody"));
                        IList<IWebElement> row_count = element.FindElements(By.XPath("//tr"));

                        int i = row_count.Count;
                        Console.WriteLine("*"+i+"*");

                        // VERIFYING DELETED SECOND RECORD

                        if (!(driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(new_dir_2_edited)))
                        
                        {

                            takescreenshot("Directory_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Directory Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }

                        else

                        {

                            takescreenshot("Second_Directory_Deleted_Failed");

                            Assert.Fail("Second Directory Deleted Failed...");

                            
                        }

                        
                    }


                }

            
            }


        }



        [Test]
        public void b_Add_Edit_Delete_Messenger()
        {



            string messenger_desc = "SMTP Messenger Description";

            string messenger_desc_2 = "SMTP Messenger Description_2";

            string messenger_desc_2_edited = "SMTP Messenger Description_2 Edited";

            check_driver_type(driver_type, "administration", "Messengers", "Sys Admin");

            Assert.AreEqual("Messengers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

         
            //-------- Select Messenger Protocol Type ---------

            IWebElement element = driver.FindElement(By.XPath("//div[@class='messenger_protocol_category_list_wrapper']/div"));
            IList<IWebElement> messenger_protocol_category_List = element.FindElements(By.XPath("//ul[@id='columnOne']/div/div/li"));

            Console.WriteLine("number of protocol category" + " " + messenger_protocol_category_List.Count);

            int DpListCount = messenger_protocol_category_List.Count;
            for (int i = 0; i < DpListCount; i++)
            {
                if (messenger_protocol_category_List[i].Text == "Email")
                {
                    Console.WriteLine("index where protocol category matched" + " " + i);
                    Console.WriteLine("Category name:" + " " + messenger_protocol_category_List[i].Text);
                    messenger_protocol_category_List[i].Click();
                }
            }

            IWebElement element1 = driver.FindElement(By.XPath("//div[@class='messenger_protocol_category_list_wrapper']/div"));
            IList<IWebElement> messenger_protocol_category_type = element.FindElements(By.XPath("//ul[@id='columnTwo']/li"));

            Console.WriteLine("number of protocol in category type EMAIL" + " " + messenger_protocol_category_type.Count);

            int DpListCount1 = messenger_protocol_category_type.Count;
            for (int i = 0; i < DpListCount1; i++)
            {
                if (messenger_protocol_category_type[i].Text == "SMTP")
                {
                    Console.WriteLine("index where protocol category type matched" + " " + i);
                    Console.WriteLine("Type name:" + " " + messenger_protocol_category_type[i].Text);
                    messenger_protocol_category_type[i].Click();
                }
            }


            //----------xxxxxxx----------


            driver.FindElement(By.Id("btnMsngr")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.Id("txtMessangerName")).Clear();
            driver.FindElement(By.Id("txtMessangerName")).SendKeys(messenger_name);


            //----------- Selecting Paging Queue ----------

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//li[text()='Default']")).Click();
          
            //----------- xxxxxxxxxxxxxxxxxxxxxx ----------


            driver.FindElement(By.Id("txtMessangerDescription")).Clear();
            driver.FindElement(By.Id("txtMessangerDescription")).SendKeys(messenger_desc);

            //----------- Selecting Paging Queue checking period ----------

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//li[text()='5']")).Click();

            //----------- xxxxxxxxxxxxxxxxxxxxxx ----------


            driver.FindElement(By.Id("btnSaveMsngr")).Click();
            Thread.Sleep(2000);
            takescreenshot("Messenger");


            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(messenger_name)))
            {
                takescreenshot("Messenger_Failed");
                Assert.Fail("Added messenger is failed ...");
            }

            else
            {
                // ADDING SECOND MESSENGER

                driver.FindElement(By.XPath("//li[text()='Email']")).Click();

                driver.FindElement(By.XPath("//li[text()='SMTP']")).Click();

                driver.FindElement(By.Id("btnMsngr")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.Id("txtMessangerName")).Clear();
                driver.FindElement(By.Id("txtMessangerName")).SendKeys(messenger_name_2);


                //----------- Selecting Paging Queue ----------

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

                Thread.Sleep(1000);

                driver.FindElement(By.XPath("//li[text()='Default']")).Click();

                //----------- xxxxxxxxxxxxxxxxxxxxxx ----------


                driver.FindElement(By.Id("txtMessangerDescription")).Clear();
                driver.FindElement(By.Id("txtMessangerDescription")).SendKeys(messenger_desc_2);

               
                driver.FindElement(By.Id("btnSaveMsngr")).Click();
                Thread.Sleep(2000);

                //VERIFYING SECOND ADDED MESSENGER

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(messenger_name_2)))
                
                {
                    takescreenshot("Messenger_Failed");
                    Assert.Fail("Second Added messenger is failed ...");
                }

                else
                
                {
                    //EDITING MESSENGER

                    driver.FindElement(By.XPath("(//img[@src='./images/bg_gd_icon_edit.png'])[2]")).Click();

                    driver.FindElement(By.Id("txtMessangerName")).Clear();
                    driver.FindElement(By.Id("txtMessangerName")).SendKeys(messenger_name_2_edited);

                    driver.FindElement(By.Id("txtMessangerDescription")).Clear();
                    driver.FindElement(By.Id("txtMessangerDescription")).SendKeys(messenger_desc_2_edited);

                    //----------- Selecting Paging Queue checking period ----------

                    driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//li[text()='4']")).Click();

                    //----------- xxxxxxxxxxxxxxxxxxxxxx ----------



                    driver.FindElement(By.Id("btnSaveMsngr")).Click();
                    Thread.Sleep(2000);


                    //VERIFYING SECOND EDITED MESSENGER


                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(messenger_name_2_edited)))
                    
                    {
                        takescreenshot("Edited_Messenger_Failed");
                        Assert.Fail("Edited messenger is failed ...");
                    }

                    else
                    
                    {
                        // DELETING MESSENGER

                        driver.FindElement(By.XPath("(//img[@src='./images/bg_gd_icon_delete.png'])[1]")).Click();

                        IAlert delete_alert = driver.SwitchTo().Alert();
                        delete_alert.Accept();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(messenger_name_2_edited)))
                        {
                            takescreenshot("Messenger_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Messenger Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }

                        else
                        {

                            takescreenshot("Second_Messenger_Deleted_Failed");

                            Assert.Fail("Second Messenger Deleted Failed...");
                        }


                    }

                }
               
            }

        }


        [Test]
        public void c_Add_Edit_Delete_Carrier()
        {

            string carrier_desc = "SMTP Carrier Description";
            string carrier_desc_2 = "SMTP Carrier Description 2";
            string carrier_desc_2_edited = "SMTP Carrier Description_2 Edited";
            string email_server = "smtp.gmail.com";
            string user_name = "hiplink@gmail.com";
            string user_pwd = "click+123";
            string email_suffix = "Email Suffix";
            string email_prefix = "Email Prefix";
            string email_subject = "Email Subject";


            check_driver_type(driver_type, "administration", "Carriers", "Sys Admin");

            Assert.AreEqual("Carriers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

           

            //-------- Select Messenger Protocol Type ---------

            IWebElement elementc = driver.FindElement(By.XPath("//div[@class='messenger_protocol_category_list_wrapper']/div"));
            IList<IWebElement> carrier_protocol_category_List = elementc.FindElements(By.XPath("//ul[@id='columnOne']/div/div/li"));

            Console.WriteLine("number of protocol category" + " " + carrier_protocol_category_List.Count);

            int DpListCountc = carrier_protocol_category_List.Count;
            for (int i = 0; i < DpListCountc; i++)
            {
                if (carrier_protocol_category_List[i].Text == "Email")
                {
                    Console.WriteLine("index where protocol category matched" + " " + i);
                    Console.WriteLine("Category name:" + " " + carrier_protocol_category_List[i].Text);
                    carrier_protocol_category_List[i].Click();
                }
            }


            IWebElement element1c = driver.FindElement(By.XPath("//div[@class='messenger_protocol_category_list_wrapper']/div"));
            IList<IWebElement> carrier_protocol_category_type = element1c.FindElements(By.XPath("//ul[@id='columnTwo']/li"));

            Console.WriteLine("number of protocol category typr" + " " + carrier_protocol_category_type.Count);

            int DpListCount1c = carrier_protocol_category_type.Count;
            for (int i = 0; i < DpListCount1c; i++)
            {
                if (carrier_protocol_category_type[i].Text == "SMTP")
                {
                    Console.WriteLine("index where protocol category type matched" + " " + i);
                    Console.WriteLine("Type name:" + " " + carrier_protocol_category_type[i].Text);
                    carrier_protocol_category_type[i].Click();
                }
            }
            
            //----------xxxxxxx----------


            driver.FindElement(By.Id("btnaddcarrier")).Click();
            Thread.Sleep(5000);


            driver.FindElement(By.Id("carrierName")).Clear();
            driver.FindElement(By.Id("carrierName")).SendKeys(carrier_name);
            Thread.Sleep(2000);
            //----------- Selecting Paging Queue ----------

           
            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
            Thread.Sleep(1000);
            string path1 = "//li[text()='";
            string path2 = "']";

            driver.FindElement(By.XPath(path1 + "Default" + path2)).Click();
            Thread.Sleep(2000);

            //----------- xxxxxxxxxxxxxxxxxxxxxx ----------

            driver.FindElement(By.Id("carrierDescription")).Clear();

            driver.FindElement(By.Id("carrierDescription")).SendKeys(carrier_desc);

            driver.FindElement(By.XPath("//span[text()='Check for automatic carrier updates']")).Click();

            driver.FindElement(By.XPath("//span[text()='Use global settings email server']")).Click();

            driver.FindElement(By.XPath("//span[text()='Use global settings email server']")).Click();

            driver.FindElement(By.Id("txtemailServer")).Clear();

            driver.FindElement(By.Id("txtemailServer")).SendKeys(email_server);
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='Require TLS']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("txtuserName")).Clear();

            driver.FindElement(By.Id("txtuserName")).SendKeys(user_name);

            driver.FindElement(By.Id("txtpassword")).Clear();

            driver.FindElement(By.Id("txtpassword")).SendKeys(user_pwd);

            driver.FindElement(By.Id("emailAddressSuffix")).Clear();

            driver.FindElement(By.Id("emailAddressSuffix")).SendKeys(email_suffix);

            driver.FindElement(By.Id("emailAddressPrefix")).Clear();

            driver.FindElement(By.Id("emailAddressPrefix")).SendKeys(email_prefix);

            driver.FindElement(By.Id("emailSubject")).Clear();

            driver.FindElement(By.Id("emailSubject")).SendKeys(email_subject);
            Thread.Sleep(1000);

            // Truncate Long Message check box
            /*
            driver.FindElement(By.XPath("//span[text()='Truncate Long Message']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txttruncatelongmessage")).Clear();
            
            driver.FindElement(By.Id("txttruncatelongmessage")).SendKeys("200");
             */

           
            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);

            takescreenshot("Carrier");

            Thread.Sleep(3000);
            Console.WriteLine("*" + driver.FindElement(By.Id("divGrid_idGridDataNode")).Text.Contains(carrier_name) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(carrier_desc) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(new_dir) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text + "*");


            if (!(driver.FindElement(By.Id("divGrid_idGridDataNode")).Text.Contains(carrier_name) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(carrier_desc)))
            {
                takescreenshot("Carrier_Failed");
                Assert.Fail("Added Carrier is failed ...");
            }

            else
            {
                // ADDING SECOND CARRIER

                driver.FindElement(By.XPath("//li[text()='Email']")).Click();

                driver.FindElement(By.XPath("//li[text()='SMTP']")).Click();

                driver.FindElement(By.Id("btnaddcarrier")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.Id("carrierName")).Clear();
                
                driver.FindElement(By.Id("carrierName")).SendKeys(carrier_name_2);
                Thread.Sleep(2000);

                //----------- Selecting Paging Queue ----------


                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
                Thread.Sleep(1000);
                string path11 = "//li[text()='";
                string path21 = "']";

                driver.FindElement(By.XPath(path11 + "Default" + path21)).Click();
                Thread.Sleep(2000);

                //----------- xxxxxxxxxxxxxxxxxxxxxx ----------

                driver.FindElement(By.Id("carrierDescription")).Clear();

                driver.FindElement(By.Id("carrierDescription")).SendKeys(carrier_desc_2);

                driver.FindElement(By.XPath("//span[text()='Use global settings email server']")).Click();

                driver.FindElement(By.XPath("//span[text()='Use global settings email server']")).Click();

                driver.FindElement(By.Id("txtemailServer")).Clear();

                driver.FindElement(By.Id("txtemailServer")).SendKeys(email_server);
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='Require TLS']")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.Id("txtuserName")).Clear();

                driver.FindElement(By.Id("txtuserName")).SendKeys(user_name);

                driver.FindElement(By.Id("txtpassword")).Clear();

                driver.FindElement(By.Id("txtpassword")).SendKeys(user_pwd);
                Thread.Sleep(1000);

                driver.FindElement(By.Id("btnsave")).Click();
                Thread.Sleep(5000);

                //VERIFYING SECOND ADDED CARRIER

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(carrier_name_2)))
                {
                    takescreenshot("Carrier_Failed");
                    Assert.Fail("Second Added carrier is failed ...");
                }

                else
                {
                    //EDITING CARRIER

                    driver.FindElement(By.XPath("(//a[@class='row_action_edit'])[2]")).Click();
                    //WaitForElementToExist("carrierName",driver);
                    Thread.Sleep(5000);
                    driver.FindElement(By.Id("carrierName")).Clear();
                    driver.FindElement(By.Id("carrierName")).SendKeys(carrier_name_2_edited);

                    //----------- Selecting Paging Queue ----------


                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
                    Thread.Sleep(1000);
                    
                    driver.FindElement(By.XPath(path11 + "Default" + path21)).Click();
                    Thread.Sleep(2000);

                    //----------- xxxxxxxxxxxxxxxxxxxxxx ----------

                    driver.FindElement(By.Id("carrierDescription")).Clear();
                    driver.FindElement(By.Id("carrierDescription")).SendKeys(carrier_desc_2_edited);

                    driver.FindElement(By.XPath("//span[text()='Use global settings email server']")).Click();

                    driver.FindElement(By.XPath("//span[text()='Use global settings email server']")).Click();

                    driver.FindElement(By.Id("txtemailServer")).Clear();

                    driver.FindElement(By.Id("txtemailServer")).SendKeys(email_server);
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//li[text()='Require TLS']")).Click();
                    Thread.Sleep(1000);

                    driver.FindElement(By.Id("txtuserName")).Clear();

                    driver.FindElement(By.Id("txtuserName")).SendKeys(user_name);

                    driver.FindElement(By.Id("txtpassword")).Clear();

                    driver.FindElement(By.Id("txtpassword")).SendKeys(user_pwd);
                    Thread.Sleep(1000);

                    driver.FindElement(By.Id("btnsave")).Click();
                    Thread.Sleep(2000);


                    //VERIFYING SECOND EDITED CARRIER


                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(carrier_name_2_edited)))
                    {
                        takescreenshot("Edited_Carrier_Failed");
                        Assert.Fail("Edited Carrier is failed ...");
                    }

                    else
                    {
                        // DELETING CARRIER

                        driver.FindElement(By.XPath("(//a[@class='row_action_delete'])[1]")).Click();

                        IAlert delete_alert = driver.SwitchTo().Alert();
                        delete_alert.Accept();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(carrier_name_2_edited)))
                        {
                            takescreenshot("Carrier_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Carrier Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }

                        else
                        {

                            takescreenshot("Second_Carrier_Deleted_Failed");

                            Assert.Fail("Second Carrier Deleted Failed...");
                        }


                    }

                }

            }

        }



        [Test]
        public void d_Add_Edit_Delete_Receiver()
        {


            string receiver_name = "receiver_smtp";
            string receiver_name_2 = "receiver_new_smtp";
            string receiver_name_2_edited = "edited_receiver_smtp_2";
            string receiver_name_3 = "a_test_receiver";
            string receiver_name_4 = "aa_test_receiver";

            string receiver_description = "Receiver Description";
            string receiver_description_2 = "Receiver Description 2";
            string receiver_description_edited = "Receiver Description Edited";

            string receiver_pin = "testm703@gmail.com";
            string receiver_pin_2 = "testm7032@gmail.com";
            
            string receiver_emailaddress = "email@address.com";

            string department_name = "Default";


            check_driver_type(driver_type, "recipients", "Receivers", "Recipients");

            Assert.AreEqual("Receivers", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

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

            driver.FindElement(By.XPath("(//li[contains(text(),'" + department_name + "')])")).Click();// selecting department
            Thread.Sleep(2000);

         //   driver.FindElement(By.Id("btnEditAttribute")).Click();
            Thread.Sleep(2000);

         //   driver.FindElement(By.XPath("//span[contains(text(),'A1')]")).Click();
            Thread.Sleep(2000);

         //   driver.FindElement(By.Id("btnAddAttribute")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'" + carrier_name + "')])")).Click();// selecting carrier

            driver.FindElement(By.Id("txtPrimaryPin")).Clear();
            driver.FindElement(By.Id("txtPrimaryPin")).SendKeys(receiver_pin);

            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);

            takescreenshot("Receiver");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(receiver_name)))
            {
                takescreenshot("Receiver_Failed");
                Assert.Fail("Add Receiver Failed...");
            }

            else
            
            {
                // ADDING SECOND RECEIVER

                driver.FindElement(By.XPath("//a[text()='Add Reciever']")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.Id("txtName")).Clear();
                driver.FindElement(By.Id("txtName")).SendKeys(receiver_name_2);

                driver.FindElement(By.Id("txtADesc")).Clear();
                driver.FindElement(By.Id("txtADesc")).SendKeys(receiver_description_2);

                driver.FindElement(By.Id("txtEmailAdrs")).Clear();
                driver.FindElement(By.Id("txtEmailAdrs")).SendKeys(receiver_emailaddress);
                Thread.Sleep(2000);

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


                //VERIFYING SECOND ADDED RECEIVER

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(receiver_name_2)))
                {
                    takescreenshot("Receiver_Failed");
                    Assert.Fail("Second Added Receiver is failed ...");
                }

                else
                {
                    //EDITING SECOND RECEIVER

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[1]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtName")).Clear();
                    driver.FindElement(By.Id("txtName")).SendKeys(receiver_name_2_edited);

                    driver.FindElement(By.Id("txtADesc")).Clear();
                    driver.FindElement(By.Id("txtADesc")).SendKeys(receiver_description_edited);

                    driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("(//li[contains(text(),'" + department_name + "')])")).Click();// selecting department
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("(//li[contains(text(),'" + carrier_name + "')])")).Click();// selecting carrier

                    driver.FindElement(By.Id("txtPrimaryPin")).Clear();
                    driver.FindElement(By.Id("txtPrimaryPin")).SendKeys(receiver_pin_2);

                    driver.FindElement(By.Id("btnsave")).Click();
                    Thread.Sleep(2000);


                    //VERIFYING SECOND EDITED RECEIVER


                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(receiver_name_2_edited)))
                    {
                        takescreenshot("Edited_Receiver_Failed");
                        Assert.Fail("Edited Receiver is failed ...");
                    }

                    else
                    {
                        // DELETING RECEIVER

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                    }

                }

            }

        }




        [Test]
        public void e_Add_Edit_Delete_Broadcast_Group()
        {



            string broadcast_group_name = "Broadcast_Group";
            string broadcast_group_name_2 = "Broadcast_new_Group_2";
            string broadcast_group_name_2_edited = "edited_Broadcast_Group_2";
            
            string broadcast_group_description = "Broadcast Group Description";
            string broadcast_group_description_2 = "Broadcast Group Description 2";
            string broadcast_group_description_2_edited = "Broadcast Group Description 2 Edited";

            string department_name = "Default";
            string owner_name = "admin";
            string receiver_name = "receiver_smtp";

            check_driver_type(driver_type, "recipients", "Broadcast", "Recipients");

            Assert.AreEqual("Broadcast Groups", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

            driver.FindElement(By.LinkText("Add Group")).Click();

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(broadcast_group_name); //name

            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(broadcast_group_description); //description

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop down
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + department_name + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //set owner drop down
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + owner_name + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Broadcast_Group");

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);

           

            if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(broadcast_group_name)))
            {
                takescreenshot("Broadcast_Group_Failed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Broadcast Group Failed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                // ADDING SECOND BROADCAST GROUP


                driver.FindElement(By.LinkText("Add Group")).Click();

                driver.FindElement(By.Id("txtName")).Clear();
            
                driver.FindElement(By.Id("txtName")).SendKeys(broadcast_group_name_2); //name

                driver.FindElement(By.Id("txtDesc")).Clear();
            
                driver.FindElement(By.Id("txtDesc")).SendKeys(broadcast_group_description_2); //description

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop down
            
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + department_name + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //set owner drop down
            
                Thread.Sleep(2000);

            
                driver.FindElement(By.XPath("//li[text()='" + owner_name + "']")).Click();

            
                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

            
                driver.FindElement(By.Id("btnSaveTabOne")).Click();
            
                Thread.Sleep(2000);

            
                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            
                Thread.Sleep(2000);

            
                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

                Thread.Sleep(5000);
            
                driver.FindElement(By.Id("addRec")).Click();

                Thread.Sleep(3000);
            
                driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            
                Thread.Sleep(2000);

            
                takescreenshot("Broadcast_Group");

            
                driver.FindElement(By.Id("btnCancelTwo")).Click();
            
                Thread.Sleep(2000);


                // VERIFYING SECOND ADDED BROADCAST GROUP

                if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(broadcast_group_name_2)))
                {
                    takescreenshot("Second_Added_Broadcast_Group_Failed");
                    Assert.Fail("Second Added Broadcast Group Failed ...");
                }
                else
                {
                   //EDITING SECOND BROADCAST GROUP 

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtName")).Clear();
                    driver.FindElement(By.Id("txtName")).SendKeys(broadcast_group_name_2_edited); //name

                    driver.FindElement(By.Id("txtDesc")).Clear();
                    driver.FindElement(By.Id("txtDesc")).SendKeys(broadcast_group_description_2_edited); //description

                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop down

                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//li[text()='" + department_name + "']")).Click();

                    driver.FindElement(By.Id("btnSaveTabOne")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab

                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                    Thread.Sleep(5000);

                    driver.FindElement(By.Id("addRec")).Click();
                    Thread.Sleep(3000);

                    driver.FindElement(By.Id("btnSaveTabTwo")).Click();

                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("btnCancelTwo")).Click();
                    Thread.Sleep(2000);

            
                    // VERIFYING SECOND EDITED BROADCAST GROUP

                    if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(broadcast_group_name_2_edited)))
                    {
                        takescreenshot("Editing_Broadcast_Group_Failed");
                        Assert.Fail("Editing Second Broadcast Group Failed ...");
                    }
                    else
                    {
                        //DELETING EDITED BROADCAST GROUP

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[2]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(broadcast_group_name_2_edited)))
                        {
                            takescreenshot("Broadcast_Group_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Broadcast Group Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }
                        else
                        {
                            takescreenshot("Deleting_Broadcast_Group_Failed");
                            Assert.Fail("Deleting Second Broadcast Group Failed ...");
                        }
                    }
                
                }

            }

        }



        [Test]
        public void f_Add_Edit_Delete_Escalation_Group()
        {


            string receiver_name = "receiver_smtp";
            
            string escalation_group_name = "Escalation_Group";
            string escalation_group_name_2 = "Escalation_new_Group_2";
            string escalation_group_name_2_edited = "edited_Escalation_Group_2_";

            string escalation_group_description = "Escalation Group Description";
            string escalation_group_description_2 = "Escalation Group Description_2";
            string escalation_group_description_2_edited = "Escalation Group Description_2 Edited";

            string set_owner = "admin";

            check_driver_type(driver_type, "recipients", "Escalation", "Recipients");

            Assert.AreEqual("Escalation Groups", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

            driver.FindElement(By.LinkText("Add Group")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(escalation_group_name); //name

            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(escalation_group_description); //description

            driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox 

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //cycle drop down

            driver.FindElement(By.XPath("//li[text()='2']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //member of department dropdown

            driver.FindElement(By.XPath("//li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click(); //set owner drop down 

            driver.FindElement(By.XPath("//li[text()='" + set_owner + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
            Thread.Sleep(5000);


            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //Delay dropp down
            Thread.Sleep(2000);

            //  driver.FindElement(By.XPath("//div[@id='membersGrid']/table/tbody/tr[3]/td[10]/div/ul/li[2]")).Click();

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Escalation_Group");

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);

          

            if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(escalation_group_name)))
            {
                takescreenshot("Escalation_Group_Failed");
                Assert.Fail("Escalation Group Failed ...");

              /*  takescreenshot("Escalation_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Escalation Group Passed ... ^^^^^^^^^^^^^^^");
               */
            }
            else
            {
                // ADDING SECOND ESCALATION GROUP


                driver.FindElement(By.LinkText("Add Group")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("txtName")).Clear();
                driver.FindElement(By.Id("txtName")).SendKeys(escalation_group_name_2); //name

                driver.FindElement(By.Id("txtDesc")).Clear();
                driver.FindElement(By.Id("txtDesc")).SendKeys(escalation_group_description_2); //description

                driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox 

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //cycle drop down

                driver.FindElement(By.XPath("//li[text()='2']")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //member of department dropdown

                driver.FindElement(By.XPath("//li[text()='Default']")).Click();

                driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click(); //set owner drop down 

                driver.FindElement(By.XPath("//li[text()='" + set_owner + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

                driver.FindElement(By.Id("btnSaveTabOne")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.Id("addRec")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //Delay dropp down
                Thread.Sleep(2000);

                //  driver.FindElement(By.XPath("//div[@id='membersGrid']/table/tbody/tr[3]/td[10]/div/ul/li[2]")).Click();

                driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                Thread.Sleep(2000);

                takescreenshot("Escalation_Group");

                driver.FindElement(By.Id("btnCancelTwo")).Click();
                Thread.Sleep(2000);


                // VERIFYING SECOND ADDED ESCALATION GROUP

                if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(escalation_group_name_2)))
                {
                    takescreenshot("Second_Added_Escalation_Group_Failed");
                    Assert.Fail("Second Added Escalation Group Failed ...");
                }
                else
                {
                    //EDITING SECOND ESCALATION GROUP 

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtName")).Clear();
                    driver.FindElement(By.Id("txtName")).SendKeys(escalation_group_name_2_edited); //name

                    driver.FindElement(By.Id("txtDesc")).Clear();
                    driver.FindElement(By.Id("txtDesc")).SendKeys(escalation_group_description_2_edited); //description

                    driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //member of department dropdown

                    driver.FindElement(By.XPath("//li[text()='Default']")).Click();

                    driver.FindElement(By.Id("btnSaveTabOne")).Click();
                    Thread.Sleep(3000);

                    driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                    Thread.Sleep(5000);

                    driver.FindElement(By.Id("addRec")).Click();
                    Thread.Sleep(2000);

                    //  driver.FindElement(By.XPath("//div[@id='membersGrid']/table/tbody/tr[3]/td[10]/div/ul/li[2]")).Click();

                    driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("btnCancelTwo")).Click();
                    Thread.Sleep(2000);


                    // VERIFYING SECOND EDITED ESCALATION GROUP

                    if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(escalation_group_name_2_edited)))
                    {
                        takescreenshot("Editing_Escalation_Group_Failed");
                        Assert.Fail("Editing Escalation Group Failed ...");
                    }
                    else
                    {
                        //DELETING EDITED ESCALATION GROUP

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(escalation_group_name_2_edited)))
                        {
                            takescreenshot("Broadcast_Group_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Escalation Group Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }
                        else
                        {
                            takescreenshot("Deleting_Escalation_Group_Failed");
                            Assert.Fail("Deleting Escalation Group Failed ...");
                        }
                    }

                }

            }

        }



        [Test]
        public void g_Add_Edit_Delete_On_Duty_Group()
        {


            string receiver_name = "receiver_smtp";

            string on_duty_group_name = "On_Duty_Group";
            string on_duty_group_name_2 = "On_Duty_new_Group_2";
            string on_duty_group_name_2_edited = "edited_On_Duty_Group_2";
            
            string on_duty_group_description = "On Duty Group Description";
            string on_duty_group_description_2 = "On Duty Group Description_2";
            string on_duty_group_description_2_edited = "On Duty Group Description_2_edited";

            string set_owner = "admin";

            check_driver_type(driver_type, "recipients", "On-Duty", "Recipients");

            Assert.AreEqual("On-Duty Groups", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

            driver.FindElement(By.LinkText("Add Group")).Click();

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(on_duty_group_name);

            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(on_duty_group_description);

            driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop odwn

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/ul/li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();//set owner drop down

            driver.FindElement(By.XPath("//li[1][text()='" + set_owner + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("schedule")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click(); //select receiver name drop down
            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.LinkText("Add Monthly")).Click();

            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys("monthly");

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath("(//li[text()='02'])[1]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath("(//li[text()='02'])[3]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[9]")).Click(); //Days dropdown

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[1]/ul/li[2][text()='First']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[10]")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[2]/ul/li[2][text()='Monday']")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("startpicker")).Click(); //Range start from calendar
            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("8")).Click();

            driver.FindElement(By.Id("btnSaveSchedule")).Click();

            takescreenshot("On_Duty Group");

            // driver.FindElement(By.LinkText("Close")).Click(); // not working right now

            driver.FindElement(By.LinkText("Recipients")).Click();

            driver.FindElement(By.XPath("(//a[contains(text(),'On-Duty')])[2]")).Click(); // as an alternate of close button , bcz its not working right now


            if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(on_duty_group_name))) // /div[1]/div[1]/div/div[3]
            {

                takescreenshot("On_Duty_Group_Failed");

                Assert.Fail("On_Duty Group Failed ...");
                /*
                takescreenshot("On_Duty_Group_Passed");

                Console.WriteLine("^^^^^^^^^^^^^^^ On_Duty Group Passed ... ^^^^^^^^^^^^^^^");
                 */ 
            }
            else
            {

                // ADDING SECOND ONDUTY GROUP

                driver.FindElement(By.LinkText("Add Group")).Click();

                driver.FindElement(By.Id("txtName")).Clear();
                driver.FindElement(By.Id("txtName")).SendKeys(on_duty_group_name_2);

                driver.FindElement(By.Id("txtDesc")).Clear();
                driver.FindElement(By.Id("txtDesc")).SendKeys(on_duty_group_description_2);

                driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop odwn

                driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/ul/li[text()='Default']")).Click();

                driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();//set owner drop down

                driver.FindElement(By.XPath("//li[1][text()='" + set_owner + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

                driver.FindElement(By.Id("btnSaveTabOne")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.Id("addRec")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("schedule")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click(); //select receiver name drop down
                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

                driver.FindElement(By.LinkText("Add Monthly")).Click();

                driver.FindElement(By.Id("sname")).Clear();
                driver.FindElement(By.Id("sname")).SendKeys("monthly");

                driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //timeframe drop down
                driver.FindElement(By.XPath("(//li[text()='02'])[1]")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click(); //timeframe drop down
                driver.FindElement(By.XPath("(//li[text()='02'])[3]")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[9]")).Click(); //Days dropdown

                driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[1]/ul/li[2][text()='First']")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[10]")).Click();

                driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[2]/ul/li[2][text()='Monday']")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.Id("startpicker")).Click(); //Range start from calendar
                Thread.Sleep(2000);

                driver.FindElement(By.LinkText("8")).Click();

                driver.FindElement(By.Id("btnSaveSchedule")).Click();

                takescreenshot("On_Duty Group");

                // driver.FindElement(By.LinkText("Close")).Click(); // not working right now

                driver.FindElement(By.LinkText("Recipients")).Click();

                driver.FindElement(By.XPath("(//a[contains(text(),'On-Duty')])[2]")).Click(); // as an alternate of close button , bcz its not working right now

            


                // VERIFYING SECOND ADDED ONDUTY GROUP

                if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(on_duty_group_name_2)))
                {
                    takescreenshot("Second_Added_Onduty_Group_Failed");
                    Assert.Fail("Second Added Onduty Group Failed ...");
                }
                else
                {
                    //EDITING SECOND ONDUTY GROUP 

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtName")).Clear();
                    driver.FindElement(By.Id("txtName")).SendKeys(on_duty_group_name_2_edited); //name

                    driver.FindElement(By.Id("txtDesc")).Clear();
                    driver.FindElement(By.Id("txtDesc")).SendKeys(on_duty_group_description_2_edited); //description

                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop odwn

                    driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/ul/li[text()='Default']")).Click();


                    driver.FindElement(By.Id("btnSaveTabOne")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                    Thread.Sleep(5000);

                    driver.FindElement(By.Id("addRec")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("schedule")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click(); //select receiver name drop down
                    driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

                    driver.FindElement(By.LinkText("Add Monthly")).Click();

                    driver.FindElement(By.Id("sname")).Clear();
                    driver.FindElement(By.Id("sname")).SendKeys("monthly1");

                    driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //timeframe drop down
                    driver.FindElement(By.XPath("(//li[text()='02'])[1]")).Click();

                    driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click(); //timeframe drop down
                    driver.FindElement(By.XPath("(//li[text()='02'])[3]")).Click();

                    driver.FindElement(By.XPath("(//a[@class='selector'])[9]")).Click(); //Days dropdown

                    driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[1]/ul/li[2][text()='First']")).Click();

                    driver.FindElement(By.XPath("(//a[@class='selector'])[10]")).Click();

                    driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[2]/ul/li[2][text()='Monday']")).Click();
                    Thread.Sleep(3000);

                    driver.FindElement(By.Id("startpicker")).Click(); //Range start from calendar
                    Thread.Sleep(2000);
                    driver.FindElement(By.LinkText("8")).Click();

                    driver.FindElement(By.Id("btnSaveSchedule")).Click();

                    driver.FindElement(By.LinkText("Close")).Click(); 



                    // VERIFYING SECOND EDITED ONDUTY GROUP

                    if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(on_duty_group_name_2_edited)))
                    {
                        takescreenshot("Editing_Onduty_Group_Failed");
                        Assert.Fail("Editing Onduty Group Failed ...");
                    }
                    else
                    {
                        //DELETING EDITED ONDUTY GROUP

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(on_duty_group_name_2_edited)))
                        {
                            takescreenshot("Onduty_Group_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Onduty Group Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }
                        else
                        {
                            takescreenshot("Deleting_Onduty_Group_Failed");
                            Assert.Fail("Deleting Onduty Group Failed ...");
                        }
                    }

                }

            }

        }



        [Test]
        public void h_Add_Edit_Delete_Follow_me_Group()
        {


            string receiver_name = "receiver_smtp";

            string follow_me_group_name = "Follow_me_Group";
            string follow_me_group_name_2 = "Follow_me_new_Group_2";
            string follow_me_group_name_2_edited = "edited_Follow_me_Group_2";
            
            string follow_me_group_description = "Follow me Group Description";
            string follow_me_group_description_2 = "Follow me Group Description_2";
            string follow_me_group_description_2_edited = "Follow me Group Description_2 Edited";

            
            string set_owner = "admin";

            check_driver_type(driver_type, "recipients", "Follow-Me", "Recipients");

            Assert.AreEqual("Follow-Me Groups", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

            driver.FindElement(By.LinkText("Add Group")).Click();

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(follow_me_group_name); //name

            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(follow_me_group_description); //description

            driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop odwn

            driver.FindElement(By.XPath("//li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();//set owner drop down

            driver.FindElement(By.XPath("//li[1][text()='" + set_owner + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("schedule")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click(); //select receiver name drop down
            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys("weekly");

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/ul/li[2]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/ul/li[2]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[8]")).Click(); // Repeat every drop down
            driver.FindElement(By.XPath(".//*[@id='sch_weekly']/fieldset[1]/div/ul/li[3]")).Click();

            driver.FindElement(By.Id("schSunday")).Click(); // On these days
            Thread.Sleep(2000);
            driver.FindElement(By.Id("schMonday")).Click();

            driver.FindElement(By.Id("startpicker")).Click(); //Range start from calendar
            driver.FindElement(By.LinkText("8")).Click();

            driver.FindElement(By.Id("btnSaveSchedule")).Click();
            Thread.Sleep(2000);

          
            takescreenshot("Follow_me_Group");

            driver.FindElement(By.LinkText("Close")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("Grid Text:" + " " + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text);

            if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(follow_me_group_name)))
            {

                takescreenshot("Follow_me_Group_Failed");

                Assert.Fail("Follow_me Group Failed ...");

                /*
                takescreenshot("Follow_me_Group_Passed");

                Console.WriteLine("^^^^^^^^^^^^^^^ Follow_me Group Passed ... ^^^^^^^^^^^^^^^");
                 */ 
            }
            else
            {

                // ADDING SECOND FOLLOWME GROUP

                driver.FindElement(By.LinkText("Add Group")).Click();

                driver.FindElement(By.Id("txtName")).Clear();
                driver.FindElement(By.Id("txtName")).SendKeys(follow_me_group_name_2); //name

                driver.FindElement(By.Id("txtDesc")).Clear();
                driver.FindElement(By.Id("txtDesc")).SendKeys(follow_me_group_description_2); //description

                driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop odwn

                driver.FindElement(By.XPath("//li[text()='Default']")).Click();

                driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();//set owner drop down

                driver.FindElement(By.XPath("//li[1][text()='" + set_owner + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

                driver.FindElement(By.Id("btnSaveTabOne")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.Id("addRec")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("btnCancelTwo")).Click();

                // VERIFYING SECOND ADDED FOLLOWME GROUP

                if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(follow_me_group_name_2)))
                {
                    takescreenshot("Second_Added_follow_me_Group_Failed");
                    Assert.Fail("Second Added follow_me Group Failed ...");
                }
                else
                {
                    //EDITING SECOND FOLLOWME GROUP 

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtName")).Clear();
                    driver.FindElement(By.Id("txtName")).SendKeys(follow_me_group_name_2_edited); //name

                    driver.FindElement(By.Id("txtDesc")).Clear();
                    driver.FindElement(By.Id("txtDesc")).SendKeys(follow_me_group_description_2_edited); //description

                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department drop odwn

                    driver.FindElement(By.XPath("//li[text()='Default']")).Click();

                    driver.FindElement(By.Id("btnSaveTabOne")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("btnCancelTwo")).Click();
                    Thread.Sleep(2000);


                    // VERIFYING SECOND EDITED FOLLOWME GROUP

                    if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(follow_me_group_name_2_edited)))
                    {
                        takescreenshot("Editing_follow_me_Group_Failed");
                        Assert.Fail("Editing follow_me Group Failed ...");
                    }
                    else
                    {
                        //DELETING EDITED FOLLOWME GROUP

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(follow_me_group_name_2_edited)))
                        {
                            takescreenshot("follow_me_Group_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete follow_me Group Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }
                        else
                        {
                            takescreenshot("Deleting_follow_me_Group_Failed");
                            Assert.Fail("Deleting follow_me Group Failed ...");
                        }
                    }

                }

            }

        }



        [Test]
        public void i_Add_Edit_Delete_Rotate_Group()
        {

            string receiver_name = "receiver_smtp";
            string rotate_group_name = "Rotate_Group";
            string rotate_group_name_2 = "Rotate_new_Group_2";
            string rotate_group_name_2_edited = "edited_Rotate_Group_2";

            
            string rotate_group_description = "Rotate Group Description";
            string rotate_group_description_2 = "Rotate Group Description_2";
            string rotate_group_description_2_edited = "Rotate Group Description_2_edited";

            string set_owner = "admin";

            check_driver_type(driver_type, "recipients", "Rotation", "Recipients");

            Assert.AreEqual("Rotate Groups", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

            driver.FindElement(By.LinkText("Add Group")).Click();

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(rotate_group_name); //name

            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(rotate_group_description); //description

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department 

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/ul/li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //set owner drop down

            driver.FindElement(By.XPath("//li[1][text()='" + set_owner + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); // Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Rotation_Group");


           

            if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(rotate_group_name)))
            {
                takescreenshot("Rotation_Group_Failed");
                Assert.Fail("Rotation Group Failed ...");
                
                /*
                takescreenshot("Rotation_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Rotation Group Passed ... ^^^^^^^^^^^^^^^");
                 */ 
            }
            else
            {
                // ADDING SECOND ROTATE GROUP

                driver.FindElement(By.LinkText("Add Group")).Click();

                driver.FindElement(By.Id("txtName")).Clear();
                driver.FindElement(By.Id("txtName")).SendKeys(rotate_group_name_2); //name

                driver.FindElement(By.Id("txtDesc")).Clear();
                driver.FindElement(By.Id("txtDesc")).SendKeys(rotate_group_description_2); //description

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department 

                driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/ul/li[text()='Default']")).Click();

                driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //set owner drop down

                driver.FindElement(By.XPath("//li[1][text()='" + set_owner + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); // Alert this owner for membership changes checkbox

                driver.FindElement(By.Id("btnSaveTabOne")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.Id("addRec")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("btnCancelTwo")).Click();
                Thread.Sleep(2000);



                // VERIFYING SECOND ADDED ROTATE GROUP

                if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(rotate_group_name_2)))
                {
                    takescreenshot("Second_Added_Rotation_Group_Failed");
                    Assert.Fail("Second Added Rotation Group Failed ...");
                }
                else
                {
                    //EDITING SECOND ROTATE GROUP 

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtName")).Clear();
                    driver.FindElement(By.Id("txtName")).SendKeys(rotate_group_name_2_edited); //name

                    driver.FindElement(By.Id("txtDesc")).Clear();
                    driver.FindElement(By.Id("txtDesc")).SendKeys(rotate_group_description_2_edited); //description

                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //member of department 

                    driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/ul/li[text()='Default']")).Click();


                    driver.FindElement(By.Id("btnSaveTabOne")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                    Thread.Sleep(5000);

                    driver.FindElement(By.Id("addRec")).Click();
                    Thread.Sleep(3000);

                    driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("btnCancelTwo")).Click();
                    Thread.Sleep(2000);


                    // VERIFYING SECOND EDITED ROTATE GROUP

                    if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(rotate_group_name_2_edited)))
                    {
                        takescreenshot("Editing_Rotation_Group_Failed");
                        Assert.Fail("Editing Rotation Group Failed ...");
                    }
                    else
                    {
                        //DELETING EDITED ROTATE GROUP

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(rotate_group_name_2_edited)))
                        {
                            takescreenshot("Rotation_Group_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Rotation Group Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }
                        else
                        {
                            takescreenshot("Deleting_Rotation_Group_Failed");
                            Assert.Fail("Deleting Rotation Group Failed ...");
                        }
                    }

                }

            }

        }



        [Test]
        public void j_Add_Edit_Delete_Subscription_Group()
        {


            string receiver_name = "receiver_smtp";
            
            string subscription_group_name = "Subscription_Group";
            string subscription_group_name_2 = "Subscription_new_Group_2";
            string subscription_group_name_2_edited = "edited_Subscription_Group_2";
            
            string subscription_group_description = "Subscription Group Description";
            string subscription_group_description_2 = "Subscription Group Description_2";
            string subscription_group_description_2_edited = "Subscription Group Description_2 Edited";

            
            string subscription_topic = "Demo topic";

            check_driver_type(driver_type, "recipients", "Subscription Groups", "Recipients");

            Assert.AreEqual("Subscription Groups", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

            driver.FindElement(By.LinkText("Add Group")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(subscription_group_name); //name

            driver.FindElement(By.Id("txtTopic")).Clear();
            driver.FindElement(By.Id("txtTopic")).SendKeys(subscription_topic); //topic

            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(subscription_group_description); //description

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click();

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Subscription_Group");

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);

          

            if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(subscription_group_name)))
            {

                takescreenshot("Subscription_Group_Failed");
                Assert.Fail("Subscription Group Failed ...");

                /*
                takescreenshot("Subscription_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Subscription Group Passed ... ^^^^^^^^^^^^^^^");
                 */ 
            }
            else
            {
                // ADDING SECOND SUBSCRIPTION GROUP

                driver.FindElement(By.LinkText("Add Group")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("txtName")).Clear();
                driver.FindElement(By.Id("txtName")).SendKeys(subscription_group_name_2); //name

                driver.FindElement(By.Id("txtTopic")).Clear();
                driver.FindElement(By.Id("txtTopic")).SendKeys(subscription_topic); //topic

                driver.FindElement(By.Id("txtDesc")).Clear();
                driver.FindElement(By.Id("txtDesc")).SendKeys(subscription_group_description_2); //description

                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click();

                driver.FindElement(By.Id("btnSaveTabOne")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.Id("addRec")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                Thread.Sleep(2000);

                takescreenshot("Subscription_Group");

                driver.FindElement(By.Id("btnCancelTwo")).Click();
                Thread.Sleep(2000);


                // VERIFYING SECOND ADDED SUBSCRIPTION GROUP

                if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(subscription_group_name_2)))
                {
                    takescreenshot("Second_Added_Subscription_Group_Failed");
                    Assert.Fail("Second Added Subscription Group Failed ...");
                }
                else
                {
                    //EDITING SECOND SUBSCRIPTION GROUP 

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtName")).Clear();
                    driver.FindElement(By.Id("txtName")).SendKeys(subscription_group_name_2_edited); //name

                    driver.FindElement(By.Id("txtTopic")).Clear();
                    driver.FindElement(By.Id("txtTopic")).SendKeys(subscription_topic); //topic


                    driver.FindElement(By.Id("txtDesc")).Clear();
                    driver.FindElement(By.Id("txtDesc")).SendKeys(subscription_group_description_2_edited); //description

                    driver.FindElement(By.Id("btnSaveTabOne")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
                    Thread.Sleep(5000);

                    driver.FindElement(By.Id("addRec")).Click();
                    Thread.Sleep(3000);

                    driver.FindElement(By.Id("btnSaveTabTwo")).Click();
                    Thread.Sleep(2000);

                    // VERIFYING SECOND EDITED SUBSCRIPTION GROUP

                    if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(subscription_group_name_2_edited)))
                    {
                        takescreenshot("Editing_Subscription_Group_Failed");
                        Assert.Fail("Editing Subscription Group Failed ...");
                    }
                    else
                    {
                        //DELETING EDITED SUBSCRIPTION GROUP

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                        if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(subscription_group_name_2_edited)))
                        {
                            takescreenshot("Subscription_Group_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Subscription Group Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }
                        else
                        {
                            takescreenshot("Deleting_Subscription_Group_Failed");
                            Assert.Fail("Deleting Subscription Group Failed ...");
                        }
                    }

                }

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

                WaitForElementToExist(id_name, driver);

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

            WaitForElementToExist(id_name, driver);

            var hoveritem = driver.FindElement(By.Id(id_name));

            Actions action1 = new Actions(driver); //simply my webdriver
            Thread.Sleep(5000);

            action1.MoveToElement(hoveritem).Perform(); //move to list element that needs to be hovered

            WaitForElementToExistUsingLinkText(link_text, driver);

            driver.FindElement(By.XPath("(//a[text()='" + link_text + "'])[1]")).Click();
            Thread.Sleep(3000);


            //------ Focus out the mouse to disappear hovered dialog ------

            Actions action2 = new Actions(driver);
            Thread.Sleep(5000);

            action2.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(3000);


        }

        public static void WaitForElementToExist(string ID, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until<bool>((d) =>
            {
                try
                {
                    // If the find succeeds, the element exists, and
                    // we want the element to *not* exist, so we want
                    // to return true when the find throws an exception.
                    IWebElement element = d.FindElement(By.Id(ID));
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        public static void WaitForElementToExistUsingLinkText(string link_text, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until<bool>((d) =>
            {
                try
                {
                    // If the find succeeds, the element exists, and
                    // we want the element to *not* exist, so we want
                    // to return true when the find throws an exception.
                    IWebElement element = d.FindElement(By.LinkText(link_text));
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
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

