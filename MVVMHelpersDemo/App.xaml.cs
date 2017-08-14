using System.Linq;
using Microsoft.Identity.Client;
using MVVMHelpersDemo.Models;
using MVVMHelpersDemo.Views;
using Xamarin.Forms;

namespace MVVMHelpersDemo
{
    public partial class App : Application
    {
		public static PublicClientApplication PCA = null;
		public static UIParent UiParent = null;
		public static AuthenticatedUser AuthUser;
		public static readonly string AppName = "MVVM Helpers";

        public App()
        {
            InitializeComponent();
	
			PCA = new PublicClientApplication(AuthParameters.ClientID, AuthParameters.Authority);
			PCA.RedirectUri = $"msal{AuthParameters.ClientID}://auth";
        }

        protected override void OnStart()
        {
			// Handle when your app starts

			if (PCA != null && PCA.Users.Count() > 0)
				MainPage = new NavigationPage(new UsersPage())
				{
					BarTextColor = Color.White,
					BarBackgroundColor = Color.FromHex("#DF7401")
				};
			else
				MainPage = new LoginPage();
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
