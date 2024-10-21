using Elastiksearch.API.DTOs;
using Elastiksearch.API.Models;
using Elastiksearch.API.Repositories;
using System.Collections.Immutable;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Elastiksearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request) //Dtoyu oluştırup dönüştürme işlemi yaptık
        {


            var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());

            if (responseProduct == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasıda bir hata meydana geldi" }, System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), System.Net.HttpStatusCode.Created); //Dto döndül modeli kullanıcıya göstermedik


        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productListDto = new List<ProductDto>();

            //var productListDto = products.Select(x => new ProductDto(x.ID, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature!.Height, x.Feature!.Color))).ToList();


            foreach (var x in products)
            {
                if (x.Feature is null) //feature null ise bunları ekle
                {
                    productListDto.Add(new ProductDto(x.ID, x.Name, x.Price, x.Stock, null));
                }
                else
                {
                    productListDto.Add(new ProductDto(x.ID, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature!.Height, x.Feature!.Color)));
                }


            }

            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
        }


        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var hasProduct = await _productRepository.GetByIdAsync(id);

            if (hasProduct == null)
            {
                return ResponseDto<ProductDto>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
            }
            var productDto = hasProduct.CreateDto();
            return ResponseDto<ProductDto>.Success(productDto, HttpStatusCode.OK);



        }
        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateDto)
        {
            var isSuccess = await _productRepository.UpdateAsync(updateDto);

            if (!isSuccess)
            {
                return ResponseDto<bool>.Fail(new List<string> { "Güncelleme işleminde hata meydana geldi" }, HttpStatusCode.InternalServerError);
            }
            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);

        }

        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var deleteResponse = await _productRepository.DeleteAsync(id);
            if (!deleteResponse.IsValid && deleteResponse.Result == Nest.Result.NotFound)
            {
                return ResponseDto<bool>.Fail(new List<string> { "Silmeye çalıştığınız ürün bulunamamıştır" }, HttpStatusCode.NotFound);
            }

            if (!deleteResponse.IsValid)
            {
                //loglama yapacağız 
                _logger.LogError(deleteResponse.OriginalException, deleteResponse.ServerError.Error.ToString());
                //bir hata meydana geldiğinde burada göreceğiz 

                return ResponseDto<bool>.Fail(new List<string> { "Silme işleminde hata meydana geldi" }, HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}
