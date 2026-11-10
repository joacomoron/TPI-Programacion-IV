using ClinicaTurnos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Paciente 1 - N Turno
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Paciente)
                .WithMany(p => p.Turnos)
                .HasForeignKey(t => t.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Medico 1 - N Turno
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Medico)
                .WithMany(m => m.Turnos)
                .HasForeignKey(t => t.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Consultorio 1 - N Turno
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Consultorio)
                .WithMany(c => c.Turnos)
                .HasForeignKey(t => t.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Turno 1 - 1 Pago (la FK vive en Pago)
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Pago)
                .WithOne(p => p.Turno)
                .HasForeignKey<Pago>(p => p.TurnoId)
                .OnDelete(DeleteBehavior.Cascade);

            // El estado se guarda como texto ("Pendiente", "Confirmado"...) y no como número
            modelBuilder.Entity<Turno>()
                .Property(t => t.Estado)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
