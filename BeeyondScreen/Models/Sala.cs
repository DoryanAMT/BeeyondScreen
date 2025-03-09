using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeyondScreen.Models
{
    [Table("SALA")]
    public class Sala
    {
        [Key]
        [Column("SALA_ID")]
        public int IdSala { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("CINE_ID")]
        public int IdCine { get; set; }
    }
}
