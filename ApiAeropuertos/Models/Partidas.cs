using System;
using System.Collections.Generic;

namespace ApiAeropuertos.Models
{
    public partial class Partidas
    {
        public int Id { get; set; }
        public string Destino { get; set; } = null!;
        public string Vuelo { get; set; } = null!;
        public string Puerta { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime Tiempo { get; set; }
    }
}
