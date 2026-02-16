using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Finances.Domain.Models.Dto.Shared;

public record AuditLogEntry()
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string Action { get; init; }
    public string Details { get; init; }
    public DateTime Timestamp { get; init; }
    public string? UserId { get; init; }
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
}