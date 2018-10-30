using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TrackId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }

    }
}