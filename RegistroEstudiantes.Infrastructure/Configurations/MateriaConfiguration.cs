using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.Infrastructure.Configurations;

public class MateriaConfiguration : IEntityTypeConfiguration<Materia>
{
    public void Configure(EntityTypeBuilder<Materia> builder)
    {
        builder.ToTable("Materias");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(m => m.Descripcion)
            .HasMaxLength(500)
            .HasColumnType("varchar(500)");

        builder.Property(m => m.Creditos)
            .IsRequired()
            .HasDefaultValue(3);

        builder.Property(m => m.ProfesorId)
            .IsRequired();
    }
}