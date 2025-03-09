using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeeyondScreen.Models
{
    #region
 //   CREATE TABLE USUARIO(
 //   USUARIO_ID INT PRIMARY KEY,
 //   NOMBRE NVARCHAR(100) NOT NULL,
 //   EMAIL NVARCHAR(150) UNIQUE NOT NULL,
	//IMAGEN NVARCHAR(150),
	//SALT NVARCHAR(50),
 //   PASS VARBINARY(MAX) NOT NULL,
 //   FECHA_CREACION DATETIME DEFAULT GETDATE()
 //   );
    #endregion
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("USUARIO_ID")]
        public int IdUsuario { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("SALT")]
        public string Salt { get; set; }
        [Column("PASS")]
        public byte[] Pass { get; set; }
        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; }
    }
}
