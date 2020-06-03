using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#pragma warning disable CS8618 // Nullable doesn't apply for EFCore models
namespace BubenBot.Data.Models
{
    public class TagEntity
    {
        public long TagId { get; private set; }
        public ulong GuildId { get; set; }
        public ulong OwnerId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }

    public class TagConfiguration : IEntityTypeConfiguration<TagEntity>
    {
        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder
                .HasKey(t => t.TagId);

            builder
                .Property(t => t.TagId)
                .HasColumnName("Id")
                .UseIdentityAlwaysColumn();
            builder
                .Property(t => t.Name)
                .HasMaxLength(25)
                .IsRequired();
            builder
                .Property(t => t.Content)
                .IsRequired();
            builder
                .Property(t => t.Created)
                .IsRequired();
        }
    }
}
#pragma warning restore CS8618 // Nullable