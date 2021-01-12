using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Nodes.DbModels;
using Bom.Core.Identity.DbModels;

namespace Bom.Core.Common
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        { }

        // Declare as non-nullable as DbSet properties should always be set
        // Use null-forgiving operator:  https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
        // also see:  https://stackoverflow.com/questions/59424673/best-practice-for-using-nullable-reference-types-for-dtos

        public DbSet<Node> Nodes { get; internal set; } = default!;

        public DbSet<DbPicture> DbPictures { get; internal set; } = default!;

        public DbSet<Path> Paths { get; internal set; } = default!;

        public DbSet<User> Users { get; internal set; } = default!;

        public DbSet<UserRole> UserRoles { get; internal set; } = default!;

        public DbSet<Role> Roles { get; internal set; } = default!;

        public async Task<Path?> FindPathAsync(params object[] keyValues)
        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            if (this.Paths == null)
            {
                return await Task.FromResult<Path?>(null);
            }
            return await this.Paths.FindAsync(keyValues);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if(modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            // to have more control and still not add still have a generic way, one could tr
            //   https://stackoverflow.com/questions/1003023/cast-to-generic-type-in-c-sharp
            //var typesToRegister = typeof(ModelMapping.NodeConfiguration).Assembly.GetTypes()
            //    .Where(type => type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Nodes.DbModels.Mapping.NodeConfiguration).Assembly);
        }

        protected internal void ExecuteRawSql(string sql, params object[] parameters)
        {
            // https://www.learnentityframeworkcore.com/raw-sql
            this.Database.ExecuteSqlRaw(sql, parameters); // previously but now deprecated:  this.Database.ExecuteSqlCommand(sql, parameters);
        }

        #region not used


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //  not needed in a webapplication -> maybe for a future development
        //    optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
        //}

        //protected internal System.Data.Common.DbCommand GetStoredProcedureCommand(string storedProcName)
        //{
        //    var cmd = this.Database.GetDbConnection().CreateCommand();
        //    cmd.CommandText = storedProcName;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    return cmd;
        //}

        #endregion

    }
}
