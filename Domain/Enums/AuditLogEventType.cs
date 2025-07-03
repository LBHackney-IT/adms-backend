namespace Domain.Enums;

public enum AuditLogEventType
{
    DataIngestion,
    ValidationError,
    ApprenticeAdded,
    ApprenticeUpdated,
    ApprenticeDeleted,
    TransactionAdded,
    TransactionUpdated,
    TransactionDeleted,
}