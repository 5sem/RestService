using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Picture
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual byte Image { get; set; }

        public Picture()
        {

        }

        public Picture(int id)
        {
            this.Id = id;
        }

    }
}