using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DbConection;
using Negocio;
using System.Data.Entity.Core.Common.CommandTrees;

namespace RolesPrueba.Controllers
{
    public class HomeController : Controller
    {
        ServicioNegocio Auth = new();
        public IActionResult Index()
        {
            HttpContext.Session.Remove("puesto");
            HttpContext.Session.Remove("empleado");
            HttpContext.Session.Remove("area");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contrasena)
        {
            if (string.IsNullOrEmpty(correo) && string.IsNullOrEmpty(contrasena))
            {
                Console.WriteLine("chale");
                return RedirectToAction("Index", "Home");
            }

            var puesto = Auth.Loggin(correo, contrasena);
            var empleado = Auth.BuscarEmpleado(correo, contrasena);
            var area = Auth.BuscarArea(correo, contrasena);

            if (puesto == 1 && empleado != null)
            {
                return RedirectToAction("Index", "Sistemas", new { puesto = puesto, empleado = empleado, area = area });
            }
            else if (puesto == 2 && empleado != null)
            {
                return RedirectToAction("Index", "Ingles", new { puesto = puesto, empleado = empleado, area = area });
            }
            else if (puesto == 3 && empleado != null)
            {
                return RedirectToAction("Index", "Historia", new { puesto = puesto, empleado = empleado, area = area });
            }
            else if (puesto == 4 && empleado != null)
            {
                return RedirectToAction("Index", "Geografia", new { puesto = puesto, empleado = empleado, area = area });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}