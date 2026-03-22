using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.ViewModels.LoginModel
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }

        public string? Role { get; set; }
    }
}
