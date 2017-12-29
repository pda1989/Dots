using Dots.UI.ViewModels;
using FreshMvvm;
using Xamarin.Forms;

namespace Dots.UI
{
    public partial class App : Application
    {
        #region Constructors

        public App()
        {
            InitializeComponent();

            MainPage = FreshPageModelResolver.ResolvePageModel<GamePageModel>();
        }

        #endregion

        #region Methods

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        #endregion
    }
}