using Dots.Core.Field;
using Dots.Core.Game;
using Dots.UI.Controls;
using Dots.UI.Models;
using Xamarin.Forms;

namespace Dots.UI
{
    public partial class MainPage : ContentPage, IGameFieldPainter
    {
        #region Fields

        private readonly Game _game;

        #endregion

        #region Constructors

        public MainPage()
        {
            InitializeComponent();

            _game = new Game(this);

            _game.Initialyze(10);

            _game.Paint();
        }

        #endregion

        #region Methods

        public void Paint(Field field)
        {
            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.DarkGray,
                Padding = new Thickness(10, 10, 10, 10),
                RowSpacing = 10,
                ColumnSpacing = 10
            };

            for (var i = 0; i < field.Size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition {Height = 40});
                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = 40});
            }

            for (var i = 0; i < field.Size; i++)
            for (var j = 0; j < field.Size; j++)
            {
                var dotView = new DotView
                {
                    Source = new DotModel
                    {
                        Row = i,
                        Column = j,
                        Dot = field[i][j]
                    },
                    TappedCommand = new Command(dotModel =>
                    {
                        if (dotModel is DotModel model)
                        {
                            _game.MakeMove(model.Row, model.Column);
                            _game.Paint();
                        }
                    })
                };

                grid.Children.Add(dotView, j, i);
            }

            MoveLabel.Text = _game.FirstPlayerMove ? "First player" : "Second player";
            MoveLabel.TextColor = _game.FirstPlayerMove ? Color.Blue : Color.Brown;
            ScoresLabel.Text = $"Score: {_game.Result.FirstPlayerScore} : {_game.Result.SecondPlayerScore}";
            ParentGrid.Children.Add(grid, 0, 1);
        }

        #endregion
    }
}