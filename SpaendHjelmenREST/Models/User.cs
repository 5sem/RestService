using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string AuthToken { get; set; }
        public virtual string UserName { get; set; }
        public virtual byte Image { get; set; }
        public virtual int ContactNumber { get; set; }
        public virtual string ContactMessage { get; set; }
        [DefaultValue(false)]
        public virtual bool Privacy { get; set; }

        public User()
        {

        }

        public User(int id)
        {
            this.Id = id;
        }
    }
}