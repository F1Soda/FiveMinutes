﻿using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteController : Controller
    {
        public IActionResult Edit()
        {
            return View();
        }
    }
}
