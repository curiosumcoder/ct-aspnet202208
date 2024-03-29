﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Store.UI.Web.Intranet.Models;
using System.Diagnostics;

namespace Northwind.Store.UI.Web.Intranet.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("time") == null)
            {
                HttpContext.Session.SetString("time", DateTime.Now.ToLongTimeString());
            }

            //throw new ApplicationException("Tenemos un problema");

            //return NotFound();
            //return Forbid();
            //return Unauthorized();

            _logger.Log(LogLevel.Information, "Esta es una prueba de log");
            _logger.LogError("Este es un error para el log");
            _logger.LogWarning("Esta es una alerta para el log");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            //https://nlog-project.org/
            //https://serilog.net/

            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var exceptionMessage = "";

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is ApplicationException)
            {
                var ex = exceptionHandlerPathFeature?.Error;

                exceptionMessage = $"Error en la aplicación: {ex.Message}";
                _logger.LogError(exceptionMessage);
            }
            else if (exceptionHandlerPathFeature?.Error is Microsoft.Data.SqlClient.SqlException)
            {
                var ex = exceptionHandlerPathFeature?.Error;
                exceptionMessage = $"Error en el SQL Server: {ex.Message}";
                _logger.LogError(exceptionMessage);
            }

            if (exceptionHandlerPathFeature?.Path == "/")
            {
                exceptionMessage += " desde la raíz.";
            }

            return View("Error", new ErrorViewModel { RequestId = requestId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorWithCode(string code)
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCodeReExecuteFeature != null)
            {
                _ =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
            }

            ViewBag.statusCode = code;

            return View("ErrorStatus", new ErrorViewModel { RequestId = requestId });
        }
    }
}