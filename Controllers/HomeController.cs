using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ConwaysGameOfLife.Processing;

namespace ConwaysGameOfLife.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetRandomData()
        { 
            return new JsonResult(GameProcessing.GetRandomData());
        }
        [HttpPost]
        public JsonResult NextStep()
        {
            if (Request.ContentLength == 0) return null;
            var requestData = Request.Form.ToList();
            GameProcessing procecessing = new GameProcessing(requestData);
            return procecessing.GetNextStep();
        }
    }
}
