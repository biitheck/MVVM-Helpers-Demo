using System;
using System.Linq;
using Plugin.Connectivity;
using Newtonsoft.Json.Linq;
using MVVMHelpersDemo.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;


namespace MVVMHelpersDemo.Services
{
    public class TodoItemService
    {

        private IMobileServiceClient _client;
        private IMobileServiceSyncTable<TodoItem> _table;

        public TodoItemService()
        {
            this._client = new MobileServiceClient(GlobalSettings.EndPointMobileService);

            // Inicialización de SQLite local
            var store = new MobileServiceSQLiteStore("mvvmHelpers.db");
            store.DefineTable<TodoItem>();

            // Inicializa utilizando IMobileServiceSyncHandler.
            this._client.SyncContext.InitializeAsync(store);

            // Obtener tabla.
            this._table = this._client.GetSyncTable<TodoItem>();
        }

        public async Task<IEnumerable<TodoItem>> GetTableAsync()
        {
            var authResult = await App.PCA.AcquireTokenSilentAsync(
                       AuthParameters.ApiScopes,
                       AuthService.GetUserByPolicy(App.PCA.Users,
                                       AuthParameters.DefaultPolicy),
                       AuthParameters.Authority, false);

            var payload = new JObject();
            payload["access_token"] = authResult.AccessToken;

            try
            {
                var User = await this._client.LoginAsync(
            MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory,
            payload);
            }
            catch (Exception ex)
            {
                throw;
            }

            // Sincronizar con Azure.
            await SynchronizeAsync();


            // Leer la tabla.
            return await this._table.ReadAsync();
        }

        private async Task SynchronizeAsync()
        {
            // Comprobar es estado de conexion.
            if (!CrossConnectivity.Current.IsConnected)
                return;

            try
            {
                // Subir cambios a la base de datos remota
                //await this._client.SyncContext.PushAsync();

                // El primer parámetro es el nombre de la query utilizada intermanente por el client SDK para implementar sync.
                // Utiliza uno diferente por cada query en la App
                await this._table.PullAsync($"all{nameof(TodoItem)}", this._table.CreateQuery());
            }
            catch (MobileServicePushFailedException ex)
            {
                //if (ex.PushResult.Status == MobileServicePushStatus.CancelledByAuthenticationError)
                //{
                //    await LoginAsync();
                //    await SynchronizeAsync();
                //    return;
                //}

                if (ex.PushResult != null)
                    foreach (var result in ex.PushResult.Errors)
                        await ResolveErrorAsync(result);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                //if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    await LoginAsync();
                //    await SynchronizeAsync();
                //    return;
                //}

                throw;
            }
        }

        private async Task ResolveErrorAsync(MobileServiceTableOperationError result)
        {
            // Ignoramos si no podemos validar ambas partes.
            if (result.Result == null || result.Item == null)
                return;

            var serverItem = result.Result.ToObject<TodoItem>();
            var localItem = result.Item.ToObject<TodoItem>();

            //if (!IsDataChanged(serverItem, localItem))
            // Los elementos son iguales, ignoramos el conflicto
            //    await result.CancelAndDiscardItemAsync();
            //else
            // El Servidor manda.
            await result.UpdateOperationAsync(JObject.FromObject(serverItem));
        }

        private bool IsDataChanged(TodoItem serverItem, TodoItem localItem)
        {
            bool isDataChanged = false;

            return isDataChanged;
        }
    }
}