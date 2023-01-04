using System;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
	public interface ICatalogService
	{
		Task<IEnumerable<CatalogModel>> GetCatalog();
		Task<IEnumerable<CatalogModel>> GetCatlogByCategory(string catalog);
		Task<CatalogModel> GetCatalog(string id);
	}
}

