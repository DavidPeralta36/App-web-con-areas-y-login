using Microsoft.AspNetCore.Mvc;
using DbConection;
using RolesPrueba.Models;
using Negocio;
using Newtonsoft.Json;
using System.Text;
using System.Data.Entity;


namespace RolesPrueba.Controllers
{
    public class InglesController : Controller
    {
        RolesPruebaEntities db = new ();
        ServicioNegocio cuestionarios = new();
        public IActionResult Index(int puesto, string empleado, int area)
        {
            var Area = "";
            switch (area)
            {
                case 1:
                    Area = "Sistemas";
                    break;

                case 2:
                    Area = "Ingles";
                    break;

                case 3:
                    Area = "Historia";
                    break;

                case 4:
                    Area = "Geografia";
                    break;

                default:
                    Area = "Desconocido";
                    break;
            }
            HttpContext.Session.SetInt32("puesto", puesto);
            HttpContext.Session.SetString("empleado", empleado);
            HttpContext.Session.SetString("area", Area);
            HttpContext.Session.SetInt32("AreaInt", area);

            ViewData["empleado"] = empleado;
            ViewData["area"] = Area;

            var viewModel = new CuestionariosViewModel();
            viewModel.Cuestionarios = db.Cuestionarios.Where(c => c.id_area == puesto).ToList();

            return View(viewModel);
        }

        public IActionResult Cuestionario(int idCuestionario)
        {
            var cuestionario = cuestionarios.ObtenerCuestionario(idCuestionario);
            var preguntas = cuestionarios.ObtenerPreguntas(idCuestionario);
            var respuestas = cuestionarios.ObtenerRespuestas(idCuestionario);
            var puesto = HttpContext.Session.GetInt32("puesto");
            var empleado = HttpContext.Session.GetString("empleado");
            int area = (int)HttpContext.Session.GetInt32("AreaInt");

            

            var modelo = new DetalleCuestionarioViewModel
            {
                Cuestionario = cuestionario,
                Preguntas = preguntas,
                Respuestas = respuestas,
                puesto = (int)puesto,
                empleado = empleado,
                area = (int)area
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var modeloBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(modelo, jsonSettings));
            HttpContext.Session.Set("modelo", modeloBytes);

            return View(modelo);
        }

        



        public IActionResult AgregarCuestionario(int IdCuestionario)
        {
            var ultimoCuestionario = db.Cuestionarios.OrderByDescending(c => c.IdCuestionario).FirstOrDefault();
            int nuevoIdCuestionario = ultimoCuestionario != null ? ultimoCuestionario.IdCuestionario + 1 : 1;

            ViewData["Mensaje"] = nuevoIdCuestionario.ToString();

            var puesto = HttpContext.Session.GetInt32("puesto");
            var empleado = HttpContext.Session.GetString("empleado");
            var area = HttpContext.Session.GetInt32("AreaInt");

            ViewData["puesto"] = puesto;
            ViewData["empleado"] = empleado;
            ViewData["area"] = area;

            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarCuestionario(DbConection.Cuestionarios itm)
        {
            cuestionarios.Agregar(itm);
            var puesto = HttpContext.Session.GetInt32("puesto");
            var empleado = HttpContext.Session.GetString("empleado");
            var area = HttpContext.Session.GetInt32("AreaInt");

            ViewData["puesto"] = puesto;
            ViewData["empleado"] = empleado;
            ViewData["area"] = area;
            return RedirectToAction("Index", "Sistemas", new { puesto = puesto, empleado = empleado, area = area });
        }

        public ActionResult EditarCuestionario(int IdCuestionario)
        {
            var puesto = HttpContext.Session.GetInt32("puesto");
            var empleado = HttpContext.Session.GetString("empleado");
            var area = HttpContext.Session.GetInt32("AreaInt");

            ViewData["puesto"] = puesto;
            ViewData["empleado"] = empleado;
            ViewData["area"] = area;
            return View(cuestionarios.Buscar(IdCuestionario));
        }

        [HttpPost]
        public ActionResult EditarCuestionario(DbConection.Cuestionarios itm)
        {
            cuestionarios.Modificar(itm);
            var puesto = HttpContext.Session.GetInt32("puesto");
            var empleado = HttpContext.Session.GetString("empleado");
            var area = HttpContext.Session.GetInt32("AreaInt");

            ViewData["puesto"] = puesto;
            ViewData["empleado"] = empleado;
            ViewData["area"] = area;
            return RedirectToAction("Index", "Sistemas", new { puesto = puesto, empleado = empleado, area = area });
        }

        public IActionResult EliminarCuestionarios(int IdCuestionario)
        {
            int preguntas = db.Preguntas.Where(p => p.IdCuestionario == IdCuestionario).ToList().Count;

            if (preguntas > 0)
            {
                ViewBag.AlertMessage = "El cuestionario que intenta eliminar contiene " + preguntas + " preguntas asignadas.";
            }

            var puesto = HttpContext.Session.GetInt32("puesto");
            var empleado = HttpContext.Session.GetString("empleado");
            var area = HttpContext.Session.GetInt32("AreaInt");

            ViewData["puesto"] = puesto;
            ViewData["empleado"] = empleado;
            ViewData["area"] = area;
            return View(cuestionarios.Buscar(IdCuestionario));
        }

        [HttpPost]
        public ActionResult EliminarCuestionarioss(int IdCuestionario)
        {
            
            cuestionarios.Eliminar(IdCuestionario);

            var puesto = HttpContext.Session.GetInt32("puesto");
            var empleado = HttpContext.Session.GetString("empleado");
            var area = HttpContext.Session.GetInt32("AreaInt");

            ViewData["puesto"] = puesto;
            ViewData["empleado"] = empleado;
            ViewData["area"] = area;

            return RedirectToAction("Index", "Sistemas", new { puesto = puesto, empleado = empleado, area = area });
        }

        public IActionResult AgregarPregunta(int IdCuestionario)
        {
            var ultimaPregunta = db.Preguntas.OrderByDescending(c => c.IdPregunta).FirstOrDefault();
            int nuevoIdPregunta = ultimaPregunta != null ? ultimaPregunta.IdPregunta + 1 : 1;

            ViewData["IdCuestionario"] = IdCuestionario;
            ViewData["Pregunta"] = nuevoIdPregunta.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarPregunta(DbConection.Preguntas itm)
        {
            cuestionarios.AgregarPregunta(itm);

            var modeloBytes = HttpContext.Session.Get("modelo");
            var modelo = JsonConvert.DeserializeObject<DetalleCuestionarioViewModel>(Encoding.UTF8.GetString(modeloBytes));

            return RedirectToAction("Cuestionario", "Sistemas", new { idCuestionario = modelo.Cuestionario.IdCuestionario });
        }


        public ActionResult EditarPregunta(int IdPregunta, int IdCuestionario)
        {

            ViewData["IdCuestionario"] = IdCuestionario;
            return View(cuestionarios.BuscarPregunta(IdPregunta));
        }

        [HttpPost]
        public ActionResult EditarPregunta(DbConection.Preguntas itm)
        {
            cuestionarios.ModificarPregunta(itm);
            var modeloBytes = HttpContext.Session.Get("modelo");
            var modelo = JsonConvert.DeserializeObject<DetalleCuestionarioViewModel>(Encoding.UTF8.GetString(modeloBytes));

            return RedirectToAction("Cuestionario", "Sistemas", new { idCuestionario = modelo.Cuestionario.IdCuestionario });
        }

        public IActionResult EliminarPregunta(int IdPregunta, int IdCuestionario)
        {
            int respuestas = db.Respuestas.Where(p => p.IdPregunta== IdPregunta).ToList().Count;

            if (respuestas > 0)
            {
                ViewBag.AlertMessage = "La pregunta que intenta eliminar contiene " + respuestas + " respuestas asignadas.";
            }
            ViewData["IdCuestionario"] = IdCuestionario;
            return View(cuestionarios.BuscarPregunta(IdPregunta));
        }

        [HttpPost]
        public ActionResult EliminarPreguntaa(int IdPregunta)
        {
            cuestionarios.EliminarPregunta(IdPregunta);

            var modeloBytes = HttpContext.Session.Get("modelo");
            var modelo = JsonConvert.DeserializeObject<DetalleCuestionarioViewModel>(Encoding.UTF8.GetString(modeloBytes));

            return RedirectToAction("Cuestionario", "Sistemas", new { idCuestionario = modelo.Cuestionario.IdCuestionario });
        }

        public IActionResult AgregarRespuesta(int IdPregunta, int IdCuestionario)
        {
            var ultimaRespuesta = db.Respuestas.OrderByDescending(c => c.IdRespuesta).FirstOrDefault();
            int nuevoIdRespuesta = ultimaRespuesta != null ? ultimaRespuesta.IdRespuesta + 1 : 1;

            ViewData["IdCuestionario"] = IdCuestionario;
            ViewData["Pregunta"] = IdPregunta;
            ViewData["Respuesta"] = nuevoIdRespuesta.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarRespuesta(DbConection.Respuestas itm, int puesto)
        {
            cuestionarios.AgregarRespuesta(itm);
            var modeloBytes = HttpContext.Session.Get("modelo");
            var modelo = JsonConvert.DeserializeObject<DetalleCuestionarioViewModel>(Encoding.UTF8.GetString(modeloBytes));

            return RedirectToAction("Cuestionario", "Sistemas", new { idCuestionario = modelo.Cuestionario.IdCuestionario });
        }

        public ActionResult EditarRespuesta(int IdRespuesta, int IdCuestionario)
        {
            ViewData["IdCuestionario"] = IdCuestionario;
            return View(cuestionarios.BuscarRespuesta(IdRespuesta));
        }

        [HttpPost]
        public ActionResult EditarRespuesta(DbConection.Respuestas itm)
        {
            cuestionarios.ModificarRespuesta(itm);
            var modeloBytes = HttpContext.Session.Get("modelo");
            var modelo = JsonConvert.DeserializeObject<DetalleCuestionarioViewModel>(Encoding.UTF8.GetString(modeloBytes));

            return RedirectToAction("Cuestionario", "Sistemas", new { idCuestionario = modelo.Cuestionario.IdCuestionario });
        }

        public IActionResult Eliminarrespuesta(int IdRespuesta, int IdCuestionario)
        {
            ViewData["IdCuestionario"] = IdCuestionario;
            return View(cuestionarios.BuscarRespuesta(IdRespuesta));
        }

        [HttpPost]
        public ActionResult EliminarRespuestaa(int IdRespuesta)
        {
            cuestionarios.EliminarRespuesta(IdRespuesta);

            var modeloBytes = HttpContext.Session.Get("modelo");
            var modelo = JsonConvert.DeserializeObject<DetalleCuestionarioViewModel>(Encoding.UTF8.GetString(modeloBytes));

            return RedirectToAction("Cuestionario", "Sistemas", new { idCuestionario = modelo.Cuestionario.IdCuestionario });
        }
    }
}
