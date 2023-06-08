using MicroService.Shareds.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Shareds.ControllerCustomBases
{
    public class CustomControllerBase : ControllerBase
    {
        public IActionResult ControllerActionInstanceResult<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
