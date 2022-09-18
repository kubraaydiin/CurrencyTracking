using Alachisoft.NCache.Client;
using Alachisoft.NCache.EntityFrameworkCore;
using CurrencyTracking.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace CurrencyTracking.Repository
{
    public class PostgreSqlContext : DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {
        }

        public DbSet<Currency> currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // configure cache with SQLServer DependencyType and CacheInitParams
            CacheConnectionOptions initParams = new CacheConnectionOptions();
            initParams.RetryInterval = new TimeSpan(0, 0, 5);
            initParams.ConnectionRetries = 2;
            initParams.ConnectionTimeout = new TimeSpan(0, 0, 5);
            initParams.AppName = "appName";
            initParams.CommandRetries = 2;
            initParams.CommandRetryInterval = new TimeSpan(0, 0, 5);
            initParams.Mode = IsolationLevel.Default;

            NCacheConfiguration.Configure("democache", DependencyType.Other, initParams);

        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
