using Elastiksearch.API.Models;

namespace Elastiksearch.API.DTOs
{
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto Feature)  //Record deferans tiplidir , değiştirilemez özelliklidir 
    {

        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature()
                {
                    Width = Feature.Width,
                    Height = Feature.Height,
                    Color = Feature.Color,
                }
            };
        }





    }
}
