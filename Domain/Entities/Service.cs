using Domain.Common;

namespace Domain.Entities;

public class Service : HistoricEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }

    public Store Store { get; private set; } = null!;
    private readonly List<ProfessionalService> _providers = new();
    public IReadOnlyCollection<ProfessionalService> Providers => _providers.AsReadOnly();

    private Service() { }
}
