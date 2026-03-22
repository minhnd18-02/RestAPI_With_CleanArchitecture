using AutoMapper;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.UserModel;
using RestAPIService.Domain.Entities;
using RestAPIService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceReponse<int>> DeleteUser(int id)
        {
            var response = new ServiceReponse<int>();
            try
            {
                var check = await _unitOfWork.UserRepo.GetByIdAsync(id);
                if (check == null)
                {
                    response.Success = false;
                    response.Message = "UserId not found";
                    return response;
                }

                await _unitOfWork.UserRepo.RemoveAsync(check);

                response.Success = true;
                response.Message = $"{id} delete successfully";

            }catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<IEnumerable<UserResponse>>> GetAllUser()
        {
            var response = new ServiceReponse<IEnumerable<UserResponse>>();
            try
            {
                var check = await _unitOfWork.UserRepo.GetAllAsync();
                if (check == null)
                {
                    response.Success = false;
                    response.Message = "Users are not found";
                }

                response.Success = true;
                response.Message = "Retrieve all user successfully";
                response.Data = _mapper.Map<IEnumerable<UserResponse>>(check);

            }catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<UserResponse>> GetUserById(int id)
        {
            var response = new ServiceReponse<UserResponse>();
            try
            {
                var check = await _unitOfWork.UserRepo.GetByIdAsync(id);
                if (check == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                response.Success = true;
                response.Message = "Retrieve user successfully";
                response.Data = _mapper.Map<UserResponse>(check);

            }catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<UserResponse>> UpdateUserById(int id, UpdateUserRequest updateUserRequest)
        {
            var response = new ServiceReponse<UserResponse>();
            try
            {
                var check = await _unitOfWork.UserRepo.GetByIdAsync(id);
                if (check == null)
                {
                    response.Success=false;
                    response.Message = "User not found";
                    return response;
                }

                var updateUser = _mapper.Map<User>(updateUserRequest);
                updateUser.Id = id;
                updateUser.RoleId = check.RoleId;
                await _unitOfWork.UserRepo.UpdateAsync(updateUser);

                response.Success = true;
                response.Message = "Update user successfully";
                response.Data = _mapper.Map<UserResponse>(updateUser);

            }catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
    }
}
