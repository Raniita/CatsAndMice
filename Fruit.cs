namespace CatsAndMice {
    public class Fruit : IGameItem {
        public Fruit() {
            Coords = new ItemCoordinates(0, 0);
            MyValue = 0;
        }

        public Fruit(ItemCoordinates coords, int myValue) {
            Coords = coords;
            MyValue = myValue;
        }

        public ItemCoordinates Coords { get; set; }
        public int MyValue { get; set; }

        public override string ToString() {
            return string.Concat(new object[] {
                GetType().Name, "Coords:", Coords
            });
        }
    }
}