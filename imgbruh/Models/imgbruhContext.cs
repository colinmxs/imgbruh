using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace imgbruh.Models
{
    public class imgbruhContext : ApplicationDbContext
    {
        public DbSet<Img> Imgs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Img>()
                .HasKey(img => img.Id);
            modelBuilder.Entity<Comment>()
                .HasKey(comment => comment.Id);
            modelBuilder.Entity<Rating>()
                .HasKey(rating => rating.Id);

            modelBuilder.Entity<Img>()
                .HasMany(img => img.Ratings);

            modelBuilder.Entity<Img>()
                .HasMany(img => img.Comments);

            modelBuilder.Entity<Rating>()
                .HasRequired(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Img>()
                .HasRequired(i => i.User)
                .WithMany(u => u.Imgs)
                .HasForeignKey(i => i.UserId);                

            modelBuilder.Entity<Img>()
                .Property(i => i.CodeName)
                .IsRequired()
                .HasMaxLength(75)
                .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute("IX_Codename") { IsUnique = true }));

            modelBuilder.Entity<ApplicationUser>()
                .Property(au => au.UserName)
                .IsRequired()
                .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute("IX_Username") { IsUnique = true }));
        }
    }
}