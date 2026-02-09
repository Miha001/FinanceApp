namespace Finances.Domain.Result;

/// <summary>
/// Класс для реализации паттерна Result Pattern.
/// Абстракция, которая представляет собой коллекции результов выполнения операции
/// </summary>
/// <typeparam name="T">Тип элемента коллекции</typeparam>
public class CollectionResult<T> : DataResult<IEnumerable<T>>
{
    private CollectionResult(IEnumerable<T> data, int totalCount, Error? error = null)
        : base(data, error)
    {
        TotalCount = totalCount;
    }

    public int TotalCount { get; }

    public static new CollectionResult<T> Success(IEnumerable<T> data, int totalCount) =>
        new(data, totalCount);

    public static new CollectionResult<T> Failure(int errorCode, string errorMessage) =>
        new([], default, new Error(errorMessage, errorCode));
}