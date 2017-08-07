using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MVVMHelpersDemo.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVVMHelpersDemo.Services
{
    public class UsersService
    {
        private static HttpClient _client;
        private static UsersService _instance;
        private static string ApiEndpoint = "https://randomuser.me/api/";

        /// <summary>
        /// Constructor
        /// </summary>
        public UsersService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.MaxResponseContentBufferSize = 9999999;
        }

        /// <summary>
        /// Obtener la instancia de UsersService.
        /// </summary>
        /// <value>La instancia.</value>
        public static UsersService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UsersService();

                return _instance;
            }
        }

        /// <summary>
        /// Obtener Usuarios.
        /// </summary>
        /// <returns>Lista  de Usuarios.</returns>
        /// <param name="results">Resultados.</param>
        /// <param name="gender">Genero.</param>
        public async Task<IEnumerable<User>> GetAsync(int results, string gender)
        {
            IEnumerable<User> items = new List<User>();

            var uri = new Uri($"{ApiEndpoint}/?results={results}&gender={gender}");

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var j = JsonConvert.DeserializeObject<JObject>(content);
                    var dr = j["results"];

                    items = JsonConvert.DeserializeObject<IEnumerable<User>>(dr.ToString());
                }
            }
            catch
            {
                throw;
            }

            return items;
        }
    }
}
