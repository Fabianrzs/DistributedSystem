using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Persistence.EFCore.Contexts;

public abstract class ApplicationsDbContext(DbContextOptions options) : DbContext(options) {  } 
