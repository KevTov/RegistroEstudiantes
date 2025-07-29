using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.Infrastructure.Configurations;

public class EstudianteConfiguration : IEntityTypeConfiguration<Estudiante>
{
    public void Configure(EntityTypeBuilder<Estudiante> builder)
    {
        builder.ToTable("Estudiantes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(e => e.Apellido)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)");

        builder.Property(e => e.Telefono)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("varchar(20)");

        builder.Property(e => e.FechaRegistro)
            .IsRequired()
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}