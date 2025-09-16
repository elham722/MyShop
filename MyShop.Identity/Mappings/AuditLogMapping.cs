using Mapster;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Identity.Models;

namespace MyShop.Identity.Mappings
{
    public class AuditLogMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AuditLog, AuditLogDto>()
                // فقط برای فیلدهای خاص override می‌نویسیم
                .Map(dest => dest.UserName, src => src.User != null ? src.User.UserName : null)
                .Map(dest => dest.UserEmail, src => src.User != null ? src.User.Email : null);
        }
    }
}