using System;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository: IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }

            //Deserialize string from redis
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            if (basket == null)
            {
                return null;
            }

            var serializedBasket = JsonConvert.SerializeObject(basket);
            await _redisCache.SetStringAsync(basket.UserName, serializedBasket);

            return await GetBasket(basket.UserName);
        }
    }
}

