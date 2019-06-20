using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsAndMice
{
    /// <summary>
    /// Define las direcciones en las que puede moverse un elemento del juego.
    /// </summary>
    public enum Direction { Up, Down, Left, Right}

    /// <summary>
    /// Define una interfaz para aquellos elementos del juego que pueden moverse
    /// por el tablero (cambiar su fila y su columna).
    /// </summary>
    public interface IMovableItem: IGameItem
    {
        /// <summary>
        /// Establece la dirección en la que se mueve el elemento del juego.
        /// Esta propiedad se ofrece para que otro objeto establezca la dirección de movimiento
        /// del elemento del juego.
        /// </summary>
        Direction CurrentDirection { get; set; }

        /// <summary>
        /// Establece la dirección en la que se mueve el elemento del juego.
        /// Esta propiedad se ofrece para que el propio elemnto del juego
        /// establezca su dirección de movimiento.
        void ChangeDirection();
    }
}
