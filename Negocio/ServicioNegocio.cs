using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbConection;

namespace Negocio
{
    public class ServicioNegocio
    {
        private readonly RolesPruebaEntities _dbContext;

        public ServicioNegocio()
        {
            _dbContext = new RolesPruebaEntities();
        }
        public int Loggin(string correo, string contrasena)
        {
            var puesto = (from p in _dbContext.Usuarios where p.correo == correo && p.contrasena == contrasena select p.IdUsuarios).FirstOrDefault();

            return puesto;
        }
        public string BuscarEmpleado(string correo, string contrasena)
        {
            var NEmpleado = (from p in _dbContext.Usuarios where p.correo == correo && p.contrasena == contrasena select p.nombre).FirstOrDefault();

            return NEmpleado;
        }
        public int BuscarArea(string correo, string contrasena)
        {
            var Area = (from p in _dbContext.Usuarios where p.correo == correo && p.contrasena == contrasena select p.id_area).FirstOrDefault();

            return (int)Area;
        }
        public List<Cuestionarios> ObtenerCuestionarios()
        {
            return _dbContext.Cuestionarios.ToList();
        }
        public Cuestionarios ObtenerCuestionario(int idCuestionario)
        {
            return _dbContext.Cuestionarios.SingleOrDefault(c => c.IdCuestionario == idCuestionario);
        }

        public List<Preguntas> ObtenerPreguntas(int idCuestionario)
        {
            return _dbContext.Preguntas.Where(p => p.IdCuestionario == idCuestionario).ToList();
        }

        public List<Respuestas> ObtenerRespuestas(int idPregunta)
        {
            return _dbContext.Respuestas.Where(r => r.IdPregunta == idPregunta).ToList();
        }

        public int Login(string correo, string contrasena)
        {
            var puesto = (from p in _dbContext.Usuarios where p.correo == correo && p.contrasena == contrasena select p.IdUsuarios).FirstOrDefault();

            return puesto;
        }

        public void Agregar(Cuestionarios itm)
        {
            var cuestionarios = new Cuestionarios();

            _dbContext.Cuestionarios.Add(itm);
            _dbContext.SaveChanges();
        }

        public Cuestionarios Buscar(int IdCuestionario)
        {
            var cuestionario = from p in _dbContext.Cuestionarios where p.IdCuestionario == IdCuestionario select p;

            return cuestionario.FirstOrDefault();
        }

        public void Modificar(Cuestionarios itm)
        {
            var reporte = (from p in _dbContext.Cuestionarios where p.IdCuestionario== itm.IdCuestionario select p).FirstOrDefault();

            reporte.IdCuestionario = itm.IdCuestionario;
            reporte.Titulo = itm.Titulo;
            reporte.Descripcion = itm.Descripcion;

            _dbContext.SaveChanges();
        }
        
        public void Eliminar(int IdCuestionario)
        {
            var cuestionario = (from p in _dbContext.Cuestionarios where p.IdCuestionario== IdCuestionario select p).FirstOrDefault();


            _dbContext.Cuestionarios.Remove(cuestionario);


            _dbContext.SaveChanges();

        }
        /*
        public void Eliminar(int IdCuestionario)
        {
            var cuestionario = _dbContext.Cuestionarios.FirstOrDefault(p => p.IdCuestionario == IdCuestionario);

            if (cuestionario != null)
            {
                // Verificar si el cuestionario tiene preguntas asociadas
                var tienePreguntas = _dbContext.Preguntas.Any(p => p.IdCuestionario == IdCuestionario);

                // Verificar si el cuestionario tiene tablas hijas asociadas

                if (tienePreguntas)
                {
                    // Si el cuestionario tiene preguntas o tablas hijas asociadas, lanzar una excepción o devolver un mensaje de error
                    throw new InvalidOperationException("No se puede eliminar el cuestionario porque tiene preguntas o tablas hijas asociadas.");
                }
                else
                {
                    // Si el cuestionario no tiene preguntas ni tablas hijas asociadas, eliminarlo
                    _dbContext.Cuestionarios.Remove(cuestionario);
                    _dbContext.SaveChanges();
                }

                
                
            }
        }*/
        public void AgregarPregunta(Preguntas itm)
        {
            var pregunta = new Preguntas();

            _dbContext.Preguntas.Add(itm);
            _dbContext.SaveChanges();
        }

        public Preguntas BuscarPregunta(int IdPregunta)
        {
            var pregunta = from p in _dbContext.Preguntas where p.IdPregunta == IdPregunta select p;

            return pregunta.FirstOrDefault();
        }

        public void ModificarPregunta(Preguntas itm)
        {
            var pregunta = (from p in _dbContext.Preguntas where p.IdPregunta == itm.IdPregunta select p).FirstOrDefault();

            pregunta.IdPregunta = itm.IdPregunta;
            pregunta.Pregunta = itm.Pregunta;
            pregunta.RespuestaCorrecta = itm.RespuestaCorrecta;

            _dbContext.SaveChanges();
        }
        
        public void EliminarPregunta(int IdPregunta)
        {
            var pregunta = (from p in _dbContext.Preguntas where p.IdPregunta== IdPregunta select p).FirstOrDefault();


            _dbContext.Preguntas.Remove(pregunta);


            _dbContext.SaveChanges();

        }

        public void AgregarRespuesta(Respuestas itm)
        {
            var respuesta = new Respuestas();

            _dbContext.Respuestas.Add(itm);
            _dbContext.SaveChanges();
        }

        public Respuestas BuscarRespuesta(int IdRespuesta)
        {
            var respuesta = from p in _dbContext.Respuestas where p.IdRespuesta == IdRespuesta select p;

            return respuesta.FirstOrDefault();
        }

        public void ModificarRespuesta(Respuestas itm)
        {
            var respueesta = (from p in _dbContext.Respuestas where p.IdRespuesta == itm.IdRespuesta select p).FirstOrDefault();

            respueesta.IdRespuesta= itm.IdRespuesta;
            respueesta.Respuesta = itm.Respuesta;

            _dbContext.SaveChanges();
        }
        
        public void EliminarRespuesta(int IdRespuesta)
        {
            var respuesta = (from p in _dbContext.Respuestas where p.IdRespuesta== IdRespuesta select p).FirstOrDefault();


            _dbContext.Respuestas.Remove(respuesta);


            _dbContext.SaveChanges();

        }
    }
}
