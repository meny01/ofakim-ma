using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ofakim_hw_ma.Models;
using ofakim_hw_ma.Services;

namespace ofakim_hw_ma.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExchangeService _exservice;

        public HomeController(IExchangeService exService)
        {
            _exservice = exService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var model = await _exservice.GetData();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SetData()
        {
            await _exservice.SetData();
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
