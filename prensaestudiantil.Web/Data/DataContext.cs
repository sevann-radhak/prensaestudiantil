using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data.Entities;

namespace prensaestudiantil.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // TODO: delete comments and files 
        //public DbSet<Manager> Managers { get; set; }

        public DbSet<PublicationCategory> PublicationCategories { get; set; }

        public DbSet<PublicationImage> PublicationImages { get; set; }

        public DbSet<Publication> Publications { get; set; }

        //public DbSet<Writer> Writers { get; set; }

        public DbSet<YoutubeVideo> YoutubeVideos { get; set; }
    }
}
