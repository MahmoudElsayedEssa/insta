using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using instagram.Models;

namespace instagram.ViewModels
{
    public class PostComments
    {
        public post Post { get; set; }
        public List<UserComment> UsersComments { get; set; }
    }
}