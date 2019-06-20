using System;

namespace CatsAndMice {
    public class Mouse : IMovableItem {
        private Direction next = Direction.Right;

        public Direction CurrentDirection {
            get { return next; }
            set { next = value; }
        }
        
        protected Mouse() {
            CurrentDirection = Direction.Right;
        }

        public Mouse(ItemCoordinates coord, int value) {
            Coords = coord;
            MyValue = value;
        }

        public virtual void ChangeDirection() {
            //Nothing to do here
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