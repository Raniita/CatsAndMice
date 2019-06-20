using System;
using System.Collections.Generic;
using System.Media;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatsAndMice.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatsAndMice {
    public class GameLogic : IGameLogic {
        public const int MaxFila = 25;
        public const int MaxColumna = 40;
        private const int fruits = 30;
        private const int poison = 12;

        // Los elementos del juego se gestionan en varios objetos contenedores de datos 
        // que apuntan hacia datos comunes.
        // Ventaja: se facilita la impelemntación de la lógica del juego.
        // Inconveniente: Hay que mantener la coherencia entre los diferents contenedores, duplicando
        // las operaciones de inserción y borrado.

        // Panel del juego: contiene referencia a todos los datos.
        // Cada casilla o bien está vacía (contiene referencia a null) o bien
        // contiene una referencia al elemento del juego que está sobre ella.
        private readonly IGameItem[,] board;

        // Lista de elementos del juego. Si se elimina o añade un elemnto en
        // esta lista también hay que elimanarlo/añadirlo en el panel de juego (board).
        private readonly List<IGameItem> gameItems = new List<IGameItem>();

        // Personajes mouse 
        //private readonly Mouse myMouse = new Mouse(new ItemCoordinates(20, 1), 0);
        private readonly Mouse myMouse = new SmartMouse(new ItemCoordinates(MaxFila / 3, 1), 0);

        // Gatos
        private readonly List<Cat> myCats = new List<Cat>();

        //Sonidos del juego
        private readonly SoundPlayer playerFruits = new SoundPlayer("../../resources/sounds/Beep8.wav");
        private readonly SoundPlayer playerPoison = new SoundPlayer("../../resources/sounds/Explosion2.wav");
        private readonly SoundPlayer playerDead = new SoundPlayer("../../resources/sounds/Danger2.wav");
        private readonly SoundPlayer playerWin = new SoundPlayer("../../resources/sounds/Space_Alert3.wav");

        private int stepCounter = 0;

        // Eventos del juego
        public event EventHandler<GameEventArgs> NewGameStepEventHandlers;

        /// <summary>
        /// Construye un tablero de filas x columnas
        /// </summary>
        public GameLogic() {
            board = new IGameItem[MaxFila, MaxColumna];
            FillBoard(fruits, poison);
            AddItem(myMouse);
            if (myMouse is SmartMouse) {
                NewGameStepEventHandlers += (myMouse as SmartMouse).UpdateGame;
            }

            //Precargamos los sonidos
            playerFruits.Load();
            playerPoison.Load();
            playerDead.Load();
            playerWin.Load();
        }

        /// <summary>
        /// Devuelve true si en la (fila, columna) especificada no hay
        /// ningún elemento de juego.
        /// </summary>
        /// <param name="fila"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        private bool IsCellAvailable(int fila, int columna) {
            return fila < MaxFila && columna < MaxColumna && fila >= 0 && board[fila, columna] == null;
        }

        /// <summary>
        /// Añade un elemento al juego en la celda especificada en las coordenadas del
        /// argumento (item), siempre que (1) item != null, (2) la posición no esté ya
        /// ocupada por otro elemntoe y (3) el elemento no esté ya en el juego.
        /// </summary>
        /// <param name="item"></param>
        private void AddItem(IGameItem item) {
            if (item == null) return;
            if (!IsCellAvailable(item.Coords.Fila, item.Coords.Columna)) return;
            if (gameItems.Contains(item)) return;
            board[item.Coords.Fila, item.Coords.Columna] = item;
            gameItems.Add(item);
        }

        /// <summary>
        /// Elimina un elemento del juego.
        /// </summary>
        /// <param name="item"></param>        
        private void RemoveItem(IGameItem item) {
            if (item == null) return;
            gameItems.Remove(item);
            if (board[item.Coords.Fila, item.Coords.Columna] == item) {
                board[item.Coords.Fila, item.Coords.Columna] = null;
            }
        }

        /// <summary>
        /// Rellena el juego con un número determinado de frutas y venenos colocados en 
        /// posiciones aleatorias.
        /// </summary>
        private void FillBoard(int nFruits, int nPoissons) {
            var random = new Random();
            for (var i = 0; i < nFruits; i++) {
                AddItem(new Fruit(new ItemCoordinates(random.Next(1, MaxFila - 1), random.Next(1, MaxColumna - 1)), 2));
            }

            for (var j = 0; j < nPoissons; j++) {
                AddItem(new Poisson(new ItemCoordinates(random.Next(1, MaxFila - 1), random.Next(1, MaxColumna - 1)),
                    -1));
            }
        }

        /// <summary>
        /// Fija la dirección de movimiento del ratón.
        /// </summary>
        /// <param name="next"></param>
        public void SetMouseDirection(Direction next) {
            myMouse.CurrentDirection = next;
        }

        /// <summary>
        /// Devuelve un enumerador de los elementos del juego.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IGameItem> GetEnumerator() {
            foreach (var item in gameItems) {
                yield return item;
            }
        }

        /// <summary>
        /// Implementa la lógica del juego que se ejecuta en cada Tick de un temporizador.
        /// </summary>
        /// <returns>>= 0 si el juego puede continuar, un valor negativo si no se puede continuar</returns>
        public int ExecuteStep(GameMode mode) {
            if (NewGameStepEventHandlers != null) {
                NewGameStepEventHandlers(this, new GameEventArgs(gameItems));
                //Console.WriteLine("Observers updated");
            }

            if (mode == GameMode.AutonomousMouse) {
                myMouse.ChangeDirection();
            }

            updateMovablePosition(myMouse);

            if (processCell() < 0) {
                playerDead.Play();
                Console.WriteLine("**GameOver** --> Life is over (processCell < 0)");
                return -2;
            }

            if (myMouse.MyValue == -1) {
                playerDead.Play();
                Console.WriteLine("**GameOver** --> Cat hit");
                return -2;
            }

            foreach (var cat in myCats) {
                cat.ChangeDirection();
                updateMovablePosition(cat);
            }

            if (board[myMouse.Coords.Fila, myMouse.Coords.Columna] is Cat) {
                playerDead.Play();
                Console.WriteLine("**GameOver** --> Cat hit mouse (ExecuteStep Cat)");
                Console.WriteLine(myMouse + " die vs " + myCats);
                return -2;
            }

            if (stepCounter == 15 && myCats.Count < 12) {
                var random = new Random();
                var new_cat = new Cat(new ItemCoordinates(0, random.Next(1, MaxColumna - 1)), 0);
                myCats.Add(new_cat);
                AddItem(new_cat);
                Console.WriteLine("Adding Cat --> " + myCats.Count);
                stepCounter = 0;
            }
            else {
                stepCounter++;
            }
            //Si nos comemos todas la frutas ganamos el juego.
            if (!gameItems.OfType<Fruit>().Any()) {
                playerWin.Play();
                return 100;
            }

            return 0;
        }

        /// <summary>
        /// Actualiza posición del elemento moviéndolo una fila o columna dependiendo
        /// de la dirección de su movimiento.
        /// Cuando el elemento llega a un límite del tablero aparece por el lado contrario.
        /// </summary>
        /// <param name="item"></param>
        private void updateMovablePosition(IMovableItem item) {
            var prevFila = item.Coords.Fila;
            var prevColumna = item.Coords.Columna;

            switch (item.CurrentDirection) {
                case Direction.Up:
                    item.Coords.Fila = (item.Coords.Fila - 1);
                    if (item.Coords.Fila <= 0) item.Coords.Fila = MaxFila - 1;
                    break;
                case Direction.Down:
                    item.Coords.Fila = (item.Coords.Fila + 1) % MaxFila;
                    break;
                case Direction.Right:
                    item.Coords.Columna = (item.Coords.Columna + 1) % MaxColumna;
                    break;
                case Direction.Left:
                    item.Coords.Columna = (item.Coords.Columna - 1);
                    if (item.Coords.Columna <= 0) item.Coords.Columna = MaxColumna - 1;
                    break;
            }

            if (item is Cat) {
                board[prevFila, prevColumna] = null;
                var moveTo = board[item.Coords.Fila, item.Coords.Columna];
                if (moveTo is Mouse) {
                    myMouse.MyValue = -1;
                }
                else {
                    RemoveItem(moveTo);
                    board[item.Coords.Fila, item.Coords.Columna] = item;
                }
            }
        }

        /// <summary>
        /// Actualiza el juego en función de lo que hay en la celda donde está el ratón.
        /// Si no hay nada, no hace nada.
        /// Si hay fruta o veneno, suma al "valor" del ratón el valor de la fruta (positivo) o del veneno (negativo) y
        /// elimina la fruta o el veneno.
        /// Si hay un gato pone el valor del ratón en -1.
        /// </summary>
        /// <returns> El valor almacenado en el ratón. </returns>
        private int processCell() {
            if (board[myMouse.Coords.Fila, myMouse.Coords.Columna] is Fruit) {
                myMouse.MyValue += board[myMouse.Coords.Fila, myMouse.Coords.Columna].MyValue;
                Console.WriteLine(myMouse + " ate " +
                                  board[myMouse.Coords.Fila, myMouse.Coords.Columna].GetType().Name + ". Points: " +
                                  myMouse.MyValue);
                RemoveItem(board[myMouse.Coords.Fila, myMouse.Coords.Columna]);
                playerFruits.Play();
            }
            if (board[myMouse.Coords.Fila, myMouse.Coords.Columna] is Poisson) {
                myMouse.MyValue += board[myMouse.Coords.Fila, myMouse.Coords.Columna].MyValue;
                Console.WriteLine(myMouse + " ate " +
                                  board[myMouse.Coords.Fila, myMouse.Coords.Columna].GetType().Name + ". Points: " +
                                  myMouse.MyValue);
                RemoveItem(board[myMouse.Coords.Fila, myMouse.Coords.Columna]);
                playerPoison.Play();
            }
            if (board[myMouse.Coords.Fila, myMouse.Coords.Columna] is Cat) {
                playerDead.Play();
                Console.WriteLine("**GameOver** --> Cat hit mouse (ProcessCell)");
                return -2;
            }
            return myMouse.MyValue;
        }
    }
}