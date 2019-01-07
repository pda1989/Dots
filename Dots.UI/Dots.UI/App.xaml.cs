using Dots.UI.ViewModels;
using FreshMvvm;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Dots.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            AppCenter.Start("2c565b56-3ce5-4ff0-b998-8862eb707d7f", typeof(Analytics));

            MainPage = FreshPageModelResolver.ResolvePageModel<GamePageModel>();
        }

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
    }
}