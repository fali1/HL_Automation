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
    public class HL_Base_Class
    {
        public string var1;
        
        public void WaitForChrome(int counter, string browser_name)
        {
            if (browser_name.ToString() == "chrome")
            {
                Thread.Sleep(counter);
            }

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






    }
}
