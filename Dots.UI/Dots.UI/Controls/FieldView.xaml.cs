using System.Collections.Generic;
using System.Windows.Input;
using Dots.Core.Field;
using Dots.Core.Game;
using Dots.UI.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dots.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FieldView : ContentView, IGameFieldPainter
    {
        #region Fields

        private static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create("TappedCommand", typeof(ICommand), typeof(DotView));

        private List<List<DotView>> _controls;
        private int _fieldSize;
        private Grid _grid;

        #endregion

        #region Constructors

        public FieldView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public ICommand TappedCommand
        {
            get => (ICommand) GetValue(TappedCommandProperty);
            set => SetValue(TappedCommandProperty, value);
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

                Content = _grid;
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
                            TappedCommand = TappedCommand
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
        }

        #endregion
    }
}