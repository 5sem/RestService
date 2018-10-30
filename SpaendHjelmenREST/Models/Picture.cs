using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Picture
    {
        public int Id { get; protected set; }
        public string Name { get; set; }
        public byte Image { get; set; }

        public Picture()
        {

        }

        public Picture(int id)
        {
            this.Id = id;
        }

    }
}