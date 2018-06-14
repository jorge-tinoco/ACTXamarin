using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaskMobile.Interfaces
{
    public interface INavigationService
    {
        void NavigateToPage(Page page);
        void NavigateBack();
    }
}
