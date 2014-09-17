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
    public class g_Smoke_Logs_Queues_Reports : HL_Base_Class
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

        string user_label;

        string trimmed_user_label;

        string create_directory_path = @".\Screenshots_Logs_Testcase";

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

            login_name = lines_local[0]; //used in login and session manager testcases

            login_pwd = lines_local[1];

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

            Assert.AreEqual(trimmed_user_label, "Welcome fahad");

            verificationErrors = new StringBuilder();

        }


        [Test]
        public void a_Logs_Settings()
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

            if (driver_type.ToString() == "OpenQA.Selenium.Firefox.FirefoxDriver")
            {
                driver.FindElement(By.XPath("(//a[@class='row_action_edit'])[2]")).Click();
                Thread.Sleep(2000);

                if (driver.FindElement(By.Id("logSettingLightbox")).Displayed)
                {
                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

                    driver.FindElement(By.XPath("//li[text()='"+log_level+"']")).Click();

                    driver.FindElement(By.Id("lnkSave")).Click();

                }
                else
                {
                    Assert.Fail("Logs Settings Failed");
                }
                
            }

            else

            {

                driver.FindElement(By.Id("divGrid_selectAllRows")).Click();  // Select all/Deselect all checkbox

                driver.FindElement(By.Id("selChangeLevel")).Click();
                Thread.Sleep(2000);

                SelectElement se = new SelectElement(driver.FindElement(By.Id("selChangeLevel")));
                se.SelectByText(log_level);

                // new SelectElement(driver.FindElement(By.Id("selChangeLevel"))).SelectByText(log_level); // select value from dropdown

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
        
        }


        [Test]
        public void b_Logs_View()
        {

            check_driver_type(driver_type, "administration", "Logs", "Sys Admin");

            Assert.AreEqual("Logs", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);

            // Main logs

            driver.FindElement(By.XPath("(//a[text()='Main'])[1]")).Click();
            Thread.Sleep(3000);

            if (driver.FindElement(By.Id("logHeader")).Text.Equals("Main"))
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^   Main Logs Passed... ^^^^^^^^^^^^^^^^");

                takescreenshot("Main_logs");

                driver.Navigate().Back();

            }
            else
            {
                Assert.Fail("Main Logs Failed...");
            }

            /*
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
             */ 

        }


        [Test]
        public void c_Queues_Panel()
        {
            string receiver_name = "receiver_smtp";
            string new_dir = "New_directory";

            ICollection<string> windowids = null;
            IEnumerator<String> iter = null;
            String mainWindowId = null;
            String curWindow = null;

            Thread.Sleep(2000);
            driver.FindElement(By.XPath(".//*[@id='queues']/a")).Click();

            Thread.Sleep(2000);
            driver.FindElement(By.LinkText(new_dir)).Click();

            Assert.AreEqual(new_dir, driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text); //verifying Queues Panel label

            takescreenshot("New_directory");

            if (driver.FindElement(By.Id("testing")).Text.Contains(new_dir) && driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(receiver_name))
            {

                takescreenshot("Queues_View_Message_Details");

                Console.WriteLine(driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text.Contains("Queues - View Message Details"));

                Thread.Sleep(1000);

             /*   mainWindowId = driver.CurrentWindowHandle;
                Console.WriteLine("Main window handle: " + mainWindowId);//main window id

                // The below step would use whatever element you need to use to 
                // open the new window. 

                driver.FindElement(By.XPath("(//ul[@class='row_grid_actions']/li[1]/a)[1]")).Click(); //click on edit button to view the message file
                Thread.Sleep(3000);


                //    Assert.AreEqual("Help", driver.FindElement(By.XPath("//div[@class='main_container pg_help']/h1")).Text);

                //the above should open a new tab on current browser window BUT Selenium will open it as a new browser window

                Thread.Sleep(25);

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

                      //  Console.WriteLine(driver.FindElement(By.XPath("//div[@class='main_container pg_help']")).Text);
                      //  Console.WriteLine(driver.FindElement(By.XPath("//li[@class='product_name']")).Text);


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
*/
        
            }
            else
            {
                Assert.Fail("Queues_Panel_Failed");
            }

        }


        [Test]
        public void d_Reports_Panel()
        {
            string receiver_name = "receiver_smtp";
            Thread.Sleep(2000);

            hover_func("reports", "Detail", "Reports");

            Assert.AreEqual("Reports", driver.FindElement(By.XPath("//span[@id='main_title']")).Text); //verfying Reports panel label

            hover_func("reports", "Summary", "Reports");

            driver.FindElement(By.Id("send")).Click(); //navigate to send panel

            driver.FindElement(By.Id("reports")).Click();
            Thread.Sleep(2000);

            takescreenshot("Reports_Panel");

            driver.FindElement(By.XPath("(//a[text()='Summary'])[3]")).Click(); //navigate to summary tab 
            Thread.Sleep(2000);

            takescreenshot("Reports_Panel_Summary");

            driver.FindElement(By.XPath("(//a[text()='Detail'])[3]")).Click();
            Thread.Sleep(2000);

        }


        [Test]
        public void e_Statistics()
        {

            hover_func("reports", "Statistics", "Reports");

            Assert.AreEqual("Statistics", driver.FindElement(By.Id("main_title")).Text);
           
            driver.FindElement(By.Id("stats_date_filter")).Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.Id("stats_start_date")).Click();
           
            driver.FindElement(By.LinkText("Traffic by Messengers")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.LinkText("Traffic by Time")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.XPath("//*[@id='tab_summary']/a")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.LinkText("Passed")).Click();
            
            driver.FindElement(By.Id("stats_export")).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.Id("btnOk")).Click();
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
    }
}


