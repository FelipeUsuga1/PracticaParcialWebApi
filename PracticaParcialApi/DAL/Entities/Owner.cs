using System.ComponentModel.DataAnnotations;

namespace PracticaParcialApi.DAL.Entities
{
    public class Owner : AuditBase
    {
        [Display(Name = "Owner")] // Para yo pintar el nombre bien bonito en el FrontEnd
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")] //Longitud de caracteres máxima que esta propiedad me permite tener, ejem varchar(50)
        [Required(ErrorMessage = "¡El campo {0} es obligatorio!")]
        public string Name { get; set; } //varchar(50)

        public string LastName { get; set; } //varchar(50)

        public int Telefono { get; set; }

        [Display(Name = "Animales")]
        //relación con State 
        public ICollection<Animal>? Animals { get; set; }
    }
}
