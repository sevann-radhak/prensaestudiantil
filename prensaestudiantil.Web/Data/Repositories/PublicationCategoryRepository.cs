using prensaestudiantil.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Data.Repositories
{
    public class PublicationCategoryRepository : GenericRepository<PublicationCategory>, IPublicationCategoryRepository
    {
        public PublicationCategoryRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
