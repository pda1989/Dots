using Dots.Core.Game;
using Dots.UI.Controls;
using Dots.UI.Models;
using Xamarin.Forms;

namespace Dots.UI
{
    public partial class MainPage : ContentPage
    {
        #region Fields

        private readonly Game _game;

        #endregion

        #region Constructors

        public MainPage()
        {
            InitializeComponent();

            var field = new FieldView
            {
                TappedCommand = new Command(dotModel =>
                {
                    if (dotModel is DotModel model)
                        try
                        {
                            _game?.MakeMove(model.Row, model.Column);
                            _game?.Paint();

                            if (_game != null)
                            {
                                MoveLabel.Text = _game.FirstPlayerMove ? "First player" : "Second player";
                                MoveLabel.TextColor = _game.FirstPlayerMove ? Color.Blue : Color.Brown;
                                ScoresLabel.Text =
                                    $"Score: {_game.Result.FirstPlayerScore} : {_game.Result.SecondPlayerScore}";
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                })
            };

            ParentGrid.Children.Add(field, 0, 1);

            _game = new Game(field);
            _game.Initialyze(10);
            _game.Paint();
        }

        #endregion
    }
}