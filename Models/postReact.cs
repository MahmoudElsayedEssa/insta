//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace instagram.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class postReact
    {
        public int id { get; set; }
        public Nullable<React> state { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> postId { get; set; }
    
        public virtual post post { get; set; }
        public virtual user user { get; set; }
    }
}
