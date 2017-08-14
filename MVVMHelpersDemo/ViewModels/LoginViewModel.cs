using System;
using System.Windows.Input;
using MvvmHelpers;
using MVVMHelpersDemo.Views;
using Xamarin.Forms;

namespace MVVMHelpersDemo.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _signInSignOutText;

        public string SignInSignOutText
        {
            get { return _signInSignOutText; }
            set
            {
                _signInSignOutText = value;
                OnPropertyChanged();
            }
        }

        public async void OnAppearing(object navigationContext)
        {

            var IsAuthenticated = await Services.AuthService.SignInAsync();

            if (IsAuthenticated)
            {
                SignInSignOutText = "Salir";
                IsBusy = false;
                Application.Current.MainPage = new NavigationPage(new UsersPage())
                {
                    BarTextColor = Color.White,
                    BarBackgroundColor = Color.FromHex("#DF7401")
                };
            }

            if (!IsAuthenticated)
                SignInSignOutText = "Ingresar";
            else
                SignInSignOutText = "Salir";

        }

        public ICommand SignInSignOutCommand => new Command(SignInSignOutAsync);

        private async void SignInSignOutAsync()
        {
            if (this.IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (SignInSignOutText == "Ingresar")
                {
                    var IsAuthenticated = await Services.AuthService.SignInAsync();

                    if (IsAuthenticated)
                    {
                        SignInSignOutText = "Salir";
                        Application.Current.MainPage = new NavigationPage(new UsersPage())
                        {
                            BarTextColor = Color.White,
                            BarBackgroundColor = Color.FromHex("#DF7401")
                        };
                    }
                }
                else
                    SignInSignOutText = "Salir";
            }
            catch
            {
                throw;
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
