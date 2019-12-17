using System.Threading.Tasks;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Models;

namespace prensaestudiantil.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<UserResponse> GetUSerResponseViewModelByEmail(string email);

        Task<Publication> ToPublicationAsync(PublicationViewModel model, bool isNew);

        PublicationViewModel ToPublicationViewModel(Publication model, bool isNew);
    }
}