using System.Data.Entity;

namespace Kapok.Data.EntityFramework;

public class EntityFrameworkDataDomainScope : DataDomainScope
{
    internal readonly DbContext DbContext;

    public EntityFrameworkDataDomainScope(IDataDomain dataDomain, IServiceProvider serviceProvider) : base(dataDomain, serviceProvider)
    {
        if (!(dataDomain is EntityFrameworkDataDomain efDataDomain))
            throw new ArgumentException(
                $"DataDomain parameter must be assignable to type {typeof(EntityFrameworkDataDomain).FullName}",
                nameof(dataDomain));

        DbContext = efDataDomain.ConstructNewDbContext();
    }

    public override void RejectChanges()
    {
        throw new NotImplementedException();
    }

    public override bool CanSave()
    {
        return DbContext.ChangeTracker.HasChanges();
    }

    public override void Save()
    {
        DbContext.SaveChanges();
    }

    public override async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}