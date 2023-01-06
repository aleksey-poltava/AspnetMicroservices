using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
	public class OrderService : IOrderService
	{
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await _client.GetAsync($"/Order/{userName}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<List<OrderResponseModel>>();
            else
            {
                throw new Exception("Something went wrong when calling API");
            }
        }
    }
}

