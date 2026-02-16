using Finances.Domain.Models.Dto.Shared;

namespace Finances.Application.Abstractions.Shared;

public interface IAuditLogService
{
    Task Log(AuditLogEntry entry, CancellationToken ct = default);
}
