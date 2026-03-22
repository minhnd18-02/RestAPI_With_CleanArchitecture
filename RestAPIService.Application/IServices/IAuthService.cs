using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.LoginModel;
using RestAPIService.Application.ViewModels.RegisterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.IServices
{
    public interface IAuthService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<ServiceReponse<LoginResponse>> Login(LoginTokenRequest loginRequest);
        Task<ServiceReponse<RegisterReponse>> Register(RegisterRequest registerRequest);
    }
}
