using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FunWithFlights.Aggregator.Application.Data.Extensions
{
    internal static class DbSetExtensions
    {
        public static Task ExecuteInsertAsync<T>(
            this DbSet<T> dbSet,
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(dbSet);
            ArgumentNullException.ThrowIfNull(entities);

            if (!entities.Any())
            {
                return Task.CompletedTask;
            }

            var context = dbSet.GetService<ICurrentDbContext>().Context;
            return context.BulkCopyAsync(entities, cancellationToken);
        }
    }
}
