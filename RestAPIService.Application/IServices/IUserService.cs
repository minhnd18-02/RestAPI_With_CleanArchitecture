using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.IServices
{
    public interface IUserService
    {
        Task<ServiceReponse<IEnumerable<UserResponse>>> GetAllUser();
        Task<ServiceReponse<UserResponse>> GetUserById(int id);
        Task<ServiceReponse<UserResponse>> UpdateUserById(int id, UpdateUserRequest updateUserRequest);
        Task<ServiceReponse<int>> DeleteUser(int id); 
    }
}
