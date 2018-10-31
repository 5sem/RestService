using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Rating
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int TrackId { get; set; }
        [Range(1,5)]
        public int UserRating { get; set; }

        public Rating()
        {

        }

        public Rating(int id)
        {
            this.Id = id;
        }
    }
}