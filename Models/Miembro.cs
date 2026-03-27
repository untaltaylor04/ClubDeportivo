using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClubDeportivo.Models
{
    public class Miembro
    {
        [Key]
        [ScaffoldColumn(false)]
        public int idMiembro { get; set; }
        public required string nombre { get; set; }
        public required string apellido { get; set; }
        public required DateTime fechaNacimiento { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public string? direccion { get; set; }
        [ScaffoldColumn(false)]
        public string estado { get; set; } = "Activo";
    }
}
