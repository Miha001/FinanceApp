using Finances.Domain.Models.Dto;
using Finances.Domain.Result;

namespace Finances.Application.Abstractions.Currencies;

/// <summary>
/// Сервисы для работы с курсами валют
/// </summary>
public interface ICurrenciesService
{
    Task<DataResult<bool>> AddToFavorites(Guid userId, Guid currencyId);
    Task<CollectionResult<CourseDto>> GetAll();
    Task<CollectionResult<CourseDto>> GetCoursesByUserId(Guid userId);
}