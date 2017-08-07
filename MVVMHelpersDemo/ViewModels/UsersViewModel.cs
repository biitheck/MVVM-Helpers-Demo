using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers;
using MVVMHelpersDemo.Models;
using MVVMHelpersDemo.Services;
using Xamarin.Forms;

namespace MVVMHelpersDemo.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        #region Attributes
        private IEnumerable<User> _users { get; set; }
        #endregion

        #region Attributes

        public ObservableRangeCollection<User> Users { get; set; }
        public Command GetUsersCommand { get; }

        #endregion

        #region Constructor

        public UsersViewModel()
        {
            this.Title = "Users";
            this.PlaceHolderFilter = "Escriba aquí para filtrar";
            this.Users = new ObservableRangeCollection<User>();
            // Asignar comando.
            this.GetUsersCommand = new Command(async () => await GetUsersAsync());
        }

        #endregion

        #region Private Methods

        private async Task GetUsersAsync()
        {
            if (this.IsBusy)
                return;

            try
            {
                this.IsBusy = true;

                this._users = await UsersService.Instance.GetAsync(300, "female");

                this.SetUsersInfo(this._users);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void SetUsersInfo(IEnumerable<User> items)
        {
            this.Users.ReplaceRange(items);

            this.Sort();

            this.Title = $"{Users.Count} Users in {UsersGrouped.Count} groups";
        }

        private void Filter()
        {
            if (this._users != null)
            {
                if (string.IsNullOrEmpty(this._textFilter))
                {
                    this.SetUsersInfo(this._users);
                }
                else
                {
                    var filtered = this._users.Where(w =>
                       w.fullName.ToLower().Contains(_textFilter.ToLower()) ||
                       w.email.ToLower().Contains(_textFilter.ToLower()) ||
                       w.phone.ToLower().Contains(_textFilter.ToLower()) ||
                       w.cell.ToLower().Contains(_textFilter.ToLower()));

                    this.SetUsersInfo(filtered);
                }
            }
        }

        #endregion

        #region Grouping


        public ObservableRangeCollection<Grouping<string, User>> UsersGrouped { get; }
                                = new ObservableRangeCollection<Grouping<string, User>>();

        void Sort()
        {
            var sorted = from u in this.Users
                         orderby u.fullName
                         group u by u.nameSort into userGroup
                         select new Grouping<string, User>(userGroup.Key, userGroup);

            this.UsersGrouped.ReplaceRange(sorted);
        }

        #endregion

        #region Filter

        private string _placeHolderFilter;
        private string _textFilter;

        public string TextFilter
        {
            get
            {
                return this._textFilter;
            }
            set
            {
                if (this._textFilter != value)
                    if (SetProperty(ref this._textFilter, value))
                        this.Filter();
            }
        }

        public string PlaceHolderFilter
        {
            get
            {
                return this._placeHolderFilter;
            }
            set
            {
                if (this._placeHolderFilter != value)
                    SetProperty(ref this._placeHolderFilter, value);
            }
        }

        #endregion
    }
}
