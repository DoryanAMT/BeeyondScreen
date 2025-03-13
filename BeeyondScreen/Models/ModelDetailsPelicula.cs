using BeeyondScreen.Models;

namespace MvcBeeyondScreen.Models
{
    public class ModelDetailsPelicula
    {
        public Pelicula Pelicula { get; set; }
        public List<HorarioPelicula> HorarioPelicula { get; set; }
    }
}
