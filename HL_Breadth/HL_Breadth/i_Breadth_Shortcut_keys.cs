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
    public class i_Breadth_Shortcut_keys : HL_Base_Class
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
        public void shortcut_keys()
        {
            /*
                        // Users data
                        driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pud");

                        if (driver.Url.Contains("#profile/my_data/user_data"))
                        {
                            Thread.Sleep(2000);
                            //  Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);



                        }
                        else
                        {

                            Assert.Fail("Shortcutkeys Users data");

                        }
             */

            /*
             // Change Password
             driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).Click();
             driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pcp");

             Thread.Sleep(1000);

             //   Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text + "*");

             Console.WriteLine("*" + driver.Url + "*");

             if (driver.Url.Contains("#profile/my_data/change_password"))
             {
                 Thread.Sleep(2000);
              //   Assert.AreEqual("Change Password", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

             }
             else
             {

                 Assert.Fail("Shortcutkeys Change Password");

             }
             */


            // Common System Information
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("psi");

            if (driver.Url.Contains("#profile/my_data/common_system_information"))
            {
                Thread.Sleep(2000);
                // Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }

            else
            {

                Assert.Fail("Shortcutkeys Common System Information");

            }

            // Usage Reports
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pur");

            if (driver.Url.Contains("#profile/my_data/usage_reports"))
            {
                Thread.Sleep(2000);
                //   Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Usage Reports");

            }

            // Bio information
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pbi");

            if (driver.Url.Contains("#profile/my_data/bio_information"))
            {
                Thread.Sleep(2000);
                //  Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Bio information");

            }

            // Recipients
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("pfr");

            if (driver.Url.Contains("#profile/favorites/recipients"))
            {
                Thread.Sleep(2000);
                //  Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Recipients");

            }

            // Statistics
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("prs");

            if (driver.Url.Contains("#profile/recent_messaging/statistics"))
            {
                Thread.Sleep(2000);
                //  Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Statistics");

            }

            // Global Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ags");

            if (driver.Url.Contains("#administration/system/global_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Global Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Global Settings");

            }

            // Directories
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asd");

            if (driver.Url.Contains("#administration/system/directories"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Directories & Queues", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Directories");

            }

            // LDAP Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("alds");

            if (driver.Url.Contains("#administration/system/ldap_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("LDAP", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys LDAP Settings");

            }

            // Log Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("als");

            if (driver.Url.Contains("#administration/system/log_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Log Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Log Settings");

            }

            // DB Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ads");

            if (driver.Url.Contains("#administration/system/db_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Database Configuration Setting", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys DB Settings");

            }

            // Timezone Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ats");

            if (driver.Url.Contains("#administration/system/timezone_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Time Zone Setting", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Timezone Settings");

            }

            // Messengers
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("amr");

            if (driver.Url.Contains("#administration/delivery/messengers"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Messengers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Messengers");

            }

            // Carriers
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("acr");

            if (driver.Url.Contains("#administration/delivery/carriers"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Carriers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Carriers");

            }

            // Filters
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("afr");

            if (driver.Url.Contains("#administration/delivery/filters"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Filters Panel", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Filters");

            }

            // System Attendant Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asa");

            if (driver.Url.Contains("#administration/monitoring_failover/system_attendant_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("System Attendant Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys System Attendant Settings");

            }

            // Backup Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("abs");

            if (driver.Url.Contains("#administration/monitoring_failover/backup_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Backup Service", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Backup Settings");

            }

            // Services
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asv");

            if (driver.Url.Contains("#administration/services_logs/services"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Services", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Services");

            }

            // Logs
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asl");

            if (driver.Url.Contains("#administration/services_logs/logs"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Logs", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Logs");

            }

            // Sessions Manager
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("asm");

            if (driver.Url.Contains("#administration/services_logs/sessions_manager"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Sessions Manager", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Sessions Manager");

            }

            // About
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("me");

            if (driver.Url.Contains("#about"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("About", driver.FindElement(By.XPath("//div[@class='main_container pg_about']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys About");

            }

            // Users
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sur");

            if (driver.Url.Contains("#settings/accounts/users"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Users", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Users");

            }

            // User Groups
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sug");

            if (driver.Url.Contains("#settings/accounts/user_groups"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("User Groups", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys User Groups");

            }

            // Department panel
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sdp");

            if (driver.Url.Contains("#settings/accounts/departments"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Departments", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Department panel");

            }

            // Recipient Users
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sru");

            if (driver.Url.Contains("#settings/accounts/recipient_users"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Recipient Users Main", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Recipient Users");

            }

            // Alarm Notification Gateway 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("san");

            if (driver.Url.Contains("#settings/integrations/alarm_notification_gateway"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Alarm Notification Gateway", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Alarm Notification Gateway");

            }

            // File System Interface
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sfs");

            if (driver.Url.Contains("#settings/integrations/file_system_interface"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("File System Interface", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys File System Interface");

            }

            // Email Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sig");

            if (driver.Url.Contains("#settings/integrations/email_gateway/view"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Email Gateway", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Email Gateway");

            }

            // SNPP Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sng");

            if (driver.Url.Contains("#settings/integrations/snpp_gateway/view"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("SNPP Gateway", driver.FindElement(By.XPath("//div[@id='main']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys SNPP Gateway");

            }

            // XMPP Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sxg");

            if (driver.Url.Contains("#settings/integrations/xmpp_gateway"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("XMPP Gateway", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys XMPP Gateway");

            }

            // TAP Gateway
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("stg");

            if (driver.Url.Contains("#settings/integrations/tap_gateway"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("TAP Gateway", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys TAP Gateway");

            }

            // SNMP ALerts
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("smg");

            if (driver.Url.Contains("#settings/integrations/snmp_alerts"))
            {
                Thread.Sleep(2000);
                // Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys SNMP ALerts");

            }

            // Websign up Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sws");

            if (driver.Url.Contains("#settings/mass_notifications/websignup-settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Web Sign-up Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Websign up Settings");

            }

            // GIS Settings
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sgs");

            if (driver.Url.Contains("#settings/mass_notifications/gis_settings"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("GIS Integration", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys GIS Settings");

            }

            // Websign up Profiles
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swp");

            if (driver.Url.Contains("#settings/mass_notifications/web_signup_profiles"))
            {
                Thread.Sleep(2000);
                //  Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Websign up Profiles");

            }

            // Websign up Topic Profiles
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swt");

            if (driver.Url.Contains("#settings/mass_notifications/web_signup_topic_profiles"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Web Sign-up Topic Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Websign up Topic Profiles");

            }

            // Websign up Characteristics Profiles
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swc");

            if (driver.Url.Contains("#settings/mass_notifications/web_signup_characteristics_profile"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Web Sign-up characteristics Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Websign up Characteristics Profiles");

            }

            // Websign up Recipients
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("swr");

            if (driver.Url.Contains("#settings/mass_notifications/web_signup_recipients"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Web Sign-up Registered Recipients", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Websign up Recipients");

            }

            // Enterprise Mobility Activation
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sea");

            if (driver.Url.Contains("#settings/enterprise_mobility/activation"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Activation", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Enterprise Mobility Activation");

            }

            // Enterprise Mobility Configuration
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sec");

            if (driver.Url.Contains("#settings/enterprise_mobility/configuration"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("HNP Configuration", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Enterprise Mobility Configuration");

            }

            // General Ploicy 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("seg");

            if (driver.Url.Contains("#settings/enterprise_mobility/general_policy"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("General Policy", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys General Ploicy");

            }

            // Message Templates 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("smt");

            if (driver.Url.Contains("#settings/templates/message_templates"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Message Template", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Message Templates");

            }

            // Schedule Templates 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sct");

            if (driver.Url.Contains("#settings/templates/schedule_templates"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Schedule Template", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Schedule Templates");

            }

            // Events Feedback 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sef");

            if (driver.Url.Contains("#settings/2way_actions/events_feedback"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Feedback", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Events Feedback");

            }

            // Response Actions 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("sra");

            if (driver.Url.Contains("#settings/2way_actions/response_actions"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Response Actions Panel", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Response Actions");

            }

            // Broadcast Group 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("cbg");

            if (driver.Url.Contains("#recipients/groups/broadcast_group"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Broadcast Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Broadcast Group");

            }

            // Onduty Group 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("cro");

            if (driver.Url.Contains("#recipients/groups/on_duty_group"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("On-Duty Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Onduty Group");

            }

            // Escalation Group 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("cre");

            if (driver.Url.Contains("#recipients/groups/escalation_group"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Escalation Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Escalation Group");

            }

            // Rotation Group 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("crg");

            if (driver.Url.Contains("#recipients/groups/rotation_group"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Rotate Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Rotation Group");

            }

            // Follow me Group 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("crf");

            if (driver.Url.Contains("#recipients/groups/follow_me_group"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Follow-Me Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Follow me Group");

            }

            // Subscription Group 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("crs");

            if (driver.Url.Contains("#recipients/groups/subscription_group"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Subscription Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Subscription Group");

            }

            // Receivers 
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("crc");

            if (driver.Url.Contains("#recipients/devices/receivers"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Receivers", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Receivers");

            }

            // Receiver Search
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("crh");

            if (driver.Url.Contains("#recipients/devices/reciever_search"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Receiver Search", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Receiver Search");

            }

            // Primary Send
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsp");

            if (driver.Url.Contains("#send/send/primary_send"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Primary Send Panel", driver.FindElement(By.Id("lblPanelTitle")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Primary Send");

            }

            // Quick Send
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsq");

            if (driver.Url.Contains("#send/send/quick_send"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Quick Send Panel", driver.FindElement(By.Id("lblPanelTitle")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Quick Send");

            }

            // Escalation Send
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nse");

            if (driver.Url.Contains("#send/send/escalation_send"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Escalation Send Panel", driver.FindElement(By.Id("lblPanelTitle")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Escalation Send");

            }

            // Fax Send
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsf");

            if (driver.Url.Contains("#send/send/fax_send"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Fax Send Panel", driver.FindElement(By.Id("lblPanelTitle")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Fax Send");

            }

            // Voice Send
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsv");

            if (driver.Url.Contains("#send/send/voice_send"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Voice Send Panel", driver.FindElement(By.Id("lblPanelTitle")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Voice Send");

            }

            // Attribute Send
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsa");

            if (driver.Url.Contains("#send/send/attribute_send"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Attribute Send Panel", driver.FindElement(By.Id("lblPanelTitle")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Attribute Send");

            }

            // Quota Send
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsu");

            if (driver.Url.Contains("#send/send/quota_send"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Quota Send Panel", driver.FindElement(By.Id("lblPanelTitle")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Quota Send");

            }

            // Websign up Notification
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nmw");

            if (driver.Url.Contains("#send/mass_notification/web_signup_notification"))
            {
                Thread.Sleep(2000);
                //  Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Websign up Notification");

            }

            // Map based Notification
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nmn");

            if (driver.Url.Contains("#send/mass_notification/map_based_notification"))
            {
                Thread.Sleep(2000);
                //    Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Map based Notification");

            }

            // Campaign Progress
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsc");

            if (driver.Url.Contains("#send/send_management/campaign_progress"))
            {
                Thread.Sleep(2000);
                //   Assert.AreEqual("Departments Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Campaign Progress");

            }

            // Confirm Message
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsg");

            if (driver.Url.Contains("#send/send_management/confirm_message"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual(true, driver.FindElement(By.XPath("//form[@id='jobConfirmationLightbox']")).Displayed);

                driver.FindElement(By.Id("btnCloseJobConfirm")).Click();
                Thread.Sleep(2000);

            }
            else
            {

                Assert.Fail("Shortcutkeys Confirm Message");

            }

            // Resend Message
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("nsn");

            if (driver.Url.Contains("#send/send_management/resend_message"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Resend Panel", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Resend Message");

            }

            // Escalation Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qe");

            if (driver.Url.Contains("#queues/message_queues/Escalation/3"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Escalation Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Escalation Queue");

            }

            // Scheduled Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qs");

            if (driver.Url.Contains("#queues/message_queues/Scheduled/2"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Scheduled Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Scheduled Queue");

            }

            // Waiting Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qw");

            if (driver.Url.Contains("#queues/message_queues/Waiting/10"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Waiting Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Waiting Queue");

            }

            // Fax Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qx");

            if (driver.Url.Contains("#queues/paging_queues/Fax/6"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Fax Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Fax Queue");

            }

            // Voice Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qv");

            if (driver.Url.Contains("#queues/paging_queues/Voice/7"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Voice Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Voice Queue");

            }

            // Default Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qd");

            if (driver.Url.Contains("#queues/paging_queues/Default/101"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Default Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Default Queue");

            }

            // Failed Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qf");

            if (driver.Url.Contains("#queues/terminal_queues/Failed/5"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Failed Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Failed Queue");

            }

            // Filtered Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ql");

            if (driver.Url.Contains("#queues/terminal_queues/Filtered/9"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Filtered Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Filtered Queue");

            }

            // Completed Queue
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("qc");

            if (driver.Url.Contains("#queues/terminal_queues/Completed/4"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Completed Queue", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Completed Queue");

            }

            // Report List
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("rl");

            if (driver.Url.Contains("#reports/main/reports/list"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Reports", driver.FindElement(By.Id("main_title")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Report List");

            }

            // Report Summary
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("ru");

            if (driver.Url.Contains("#reports/main/reports/summary"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Reports", driver.FindElement(By.Id("main_title")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Report Summary");

            }

            // Stats
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("rt");

            if (driver.Url.Contains("#reports/main/stats"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Statistics", driver.FindElement(By.Id("main_title")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Stats");

            }

            // Websign up
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("rw");

            if (driver.Url.Contains("#reports/main/reports/webSignup"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Reports", driver.FindElement(By.Id("main_title")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Websign up");

            }

            // Reports
            driver.FindElement(By.XPath(".//*[@id='lblCustomHeader']")).SendKeys("rs");

            if (driver.Url.Contains("#reports"))
            {
                Thread.Sleep(2000);
                Assert.AreEqual("Reports", driver.FindElement(By.Id("main_title")).Text);

            }
            else
            {

                Assert.Fail("Shortcutkeys Reports");

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

