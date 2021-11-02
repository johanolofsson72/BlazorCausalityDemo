using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorCausality
{
    /// <summary>
    /// Reusable API Repository base class that provides access to CRUD APIs
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ClientRepository<TEntity> : IClientRepository<TEntity> where TEntity : class
    {
        private readonly string controllerName = typeof(TEntity).Name.ToLower();
        private readonly string primaryKeyName = "id";
        private readonly HttpClient http;

        public ClientRepository(HttpClient _http) => http = _http;

        public async Task<List<TEntity>> GetAsync(string jsonQuery)
        {
            try
            {
                string requestUri = controllerName + "/get";
                HttpResponseMessage result = await http.PostAsJsonAsync(requestUri, jsonQuery);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                List<TEntity> response = JsonConvert.DeserializeObject<List<TEntity>>(responseBody);
                if (response is not null)
                {
                    return response;
                }
                else
                {
                    return new List<TEntity>();
                }
            }
            catch (Exception ex)
            {
                _ = ex.ToString();
                return null;
            }
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            try
            {
                string arg = WebUtility.HtmlEncode(id.ToString());
                string url = controllerName + "/" + arg;
                HttpResponseMessage result = await http.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                TEntity response = JsonConvert.DeserializeObject<TEntity>(responseBody);
                if (response is not null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            try
            {
                HttpResponseMessage result = await http.PostAsJsonAsync(controllerName, entity);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                TEntity response = JsonConvert.DeserializeObject<TEntity>(responseBody);
                if (response is not null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entityToUpdate)
        {
            try
            {
                HttpResponseMessage result = await http.PutAsJsonAsync(controllerName, entityToUpdate);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                TEntity response = JsonConvert.DeserializeObject<TEntity>(responseBody);
                if (response is not null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(TEntity entityToDelete)
        {
            try
            {
                string value = entityToDelete.GetType()
                    .GetProperty(primaryKeyName)
                    .GetValue(entityToDelete, null)
                    .ToString();

                string arg = WebUtility.HtmlEncode(value);
                string url = controllerName + "/" + arg;
                HttpResponseMessage result = await http.DeleteAsync(url);
                result.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            try
            {
                string url = controllerName + "/" + WebUtility.HtmlEncode(id.ToString());
                HttpResponseMessage result = await http.DeleteAsync(url);
                result.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
