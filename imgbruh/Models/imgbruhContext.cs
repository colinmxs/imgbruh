using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace imgbruh.Models
{
    public class ImgbruhContext : DbContext
    {
        public DbSet<Img> Imgs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Img>()
                .HasKey(img => img.Id);

            modelBuilder.Entity<Img>()
                .Property(i => i.CodeName)
                .IsRequired()
                .HasMaxLength(75)
                .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute("IX_Codename") { IsUnique = true }));
        }
    }
}