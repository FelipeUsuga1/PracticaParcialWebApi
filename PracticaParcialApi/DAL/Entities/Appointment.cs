using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.DAL.Entities
{
    public class Appointment : AuditBase
    {
        [Display(Name = "Motivo")] // Para yo pintar el nombre bien bonito en el FrontEnd
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")] //Longitud de caracteres máxima que esta propiedad me permite tener, ejem varchar(50)
        [Required(ErrorMessage = "¡El campo {0} es obligatorio!")]
        public string Motivo { get; set; } //varchar(50)

        public DateTime Fecha { get; set; }

        public string Diagnostico { get; set; }

        public string? Tratamiento { get; set; }

        public Animal? Animal { get; set; } //Este representa un OBJETO Owner

        [Display(Name = "Id Animal")]
        public Guid AnimalId { get; set; } //FK
    }
}
