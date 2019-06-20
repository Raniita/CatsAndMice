using System;
using System.Collections.Generic;

namespace CatsAndMice.Objects {
    internal class SmartMouse : Mouse {
        private readonly List<IGameItem> items = new List<IGameItem>();

        public SmartMouse() {
            CurrentDirection = Direction.Right;
        }

        public SmartMouse(ItemCoordinates coords, int myValue) : base(coords, myValue) {
        }

        public override void ChangeDirection() {
            base.ChangeDirection();
            if (items != null) {
                foreach (IGameItem current in items) {
                    if (current is Fruit) {
                        if (current.Coords.Columna != Coords.Columna) {
                            CurrentDirection = ((current.Coords.Columna > Coords.Columna)
                                ? Direction.Right
                                : Direction.Left);
                        }
                        else {
                            if (current.Coords.Fila != Coords.Fila) {
                                CurrentDirection = ((current.Coords.Fila > Coords.Fila)
                                    ? Direction.Down
                                    : Direction.Up);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateGame(object sender, GameEventArgs e) {
            if (items == null) {
                Console.WriteLine("*SmartMouse* UpdateGame --> items = null");
            }
            else {
                foreach (IGameItem current in e) {
                    items.Add(current);
                }
            }
        }
    }
}