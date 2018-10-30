using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class User
    {
        public int Id { get; set; }
        public string AuthToken { get; set; }
        public string UserName { get; set; }
        public byte Image { get; set; }
        public int ContactNumber { get; set; }
        public string ContactMessage { get; set; }
        public bool Privacy { get; set; }

    }
}