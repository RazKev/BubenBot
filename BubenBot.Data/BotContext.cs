using System.Reflection;
using BubenBot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BubenBot.Data
{
    public class BotContext : DbContext
    {
        public DbSet<TagEntity> Tags { get; set; }

        public BotContext(DbContextOptions<BotContext> options) : base(options) {}

        public BotContext() {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}