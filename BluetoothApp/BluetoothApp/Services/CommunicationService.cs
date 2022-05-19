using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothApp.Services
{
    public interface ICommunicationService
    {
        Task<string> GetAsStringAsync(string url);
        Task<string> PostAsStringAsync(string url, HttpContent content);
        Task<T> GetAsObjectAsync<T>(string url);
        Task<T> PostAsObjectAsync<T>(string url, HttpContent content);
        Task<byte[]> GetAsBytesAsync(string url);
    }
    public class CommunicationService : ICommunicationService
    {
      
        public async Task<string> GetAsStringAsync(string url)
        {
            HttpContent res = await GetAsync(url);
            return await res?.ReadAsStringAsync();
        }
        public async Task<string> PostAsStringAsync(string url, HttpContent content)
        {
            HttpContent res = await PostAsync(url, content);
            return await res?.ReadAsStringAsync();
        }
        public async Task<TResponse> GetAsObjectAsync<TResponse>(string url)
        {
            HttpContent res = await GetAsync(url);
            if (res != null)
            {
                string resStr = await res.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(resStr);
            }
            return default;
        }
        public async Task<TResponse> PostAsObjectAsync<TResponse>(string url, HttpContent content)
        {
            HttpContent res = await PostAsync(url, content);
            string resStr = await res.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(resStr);
            
          
        }
        public async Task<byte[]> GetAsBytesAsync(string url)
        {
            HttpContent res = await GetAsync(url);
            return await res?.ReadAsByteArrayAsync();
        }

        //  Trys to get data. If token is expired trying to refresh. If refreshing failed logout. If refreshing done try to get data again. 
        //  In all other cases, if status code is not success just returns default value.
        private async Task<HttpContent> GetAsync(string url)
        {
            HttpResponseMessage resp = await GetHttpResponseMessageAsync(url);
            return resp.Content;
        }
        private async Task<HttpContent> PostAsync(string url, HttpContent content)
        {
            HttpResponseMessage resp = await PostHttpResponseMessageAsync(url, content);
            return resp.Content;
        }


        //  Just makes request and returns response.
        private async Task<HttpResponseMessage> GetHttpResponseMessageAsync(string url)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    return await client.GetAsync(url);
                }
            }
            catch (System.Exception)
            {
                return default;
            }

        }
        private async Task<HttpResponseMessage> PostHttpResponseMessageAsync(string url, HttpContent content)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    return await client.PostAsync(url, content);
                }
            }
            catch (System.Exception)
            {

                return default;
            }

        }
    }
}
