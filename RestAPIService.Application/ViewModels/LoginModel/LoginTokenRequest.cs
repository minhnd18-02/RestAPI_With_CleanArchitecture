using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.ViewModels.LoginModel
{
    public class LoginTokenRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
