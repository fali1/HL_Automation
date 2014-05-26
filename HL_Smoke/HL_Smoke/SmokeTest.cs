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
    public class SmokeTest
    {
        private IWebDriver driver;

        private StringBuilder verificationErrors;
        
        private string baseURL;
        
        private bool acceptNextAlert = true;
        
        public string login_name = "fahad"; //used in login and session manager testcases
        
        public string login_pwd = "123";
        
        public string welcome_username = "Welcome admin"; //used in login testcase to verify 'Welcome user' label after login
        
        public string browser = "Mozilla"; //used in session manager according to browser(firefox,chrome,IE)
        
        public string driver_type;

        string browser_type;

        string browser_name;

        string testcase_name;
        
        string testcase_executed;
        
        string testcase_result;
        
        string testcase_sucess;
        
        string testcase_time;

        string user_label;

        string trimmed_user_label;

        string create_directory_path = @".\Screenshots_Testcase_Results";

        int testcase_count=0;

        int test_result_exist = 0;

        int testcase_success_count = 0;

        int testcase_failed_count = 0;

        ArrayList testcase_name_list = new ArrayList();
        ArrayList testcase_executed_list= new ArrayList();
        ArrayList testcase_result_list= new ArrayList();
        ArrayList testcase_success_list= new ArrayList();
        ArrayList testcase_time_list = new ArrayList();
        ArrayList testcase_msg_list = new ArrayList();
        ArrayList testcase_stack_list = new ArrayList();


        string create_directory_path_with_time;


        
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


            baseURL = "http://localhost:8000/";

            driver.Navigate().GoToUrl(baseURL + "/HipLink5UI-Work/index.html#login");

            driver.Manage().Window.Maximize();//maximize browser

            driver.FindElement(By.Id("username")).Clear();

            driver.FindElement(By.Id("username")).SendKeys(login_name);

            driver.FindElement(By.Id("password")).Clear();

            driver.FindElement(By.Id("password")).SendKeys(login_pwd);

            driver.FindElement(By.CssSelector("a.c_btn_large1.login_button")).Click();// user login button

            Thread.Sleep(3000);

            takescreenshot("login");

            Console.WriteLine("User label:"+"*"+driver.FindElement(By.XPath("//li[@class='user_name']")).Text+"*");

            user_label = driver.FindElement(By.XPath("//li[@class='user_name']")).Text.ToString();

            trimmed_user_label = user_label.TrimEnd();

            Console.WriteLine("User label Trimmed at the end:" + "*"+trimmed_user_label+ "*");
          
            Assert.AreEqual(trimmed_user_label, "Welcome fahad");
            
            verificationErrors = new StringBuilder();
        }


        [Test]
        public void shortcut_keys()
        {

            // Users data
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pud"); 

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);
            
            }
            else
            {

                Assert.Fail("Shortcutkeys ");
            
            }

            // Change Password
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pcp");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Common System Information
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("psi");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Usage Reports
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pur");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Bio information
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pbi");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Recipients
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pfr");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Statistics
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("prs");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Global Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ags");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Directories
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asd");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // LDAP Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("alds");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Log Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("als");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // DB Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ads");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Timezone Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ats");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Messengers
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("amr");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Carriers
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("acr");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Filters
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("afr");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // System Attendant Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asa");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Backup Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("abs");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Services
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asv");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Logs
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asl");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Sessions Manager
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asm");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // About
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("me");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Users
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sur");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // User Groups
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sug");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // department panel
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sdp"); 

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Recipient Users
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sru");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Alarm Notification Gateway 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("san");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // File System Interface
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sfs");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Email Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("seg");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // SNPP Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sng");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // XMPP Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sxg");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // TAP Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("stg");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // SNMP ALerts
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("smg");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Websign up Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sws");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // GIS Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sgs");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Websign up Profiles
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swp");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Websign up Topic Profiles
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swt");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Websign up Characteristics Profiles
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swc");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Websign up Recipients
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swr");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Enterprise Mobility Activation
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sea");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }

            // Enterprise Mobility Configuration
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sec");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {

                Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys ");

            }


        }


        [Test]
        public void Reports_Panel()
        {
            string receiver_name = "receiver_smtp";
            Thread.Sleep(2000);

            hover_func("reports", "Detail", "Reports");

            Assert.AreEqual("Reports", driver.FindElement(By.XPath("//span[@id='main_title']")).Text); //verfying Reports panel label

            hover_func("reports", "Summary", "Reports");

            driver.FindElement(By.LinkText("Send")).Click(); //navigate to send panel

            driver.FindElement(By.LinkText("Reports")).Click();
            Thread.Sleep(2000);

            takescreenshot("Reports_Panel");

            if (driver.FindElement(By.XPath("(//div[@class='mCSB_container']/div[1]/div[4])[2]")).Text.Contains(receiver_name))
            {

                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//a[text()='Summary'])[3]")).Click(); //navigate to summary tab 
                Thread.Sleep(2000);

                takescreenshot("Reports_Panel_Summary");

                driver.FindElement(By.XPath("(//a[text()='Detail'])[3]")).Click();
                Thread.Sleep(2000);

                Assert.Pass("Reports_Panel Passed...");
            }
            else
            {
                Assert.Fail("Reports_Panel Failed...");
            }
            

        }



        [Test]
        public void Queues_Panel()
        {
            string receiver_name = "receiver_smtp";

            check_driver_type(driver_type, "queues", "Default", "Queues");

            Assert.AreEqual("Default Queue", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text); //verifying Queues Panel label

            takescreenshot("Default_Queue_Main_Panel");

            if (driver.FindElement(By.Id("testing")).Text.Contains("Default Queue") && driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(receiver_name))
            {

                driver.FindElement(By.XPath("(//ul[@class='row_grid_actions']/li[1]/a)[1]")).Click(); //click on edit button to view the message file
                Thread.Sleep(2000);

                takescreenshot("Queues_View_Message_Details");

                if (driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text.Contains("Queues - View Message Details")) //verfying Queues - View Message Details label 
                {
                    Assert.Pass("Queues_Panel_Passed");
                }
                else
                {
                    Assert.Fail("Queues_Panel_Failed");
                }

            }
            else
            {
                Assert.Fail("Queues_Panel_Failed");
            }

        }


        [Test]
        public void Add_Message_Template_Primary_Send_Panel()
        {
            string receiver_name = "receiver_smtp";

            driver.FindElement(By.Id("send")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("Primary Send")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//*[contains(text(),'" + receiver_name + "')]")).Click(); //selecting receiver from members list

            driver.FindElement(By.Id("moveItemRight")).Click();

            driver.FindElement(By.XPath("//div[@class='custom dropdown small template_open']/a[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@class='custom dropdown small template_open open']/ul/li[contains(text(),'Message Template')]")).Click(); // selecting message template from template dropdown
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//ul[@id='msgTemplateList']/li/a[contains(text(),'Message Template')]")).Click(); //message template popup 
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnSendTemp")).Click(); //open template button
            Thread.Sleep(2000);

            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='txtAreaMessage']")).Text + "*");

            Console.WriteLine("condition passed");

            driver.FindElement(By.Id("btnSend")).Click(); //send message button
            Thread.Sleep(2000);

            takescreenshot("Add_Message_Template_Primary_Send_Panel");

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click();  //return to message button
            Thread.Sleep(2000);

            Assert.Pass("Add_Message_Template_Primary_Send_Panel Passed...");
            

        }


        [Test]
        public void Escalation_Send_Panel()
        {
            string receiver_name = "receiver_smtp";
            string msg = "hello escalation send panel";

            check_driver_type(driver_type, "send", "Escalation Send", "Send");

            Assert.AreEqual("Escalation Send Panel", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text); // verifying page title

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

            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            takescreenshot("Escalation_Send_Panel");

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); //return to message button
            Thread.Sleep(2000);

            Assert.Pass("Escalation_Send_Panel Passed...");
            
        }


        [Test]
        public void Fax_Send_Panel()
        {
            string receiver_name = "receiver_fax";
            string msg = "hello fax receiver";
            
            check_driver_type(driver_type, "send", "Fax Send", "Send");

            Assert.AreEqual("Fax Send Panel", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text);

            driver.FindElement(By.XPath("//*[contains(text(),'" + receiver_name + "')]")).Click(); //select receiver to send message

            driver.FindElement(By.Id("moveItemRight")).Click();

            driver.FindElement(By.Id("txtAreaMessage")).Clear();

            driver.FindElement(By.Id("txtAreaMessage")).SendKeys(msg);

            driver.FindElement(By.Id("incMsgIdCheck")).Click(); // include message id checkbox

            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            takescreenshot("Fax_Send_Panel");

            //driver.FindElement(By.Id("btnToMessage")).Displayed

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); // return to message button
            Thread.Sleep(2000);

            Assert.Pass("Fax_Send_Panel Passed...");

        }


        [Test]
        public void Voice_Send_Panel()
        {
            string receiver_name= "receiver_voice";
            string msg = "hello voice receiver";
            
            check_driver_type(driver_type, "send", "Voice Send", "Send");

            Assert.AreEqual("Voice Send Panel", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text); //verifying page title

            driver.FindElement(By.XPath("//*[contains(text(),'" + receiver_name + "')]")).Click(); //select receiver to send message

            driver.FindElement(By.Id("moveItemRight")).Click();
            
            driver.FindElement(By.Id("txtAreaMessage")).Clear();
            
            driver.FindElement(By.Id("txtAreaMessage")).SendKeys(msg);
            
            driver.FindElement(By.Id("incMsgIdCheck")).Click(); // include message id checkbox

            driver.FindElement(By.Id("incSound")).Click(); // send sound file first checkbox
            
            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);

            takescreenshot("Voice_Send_Panel");

            Assert.AreEqual(true, driver.FindElement(By.Id("btnToMessage")).Displayed); // Visible Works
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnToMessage")).Click(); // return to message button
            Thread.Sleep(2000);

            Assert.Pass("Voice_Send_Panel Passed...");
            


        }

        [Test]
        public void Add_Schedule_Template()
        {
            
            string schedule_template_name = "Schedule_Template_Monthly";

            check_driver_type(driver_type, "settings", "Schedule Template", "Settings");

            Assert.AreEqual("Schedule Template", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Schedule Template")).Click();

            driver.FindElement(By.XPath(".//*[@id='light']/div[2]/div[2]/div/a[2]")).Click(); //schedule type

            driver.FindElement(By.XPath(".//*[@id='light']/div[2]/div[2]/div/ul/li[text()='Monthly']")).Click();
            
            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys(schedule_template_name); //schedule template name

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/a[2]")).Click(); //time frame

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/ul/li[text()='01']")).Click();

            driver.FindElement(By.XPath("//div[@id='sch_main']/div/fieldset[2]/div[3]/a[2]")).Click(); //time frame

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/ul/li[text()='02']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("startpicker")).Click();//range start from
            
            driver.FindElement(By.LinkText("14")).Click();

            driver.FindElement(By.XPath(".//*[@class='end_after_label']")).Click(); //radio button 'End After'
            
            driver.FindElement(By.Id("txtOccurences")).Clear();
            driver.FindElement(By.Id("txtOccurences")).SendKeys("3");
            
            driver.FindElement(By.Id("saveScheduleTemplate")).Click();
            Thread.Sleep(1000);

            takescreenshot("Schedule_Template");
            Thread.Sleep(2000);

            Console.WriteLine("text:" + driver.FindElement(By.XPath(".//*[@id='gridDiv']/table/tbody")).Text);

            if (driver.FindElement(By.XPath(".//*[@id='gridDiv']/table/tbody")).Text.Contains(schedule_template_name))
            {
                takescreenshot("Schedule_Template_Passed");

                Console.WriteLine("^^^^^^^^^^^^^^^ Schedule_Template Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("Schedule_Template_Failed");

                Assert.Fail("Schedule_Template Failed ...");
            }

            
        }


        [Test]
        public void Add_Message_Template()
        {
            
            string message_template_name = "Message Template";
            string message_template_description = "Message Template Description";
            string receiver_name = "receiver_smtp";

            check_driver_type(driver_type, "settings", "Message Template", "Settings");

            Assert.AreEqual("Message Template", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Template")).Click();
            
            driver.FindElement(By.Id("txtTmpltName")).Clear();
            driver.FindElement(By.Id("txtTmpltName")).SendKeys(message_template_name); //message template name
            
            driver.FindElement(By.Id("txtAdesc")).Clear();
            driver.FindElement(By.Id("txtAdesc")).SendKeys(message_template_description); //message template description

            driver.FindElement(By.XPath(".//li[text()='" + receiver_name + "']")).Click(); //Assign Templates To Recipients list box
            Thread.Sleep(2000);

            driver.FindElement(By.Id("recipientMoveRight")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("DivTemplate")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("DivTemplate")).SendKeys("number of messages: ");
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnSelectField")).Click(); //Drop down of Create Template Content
            Thread.Sleep(2000);

            driver.FindElement(By.XPath(".//*[@id='selectTempBox']/form/div[@id='comboDiv']/div/div[2]/div/div/div/div/div/fieldset/input")).SendKeys("1");

            driver.FindElement(By.XPath("//li[text()='Add New']")).Click();

            driver.FindElement(By.XPath(".//*[@id='selectTempBox']/form/div[@id='comboDiv']/div/div[2]/div/div/div/div/div[2]/fieldset/input")).SendKeys("2");

            driver.FindElement(By.Id("btnSelOk")).Click();

            /* driver.FindElement(By.Id("btnInsertField")).Click();//btnSelectField

             driver.FindElement(By.XPath(".//*[@id='DivTemplate']/input[@type='text']")).Clear();

             driver.FindElement(By.CssSelector(".//*[@id='DivTemplate']/input[@type='text']")).SendKeys("Age:12");.//*[@id='selectTempBox']/form/div[@id='comboDiv']/div/div[2]/div/div/div/div/div/fieldset/input
            
             driver.FindElement(By.Id("btnSelectField")).Click();
            
             driver.FindElement(By.CssSelector("#objBar_360 > fieldset > input[type=\"text\"]")).Clear();
            
             driver.FindElement(By.CssSelector("#objBar_360 > fieldset > input[type=\"text\"]")).SendKeys("1");
             driver.FindElement(By.Id("add_a718c602-78a5-458c-beaf-6bd80902df40")).Click();
             driver.FindElement(By.CssSelector("#objBar_392 > fieldset > input[type=\"text\"]")).Clear();
             driver.FindElement(By.CssSelector("#objBar_392 > fieldset > input[type=\"text\"]")).SendKeys("2");
             driver.FindElement(By.Id("add_a718c602-78a5-458c-beaf-6bd80902df40")).Click();
             */

                 driver.FindElement(By.Id("btnSaveTemp")).Click();
                 Thread.Sleep(1000);

                 takescreenshot("Message_Template");
                 Thread.Sleep(2000);


                 Console.WriteLine(driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div[1]/div[1]/div[1]/div[2]")).Text);

                 if (driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div[1]/div[1]/div[1]/div[2]")).Text.Contains(message_template_name))
                 {
                     takescreenshot("Message_Template_Passed");

                     Console.WriteLine("^^^^^^^^^^^^^^^ Message_Template Passed ... ^^^^^^^^^^^^^^^");
                 }
                 else
                 {
                     takescreenshot("Message_Template_Failed");

                     Assert.Fail("Message_Template Failed ...");
                 }

        }


        [Test]
        public void Add_On_Duty_Group()
        {
            string receiver_name = "receiver_smtp";
            string on_duty_group_name = "On_Duty_Group3";
            string on_duty_group_description = "On Duty Group Description";
            string set_owner = "admin";

            check_driver_type(driver_type, "recipients", "On-Duty", "Recipients");

            Assert.AreEqual("On-Duty Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Group")).Click();
            
            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(on_duty_group_name);
            
            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(on_duty_group_description);

            driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/a[2]")).Click(); //member of department drop odwn

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/ul/li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[3]/fieldset/div[1]/a[2]")).Click();//set owner drop down

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[3]/fieldset/div[1]/ul/li[1][text()='" + set_owner + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("schedule")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@id='scheduleTab']/div/div[1]/div[1]/div/a[2]")).Click(); //select receiver name drop down
            driver.FindElement(By.XPath("//div[@id='scheduleTab']/div/div[1]/div[1]/div/ul/li[text()='" + receiver_name + "']")).Click();
            
            driver.FindElement(By.LinkText("Add Monthly")).Click();
            
            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys("monthly");

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/a[2]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/ul/li[2]")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/a[2]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/ul/li[2]")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[1]/a[2]")).Click(); //Days dropdown

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[1]/ul/li[2][text()='First']")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[2]/a[2]")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[2]/ul/li[2][text()='Monday']")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("startpicker")).Click(); //Range start from calendar
            driver.FindElement(By.LinkText("8")).Click();
            
            driver.FindElement(By.Id("btnSaveSchedule")).Click();

            takescreenshot("On_Duty Group");
            
           // driver.FindElement(By.LinkText("Close")).Click(); // not working right now

            driver.FindElement(By.LinkText("Recipients")).Click();
            
            driver.FindElement(By.XPath("(//a[contains(text(),'On-Duty')])[2]")).Click(); // as an alternate of close button , bcz its not working right now

            Console.WriteLine("Grid Text:" + " " + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text);

            if (driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(on_duty_group_name)) // /div[1]/div[1]/div/div[3]
            {
                takescreenshot("On_Duty_Group_Passed");

                Console.WriteLine("^^^^^^^^^^^^^^^ On_Duty Group Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("On_Duty_Group_Failed");

                Assert.Fail("On_Duty Group Failed ...");
            }
 
        }



        [Test]
        public void Add_Follow_me_Group()
        {
            string receiver_name = "receiver_smtp";
            string follow_me_group_name = "Follow_me_Group3";
            string follow_me_group_description = "Follow me Group Description";
            string set_owner = "admin";

            check_driver_type(driver_type, "recipients", "Follow-Me", "Recipients");

            Assert.AreEqual("Follow-Me Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Group")).Click();
           
            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(follow_me_group_name); //name
            
            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(follow_me_group_description); //description

            driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/a[2]")).Click(); //member of department drop odwn

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/ul/li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[3]/fieldset/div[1]/a[2]")).Click();//set owner drop down

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[3]/fieldset/div[1]/ul/li[1][text()='" + set_owner + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.Id("addRec")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("schedule")).Click();

            driver.FindElement(By.XPath("//div[@id='scheduleTab']/div/div[1]/div[1]/div/a[2]")).Click(); //select receiver name drop down
            driver.FindElement(By.XPath("//div[@id='scheduleTab']/div/div[1]/div[1]/div/ul/li[text()='"+receiver_name+"']")).Click();
            
            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys("weekly");

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/a[2]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/ul/li[2]")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/a[2]")).Click(); //timeframe drop down
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/ul/li[2]")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_weekly']/fieldset[1]/div/a[2]")).Click(); // Repeat every drop down
            driver.FindElement(By.XPath(".//*[@id='sch_weekly']/fieldset[1]/div/ul/li[3]")).Click();

            driver.FindElement(By.Id("schSunday")).Click(); // On these days
            Thread.Sleep(2000);
            driver.FindElement(By.Id("schMonday")).Click();
            
            driver.FindElement(By.Id("startpicker")).Click(); //Range start from calendar
            driver.FindElement(By.LinkText("6")).Click();
            
            driver.FindElement(By.Id("btnSaveSchedule")).Click();
            
        /*    driver.FindElement(By.LinkText("Add Monthly")).Click();
            
            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys("monthly");

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/a[2]")).Click();
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/ul/li[2]")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/a[2]")).Click();
            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/ul/li[2]")).Click();//*[@id='sch_monthly']/fieldset[1]/div[1]/a[2]

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[1]/a[2]")).Click();
            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[1]/ul/li[text()='Day']")).Click();

            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[2]/a[2]")).Click();
            driver.FindElement(By.XPath(".//*[@id='sch_monthly']/fieldset[1]/div[2]/ul/li[2]")).Click();

            driver.FindElement(By.CssSelector("#sch_monthly > fieldset > div.custom.dropdown > a.selector")).Click();
            driver.FindElement(By.CssSelector("#sizzle-1399274460198 > li.selected")).Click();
            driver.FindElement(By.XPath("//div[@id='sch_monthly']/fieldset/div[2]/a[2]")).Click();
            driver.FindElement(By.XPath("//ul[@id='sizzle-1399274460198']/li[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='sch_monthly']/fieldset[2]/div/a[2]")).Click();
            driver.FindElement(By.XPath("//ul[@id='sizzle-1399274460198']/li[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='sch_monthly']/fieldset[3]/div/a[2]")).Click();
            driver.FindElement(By.XPath("//ul[@id='sizzle-1399274460198']/li[2]")).Click();
            driver.FindElement(By.Id("startpicker")).Click();
            driver.FindElement(By.LinkText("7")).Click();
            driver.FindElement(By.CssSelector("#sizzle-1399274460198 > span.custom.radio")).Click();
            
            driver.FindElement(By.Id("txtOccurencies")).Clear();
            driver.FindElement(By.Id("txtOccurencies")).SendKeys("2");
            
            driver.FindElement(By.Id("btnSaveSchedule")).Click();
            
            driver.FindElement(By.LinkText("Add Non-recurrent")).Click();
            
            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys("non_recurrent");
            
            driver.FindElement(By.CssSelector("fieldset.fs_duration > div.custom.dropdown > a.selector")).Click();
            driver.FindElement(By.XPath("//ul[@id='sizzle-1399274460198']/li[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='sch_main']/div/fieldset[2]/div[3]/a[2]")).Click();
            driver.FindElement(By.XPath("//ul[@id='sizzle-1399274460198']/li[3]")).Click();
            driver.FindElement(By.LinkText("7")).Click();
            driver.FindElement(By.Id("txtNonRecEndDate")).Click();
            driver.FindElement(By.LinkText("9")).Click();*/
            
            takescreenshot("Follow_me_Group");

            driver.FindElement(By.LinkText("Close")).Click();
           
            driver.FindElement(By.LinkText("Recipients")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("Follow-Me")).Click();

            Console.WriteLine("Grid Text:" + " " + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text);

            if (driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(follow_me_group_name))
            {
                takescreenshot("Follow_me_Group_Passed");

                Console.WriteLine("^^^^^^^^^^^^^^^ Follow_me Group Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("Follow_me_Group_Failed");

                Assert.Fail("Follow_me Group Failed ...");
            }

        }

        [Test]
        public void Add_Rotate_Group()
        {
            string receiver_name = "receiver_smtp";
            string rotate_group_name = "Rotate_Group3";
            string rotate_group_description = "Rotate Group Description";
            string set_owner = "admin";

            check_driver_type(driver_type, "recipients", "Rotation", "Recipients");

            Assert.AreEqual("Rotate Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Group")).Click();
            
            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(rotate_group_name); //name

            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(rotate_group_description); //description

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/a[2]")).Click(); //member of department 

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/ul/li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/fieldset/div[1]/a[2]")).Click(); //set owner drop down

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/fieldset/div[1]/ul/li[1][text()='" + set_owner + "']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); // Alert this owner for membership changes checkbox

            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.Id("addRec")).Click();

            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Rotation_Group");

            
            Console.WriteLine("Grid Text:" + " " + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text);

            if (driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text.Contains(rotate_group_name))
            {
                takescreenshot("Rotation_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Rotation Group Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("Rotation_Group_Failed");
                Assert.Fail("Rotation Group Failed ...");
            }
           
 
        }

        [Test]
        public void Add_Escalation_Group()
        {

            string receiver_name = "receiver_smtp";
            string escalation_group_name = "Escalation_Group3";
            string escalation_group_description = "Escalation Group Description";
            string set_owner="admin";

            check_driver_type(driver_type, "recipients", "Escalation", "Recipients");

            Assert.AreEqual("Escalation Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Group")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(escalation_group_name); //name
            
            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(escalation_group_description); //description

            driver.FindElement(By.XPath("//span[text()='Rotating']")).Click(); //Rotating checkbox 

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/a[2]")).Click(); //cycle drop down

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/div/ul/li[2]")).Click();

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[3]/div/a[2]")).Click(); //member of department dropdown

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[3]/div/ul/li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[4]/fieldset/div[1]/a[2]")).Click(); //set owner drop down 

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[4]/fieldset/div[1]/ul/li[text()='"+set_owner+"']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox
            
            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(3000);
            
            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='"+receiver_name+"']")).Click();

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();
            
            
            driver.FindElement(By.Id("addRec")).Click();

            driver.FindElement(By.XPath("//div[@id='membersGrid']/table/tbody/tr/td[9]/div/a[2]")).Click(); //Delay dropp down
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@id='membersGrid']/table/tbody/tr/td[9]/div/ul/li[2]")).Click();
            
            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Escalation_Group");

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);
            
            Console.WriteLine("Grid Text:" + " " + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text);
            
            if (driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text.Contains(escalation_group_name))
            {
                takescreenshot("Escalation_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Escalation Group Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("Escalation_Group_Failed");
                Assert.Fail("Escalation Group Failed ...");
            }
        }

        [Test]
        public void Add_Subscription_Group()
        {
            string receiver_name = "receiver_smtp";
            string subscription_group_name = "Subscription_Group4";
            string subscription_group_description = "Subscription Group Description";
            string subscription_topic = "Demo topic";
            
            check_driver_type(driver_type, "recipients", "Subscription Groups", "Recipients");

            Assert.AreEqual("Subscription Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

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

            driver.FindElement(By.XPath("//li[text()='"+receiver_name+"']")).Click();

            driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

            driver.FindElement(By.Id("addRec")).Click();
            
            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Subscription_Group");

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("Grid Text:" + " " + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text);

            if (driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text.Contains(subscription_group_name))
            {
                takescreenshot("Subscription_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Subscription Group Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("Subscription_Group_Failed");
                Assert.Fail("Subscription Group Failed ...");
            }

 
        }



        [Test]
        public void Add_Broadcast_Group()
        {
            string broadcast_group_name = "Broadcast_Group3";
            string broadcast_group_description = "Broadcast Group Description";
            string department_name = "Default";
            string owner_name = "admin";
            string receiver_name = "receiver_smtp";

            check_driver_type(driver_type, "recipients", "Broadcast", "Recipients");

            Assert.AreEqual("Broadcast Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Group")).Click();
            
            driver.FindElement(By.Id("txtName")).Clear();
            driver.FindElement(By.Id("txtName")).SendKeys(broadcast_group_name); //name
            
            driver.FindElement(By.Id("txtDesc")).Clear();
            driver.FindElement(By.Id("txtDesc")).SendKeys(broadcast_group_description); //description

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/a[2]")).Click(); //member of department drop down
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/ul/li[text()='"+department_name+"']")).Click();

            driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/fieldset/div[1]/a[2]")).Click(); //set owner drop down
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/fieldset/div[1]/ul/li[text()='"+owner_name+"']")).Click();

            driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox
            
            driver.FindElement(By.Id("btnSaveTabOne")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='"+receiver_name+"']")).Click();

            driver.FindElement(By.XPath("//li[text()='"+receiver_name+"']")).Click();
            
            driver.FindElement(By.Id("addRec")).Click();
            
            driver.FindElement(By.Id("btnSaveTabTwo")).Click();
            Thread.Sleep(2000);

            takescreenshot("Broadcast_Group");

            driver.FindElement(By.Id("btnCancelTwo")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("Grid Text:" + " " + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text);

            if (driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[3]")).Text.Contains(broadcast_group_name))
            {
                takescreenshot("Broadcast_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Broadcast Group Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("Broadcast_Group_Failed");
                Assert.Fail("Broadcast Group Failed ...");
            }
        }


        [Test]
        public void Logs_Settings()
        {
            string log_level = "WARN";
            check_driver_type(driver_type, "administration", "Log Settings", "Sys Admin");

            Assert.AreEqual("Log Settings", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            // it will check individual of different categories logs

        /*    driver.FindElement(By.XPath("//input[@value='Alarm Notification Gateway']")).Click();

            driver.FindElement(By.XPath("//input[@value='Campaign Manager']")).Click();

            driver.FindElement(By.XPath("//input[@value='Confirmations']")).Click();

            driver.FindElement(By.XPath("//input[@value='GIS Campaign Manager']")).Click();
            Thread.Sleep(2000);

            // scroll down the screen until "smtp_messenger" logs is displayed

            IWebElement element = driver.FindElement(By.XPath("//div[text()='smtp_messenger']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(500);

            driver.FindElement(By.XPath("//input[@value='smtp_messenger']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("selChangeLevel")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//*[@id='selChangeLevel']/option[text()='ERROR']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnOk")).Click();
            Thread.Sleep(2000);*/

            driver.FindElement(By.Id("divGrid_selectAllRows")).Click();  // Select all/Deselect all checkbox

            driver.FindElement(By.Id("selChangeLevel")).Click();
            Thread.Sleep(2000);

            
            //driver.FindElement(By.XPath("//*[@id='selChangeLevel']/option[text()='TRACE']")).Click();*/

            new SelectElement(driver.FindElement(By.Id("selChangeLevel"))).SelectByText(log_level); // select value from dropdown
            Thread.Sleep(3000);

            driver.FindElement(By.Id("selChangeLevel")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnOk")).Click();
            Thread.Sleep(2000);
            
            takescreenshot("Log_Settings");

            if (driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div[1]/div[1]/div/div[5]/descendant::div")).Text.Equals(log_level))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^ Log Settings Passed... ^^^^^^^^^^^^^^^^^^");
            }
            else
            {
                Assert.Fail("Log Settings Failed...");
            }

        }


        [Test]
        public void Logs_View()
        {

          
            check_driver_type(driver_type, "administration", "Logs", "Sys Admin");

            Assert.AreEqual("Logs Panel", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            // Main logs

            driver.FindElement(By.XPath("//div[@class='logs_panel_list_panel']/table/tbody/tr[3]/td[2]/a[text()='Main']")).Click();
            Thread.Sleep(2000);

            if (driver.FindElement(By.Id("logHeader")).Text.Equals("Logs Panel - Main"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^   Main Logs Passed... ^^^^^^^^^^^^^^^^");

                takescreenshot("Main_logs");

                driver.Navigate().Back();

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Logs']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);
            }
            else
            {
                Assert.Fail("Main Logs Failed...");
            }

            // Messenger logs

            driver.FindElement(By.XPath("//div[@class='logs_panel_list_panel']/table/tbody/tr/td[2]/a[text()='smtp_messenger']")).Click();
            Thread.Sleep(2000);

            if (driver.FindElement(By.Id("logHeader")).Text.Equals("Logs Panel - smtp_messenger"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^   Messenger Logs Passed... ^^^^^^^^^^^^^^^^");

                takescreenshot("Messenger_logs");

                driver.Navigate().Back();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Logs']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);
            }
            else
            {
                Assert.Fail("Messenger Logs Failed...");
            }

            // System logs

            driver.FindElement(By.XPath("//div[@class='logs_panel_list_panel']/table/tbody/tr[6]/td[2]/a[text()='System Attendant']")).Click();
            Thread.Sleep(2000);

            if (driver.FindElement(By.Id("logHeader")).Text.Equals("Logs Panel - System Attendant"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^   System Logs Passed... ^^^^^^^^^^^^^^^^");

                takescreenshot("System_logs");

                driver.Navigate().Back();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Logs']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);
            }
            else
            {
                Assert.Fail("System Logs Failed...");
            }

            // Gateway logs

            driver.FindElement(By.XPath("//div[@class='logs_panel_list_panel']/table/tbody/tr[2]/td[2]/a[text()='SNPP Gateway']")).Click();
            Thread.Sleep(2000);

            if (driver.FindElement(By.Id("logHeader")).Text.Equals("Logs Panel - SNPP Gateway"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^   Gateway Logs Passed... ^^^^^^^^^^^^^^^^");

                takescreenshot("Gateway_logs");

                driver.Navigate().Back();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Logs']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);
            }
            else
            {
                Assert.Fail("Gateway Logs Failed...");
            }

            // scroll down the screen until "Campaign Manager" logs is displayed

            IWebElement element = driver.FindElement(By.XPath("//div[@class='logs_panel_list_panel']/table/tbody/tr[1]/td[2]/a[text()='Campaign Manager']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(500);

            // Miscellaneous logs

            driver.FindElement(By.XPath("//div[@class='logs_panel_list_panel']/table/tbody/tr[1]/td[2]/a[text()='Campaign Manager']")).Click();
            Thread.Sleep(2000);

            if (driver.FindElement(By.Id("logHeader")).Text.Equals("Logs Panel - Campaign Manager"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^   Miscellaneous Logs Passed... ^^^^^^^^^^^^^^^^");

                takescreenshot("Miscellaneous_logs");

                driver.Navigate().Back();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Logs']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);
            }
            else
            {
                Assert.Fail("Miscellaneous Logs Failed...");
            }

 
        }



        [Test]
        public void Services_Settings_Panel()

        {

            check_driver_type(driver_type, "administration", "Services", "Sys Admin");

            Assert.AreEqual("Services", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            //hover_func("administration", "Services");

            driver.FindElement(By.XPath("//div[@id='tab_messenger']/div[2]/div/a[2]")).Click();
            Thread.Sleep(2000);

            // for messenger services panel 

            if (IsElementPresent(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[2]/a[@class='action service_action_play']"))) //messenger service start button
            {
                driver.FindElement(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[2]/a[@class='action service_action_play']")).Click();
                Thread.Sleep(2000);

                if (IsElementPresent(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[3]/a[@class='service_action_refresh']"))) //messenger service restart button
                {
                    driver.FindElement(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[3]/a[@class='service_action_refresh']")).Click();
                    Thread.Sleep(2000);

                    if (IsElementPresent(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[2]/a[@class='action service_action_stop']"))) //messenger service stop button
                    {
                        driver.FindElement(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[2]/a[@class='action service_action_stop']")).Click();
                        Thread.Sleep(2000);

                        if (IsElementPresent(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[2]/a[@class='action service_action_play']"))) //messenger service start button
                        {
                            driver.FindElement(By.XPath("//table[@id='tblmessenger']/tbody/tr[1]/td[2]/a[@class='action service_action_play']")).Click();
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

            
            Thread.Sleep(2000);

        }


        [Test]
        public void Global_Settings_Panel()
        {
            check_driver_type(driver_type, "administration", "Global Settings", "Sys Admin");

            Assert.AreEqual("Global Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

          //  hover_func("administration", "Global Settings");

            driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click(); //goto landing for particular ID
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Global Settings']")).Click(); //goto particular panel w.r.t link
            Thread.Sleep(2000);

            //Receiver section
            driver.FindElement(By.XPath("//div[@class='viewport global_setting_type_height']/div/ul[1]/li[2]/ul/li/a[text()='Receiver']")).Click();
        
            driver.FindElement(By.LinkText("Receiver")).Click();
            
            driver.FindElement(By.Id("btnEdit")).Click();


            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[1]/div/a[2]")).Click();//Detail Receiver/User Display
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[1]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[2]/div/a[2]")).Click();//Enable Receiver First Name
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[2]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[3]/div/a[2]")).Click();//Enable Receiver Last Name
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[3]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[4]/div/a[2]")).Click();//Enable Receiver Security Code
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[4]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[5]/div/a[2]")).Click();//Enable Receiver Status
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[5]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[6]/div/a[2]")).Click();//Allow Receiver Login
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[6]/div/ul/li[text()='Yes']")).Click();

            Thread.Sleep(2000);

            // scroll down the screen until "Receiver Logon via Assigned Owner" dropdown is displayed

            IWebElement element = driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[7]/div/a[2]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(500);

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[7]/div/a[2]")).Click();//Receiver Logon via Assigned Owner
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[7]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.Id("txtemailSubject")).Clear();
            driver.FindElement(By.Id("txtemailSubject")).SendKeys("Failed over email subject");//Failed Over Email Subject 

            // scroll down the screen until "Failed Over Email Message" textbox is displayed

            IWebElement element1 = driver.FindElement(By.Id("txtemailMsg"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element1);
            Thread.Sleep(500);

            driver.FindElement(By.Id("txtemailMsg")).Clear();
            driver.FindElement(By.Id("txtemailMsg")).SendKeys("failed over email message");//Failed Over Email Message

     
            // driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[12]/div/a[2]")).SendKeys(OpenQA.Selenium.Keys.Tab);*/

            // scroll down the screen until "Notify Admin when receiver changes his/her own schedule" dropdown is displayed

            IWebElement element12 = driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[11]/div/a[2]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element12);
            Thread.Sleep(500);

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[11]/div/a[2]")).Click();//Notify Admin when receiver changes his/her own schedule
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[11]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[12]/div/a[2]")).Click();//Notify Receiver on covering other receiver
            driver.FindElement(By.XPath("//div[@id='display_receiverEdit']/fieldset[12]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//b[text()='Receiver to send Test Message']")).Click();
            
            driver.FindElement(By.Id("txtAenableReceiver")).Clear();
            driver.FindElement(By.Id("txtAenableReceiver")).SendKeys("Receiver to send test message");
            
            //Common section
            driver.FindElement(By.XPath("//*[@class='viewport global_setting_type_height']/div/ul[2]/li[2]/ul/li[1]/a[text()='Common']")).Click();
           
            driver.FindElement(By.Id("btnEdit")).Click();

            driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[1]/div/a[2]")).Click();//Enable time-stamp on all messages
            driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[1]/div/ul/li[text()='Yes']")).Click();

            driver.FindElement(By.XPath("//div[@id='messagesending_commonEdit']/fieldset[2]/div/a[2]")).Click();//Enable sender name on all messages
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

            //Department section
            driver.FindElement(By.CssSelector("a.departments")).Click();
            
            driver.FindElement(By.Id("btnEdit")).Click();

            driver.FindElement(By.XPath("//b[text()='Maximum Receivers Allowed in a Department']")).Click();//Maximum Receivers Allowed in a Department
            
            driver.FindElement(By.Id("txtMaxRecieverDPT")).Clear();
            driver.FindElement(By.Id("txtMaxRecieverDPT")).SendKeys("100");//Maximum Receivers Allowed in a Department

            driver.FindElement(By.XPath("//div[@id='departmentsEdit']/fieldset[2]/div/a[2]")).Click();//Count Recipient Groups as Members
            driver.FindElement(By.XPath("//div[@id='departmentsEdit']/fieldset[2]/div/ul/li[text()='Yes']")).Click();

            //Session section
            driver.FindElement(By.LinkText("Session")).Click();

            driver.FindElement(By.Id("btnEdit")).Click();

            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset/div/a[2]")).Click();//Temporary session
            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset/div/ul/li[text()='No']")).Click();

            driver.FindElement(By.Id("txtsessionTimeout")).Clear();//Session Timeout (minutes)
            driver.FindElement(By.Id("txtsessionTimeout")).SendKeys("40");

            driver.FindElement(By.Id("txtuserPasswordExpire")).Clear();//User password expires after (days)
            driver.FindElement(By.Id("txtuserPasswordExpire")).SendKeys("10");

            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[4]/div/a[2]")).Click();//Minimum user password length in characters
            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[4]/div/ul/li[4]")).Click();

            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[5]/div/a[2]")).Click();//User password needs at least a numeric, an alphabetic and a special char
            driver.FindElement(By.XPath("//div[@id='sessionEdit']/fieldset[5]/div/ul/li[text()='No']")).Click();
            
            driver.FindElement(By.Id("btnSave")).Click();
            
            driver.FindElement(By.Id("btnOk")).Click();

 
        }
         


        [Test]
        public void e_Add_Receiver()
        {
            string receiver_name = "receiver_smtp";
            string receiver_pin = "testm703@gmail.com";
            string receiver_description = "Receiver Description";
            string receiver_emailaddress = "email@address.com";
            string carrier_name = "smtp_carrier";
            string department_name = "Default";

            check_driver_type(driver_type, "recipients", "Receivers","Recipients");

            Assert.AreEqual("Receivers Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.XPath(".//div[@class='filter_panel']/a[text()='Add Reciever']")).Click();
            Thread.Sleep(3000);

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

            driver.FindElement(By.XPath("//div[@class='add_receiver_block']/div[1]/div[1]/div[1]/fieldset[4]/div/a[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'" + department_name + "')])")).Click();// selecting department

            driver.FindElement(By.XPath("//div[@class='add_receiver_block']/div/div[2]/div[3]/fieldset[1]/div/a[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//li[contains(text(),'"+ carrier_name +"')])")).Click();// selecting carrier

            driver.FindElement(By.Id("txtPrimaryPin")).Clear();
            driver.FindElement(By.Id("txtPrimaryPin")).SendKeys(receiver_pin);

            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);

            takescreenshot("Receiver");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(receiver_name) &&
               driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(receiver_description)))
            {
                Assert.Fail("Add Receiver Failed...");
            }

            else
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^  Add Receiver Passed ...  ^^^^^^^^^^^^^^^^^");
              //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }

        }

        [Test]
        public void Quick_Send_Panel()
        {
            string pin_number = "testm703@gmail.com";
            string carrier_name = "smtp_carrier";
            string quick_message = "test message";

            check_driver_type(driver_type, "send", "Quick Send","Send");

            Assert.AreEqual("Quick Send Panel", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text);

            driver.FindElement(By.XPath("//div[@class='data_row_col1']/input")).Clear();

            driver.FindElement(By.XPath("//div[@class='data_row_col1']/input")).SendKeys(pin_number);

            driver.FindElement(By.XPath("//div[@class='data_row_col2']/div/a[2]")).Click();
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

        }


        [Test]
        public void Primary_Send_Panel()
        {
            string receiver_name = "receiver_smtp";
            string primary_message = "Test Automation Message";
            string response_action_name = "Test Response Action";

            check_driver_type(driver_type, "send", "Primary Send","Send");

            Assert.AreEqual("Primary Send Panel", driver.FindElement(By.XPath("//span[@id='lblPanelTitle']")).Text);

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

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[1]/div/a[2]")).Click();// opening severity dropdown

            driver.FindElement(By.XPath("//li[text()='Important']")).Click();// selecting severity as Important

            driver.FindElement(By.XPath("//b[text()='Expire After']")).Click();// checking 'Expire After' checkbox

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[2]/div[2]/div/a[2]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//li[text()='02']")).Click();// selecting '02'

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[2]/div[3]/div/a[2]")).Click();// selecting Hours drop down

            driver.FindElement(By.XPath("//div[@id='advOptPanel']/div/fieldset[2]/div[3]/div/ul/li[3]")).Click();// selecting '02'

            driver.FindElement(By.XPath("//div[@id='response2wayPanel']/div[1]/b")).Click();// opening 2way Responses section

            driver.FindElement(By.Id("txtRespName")).Clear();

            driver.FindElement(By.Id("txtRespName")).SendKeys(response_action_name);// typing Response Action name

            driver.FindElement(By.Id("btnAddAction")).Click();

            driver.FindElement(By.Id("btnSend")).Click();
            Thread.Sleep(2000);
            
            if (driver.FindElement(By.XPath("//div[@class='popup_message']")).Displayed)
            {
                if (driver.FindElement(By.Id("statusMessage")).Text.Contains("Created a total of 1 separate message(s)"))
                {

                    takescreenshot("Primary_Send_Passed");
                    driver.FindElement(By.Id("btnToMessage")).Click();
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
                Assert.Fail(" Primary Send Panel Faled ... ");
            }
            

        }



        [Test]
        public void i_Timezone()
        {
            string timezone_name = "Karachi Timezone";
            string timezone_desc = "Karachi Timezone description";
            string offset = "5 Hours";
            string offset_digit = "5";
           
            check_driver_type(driver_type, "administration", "Timezone Settings","Sys Admin");

            Assert.AreEqual("Time Zone Setting", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Time Zone")).Click();

            driver.FindElement(By.Id("txtName")).Clear();

            driver.FindElement(By.Id("txtName")).SendKeys(timezone_name);
            
            driver.FindElement(By.Id("txtDesc")).Clear();
            
            driver.FindElement(By.Id("txtDesc")).SendKeys(timezone_desc);
            
            driver.FindElement(By.CssSelector("a.selector")).Click();
            Thread.Sleep(2000);

            string path1 = "//li[(text()='";
            string path2 = "')]";

            driver.FindElement(By.XPath(path1 + offset + path2)).Click();
            
            //driver.FindElement(By.XPath("//li[(text()='5 Hours')]")).Click();  -----Xpath as it is

            driver.FindElement(By.Id("btnSaveTimeBox")).Click();
            Thread.Sleep(2000);
            
            
         
            Console.WriteLine(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text);
            
            //driver.FindElement(By.XPath("//div[@title='Karachi Timezone']"))
            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(timezone_name) &&
                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(timezone_desc) &&
                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(offset_digit)))
            
            {
                takescreenshot("Timezone_Failed");
                Assert.Fail("Time zone Failed...");
            }

            else

            {
                takescreenshot("Timezone_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^^^  Add Time Zone Passed ...  ^^^^^^^^^^^^^^^^^");
                //driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }
        }


        [Test]
        public void System_Attendant_Settings()
        {
            string administrationemail = "fali@folio3.com";
            string numberofcompletemsgs = "20";
            string numberoffailedmsgs = "30";
            string numberoffilteredmsgs = "40";
            string idlemsgtime = "60";
            string alertcommand = "Test Alert Command";
            string expired = "Yes";
            string expired_status_on_page_load;
            string expired_status_after_saving;


            check_driver_type(driver_type, "administration", "System Attendent Settings","Sys Admin");

            Assert.AreEqual("System Attendant Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

               //Console.WriteLine("Text................."+driver.FindElement(By.XPath("//*[@id='lblAdminEmail']")).Text);

                expired_status_on_page_load=driver.FindElement(By.Id("lblDelExpired")).Text.ToString();

                Console.WriteLine(expired_status_on_page_load);

                driver.FindElement(By.Id("btnedit")).Click();
                Thread.Sleep(2000);
               
            driver.FindElement(By.Id("txtemail")).Clear();
            
            driver.FindElement(By.Id("txtemail")).SendKeys(administrationemail);
            
            driver.FindElement(By.Id("txtcompmsgs")).Clear();
            
            driver.FindElement(By.Id("txtcompmsgs")).SendKeys(numberofcompletemsgs);
            
            driver.FindElement(By.Id("txtfailedmsgs")).Clear();
            
            driver.FindElement(By.Id("txtfailedmsgs")).SendKeys(numberoffailedmsgs);
            
            driver.FindElement(By.Id("txtfilteredmsgs")).Clear();
            
            driver.FindElement(By.Id("txtfilteredmsgs")).SendKeys(numberoffilteredmsgs);
            
            driver.FindElement(By.Id("txtcommand")).Clear();
            
            driver.FindElement(By.Id("txtcommand")).SendKeys(alertcommand);
            
            driver.FindElement(By.Id("txtidlemsgs")).Clear();
            
            driver.FindElement(By.Id("txtidlemsgs")).SendKeys(idlemsgtime);
            
            driver.FindElement(By.XPath("//span[text()='Delete Expired Web Sign-up Recipients']")).Click();
 
            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(1000);
            
            expired_status_after_saving = driver.FindElement(By.Id("lblDelExpired")).Text.ToString();

            Console.WriteLine(expired_status_after_saving);
            Thread.Sleep(2000);
        
            if (driver.FindElement(By.XPath("//*[@id='lblAdminEmail']")).Text.Equals(administrationemail) &&

                    driver.FindElement(By.XPath("//*[@id='lblCompMsg']")).Text.Equals(numberofcompletemsgs) &&
                
                    driver.FindElement(By.XPath("//*[@id='lblFailedMsg']")).Text.Equals(numberoffailedmsgs) &&
                    
                    driver.FindElement(By.XPath("//*[@id='lblFilterMsg']")).Text.Equals(numberoffilteredmsgs) &&
                    
                    driver.FindElement(By.XPath("//*[@id='lblCommand']")).Text.Equals(alertcommand) &&
                    
                    driver.FindElement(By.XPath("//*[@id='lblIdleMsg']")).Text.Equals(idlemsgtime) &&
                    
                    driver.FindElement(By.XPath("//*[@id='lblDelExpired']")).Text.Equals(expired))

                {
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   System_Attendant_Settings Testcase Passed    ^^^^^^^^^^^^^^^^^^^^^");
                    //driver.FindElement(By.XPath("//*[@id='logout']")).Click();

                }

                else

                {
                    Assert.Fail("System_Attendant_Settings Testcase Failed");
                }

        }


        [Test]
        public void x_Backup_Settings_Panel()
        {
            string backup_dir = "C:\\Program Files (x86)\\Hiplink Software\\HipLink\\backup";
            string backup_keep_days = "05";
            string backup_start_time = "05:06";
            string backup_interval = "02";

            check_driver_type(driver_type, "administration", "Backup Service","Sys Admin");

            Assert.AreEqual("Backup Service", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

          /*  var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Backup Service']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Backup Service']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                hover_func("administration", "Backup Service");
            }*/
            

            
            driver.FindElement(By.Id("btnedit")).Click();
           
            driver.FindElement(By.Id("backup_dir")).Clear();
            
            driver.FindElement(By.Id("backup_dir")).SendKeys(backup_dir);

            driver.FindElement(By.Id("backup_keep_days")).Clear();
            
            driver.FindElement(By.Id("backup_keep_days")).SendKeys(backup_keep_days);

            driver.FindElement(By.Id("backup_start_time")).Click();
            
            driver.FindElement(By.Id("backup_start_time")).Clear();
            
            driver.FindElement(By.Id("backup_start_time")).SendKeys(backup_start_time);
                
            driver.FindElement(By.Id("backup_interval")).Clear();
            
            driver.FindElement(By.Id("backup_interval")).SendKeys(backup_interval);
            
            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//*[@id='tab_backupfiles']/a")).Click();
            
            driver.FindElement(By.XPath("//*[@id='tab_backupservice']/a")).Click();
            Thread.Sleep(2000);
            
            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblDirectory']")).Text + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblBackupDays']")).Text + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblStartTime']")).Text + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblBackupHours']")).Text + "*");

            Thread.Sleep(2000);
            
            if (driver.FindElement(By.XPath("//*[@id='lblDirectory']")).Text.Equals(backup_dir)  &&

                    driver.FindElement(By.XPath("//*[@id='lblBackupDays']")).Text.Contains("5") &&

                driver.FindElement(By.XPath("//*[@id='lblStartTime']")).Text.Contains("5:6") &&

                    driver.FindElement(By.XPath("//*[@id='lblBackupHours']")).Text.Contains("2"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Backup Settings Passsed...   ^^^^^^^^^^^^^^^^^^^^^");
               // driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                
            }
            
            else
            {
                Assert.Fail("Backup_settings_panel ---------- Testcase Failed2");
            }

        }


        [Test]
        public void Snpp_Gateway()
        {
            string port = "8080";
            string timeout= "10";
            string current_radiobtn_txt;
            string NoAuth = "No Authentication: Allow incoming connections from any IP address";
            string CLI_USER = "CLI User Authentication: Each incoming connection will be authenticated against a CLI user's IP address";

            check_driver_type(driver_type, "settings", "SNPP Gateway", "Settings");

            Assert.AreEqual("SNPP Gateway", driver.FindElement(By.XPath("//div[@id='main']/h1")).Text);


       /*     var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='SNPP Gateway']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='SNPP Gateway']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                hover_func("settings", "SNPP Gateway");
            }*/

            

            current_radiobtn_txt = driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.ToString();
          
            Console.WriteLine("*" + current_radiobtn_txt + "*");

            if (driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.Equals(NoAuth))
            {
                driver.FindElement(By.Id("btnedit")).Click();
            
                driver.FindElement(By.Id("txtport")).Clear();
            
                driver.FindElement(By.Id("txtport")).SendKeys(port);
            
                driver.FindElement(By.CssSelector("a.selector")).Click();
                Thread.Sleep(2000);
              
                string path1="//li[contains(text(),'";
                string path2="')]";
            
                driver.FindElement(By.XPath(path1+timeout+path2)).Click();
           
                Console.WriteLine("*"+driver.FindElement(By.XPath("//span[@class='custom radio checked']")).Text+"*");
                Thread.Sleep(2000);
    
                driver.FindElement(By.XPath("//span[@class='custom radio']")).Click();
            
                Console.WriteLine(driver.FindElement(By.XPath("//label[@class='radio_btn_box nopad_right']")).Text);
                Thread.Sleep(2000);
            
                driver.FindElement(By.Id("btnsave")).Click();
                Thread.Sleep(2000);
            
                driver.FindElement(By.Id("btncancel")).Click();
                Thread.Sleep(2000);

                if(driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.Equals(CLI_USER) && 

                    driver.FindElement(By.Id("txtport")).Text.Equals(port) &&

                    driver.FindElement(By.Id("lblTimeout")).Text.Equals(timeout))
                
                {
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Snpp Gateway Passsed...No Auth is changed to CLI User   ^^^^^^^^^^^^^^^^^^^^^");
                   // driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }

                else
                
                {
                    Assert.Fail("Failed...First No Auth was checked and not converted to CLI User");
                }
            
            }
            
            else if (driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.Equals(CLI_USER))
            
            {
                driver.FindElement(By.Id("btnedit")).Click();
                driver.FindElement(By.Id("txtport")).Clear();
                driver.FindElement(By.Id("txtport")).SendKeys(port);
                driver.FindElement(By.CssSelector("a.selector")).Click();
                Thread.Sleep(2000);
              //  driver.FindElement(By.XPath("//li[contains(text(),'30')]")).Click();
                
                string path1 = "//li[contains(text(),'";
                string path2 = "')]";

                driver.FindElement(By.XPath(path1 + timeout + path2)).Click();

                Console.WriteLine("*" + driver.FindElement(By.XPath("//span[@class='custom radio checked']")).Text + "*");
                Thread.Sleep(2000);
              
                driver.FindElement(By.XPath("//span[@class='custom radio']")).Click();

                Console.WriteLine(driver.FindElement(By.XPath("//label[@class='radio_btn_box nopad_right']")).Text);
                Thread.Sleep(2000);
                
                driver.FindElement(By.Id("btnsave")).Click();
                Thread.Sleep(2000);
                
                driver.FindElement(By.Id("btncancel")).Click();
                Thread.Sleep(2000);

                if (driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.Equals(NoAuth) &&

                    driver.FindElement(By.Id("txtport")).Text.Equals(port) &&

                    driver.FindElement(By.Id("lblTimeout")).Text.Equals(timeout))
                
                {
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Snpp Gateway Passsed...CLI User is changed to No Auth    ^^^^^^^^^^^^^^^^^^^^^");
                  //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }
                
                else
                
                {
                    Assert.Fail("Failed...First CLI user was checked and not converted to No Auth");
                }

            }

            else
            
            {
                Assert.Fail("Both options do no t exist");
            }

        }


        [Test]
        public void f_Directory_Settings_Panel()
        {
            string new_dir = "new_directory";
            string dir_path = @"C:\Program Files (x86)\Hiplink Software\HipLink\new_directory";

            check_driver_type(driver_type, "administration", "Directories & Queues","Sys Admin");

            Assert.AreEqual("Directories & Queues", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

         /*   var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Directories & Queues']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Directories & Queues']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                hover_func("administration", "Directories & Queues");
            }*/
            


            driver.FindElement(By.Id("btnAddQue")).Click();
            
            driver.FindElement(By.Id("txtqueName")).Clear();
            
            driver.FindElement(By.Id("txtqueName")).SendKeys(new_dir);
            
            driver.FindElement(By.Id("txtquePath")).Clear();
            
            driver.FindElement(By.Id("txtquePath")).SendKeys(dir_path);
            
            driver.FindElement(By.LinkText("OK")).Click();
            Thread.Sleep(2000);
            
            

            Console.WriteLine(driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text);

            if (!(driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(new_dir) &&

                driver.FindElement(By.XPath("//div[@id='lightGridDiv']")).Text.Contains(dir_path)))
            
            {
                takescreenshot("Directory_Failed");
                Assert.Fail("Add Directory Failed...");
            }

            else
            
            {
                takescreenshot("Directory_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add Directory Passed... ^^^^^^^^^^^^^^^^^^^^^");
               // driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                
            }

        //  driver.FindElement(By.Id("btnOk")).Click();
        //  driver.FindElement(By.CssSelector("a.close")).Click();

        }


        [Test]
        public void g_Add_Tap_Gateway()
        {
            string port = "1452";
            string Initial_String = "Initial String";
            string answer_string = "Auto Answer String";

            check_driver_type(driver_type, "settings", "TAP Gateway","Settings");

            Assert.AreEqual("TAP Gateway", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

       /*     var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='TAP Gateway']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='TAP Gateway']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                hover_func("settings", "TAP Gateway");
            }*/

            
            driver.FindElement(By.LinkText("Add TAP Gateway")).Click();
         
            driver.FindElement(By.Id("txtSerialPort")).Clear();
            
            driver.FindElement(By.Id("txtSerialPort")).SendKeys(port);
            
            driver.FindElement(By.CssSelector("a.selector")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//li[(contains(text(),'1200'))]")).Click();
            Thread.Sleep(1000);
            //driver.FindElement(By.XPath("//ul[@id='sizzle-1396510222684']/li[2]")).Click();
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[2]/fieldset[2]/div/a[2]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//li[(contains(text(),'Odd'))]")).Click();
            Thread.Sleep(1000);
            //driver.FindElement(By.XPath("//ul[@id='sizzle-1396510222684']/li[2]")).Click();
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[2]/fieldset[3]/div/a[2]")).Click();
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[2]/fieldset[3]/div/ul/li[2]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[2]/fieldset[4]/div/a[2]")).Click();
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[2]/fieldset[4]/div/ul/li[2]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[2]/fieldset[5]/div/a[2]")).Click();
            
            driver.FindElement(By.XPath("//li[(contains(text(),'Hardware'))]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[3]/fieldset[1]/div/a[2]")).Click();
            
            driver.FindElement(By.XPath("//li[(contains(text(),'2 sec'))]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//div[@id='tapGatewayLightbox']/div[2]/div[3]/fieldset[2]/div/a[2]")).Click();
            
            driver.FindElement(By.XPath("//li[(contains(text(),'30 minutes'))]")).Click();

            driver.FindElement(By.XPath("//span[contains(text(),'Leased Line')]")).Click();

         //   Console.WriteLine(driver.FindElement(By.XPath("//span[contains(text(),'Leased Line')]")));
         //   driver.FindElement(By.XPath("//input[(@id='chkLeasedLine')]")).Click();
         //   driver.FindElement(By.CssSelector("input[id='chkLeasedLine']")).Click();
            Thread.Sleep(1000);
            Console.WriteLine(driver.FindElement(By.Id("txtIntString")).Enabled);

            if (driver.FindElement(By.Id("txtIntString")).Enabled==false)
            
            {
                driver.FindElement(By.XPath("//span[contains(text(),'Leased Line')]")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.Id("txtIntString")).Clear();
            
                driver.FindElement(By.Id("txtIntString")).SendKeys("Initial String");
            
                driver.FindElement(By.Id("txtAutoAnswer")).Clear();
            
                driver.FindElement(By.Id("txtAutoAnswer")).SendKeys("Auto Answer String");
            
                driver.FindElement(By.Id("btnsave")).Click();
                Thread.Sleep(2000);
            
                takescreenshot("TAP Gateway");
            }

            Thread.Sleep(1000);

            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(port) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(Initial_String) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(answer_string) + "*");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(port) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(Initial_String) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(answer_string)))
            
            {
                Assert.Fail("TAP Gateway Failed...");
            }

            else
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   TAP Gateway Passed...   ^^^^^^^^^^^^^^^^^^^^^");
              //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }

        }


        [Test]
        public void a_Add_User()
        {
            string username = "fahad1";
            string userdescription = "user description";
            string email = "b@folio3.com";
            string access_code = "1228";
            string user_group = "sysAdmin";
            string status= "Enabled";
            
            check_driver_type(driver_type, "settings", "Users","Settings");

            Assert.AreEqual("Users", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

       /*     var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Users']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Users']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                hover_func("settings", "Users");
            }*/

           
            driver.FindElement(By.LinkText("Add User")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("txtName")).Clear();
            
            driver.FindElement(By.Id("txtName")).SendKeys(username);
            
            driver.FindElement(By.Id("txtEmail")).Clear();
            
            driver.FindElement(By.Id("txtEmail")).SendKeys(email);
            
            driver.FindElement(By.XPath("//span[text()='Result to User Email']")).Click();
            
            driver.FindElement(By.CssSelector("a.selector")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//li[text()='Server Time']")).Click();
            
            driver.FindElement(By.Id("txtAccess_code")).Clear();
            
            driver.FindElement(By.Id("txtAccess_code")).SendKeys(access_code);
            
            driver.FindElement(By.Id("txtDesc")).Clear();
            
            driver.FindElement(By.Id("txtDesc")).SendKeys(userdescription);
            Thread.Sleep(1000);
            
     //       driver.FindElement(By.XPath("//div[@class='user_group_col1']/div/div/fieldset[1]/div/a[2]")).Click();
     //       Thread.Sleep(1000);
            
     //       driver.FindElement(By.XPath("//li[text()='Hiplink']")).Click();
     //     driver.FindElement(By.XPath("//form[@id='userPanel']/div[2]/div/div/div/fieldset/div/a[2]")).Click();
            
            driver.FindElement(By.Id("txtpassword")).Clear();
            
            driver.FindElement(By.Id("txtpassword")).SendKeys("123");
            
            driver.FindElement(By.Id("txtRetypePass")).Clear();
            
            driver.FindElement(By.Id("txtRetypePass")).SendKeys("123");
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//form[@id='userPanel']/div[2]/div/div/div/fieldset[4]/div/a[2]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//li[text()='GUI']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//div[@class='user_group_col1']/div/div[2]/fieldset/div/a[2]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//li[text()='"+user_group+"']")).Click();
            Thread.Sleep(1000);
      //    driver.FindElement(By.Id("txtIp")).Clear();
      //    driver.FindElement(By.Id("txtIp")).SendKeys("10.0.0.40");
      
            driver.FindElement(By.Id("btnsave")).Click();
            
            
            Thread.Sleep(3000);
            takescreenshot("User");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            IWebElement myDynamicElement = wait.Until<IWebElement>(d => driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")));
            //wait.Until(driver => driver.FindElement(searchBy));
            
            
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(userdescription) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(email) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(status) + "*");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(userdescription) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(email)))
            
            {
                takescreenshot("User_Failed");
                Assert.Fail("Add User Failed...");
            }
            
            else 
            
            {
                takescreenshot("User_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add User Passed...   ^^^^^^^^^^^^^^^^^^^^^");
              //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                Thread.Sleep(2000);
            }

        }


        [Test]
        public void b_Add_User_Group()
        {
            string user_group = "ug2";
            string user_group_desc = "Description";

            Thread.Sleep(2000);
            
            check_driver_type(driver_type, "settings", "User Groups- Permissioning","Settings");

            Assert.AreEqual("User Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);
            
            Thread.Sleep(2000);


        /*    var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='User Groups- Permissioning']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='User Groups- Permissioning']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                hover_func("settings", "User Groups- Permissioning");
            }*/

            driver.FindElement(By.LinkText("Add User Group")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("txtname")).Clear();
            
            driver.FindElement(By.Id("txtname")).SendKeys(user_group);
            
            driver.FindElement(By.CssSelector("a.selector")).Click();
            
            driver.FindElement(By.XPath("//li[text()='Reports Menu']")).Click();
            
            driver.FindElement(By.Id("txtdesc")).Clear();
            
            driver.FindElement(By.Id("txtdesc")).SendKeys(user_group + " "+user_group_desc);
            
            driver.FindElement(By.Id("btnAddUserGroup")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[2]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[3]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[4]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[5]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[6]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[7]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[8]/label")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.LinkText("System")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//table[@id='systemTable']/tbody/tr[12]/td[1]/label/span")).Click();//*[@id='systemTable']/tbody/tr[12]/td[1]/label/span
            Thread.Sleep(1000);
            
            driver.FindElement(By.CssSelector("#tab_send > a")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//table[@id='sendTable']/tbody/tr[8]/td[1]/label/span")).Click();//*[@id='sendTable']/tbody/tr[8]/td[1]/label/span
            Thread.Sleep(1000);
            
            driver.FindElement(By.LinkText("User Group")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[2]/label/span")).Click();//*[@id='ug_grid']/tbody/tr[4]/td[2]/label/span
            
            driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[3]/label/span")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.LinkText("Response Action")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//table[@id='responseTable']/tbody/tr[8]/td[2]/label/span")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);
            
            takescreenshot("User group");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(user_group) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(user_group + " " + user_group_desc)))
            
            {
                takescreenshot("User_Group_Failed");
                Assert.Fail("Add User Group Failed...");
            }

            else
            
            {
                takescreenshot("User_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add User Group Passed...    ^^^^^^^^^^^^^^^^^^^^^");
               // driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }

        }


        [Test]
        public void User_Session()
        {
            
            check_driver_type(driver_type, "administration", "Sessions Manager","Sys Admin");

            Assert.AreEqual("Sessions Manager", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

       /*     var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Sessions Manager']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Sessions Manager']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                hover_func("administration", "Sessions Manager");
            }*/

            Console.WriteLine(driver.FindElement(By.XPath("//div[@class='tab_block tab_session tab_active']")).Text);

            if (!(driver.FindElement(By.XPath("//div[@class='tab_block tab_session tab_active']")).Text.Contains(login_name) &&

                driver.FindElement(By.XPath("//div[@class='tab_block tab_session tab_active']")).Text.Contains(browser)))
            
            {
                Assert.Fail("Session Manager Failed ...");
            }

            else
            
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Session Manager Passed ...    ^^^^^^^^^^^^^^^^^^^^^");
              //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }
 
        }



        [Test]
        public void j_File_System_Interface()
        {
            string hiplink_url = "info@folio3.com";
            string spool_dir = @"C:\Program Files (x86)\HipLink Software\Hiplink\test";
            string bulk_spool_dir = @"C:\Program Files (x86)\HipLink Software\Hiplink\new";
            string bulk_message_recipient_pattern = "^.*<Receiver:(.*)>.*$";
            string bulk_message_pattern = "^.*>(.*)$";
            string bulk_message_file_pattern = "*";

            check_driver_type(driver_type, "settings", "File System Interface","Settings");

            Assert.AreEqual("File System Interface", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);


        /*    var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='File System Interface']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='File System Interface']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                hover_func("settings", "File System Interface");
            }*/
          
            
            Thread.Sleep(2000);

            if (driver.FindElement(By.Id("lblBulkMsg")).Text.Equals("Enabled"))
            
            {
                driver.FindElement(By.Id("btnedit")).Click();
                Thread.Sleep(2000);
            
                driver.FindElement(By.Id("txturl")).Clear();
                
                driver.FindElement(By.Id("txturl")).SendKeys(hiplink_url);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtdir")).Clear();
                
                driver.FindElement(By.Id("txtdir")).SendKeys(spool_dir);
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[1]/div[1]/fieldset[2]/div/a[2]")).Click();
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//li[text()='35']")).Click();
                Thread.Sleep(1000);
           //   driver.FindElement(By.XPath("//span[text()='Enable Bulk Message Processing']")).Click();  // checkbox already checked

                driver.FindElement(By.Id("txtspooldir")).Clear();
                
                driver.FindElement(By.Id("txtspooldir")).SendKeys(bulk_spool_dir);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtmsgfilepat")).Clear();
                
                driver.FindElement(By.Id("txtmsgfilepat")).SendKeys(bulk_message_file_pattern);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtmsgrec")).Clear();
                
                driver.FindElement(By.Id("txtmsgrec")).SendKeys(bulk_message_recipient_pattern);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtbulkmsgpat")).Clear();
                
                driver.FindElement(By.Id("txtbulkmsgpat")).SendKeys(bulk_message_pattern);
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[2]/div/fieldset[4]/div/a[2]")).Click();
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[2]/div[1]/fieldset[4]/div/ul/li[17]")).Click();
                
                string lblBulkSpoolDirChk = driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[2]/div[1]/fieldset[4]/div/ul/li[17]")).Text;
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("btnsave")).Click();
                Thread.Sleep(2000);
                
                takescreenshot("FSI");
                
                Console.WriteLine("Enable Bulk Message Processing was checked");

                Console.WriteLine(driver.FindElement(By.XPath(".//*[@id='lblUrl']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblSpoolDir']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblDirChk']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsg']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblBulkSpoolDir']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblBullFilePatt']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgRec']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgPatt']")).Text +
                    driver.FindElement(By.XPath(".//*[@id='lblBulkSpoolDirChk']")).Text);

                if (!(driver.FindElement(By.XPath(".//*[@id='lblUrl']")).Text.Equals(hiplink_url) &&

                    driver.FindElement(By.XPath(".//*[@id='lblSpoolDir']")).Text.Equals(spool_dir) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblDirChk']")).Text.Equals("35") &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsg']")).Text.Equals("Enabled") &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkSpoolDir']")).Text.Equals(bulk_spool_dir) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBullFilePatt']")).Text.Equals(bulk_message_file_pattern) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgRec']")).Text.Equals(bulk_message_recipient_pattern) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgPatt']")).Text.Equals(bulk_message_pattern)))
                
                {
                    Assert.Fail("File System Interface Failed ...");
                }

                else
                {
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   File System Interface Passed ...when Enable Bulk Message Processing was checked  ^^^^^^^^^^^^^^^^^^^^^");
                  //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }

            }

            else
            
            {

                driver.FindElement(By.Id("btnedit")).Click();
                Thread.Sleep(2000);
            
                driver.FindElement(By.Id("txturl")).Clear();
                
                driver.FindElement(By.Id("txturl")).SendKeys(hiplink_url);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtdir")).Clear();
                
                driver.FindElement(By.Id("txtdir")).SendKeys(spool_dir);
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[1]/div[1]/fieldset[2]/div/a[2]")).Click();
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//li[text()='35']")).Click();
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//span[text()='Enable Bulk Message Processing']")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.Id("txtspooldir")).Clear();
                
                driver.FindElement(By.Id("txtspooldir")).SendKeys(bulk_spool_dir);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtmsgfilepat")).Clear();
                
                driver.FindElement(By.Id("txtmsgfilepat")).SendKeys(bulk_message_file_pattern);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtmsgrec")).Clear();
                
                driver.FindElement(By.Id("txtmsgrec")).SendKeys(bulk_message_recipient_pattern);
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("txtbulkmsgpat")).Clear();
                
                driver.FindElement(By.Id("txtbulkmsgpat")).SendKeys(bulk_message_pattern);
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[2]/div/fieldset[4]/div/a[2]")).Click();
                Thread.Sleep(1000);
                
                driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[2]/div[1]/fieldset[4]/div/ul/li[17]")).Click();
                
                string lblBulkSpoolDirChk = driver.FindElement(By.XPath("//*[@id='fileSysPanel']/div[2]/div[1]/fieldset[4]/div/ul/li[17]")).Text;
                Thread.Sleep(1000);
                
                driver.FindElement(By.Id("btnsave")).Click();
                
                takescreenshot("FSI");
                Thread.Sleep(2000);
                
                Console.WriteLine("Enable Bulk Message Processing was unchecked");
            
                if (!(driver.FindElement(By.XPath(".//*[@id='lblUrl']")).Text.Equals(hiplink_url) &&

                    driver.FindElement(By.XPath(".//*[@id='lblSpoolDir']")).Text.Equals(spool_dir) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblDirChk']")).Text.Equals("35") &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsg']")).Text.Equals("Enabled") &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkSpoolDir']")).Text.Equals(bulk_spool_dir) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBullFilePatt']")).Text.Equals(bulk_message_file_pattern) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgRec']")).Text.Equals(bulk_message_recipient_pattern) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgPatt']")).Text.Equals(bulk_message_pattern) &&
                    
                    driver.FindElement(By.XPath(".//*[@id='lblBulkSpoolDirChk']")).Text.Equals(lblBulkSpoolDirChk)))
                
                {
                    Assert.Fail("File System Interface Failed ...");
                }

                else
                
                {
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   File System Interface Passed ...when Enable Bulk Message Processing was unchecked   ^^^^^^^^^^^^^^^^^^^^^");
                   // driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }

            }
            
        }


        [Test]
        public void c_Add_Messenger()
        {
            string messenger_name = "smtp_messenger";
            string new_dir = "new_directory";
            string messenger_desc = "SMTP Messenger Description";

            check_driver_type(driver_type, "administration", "Messengers","Sys Admin");

            Assert.AreEqual("Messengers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

    /*        var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Messengers']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Messengers']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                hover_func("administration", "Messengers");
            }*/

               
                
               //-------- Select Messenger Protocol Type ---------

            IWebElement element = driver.FindElement(By.XPath("//div[@class='messenger_protocol_category_list_wrapper']/div"));
                IList<IWebElement> messenger_protocol_category_List = element.FindElements(By.XPath("//ul[@id='columnOne']/li"));

                Console.WriteLine("number of protocol category" + " " +messenger_protocol_category_List.Count);

                int DpListCount = messenger_protocol_category_List.Count;
                for (int i = 0; i < DpListCount; i++)
                {
                    if (messenger_protocol_category_List[i].Text == "Email")
                    {
                        Console.WriteLine("index where protocol category matched"+" "+i);
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
                        Console.WriteLine("Type name:"+" "+messenger_protocol_category_type[i].Text);
                        messenger_protocol_category_type[i].Click();
                    }
                }
               
             
                //----------xxxxxxx----------
                                                                              
                          
                        driver.FindElement(By.Id("btnMsngr")).Click();
                        Thread.Sleep(2000);
                        driver.FindElement(By.Id("txtMessangerName")).Clear();
                        driver.FindElement(By.Id("txtMessangerName")).SendKeys(messenger_name);


                //----------- Selecting Paging Queue ----------

                    /*    driver.FindElement(By.CssSelector("a.selector")).Click();
                        SelectElement select_msngr_Queue = new SelectElement(driver.FindElement(By.Id("selMessangerQueue"))); // Creating SelectElement.
                        select_msngr_Queue.SelectByText("new_directory");*/

                        driver.FindElement(By.CssSelector("a.selector")).Click();
                        driver.FindElement(By.CssSelector("a.selector")).Click();
                        Thread.Sleep(1000);
           /*             string path1 = "//li[text()='";
                        string path2 = "']";

                        driver.FindElement(By.XPath(path1 + new_dir + path2)).Click();*/

                     //   driver.FindElement(By.XPath("//li[text()='new_directory']")).Click();


                //----------- xxxxxxxxxxxxxxxxxxxxxx ----------

                   
                        driver.FindElement(By.Id("txtMessangerDescription")).Clear();
                        driver.FindElement(By.Id("txtMessangerDescription")).SendKeys(messenger_desc);

                        //----------- Selecting Paging Queue checking period ----------

                     
                  /*     SelectElement select_Queue_checking_period = new SelectElement(driver.FindElement(By.Id("selQueCheckingPeriod"))); // Creating SelectElement.
                       select_Queue_checking_period.SelectByValue("5");*/
                  
            
                        driver.FindElement(By.XPath(".//*[@id='protocolParameters']/div/div[2]/fieldset/div/a[2]")).Click();
                        Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//li[text()='5']")).Click();

                        //----------- xxxxxxxxxxxxxxxxxxxxxx ----------
                       
                      
                        driver.FindElement(By.Id("btnSaveMsngr")).Click();
                        Thread.Sleep(2000);
                        takescreenshot("Messenger");

      
                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(messenger_name) &&

                            driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains("SMTP") &&
                            
                            driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains("Default")))
                        
                        {
                            takescreenshot("Messenger_Failed");
                            Assert.Fail("Added messenger is failed ...");
                        }

                        else
                        
                        {
                            takescreenshot("Messenger_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Added messenger Passed ...   ^^^^^^^^^^^^^^^^^^^^^");
                          //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                        }
                
        }


        [Test]
        public void d_Add_Carrier()
        {

            string carrier_name = "smtp_carrier";
            string queue = "Default";
            string carrier_desc = "SMTP Carrier Description";
            string email_server = "smtp.gmail.com";
            string user_name = "hiplink@gmail.com";
            string user_pwd = "click+123";
            string email_suffix = "Email Suffix";
            string email_prefix = "Email Prefix";
            string email_subject = "Email Subject";


            check_driver_type(driver_type, "administration", "Carriers","Sys Admin");

            Assert.AreEqual("Carriers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

        /*    var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Carriers']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Carriers']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                hover_func("administration", "Carriers");
            }*/

           

            //-------- Select Messenger Protocol Type ---------

            IWebElement elementc = driver.FindElement(By.XPath("//div[@class='messenger_protocol_category_list_wrapper']/div"));
            IList<IWebElement> carrier_protocol_category_List = elementc.FindElements(By.XPath("//ul[@id='columnOne']/li"));

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
            //Console.WriteLine(DpListCount1);

            //----------xxxxxxx----------


            driver.FindElement(By.Id("btnaddcarrier")).Click();
            Thread.Sleep(2000);


            driver.FindElement(By.Id("carrierName")).Clear();
            driver.FindElement(By.Id("carrierName")).SendKeys(carrier_name);
            Thread.Sleep(2000);
            //----------- Selecting Paging Queue ----------

            //  html.js body div.wrapper div.middle_area div div.main_container form.add_carrier div.c_form_inner div.col_form fieldset div.custom ul li.selected
            driver.FindElement(By.CssSelector("a.selector")).Click();
            Thread.Sleep(1000);
            string path1 = "//li[text()='";
            string path2 = "']";

            driver.FindElement(By.XPath(path1 + queue + path2)).Click();
            Thread.Sleep(2000);

            //----------- xxxxxxxxxxxxxxxxxxxxxx ----------

            driver.FindElement(By.Id("carrierDescription")).Clear();
          
            driver.FindElement(By.Id("carrierDescription")).SendKeys(carrier_desc);

            driver.FindElement(By.XPath("//span[text()='Check for Automatic Carrier Updates']")).Click();

            driver.FindElement(By.XPath("//span[text()='Use Global Settings Email Server']")).Click();
            
            driver.FindElement(By.XPath("//span[text()='Use Global Settings Email Server']")).Click();
            
            driver.FindElement(By.Id("txtemailServer")).Clear();
            
            driver.FindElement(By.Id("txtemailServer")).SendKeys(email_server);
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//div[@id='protocolParameters']/div/div[2]/fieldset[2]/div/a[2]")).Click();
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

  /*          driver.FindElement(By.XPath("//span[text()='Split Long Message into Parts']")).Click();
            driver.FindElement(By.Id("txtsplitLongMsg")).Clear();
            driver.FindElement(By.Id("txtsplitLongMsg")).SendKeys("11");
            driver.FindElement(By.XPath("//form[@class='add_carrier c_form_wrapper custom']/div[3]/div[1]/fieldset[3]/div/a[2]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//li[text()='At the end']")).Click();

            driver.FindElement(By.XPath("//form[@class='add_carrier c_form_wrapper custom']/div[3]/div[2]/fieldset[1]/div/a[2]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//li[text()='2']")).Click();*/

            



            /*     driver.FindElement(By.XPath("html/body/div/div[3]/div/div/form/div[3]/div/fieldset[2]/label/span")).Click();
                             driver.FindElement(By.Id("txtsplitLongMsg")).Clear();
                             driver.FindElement(By.Id("txtsplitLongMsg")).SendKeys("11");
                           //  html.js body div.wrapper div.middle_area div div.main_container form.add_carrier div.c_form_inner div.col_form fieldset div.custom a.selector

                             driver.FindElement(By.XPath("/html/body/div/div[3]/div/div/form/div[3]/div/fieldset[3]/div/a[2]")).Click();
                                 Thread.Sleep(2000);
                            // /html/body/div/div[3]/div/div/form/div[3]/div/fieldset[3]/div/a[2]
                                 driver.FindElement(By.XPath("/html/body/div/div[3]/div/div/form/div[3]/div/fieldset[3]/div/ul/li[2]")).Click();
                            // driver.FindElement(By.XPath("//ul[@id='sizzle-1394732679492']/li[2]")).Click();
                            // driver.FindElement(By.XPath("(//a[contains(@href, '#')])[104]")).Click();
                            // driver.FindElement(By.CssSelector("#sizzle-1394732679492 > li")).Click();
                             driver.FindElement(By.CssSelector("div.c_form_inner > div.col_form > fieldset > div.custom.dropdown > a.selector")).Click();
                             // /html/body/div/div[3]/div/div/form/div[3]/div/fieldset[3]/div/a[2]
                             //html/body/div/div[3]/div/div/form/div[3]/div[2]/fieldset/div/a[2]
                             driver.FindElement(By.XPath("//fieldset/div/a[2]")).Click();
          
                             */


            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);
         
            takescreenshot("Carrier");
           
            Thread.Sleep(3000);
            Console.WriteLine("*" + driver.FindElement(By.Id("divGrid_idGridDataNode")).Text.Contains(carrier_name) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(carrier_desc) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(queue) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text + "*");

            
            if (!(driver.FindElement(By.Id("divGrid_idGridDataNode")).Text.Contains(carrier_name) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(carrier_desc)))
            
            {
                takescreenshot("Carrier_Failed");
                Assert.Fail("Added Carrier is failed ...");
            }

            else
            
            {
                takescreenshot("Carrier_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Added Carrier Passed ...   ^^^^^^^^^^^^^^^^^^^^^");
              //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }
              
        }


        [Test]
        public void Help()
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

                        Console.WriteLine("^^^^^^^^^^^^^^^  Help Passed ... ^^^^^^^^^^^^^^^^");
                    }

                    else
                  
                    {
                        Assert.Fail("Help Failed ...");
                    }


                    /*  driver.FindElement(By.XPath("//span[text()='Installation and Administration Guide']")).Click();
                        driver.FindElement(By.XPath("//span[text()='User Guide']")).Click();
                        driver.FindElement(By.XPath("//span[text()='Programmer Guide']")).Click();
                        driver.FindElement(By.LinkText("HLSales@hiplink.com")).Click();*/

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
          
            

            

            // Do some operations in the new window and close it 
            driver.Close(); 

            // Switch "focus" back to the original window. 
         //   driver.SwitchTo().Window(originalHandle);

     //-------------------------------------------------------------------      

      

        }


        [Test]
        public void k_Add_Department()
        {

            string user_group = "ug2";

            check_driver_type(driver_type, "settings", "Departments","Settings");

            Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

          /*  var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Departments']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Departments']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                hover_func("settings", "Departments");
            }*/
   
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Add Department")).Click();

            Thread.Sleep(1000);

            driver.FindElement(By.Id("txtname")).Clear();
            
            driver.FindElement(By.Id("txtname")).SendKeys("new_dpt");
            
            driver.FindElement(By.Id("txtdesc")).Clear();
            
            driver.FindElement(By.Id("txtdesc")).SendKeys("department description...");
        //  driver.FindElement(By.XPath("//a[text()='Permission']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@class='add_user_department_add']/div/a[2]")).Click();

            string path1 = "//li[text()='";
            string path2 = "']";

            driver.FindElement(By.XPath( path1+user_group+path2 )).Click();
            
            driver.FindElement(By.Id("btnUsergroup")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[2]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[3]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[4]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[5]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[6]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[7]/label")).Click();
            
            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[8]/label")).Click();

            driver.FindElement(By.LinkText("Member")).Click();
            
            driver.FindElement(By.XPath("//div[@id='memberTab']/div/div/fieldset/div/a[2]")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("moveMemberRight")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("moveMemberLeft")).Click();
            Thread.Sleep(2000);

          //  driver.FindElement(By.CssSelector("fieldset > div.custom.dropdown > a.selector")).Click();
            

            driver.FindElement(By.LinkText("Guest")).Click();
            
            driver.FindElement(By.XPath("//div[@id='guestTab']/div/div/fieldset/div/a[2]")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("moveMemberRight")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("moveMemberLeft")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("btnsave")).Click();
            
            driver.FindElement(By.Id("btncancel")).Click();

        }



        [Test]
        public void w_Email_Gateway_Settings()
        {
            string type = "SMTP";
            string hiplink_url = @"http://192.168.4.237:8000/cgi-bin/no_action.exe";
            string email_spool_directory = @"C:\Program Files (x86)\HipLink Software\HipLink\test_email_spool";
            string server_ip_address = "192.168.5.184";
            string server_port = "1337";
            string path_external_script = "/test/hiplink/5.0";
            string one_way_email = "scenario1@email.com";
            string two_way_email = "two-way@email.com";
            string pop_server = "test_pop";
            string pop_port = "8080";
            string pop_one_acc = "popone@account.com";
            string pop_one_pwd = "123";
            string pop_two_acc = "poptwo@account.com";
            string pop_two_pwd = "123";
            string standard_send_pattern="p1";

    /*        //------ Hover functionality and click ------

            var phonec = driver.FindElement(By.Id("settings"));
            var phoneLic = phonec.FindElements(By.ClassName("integrations"));
            Actions actionc = new Actions(driver);//simply my webdriver
            actionc.MoveToElement(phonec).Perform();//move to list element that needs to be hovered
            driver.FindElement(By.LinkText("Email Gateway")).Click();
            Thread.Sleep(3000);

            //------ Focus out the mouse to disappear hovered dialog ------

            Actions a1c = new Actions(driver);
            //  a.MoveByOffset(0, 0);
            a1c.MoveToElement(driver.FindElement(By.ClassName("top_bar"))).Perform();
            Thread.Sleep(2000);*/

            check_driver_type(driver_type, "settings", "Email Gateway","Settings");

            Assert.AreEqual("Email Gateway", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

        /*    var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Email Gateway']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Email Gateway']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
                hover_func("settings", "Email Gateway");
            }*/


           

            driver.FindElement(By.Id("btnedit")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//form[@id='emailPanel']/div/div[2]/div/fieldset/div/a[2]")).Click();
            driver.FindElement(By.XPath("//li[text()='SMTP']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("txturl")).Clear();
            driver.FindElement(By.Id("txturl")).SendKeys(hiplink_url);

            driver.FindElement(By.Id("txtspooldir")).Clear();
            driver.FindElement(By.Id("txtspooldir")).SendKeys(email_spool_directory);

            driver.FindElement(By.XPath("//form[@id='emailPanel']/div/div[2]/div[2]/fieldset[2]/div/a[2]")).Click();

            driver.FindElement(By.XPath("//span[text()='Preserve sender email in email messages']")).Click();
            driver.FindElement(By.XPath("//span[text()='Send delivery report to the sender']")).Click();

            driver.FindElement(By.Id("txtserverip")).Clear();
            driver.FindElement(By.Id("txtserverip")).SendKeys(server_ip_address);

            driver.FindElement(By.Id("txtserverport")).Clear();
            driver.FindElement(By.Id("txtserverport")).SendKeys(server_port);

            driver.FindElement(By.XPath("//span[text()='Preserve Email Subject']")).Click();
            driver.FindElement(By.XPath("//span[text()='Include the Subject in Message Body']")).Click();

            driver.FindElement(By.Id("txtextscript")).Clear();
            driver.FindElement(By.Id("txtextscript")).SendKeys(path_external_script);

            driver.FindElement(By.Id("txtSmtpOneEmail")).Clear();
            driver.FindElement(By.Id("txtSmtpOneEmail")).SendKeys(one_way_email);

            driver.FindElement(By.Id("txtSmtpTwoEmail")).Clear();
            driver.FindElement(By.Id("txtSmtpTwoEmail")).SendKeys(two_way_email);
            Thread.Sleep(1000);

            driver.FindElement(By.LinkText("POP")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("txtpopserver")).Clear();
            driver.FindElement(By.Id("txtpopserver")).SendKeys(pop_server);

            driver.FindElement(By.Id("txtpopserverport")).Clear();
            driver.FindElement(By.Id("txtpopserverport")).SendKeys(pop_port);

            driver.FindElement(By.XPath("//div[@id='POP']/div/div[2]/div[1]/fieldset[2]/div/a[2]")).Click();
            Thread.Sleep(2000);
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='POP']/div/div[2]/div[1]/fieldset[2]/div/ul")).Text + "*");
   //         driver.FindElement(By.XPath("//li[text()='31')]")).Click();
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='POP']/div/div[2]/div[1]/fieldset[2]/div/ul/li")).Text + "*");

            driver.FindElement(By.Id("txtPopOneAccount")).Clear();
            driver.FindElement(By.Id("txtPopOneAccount")).SendKeys(pop_one_acc);

            driver.FindElement(By.Id("txtPopOnePassword")).Clear();
            driver.FindElement(By.Id("txtPopOnePassword")).SendKeys(pop_one_pwd);

            driver.FindElement(By.Id("txtPopTwoAccount")).Clear();
            driver.FindElement(By.Id("txtPopTwoAccount")).SendKeys(pop_two_acc);

            driver.FindElement(By.Id("txtPoptwoPassword")).Clear();
            driver.FindElement(By.Id("txtPoptwoPassword")).SendKeys(pop_two_pwd);

            driver.FindElement(By.XPath("//div[@id='stdSendPat']/div/ul/li[text()='Add New']")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@class='viewport c_list_scroll_height']/div/div/fieldset/input")).Click();
            driver.FindElement(By.XPath("//div[@class='viewport c_list_scroll_height']/div/div/fieldset/input")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@class='viewport c_list_scroll_height']/div/div/fieldset/input")).SendKeys(standard_send_pattern);
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//div[@id='pin']/div/ul/li[text()='Add New']")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='pin']/div/div[2]/div/div[2]/div/div/fieldset/div[1]/input")).Click();
            driver.FindElement(By.XPath("//*[@id='pin']/div/div[2]/div/div[2]/div/div/fieldset/div[1]/input")).Clear();
            driver.FindElement(By.XPath("//*[@id='pin']/div/div[2]/div/div[2]/div/div/fieldset/div[1]/input")).SendKeys("abc@abc.com");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='pin']/div/div[2]/div/div[2]/div/div/fieldset/div[2]/input")).Click();
            driver.FindElement(By.XPath("//*[@id='pin']/div/div[2]/div/div[2]/div/div/fieldset/div[2]/input")).Clear();
            driver.FindElement(By.XPath("//*[@id='pin']/div/div[2]/div/div[2]/div/div/fieldset/div[2]/input")).SendKeys("SMTP_Carrier");
         
            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);
            takescreenshot("Email Gateway");

            Console.WriteLine("*" + driver.FindElement(By.Id("spanClientType")).Text + "*"); //type
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[2]/fieldset[1]/span")).Text + "*");//hiplink url
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[1]/fieldset[2]/span")).Text + "*");//email spool directory
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[2]/fieldset[2]/span")).Text + "*");//spool directory check period
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[1]/fieldset[3]/span")).Text + "*");//preserve sender email in email messages
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[2]/fieldset[3]/span")).Text + "*");//send delivery report to the sender

            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[1]/fieldset[1]/span")).Text + "*");//server ip address
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[2]/fieldset[1]/span")).Text + "*");//server port
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[1]/fieldset[2]/span")).Text + "*");//preserve email subject 
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[2]/fieldset[2]/span")).Text + "*");//include the subject
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[1]/fieldset[3]/span")).Text + "*");//path of external script

            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[3]/fieldset/span[2]")).Text + "*");//smtp one way email
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[5]/fieldset/span[2]")).Text + "*");//smtp two way email

            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[4]/div/div[2]/fieldset[1]/span")).Text + "*");//standard send pattern

            driver.FindElement(By.LinkText("POP")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[1]/fieldset[1]/span")).Text + "*");//pop3 server
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[2]/fieldset/span")).Text + "*");//pop3 server port
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[1]/fieldset[2]/span")).Text + "*");//pop3 server pull interval

            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[3]/fieldset/span")).Text + "*");//pop one way email
            Console.WriteLine("*" + driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[4]/fieldset/span")).Text + "*");//pop two way email

            driver.FindElement(By.LinkText("SMTP")).Click();
            Thread.Sleep(2000);

            if (driver.FindElement(By.Id("spanClientType")).Text.Contains(type) &&
                driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[2]/fieldset[1]/span")).Text.Contains(hiplink_url) &&
                driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[1]/fieldset[2]/span")).Text.Contains(email_spool_directory) &&
                driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[1]/fieldset[3]/span")).Text.Contains("true") &&
                driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[1]/div[2]/fieldset[3]/span")).Text.Contains("true") &&
                driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[1]/fieldset[1]/span")).Text.Contains(server_ip_address) &&
                driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[2]/fieldset[1]/span")).Text.Contains(server_port) &&
                driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[1]/fieldset[2]/span")).Text.Contains("true") &&
                driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[2]/fieldset[2]/span")).Text.Contains("true") &&
                driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[1]/fieldset[3]/span")).Text.Contains(path_external_script) &&
                driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[3]/fieldset/span[2]")).Text.Contains(one_way_email) &&
                driver.FindElement(By.XPath(".//*[@id='SMTP']/div/div[1]/div[5]/fieldset/span[2]")).Text.Contains(two_way_email) &&
                driver.FindElement(By.XPath(".//*[@id='emailPanel']/div/div[4]/div/div[2]/fieldset[1]/span")).Text.Contains(standard_send_pattern))
            {
                driver.FindElement(By.LinkText("POP")).Click();
                Thread.Sleep(2000);

                if (driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[1]/fieldset[1]/span")).Text.Contains(pop_server) &&
                    driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[2]/fieldset/span")).Text.Contains(pop_port) &&
                    driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[3]/fieldset/span")).Text.Contains(pop_one_acc) &&
                    driver.FindElement(By.XPath(".//*[@id='POP']/div/div[1]/div[4]/fieldset/span")).Text.Contains(pop_two_acc))
                {

                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Email Gateway Passed ...  ^^^^^^^^^^^^^^^^^^^^^^^^^");
                }
                else
                {
                    Assert.Fail("Email Gateway Failed in POP section ...");
                }
            }
            else
            {

                Assert.Fail("Email Gateway Failed in SMTP section ...");
            }

        }


        [Test]
        public void License_Page()
        {

            check_driver_type(driver_type, "administration", "Install Licence","Sys Admin");

            Assert.AreEqual("Install Licence", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

      /*      var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Install Licence']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Install Licence']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                hover_func("administration", "Install Licence");
            }*/

        

            Console.WriteLine("*"+driver.FindElement(By.XPath("//div[@class='license_table']/ul/li[@class='product_name']")).Text+"*");

            if (!(driver.FindElement(By.XPath("//div[@class='license_table']/ul/li[@class='product_name']")).Text.Contains("HipLink") &&
                driver.FindElement(By.XPath("//div[@class='license_table']/ul/li[@class='version']")).Text.Contains("4.7") &&
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
        public void z_About_Page()
        {

            check_driver_type(driver_type, "administration", "About Hiplink","Sys Admin");

            Assert.AreEqual("About", driver.FindElement(By.XPath("//div[@class='main_container pg_about']/h1")).Text);

          /*  var driver_type = driver.GetType();
            Console.WriteLine("*" + driver_type + "*");

            if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
            {
                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='About Hiplink']")).Click();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='About Hiplink']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
                hover_func("administration", "About Hiplink");
            }*/


          

           
            if (!(driver.FindElement(By.Id("lblProductName")).Text.Equals("HipLink") &&
                driver.FindElement(By.XPath("//i[@class='product_name_sec']")).Text.Contains("Hiplink Alert Notification Solutions") &&
                driver.FindElement(By.XPath("//div[@class='about_left_panel notBorder']/p")).Text.Contains("HipLink Software was founded in 1993 with corporate headquarters in the heart of Silicon Valley California. As a stable, profitable, woman-owned business, HipLink continues to demonstrate a high commitment to its customers, while introducing numerous technological innovations. HipLink Software has been the premier provider of software for wireless text and voice communication to global organizations of all sizes for over fifteen years.")))
            {
                Assert.Fail("About page Failed...");
            }
            else
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^  About page Passed...  ^^^^^^^^^^^^^^^^^^^");
            }
        }


        [Test]
        public void Global_Search_Panel()
        {
            string user = "fahad";
            string department = "new_dpt";
            string receiver = "receiver1";
            string user_group = "user_group";
            string receiver_group = "bg";

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
            if (driver.FindElement(By.XPath(".//*[@id='tab_UserBlock']/div/div[2]/div/div/div/div[1]")).Text.Contains(user))
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
            if (driver.FindElement(By.XPath(".//*[@id='tab_DepartmentBlock']/div/div[2]/div/div/div/div[1]")).Text.Contains(department))
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
            if (driver.FindElement(By.XPath(".//*[@id='tab_ReceiverBlock']/div/div[2]/div/div/div/div[1]")).Text.Contains(receiver))
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
            if (driver.FindElement(By.XPath(".//*[@id='tab_UserGroupBlock']/div/div[2]/div/div/div/div[1]")).Text.Contains(user_group))
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

     /*       driver.FindElement(By.XPath("//span[text()='Receive Groups']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("txtSearch")).Click();
            driver.FindElement(By.Id("txtSearch")).Clear();
            driver.FindElement(By.Id("txtSearch")).SendKeys(receiver_group);
            driver.FindElement(By.Id("btnSearchGo")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Receive Groups")).Click();
            Thread.Sleep(2000);
            if (driver.FindElement(By.XPath(".//*[@id='tab_ReceiveGroupsBlock']/div/div[2]/div/div/div/div[1]")).Text.Contains(receiver_group))
            {
                Console.WriteLine("User Group record fetched ...");
                driver.FindElement(By.Id("btnClose")).Click();
            }
            else
            {
                Assert.Fail("User Group record not fetched ...");
            }
            */

   
            
        }

        public void takescreenshot(string suffix)
        {
           
            string image_name=suffix;
       
            Screenshot Shot = ((ITakesScreenshot)driver).GetScreenshot();

            Shot.SaveAsFile(create_directory_path_with_time+"\\"+image_name+".png", System.Drawing.Imaging.ImageFormat.Png);

        }


        public string random_alphanum(string alphanumeric)
        {

        Random r = new Random();
        string random_alpha =  alphanumeric+ r.Next(1, 1000);

        return random_alpha;

        
        }

        public void check_driver_type(string drivertype, string id_name, string link_text,string a_text) //drivertype= browser , id_name = landing page , link_text = panel(e.g Add user page) 
        {

            Thread.Sleep(2000);

            if (drivertype.ToString() == "OpenQA.Selenium.Safari.SafariDriver") //for safari
            {

                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);
                
                driver.FindElement(By.XPath(".//*[@id='"+id_name+"']/a")).Click(); //goto landing for particular ID
                Thread.Sleep(2000);

                Assert.AreEqual(trimmed_user_label, "Welcome fahad");

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='" + link_text + "']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(2000);

                Assert.AreEqual(trimmed_user_label, "Welcome fahad");
            
            }
            
            else if (drivertype.ToString() == "OpenQA.Selenium.Chrome.ChromeDriver" || drivertype.ToString() == "OpenQA.Selenium.Firefox.FirefoxDriver") //for firefox and chrome
            
            {

                Console.WriteLine("using hover func() ....");
                Thread.Sleep(2000);

                //a[contains(text(),'On-Duty')])[2]

                driver.FindElement(By.XPath("//li[@id='" + id_name + "']/a[text()='"+a_text+"']")).Click(); //goto landing for particular ID
                Thread.Sleep(2000);



                Actions a1c = new Actions(driver);
                Thread.Sleep(2000);

                a1c.MoveToElement(driver.FindElement(By.XPath("//div[@class='footer']"))).Perform();
                Thread.Sleep(3000);

                Assert.AreEqual(trimmed_user_label, "Welcome fahad");

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

                Assert.AreEqual(trimmed_user_label, "Welcome fahad");

                driver.FindElement(By.XPath(".//*[@id='" + id_name + "']/a")).Click(); //goto landing for particular ID
                Thread.Sleep(2000);

                hover_func(id_name, link_text, a_text);
                Thread.Sleep(2000);
             
            }

            else // for IE
            
            {
                
                // drivertype.ToString() == "OpenQA.Selenium.IE.InternetExplorerDriver"

                Assert.AreEqual(trimmed_user_label, "Welcome fahad");

                hover_func(id_name, link_text,a_text);
                Thread.Sleep(2000);
            }

        }


       
        public void hover_func(string id_name,string link_text,string a_text)
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


        [Test]
        public void y_Get_Testcase_Node() // Get all testcases results
        {
            using (XmlTextReader reader = new XmlTextReader(@".\TestResult.xml"))
            {


                while (reader.Read()) // read all test case nodes from TestResult.xml
                {
                    if (reader.IsStartElement("test-case"))
                    {
                        Console.WriteLine("*name*" + reader.GetAttribute("name"));
                        Console.WriteLine("*executed*" + reader.GetAttribute("executed"));
                        Console.WriteLine("*result*" + reader.GetAttribute("result"));
                        Console.WriteLine("*success*" + reader.GetAttribute("success"));
                        Console.WriteLine("*time*" + reader.GetAttribute("time"));
                        Console.WriteLine("*asserts*" + reader.GetAttribute("asserts"));




                        testcase_name_list.Add(reader.GetAttribute("name"));            // get the value of name attribute under testcase node <test-case name="Add user">
                        testcase_executed_list.Add(reader.GetAttribute("executed"));
                        testcase_result_list.Add(reader.GetAttribute("result"));
                        testcase_success_list.Add(reader.GetAttribute("success"));
                        testcase_time_list.Add(reader.GetAttribute("time"));

                        if (reader.GetAttribute("success").ToString().Equals("True"))  // if test case is passed then msg and stack column will be displayed as "None"
                        {
                            testcase_msg_list.Add("None");
                            testcase_stack_list.Add("None");
                        }

                        testcase_count = testcase_count + 1;        // count the number of testcases


                    }

                    else if (reader.IsStartElement("message")) //read the message node for failed testcase
                    {

                        Console.WriteLine(testcase_msg_list);
                        testcase_msg_list.Add(reader.ReadElementString());

                    }

                    else if (reader.IsStartElement("stack-trace")) //read the stack trace node for failed testcase
                    {

                        Console.WriteLine(testcase_stack_list);
                        testcase_stack_list.Add(reader.ReadElementString());

                    }

                  
                }
              
                create_file(); // function to create html report called at the end of while loop

            }
         
        }





        public void create_file() // function to create html report
        {

            DateTime todaydatetime_createfile = DateTime.Now;          // Use current time

            string format = "yyyy_MM_dd_hh_mm_ss";                     // Use this format Year_Month_Date_Hour_Minute_Second => 2014_04_21_02_35_09

            string format1 = "ddd, MMM d, yyy HH:mm:ss";                // used inside file

            /*     MMM     display three-letter month
                   ddd     display three-letter day of the WEEK
                   d       display day of the MONTH
                   HH      display two-digit hours on 24-hour scale
                   mm      display two-digit minutes
                   yyyy    display four-digit year */

            string path = create_directory_path_with_time+@"\Testcase_result" + todaydatetime_createfile.ToString(format) + ".html";
            

            Console.WriteLine(path);

            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("<!DOCTYPE html>"); //creating html tags
                    sw.WriteLine("<html>");
                    sw.WriteLine("<body>");

                    sw.WriteLine("<head>");

                    sw.WriteLine("<style>");  // set style for table
                    sw.WriteLine("table,th,td");
                    sw.WriteLine("{");
                    sw.WriteLine("border:1px solid black;");
                    sw.WriteLine("border-collapse:collapse;");
                    sw.WriteLine("}");
                    sw.WriteLine("th,td");
                    sw.WriteLine("{");
                    sw.WriteLine("padding:5px;");
                    sw.WriteLine("}");
                    sw.WriteLine("th");
                    sw.WriteLine("{");
                    sw.WriteLine("text-align:left;");
                    sw.WriteLine("}");
                    sw.WriteLine("</style>");

                    sw.WriteLine("</head>");

                    sw.WriteLine("<table>"); //creating table

                    sw.WriteLine("<tr style=\"font-weight:bold\">"); // creating header row
                    sw.WriteLine("<td>Testcase Name</td>");
                    sw.WriteLine("<td>Testcase Executed</td>");
                    sw.WriteLine("<td>Testcase Result</td>");
                    sw.WriteLine("<td>Testcase Success</td>");
                    sw.WriteLine("<td>Testcase Time (sec)</td>");
                    sw.WriteLine("<td>Testcase Message</td>");
                    sw.WriteLine("<td>Stack Trace</td>");

                    sw.WriteLine("</tr>");

                    for (int i = 0; i < testcase_name_list.Count; i++)
                    {

                        sw.WriteLine("<tr>");  //creating testcase result rows
                        sw.WriteLine("<td>" + testcase_name_list[i] + "</td>");
                        sw.WriteLine("<td>" + testcase_executed_list[i] + "</td>");
                       

                        if (testcase_success_list[i].Equals("True"))
                        {
                            sw.WriteLine("<td style=\"color:green\">" + testcase_result_list[i] + "</td>");
                            sw.WriteLine("<td style=\"color:green\">" + testcase_success_list[i] + "</td>");
                        }
                        else if (testcase_success_list[i].Equals("False"))
                        {
                            sw.WriteLine("<td style=\"color:red\">" + testcase_result_list[i] + "</td>");
                            sw.WriteLine("<td style=\"color:red\">" + testcase_success_list[i] + "</td>");
                        }

                        sw.WriteLine("<td>" + testcase_time_list[i] + "</td>");
                        sw.WriteLine("<td>" + testcase_msg_list[i] + "</td>");
                        sw.WriteLine("<td>" + testcase_stack_list[i] + "</td>");
                        sw.WriteLine("</tr>");

                        if (testcase_success_list[i].Equals("True")) //get the count of passed testcases
                        {
                            testcase_success_count = testcase_success_count + 1;
                            Console.WriteLine("Testcase_success_count:" + testcase_success_count);
                        }
                        else if (testcase_success_list[i].Equals("False")) //get the count of failed testcases
                        {
                            testcase_failed_count = testcase_failed_count + 1;
                            Console.WriteLine("Testcase_failed_count:" + testcase_failed_count);
                        }

                    }


                    sw.WriteLine("<center>");
                    sw.WriteLine("<b> <font size=\"6\"><u>TESTCASE EXECUTION REPORT</u> </font> </b>"); // report heading
                    sw.WriteLine("</center>");


                    sw.WriteLine("<p>");
                    sw.WriteLine("<b> <u>SUMMARY </u></b>");
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p>");
                    sw.WriteLine("Date :" + " " + "<b>" + todaydatetime_createfile.ToString(format1) + "</b>"); //report date and time
                    sw.WriteLine("</p>");


                    sw.WriteLine("<p>");
                    sw.WriteLine("Total Testcases :" + " " + "<b>" + testcase_count + "</b>"); //total testcases
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p style=\"color:green\">");
                    sw.WriteLine("Testcases Passed :" + " " + "<b>" + testcase_success_count + "</b>"); // passed testcases
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p style=\"color:red\">");
                    sw.WriteLine("Testcases Failed :" + " " + "<b>" + testcase_failed_count + "</b>"); // failed testcases
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p/>");

                    sw.WriteLine("<p>");
                    sw.WriteLine("<b> <u>DETAILS </u></b>");
                    sw.WriteLine("</p>");


                    sw.WriteLine("</table>");
                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");


                    /*      sw.WriteLine("<tr>");
                          sw.WriteLine("<td>" + testcase_name + "</td>");
                          sw.WriteLine("<td>" + testcase_executed + "</td>");
                          sw.WriteLine("<td>" + testcase_result + "</td>");
                          sw.WriteLine("<td>" + testcase_sucess + "</td>");
                          sw.WriteLine("</tr>");*/

                    //   create_table();





                    /*     sw.WriteLine("Date and time :" + " " + System.DateTime.Now.Date.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString());
                         sw.WriteLine("Test case name : " + " " + testcase_name);
                         sw.WriteLine("Test case executed : " + " " + testcase_executed);
                         sw.WriteLine("Test case result : " + " " + testcase_result);
                         sw.WriteLine("Test case success : " + " " + testcase_sucess);
                         sw.WriteLine("Test case time : " + " " + testcase_time);
                         sw.WriteLine("\n");*/

                    //    sw.WriteLine("Test case name : " + " " +"Test case executed : " + " " + "Test case result : " + " " + "Test case success : " + " " + "Test case time : ");
                    //    sw.WriteLine(testcase_name + " " + testcase_executed + " " + testcase_result + " " + testcase_sucess + " " + testcase_time);

                }
            }


            /* else
             {
             
                 using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug\MyTestResults.html", true))
                 {

                   
                     sw.WriteLine("<tr>");
                     sw.WriteLine("<td>" + testcase_name + "</td>");
                     sw.WriteLine("<td>" + testcase_executed + "</td>");
                     sw.WriteLine("<td>" + testcase_result + "</td>");
                     sw.WriteLine("<td>" + testcase_sucess + "</td>");
                     sw.WriteLine("</tr>");


                         sw.WriteLine("<p>");
                         sw.WriteLine("Testcases Count :" + " " + "<b>" + testcase_count + "</b>");
                         sw.WriteLine("</p>");

                         sw.WriteLine("</body>");
                         sw.WriteLine("</html>");
                    
           /*          sw.WriteLine("Date and time :" + " " + System.DateTime.Now.Date.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString());
                     sw.WriteLine("Test case name : " + " " + testcase_name);
                     sw.WriteLine("Test case executed : " + " " + testcase_executed);
                     sw.WriteLine("Test case result : " + " " + testcase_result);
                     sw.WriteLine("Test case success : " + " " + testcase_sucess);
                     sw.WriteLine("Test case time : " + " " + testcase_time);
                     sw.WriteLine("\n");

                //     sw.WriteLine("Test case name : " + " " + "Test case executed : " + " " + "Test case result : " + " " + "Test case success : " + " " + "Test case time : ");
                //     sw.WriteLine(testcase_name + " " + testcase_executed + " " + testcase_result + " " + testcase_sucess + " " + testcase_time);

                 }
             }*/
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

        /*  using (System.IO.StreamReader sr = new System.IO.StreamReader(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug\MyTestResults.html", true))
               {
                   StringBuilder newFile = new StringBuilder();
                   StringBuilder newFile1 = new StringBuilder();

                   Console.WriteLine(sr.ReadToEnd());

                   string temp = "";
                   string temp1 = "";

                   string[] readfile = File.ReadAllLines(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug\MyTestResults.html");

                   foreach (string line in readfile)
                   {

                       if (line.Contains("</body>"))
                       {

                           temp = line.Replace("</body>", "");

                           newFile.Append(temp + "\r\n");

                       }
                       if (line.Contains("</html>"))
                       {
                           temp1 = line.Replace("</html>", "");

                           newFile.Append(temp1 + "\r\n");

                       }

                       newFile.Append(line + "\r\n");

                   }

                   File.WriteAllText(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug\MyTestResults.html", newFile.ToString());
                    
               }*/







        /*  var driver_type = driver.GetType();
           Console.WriteLine("*" + driver_type + "*");

           if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
           {
               Console.WriteLine("if clause ....");
               Thread.Sleep(2000);
               driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
               Thread.Sleep(2000);
               driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='System Attendent Settings']")).Click();
               Thread.Sleep(2000);
           }
           else
           {
               Console.WriteLine("using hover func() ....");
               Thread.Sleep(2000);
               driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
               Thread.Sleep(2000);
               driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='System Attendent Settings']")).Click();
               Thread.Sleep(2000);
               driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
               hover_func("administration", "System Attendent Settings");
           }*/
         




        /*     if (driver_type.ToString() == "OpenQA.Selenium.Safari.SafariDriver")
         {
             Console.WriteLine("if clause ....");
             Thread.Sleep(2000);
             driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
             Thread.Sleep(2000);
             driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Timezone Settings']")).Click();
             Thread.Sleep(2000);
         }
         else
         {
             Console.WriteLine("using hover func() ....");
             Thread.Sleep(2000);
             driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
             Thread.Sleep(2000);
             driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='Timezone Settings']")).Click();
             Thread.Sleep(2000);
             driver.FindElement(By.XPath(".//*[@id='administration']/a")).Click();
             hover_func("administration", "Timezone Settings");
         }*/



        /*   [Test]
       public void safari()
       {
           var driver_type = driver.GetType();
           Console.WriteLine("*" + driver_type + "*");

           if (driver_type.Equals(driver_type))
           {
           Thread.Sleep(2000);
           driver.FindElement(By.XPath(".//*[@id='settings']/a")).Click();
           Thread.Sleep(2000);
           }

       }*/


        /*    [Test]
            public void login()
            {
            
                driver.Navigate().GoToUrl(baseURL + "/HipLink5UI-Work/index.html#login");
        
                Console.WriteLine("Here is Welcome text "+" "+driver.FindElement(By.CssSelector("li.user_name")).Text);
                Console.WriteLine("Here is Page title1 " + " " + driver.Title);
            
                driver.FindElement(By.Id("username")).Clear();
                driver.FindElement(By.Id("username")).SendKeys(login_name);
                driver.FindElement(By.Id("password")).Clear();
                driver.FindElement(By.Id("password")).SendKeys(login_pwd);
                driver.FindElement(By.CssSelector("a.c_btn_large1.login_button")).Click();
                Thread.Sleep(2000);
           
                Console.WriteLine("after login " + "*" + driver.FindElement(By.CssSelector("li.user_name")).Text + "*");

                if (driver.FindElement(By.CssSelector("li.user_name")).Text.Contains(welcome_username))
                {
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^  Login Testcase Passed   ^^^^^^^^^^^^^^^^^^^^^");
                    driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }
                else
                {
                    Assert.Fail("Login Testcase Failed");
                }

                Thread.Sleep(1000);

            }*/


        /*     [Test]
       public void xmlP()
       {
          using  (XmlTextReader  reader = new  XmlTextReader("D:\\TestResult.xml"))
           {
               while  (reader.Read())
               {
                   if  (reader.IsStartElement())
                   {
                        
                       switch  (reader.Name)
                       {
                           case "test-case":
                               Console.WriteLine();
                               break;

                           case "name":
                               Console.WriteLine("name: " + reader.ReadString());
                               break;
                           case  "executed":
                               Console.WriteLine("executed: " + reader.ReadString());
                               break;
                           case  "result":
                               Console.WriteLine("result: " + reader.ReadString());
                               break;
                       }
                   }
               }

           }
        
       }*/


        /*    [Test]
            public void SwitchToTest() { 
   
                IWebDriver driver;
                ICollection<string> windowids = null;
                IEnumerator<String> iter = null;
            String mainWindowId=null;
            String curWindow = null;

        driver = new FirefoxDriver();
        driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5L));

        Console.WriteLine("Handling popup window");
        driver.Navigate().GoToUrl("http://hdfc.com");

        windowids = driver.WindowHandles; // returns an ID for every opened window
                iter = windowids.GetEnumerator();

                mainWindowId = driver.CurrentWindowHandle;
        Console.WriteLine("Main window handle: "+mainWindowId);

        Console.WriteLine("List Window IDs. There should be 1 id now");
        Console.WriteLine("=========================================");

                for (int i = 0; i < windowids.Count; i++)
                {
                    Console.WriteLine(windowids.ElementAt(i));
                }

        driver.FindElement(By.XPath("//*[@id='acc-1-head-2']/a")).Click(); // customer care link
        driver.FindElement(By.XPath("//*[@id='acc-1-section-2']/li[1]/a")).Click(); //write to us link
        //the above should open a new tab on current browser window BUT Selenium will open it as a new browser window

        Thread.Sleep(25);
        windowids = null;
        windowids = driver.WindowHandles; // returns an ID for every opened window
                iter = windowids.GetEnumerator(); ;  // iterate through open browser and print out their ids. One id only for now.
        Console.WriteLine("List Window IDs. There should be 2 id now");
        Console.WriteLine("=========================================");

        for (int i=0; i<windowids.Count;i++){
                     Console.WriteLine(windowids.ElementAt(i));
        }	

        // Now switch between the open window. 
        for (int yy=0; yy<3; yy++)
        { 
            // do the switching this number of times
            
            for(int y=0; y<2; y++)
             
            {
                // switch b/w windows 1 and 2
            
                driver.SwitchTo().Window(windowids.ElementAt(y)).Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5L));

                if(y==0)
                {

                    curWindow="Main Window";

                }
                else
                {

                    curWindow="Popup Window";
                }

                Console.WriteLine("Switching to " + curWindow + " window... Main window title: "+driver.Title);
                Thread.Sleep(25);
            }
        }

                driver.Quit();
            }*/
    

    }
}
