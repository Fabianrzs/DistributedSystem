using Domain.Abstractions.Entities;

namespace Reports.Domain.Modules.Entities;
public class CustomerInfo : Entity
{
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
}
