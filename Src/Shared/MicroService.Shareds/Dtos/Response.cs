using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MicroService.Shareds.Dtos
{
    public class Response<T>
    {
        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccessFull { get; set; }
        public List<string> Errors { get; set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = statusCode,
                IsSuccessFull = true
            };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T>
            {
                Data = default(T),
                StatusCode = statusCode,
                IsSuccessFull = true
            };
        }
        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessFull = false
            };
        }
        public static Response<T> Fail(string errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = new List<string> { errors },
                StatusCode = statusCode,
                IsSuccessFull = false
            };
        }

    }
}
