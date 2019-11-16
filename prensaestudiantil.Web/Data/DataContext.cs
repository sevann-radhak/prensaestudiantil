using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data.Entities;

namespace prensaestudiantil.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationCategory> PublicationCategories { get; set; }
        public DbSet<PublicationImage> PublicationImages { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<YoutubeVideo> YoutubeVideos { get; set; }
    }
}
