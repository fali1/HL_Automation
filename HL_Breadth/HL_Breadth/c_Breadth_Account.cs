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
    public class c_Breadth_Account : HL_Base_Class
    {

   //     private IWebDriver driver;

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
        public void a_Add_Edit_Delete_Timezone()
        {

            string timezone_name_karachi = "Karachi Timezon";
            string timezone_name_karachi_2 = "New Karachi Timezon";
            string timezone_name_karachi_2_edited = "edited new Karachi Timezon";

            string timezone_desc = "Karachi Timezone description";
            string timezone_desc_2 = "Karachi Timezone description new";
            string timezone_desc_2_edited = "Karachi Timezone description new Edited";

            string offset = "5 Hours";
            string offset_digit = "5";

            check_driver_type(driver_type, "administration", "Time Zone Settings", "Sys Admin");

            Assert.AreEqual("Time Zone Setting", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Time Zone")).Click();

            driver.FindElement(By.Id("txtName")).Clear();

            driver.FindElement(By.Id("txtName")).SendKeys(timezone_name_karachi);

            driver.FindElement(By.Id("txtDesc")).Clear();

            driver.FindElement(By.Id("txtDesc")).SendKeys(timezone_desc);

            driver.FindElement(By.XPath("//a[@class='selector']")).Click();
            Thread.Sleep(2000);

            string path1 = "//li[(text()='";
            string path2 = "')]";

            driver.FindElement(By.XPath(path1 + offset + path2)).Click();

            //driver.FindElement(By.XPath("//li[(text()='5 Hours')]")).Click();  -----Xpath as it is

            driver.FindElement(By.Id("btnSaveTimeBox")).Click();
            Thread.Sleep(2000);

            takescreenshot("Timezone");

            Console.WriteLine(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text);

            //driver.FindElement(By.XPath("//div[@title='Karachi Timezone']"))
            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(timezone_name_karachi) &&
                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(timezone_desc) &&
                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(offset_digit)))
            {
                takescreenshot("Timezone_Failed");
                Assert.Fail("Timezone Failed...");
            }

            else
            {
                // ADDED SECOND RECORD


                driver.FindElement(By.LinkText("Add Time Zone")).Click();

                driver.FindElement(By.Id("txtName")).Clear();

                driver.FindElement(By.Id("txtName")).SendKeys(timezone_name_karachi_2);

                driver.FindElement(By.Id("txtDesc")).Clear();

                driver.FindElement(By.Id("txtDesc")).SendKeys(timezone_desc_2);

                driver.FindElement(By.XPath("//a[@class='selector']")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath(path1 + offset + path2)).Click();
                //driver.FindElement(By.XPath("//li[(text()='5 Hours')]")).Click();  -----Xpath as it is

                driver.FindElement(By.Id("btnSaveTimeBox")).Click();
                Thread.Sleep(2000);


                // VERIFYING SECOND RECORD

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(timezone_name_karachi_2)))
                {

                    takescreenshot("Timezone_Failed");

                    Assert.Fail("Timezone Failed...");

                }

                else
                {
                    // EDITING SECOND RECORD

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();

                    driver.FindElement(By.Id("txtName")).Clear();

                    driver.FindElement(By.Id("txtName")).SendKeys(timezone_name_karachi_2_edited);

                    driver.FindElement(By.Id("txtDesc")).Clear();

                    driver.FindElement(By.Id("txtDesc")).SendKeys(timezone_desc_2_edited);

                    driver.FindElement(By.Id("btnSaveTimeBox")).Click();
                    Thread.Sleep(2000);


                    // VERIFYING EDITED SECOND RECORD

                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(timezone_name_karachi_2_edited)))
                    {
                        takescreenshot("Timezone_Edited_Failed");

                        Assert.Fail("Timezone Edited Failed...");
                    }
                    else
                    {
                        // DELETING SECOND RECORD

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);


                        // VERIFYING DELETED SECOND RECORD

                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(timezone_name_karachi_2_edited)))
                        {

                            takescreenshot("Timezone_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Timezone Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }

                        else
                        {

                            takescreenshot("Timezone_Deleted_Failed");

                            Assert.Fail("Timezone Deleted Failed...");


                        }


                    }

                }
            }
        }



        [Test]
        public void b_Add_Edit_Delete_Message_Template()
        {
            string message_template_name = "Message Template";
            string message_template_name_2 = "New Message Template";
            string message_template_name_2_edited = "edited Message Template";


            string message_template_description = "message template description";
            string message_template_description_2 = "message new template description";
            string message_template_description_2_edited = "message new template description Edited";


            check_driver_type(driver_type, "settings", "Message Template", "Settings");

            Assert.AreEqual("Message Templates", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Template")).Click();

            driver.FindElement(By.Id("txtTmpltName")).Clear();
            driver.FindElement(By.Id("txtTmpltName")).SendKeys(message_template_name); //message template name

            driver.FindElement(By.Id("txtAdesc")).Clear();
            driver.FindElement(By.Id("txtAdesc")).SendKeys(message_template_description); //message template description

            //   driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[1]")).Click();
            Thread.Sleep(2000);


            /*
                // driver will scrol down the screen untill the required element is in view... 

                IWebElement element = driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[contains(text(),'" + receiver_name + "')]"));
                ((IJavaScriptExecutor)driver).ExecuteScript("var $myLi = $(arguments[0]), $myParent = $myLi.parents('.common_scrollbar'); $myParent.mCustomScrollbar('scrollTo', '#' + $myLi.attr('id'));", element);
                Thread.Sleep(2000);
             */

            Thread.Sleep(2000);

            //     driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[contains(text(),'" + receiver_name + "')]")).Click(); //Assign Templates To Recipients list box
            driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[1]")).Click(); //Assign Templates To Recipients list box    
            Thread.Sleep(2000);

            driver.FindElement(By.Id("recipientMoveRight")).Click();
            Thread.Sleep(2000);

            /*       driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

                   driver.FindElement(By.XPath("//li[text()='Quick Send']")).Click();

                   driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();

                   driver.FindElement(By.XPath("//li[text()='Confidential']")).Click();

                   driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();

                   driver.FindElement(By.XPath("//li[text()='Important']")).Click();
             */

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

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

            driver.FindElement(By.XPath("//li[text()='Primary Send']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();

            driver.FindElement(By.XPath("//li[text()='Normal']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();

            driver.FindElement(By.XPath("//li[text()='Important']")).Click();

            driver.FindElement(By.Id("btnAddResp")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("txtResponseText")).Clear();
            driver.FindElement(By.Id("txtResponseText")).SendKeys("ra1");

            driver.FindElement(By.Id("respSave")).Click();
            WaitForChrome(5000,browser_name);

            driver.FindElement(By.Id("btnSaveTemp")).Click();
            WaitForChrome(5000, browser_name);
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnCancel")).Click();
            Thread.Sleep(1000);

            takescreenshot("Message_Template");
            Thread.Sleep(2000);


            Console.WriteLine(driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div[1]/div[1]/div[1]/div[2]")).Text);

            if (!(driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div[1]/div[1]/div[1]/div[2]")).Text.Contains(message_template_name)))
            {

                takescreenshot("Message_Template_Failed");

                Assert.Fail("Message_Template Failed ...");

                /*
                takescreenshot("Message_Template_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^ Message_Template Passed ... ^^^^^^^^^^^^^^^");
                 */
            }
            else
            {

                // ADDED SECOND RECORD


                driver.FindElement(By.LinkText("Add Template")).Click();

                driver.FindElement(By.Id("txtTmpltName")).Clear();
                driver.FindElement(By.Id("txtTmpltName")).SendKeys(message_template_name_2); //message template name


                driver.FindElement(By.Id("txtAdesc")).Clear();

                driver.FindElement(By.Id("txtAdesc")).SendKeys(message_template_description_2); //message template description

                Thread.Sleep(2000);


                driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[1]")).Click(); //Assign Templates To Recipients list box    

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


                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();


                driver.FindElement(By.XPath("//li[text()='Primary Send']")).Click();


                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();


                driver.FindElement(By.XPath("//li[text()='Normal']")).Click();


                driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();


                driver.FindElement(By.XPath("//li[text()='Important']")).Click();


                driver.FindElement(By.Id("btnAddResp")).Click();

                Thread.Sleep(2000);


                driver.FindElement(By.Id("txtResponseText")).Clear();

                driver.FindElement(By.Id("txtResponseText")).SendKeys("ra1");


                driver.FindElement(By.Id("respSave")).Click();


                driver.FindElement(By.Id("btnSaveTemp")).Click();

                Thread.Sleep(1000);


                driver.FindElement(By.Id("btnCancel")).Click();

                Thread.Sleep(1000);


                // VERIFYING SECOND RECORD

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(message_template_name_2)))
                {

                    takescreenshot("Message_Template_Failed");

                    Assert.Fail("Message_Template Failed...");

                }

                else
                {
                    // EDITING SECOND RECORD

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[2]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtTmpltName")).Clear();
                    driver.FindElement(By.Id("txtTmpltName")).SendKeys(message_template_name_2_edited); //message template name

                    driver.FindElement(By.Id("txtAdesc")).Clear();
                    driver.FindElement(By.Id("txtAdesc")).SendKeys(message_template_description_2_edited); //message template description
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("btnSelectField")).Click(); //Drop down of Create Template Content
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath(".//*[@id='selectTempBox']/form/div[@id='comboDiv']/div/div[2]/div/div/div/div/div/fieldset/input")).SendKeys("1");

                    driver.FindElement(By.XPath("//li[text()='Add New']")).Click();

                    driver.FindElement(By.XPath(".//*[@id='selectTempBox']/form/div[@id='comboDiv']/div/div[2]/div/div/div/div/div[2]/fieldset/input")).SendKeys("2");

                    driver.FindElement(By.Id("btnSelOk")).Click();

                    driver.FindElement(By.Id("btnAddResp")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtResponseText")).Clear();

                    driver.FindElement(By.Id("txtResponseText")).SendKeys("ra2");

                    driver.FindElement(By.Id("respSave")).Click();

                    driver.FindElement(By.Id("btnSaveTemp")).Click();
                    Thread.Sleep(1000);

                    driver.FindElement(By.Id("btnCancel")).Click();
                    Thread.Sleep(1000);


                    // VERIFYING EDITED SECOND RECORD

                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(message_template_name_2_edited)))
                    {
                        takescreenshot("Message_Template_Edited_Failed");

                        Assert.Fail("Message_Template Edited Failed...");
                    }
                    else
                    {
                        // DELETING SECOND RECORD

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);


                        // VERIFYING DELETED SECOND RECORD

                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(message_template_name_2_edited)))
                        {

                            takescreenshot("Message_Template_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Message_Template Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }

                        else
                        {

                            takescreenshot("Message_Template_Deleted_Failed");

                            Assert.Fail("Message_Template Deleted Failed...");


                        }

                    }

                }

            }
        
        }



        [Test]
        public void c_Add_Edit_Delete_Response_Action()
        {
            string response_action_name = "Reply_Response_Action";
            string response_action_name_2 = "New Reply_Response_Action";
            string response_action_name_2_edited = "edited Reply_Response_Action";


            string response_action_description = "Reply Response Action description";
            string response_action_description_2 = "Reply Response Action new description";
            string response_action_description_2_edited = "Reply Response Action new description Edited";
            
            string command_line_statement = ("echo \"fahad\"");
            string command_line_statement_edited = ("echo \"fahad , its edited statment\"");

            check_driver_type(driver_type, "settings", "Response Actions", "Settings");

            Assert.AreEqual("Response Actions", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("Add Response Action")).Click();

            driver.FindElement(By.Id("txtname")).Clear();
            driver.FindElement(By.Id("txtname")).SendKeys(response_action_name);

            driver.FindElement(By.Id("txtdescription")).Clear();
            driver.FindElement(By.Id("txtdescription")).SendKeys(response_action_description);
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//label[@for='rdReply']")).Click(); // Reply type radio button
            Thread.Sleep(2000);

            /*   driver.FindElement(By.XPath("//label[@for='rdstandard']")).Click();

               driver.FindElement(By.XPath("//input[@class='text_name']")).Clear();
               driver.FindElement(By.XPath("//input[@class='text_name']")).SendKeys("Parameter1");
               Thread.Sleep(1000);

               driver.FindElement(By.XPath("//input[@class='text_length']")).Clear();
               driver.FindElement(By.XPath("//input[@class='text_length']")).SendKeys("3");
               Thread.Sleep(1000);

               driver.FindElement(By.XPath("//input[@class='text_default']")).Clear();
               driver.FindElement(By.XPath("//input[@class='text_default']")).SendKeys("yes");
               Thread.Sleep(2000);

               driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();
               driver.FindElement(By.XPath("//li[text()='Yes']")).Click();
               Thread.Sleep(2000);

             */

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();  // Actions dropdown
            driver.FindElement(By.XPath("//li[text()='Command Line']")).Click();

            driver.FindElement(By.Id("txtAcommand")).Clear();
            driver.FindElement(By.Id("txtAcommand")).SendKeys(command_line_statement); // Commands textbox

            driver.FindElement(By.Id("saveResponse")).Click();

            takescreenshot("Response_Action");
            Thread.Sleep(2000);


            

            if (!(driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div/div[1]/div/div[3]")).Text.Contains(response_action_name)))
            {
                takescreenshot("Response_Action_Failed");

                Console.WriteLine("^^^^^^^^^^^^^^^ Response_Action Failed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                // ADDED SECOND RECORD

                driver.FindElement(By.LinkText("Add Response Action")).Click();

                driver.FindElement(By.Id("txtname")).Clear();
                driver.FindElement(By.Id("txtname")).SendKeys(response_action_name_2);

                driver.FindElement(By.Id("txtdescription")).Clear();
                driver.FindElement(By.Id("txtdescription")).SendKeys(response_action_description_2);
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//label[@for='rdReply']")).Click(); // Reply type radio button
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();  // Actions dropdown
                driver.FindElement(By.XPath("//li[text()='Command Line']")).Click();

                driver.FindElement(By.Id("txtAcommand")).Clear();
                driver.FindElement(By.Id("txtAcommand")).SendKeys(command_line_statement); // Commands textbox

                driver.FindElement(By.Id("saveResponse")).Click();
                Thread.Sleep(2000);


                // VERIFYING SECOND RECORD

                if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(response_action_name_2)))
                {

                    takescreenshot("Response_Action_Failed");

                    Assert.Fail("Response_Action Failed...");

                }

                else
                {
                    // EDITING SECOND RECORD

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[1]")).Click();

                    driver.FindElement(By.Id("txtname")).Clear();
                    driver.FindElement(By.Id("txtname")).SendKeys(response_action_name_2_edited);

                    driver.FindElement(By.Id("txtdescription")).Clear();
                    driver.FindElement(By.Id("txtdescription")).SendKeys(response_action_description_2_edited);
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();  // Actions dropdown
                    driver.FindElement(By.XPath("//li[text()='Command Line']")).Click();

                    driver.FindElement(By.Id("txtAcommand")).Clear();
                    driver.FindElement(By.Id("txtAcommand")).SendKeys(command_line_statement_edited); // Commands textbox

                    driver.FindElement(By.Id("saveResponse")).Click();
                    Thread.Sleep(2000);

                    // VERIFYING EDITED SECOND RECORD

                    if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(response_action_name_2_edited)))
                    {
                        takescreenshot("Response_Action_Edited_Failed");

                        Assert.Fail("Response_Action Edited Failed...");
                    }
                    else
                    {
                        // DELETING SECOND RECORD

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);

                        // VERIFYING DELETED SECOND RECORD

                        if (!(driver.FindElement(By.XPath("//div[@class='mCSB_container mCS_no_scrollbar']")).Text.Contains(response_action_name_2_edited)))
                        {

                            takescreenshot("Response_Action_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete Response_Action Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }

                        else
                        {

                            takescreenshot("Response_Action_Deleted_Failed");

                            Assert.Fail("Response_Action Deleted Failed...");


                        }

                    }

                }

            }
        }



        [Test]
        public void d_Add_Edit_Delete_User_Group()
        {
            string user_group_name = "new_usergroup";
            string user_group_name_2 = "test_new_usergroup";
            string user_group_name_2_edited = "edited_new_usergroup";

            string user_group_desc = "User Group Description";
            string user_group_desc_2 = "User Group Description Test";
            string user_group_desc_2_edited = "User Group Description Edited";

            Thread.Sleep(2000);

            check_driver_type(driver_type, "settings", "User Groups", "Settings");

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
            Thread.Sleep(5000);

            driver.FindElement(By.Id("txtname")).Clear();

            driver.FindElement(By.Id("txtname")).SendKeys(user_group_name);

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

            driver.FindElement(By.XPath("//li[text()='Reports Menu']")).Click();

            driver.FindElement(By.Id("txtdesc")).Clear();

            driver.FindElement(By.Id("txtdesc")).SendKeys(user_group_name + " " + user_group_desc);

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();

            driver.FindElement(By.XPath("//li[text()='Default']")).Click();

            driver.FindElement(By.Id("btnAddUserGroup")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[2]/label")).Click(); // Department permission checkboxes

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[3]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[4]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[5]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[6]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[7]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[8]/label")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("System")).Click();                        //System tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//label[@for='selectAll_system']/span")).Click();//*[@id='systemTable']/tbody/tr[12]/td[1]/label/span
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("#tab_send > a")).Click();               //Send tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//label[@for='selectAll_send']/span")).Click();//*[@id='sendTable']/tbody/tr[8]/td[1]/label/span
            Thread.Sleep(1000);

            driver.FindElement(By.LinkText("User Group")).Click();                     //User group tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[2]/label/span")).Click();//*[@id='ug_grid']/tbody/tr[4]/td[2]/label/span

            driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[3]/label/span")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.LinkText("Response Action")).Click();                     //Response Action tab
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//label[@for='selectAll_response']/span")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("btnsave")).Click();
            Thread.Sleep(2000);

            takescreenshot("User group");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(user_group_name) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(user_group_name + " " + user_group_desc)))
            {
                takescreenshot("User_Group_Failed");
                Assert.Fail("Add User Group Failed...");
            }

            else
            {
                // ADDED SECOND RECORD

                driver.FindElement(By.LinkText("Add User Group")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.Id("txtname")).Clear();

                driver.FindElement(By.Id("txtname")).SendKeys(user_group_name_2);

                driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

                driver.FindElement(By.XPath("//li[text()='Reports Menu']")).Click();

                driver.FindElement(By.Id("txtdesc")).Clear();

                driver.FindElement(By.Id("txtdesc")).SendKeys(user_group_name_2 + " " + user_group_desc_2);

                driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();

                driver.FindElement(By.XPath("//li[text()='Default']")).Click();

                driver.FindElement(By.Id("btnAddUserGroup")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[2]/label")).Click(); // Department permission checkboxes

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[3]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[4]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[5]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[6]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[7]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[8]/label")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.LinkText("System")).Click();                        //System tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//label[@for='selectAll_system']/span")).Click();//*[@id='systemTable']/tbody/tr[12]/td[1]/label/span
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("#tab_send > a")).Click();               //Send tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//label[@for='selectAll_send']/span")).Click();//*[@id='sendTable']/tbody/tr[8]/td[1]/label/span
                Thread.Sleep(1000);

                driver.FindElement(By.LinkText("User Group")).Click();                     //User group tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[2]/label/span")).Click();//*[@id='ug_grid']/tbody/tr[4]/td[2]/label/span

                driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[3]/label/span")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.LinkText("Response Action")).Click();                     //Response Action tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//label[@for='selectAll_response']/span")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.Id("btnsave")).Click();
                Thread.Sleep(2000);


                // VERIFYING SECOND RECORD

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group_name_2)))
                {

                    takescreenshot("Second_Directory_Failed");

                    Assert.Fail("Second Directory Failed...");

                }

                else
                {
                    // EDITING SECOND RECORD

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[3]")).Click();
                    Thread.Sleep(2000);

                    driver.FindElement(By.Id("txtname")).Clear();

                    driver.FindElement(By.Id("txtname")).SendKeys(user_group_name_2_edited);

                    driver.FindElement(By.Id("txtdesc")).Clear();

                    driver.FindElement(By.Id("txtdesc")).SendKeys(user_group_name_2_edited + " " + user_group_desc_2_edited);
                    
                    driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

                    driver.FindElement(By.XPath("//li[text()='Setting Menu']")).Click();

                    driver.FindElement(By.LinkText("User Group")).Click();                     //User group tab
                    Thread.Sleep(2000);

                    driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[2]/label/span")).Click();//*[@id='ug_grid']/tbody/tr[4]/td[2]/label/span

                    driver.FindElement(By.XPath("//table[@id='ug_grid']/tbody/tr[4]/td[3]/label/span")).Click();
                    Thread.Sleep(1000);

                    driver.FindElement(By.Id("btnsave")).Click();
                    Thread.Sleep(2000);

                    // VERIFYING EDITED SECOND RECORD

                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group_name_2_edited)))
                    {
                        takescreenshot("Second_Directory_Edited_Failed");

                        Assert.Fail("Second Directory Edited Failed...");
                    }
                    else
                    {
                        // DELETING SECOND RECORD

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);


                        // VERIFYING DELETED SECOND RECORD

                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group_name_2_edited)))
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
        public void e_Add_Edit_Delete_Department()
        {
            string department_name = "new_department";
            string department_name_2 = "test_new_department";
            string department_name_2_edited = "edited_new_department";


            string department_description = "Department description";
            string department_description_2 = "Department description_2";
            string department_description_2_edited = "Department description Edited";

            check_driver_type(driver_type, "settings", "Departments", "Settings");

            Assert.AreEqual("Departments", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);


            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Add Department")).Click();

            Thread.Sleep(1000);

            driver.FindElement(By.Id("txtname")).Clear();

            driver.FindElement(By.Id("txtname")).SendKeys(department_name);

            driver.FindElement(By.Id("txtdesc")).Clear();

            driver.FindElement(By.Id("txtdesc")).SendKeys(department_description);
            //  driver.FindElement(By.XPath("//a[text()='Permission']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();

            string path1 = "//li[text()='";
            string path2 = "']";

            driver.FindElement(By.XPath(path1 + "sysOper" + path2)).Click();

            driver.FindElement(By.Id("btnUsergroup")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[2]/label")).Click(); // Default Permission checkboxes

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[3]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[4]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[5]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[6]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[7]/label")).Click();

            driver.FindElement(By.XPath("//tr[@id='footerTr']/td[8]/label")).Click();

            driver.FindElement(By.LinkText("Member")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//li[contains(text(),'receiver_smtp')]")).Click();

            driver.FindElement(By.Id("moveMemberRight")).Click();
            Thread.Sleep(2000);


            /*     driver.FindElement(By.LinkText("Guest")).Click();

                 driver.FindElement(By.XPath("//div[@id='guestTab']/div/div/fieldset/div/a[2]")).Click();
                 Thread.Sleep(2000);

                 driver.FindElement(By.XPath(".//*[@id='guestTab']/div[1]/div/fieldset/div/ul/li[text()='Default']")).Click();

                 driver.FindElement(By.XPath("//div[@id='selMemberListGuest']/div/div/div/div/ul/li[contains(text(),'Broadcast_Group3')]")).Click();

                 driver.FindElement(By.Id("moveGuestRight")).Click();
                 Thread.Sleep(2000);
             */

            driver.FindElement(By.Id("btnsave")).Click();

            takescreenshot("Department");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridNode']")).Text.Contains(department_name)))
            {
                takescreenshot("Add_Department_Failed");
                Assert.Fail("Add Department Failed...");
            }

            else
            {
                // ADDED SECOND RECORD

                driver.FindElement(By.LinkText("Add Department")).Click();

                Thread.Sleep(1000);

                driver.FindElement(By.Id("txtname")).Clear();

                driver.FindElement(By.Id("txtname")).SendKeys(department_name_2);

                driver.FindElement(By.Id("txtdesc")).Clear();

                driver.FindElement(By.Id("txtdesc")).SendKeys(department_description_2);
                //  driver.FindElement(By.XPath("//a[text()='Permission']")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();

                driver.FindElement(By.XPath(path1 + "usrSend" + path2)).Click();

                driver.FindElement(By.Id("btnUsergroup")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[2]/label")).Click(); // Default Permission checkboxes

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[3]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[4]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[5]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[6]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[7]/label")).Click();

                driver.FindElement(By.XPath("//tr[@id='footerTr']/td[8]/label")).Click();

             /*   
                driver.FindElement(By.LinkText("Member")).Click();

                driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='Default']")).Click();

                driver.FindElement(By.XPath("//li[contains(text(),'receiver_smtp')]")).Click();

                driver.FindElement(By.Id("moveMemberRight")).Click();
                Thread.Sleep(2000);


                     driver.FindElement(By.LinkText("Guest")).Click();

                     driver.FindElement(By.XPath("//div[@id='guestTab']/div/div/fieldset/div/a[2]")).Click();
                     Thread.Sleep(2000);

                     driver.FindElement(By.XPath(".//*[@id='guestTab']/div[1]/div/fieldset/div/ul/li[text()='Default']")).Click();

                     driver.FindElement(By.XPath("//div[@id='selMemberListGuest']/div/div/div/div/ul/li[contains(text(),'Broadcast_Group3')]")).Click();

                     driver.FindElement(By.Id("moveGuestRight")).Click();
                     Thread.Sleep(2000);
                 */

                driver.FindElement(By.Id("btnsave")).Click();
                Thread.Sleep(2000);

                // VERIFYING SECOND RECORD

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(department_name_2)))
                {

                    takescreenshot("Second_Directory_Failed");

                    Assert.Fail("Second Directory Failed...");

                }

                else
                {
                    // EDITING SECOND RECORD

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[3]")).Click();

                    driver.FindElement(By.Id("txtname")).Clear();

                    driver.FindElement(By.Id("txtname")).SendKeys(department_name_2_edited);

                    driver.FindElement(By.Id("txtdesc")).Clear();

                    driver.FindElement(By.Id("txtdesc")).SendKeys(department_description_2_edited);
                    //  driver.FindElement(By.XPath("//a[text()='Permission']")).Click();
                    Thread.Sleep(2000);

            /*        driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();

                    driver.FindElement(By.XPath(path1 + "sysOper" + path2)).Click();

                    driver.FindElement(By.XPath("//tr[@id='footerTr']/td[3]/label")).Click();


                    driver.FindElement(By.Id("btnUsergroup")).Click();
                    Thread.Sleep(2000);
             */ 

                    driver.FindElement(By.Id("btnsave")).Click();
                    Thread.Sleep(2000);


                    // VERIFYING EDITED SECOND RECORD

                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(department_name_2_edited)))
                    {
                        takescreenshot("Second_Directory_Edited_Failed");

                        Assert.Fail("Second Directory Edited Failed...");
                    }
                    else
                    {
                        // DELETING SECOND RECORD

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[2]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);


                        // VERIFYING DELETED SECOND RECORD

                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(department_name_2_edited)))
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
        public void f_Add_Edit_Delete_User()
        {
            string username = "new_user";
            string username_2 = "test_new_user";
            string username_2_edited = "edited_new_user";
            
            string userdescription = "user description";
            string userdescription_2 = "test user description";
            string userdescription_2_edited = "edited user description";
            
            
            string email = "b@folio3.com";
            string email_2 = "d@folio3.com";
            string email_3 = "m@folio3.com";

            string access_code = "1228";
            string status = "Enabled";

            check_driver_type(driver_type, "settings", "Users", "Settings");

            Assert.AreEqual("Users", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);
            
            driver.FindElement(By.LinkText("Add User")).Click();
            Thread.Sleep(5000);

            driver.FindElement(By.Id("txtName")).Clear();

            driver.FindElement(By.Id("txtName")).SendKeys(username);

            driver.FindElement(By.Id("txtEmail")).Clear();

            driver.FindElement(By.Id("txtEmail")).SendKeys(email);

            driver.FindElement(By.XPath("//span[text()='Result to User Email']")).Click();

            //    driver.FindElement(By.CssSelector("a.selector")).Click();
            Thread.Sleep(1000);

            /*    driver.FindElement(By.XPath("//li[text()='"+timezone_name+"']")).Click();

                driver.FindElement(By.Id("txtAccess_code")).Clear();

                driver.FindElement(By.Id("txtAccess_code")).SendKeys(access_code);
             */

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

            /*     driver.FindElement(By.XPath("//form[@id='userPanel']/div[2]/div/div/div/fieldset[4]/div/a[2]")).Click();
                 Thread.Sleep(1000);

                 driver.FindElement(By.XPath("//li[text()='GUI']")).Click();
                 Thread.Sleep(1000);
             */

            driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//li[text()='sysAdmin']")).Click();
            Thread.Sleep(1000);
            //    driver.FindElement(By.Id("txtIp")).Clear();
            //    driver.FindElement(By.Id("txtIp")).SendKeys("10.0.0.40");

            driver.FindElement(By.Id("btnsave")).Click();


            Thread.Sleep(3000);
            takescreenshot("User");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            IWebElement myDynamicElement = wait.Until<IWebElement>(d => driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")));
            //wait.Until(driver => driver.FindElement(searchBy));


            /*      Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username) + "*");
                  Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(userdescription) + "*");
                  Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group_name) + "*");
                  Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(email) + "*");
                  Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(status) + "*");
             */

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(userdescription) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains("sysAdmin") &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(email)))
            {
                takescreenshot("User_Failed");
                Assert.Fail("Add User Failed...");
            }

            else
            {
                // ADDED SECOND RECORD

                driver.FindElement(By.LinkText("Add User")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.Id("txtName")).Clear();

                driver.FindElement(By.Id("txtName")).SendKeys(username_2);

                driver.FindElement(By.Id("txtEmail")).Clear();

                driver.FindElement(By.Id("txtEmail")).SendKeys(email_2);

                driver.FindElement(By.XPath("//span[text()='Result to User Email']")).Click();

                Thread.Sleep(1000);

                /*    driver.FindElement(By.XPath("//li[text()='"+timezone_name+"']")).Click();

                    driver.FindElement(By.Id("txtAccess_code")).Clear();

                    driver.FindElement(By.Id("txtAccess_code")).SendKeys(access_code);
                 */

                driver.FindElement(By.Id("txtDesc")).Clear();

                driver.FindElement(By.Id("txtDesc")).SendKeys(userdescription_2);
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

                /*     driver.FindElement(By.XPath("//form[@id='userPanel']/div[2]/div/div/div/fieldset[4]/div/a[2]")).Click();
                     Thread.Sleep(1000);

                     driver.FindElement(By.XPath("//li[text()='GUI']")).Click();
                     Thread.Sleep(1000);
                 */

                driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.XPath("//li[text()='sysAdmin']")).Click();
                Thread.Sleep(1000);
                //    driver.FindElement(By.Id("txtIp")).Clear();
                //    driver.FindElement(By.Id("txtIp")).SendKeys("10.0.0.40");

                driver.FindElement(By.Id("btnsave")).Click();



                // VERIFYING SECOND RECORD

                if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username_2)))
                {

                    takescreenshot("User_Failed");

                    Assert.Fail("User Failed...");

                }

                else
                {
                    // EDITING SECOND RECORD

                    driver.FindElement(By.XPath("(//a[@title='Edit'])[3]")).Click();

                    driver.FindElement(By.Id("txtName")).Clear();

                    driver.FindElement(By.Id("txtName")).SendKeys(username_2_edited);

                    driver.FindElement(By.Id("txtEmail")).Clear();

                    driver.FindElement(By.Id("txtEmail")).SendKeys(email_2);

                    driver.FindElement(By.Id("txtDesc")).Clear();

                    driver.FindElement(By.Id("txtDesc")).SendKeys(userdescription_2_edited);

                    driver.FindElement(By.Id("txtpassword")).Clear();

                    driver.FindElement(By.Id("txtpassword")).SendKeys("123");

                    driver.FindElement(By.Id("txtRetypePass")).Clear();

                    driver.FindElement(By.Id("txtRetypePass")).SendKeys("123");
                    Thread.Sleep(1000);

                    driver.FindElement(By.Id("btnsave")).Click();
                    Thread.Sleep(2000);


                    // VERIFYING EDITED SECOND RECORD

                    if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username_2_edited)))
                    {
                        takescreenshot("User_Edited_Failed");

                        Assert.Fail("User Edited Failed...");
                    }
                    else
                    {
                        // DELETING SECOND RECORD

                        driver.FindElement(By.XPath("(//a[@title='Delete'])[1]")).Click();

                        driver.FindElement(By.Id("btnOk")).Click();
                        Thread.Sleep(2000);


                        // VERIFYING DELETED SECOND RECORD

                        if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username_2_edited)))
                        {

                            takescreenshot("User_Passed");
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add_Edit_Delete User Passed... ^^^^^^^^^^^^^^^^^^^^^");
                        }

                        else
                        {

                            takescreenshot("User_Deleted_Failed");

                            Assert.Fail("User Deleted Failed...");


                        }


                    }


                }


            }


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

