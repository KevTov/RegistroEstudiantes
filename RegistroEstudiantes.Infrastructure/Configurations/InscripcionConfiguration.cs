using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.Infrastructure.Configurations;

public class InscripcionConfiguration : IEntityTypeConfiguration<Inscripcion>
{
    public void Configure(EntityTypeBuilder<Inscripcion> builder)
    {
        builder.ToTable("Inscripciones");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.Property(i => i.EstudianteId)
            .IsRequired();

        builder.Property(i => i.MateriaId)
            .IsRequired();

        builder.Property(i => i.FechaInscripcion)
            .IsRequired()
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(i => i.Activa)
            .IsRequired()
            .HasDefaultValue(true);
    }
}