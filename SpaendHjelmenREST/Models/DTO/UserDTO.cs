using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models.DTO
{
    public class UserDTO
    {

        public virtual int Id { get; set; }
        public virtual string AuthToken { get; set; }

    }
}