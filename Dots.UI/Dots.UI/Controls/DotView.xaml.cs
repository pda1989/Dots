using System.Windows.Input;
using Dots.UI.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dots.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DotView : ContentView
    {
        #region Fields

        private static readonly BindableProperty SourceProperty =
            BindableProperty.Create("Source", typeof(DotModel), typeof(DotView));

        private static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create("TappedCommand", typeof(ICommand), typeof(DotView));

        #endregion

        #region Constructors

        public DotView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public DotModel Source
        {
            get => (DotModel) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public ICommand TappedCommand
        {
            get => (ICommand) GetValue(TappedCommandProperty);
            set => SetValue(TappedCommandProperty, value);
        }

        #endregion

        #region Methods

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SourceProperty.PropertyName)
                UpdateDotView();
        }

        private void UpdateDotView()
        {
            var grid = new Grid();

            var label = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 12,
                FontAttributes = FontAttributes.Bold
            };
            if (Source.Dot.Chain)
                label.Text = "C";
            else if (Source.Dot.Closed)
                label.Text = "*";
            else
                label.Text = "";

            var control = new BoxView();
            switch (Source.Dot.Value)
            {
                case 0:
                    control.BackgroundColor = Color.AliceBlue;
                    break;
                case 1:
                    control.BackgroundColor = Color.Blue;
                    break;
                case 2:
                    control.BackgroundColor = Color.Brown;
                    break;
            }
            if (!Source.Dot.Active && Source.Dot.Value == 1)
                control.BackgroundColor = Color.CornflowerBlue;
            if (!Source.Dot.Active && Source.Dot.Value == 2)
                control.BackgroundColor = Color.DarkSalmon;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                TappedCommand?.Execute(Source);
            };
            control.GestureRecognizers.Add(tapGestureRecognizer);

            grid.Children.Add(control, 0, 0);
            grid.Children.Add(label, 0, 0);

            Content = grid;
        }

        #endregion
    }
}