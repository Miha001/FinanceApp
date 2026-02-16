using Finances.Application.Abstractions.Shared;
using Finances.Domain.Models.Dto.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Finances.Application.Behaviors;

public class AuditLogBehavior<TRequest, TResponse>(
    IAuditLogService auditService,
    IHttpContextAccessor httpContextAccessor,
    TimeProvider timeProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var actionName = typeof(TRequest).Name;

        // Игнорируем Query 
        if (actionName.StartsWith("Get") || actionName.EndsWith("Query"))
        {
            return await next();
        }

        var userId = httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                     ?? httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var details = JsonSerializer.Serialize(request);

        TResponse response;
        try
        {
            response = await next();

            await auditService.Log(new AuditLogEntry()
            {
                Timestamp = timeProvider.GetUtcNow().UtcDateTime,
                Action = actionName,
                UserId = userId,
                Details = details,
                IsSuccess = true
            }, ct);

            return response;
        }
        catch (Exception ex)
        {
            await auditService.Log(new AuditLogEntry()
            {
                Timestamp = timeProvider.GetUtcNow().UtcDateTime,
                Action = actionName,
                UserId = userId,
                Details = details,
                IsSuccess = false,
                ErrorMessage = ex.Message
            }, ct);

            throw;
        }
    }
}