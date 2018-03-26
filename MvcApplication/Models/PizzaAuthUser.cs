using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaAuth.Models
{
    public class PizzaAuthUser
    {
        public string Name { get; set; }
        public bool? EmailVerified { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}