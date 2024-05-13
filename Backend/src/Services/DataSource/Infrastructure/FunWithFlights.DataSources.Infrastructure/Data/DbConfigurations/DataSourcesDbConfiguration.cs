using FunWithFlights.DataSources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunWithFlights.DataSources.Infrastructure.Data.DbConfigurations;

internal class DataSourcesDbConfiguration : IEntityTypeConfiguration<DataSource>
{
    public void Configure(EntityTypeBuilder<DataSource> builder)
    {
        builder.HasKey(dataSource => dataSource.Id);

        builder.HasData([
            new 
            {
                Id = 1,
                Name = "FlightRouter v1",
                Url = "http://localhost:5156/flight-routes",
                Description = "Mock Flight Routes Provider"
            }
        ]);
    }
}
