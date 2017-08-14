using System;
using System.Collections.Generic;
using MVVMHelpersDemo.ViewModels;
using Xamarin.Forms;

namespace MVVMHelpersDemo.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            this.BindingContext = new LoginViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (LoginViewModel)this.BindingContext;

            if (viewModel != null)
                viewModel.OnAppearing(null);
        }
    }
}
