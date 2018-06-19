using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels
{
    /// <summary>
    /// View model representing <see cref="Views._Master"/> view.
    /// </summary>
    public class _MasterViewModel : BindableBase
    {
       
        INavigationService _navigationService;
        public DelegateCommand<string> NavigateCommand { get; set; }
        public _MasterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(NavigateAction);
        }

        /// <summary>
        /// Navigate action trigered when clicking one item in hamburguer menú.
        /// </summary>
        /// <param name="name">Page to navigate.</param>
        private async void NavigateAction(string name)
        {
            App.MasterD.IsPresented = false;
            await _navigationService.NavigateAsync( new Uri( name, UriKind.Relative));
        }
    }
    
}
