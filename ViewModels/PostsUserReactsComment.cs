using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using instagram.Models;

namespace instagram.ViewModels
{
    public class PostsUserReactsComment
    {
        public List<post> Posts { get; set; }
        public PostComments Comments { get; set; }
        public postReact Reacts { get; set; }
    }
}