using RestAPIService.Domain.Entities;
using RestAPIService.Domain.Interfaces;
using RestAPIService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Infrastructure.Repositories
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        public UserRepo(ApiContext context) : base(context)
        {
        }
    }
}
