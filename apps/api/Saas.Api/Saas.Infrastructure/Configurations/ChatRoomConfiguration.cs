using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saas.Domain;
using Saas.Infrastructure.Configurations.Extensions;

namespace Saas.Infrastructure.Configurations;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder
            .HasMany(c => c.Participants)
            .WithMany();

        builder
            .HasMany(c => c.Messages)
            .WithOne();

        builder.HasTitle(x => x.Name);
    }
}