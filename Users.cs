using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace distributed_caching_redis
{
    public interface IUsersService
    {
        Task<IEnumerable<Users>> GetUsersAsync();
    }
    public class UsersService : IUsersService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly HttpClient _httpClient;
        private const string CACHE_KEY = "users.cache.key";
        public UsersService(IDistributedCache distributedCache, HttpClient httpClient)
        {
            _distributedCache = distributedCache;
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            var usersCache = _distributedCache.GetString(CACHE_KEY);
            if (String.IsNullOrEmpty(usersCache))
            {
                var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _distributedCache.SetString(CACHE_KEY, responseBody);
                var res = JsonSerializer.Deserialize<IEnumerable<Users>>(responseBody);
                return res;
            }
            else
            {
                var res = JsonSerializer.Deserialize<IEnumerable<Users>>(usersCache);
                return res;
            }
        }
    }

    public class Users
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public Company company { get; set; }
    }

    public class Company
    {
        public string name { get; set; }
        public string catchPhrase { get; set; }
        public string bs { get; set; }
    }

    public class Address
    {
        public string street { get; set; }
        public string suite { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public Geo geo { get; set; }
    }
    
    public class Geo
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}