using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prensaestudiantil.Prism.ViewModels
{
    public class ModifyUsesrPageViewModel : ViewModelBase
    {
        public ModifyUsesrPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Modify User";
        }
    }
}
