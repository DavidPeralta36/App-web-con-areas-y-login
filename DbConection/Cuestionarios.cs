//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DbConection
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cuestionarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cuestionarios()
        {
            this.Preguntas = new HashSet<Preguntas>();
        }
    
        public int IdCuestionario { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<int> id_area { get; set; }
    
        public virtual Areas Areas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Preguntas> Preguntas { get; set; }
    }
}
