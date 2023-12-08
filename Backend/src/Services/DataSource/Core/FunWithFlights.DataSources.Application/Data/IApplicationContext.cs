using FunWithFlights.DataSources.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.DataSources.Application.Data
{
    public interface IApplicationContext
    {
        DbSet<DataSource> DataSources { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
