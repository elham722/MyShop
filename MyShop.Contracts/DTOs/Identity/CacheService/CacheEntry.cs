using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.CacheService
{
    /// <summary>
    /// Cache entry model
    /// </summary>
    public class CacheEntry
    {
        public string Key { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public long Size { get; set; }
        public int AccessCount { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
