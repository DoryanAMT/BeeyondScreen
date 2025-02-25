using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeyondScreen.Models
{
    [Table("PELICULA")]
    public class Pelicula
    {
        [Key]
        [Column("ID_PELICULA")]
        public int IdPelicula { get; set; }
    }
}
