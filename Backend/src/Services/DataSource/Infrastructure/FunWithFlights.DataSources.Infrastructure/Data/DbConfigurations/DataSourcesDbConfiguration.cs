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
                Name = "FlightRouter",
                Url = "https://localhost:7001/flight-routes",
                Description = "Mock Flight Routes Provider"
            }
            //new 
            //{
            //    Id = 1,
            //    Name = "Provider 1",
            //    Url = "https://zretmlbsszmm4i35zrihcflchm0ktwwj.lambda-url.eu-central-1.on.aws/provider/flights1",
            //    Description = "Sample Data Source provider"
            //},
            //new 
            //{
            //    Id =2,
            //    Name = "Provider 2",
            //    Url = "https://zretmlbsszmm4i35zrihcflchm0ktwwj.lambda-url.eu-central-1.on.aws/provider/flights2",
            //    Description = "Sample Data Source provider"
            //}
        ]);
    }
}
