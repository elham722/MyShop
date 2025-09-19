using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser
{
    public class UnlockUserResponseDto
    {
        public string? Reason { get; set; }
        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
    }
}
