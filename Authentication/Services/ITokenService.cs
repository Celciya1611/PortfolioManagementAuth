using Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Service
{
    public interface ITokenService
    {
        public string CreateToken(UserDTO user);

    }
}
