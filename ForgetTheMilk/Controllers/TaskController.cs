using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForgetTheMilk.Controllers
{
    public class TaskController : Controller
    {
        public ActionResult Index()
        {
            return View(Tasks);
        }

        // in-memory list to hold the entered
        public static readonly List<Task> Tasks = new List<Task>();

        [HttpPost]
        public ActionResult Add(string task)
        {
            var taskItem = new Task(task, DateTime.Today);
            Tasks.Add(taskItem);
            return RedirectToAction("Index");
        }
    }

    public class Task
    {
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }

        public Task(string task, DateTime today)
        {
            var month = "";

            for (int i = 1; i<=12; i++)
            {
                month = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                var patternStr = "(?i:" + month + ")\\s(\\d+)";
                Console.WriteLine("pattern: " + patternStr);

                Description = task;
                var dueDatePattern = new System.Text.RegularExpressions.Regex(patternStr);
                var hasDueDate = dueDatePattern.IsMatch(task);
                if (hasDueDate)
                {
                    var dueDate = dueDatePattern.Match(task);
                    var day = Convert.ToInt32(dueDate.Groups[1].Value);
                    DueDate = new DateTime(DateTime.Today.Year, i, day);
                    if (DueDate < DateTime.Today)
                    {
                        DueDate = DueDate.Value.AddYears(1);
                    }
                    break;
                }
            }
        }

        public string GetDataFromFile()
        {
            string[] rows = File.ReadAllLines(@"D:\CSharpCode\ForgetTheMilk\ForgetTheMilk\input.csv");
            string[] inputItems = File.ReadAllText(@"D:\CSharpCode\ForgetTheMilk\ForgetTheMilk\input.csv").Split(',');

            return "";
        }
    }
}