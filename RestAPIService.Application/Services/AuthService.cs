using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.LoginModel;
using RestAPIService.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace RestAPIService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSection:SecretKey"] ?? ""));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["JWTSection:Issuer"],
                audience: _configuration["JWTSection:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(30),
                signingCredentials: signInCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSection:SecretKey"] ?? "")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<ServiceReponse<LoginResponse>> Login(LoginTokenRequest loginRequest)
        {
            var response = new ServiceReponse<LoginResponse>();
            try
            {
                var check = await _unitOfWork.UserRepo.FindEntityAsync(s => s.Email.Equals(loginRequest.Email) && s.PasswordHash.Equals(loginRequest.Password));
                if (check == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, check.Id.ToString() ?? "NoId"),
                    new Claim(ClaimTypes.Name, check.Username),
                    new Claim(ClaimTypes.Role, check.RoleId.ToString())
                };

                var accessToken = GenerateAccessToken(claims);

                response.Success = true;
                response.Message = "Login Successfully";
                response.Data = new LoginResponse
                {
                    AccessToken = accessToken,
                    Role = check.RoleId.ToString()
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
    }
}
