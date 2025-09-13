namespace MyShop.Contracts.DTOs.Options;

public enum ValidationOperationType
{
    Create,
    Update,
    Delete,
    BusinessOperation,
    BulkOperation,
    EventSourcingOperation
}