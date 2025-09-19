using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser
{
    public class LockUserResponseDto
    {
        public DateTime LockoutEnd { get; set; }
        public int DurationMinutes { get; set; }
        public string? Reason { get; set; }
    }

}
