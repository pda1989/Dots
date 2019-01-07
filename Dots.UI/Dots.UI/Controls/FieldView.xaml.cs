using Dots.Core.Field.Models;
using Dots.UI.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dots.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FieldView : ContentView
    {
        public static readonly BindableProperty CellTappedProperty =
            BindableProperty.Create(nameof(CellTapped), typeof(ICommand), typeof(FieldView));

        public static readonly BindableProperty FieldColorProperty =
            BindableProperty.Create(nameof(FieldColor), typeof(Color), typeof(FieldView), Color.White);

        public static readonly BindableProperty FieldProperty =
                    BindableProperty.Create(nameof(Field), typeof(Field), typeof(FieldView));

        private List<List<DotView>> _controls;
        private int _fieldSize;
        private Grid _grid;

        public FieldView()
        {
            InitializeComponent();
        }

        public ICommand CellTapped
        {
            get => (ICommand)GetValue(CellTappedProperty);
            set => SetValue(CellTappedProperty, value);
        }

        public Field Field
        {
            get => (Field)GetValue(FieldProperty);
            set => SetValue(FieldProperty, value);
        }

        public Color FieldColor
        {
            get => (Color)GetValue(FieldColorProperty);
            set => SetValue(FieldColorProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CellTappedProperty.PropertyName)
                _controls?.ForEach(row =>
                    row.ForEach(control =>
                        control.TappedCommand = CellTapped));

            if (propertyName == FieldProperty.PropertyName)
                Paint();

            if (propertyName == FieldColorProperty.PropertyName && _grid != null)
                _grid.BackgroundColor = FieldColor;
        }

        private void Paint()
        {
            if (Field == null)
                return;

            if (_grid == null)
            {
                _grid = new Grid
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = FieldColor,
                    Padding = new Thickness(10, 10, 10, 10),
                    RowSpacing = 10,
                    ColumnSpacing = 10
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

            if (_fieldSize != Field.Size)
            {
                _controls = new List<List<DotView>>();

                _grid.RowDefinitions.Clear();
                _grid.ColumnDefinitions.Clear();

                for (var i = 0; i < Field.Size; i++)
                {
                    _grid.RowDefinitions.Add(new RowDefinition());
                    _grid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (var i = 0; i < Field.Size; i++)
                {
                    _controls.Add(new List<DotView>());
                    for (var j = 0; j < Field.Size; j++)
                    {
                        var dotView = new DotView
                        {
                            Source = new DotModel
                            {
                                Row = i,
                                Column = j,
                                Dot = Field[i][j]
                            }
                        };

                        _grid.Children.Add(dotView, j, i);
                        _controls[i].Add(dotView);
                    }
                }

                _fieldSize = Field.Size;
            }

            for (var i = 0; i < Field.Size; i++)
                for (var j = 0; j < Field.Size; j++)
                {
                    if (_controls[i][j].Source?.Dot != Field[i][j])
                        _controls[i][j].Source = new DotModel
                        {
                            Row = i,
                            Column = j,
                            Dot = Field[i][j]
                        };
                    _controls[i][j].TappedCommand = CellTapped;
                }
        }
    }
}