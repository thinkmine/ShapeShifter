using ShapeShifter.Library;
using ShapeShifter.Library.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.Networking.Vpn;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShapeShifter.UI.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameField : Page
    {
        public event Action NoAvailableSpace,
            ScoreChanged,
            NextShapeCalculated,
            ShapeWedged,
            ShapeDescended,
            ShapeShifted,
            ShapeRotated,
            ShapeTurbo,
            LevelChanged,
            Pulse;


        public bool IsGameRunning { get; set; }
        public double GameSpeed { get; set; } = 1;

        public int Score { get; private set; } = 0;

        public int LevelChangeScore { get; set; } = 100;

        public int CurrentLevel { get; private set; } = 1;

        public int ScoreIncrement { get; set; } = 10;

        public int ShapeStartRow { get; set; } = 2;
        public int ShapeStartColumn { get; set; } = 4;

        public int ShapeDroughtMax { get; set; } = 7;

        public double GameAcceleration { get; set; } = 1.0;
        Shape ActiveShape { get; set; }
        internal Shape CurrentPreviewShape { get; set; }

        internal BlockCollection Blocks { get; set; }

        public Brush FieldBackground { get; set; }

        public int GameHeight { get; set; } = 20;
        public int GameWidth { get; set; } = 10;

        DispatcherTimer _game_timer;
        Random _random = new Random();
        List<Func<Shape>> _shapes = new List<Func<Shape>>
            {
                () => new LShape(),
                () =>new JShape(),
                () =>new TShape(),
                () => new OShape(),
                () => new IShape(),
                () => new SShape(),
                () => new ZShape(),
            };

        Dictionary<int, Queue<int>> _shape_frequency = new Dictionary<int, Queue<int>>();

        public List<FieldLineManager> LineManagers { get; set; }

        public GameField()
        {
            ScoreIncrement = GameWidth;
            this.InitializeComponent();

            this.Loaded += (sender, args) => LoadGameComponents();
        }

        void LoadGameComponents()
        {
            IsGameRunning = false;

            if (_game_timer != null)
                _game_timer.Stop();

            LineManagers = new List<FieldLineManager>();

            Blocks = new BlockCollection(this.GameWidth, this.GameHeight);
            //populate columns
            foreach (int game_width in Enumerable.Range(0, this.GameWidth))
            {
                ColumnDefinition col_def = new ColumnDefinition();
                col_def.Width = new GridLength(30, GridUnitType.Pixel);
                LayoutRoot.ColumnDefinitions.Add(col_def);
            }

            //populate rows
            foreach (var game_height in Enumerable.Range(0, GameHeight))
            {
                RowDefinition row_def = new RowDefinition();
                row_def.Height = new GridLength(30, GridUnitType.Pixel);
                LayoutRoot.RowDefinitions.Add(row_def);
            }

            //add block controls
            int index = 0;
            foreach (var game_row in Enumerable.Range(0, this.GameHeight))
            {
                LineManagers.Add(new FieldLineManager(this.GameWidth, game_row, this));

                foreach (var game_column in Enumerable.Range(0, this.GameWidth))
                {
                    Block block = new Block(this);
                    block.SetValue(Grid.ColumnProperty, game_column);
                    block.SetValue(Grid.RowProperty, game_row);

                    //add to children column
                    LayoutRoot.Children.Add(block);
                    Blocks.Add(block, game_column, game_row);
                    index++;
                }
            }


            _game_timer = new DispatcherTimer();
            _game_timer.Interval = TimeSpan.FromSeconds(this.GameSpeed);
            _game_timer.Tick += (sender, args) =>
            {
                Pulse?.Invoke();
                ActiveShape?.Descend(this);
            };
        }

        Shape GenerateNextShape()
        {
            var random_number = _random.Next(100);
            int shape_id = random_number % _shapes.Count;

            //see if there are any droughts to address
            foreach (var shape_index in _shape_frequency.Keys)
            {
                if (_shape_frequency[shape_index].Count >= ShapeDroughtMax)
                {
                    //reset the shape to this
                    shape_id = shape_index;
                    _shape_frequency[shape_index].Clear();
                    break;
                }
            }

            var selected_shape = _shapes[shape_id].Invoke();

            //now increment the drought number for every shape other the one that was selected
            foreach (var shape_index in _shape_frequency.Keys)
            {
                if (shape_id != shape_index)
                    _shape_frequency[shape_index].Enqueue(0);
            }


            return selected_shape;

        }

        public void Clear()
        {
            foreach (var game_row in Enumerable.Range(0, this.GameHeight))
            {
                foreach (var game_column in Enumerable.Range(0, this.GameWidth))
                {
                    Blocks[game_column, game_row].Clear();
                }
            }
        }

        public void InitializeActiveShape()
        {
            ActiveShape.Row = ShapeStartRow;
            ActiveShape.Column = ShapeStartColumn;
            if (ActiveShape.CanBeAdded(this))
            {
                ActiveShape.Rotated += () => ShapeRotated?.Invoke();
                ActiveShape.Shifted += () => ShapeShifted?.Invoke();
                ActiveShape.Descended += () => ShapeDescended?.Invoke();
                ActiveShape.Wedged += shape =>
                {
                    ActiveShape = null;
                    GameAcceleration = 1.0;
                    _game_timer.Interval = TimeSpan.FromSeconds(this.GameSpeed * GameAcceleration);
                    EvaluateLines();

                    ActiveShape = CurrentPreviewShape;
                    CurrentPreviewShape = GenerateNextShape();
                    NextShapeCalculated?.Invoke();
                    InitializeActiveShape();
                    ScoreChanged?.Invoke();

                    //increment the score
                    
                };

                ActiveShape.Draw(this);
            }
            else
            {
                NoAvailableSpace?.Invoke();
            }
        }

        public void PreviewShape(Shape preview_shape)
        {
            //show in preview mode
            Clear();
            preview_shape.Column = 2;
            preview_shape.Row = 3;
            preview_shape.Draw(this);
        }

        void EvaluateLines()
        {
            for (int line = GameHeight - 1; line >= 0; line--)
            {
                var line_manager = LineManagers[line];
                if (line_manager.IsFull)
                {
                    line_manager.ClearLine();
                    line_manager.ShiftDown();

                    Score += ScoreIncrement;
                    ScoreChanged?.Invoke();
                    if (Score % LevelChangeScore == 0)
                    {
                        CurrentLevel++;
                        LevelChanged?.Invoke();
                    }
                    //todo: restart again after lines are cleared (maybe)
                    line = GameHeight;
                }
            }
        }

        #region Shape Movement
        public void ShiftActiveShapeLeft()
        {
            ActiveShape?.ShiftLeft(this);
        }

        public void ShiftActiveShapeRight()
        {
            ActiveShape?.ShiftRight(this);
        }

        public void RotateActiveShape()
        {
            ActiveShape?.Rotate(this);
        }

        public void TurboActiveShape()
        {
            if (ActiveShape != null)
            {
                GameAcceleration = .5;
                _game_timer.Interval = TimeSpan.FromSeconds(this.GameSpeed * GameAcceleration);
                ActiveShape.Descend(this);
                ShapeTurbo?.Invoke();
            }
        }

        public void EndActiveShapeTurbo()
        {
            GameAcceleration = 1.0;
            _game_timer.Interval = TimeSpan.FromSeconds(this.GameSpeed * GameAcceleration);
            ActiveShape.Descend(this);
        }
        #endregion

        public void StartGame()
        {
            _shape_frequency = new Dictionary<int, Queue<int>>();
            foreach (var item in Enumerable.Range(0, 7))
            {
                _shape_frequency.Add(item, new Queue<int>());
            }

            ActiveShape = GenerateNextShape();
            ActiveShape.Row = ShapeStartRow;
            ActiveShape.Column = ShapeStartColumn;

            CurrentPreviewShape = GenerateNextShape();
            NextShapeCalculated?.Invoke();
            InitializeActiveShape();

            _game_timer.Start();
            IsGameRunning = true;
        }

        public void StopGame()
        {
            _game_timer.Stop();
            IsGameRunning = false;
        }

        public void Reset()
        {
            LayoutRoot.ColumnDefinitions.Clear();
            LayoutRoot.RowDefinitions.Clear();
            LayoutRoot.Children.Clear();
            LoadGameComponents();
        }

    }
}
