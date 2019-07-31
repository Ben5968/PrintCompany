using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrintCompany.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        [Required(ErrorMessage = "Role Name is Required")]
        public string Id { get; set; }
        public string RoleName { get; set; }

        public List<string> Users { get; set; }

    }
}
