using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class User
    {
        public int Id { get; protected set; }
        public string AuthToken { get; set; }
        public string UserName { get; set; }
        public byte Image { get; set; }
        public int ContactNumber { get; set; }
        public string ContactMessage { get; set; }
        [DefaultValue(false)]
        public bool Privacy { get; set; }

        public User()
        {

        }

        public User(int id)
        {
            this.Id = id;
        }
    }
}