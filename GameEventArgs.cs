using System;
using System.Collections.Generic;

namespace CatsAndMice
{
    /// <summary>
    /// Evento producido cada vez que se ejecuta un paso del juego.
    /// </summary>
    public class GameEventArgs : EventArgs
    {
        public List<IGameItem> gameItems;
        public GameEventArgs(List<IGameItem> items)
        {
            gameItems = items;
        }

        public IEnumerator<IGameItem> GetEnumerator()
        {
            foreach(var item in gameItems)
            {
                yield return item;
            }
        }

    }
}
