using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsAndMice {
    /// <summary>
    /// Proporciona una implementación común para todos los
    /// elementos del juego.
    /// </summary>
    abstract class GameItem : IGameItem {
        /// <summary>
        /// Crea un elemento del juego en (0,0) y con valor 0.
        /// </summary>
        public GameItem() : this(null, 0) {
        }

        /// <summary>
        /// Crea un elemento del juego.
        /// </summary>
        /// <param name="coords">fila y columna en la que posiciona el elemento. Si es null, el elemento se posiciona en las coordenadas (0,0)</param>
        /// <param name="myValue">valor asignado al elemento</param>
        public GameItem(ItemCoordinates coords, int myValue) {
            if (coords != null) {
                Coords = coords;
            }
            else {
                Coords = new ItemCoordinates();
            }
            MyValue = myValue;
        }

        public ItemCoordinates Coords { get; set; }
        public int MyValue { get; set; }
    }
}