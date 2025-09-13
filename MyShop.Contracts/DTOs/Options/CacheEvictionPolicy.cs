namespace MyShop.Contracts.DTOs.Options;

public enum CacheEvictionPolicy
{
    LRU,
    LFU,
    FIFO,
    Random,
    TTL
}