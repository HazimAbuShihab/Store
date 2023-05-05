
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Core7.Model
{
    [Table("Users")]
    public class UserModel : IdentityUser
    {
        [Column("Name")]
        public string? Name { get; set; }
        [Column("IsBlocked")]
        public bool IsBlocked { get; set; }
        [Column("Latitude")]
        public string? Latitude { get; set; }
        [Column("Longitude")]
        public string? Longitude { get; set; }
        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }
        public ICollection<OrderModel>? orders { get; set; }
    }
}
