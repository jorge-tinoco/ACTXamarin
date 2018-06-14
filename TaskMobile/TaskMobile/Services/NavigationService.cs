using System;
using System.Collections.ObjectModel;
using TaskMobile.Interfaces;
using Xamarin.Forms;

namespace TaskMobile.Services
{
    public class NavigationService : INavigationService
    {
        public async void NavigateBack()
        {
            Page CurrentPage = GetCurrentPage();
            await CurrentPage.Navigation.PopModalAsync();

        }

        public async void NavigateToPage(Page page)
        {
            Page CurrentPage = GetCurrentPage();
            await CurrentPage.Navigation.PushModalAsync(page);
        }

        private Page GetCurrentPage()
        {
            if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {

                int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;

                return Application.Current.MainPage.Navigation.NavigationStack[index];
            }
            else
                return Application.Current.MainPage;

        }
    }
}
