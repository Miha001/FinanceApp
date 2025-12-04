namespace Application.Abstractions.Entities;
public interface IEntity//<TKey>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}