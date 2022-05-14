using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using instagram.Models;

namespace instagram.ViewModels
{
    public class UserReact
    {
        public postReact PostReact { get; set; }
        public user User { get; set; }
    }
}