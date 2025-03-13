using BeeyondScreen.Models;

namespace MvcBeeyondScreen.Models
{
    public class ModelAsientosReserva
    {
        public Pelicula Pelicula { get; set; }
        public HorarioPelicula HorarioPelicula { get; set; }
        public List<Boleto> Boletos{ get; set; }

    }
}
