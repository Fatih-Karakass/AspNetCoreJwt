using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class ErrorDto
    {
        public List<string> Errors { get; private set; }
        public bool IsShow { get; private set; }
        public ErrorDto()
        {

        }
        public ErrorDto(string error,bool IsShow)
        {
            Errors.Add(error);
            this.IsShow= IsShow;
        }
        public ErrorDto(List<string> error,bool isShow)
        {
            Errors=error;
            this.IsShow= isShow;
        }
            
    }
}
