using Elastiksearch.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Elastiksearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction] //Bu sadece metot dış dünya ile ilişiği yok GEt post filan algılama

        public IActionResult CreateActionresult<T>(ResponseDto<T> response)
        {
            if (response.Status == HttpStatusCode.NoContent) //Başarısız ollması drumnda 
                return new ObjectResult(null)
                {
                    StatusCode = response.Status.GetHashCode(),
                };
            return new ObjectResult(response) //Başarlı olması durumunda
            {
                StatusCode = response.Status.GetHashCode(),
            };
        }
    }
}
//ObjectResult Anadır 