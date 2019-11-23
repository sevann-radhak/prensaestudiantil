using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Common.Models;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Models;

namespace prensaestudiantil.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public ConverterHelper(
            DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        public async Task<UserResponse> GetUSerResponseViewModelByEmail(string email)
        {
            User user = await _dataContext.Users
                .Include(u => u.YoutubeVideos)
                .Include(u => u.Publications)
                .ThenInclude(p => p.PublicationCategory)
                .Include(u => u.Publications)
                .ThenInclude(p => p.PublicationImages)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                ImageUrl = user.ImageFullPath,
                LastName = user.LastName,
                IsManager = await _userHelper.IsInRoleAsync(user.Email, "Manager"),
                Publications = user.Publications?.Select(p => new PublicationResponse
                {
                    Author = p.Author,
                    Body = p.Body,
                    Date = p.Date,
                    Footer = p.Footer,
                    Header = p.Header,
                    Id = p.Id,
                    ImageDescription = p.ImageDescription,
                    ImageUrl = p.ImageFullPath,
                    LastUpdate = p.LastUpdate,
                    PublicationCategory = p.PublicationCategory.Name,
                    PublicationImages = p.PublicationImages?.Select(pi => new PublicationImageResponse
                    {
                        Description = pi.Description,
                        Id = pi.Id,
                        ImageUrl = pi.ImageFullPath
                    }).ToList(),
                    Title = p.Title,
                    User = p.User.FullName
                }).OrderByDescending(p => p.Date)
                 .ToList(),
                PhoneNumber = user.PhoneNumber,
                Roles = await _userHelper.GetRolesAsync(user.Email),
                YoutubeVideos = user.YoutubeVideos?.Select(y => new YoutubeVideoResponse
                {
                    Name = y.Name,
                    URL = y.URL
                }).ToList()
            };
        }

        public async Task<Publication> ToPublicationAsync(PublicationViewModel model, bool isNew)
        {
            Publication publication = new Publication
            {
                Author = model.Author,
                Body = model.Body,
                Date = model.Date,
                Footer = model.Footer,
                Header = model.Header,
                Id = isNew ? 0 : model.Id,
                ImageDescription = model.ImageDescription,
                ImageUrl = model.ImageUrl,
                LastUpdate = model.LastUpdate,
                PublicationCategory = await _dataContext.PublicationCategories.FindAsync(model.PublicationCategoryId),
                PublicationImages = isNew ? new List<PublicationImage>() : model.PublicationImages,
                Title = model.Title,
                User = await _dataContext.Users.FindAsync(model.UserId)
            };

            return publication;
        }

        public PublicationViewModel ToPublicationViewModel(Publication model, bool isNew)
        {
            PublicationViewModel publicationViewModel = new PublicationViewModel
            {
                Author = model.Author,
                Body = model.Body,
                Date = model.Date,
                Footer = model.Footer,
                Header = model.Header,
                Id = isNew ? 0 : model.Id,
                ImageDescription = model.ImageDescription,
                ImageUrl = model.ImageUrl,
                LastUpdate = model.LastUpdate,
                PublicationCategoryId = model.PublicationCategory.Id,
                PublicationImages = isNew ? new List<PublicationImage>() : model.PublicationImages,
                Title = model.Title,
                UserId = model.User.Id,
                User = model.User
            };

            return publicationViewModel;
        }
    }
}
