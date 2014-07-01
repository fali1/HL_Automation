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
    public class b_Smoke_Account
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

        string create_directory_path = @".\Screenshots_Testcase_Results";

        string create_directory_path_directory = @"C:\Program Files (x86)\Hiplink Software\HipLink\new_directory";

        int test_result_exist = 0;

        string create_directory_path_with_time;

        string receiver_name = "receiver_smtp";

        string user_group_name = "new_user_group";

        string department_name = "new_department";

        string username = "new_user";

        string timezone_name = "Karachi Timezone";

        string message_template_name = "Message Template";



        [TestFixtureSetUp]

        public void SetupTest()
        {

            // driver = new ChromeDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch chrome browser


            // driver = new InternetExplorerDriver(@"C:\Users\fali\Documents\Visual Studio 2012\Projects\HL_Smoke\HL_Smoke\bin\Debug"); // launch IE browser

            // driver = new SafariDriver();// launch safari browser

            // driver = new FirefoxDriver();// launch firefox browser

            // System.Diagnostics.Debugger.Launch();// launch debugger

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

            Console.WriteLine("User label:" + "*" + driver.FindElement(By.XPath("//li[@class='user_name']")).Text + "*");

            user_label = driver.FindElement(By.XPath("//li[@class='user_name']")).Text.ToString();

            trimmed_user_label = user_label.TrimEnd();

            Console.WriteLine("User label Trimmed at the end:" + "*" + trimmed_user_label + "*");

            Assert.AreEqual(trimmed_user_label, "Welcome fahad");

            verificationErrors = new StringBuilder();
        }


        [Test]
        public void a_Add_Timezone()
        {
            
            string timezone_desc = "Karachi Timezone description";
            string offset = "5 Hours";
            string offset_digit = "5";

            check_driver_type(driver_type, "administration", "Time Zone Settings", "Sys Admin");

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
        public void b_Add_Message_Template()
        {

            
            string message_template_description = "Message Template Description";
            

            check_driver_type(driver_type, "settings", "Message Template", "Settings");

            Assert.AreEqual("Message Template", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            driver.FindElement(By.LinkText("Add Template")).Click();

            driver.FindElement(By.Id("txtTmpltName")).Clear();
            driver.FindElement(By.Id("txtTmpltName")).SendKeys(message_template_name); //message template name

            driver.FindElement(By.Id("txtAdesc")).Clear();
            driver.FindElement(By.Id("txtAdesc")).SendKeys(message_template_description); //message template description

            driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[1]")).Click();
            Thread.Sleep(2000);

            // driver will scrol down the screen untill the required element is in view... 

            IWebElement element = driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[contains(text(),'" + receiver_name + "')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("var $myLi = $(arguments[0]), $myParent = $myLi.parents('.common_scrollbar'); $myParent.mCustomScrollbar('scrollTo', '#' + $myLi.attr('id'));", element);
            Thread.Sleep(2000);


           // driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[1]")).SendKeys("rec");
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@id='recipientList']/div/div/div/div[1]/ul/li[contains(text(),'" + receiver_name + "')]")).Click(); //Assign Templates To Recipients list box
            Thread.Sleep(2000);

            driver.FindElement(By.Id("recipientMoveRight")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("(//a[@class='selector'])[1]")).Click();

            driver.FindElement(By.XPath("//li[text()='Quick Send']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();

            driver.FindElement(By.XPath("//li[text()='Confidential']")).Click();

            driver.FindElement(By.XPath("(//a[@class='selector'])[3]")).Click();

            driver.FindElement(By.XPath("//li[text()='Important']")).Click();

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
        public void c_Add_Response_Action()
        {
            string response_action_name = "Standard_Response_Action";
            string response_action_description = "Standard Response Action description";
            string command_line_statement=("echo \"fahad\"");

            check_driver_type(driver_type, "settings", "Response Actions", "Settings");

            Assert.AreEqual("Response Actions Panel", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);

            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("Add Action")).Click();

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
          
            driver.FindElement(By.XPath("(//a[@class='selector'])[2]")).Click();  // Actions dropdown
            driver.FindElement(By.XPath("//li[text()='Command Line']")).Click();

            driver.FindElement(By.Id("txtAcommand")).Clear();
            driver.FindElement(By.Id("txtAcommand")).SendKeys(command_line_statement); // Commands textbox
            
            driver.FindElement(By.Id("saveResponse")).Click();

            takescreenshot("Response_Action");
            Thread.Sleep(2000);


            Console.WriteLine(driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div[1]/div[1]/div[1]/div[2]")).Text);

            if (driver.FindElement(By.XPath(".//*[@id='divGrid_idGridDataNode']/div/div[1]/div/div[3]")).Text.Contains(response_action_name))
            {
                takescreenshot("Response_Action_Passed");

                Console.WriteLine("^^^^^^^^^^^^^^^ Response_Action Passed ... ^^^^^^^^^^^^^^^");
            }
            else
            {
                takescreenshot("Response_Action_Failed");

                Assert.Fail("Response_Action Failed ...");
            }
            
        }


        [Test]
        public void d_Add_User_Group()
        {
            
            string user_group_desc = "User Group Description";

            Thread.Sleep(2000);

            check_driver_type(driver_type, "settings", "User Groups- Permissioning", "Settings");

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

            driver.FindElement(By.CssSelector("a.selector")).Click();

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
                takescreenshot("User_Group_Passed");
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Add User Group Passed...    ^^^^^^^^^^^^^^^^^^^^^");
                // driver.FindElement(By.XPath("//*[@id='logout']")).Click();
            }

        }

        [Test]
        public void e_Add_Department()
        {
            string department_description = "Department description";

            check_driver_type(driver_type, "settings", "Departments", "Settings");

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

            driver.FindElement(By.Id("txtname")).SendKeys(department_name);

            driver.FindElement(By.Id("txtdesc")).Clear();

            driver.FindElement(By.Id("txtdesc")).SendKeys(department_description);
            //  driver.FindElement(By.XPath("//a[text()='Permission']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[@class='add_user_department_add']/div/a[2]")).Click();

            string path1 = "//li[text()='";
            string path2 = "']";

            driver.FindElement(By.XPath(path1 + user_group_name + path2)).Click();

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

            driver.FindElement(By.XPath("//div[@id='memberTab']/div/div/fieldset/div/a[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//div[@id='selMemberList']/div/div/div/div/ul/li[contains(text(),'receiver_smtp')]")).Click();

            driver.FindElement(By.Id("moveMemberRight")).Click();
            Thread.Sleep(2000);

            //  driver.FindElement(By.CssSelector("fieldset > div.custom.dropdown > a.selector")).Click();


            driver.FindElement(By.LinkText("Guest")).Click();

            driver.FindElement(By.XPath("//div[@id='guestTab']/div/div/fieldset/div/a[2]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//li[text()='Default']")).Click();

            driver.FindElement(By.XPath("//div[@id='selMemberListGuest']/div/div/div/div/ul/li[contains(text(),'Broadcast_Group3')]")).Click();

            driver.FindElement(By.Id("moveGuestRight")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("btnsave")).Click();
                        

        }

        [Test]
        public void f_Add_User()
        {
            
            string userdescription = "user description";
            string email = "b@folio3.com";
            string access_code = "1228";
            string status = "Enabled";

            check_driver_type(driver_type, "settings", "Users", "Settings");

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
            Thread.Sleep(5000);

            driver.FindElement(By.Id("txtName")).Clear();

            driver.FindElement(By.Id("txtName")).SendKeys(username);

            driver.FindElement(By.Id("txtEmail")).Clear();

            driver.FindElement(By.Id("txtEmail")).SendKeys(email);

            driver.FindElement(By.XPath("//span[text()='Result to User Email']")).Click();

            driver.FindElement(By.CssSelector("a.selector")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//li[text()='"+timezone_name+"']")).Click();

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

            driver.FindElement(By.XPath("//li[text()='" + user_group_name + "']")).Click();
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
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group_name) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(email) + "*");
            Console.WriteLine("*" + driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(status) + "*");

            if (!(driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(username) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(userdescription) &&

                driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(user_group_name) &&

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






        public void check_driver_type(string drivertype, string id_name, string link_text, string a_text) //drivertype= browser , id_name = landing page , link_text = panel(e.g Add user page) 
        {

            Thread.Sleep(2000);

            if (drivertype.ToString() == "OpenQA.Selenium.Safari.SafariDriver") //for safari
            {

                Console.WriteLine("if clause ....");
                Thread.Sleep(2000);

                driver.FindElement(By.XPath(".//*[@id='" + id_name + "']/a")).Click(); //goto landing for particular ID
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

                driver.FindElement(By.XPath("//li[@id='" + id_name + "']/a[text()='" + a_text + "']")).Click(); //goto landing for particular ID
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
    }
}
