using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public  class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto error { get;  set; }
        [JsonIgnore]
        public bool IsSuccessful { get;  set; }
        public static Response<T> Succsess(T data,int statusCode) 
        {
            return new Response<T> { Data = data, StatusCode = statusCode ,IsSuccessful=true};
        }
        public static Response<T> Succsess(int statusCode)
        {
            return new Response<T> { StatusCode = statusCode,IsSuccessful=true};
        }
        public static Response<T> Fail(ErrorDto errorDto,int statusCode)
        {
            return new Response<T> {error = errorDto, StatusCode = statusCode, IsSuccessful = false }; 
        }
        public static Response<T> Fail(string ErrorMessage, int statusCode, bool isShow)
        {
            var errorDto=new ErrorDto(ErrorMessage,isShow);
            return new Response<T> { error=errorDto, StatusCode = statusCode,IsSuccessful=false };
        }
    }
}
