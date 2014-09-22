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
    public class e_Smoke_System : HL_Base_Class
    {
        private IWebDriver driver;

        private StringBuilder verificationErrors;

        private string baseURL;

        private bool acceptNextAlert = true;

        public string login_name;

        public string login_pwd;

        public string welcome_username = "Welcome admin"; //used in login testcase to verify 'Welcome user' label after login

        public string browser = "Mozilla"; //used in session manager according to browser(firefox,chrome,IE)

        public string driver_type;

        string browser_type;

        string browser_name;

        string user_label;

        string trimmed_user_label;

        string create_directory_path = @".\Screenshots_System_Testcase";

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

            // System.Diagnostics.Debugger.Launch();// launch debugger

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

            Assert.AreEqual(trimmed_user_label, "Welcome "+lines_local[0]);

            verificationErrors = new StringBuilder();
        
        }

        

        [Test]
        public void a_System_Attendant_Settings()
        {
          //  string administrationemail = "fali@folio3.com";
            string numberofcompletemsgs = "20";
            string numberoffailedmsgs = "30";
            string numberoffilteredmsgs = "40";
            string idlemsgtime = "60";
            string alertcommand = "Test Alert Command";
            string expired = "Yes";
            string expired_status_on_page_load;
            string expired_status_after_saving;


            check_driver_type(driver_type, "administration", "System Attendent Settings", "Sys Admin");

            Assert.AreEqual("System Attendant Settings", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            string[] lines_local = read_from_file("system_attendant_settings"); // return all the data in the form of array

            string administrationemail = lines_local[0];

            expired_status_on_page_load = driver.FindElement(By.Id("lblDelExpired")).Text.ToString();

            Console.WriteLine(expired_status_on_page_load);

            driver.FindElement(By.Id("btnedit")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtemail")).Clear();

            driver.FindElement(By.Id("txtemail")).SendKeys(administrationemail);

     //      driver.FindElement(By.XPath("//select[@id='selrecipient']")).Click();

    //        driver.FindElement(By.XPath("//li[contains(text(),'receiver_smtp')]")).Click();

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

            driver.FindElement(By.Id("btnOk")).Click();

            expired_status_after_saving = driver.FindElement(By.Id("lblDelExpired")).Text.ToString();

            Console.WriteLine(expired_status_after_saving);
            Thread.Sleep(2000);

            if (driver.FindElement(By.XPath("//*[@id='lblAdminEmail']")).Text.Equals(administrationemail) &&

                    driver.FindElement(By.XPath("//*[@id='lblCompMsg']")).Text.Equals(numberofcompletemsgs) &&

                    driver.FindElement(By.XPath("//*[@id='lblFailedMsg']")).Text.Equals(numberoffailedmsgs) &&

                    driver.FindElement(By.XPath("//*[@id='lblFilterMsg']")).Text.Equals(numberoffilteredmsgs) &&

                    driver.FindElement(By.XPath("//*[@id='lblCommand']")).Text.Equals(alertcommand) &&

                    driver.FindElement(By.XPath("//*[@id='lblIdleMsg']")).Text.Equals(idlemsgtime))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   System_Attendant_Settings Testcase Passed    ^^^^^^^^^^^^^^^^^^^^^");
                

            }

            else
            {
                Assert.Fail("System_Attendant_Settings Testcase Failed");
            }

        }

        [Test]
        public void b_Email_Gateway_Settings()
        {
            string type = "SMTP";
        //    string hiplink_url = @"http://192.168.4.237:8000/cgi-bin/no_action.exe";
        //    string email_spool_directory = @"C:\Program Files (x86)\HipLink Software\HipLink\test_email_spool";
        //    string server_ip_address = "192.168.5.184";
        //    string server_port = "1337";
            string path_external_script = "/test/hiplink/5.0";
            string one_way_email = "scenario1@email.com";
            string two_way_email = "two-way@email.com";
            string pop_server = "test_pop";
            string pop_port = "8080";
            string pop_one_acc = "popone@account.com";
            string pop_one_pwd = "123";
            string pop_two_acc = "poptwo@account.com";
            string pop_two_pwd = "123";
            string standard_send_pattern = "p1";
           
            check_driver_type(driver_type, "settings", "Email Gateway", "Settings");

          //  Assert.AreEqual("Email Gateway", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            string[] lines_local = read_from_file("email_gateway_settings"); // return all the data in the form of array

            string hiplink_url = lines_local[0];
            string email_spool_directory = lines_local[1];
            string server_ip_address = lines_local[2];
            string server_port = lines_local[3];

            driver.FindElement(By.Id("btnedit")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
            driver.FindElement(By.XPath("//li[text()='SMTP']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("txturl")).Clear();
            driver.FindElement(By.Id("txturl")).SendKeys(hiplink_url);

            driver.FindElement(By.Id("txtspooldir")).Clear();
            driver.FindElement(By.Id("txtspooldir")).SendKeys(email_spool_directory);

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();

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

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();
            Thread.Sleep(2000);
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='POP']/div/div[2]/div[1]/fieldset[2]/div/ul")).Text + "*");
            
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='POP']/div/div[2]/div[1]/fieldset[2]/div/ul/li")).Text + "*");

            driver.FindElement(By.Id("txtPopOneAccount")).Clear();
            driver.FindElement(By.Id("txtPopOneAccount")).SendKeys(pop_one_acc);

            driver.FindElement(By.Id("txtPopOnePassword")).Clear();
            driver.FindElement(By.Id("txtPopOnePassword")).SendKeys(pop_one_pwd);

            driver.FindElement(By.Id("txtPopTwoAccount")).Clear();
            driver.FindElement(By.Id("txtPopTwoAccount")).SendKeys(pop_two_acc);

            driver.FindElement(By.Id("txtPoptwoPassword")).Clear();
            driver.FindElement(By.Id("txtPoptwoPassword")).SendKeys(pop_two_pwd);

            driver.FindElement(By.XPath("(//li[text()='Add New'])[1]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@class='viewport c_list_scroll_height']/div/div/fieldset/input")).Click();
            driver.FindElement(By.XPath("//div[@class='viewport c_list_scroll_height']/div/div/fieldset/input")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@class='viewport c_list_scroll_height']/div/div/fieldset/input")).SendKeys(standard_send_pattern);
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//li[text()='Add New'])[2]")).Click();
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
        public void c_Backup_Settings_Panel()
        {
          //  string backup_dir = @"C:\Program Files (x86)\Hiplink Software\HipLink\backup";
            string backup_keep_days = "05";
            string backup_start_time = "05:06";
            string backup_interval = "02";

            check_driver_type(driver_type, "administration", "Backup Service", "Sys Admin");

            Assert.AreEqual("Backup Service", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            string[] lines_local = read_from_file("backup_settings"); // return all the data in the form of array

            string backup_dir = lines_local[0];

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

            driver.FindElement(By.Id("btnbackup")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//*[@id='tab_backupservice']/a")).Click();
            Thread.Sleep(2000);

            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblDirectory']")).Text + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblBackupDays']")).Text + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblStartTime']")).Text + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//*[@id='lblBackupHours']")).Text + "*");

            Thread.Sleep(2000);

            if (driver.FindElement(By.XPath("//*[@id='lblDirectory']")).Text.Equals(backup_dir) &&

                    driver.FindElement(By.XPath("//*[@id='lblBackupDays']")).Text.Contains("5") &&

                driver.FindElement(By.XPath("//*[@id='lblStartTime']")).Text.Contains("5:6") &&

                    driver.FindElement(By.XPath("//*[@id='lblBackupHours']")).Text.Contains("2"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Backup Settings Passsed...   ^^^^^^^^^^^^^^^^^^^^^");
                // driver.FindElement(By.XPath("//*[@id='logout']")).Click();

            }

            else
            {
                Assert.Fail("Backup_settings_panel Testcase Failed");
            }

        }


        [Test]
        public void d_Snpp_Gateway()
        {
            string port = "8080";
            string timeout = "10";
            string current_radiobtn_txt;
            string NoAuth = "No Authentication: Allow incoming connections from any IP address";
            string CLI_USER = "CLI User Authentication: Each incoming connection will be authenticated against a CLI user's IP address";

            check_driver_type(driver_type, "settings", "SNPP Gateway", "Settings");

            Assert.AreEqual("SNPP Gateway", driver.FindElement(By.XPath("//div[@id='main']/h1")).Text);

            current_radiobtn_txt = driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.ToString();

            Console.WriteLine("*" + current_radiobtn_txt + "*");

            if (driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.Equals(NoAuth))
            {
                driver.FindElement(By.Id("btnedit")).Click();

                driver.FindElement(By.Id("txtport")).Clear();

                driver.FindElement(By.Id("txtport")).SendKeys(port);

                driver.FindElement(By.CssSelector("a.selector")).Click();
                Thread.Sleep(2000);

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


                if (driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.Equals(CLI_USER) &&

                    driver.FindElement(By.Id("lblPort")).Text.Equals(port) &&

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

                if (driver.FindElement(By.XPath("//*[@id='lblAuth']")).Text.Equals(NoAuth) &&

                    driver.FindElement(By.Id("lblPort")).Text.Equals(port) &&

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
        public void e_Add_Tap_Gateway()
        {
            string port = "1452";
            string Initial_String = "Initial String";
            string answer_string = "Auto Answer String";

            check_driver_type(driver_type, "settings", "TAP Gateway", "Settings");

            Assert.AreEqual("TAP Gateway", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add TAP Gateway")).Click();

            driver.FindElement(By.Id("txtSerialPort")).Clear();

            driver.FindElement(By.Id("txtSerialPort")).SendKeys(port);

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//li[(contains(text(),'1200'))]")).Click();
            Thread.Sleep(1000);
            //driver.FindElement(By.XPath("//ul[@id='sizzle-1396510222684']/li[2]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//li[(contains(text(),'Even'))]")).Click();
            Thread.Sleep(1000);
            //driver.FindElement(By.XPath("//ul[@id='sizzle-1396510222684']/li[2]")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();

            driver.FindElement(By.XPath(".//*[@id='tapGatewayLightbox']/div[3]/div[2]/fieldset[3]/div/ul/li[3]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();

            driver.FindElement(By.XPath(".//*[@id='tapGatewayLightbox']/div[3]/div[2]/fieldset[4]/div/ul/li[3]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();

            driver.FindElement(By.XPath("//li[(contains(text(),'Hardware'))]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[6]")).Click();

            driver.FindElement(By.XPath("//li[(contains(text(),'2 sec'))]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[7]")).Click();

            driver.FindElement(By.XPath("//li[(contains(text(),'30 minutes'))]")).Click();

            driver.FindElement(By.XPath("//span[contains(text(),'Leased Line')]")).Click();

            //   Console.WriteLine(driver.FindElement(By.XPath("//span[contains(text(),'Leased Line')]")));
            //   driver.FindElement(By.XPath("//input[(@id='chkLeasedLine')]")).Click();
            //   driver.FindElement(By.CssSelector("input[id='chkLeasedLine']")).Click();
            Thread.Sleep(1000);
            Console.WriteLine(driver.FindElement(By.Id("txtIntString")).Enabled);

            if (driver.FindElement(By.Id("txtIntString")).Enabled == false)
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
        public void f_File_System_Interface()
        {
         //   string hiplink_url = "http://192.168.4.237:8000/cgi-bin/no_action.exe";
         //   string spool_dir = @"C:\Program Files (x86)\HipLink Software\Hiplink\test";
         //   string bulk_spool_dir = @"C:\Program Files (x86)\HipLink Software\Hiplink\new";
            string bulk_message_recipient_pattern = "^.*<Receiver:(.*)>.*$";
            string bulk_message_pattern = "^.*>(.*)$";
            string bulk_message_file_pattern = "*";

            string[] lines_local = read_from_file("file_system_interface_settings"); // return all the data in the form of array

            string hiplink_url = lines_local[0];
            string spool_dir = lines_local[1];
            string bulk_spool_dir = lines_local[2];

            if (!Directory.Exists(spool_dir))
            {
                Directory.CreateDirectory(spool_dir);
            }

            if (!Directory.Exists(bulk_spool_dir))
            {
                Directory.CreateDirectory(bulk_spool_dir);
            }

            check_driver_type(driver_type, "settings", "File System Interface", "Settings");

            Assert.AreEqual("File System Interface", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

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

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.XPath("//li[text()='34']")).Click();
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

                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();
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

                if (!(driver.FindElement(By.XPath(".//*[@id='lblUrl']")).Text.Equals(hiplink_url)) &&

                    driver.FindElement(By.XPath(".//*[@id='lblSpoolDir']")).Text.Equals(spool_dir) &&

                    driver.FindElement(By.XPath(".//*[@id='lblDirChk']")).Text.Equals("34") &&

                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsg']")).Text.Equals("Enabled") &&

                    driver.FindElement(By.XPath(".//*[@id='lblBulkSpoolDir']")).Text.Equals(bulk_spool_dir) &&

                    driver.FindElement(By.XPath(".//*[@id='lblBullFilePatt']")).Text.Equals(bulk_message_file_pattern) &&

                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgRec']")).Text.Equals(bulk_message_recipient_pattern))/* &&

                    driver.FindElement(By.XPath(".//*[@id='lblBulkMsgPatt']")).Text.Equals(bulk_message_pattern)))*/
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

                    driver.FindElement(By.XPath(".//*[@id='lblDirChk']")).Text.Equals("34") &&

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
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   File System Interface Passed ...when Enable Bulk Message Processing was unchecked   ^^^^^^^^^^^^^^^^^^^^^");
                    // driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }

            }

        }


        [Test]
        public void g_Add_Feedback()
        {
            check_driver_type(driver_type, "settings", "Feedback", "Settings");

            Assert.AreEqual("Feedback", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);
            //hover mouse on msg backedup

            Actions feedback_action = new Actions(driver);
           
            feedback_action.MoveToElement(driver.FindElement(By.Id("eventsList"))).Perform();
            
            driver.FindElement(By.XPath("//*[@id='3']")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//li[text()='Add New']")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//fieldset/div/a[2]")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.Id("btnSave")).Click();
            
            takescreenshot("Feedback_Action");

            Thread.Sleep(3000);
            
            if (!(driver.FindElement(By.XPath(".//*[@id='3']/span")).Text.Equals("1")))
            {
                Assert.Fail("Feedback Action failed...");
            }

            else
            {
                
                Console.WriteLine("Feedback Action Passed");

            }
        }

        [Test]
        public void h_Add_Schedule_Template()
        {

            string schedule_template_name = "Schedule_Template_Monthly";

            check_driver_type(driver_type, "settings", "Schedule Template", "Settings");

            Assert.AreEqual("Schedule Templates", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Schedule Template")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click(); //schedule type

            driver.FindElement(By.XPath(".//*[@id='light']/div[2]/div[2]/div/ul/li[text()='Monthly']")).Click();

            driver.FindElement(By.Id("sname")).Clear();
            driver.FindElement(By.Id("sname")).SendKeys(schedule_template_name); //schedule template name

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click(); //time frame

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[1]/ul/li[text()='01']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //time frame

            driver.FindElement(By.XPath(".//*[@id='sch_main']/div[1]/fieldset[2]/div[3]/ul/li[text()='02']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("startpicker")).Click();//range start from

            driver.FindElement(By.LinkText("14")).Click();
            WaitForChrome(5000,browser_name);

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
        public void i_LDAP_Panel()
        {

            check_driver_type(driver_type, "administration", "LDAP Settings", "Sys Admin");

            Assert.AreEqual("LDAP", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            // General Setting Section

            driver.FindElement(By.XPath("(//span[text()='Enabled'])[1]")).Click();

            driver.FindElement(By.Id("syncEnabled")).Click();
            

            // Connection Parameter Section

            driver.FindElement(By.Id("connectionParameters")).Click();
            Thread.Sleep(2000);

            new SelectElement(driver.FindElement(By.Id("directoryType"))).SelectByText("Active Directory"); // select value from dropdown

            driver.FindElement(By.Id("btnGetDefaults")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnOk")).Click();


            driver.FindElement(By.Id("primaryServerHost")).Clear();
            driver.FindElement(By.Id("primaryServerHost")).SendKeys("192.168.4.7");

            driver.FindElement(By.Id("baseDn")).Clear();
            driver.FindElement(By.Id("baseDn")).SendKeys("dc=folio3pk, dc=com");

            driver.FindElement(By.Id("anonymousAccess")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("anonymousAccess")).Click();
            Thread.Sleep(2000);
            
            

            driver.FindElement(By.Id("domainName")).Clear();
            driver.FindElement(By.Id("domainName")).SendKeys("folio3pk");

            driver.FindElement(By.Id("userId")).Clear();
            driver.FindElement(By.Id("userId")).SendKeys("nakhter");

            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("rana05");

            // LDAP User Parameters Section

            driver.FindElement(By.Id("userParameters")).Click();
            Thread.Sleep(2000);

            new SelectElement(driver.FindElement(By.Id("isUserId"))).SelectByText("User Name Attribute"); // select value from dropdown

            new SelectElement(driver.FindElement(By.Id("rdnFilterOnly"))).SelectByText("User RDN Template(s)"); // select value from dropdown
            
            driver.FindElement(By.Id("userEmailAttribute")).Clear();
            driver.FindElement(By.Id("userEmailAttribute")).SendKeys("mail");

            driver.FindElement(By.Id("userNameAttribute")).Clear();
            driver.FindElement(By.Id("userNameAttribute")).SendKeys("sAMAccountName");

            driver.FindElement(By.Id("rdnName")).Clear();
            driver.FindElement(By.Id("rdnName")).SendKeys("CN=?,CN=Users");

            driver.FindElement(By.Id("rdnFilter")).Clear();
            driver.FindElement(By.Id("rdnFilter")).SendKeys("(objectClass=user)");

            
            // LDAP Group Parameters Section
            
            driver.FindElement(By.Id("groupParameters")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("autoUser")).Click();

            driver.FindElement(By.Id("groupNameAttribute")).Clear();

            driver.FindElement(By.Id("groupNameAttribute")).SendKeys("sAMAccountName");

            driver.FindElement(By.Id("memberAttribute")).Clear();

            driver.FindElement(By.Id("memberAttribute")).SendKeys("member");
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//*[@id='rdnName'])[2]")).Clear();
            driver.FindElement(By.XPath("(//*[@id='rdnName'])[2]")).SendKeys("CN=?,CN=Users");


            driver.FindElement(By.XPath("(//*[@id='rdnFilter'])[3]")).Clear();
            driver.FindElement(By.XPath("(//*[@id='rdnFilter'])[3]")).SendKeys("(objectClass=group)");
           


           // driver.FindElement(By.XPath("//li[text()='Add New']")).Click();

            // LDAP Group Mappings

            driver.FindElement(By.Id("groupMappings")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("selectAll")).Click();

            new SelectElement(driver.FindElement(By.Id("mbox_listB"))).SelectByText("sysAdmin");

            driver.FindElement(By.Id("mbox_mapping_add")).Click();

            // Receiver Parameters

            driver.FindElement(By.Id("receiverParameters")).Click();
            
            driver.FindElement(By.Id("recEnabled")).Click();

            driver.FindElement(By.Id("recNameAttribute")).Clear();
            driver.FindElement(By.Id("recNameAttribute")).SendKeys("sAMAccountName");


            driver.FindElement(By.Id("recEmailAttribute")).Clear();
            driver.FindElement(By.Id("recEmailAttribute")).SendKeys("mail");
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//*[@id='rdnName'])[3]")).Clear();
            driver.FindElement(By.XPath("(//*[@id='rdnName'])[3]")).SendKeys("CN=?,CN=Users");


            driver.FindElement(By.XPath("(//*[@id='rdnFilter'])[4]")).Clear();
            driver.FindElement(By.XPath("(//*[@id='rdnFilter'])[4]")).SendKeys("(objectClass=user)");

            // Receiver Carrier Mapping

            driver.FindElement(By.Id("receiverCarrierMappings")).Click();
            Thread.Sleep(2000);

            new SelectElement(driver.FindElement(By.Id("carrierLdapMode"))).SelectByText("Manual");
            
            driver.FindElement(By.Id("carrierPinName")).SendKeys("mail");

            new SelectElement(driver.FindElement(By.XPath("(//*[@id='mbox_listB'])[2]"))).SelectByText("smtp_carrier");
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//*[@id='selectAll'])[2]")).Click();
            Thread.Sleep(4000);

            driver.FindElement(By.XPath("(//*[@id='mbox_mapping_add'])[2]")).Click();
            Thread.Sleep(4000);

            // Receiver Department Mapping

            driver.FindElement(By.Id("receiverDepartmentMappings")).Click();
            Thread.Sleep(2000);

            new SelectElement(driver.FindElement(By.Id("departLdapMode"))).SelectByText("Manual");

            new SelectElement(driver.FindElement(By.XPath("(//*[@id='mbox_listB'])[3]"))).SelectByText("Default");
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//*[@id='selectAll'])[4]")).Click();
            Thread.Sleep(4000);

            driver.FindElement(By.XPath("(//*[@id='mbox_mapping_add'])[3]")).Click();
            Thread.Sleep(4000);

            driver.FindElement(By.Id("btnSave")).Click();
            Thread.Sleep(2000);

        }


        [Test]
        public void j_Filter_Panel()
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


            /* Adding API Filter */

            WaitForChrome(5000,browser_name);
            check_driver_type(driver_type, "administration", "Filters", "Sys Admin");

            Assert.AreEqual("Filters Panel", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            Actions action = new Actions(driver);
            action.MoveToElement(driver.FindElement(By.XPath("//i[text()='Add Filter']"))).Perform();
            Thread.Sleep(2000);

            driver.FindElement(By.Id(("api"))).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("name")).Clear();
            
            driver.FindElement(By.Id("name")).SendKeys("API Filter");
            
            driver.FindElement(By.Id("description")).Clear();
            
            driver.FindElement(By.Id("description")).SendKeys("This is api filter");
            
            driver.FindElement(By.Id("messageCount")).Clear();
            
            driver.FindElement(By.Id("messageCount")).SendKeys("2");
            
            driver.FindElement(By.Id("actionInterval")).Clear();
            
            driver.FindElement(By.Id("actionInterval")).SendKeys("1");
            
            driver.FindElement(By.Id("btnSave")).Click();
            Thread.Sleep(1000);

            /* Adding Message Filter */

            action.MoveToElement(driver.FindElement(By.XPath("//i[text()='Add Filter']"))).Perform();
            Thread.Sleep(2000);

            driver.FindElement(By.Id(("message"))).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("name")).SendKeys("Message Filter");
            
            driver.FindElement(By.Id("description")).Clear();
            
            driver.FindElement(By.Id("description")).SendKeys("This is message filter");
            
            driver.FindElement(By.XPath("//div/ul/li[text()='Add New']")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.Id("equalText")).Clear();
            
            driver.FindElement(By.Id("equalText")).SendKeys("Test");
            
            driver.FindElement(By.XPath("(//a[@class='selector'])[5]")).Click();
            
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//li[text()='Suppress']")).Click();
            
            driver.FindElement(By.Id("btnSave")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.Id("btnOk")).Click();

            /*  ---------------------------------------------------------------- */
        }


        public void check_driver_type(string drivertype, string id_name, string link_text, string a_text) //drivertype= browser , id_name = landing page , link_text = panel(e.g Add user page) 
        {

            Thread.Sleep(3000);

            if (drivertype.ToString() == "OpenQA.Selenium.Safari.SafariDriver") //for safari
            {

                Console.WriteLine("if clause ....");
                Thread.Sleep(3000);

                driver.FindElement(By.XPath(".//*[@id='" + id_name + "']/a")).Click(); //goto landing for particular ID
                Thread.Sleep(3000);

               

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='" + link_text + "']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(3000);

                

            }

            else if (drivertype.ToString() == "OpenQA.Selenium.Chrome.ChromeDriver" || drivertype.ToString() == "OpenQA.Selenium.Firefox.FirefoxDriver") //for firefox and chrome
            {

                Console.WriteLine("using hover func() ....");
                Thread.Sleep(3000);

                //a[contains(text(),'On-Duty')])[2]

                driver.FindElement(By.XPath("//li[@id='" + id_name + "']/a[text()='" + a_text + "']")).Click(); //goto landing for particular ID
                Thread.Sleep(3000);



                Actions a1c = new Actions(driver);
                Thread.Sleep(3000);

                a1c.MoveToElement(driver.FindElement(By.XPath("//div[@class='footer']"))).Perform();
                Thread.Sleep(3000);

                

                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='" + link_text + "']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(3000);

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
                Thread.Sleep(3000);

                hover_func(id_name, link_text, a_text);
                Thread.Sleep(3000);

            }

            else // for IE
            {

                // drivertype.ToString() == "OpenQA.Selenium.IE.InternetExplorerDriver"

               

                hover_func(id_name, link_text, a_text);
                Thread.Sleep(3000);
            }

        }



        public void hover_func(string id_name, string link_text, string a_text)
        {

            //------ Hover functionality and click ------

            var hoveritem = driver.FindElement(By.Id(id_name));

            Actions action1 = new Actions(driver); //simply my webdriver
            Thread.Sleep(3000);

            action1.MoveToElement(hoveritem).Perform(); //move to list element that needs to be hovered

            Thread.Sleep(3000);

            driver.FindElement(By.XPath("(//a[text()='" + link_text + "'])[1]")).Click();
            Thread.Sleep(3000);


            //------ Focus out the mouse to disappear hovered dialog ------

            Actions action2 = new Actions(driver);
            Thread.Sleep(3000);

            action2.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(3000);


        }

        public static void WaitForElementToExist(string ID, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
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

        public string[] read_from_file(string file_name)
        {
            // Read each line of the file into a string array. Each element 
            // of the array is one line of the file. 

            string[] lines = System.IO.File.ReadAllLines(@".\"+file_name+".txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of "+file_name+".txt = ");
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
                driver.FindElement(By.XPath("//a[text()='Logout']")).Click();
                Thread.Sleep(2000);

                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            //  Assert.AreEqual("", verificationErrors.ToString());
        }
    }
}

