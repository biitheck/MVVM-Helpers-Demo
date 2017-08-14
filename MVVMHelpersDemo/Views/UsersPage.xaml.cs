using System;
using System.Collections.Generic;
using MVVMHelpersDemo.Models;
using MVVMHelpersDemo.ViewModels;
using Xamarin.Forms;

namespace MVVMHelpersDemo.Views
{
    public partial class UsersPage : ContentPage
    {
        public UsersPage()
        {
            InitializeComponent();

            this.BindingContext = new UsersViewModel();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var item = (User)e.SelectedItem;
                await this.DisplayAlert(item.fullName,
                           $"Phone: {item.phone}\nCell: {item.cell}", "OK");
            }
        }
    }
}
