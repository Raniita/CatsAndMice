using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CatsAndMice.Objects;

namespace CatsAndMice {
    public static class ViewFactory {
        public static IElementView GetView(IGameItem item, int size) {
            IElementView view = null;
            if (item is Fruit) {
                view = new FruitView(item, size);
            }
            else if (item is Poisson) {
                view = new PoissonView(item, size);
            }
            else if (item is SmartMouse) {
                view = new MouseView(item, size);
            }
            else if (item is Mouse) {
                view = new MouseView(item, size);
            }
            else if (item is Cat) {
                view = new CatView(item, size);
            }
            return view;
        }
    }

    public abstract class GameItemView : IElementView {
        protected int size = 12;
        protected int x, y;
        protected int offset;
        protected IGameItem item;

        protected GameItemView(IGameItem item, int size) {
            this.item = (item != null) ? item : new Fruit();

            this.item = item;
            this.size = size;
            offset = size / 2;
            x = (item.Coords.Columna * size) - offset;
            y = (item.Coords.Fila * size) - offset;
        }

        public abstract void draw(System.Windows.Forms.PaintEventArgs e);
    }

    public class FruitView : GameItemView {
        private Bitmap image;

        public FruitView(IGameItem item, int size) : base(item, size) {
            image = new Bitmap("../../resources/images/apple.png");
        }

        public override void draw(System.Windows.Forms.PaintEventArgs e) {
            //e.Graphics.FillEllipse(Brushes.Pink, x, y, size, size);
            e.Graphics.DrawImage(image, new Rectangle(x, y, size, size));
        }
    }

    public class PoissonView : GameItemView {
        private Bitmap image;

        public PoissonView(IGameItem item, int size) : base(item, size) {
            image = new Bitmap("../../resources/images/poison.png");
        }

        public override void draw(System.Windows.Forms.PaintEventArgs e) {
            //e.Graphics.FillEllipse(Brushes.DarkGray, x, y, size, size);
            e.Graphics.DrawImage(image, new Rectangle(x, y, size, size));
        }
    }

    public class MouseView : GameItemView {
        private Bitmap image;

        public MouseView(IGameItem item, int size) : base(item, size) {
            image = new Bitmap("../../resources/images/frog.png");
        }

        public override void draw(System.Windows.Forms.PaintEventArgs e) {
            //e.Graphics.FillEllipse(Brushes.Blue, x, y, size, size);
            e.Graphics.DrawImage(image, new Rectangle(x, y, size, size));
        }
    }

    public class CatView : GameItemView {
        private Bitmap image;

        public CatView(IGameItem item, int size) : base(item, size) {
            image = new Bitmap("../../resources/images/visualstudio.png");
        }

        public override void draw(System.Windows.Forms.PaintEventArgs e) {
            //e.Graphics.FillEllipse(Brushes.Red, x, y, size, size);
            e.Graphics.DrawImage(image, new Rectangle(x, y, size, size));
        }
    }
}