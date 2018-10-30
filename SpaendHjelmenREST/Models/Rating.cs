using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Rating
    {
        public int Id { get; protected set; }
        public int UserId { get; set; }
        public int TrackId { get; set; }
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