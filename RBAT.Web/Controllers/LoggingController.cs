using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace RBAT.Web.Controllers
{
    public class LoggingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult RadData()
        {
            var LoggingPath = @"c:\Log\solver_results.txt";
            List<string> allLinesText = new List<string>(); ;
            try
            {
                var fileStream = new FileStream(LoggingPath, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        allLinesText.Add(line);
                    }
                }
            }
            catch (Exception)
            {

            }

            return Json(allLinesText);
        }

        [HttpPost]
        public IActionResult DeleteAllData()
        {
            try
            {
                if (System.IO.File.Exists(@"c:\Log\solver_results.txt"))
                {
                    System.IO.File.Delete(@"c:\Log\solver_results.txt");
                }

                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}