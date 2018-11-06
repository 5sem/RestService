using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Comment
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int TrackId { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Edited { get; set; }
        public virtual string UserComment { get; set; }


        public Comment()
        {

        }

        public Comment(int id)
        {
            this.Id = id;
        }
    }
}