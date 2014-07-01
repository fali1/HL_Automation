using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Collections;
using System.IO;

namespace report_console
{
    class Program
    {
        static ArrayList testcase_name_list = new ArrayList();     // list for all testcase names
        static ArrayList testcase_executed_list = new ArrayList(); // list for all testcase executions status
        static ArrayList testcase_result_list = new ArrayList();   // list for all testcase results
        static ArrayList testcase_success_list = new ArrayList();  // list for test case success status
        static ArrayList testcase_time_list = new ArrayList();     // list for test case execution time
        static ArrayList testcase_msg_list = new ArrayList();      // list for test case messages
        static ArrayList testcase_stack_list = new ArrayList();    // list for test case stack trace

        static int testcase_count = 0;

        static int test_result_exist = 0;

        static int testcase_success_count = 0;

        static int testcase_failed_count = 0;

        static string given_path;

        static string testresultpath;

        static string testreportpath;

        


        static void Main(string[] args)
        {

            using (XmlTextReader reader = new XmlTextReader(@".\paths.xml"))
            {


                while (reader.Read())
                {
                    if (reader.IsStartElement("paths"))
                    {

                        Console.WriteLine("testresultpath:" + " " + reader.GetAttribute("testresultpath"));
                        testresultpath = reader.GetAttribute("testresultpath"); // get the path from <testresultpath> tag where testresult.xml is present

                        Console.WriteLine("testreportpath:" + " " + reader.GetAttribute("testreportpath"));
                        testreportpath = reader.GetAttribute("testreportpath"); // get the path from <testreportpath> tag where report to be created

                    }
                }
            }

       /*     Console.WriteLine("Enter path where TestResult.xml is present:" + " ");
             
            given_path= Console.ReadLine();*/
            
            y_get_testcase_node();

            
        }
           
       static public void y_get_testcase_node()
        {
            using (XmlTextReader reader = new XmlTextReader(testresultpath))
            {


                while (reader.Read())
                {
                    if (reader.IsStartElement("test-case")) // read all test case nodes from testresult.xml
                    {
                        Console.WriteLine("*name*" + reader.GetAttribute("name"));
                        Console.WriteLine("*executed*" + reader.GetAttribute("executed"));
                        Console.WriteLine("*result*" + reader.GetAttribute("result"));
                        Console.WriteLine("*success*" + reader.GetAttribute("success"));
                        Console.WriteLine("*time*" + reader.GetAttribute("time"));
                        Console.WriteLine("*asserts*" + reader.GetAttribute("asserts"));


                        // adding values in list arrays

                        testcase_name_list.Add(reader.GetAttribute("name")); 
                        testcase_executed_list.Add(reader.GetAttribute("executed"));
                        testcase_result_list.Add(reader.GetAttribute("result"));
                        testcase_success_list.Add(reader.GetAttribute("success"));
                        testcase_time_list.Add(reader.GetAttribute("time"));

                        if (reader.GetAttribute("success").ToString().Equals("True")) // if testcase is passed message and tack trace columsn will be displayed as 'None'
                        {

                            testcase_msg_list.Add("None");
                            testcase_stack_list.Add("None");

                        }

                        testcase_count = testcase_count + 1; // test cases count

                    }

                    else if (reader.IsStartElement("message"))
                    {

                        Console.WriteLine(testcase_msg_list);
                        testcase_msg_list.Add(reader.ReadElementString());

                    }
                    else if (reader.IsStartElement("stack-trace"))
                    {

                        Console.WriteLine(testcase_stack_list);
                        testcase_stack_list.Add(reader.ReadElementString());

                    }

                }
                create_file();

            }
           
            //  return browser_type;
        }


       static public void create_file()
        {

            DateTime todaydatetime_createfile = DateTime.Now;          // Use current time

            string format = "yyyy_MM_dd_hh_mm_ss";                     // Use this format Year_Month_Date_Hour_Minute_Second => 2014_04_21_02_35_09

            string format1 = "ddd, MMM d, yyy HH:mm:ss";               // used inside report

            /*     MMM     display three-letter month
                 ddd     display three-letter day of the WEEK
                 d       display day of the MONTH
                 HH      display two-digit hours on 24-hour scale
                 mm      display two-digit minutes
                 yyyy    display four-digit year*/


       /*     Console.WriteLine("Enter path where you want to create Test Report:"+" ");

            string path = Console.ReadLine()+"Testcase_Report_" + todaydatetime_createfile.ToString(format) + ".html";

            Console.WriteLine(path);*/

            string path = testreportpath + "Testcase_Report_" + todaydatetime_createfile.ToString(format) + ".html"; //creating path to create file with time stanp

            Console.WriteLine(path);

            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("<!DOCTYPE html>"); // adding html tags
                    sw.WriteLine("<html>");
                    sw.WriteLine("<body>");

                    sw.WriteLine("<head>");
                    sw.WriteLine("<style>");
                    sw.WriteLine("table,th,td"); // formatting table
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


                    sw.WriteLine("<center>");
                    sw.WriteLine("<b> <font size=\"6\"><u>TESTCASE EXECUTION REPORT</u> </font> </b>");    // Main heading
                    sw.WriteLine("</center>");


                    sw.WriteLine("<p>");
                    sw.WriteLine("<b> <u> SUMMARY </u></b>");    // SUMMARY heading
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p>");
                    sw.WriteLine("Date :" + " " + "<b>" + todaydatetime_createfile.ToString(format1) + "</b>"); //datetime formatting
                    sw.WriteLine("</p>");


                    sw.WriteLine("<p>");
                    sw.WriteLine("Total Testcases :" + " " + "<b>" + testcase_count + "</b>");           // total testcases count
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p style=\"color:green\">");
                    sw.WriteLine("Testcases Passed :" + " " + "<b>" + testcase_success_count + "</b>");  // total succeeded testcases
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p style=\"color:red\">");
                    sw.WriteLine("Testcases Failed :" + " " + "<b>" + testcase_failed_count + "</b>");   // total failed testcases
                    sw.WriteLine("</p>");

                    sw.WriteLine("<p/>");

                    sw.WriteLine("<p>");
                    sw.WriteLine("<b> <u>DETAILS </u></b>");
                    sw.WriteLine("</p>");



                    sw.WriteLine("<table>");
                    
                    sw.WriteLine("<tr>");                  // creating header row
                    sw.WriteLine("<th>Testcase Name</th>");
                    sw.WriteLine("<th>Testcase Executed</th>");
                    sw.WriteLine("<th>Testcase Result</th>");
                    sw.WriteLine("<th>Testcase Success</th>");
                    sw.WriteLine("<th>Testcase Time</th>");
                    sw.WriteLine("<th>Testcase Message</th>");
                    sw.WriteLine("<th>Stack Trace</th>");
                    sw.WriteLine("</tr>");

                    for (int i = 0; i < testcase_name_list.Count; i++)
                    {

                        sw.WriteLine("<tr>");
                        sw.WriteLine("<td>" + testcase_name_list[i] + "</td>");
                        sw.WriteLine("<td>" + testcase_executed_list[i] + "</td>");

                        if (testcase_success_list[i].Equals("True")) // if testcase is succeeded then display it as Green
                        {

                            sw.WriteLine("<td style=\"color:green\">" + testcase_result_list[i] + "</td>");
                            sw.WriteLine("<td style=\"color:green\">" + testcase_success_list[i] + "</td>");

                        }
                        else if (testcase_success_list[i].Equals("False")) // if testcase is succeeded then display it as Red
                        {

                            sw.WriteLine("<td style=\"color:red\">" + testcase_result_list[i] + "</td>");
                            sw.WriteLine("<td style=\"color:red\">" + testcase_success_list[i] + "</td>");

                        }

                        sw.WriteLine("<td>" + testcase_time_list[i] + "</td>");
                        sw.WriteLine("<td>" + testcase_msg_list[i] + "</td>");
                        sw.WriteLine("<td>" + testcase_stack_list[i] + "</td>");
                        sw.WriteLine("</tr>");

                        if (testcase_success_list[i].Equals("True"))
                        {

                            testcase_success_count = testcase_success_count + 1; // count of succeeded testcase
                            Console.WriteLine("Testcase_success_count:" + testcase_success_count);

                        }
                        else if (testcase_success_list[i].Equals("False")) 
                        {

                            testcase_failed_count = testcase_failed_count + 1; // count of failed testcase
                            Console.WriteLine("Testcase_failed_count:" + testcase_failed_count);

                        }

                    }

                    sw.WriteLine("</table>");
                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");

                }
            }
       }
    }
}
