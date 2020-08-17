using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InstitutionService.Models.DBModels
{
    public partial class institutionserviceContext : DbContext
    {
        public institutionserviceContext()
        {
        }

        public institutionserviceContext(DbContextOptions<institutionserviceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Institutions> Institutions { get; set; }
        public virtual DbSet<Invitations> Invitations { get; set; }
        public virtual DbSet<Officers> Officers { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<Servicesinstitutions> Servicesinstitutions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=nirmal;password=NirmalTheOne@123;database=institutionservice", x => x.ServerVersion("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Institutions>(entity =>
            {
                entity.HasKey(e => e.InstitutionId)
                    .HasName("PRIMARY");

                entity.ToTable("institutions");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.CountryIso)
                    .HasColumnName("countryIso")
                    .HasColumnType("char(2)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
            });

            modelBuilder.Entity<Invitations>(entity =>
            {
                entity.HasKey(e => e.InvitationId)
                    .HasName("PRIMARY");

                entity.ToTable("invitations");

                entity.HasIndex(e => e.OfficerId)
                    .HasName("officerId");

                entity.Property(e => e.InvitationId).HasColumnName("invitationId");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasMaxLength(255);

                entity.Property(e => e.Link)
                    .HasColumnName("link")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OfficerId).HasColumnName("officerId");

                entity.Property(e => e.RecipientName)
                    .HasColumnName("recipientName")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Officer)
                    .WithMany(p => p.Invitations)
                    .HasForeignKey(d => d.OfficerId)
                    .HasConstraintName("invitations_ibfk_1");
            });

            modelBuilder.Entity<Officers>(entity =>
            {
                entity.HasKey(e => e.OfficerId)
                    .HasName("PRIMARY");

                entity.ToTable("officers");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("institutionId");

                entity.Property(e => e.OfficerId).HasColumnName("officerId");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Officers)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("officers_ibfk_1");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("services");

                entity.Property(e => e.ServiceId).HasColumnName("serviceId");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Servicesinstitutions>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.InstitutionId, e.ServiceId })
                    .HasName("PRIMARY");

                entity.ToTable("servicesinstitutions");

                entity.HasIndex(e => e.Id)
                    .HasName("id");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("servicesinstitutions_ibfk_1");

                entity.HasIndex(e => e.ServiceId)
                    .HasName("servicesinstitutions_ibfk_2");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.ServiceId).HasColumnName("serviceId");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Servicesinstitutions)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("servicesinstitutions_ibfk_1");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Servicesinstitutions)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("servicesinstitutions_ibfk_2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
