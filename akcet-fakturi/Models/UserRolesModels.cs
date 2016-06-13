using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace akcet_fakturi.Models
{
    public class UserRolesModels
    {
        [Required]
        public string SelectedUsers { get; set; }


        public string SelectedRoles { get; set; }

    }

    public class RolesInfo
    {
        public string RoleName { get; set; }

        public int RoleId { get; set; }

        public int CountUsersInRole { get; set; }

   
    }

    public class ListOfRoles
    {
        public List<RolesInfo> ListRoles = new List<RolesInfo>();
    }
}