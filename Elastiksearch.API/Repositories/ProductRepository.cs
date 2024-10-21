using Elastiksearch.API.DTOs;
using Elastiksearch.API.Models;
using Nest;
using System.Collections.Immutable;

namespace Elastiksearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        private const string indexName = "products";

        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct) //Null değer dönebilir
        {
            newProduct.Created = DateTime.Now;

            var response = await _client.IndexAsync(newProduct, x => x.Index(indexName));

            if (!response.IsValid)
            {
                return null;
            }

            newProduct.ID = response.Id;
            return newProduct;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.ID = hit.Id;


            return result.Documents.ToImmutableList();//Kimse bu listede değişiklik yapmasın dedik

        }

        public async Task<Product?> GetByIdAsync(string Id)
        {
            var response = await _client.GetAsync<Product>(Id, x => x.Index(indexName));

            if (!response.IsValid)
            {
                return null;
            }
            response.Source.ID = response.Id; //mapleme işlemi yaptık
            return response.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateDto)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(updateDto.Id, x => x.Index(indexName).Doc(updateDto));
            return response.IsValid;
        }


        //Hata yönetimi için 
        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id,x=>x.Index(indexName));
            return response;
        }
    }
}
//ImmutableList = Kimse değişiklik yapamaz 