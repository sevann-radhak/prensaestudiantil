using prensaestudiantil.Web.Data.Entities;

namespace prensaestudiantil.Web.Data.Repositories
{
    public class YoutubeVideosRepository : GenericRepository<YoutubeVideo>, IYoutubeVideosRepository
    {
        public YoutubeVideosRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
}
