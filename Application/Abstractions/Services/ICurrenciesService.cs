using Finances.Domain.Models.VM;
using Finances.Domain.Result;

namespace Finances.Application.Abstractions.Services;

/// <summary>
/// Сервисы для работы с курсами валют
/// </summary>
public interface ICurrenciesService
{
    Task<DataResult<bool>> AddToFavorites(Guid userId, Guid currencyId);
    Task<CollectionResult<CourseVM>> GetAll();
    Task<CollectionResult<CourseVM>> GetCoursesByUserId(Guid userId);
}