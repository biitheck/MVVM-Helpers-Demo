using MVVMHelpersDemo.Views;
using Xamarin.Forms;

namespace MVVMHelpersDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new UsersPage())
            {
                BarTextColor = Color.White,
                BarBackgroundColor = Color.FromHex("#DF7401")
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
