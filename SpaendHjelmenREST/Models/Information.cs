using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Information
    {
        public virtual int Id { get; set; }
        public virtual string AppInfo { get; set; }

        public Information()
        {

        }

        public Information(int id)
        {
            this.Id = id;
        }
    }
}