using System.ComponentModel.DataAnnotations.Schema;

namespace BeeyondScreen.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Column("USUARIO_ID")]
        public int UsuarioId { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("CORREO")]
        public string Correo { get; set; }
        [Column("CONTRASENA_HASH")]
        public string ContrasenaHash { get; set; }
        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; }
    }
}
