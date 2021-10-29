using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class UserDTO
    {
        //internal string JWToken;

        public string PortfolioID { get; set; }
        public string Password { get; set; }
        public string JWTToken { get; set; }
    }
}
