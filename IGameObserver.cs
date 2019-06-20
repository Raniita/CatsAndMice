using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsAndMice
{
    public interface IGameObserver
    {
        void UpdateGame(object sender, GameEventArgs e);
    }
}
