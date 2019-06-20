using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatsAndMice
{
    // Juego dirigido por teclado (barra espaciadora) o por timer.
    enum ExecutionMode { SpaceClick, TimerTick }
    partial class CatsAndMiceForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.boardPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // boardPanel
            // 
            this.boardPanel.BackColor = System.Drawing.Color.Black;
            this.boardPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.boardPanel.Location = new System.Drawing.Point(12, 12);
            this.boardPanel.Name = "boardPanel";
            //Good values (1000,690)
            this.boardPanel.Size = new System.Drawing.Size(1000, 690);
            this.boardPanel.TabIndex = 0;
            this.boardPanel.BackgroundImage = new Bitmap("../../resources/background/gnu-love_inicio.jpg");
            this.boardPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.CatsAndMiceForm_Paint);
            // 
            // CatsAndMiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //Good values (1000,690)
            this.ClientSize = new System.Drawing.Size(1000, 690);
            this.Controls.Add(this.boardPanel);
            this.Name = "CatsAndMiceForm";
            this.Text = "P4_CatsAndMice_EnriqueFernandezSanchez";
            this.Load += new System.EventHandler(this.CatsAndMiceForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CatsAndMiceForm_KeyDown);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel boardPanel;

        ExecutionMode executionMode = ExecutionMode.TimerTick;
        int status = -1;

        private Timer MyTimer = new Timer();
        //private int counter = 0;

        IGameLogic game;
        GameMode gameMode = GameMode.ClientDriven;
        
        //Default 20
        int cellSize = 28;

        private void CatsAndMiceForm_Load(object sender, System.EventArgs e)
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            this.boardPanel.Size = new System.Drawing.Size(GameLogic.MaxColumna * cellSize, GameLogic.MaxFila * cellSize);

            this.UpdateStyles();
            MyTimer.Interval = 110;
            MyTimer.Enabled = true;
            MyTimer.Tick += new EventHandler(Timer_Tick);
        }

        /// <summary>
        /// 
        /// Inicia un nuevo juego con el ratón dirigido por teclado o con ratón autónomo
        /// El modo se puede cambiar después desde teclado.
        /// </summary>
        /// <param name="mode"></param>
        private void startGame(GameMode mode)
        {
            game = new GameLogic();
            status = 0;
            gameMode = mode;
            
            if (executionMode == ExecutionMode.TimerTick)
            {
                MyTimer.Start();
            }
        }

        //Implementamos la condición de comer todas la frutas.
        private void executeGameStep(GameMode mode) {
            if (status == 100) {
                //When win
                boardPanel.BackgroundImage = new Bitmap("../../resources/background/gnu-love_frog_win.jpg");
                Console.WriteLine("No more Fruits on board");
                Console.WriteLine("**** WIN ****");
                Console.WriteLine("");
                Console.WriteLine("*** Eat all the fruits to win ***");
                Console.WriteLine("Start a new game pressing <n> for keyboard driven or <m> for autonomous mouse");
                MyTimer.Stop();
                status = -1;
                Refresh();   
            }
            if (status == -2) {
                //When die
                this.boardPanel.BackgroundImage = new Bitmap("../../resources/background/gnu-love_visual_win.jpg"); 
                //System.Console.WriteLine("Game is over. Status = " + status);
                System.Console.WriteLine("");
                System.Console.WriteLine("*** Eat all the fruits to win ***");
                System.Console.WriteLine("Start a new game pressing <n> for keyboard driven or <m> for autonomous mouse");
                MyTimer.Stop();
            }
            else if (status == 0){
                status = game.ExecuteStep(mode);
                Refresh();
            }
            else {
                //First boot
                this.boardPanel.BackgroundImage = new Bitmap("../../resources/background/gnu-love_inicio.jpg"); 
                //System.Console.WriteLine("Game is over. Status = " + status);
                System.Console.WriteLine("");
                System.Console.WriteLine("*** Eat all the fruits to win ***");
                System.Console.WriteLine("Start a new game pressing <n> for keyboard driven or <m> for autonomous mouse");
                MyTimer.Stop();  
            }
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            executeGameStep(gameMode);
        }

        private void CatsAndMiceForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //System.Console.WriteLine(e.KeyValue);

            // Start new game..................................................................................

            if (e.KeyValue == 78)        // 'n' empieza un nuevo juego en modo teclado (client driven)
            {
                boardPanel.BackgroundImage = new Bitmap("../../resources/background/gnu-love.jpg"); 
                if (status < 0)
                {
                    startGame(GameMode.ClientDriven);
                }
                else
                {
                    System.Console.WriteLine("Game is running. Impossible to start new game Status = " + status);                   
                }
            }
            else if (e.KeyValue == 77)    // 'm' empieza un nuevo juego con ratón autónomo
            {
                boardPanel.BackgroundImage = new Bitmap("../../resources/background/gnu-love.jpg");
                if (status < 0)
                {       
                    startGame(GameMode.AutonomousMouse);
                }
                else
                {
                    System.Console.WriteLine("Game is running. Impossible to start new game Status = " + status);
                }
            }

            // Change Execution Mode if required ....................................................................

            else if (e.KeyValue == 83 && status >= 0)   // 's' --> Juego avanza con barra espaciadora.
            {
                executionMode = ExecutionMode.SpaceClick;
                MyTimer.Stop();
            }
            else if (e.KeyValue == 84 && status >= 0)   // 't' --> juego avanza con timer (valor por defecto)
            {
                executionMode = ExecutionMode.TimerTick;
                MyTimer.Start();
            }

            // Set mouse direction if game mode is Client Driven
            else if (e.KeyValue == 37 && status >= 0 && gameMode == GameMode.ClientDriven)
            {
                game.SetMouseDirection(Direction.Left);
            }
            else if (e.KeyValue == 38 && status >= 0 && gameMode == GameMode.ClientDriven)
            {
                game.SetMouseDirection(Direction.Up);
            }
            else if (e.KeyValue == 39 && status >= 0 && gameMode == GameMode.ClientDriven)
            {
                game.SetMouseDirection(Direction.Right);
            }               
            else if (e.KeyValue == 40 && status >= 0 && gameMode == GameMode.ClientDriven)
            {
                game.SetMouseDirection(Direction.Down);
            }
               
            // Update game in SpaceClick executionMode.
            if (executionMode == ExecutionMode.SpaceClick)
            {
                executeGameStep(gameMode);
            }
            e.Handled = true;    
        }

        private void CatsAndMiceForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (status < 0)
            {
                return;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            // e.Graphics.Clear(BackColor);

            IEnumerator<IGameItem> enumerator = game.GetEnumerator();

            foreach (var item in game)
            {
                if (item != null)
                {
                    ViewFactory.GetView(item, cellSize).draw(e);
                }               
            }
        }
    }
}

