using Microsoft.AspNetCore.Mvc.Rendering;
using prensaestudiantil.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace prensaestudiantil.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboPublicationCategories()
        {
            var list = _dataContext.PublicationCategories
                .Select(pt => new SelectListItem
                { Text = pt.Name, Value = $"{pt.Id}" })
                .OrderBy(pt => pt.Text)
                .ToList();

            return list;
        }
    }
}
