using Elastiksearch.API.Models;
using Nest;

namespace Elastiksearch.API.DTOs
{
    public record ProductDto(string ID, string Name,decimal Price,int Stock, ProductFeatureDto? Feature)
    {


       
    }
}
