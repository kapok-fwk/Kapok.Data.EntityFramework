using System.Data.Entity;

namespace Kapok.Data.EntityFramework;

public sealed class EntityFrameworkDataDomain : DataDomain
{
    public EntityFrameworkDataDomain(Type dbContextType, string connectionString)
    {
        DbContextType = dbContextType;
        ConnectionString = connectionString;
    }

    public override IDataDomainScope CreateScope()
    {
        var scope = new EntityFrameworkDataDomainScope(this, ServiceProvider);
        return scope;
    }

    private Type DbContextType { get; }
    private string ConnectionString { get; }

    public DbContext ConstructNewDbContext()
    {
        var constructorInfo = DbContextType.GetConstructor(new[]
        {
            typeof(string) // connectionString
        });

        if (constructorInfo == null)
            throw new NotSupportedException("The DbContext must have a public constructor with the connection string as its only parameter.");

        var newDbContext = (DbContext)constructorInfo.Invoke(new object?[]
        {
            ConnectionString
        });

        return newDbContext;
    }
}
