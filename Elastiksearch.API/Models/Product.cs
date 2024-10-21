using Elastiksearch.API.DTOs;
using Nest;

namespace Elastiksearch.API.Models
{
    public class Product
    {

        [PropertyName("_id")] //Elasticsearch e kardeşim bak bu id id haberin olsun dedik yoksa bunun id olduğunu görmezdi

        public string ID { get; set; } = null!;  //Elasticsearch default da id değerini string türde tutar ve bunun asla null olmayacağını dedik
        public string Name { get; set; } = null!; //Name değeri asla null olamaz
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public ProductFeature? Feature { get; set; } //Product ile productfeature arasında ilişki kurduk


        public ProductDto CreateDto()
        {
            if (Feature == null)
                return new ProductDto(ID, Name, Price, Stock, null);
            return new ProductDto(ID, Name, Price, Stock, new ProductFeatureDto(Feature.Width, Feature.Height, Feature.Color));
        }

    }
}
