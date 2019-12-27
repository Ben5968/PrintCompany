using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrintCompany.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
