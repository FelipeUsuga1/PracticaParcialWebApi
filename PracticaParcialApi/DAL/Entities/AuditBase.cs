using System.ComponentModel.DataAnnotations;

namespace PracticaParcialApi.DAL.Entities
{
    public class AuditBase
    {
        [Key] //DataAnnotation me sirve para definir que esta propiedad ID es un PK
        [Required] //Para campos obligatorios, o sea que deben tener un valor (no permite nulls)
        public virtual Guid Id { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
