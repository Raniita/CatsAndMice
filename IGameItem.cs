using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CatsAndMice {
    /// <summary>
    /// Proporciona una interfaz común para todos los elementos
    /// del juego: su posición en una matriz (fila, columna) y un
    /// valor cuya interprestación dependerá de la lógica del
    /// juego.
    /// </summary>
    public interface IGameItem {
        ItemCoordinates Coords { get; set; }
        int MyValue { get; set; }
    }
}