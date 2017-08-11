using NUnit.Framework;
using System;
using System.Collections.Generic;
using CsvHelper;
using ForgetTheMilk.Controllers;
using System.IO;
using System.Text;

namespace ForgetTheMilkTests
{
    public class Tests
    {
        [Test]
        [Ignore("input in code")]
        public void MainTest()
        {
            for( int i = 0; i < 5; i++)
            {
                Console.WriteLine("Executing test case: " + i);
                subTest(i);
            }
        }

        [TestCase]
        [Ignore("Executed from MainTest()")]
        public void subTest(int i)
        {
            Console.WriteLine("In Test case: " + i);
            Equals(i);
        }

        public static IEnumerable<string[]> GetTestData()
        {
            CsvFileReader reader = new CsvFileReader("D:\\CSharpCode\\ForgetTheMilk\\ForgetTheMilk\\input.csv");
            CsvRow row = new CsvRow();
            while (reader.ReadRow(row))
            {
                var testName = row[0];
                Console.WriteLine("Test name: " + testName);
                var expDesc = row[1];
                Console.WriteLine("Description: " + expDesc);
                var expDueDate = row[2];
                Console.WriteLine("Expected due date: " + expDueDate);
                Console.WriteLine();

                yield return new[] { testName, expDesc, expDueDate };
            }
        }

        [Test, TestCaseSource("GetTestData")]
        public void validateInput(string testName, string expDesc, string expDueDate)
        {
            //Act / Arrange

            var task = new Task(expDesc, default(DateTime));
            DateTime parsedExpDueDate;

            Console.WriteLine("Test name: " + testName);
            Console.WriteLine("Description: " + expDesc);
            Console.WriteLine("Expected due date: " + expDueDate);
            Console.WriteLine();

            if (expDueDate == "null")
            {
                expDueDate = null;
                Assert.AreEqual(expDueDate, task.DueDate, "Due Date is not correct.");
            }
            else
            {
                parsedExpDueDate = DateTime.ParseExact(expDueDate, "yyyy-MM-dd HH:mm:ss", null);
                Console.WriteLine("----- parsedExpDueDate: " + parsedExpDueDate);
                Assert.AreEqual(parsedExpDueDate, task.DueDate, "Due Date is not correct.");
            }

            Assert.AreEqual(expDesc, task.Description, "Description is not correct.");
        }
    }
}
