namespace Domain.Models.VM;
public class CourseVM
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название валюты
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Курс валюты к рублю
    /// </summary>
    public float Rate { get; set; }
}
