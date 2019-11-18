using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;

        public ConverterHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
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
            PublicationViewModel publication = new PublicationViewModel
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

            return publication;
        }
    }
}
