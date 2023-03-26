using DbConection;

namespace RolesPrueba.Models
{
    public class DetalleCuestionarioViewModel
    {
        public Cuestionarios? Cuestionario { get; set; }
        public List<Preguntas>? Preguntas { get; set; }
        public List<Respuestas>? Respuestas { get; set; }

        public int? puesto { get; set; }

        public string? empleado { get; set; }

        public int? area { get; set; }
    }

}