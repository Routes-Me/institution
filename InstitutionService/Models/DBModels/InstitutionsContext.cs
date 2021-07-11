using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InstitutionService.Models.DBModels
{
    public partial class InstitutionsContext : DbContext
    {
        public InstitutionsContext()
        {
        }

        public InstitutionsContext(DbContextOptions<InstitutionsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Authorities> Authorities { get; set; }
        public virtual DbSet<Institutions> Institutions { get; set; }
        public virtual DbSet<Invitations> Invitations { get; set; }
        public virtual DbSet<Officers> Officers { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<ServicesInstitutions> ServicesInstitutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authorities>(entity =>
            {
                entity.HasKey(e => e.AuthorityId)
                    .HasName("PRIMARY");

                entity.ToTable("authorities");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("institution_id");

                entity.Property(e => e.AuthorityId).HasColumnName("authority_id");

                entity.Property(e => e.InstitutionId).HasColumnName("institution_id");

                entity.Property(e => e.Pin)
                    .HasColumnName("pin")
                    .HasColumnType("char(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Authorities)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("authorities_ibfk_1");
            });

            modelBuilder.Entity<Institutions>(entity =>
            {
                entity.HasKey(e => e.InstitutionId)
                    .HasName("PRIMARY");

                entity.ToTable("institutions");

                entity.Property(e => e.InstitutionId).HasColumnName("institution_id");

                entity.Property(e => e.CountryIso)
                    .HasColumnName("country_iso")
                    .HasColumnType("char(2)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Invitations>(entity =>
            {
                entity.HasKey(e => e.InvitationId)
                    .HasName("PRIMARY");

                entity.ToTable("invitations");

                entity.HasIndex(e => e.OfficerId)
                    .HasName("officer_id");

                entity.Property(e => e.InvitationId).HasColumnName("invitation_id");

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

                entity.Property(e => e.OfficerId).HasColumnName("officer_id");

                entity.Property(e => e.RecipientName)
                    .HasColumnName("recipient_name")
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
                    .HasName("institution_id");

                entity.Property(e => e.OfficerId).HasColumnName("officer_id");

                entity.Property(e => e.InstitutionId).HasColumnName("institution_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

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

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.Property(e => e.Descriptions)
                    .HasColumnName("descriptions")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<ServicesInstitutions>(entity =>
            {
                entity.HasKey(e => new { e.InstitutionId, e.ServiceId })
                    .HasName("PRIMARY");

                entity.ToTable("services_institutions");

                entity.HasIndex(e => e.ServiceId)
                    .HasName("service_id");

                entity.Property(e => e.InstitutionId).HasColumnName("institution_id");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.ServicesInstitutions)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("services_institutions_ibfk_1");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServicesInstitutions)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("services_institutions_ibfk_2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
