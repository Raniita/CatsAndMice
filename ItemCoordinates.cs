using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsAndMice
{
    /// <summary>
    /// Modela las coordenadas de un elemento del juego en una
    /// matriz (fila, columna)
    /// </summary>
    public class ItemCoordinates
    {
        public int Fila { get; set; }
        public int Columna { get; set; }

        public ItemCoordinates(): this(0, 0) { }

        public ItemCoordinates(int fila, int columna)
        {
            Fila = fila; Columna = columna;
        }

        public ItemCoordinates(ItemCoordinates coords)
        {
            if (coords != null)
            {
                Fila = coords.Fila; Columna = coords.Columna;
            }
            else
            {
                Fila = 0; Columna = 0;
            }
        }

        public override string ToString()
        {
            return "[" + Fila + ", " + Columna + "]";
        }
    }
}
