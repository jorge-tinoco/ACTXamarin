using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels
{
    /// <summary>
    ///  View model representing <see cref="MainPage"/> view.
    /// </summary>
    public class MainPageViewModel : BindableBase
    {
        private INavigationService _navigationService;

        public  MainPageViewModel(INavigationService navigationService)
        {
                _navigationService = navigationService;
        }
    }
}
