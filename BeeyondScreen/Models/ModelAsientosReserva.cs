using BeeyondScreen.Models;

namespace BeeyondScreen.Models
{
    public class ModelAsientosReserva
    {
        public HorarioPelicula HorarioPelicula { get; set; }
        public Pelicula Pelicula { get; set; }
        public List<Asiento> Asientos { get; set; }
    }
}
