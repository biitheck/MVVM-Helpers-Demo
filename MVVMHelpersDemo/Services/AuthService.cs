using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using MVVMHelpersDemo.Models;
using MVVMHelpersDemo.Views;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace MVVMHelpersDemo.Services
{
    public class AuthService
    {
        public static async Task<bool> SignInAsync()
        {
            try
            {
                AuthenticationResult authResult =
                            await App.PCA.AcquireTokenAsync(
                                AuthParameters.Scopes,
                                AuthService.GetUserByPolicy(App.PCA.Users, AuthParameters.DefaultPolicy),
                                App.UiParent);

                UpdateUserInfo(authResult);
                return true;
            }
            catch (MsalException ex)
            {
                if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                    OnForgotPassword();
                // Alert if any exception excludig user cancelling sign-in dialog
                //else if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
                return false;
            }
        }

        public static async Task<bool> VerifyAccessTokenAsync()
        {
            try
            {
                AuthenticationResult authResult =
                    await App.PCA.AcquireTokenSilentAsync(
                        AuthParameters.Scopes,
                        GetUserByPolicy(App.PCA.Users,
                                        AuthParameters.DefaultPolicy),
                        AuthParameters.Authority, false);

                UpdateUserInfo(authResult);

                return true;
            }
            catch(Exception ex)
            {
                App.AuthUser = null;
                Application.Current.MainPage = new LoginPage();
                //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }

            return false;
        }

		public static void SignOut()
		{
			foreach (var user in App.PCA.Users)
				App.PCA.Remove(user);

        //    Xamarin.Forms.DependencyService
        //               .Get<ISecureStorageCredentials>()
					   //.DeleteAccountAsync();

            Application.Current.MainPage = new LoginPage();
		}

        private static async void OnForgotPassword()
        {
            try
            {
                AuthenticationResult authResult =
                    await App.PCA.AcquireTokenAsync(AuthParameters.Scopes,
                                                    (IUser)null,
                                                    UIBehavior.SelectAccount,
                                                    string.Empty, null,
                                                    AuthParameters.AuthorityPasswordReset,
                                                    App.UiParent);
                UpdateUserInfo(authResult);
            }
            catch (Exception ex)
            {
                // Alert if any exception excludig user cancelling sign-in dialog
                //if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }

        private static void UpdateUserInfo(AuthenticationResult authResult)
        {
            // Guardar Credenciales de forma segura.
            //SaveAccount(authResult);

            // Obtener Credenciales guardadas.
            //Account account = RetrievingAccount();

            // Crear Objeto AuthUser
            App.AuthUser = new AuthenticatedUser();

            JObject user = ParseIdToken(authResult.IdToken);

			App.AuthUser.Name = user["name"]?.ToString();
			App.AuthUser.Id = user["oid"]?.ToString();
            App.AuthUser.AccessToken = authResult.IdToken;

			//App.AuthUser.Name = account.Properties["Username"];
			//App.AuthUser.Id = account.Username; // IdUser.
			//App.AuthUser.AccessToken = account.Properties["AccessToken"];
		}

        private static JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        public static IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower(), StringComparison.Ordinal))
                    return user;
            }

            return null;
        }

        private static string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        //public static void SaveAccount(AuthenticationResult authResult)
        //{
        //    if (authResult != null)
        //    {
        //        IUser u = authResult.User;
        //        JObject user = ParseIdToken(authResult.IdToken);
        //        var ExpiresOn = user["exp"]?.ToString();
        //        // Name = user["name"]?.ToString();
        //        // Id = user["oid"]?.ToString();

        //        Account account = new Account
        //        {
        //            Username = authResult.UniqueId
        //        };

        //        account.Properties.Add("DisplayableId", u.DisplayableId ?? "");
        //        account.Properties.Add("Identifier", u.Identifier ?? "");
        //        account.Properties.Add("IdentityProvider", u.IdentityProvider ?? "");
        //        account.Properties.Add("Username", u.Name);
        //        account.Properties.Add("AccessToken", authResult.IdToken);
        //        account.Properties.Add("ExpiresOn", ExpiresOn);

        //        // Dependency Service para guardar credenciales. 
        //        Xamarin.Forms.DependencyService
        //               .Get<ISecureStorageCredentials>()
        //               .SaveAccountAsync(account);
        //    }
        //}

        //public static Account RetrievingAccount()
        //{
        //    return Xamarin.Forms.DependencyService
        //                      .Get<ISecureStorageCredentials>()
        //                      .RetrievingAccount();
        //}
    }
}
