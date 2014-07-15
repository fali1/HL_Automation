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
    public class b_Smoke_Recipients
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

            Assert.AreEqual(trimmed_user_label, "Welcome admin");

            verificationErrors = new StringBuilder();
        }


        [Test]
        public void a_Directory_Settings_Panel()
        {
            
            

                string dir_path = @"C:\Program Files (x86)\Hiplink Software\HipLink\new_directory";

                if (!Directory.Exists(create_directory_path_directory))
                {
                    Directory.CreateDirectory(create_directory_path_directory);
                }


                check_driver_type(driver_type, "administration", "Directories & Queues", "Sys Admin");

                Assert.AreEqual("Directories & Queues", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

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
        public void b_Add_Messenger()
        {
            
            

                string messenger_desc = "SMTP Messenger Description";

                check_driver_type(driver_type, "administration", "Messengers", "Sys Admin");

                Assert.AreEqual("Messengers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

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

                /*    driver.FindElement(By.CssSelector("a.selector")).Click();
                    SelectElement select_msngr_Queue = new SelectElement(driver.FindElement(By.Id("selMessangerQueue"))); // Creating SelectElement.
                    select_msngr_Queue.SelectByText("new_directory");*/

                driver.FindElement(By.CssSelector("a.selector")).Click();

                Thread.Sleep(1000);

                driver.FindElement(By.XPath("//fieldset[1]/div/ul/li[text()='" + new_dir + "']")).Click();
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

                    driver.FindElement(By.XPath("//div[@id='divGrid_idGridDataNode']")).Text.Contains(new_dir)))
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
        public void c_Add_Carrier()
        {


                string carrier_desc = "SMTP Carrier Description";
                string email_server = "smtp.gmail.com";
                string user_name = "hiplink@gmail.com";
                string user_pwd = "click+123";
                string email_suffix = "Email Suffix";
                string email_prefix = "Email Prefix";
                string email_subject = "Email Subject";


                check_driver_type(driver_type, "administration", "Carriers", "Sys Admin");

                Assert.AreEqual("Carriers", driver.FindElement(By.XPath("//div[@class='main_container']/h1")).Text);  //verifying page name

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
                //Console.WriteLine(DpListCount1);

                //----------xxxxxxx----------


                driver.FindElement(By.Id("btnaddcarrier")).Click();
                Thread.Sleep(5000);


                driver.FindElement(By.Id("carrierName")).Clear();
                driver.FindElement(By.Id("carrierName")).SendKeys(carrier_name);
                Thread.Sleep(2000);
                //----------- Selecting Paging Queue ----------

                //  html.js body div.wrapper div.middle_area div div.main_container form.add_carrier div.c_form_inner div.col_form fieldset div.custom ul li.selected
                driver.FindElement(By.CssSelector("a.selector")).Click();
                Thread.Sleep(1000);
                string path1 = "//li[text()='";
                string path2 = "']";

                driver.FindElement(By.XPath(path1 + new_dir + path2)).Click();
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
                    takescreenshot("Carrier_Passed");
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^   Added Carrier Passed ...   ^^^^^^^^^^^^^^^^^^^^^");
                    //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }

          
        }


        [Test]
        public void d_Add_Receiver()
        {
            
           
                string receiver_name = "receiver_smtp";
                string receiver_pin = "testm703@gmail.com";
                string receiver_description = "Receiver Description";
                string receiver_emailaddress = "email@address.com";

                string department_name = "Default";


                check_driver_type(driver_type, "recipients", "Receivers", "Recipients");

                Assert.AreEqual("Receivers", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

                driver.FindElement(By.XPath(".//div[@class='filter_panel']/a[text()='Add Reciever']")).Click();
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

                driver.FindElement(By.XPath("//div[@class='add_receiver_block']/div[1]/div[1]/div[1]/fieldset[4]/div/a[2]")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//li[contains(text(),'" + department_name + "')])")).Click();// selecting department
                Thread.Sleep(2000);

                driver.FindElement(By.Id("btnEditAttribute")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//span[contains(text(),'A1')]")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.Id("btnAddAttribute")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@class='add_receiver_block']/div/div[2]/div[3]/fieldset[1]/div/a[2]")).Click();
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
                    takescreenshot("Receiver_Passed");
                    Console.WriteLine("^^^^^^^^^^^^^^^^^  Add Receiver Passed ...  ^^^^^^^^^^^^^^^^^");
                    //  driver.FindElement(By.XPath("//*[@id='logout']")).Click();
                }
          

        }



        [Test]
        public void e_Add_Broadcast_Group()
        {
            
           

                string broadcast_group_name = "Broadcast_Group3";
                string broadcast_group_description = "Broadcast Group Description";
                string department_name = "Default";
                string owner_name = "admin";
                string receiver_name = "receiver_smtp";

                check_driver_type(driver_type, "recipients", "Broadcast", "Recipients");

                Assert.AreEqual("Broadcast Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

                driver.FindElement(By.LinkText("Add Group")).Click();

                driver.FindElement(By.Id("txtName")).Clear();
                driver.FindElement(By.Id("txtName")).SendKeys(broadcast_group_name); //name

                driver.FindElement(By.Id("txtDesc")).Clear();
                driver.FindElement(By.Id("txtDesc")).SendKeys(broadcast_group_description); //description

                driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/a[2]")).Click(); //member of department drop down
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[1]/div/ul/li[text()='" + department_name + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Set Owner']")).Click();

                driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/fieldset/div[1]/a[2]")).Click(); //set owner drop down
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[2]/fieldset/div[1]/ul/li[text()='" + owner_name + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

                driver.FindElement(By.Id("btnSaveTabOne")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

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
        public void f_Add_Escalation_Group()
        {
            

                string receiver_name = "receiver_smtp";
                string escalation_group_name = "Escalation_Group3";
                string escalation_group_description = "Escalation Group Description";
                string set_owner = "admin";

                check_driver_type(driver_type, "recipients", "Escalation", "Recipients");

                Assert.AreEqual("Escalation Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

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

                driver.FindElement(By.XPath("//div[@id='additionalInfo']/fieldset[4]/fieldset/div[1]/ul/li[text()='" + set_owner + "']")).Click();

                driver.FindElement(By.XPath("//span[text()='Alert this owner for membership changes']")).Click(); //Alert this owner for membership changes checkbox

                driver.FindElement(By.Id("btnSaveTabOne")).Click();
                Thread.Sleep(3000);

                driver.FindElement(By.XPath("//a[text()='Define Members']")).Click(); //Members tab
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();

                driver.FindElement(By.XPath("//li[text()='" + receiver_name + "']")).Click();


                driver.FindElement(By.Id("addRec")).Click();
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("(//a[@class='selector'])[4]")).Click(); //Delay dropp down
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//div[@id='membersGrid']/table/tbody/tr[3]/td[10]/div/ul/li[2]")).Click();

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
        public void g_Add_On_Duty_Group()
        {


                string receiver_name = "receiver_smtp";
                string on_duty_group_name = "On_Duty_Group3";
                string on_duty_group_description = "On Duty Group Description";
                string set_owner = "admin";

                check_driver_type(driver_type, "recipients", "On-Duty", "Recipients");

                Assert.AreEqual("On-Duty Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

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
        public void h_Add_Follow_me_Group()
        {


                string receiver_name = "receiver_smtp";
                string follow_me_group_name = "Follow_me_Group3";
                string follow_me_group_description = "Follow me Group Description";
                string set_owner = "admin";

                check_driver_type(driver_type, "recipients", "Follow-Me", "Recipients");

                Assert.AreEqual("Follow-Me Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

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
                driver.FindElement(By.XPath("//div[@id='scheduleTab']/div/div[1]/div[1]/div/ul/li[text()='" + receiver_name + "']")).Click();

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
                Thread.Sleep(2000);

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
        public void i_Add_Rotate_Group()
        {
            
                string receiver_name = "receiver_smtp";
                string rotate_group_name = "Rotate_Group3";
                string rotate_group_description = "Rotate Group Description";
                string set_owner = "admin";

                check_driver_type(driver_type, "recipients", "Rotation", "Recipients");

                Assert.AreEqual("Rotate Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

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
        public void j_Add_Subscription_Group()
        {

           
                string receiver_name = "receiver_smtp";
                string subscription_group_name = "Subscription_Group4";
                string subscription_group_description = "Subscription Group Description";
                string subscription_topic = "Demo topic";

                check_driver_type(driver_type, "recipients", "Subscription Groups", "Recipients");

                Assert.AreEqual("Subscription Groups", driver.FindElement(By.XPath("//div[@id='testing']/h1")).Text);  //verifying page name

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
