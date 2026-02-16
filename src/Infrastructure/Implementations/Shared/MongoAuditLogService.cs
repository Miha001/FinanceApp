using Finances.Application.Abstractions.Shared;
using Finances.Domain.Models.Dto.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Finances.DAL.Implementations.Shared;
public class MongoAuditLogService : IAuditLogService
{
    private readonly IMongoCollection<AuditLogEntry> _collection;
    private readonly ILogger<MongoAuditLogService> _logger;

    public MongoAuditLogService(
        IConfiguration configuration,
        IMongoDatabase mongoDatabase,
        ILogger<MongoAuditLogService> logger)
    {
        _collection = mongoDatabase.GetCollection<AuditLogEntry>("AuditLogs");
        _logger = logger;
    }

    public async Task Log(AuditLogEntry entry, CancellationToken ct = default)
    {
        try
        {
            await _collection.InsertOneAsync(entry, cancellationToken: ct);
        }
        catch(Exception ex)
        {
            _logger.LogCritical(ex,
                "AUDIT LOG FALLBACK: ошибка записи в mongo audit: {@AuditEntry}",
                entry);
        }
    }
}