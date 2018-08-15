using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using SnakeUWP.Annotations;
using SnakeUWP.Model;

namespace SnakeUWP.ViewModel
{
    /// <summary>
    /// ViewModel which is used as DataContext for MainPage
    /// </summary>
    class MainPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            NewGame();
            Width = Height = Settings.SideSize;
            CoreApplication.MainView.CoreWindow.KeyDown += OnKeyUp;
        }

        /// <summary>The _snake property represents snake object.</summary>
        private Snake _snake;

        /// <summary>The _food property represents food object.</summary>
        private Food _food;

        /// <summary>The _timer property represents ThreadPoolTimer which is used to call method <see cref="MoveSnake"/> and <see cref="Draw"/>.</summary>
        private ThreadPoolTimer _timer;

        /// <summary>Get dispatcher.</summary>
        private readonly CoreDispatcher _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

        /// <summary>The Width property represents width of an image.</summary>
        public int Width { get; set; }

        /// <summary>The Width property represents width of an image.</summary>
        public int Height { get; set; }

        /// <summary>The _imageSource property represents an image.</summary>
        private WriteableBitmap _imageSource = BitmapFactory.New(Settings.SideSize, Settings.SideSize);

        /// <summary>
        /// Gets or sets the _imageSource.
        /// </summary>
        public WriteableBitmap ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        /// <summary>The _points property represents number of points.</summary>
        private int _points;

        /// <summary>
        /// Gets or sets the _points.
        /// </summary>
        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                if (_dispatcher.HasThreadAccess)
                {
                    OnPropertyChanged(nameof(Points));
                }
                else
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        OnPropertyChanged(nameof(Points));
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        
        }

        /// <summary>
        /// Reset settings to default.
        /// </summary>
        public void NewGame()
        {
            Points = 0;
            _snake = new Snake();
            NewFood();
            Draw();
            CreateTimer();
        }

        /// <summary>
        /// All operations needed for snake to move.
        /// </summary>
        public void MoveSnake()
        {
            Rectangle head = _snake.SnakeParts.First().Clone();
            switch (_snake.SnakeDirection)
            {
                case Direction.Top:
                    --head.Y1;
                    --head.Y2;
                    if (head.Y2 <= 0)
                    {
                        head.Y1 = Settings.NumberOfCellsPerSide - 1;
                        head.Y2 = Settings.NumberOfCellsPerSide;
                    }
                    break;
                case Direction.Bottom:
                    ++head.Y1;
                    ++head.Y2;
                    if (head.Y1 >= Settings.NumberOfCellsPerSide)
                    {
                        head.Y1 = 0;
                        head.Y2 = 1;
                    }
                    break;
                case Direction.Left:
                    --head.X1;
                    --head.X2;
                    if (head.X2 <= 0)
                    {
                        head.X1 = Settings.NumberOfCellsPerSide - 1;
                        head.X2 = Settings.NumberOfCellsPerSide;
                    }
                    break;
                case Direction.Right:
                    ++head.X1;
                    ++head.X2;
                    if (head.X1 >= Settings.NumberOfCellsPerSide)
                    {
                        head.X1 = 0;
                        head.X2 = 1;
                    }
                    break;
            }
            _snake.SnakeParts.AddFirst(head);
            if (IsEaten())
            {
                Points = Points + (1 * Settings.PointMultipier);
                if (_snake.SnakeParts.Count != (Settings.NumberOfCellsPerSide * Settings.NumberOfCellsPerSide))
                    NewFood();
            }
            else
            {
                _snake.SnakeParts.RemoveLast();
                for (int i = 1; i < _snake.SnakeParts.Count; i++)
                {
                    if (head.X1 == _snake.SnakeParts.ElementAt(i).X1 && head.Y1 == _snake.SnakeParts.ElementAt(i).Y1)
                    {
                        _timer.Cancel();
                    }
                }
            }   
        }

        /// <summary>
        /// All operations needed for draw.
        /// </summary>
        public void Draw()
        {
            using (ImageSource.GetBitmapContext())
            {
                ImageSource.Clear(Colors.White);
                foreach (var part in _snake.SnakeParts)
                {
                    int snakeX1 = part.X1 * Settings.CellSize;
                    int snakeY1 = part.Y1 * Settings.CellSize;
                    int snakeX2 = part.X2 * Settings.CellSize;
                    int snakeY2 = part.Y2 * Settings.CellSize;
                    ImageSource.DrawRectangle(snakeX1, snakeY1, snakeX2, snakeY2, part.Color);
                    for (int i = snakeX1; i < snakeX2; i++)
                        for (int j = snakeY1; j < snakeY2; j++)
                            ImageSource.SetPixel(i, j, part.Color);
                }

                int foodX = _food.X * Settings.CellSize + Settings.CellSize / 2;
                int foodY = _food.Y * Settings.CellSize + Settings.CellSize / 2;
                ImageSource.FillEllipseCentered(foodX, foodY, _food.Width, _food.Height, _food.Color);
            }
        }

        /// <summary>
        /// All operation needed to generate random coordinates for food.
        /// </summary>
        public void NewFood()
        {
            bool isOverlap;
            int x;
            int y;
            Random rnd = new Random();
            do
            {
                isOverlap = false;
                x = rnd.Next(0, Settings.NumberOfCellsPerSide);
                y = rnd.Next(0, Settings.NumberOfCellsPerSide);

                foreach (var part in _snake.SnakeParts)
                {
                    if (x == part.X1 && y == part.Y1)
                        isOverlap = true;
                }
            } while (isOverlap);
            _food = new Food(x, y);
        }

        /// <summary>
        /// Checking if the snake is eating food.
        /// </summary>
        public bool IsEaten()
        {
            if (_snake.SnakeParts.First().X1 == _food.X && _snake.SnakeParts.First().Y1 == _food.Y)
                return true;
            return false;
        }

        /// <summary>
        /// Shows Message dialog with score after game is over.
        /// </summary>
        public async void GameOver()
        {
            var dialog = new MessageDialog($"Your score: {Points}") {Title = "Game over"};
            dialog.Commands.Add(new UICommand { Label = "New Game", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Close application", Id = 1 });
            var result = await dialog.ShowAsync();

            if ((int)result.Id == 0)
                NewGame();
            else if ((int)result.Id == 1)
                Application.Current.Exit();
        }

        /// <summary>
        /// Creates periodic timer needed to call methods.
        /// </summary>
        public void CreateTimer()
        {
            _timer = ThreadPoolTimer.CreatePeriodicTimer(async (t) =>
                {
                    MoveSnake();
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, Draw);
                }, TimeSpan.FromMilliseconds(Settings.RefreshTaskPeriod),
                async (t) =>
                {
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, GameOver);
                });
        }

        /// <summary>
        /// Raises the KeyUp event.
        /// </summary>
        public void OnKeyUp(CoreWindow sender, KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case VirtualKey.Left:
                    if(_snake.SnakeDirection != Direction.Right) _snake.SnakeDirection = Direction.Left;
                    break;
                case VirtualKey.Right:
                    if (_snake.SnakeDirection != Direction.Left)  _snake.SnakeDirection = Direction.Right;
                    break;
                case VirtualKey.Up:
                    if (_snake.SnakeDirection != Direction.Bottom)  _snake.SnakeDirection = Direction.Top;
                    break;
                case VirtualKey.Down:
                    if (_snake.SnakeDirection != Direction.Top)  _snake.SnakeDirection = Direction.Bottom;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
