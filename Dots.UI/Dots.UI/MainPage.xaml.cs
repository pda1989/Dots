using Dots.Core.Field;
using Dots.Core.Game;
using Dots.UI.Controls;
using Dots.UI.Models;
using Xamarin.Forms;

namespace Dots.UI
{
    public partial class MainPage : ContentPage, IGameFieldPainter
    {
        private Game _game;
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
            var grid = new Grid();

            for (var i = 0; i < field.Size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (var i = 0; i < field.Size; i++)
            for (var j = 0; j < field.Size; j++)
            {
                var dotView = new DotView
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Source = new DotModel
                    {
                        I = i,
                        J = j,
                        Dot = field[i][j]
                    },
                    TappedCommand = new Command(dotModel =>
                    {
                        if (dotModel is DotModel model)
                        {
                            _game.MakeMove(model.I, model.J);
                            _game.Paint();
                           //DisplayAlert("Debug", $"Model: {model.I}:{model.J}", "OK");
                        }
                    })
                };

                grid.Children.Add(dotView, j, i);
            }

            Content = grid;
        }

        #endregion
    }
}