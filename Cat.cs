using System;

namespace CatsAndMice {
    internal class Cat : IGameItem, IMovableItem {
        private Direction next = Direction.Down;

        public Cat() {
            CurrentDirection = Direction.Right;
        }

        public Cat(ItemCoordinates coords, int myValue) {
            Coords = coords;
            MyValue = myValue;
        }

        public Direction CurrentDirection {
            get { return next; }
            set { next = value; }
        }

        public virtual void ChangeDirection() {
            var random = new Random();
            var num = random.Next(1, 10);
            switch (num) {
                case 1:
                    next = Direction.Right;
                    break;
                case 2:
                    next = Direction.Left;
                    break;
                case 3:
                    next = Direction.Down;
                    break;
            }
        }

        public ItemCoordinates Coords { get; set; }
        public int MyValue { get; set; }

        public override string ToString() {
            return string.Concat(new object[] {
                GetType().Name, " Coords:", Coords
            });
        }
    }
}