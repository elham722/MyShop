using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyShop.Identity.Models;

namespace MyShop.Identity.Context
{
    public class MyShopIdentityDbContext:IdentityDbContext<ApplicationUser>
    {
    }
}
