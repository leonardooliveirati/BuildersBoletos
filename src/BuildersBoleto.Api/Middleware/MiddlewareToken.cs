﻿using BuildersBoleto.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace BuildersBoleto.Api.Token
{
    public class MiddlewareToken
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public MiddlewareToken(RequestDelegate next, IMemoryCache cache, IConfiguration configuration)
        {
            _next = next;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_cache.TryGetValue("AuthToken", out string token))
            {
                token = await GetTokenAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); 

                _cache.Set("AuthToken", token, cacheEntryOptions);
            }

            context.Request.Headers["Authorization"] = token;

            await _next(context);
        }

        private async Task<string> GetTokenAsync()
        {
            using var client = new HttpClient();
            var request = new
            {
                client_id = _configuration["BoletoApi:ClientId"],
                client_secret = _configuration["BoletoApi:ClientSecret"]
            };

            var response = await client.PostAsync("https://vagas.builders/api/builders/auth/tokens",
                new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

            return tokenResponse.Token;
        }
    }
}
