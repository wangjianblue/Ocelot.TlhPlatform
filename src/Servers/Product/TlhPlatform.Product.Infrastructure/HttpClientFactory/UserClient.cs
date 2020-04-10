using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using Microsoft.Net.Http.Headers;

namespace TlhPlatform.Product.Infrastructure.HttpClientFactory
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _client;

        public UserClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.UserClient.com/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            _client = httpClient;
        }

        public async Task<string> GetData(string url)
        {
            return await _client.GetStringAsync("/");
        }
        public async Task<HttpResponseMessage> PostAsync(string url, Func<HttpContent> contentFunc)
        { 
            if (string.IsNullOrWhiteSpace(url))
            {
                return null; 
            }
            return await _client.PostAsync(url, contentFunc());
        }
    }
}
