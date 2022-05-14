using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using instagram.Models;

namespace instagram.ViewModels
{
    public class UserComment
    {
        public user User { get; set; }
        public comment Comment { get; set; }
    }
}