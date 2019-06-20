using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsAndMice
{
    /// <summary>
    /// Define dos modos de juego, uno (ClientDriven) donde el ratón es dirigido por un objeto externo,
    /// otro en el que el ratón se dirige a sí mismo mediante el método ChangeDirection.
    /// </summary>
    public enum GameMode { ClientDriven, AutonomousMouse }

    /// <summary>
    /// Proporciona una interfaz común para todos los juegos que pueden ejecutarse desde la ventana Form1
    /// </summary>
    public interface IGameLogic
    {
        /// <summary>
        /// Ejecuta un paso del juego. En cada paso del juego se actualizan todos los elementos
        /// del juego de acuerdo con la implementación de este método.
        /// </summary>
        /// <param name="mode"> modo de juego</param>
        /// <returns> >= 0 si el juego puede continuar, menor que cero si el juego
        /// no puede continuar.</returns>
        int ExecuteStep(GameMode mode);

        /// <summary>
        /// Fija la dirección de movimiento del ratón la siguiente vez que se ejecute ExecuteStep
        /// </summary>
        /// <param name="next">dirección de movimiento del ratón la siguiente vez que se ejecute ExecuteStep</param>
        void SetMouseDirection(Direction next);

        /// <summary>
        /// Devuelve un enumerador que permite iterar sobre todos los elementos del juego.
        /// </summary>
        /// <returns> enumerador que permite iterar sobre todos los elementos del juego</returns>
        IEnumerator<IGameItem> GetEnumerator();
    }
}
