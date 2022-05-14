using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using instagram.Models;

namespace instagram.ViewModels
{
    public class PostReacts
    {
        public post Post { get; set; }
        public List<UserReact> UserReacts { get; set; }
        public List<UserComment> UsersComments { get; set; }

    }
}