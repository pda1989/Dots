using System;
using System.Collections.Generic;
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
        private List<List<DotView>> _controls;
        private int _fieldSize;
        private Grid _grid;

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
            if (_grid == null)
            {
                _grid = new Grid
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Color.DarkGray,
                    Padding = new Thickness(10, 10, 10, 10),
                    Margin = new Thickness(10, 10, 10, 10),
                    RowSpacing = 5,
                    ColumnSpacing = 5
                };

                _grid.SizeChanged += (s, e) =>
                {
                    if (Height <= Width)
                    {
                        _grid.WidthRequest = _grid.Height;
                        _grid.HeightRequest = _grid.Height;
                    }
                    else
                    {
                        _grid.HeightRequest = _grid.Width;
                        _grid.WidthRequest = _grid.Width;
                    }
                };
            }

            if (_fieldSize != field.Size)
            {
                _controls = new List<List<DotView>>();

                _grid.RowDefinitions.Clear();
                _grid.ColumnDefinitions.Clear();

                for (var i = 0; i < field.Size; i++)
                {
                    _grid.RowDefinitions.Add(new RowDefinition());
                    _grid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (var i = 0; i < field.Size; i++)
                {
                    _controls.Add(new List<DotView>());
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
                                    try
                                    {
                                        _game.MakeMove(model.Row, model.Column);
                                        _game.Paint();
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                            })
                        };

                        _grid.Children.Add(dotView, j, i);
                        _controls[i].Add(dotView);
                    }
                }

                _fieldSize = field.Size;
            }

            for (var i = 0; i < field.Size; i++)
            for (var j = 0; j < field.Size; j++)
                if (_controls[i][j].Source?.Dot != field[i][j])
                    _controls[i][j].Source = new DotModel
                    {
                        Row = i,
                        Column = j,
                        Dot = field[i][j]
                    };

            MoveLabel.Text = _game.FirstPlayerMove ? "First player" : "Second player";
            MoveLabel.TextColor = _game.FirstPlayerMove ? Color.Blue : Color.Brown;
            ScoresLabel.Text = $"Score: {_game.Result.FirstPlayerScore} : {_game.Result.SecondPlayerScore}";
            ParentGrid.Children.Add(_grid, 0, 1);
        }

        #endregion
    }
}