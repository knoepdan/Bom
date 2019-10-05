using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Model;

namespace Bom.Core.Data
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        { }

        internal DbSet<Node>? Nodes { get; set; }

        internal DbSet<DbPicture>? DbPictures { get; set; }

        public IQueryable<DbPicture>? GetDbPictures()
        {
            return DbPictures;
        }

        public IQueryable<Node>? GetNodes()
        {

            return Nodes;
        }

        internal DbSet<Path>? Paths { get; set; }

        public IQueryable<Path>? GetPaths()
        {
            return Paths;
        }
        
        public async Task<Path?> FindPathAsync(params object[] keyValues)
        {
            if(this.Paths == null)
            {
                return await Task.FromResult<Path?>(null);
            }
            return await this.Paths.FindAsync(keyValues);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // to have more control and still not add still have a generic way, one could tr
            //   https://stackoverflow.com/questions/1003023/cast-to-generic-type-in-c-sharp
            //var typesToRegister = typeof(ModelMapping.NodeConfiguration).Assembly.GetTypes()
            //    .Where(type => type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelMapping.NodeConfiguration).Assembly);
        }


        protected internal System.Data.Common.DbCommand GetStoredProcedureCommand(string storedProcName)
        {
            var cmd = this.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
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

        #endregion

    }
}
