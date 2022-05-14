using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using instagram.Models;

namespace instagram.ViewModels
{
    public class UserFollow
    {
        public user User { get; set; }
        public IEnumerable<follower> Followers { get; set; }
    }
}