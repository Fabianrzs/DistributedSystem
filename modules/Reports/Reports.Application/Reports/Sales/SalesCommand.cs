using Microsoft.AspNetCore.Http;

namespace Reports.Application.Reports.Sales;

public sealed record SalesCommand() : ICommand<IFormFile>;
