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
    public class HL_Base_Class
    {
        public string browser_name;
        public string browser_type;
        public IWebDriver driver;

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



        public void check_driver_type(string drivertype, string id_name, string link_text, string a_text) //drivertype= browser , id_name = landing page , link_text = panel(e.g Add user page) 
        {

            Thread.Sleep(3000);

            if (drivertype.ToString() == "OpenQA.Selenium.Safari.SafariDriver") //for safari
            {

                Console.WriteLine("if clause ....");
                Thread.Sleep(3000);
                WaitForChrome(5000, browser_name);

                driver.FindElement(By.XPath(".//*[@id='" + id_name + "']/a")).Click(); //goto landing for particular ID
                Thread.Sleep(3000);
                WaitForChrome(5000, browser_name);



                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='" + link_text + "']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(3000);
                WaitForChrome(5000, browser_name);



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
                WaitForChrome(5000, browser_name);



                driver.FindElement(By.XPath("//div[@class='category']/ul/li/a[text()='" + link_text + "']")).Click(); //goto particular panel w.r.t link
                Thread.Sleep(3000);
                WaitForChrome(5000, browser_name);


                driver.FindElement(By.XPath("//li[@id='" + id_name + "']/a")).Click(); //goto landing for particular ID
                Thread.Sleep(3000);
                WaitForChrome(5000, browser_name);

                hover_func(id_name, link_text, a_text);
                Thread.Sleep(3000);
                WaitForChrome(5000, browser_name);

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

            WaitForElementToExist(id_name, driver);
            WaitForChrome(5000, browser_name);

            action1.MoveToElement(hoveritem).Perform(); //move to list element that needs to be hovered
            Thread.Sleep(3000);
            WaitForChrome(5000, browser_name);

            driver.FindElement(By.XPath("(//a[text()='" + link_text + "'])[1]")).Click();
            Thread.Sleep(3000);
            WaitForChrome(5000, browser_name);


            //------ Focus out the mouse to disappear hovered dialog ------


            action1.MoveToElement(driver.FindElement(By.Id("lblCustomHeader"))).Perform();
            Thread.Sleep(3000);
            WaitForChrome(5000, browser_name);


        }


        // this function will be called only when 
        // chrome browser is selected in Browser.xml
        // located in Debug folder
        public void WaitForChrome(int counter, string browser_name)
        {
            if (browser_name.ToString() == "chrome")
            {
                Thread.Sleep(counter);
            }

        }


        // this function will restrict browser to wait 
        // untill desired element is not appeared
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


        // this function is used to read url form file
        // and called at the start of every script in login section
        public string[] read_url_from_file(string file_name)
        {
            // Read each line of the file into a string array. Each element 
            // of the array is one line of the file. 

            string[] lines_url = System.IO.File.ReadAllLines(@".\" + file_name + ".txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of url.txt = ");
            foreach (string line in lines_url)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\n" + line);
            }
            return lines_url;
        }


        public void create_receiver(string file_name)
        {
            string[] lines_local = read_from_file("hipadm_cmd");
            string[] receiver_cmd = read_from_file(file_name);

            //Build Commands - You may have to play with the syntax
            // on executing the batch file , in whch the following content will be written ,
            // first directory will be Changed using '%1', to the specified path defined in Arguments
            // then hipadm cmnd will be concatinated in th cmd prompt
            // then it will take pause


            string[] cmdBuilder = new string[] 
      
            {
                // '/d' will change the directory and '%1' will get the complete path till hiplink bin
                @"cd /d ""%1""",
                receiver_cmd[0],
                @"exit"
            };

            //Create a File Path
            string BatFile = @".\add_receiver.bat";

            //Create Stream to write to file.
            StreamWriter sWriter = new StreamWriter(BatFile);

            foreach (string cmd in cmdBuilder)
            {
                sWriter.WriteLine(cmd);
            }

            sWriter.Close();

            //Run your Batch File & Remove it when finished.

            Process p = new Process();
            ProcessStartInfo ps = new ProcessStartInfo();

            ps.CreateNoWindow = true;
            ps.UseShellExecute = true;
            ps.FileName = @".\add_receiver.bat"; // this batch file will be executed
            ps.Arguments = lines_local[0]; //this argument will be replaced by '%1' in batch file created bove
            p.StartInfo = ps;
            p.Start();
            p.WaitForExit();
            // File.Delete(@".\add_receiver.bat");

        }
     



    }
}
