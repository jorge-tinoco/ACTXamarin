using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.ViewModels
{
    /// <summary>
    /// Common base class for using on all ViewModels.
    /// </summary>
    /// <example>
    ///     <code>
    ///     public YourClass: BaseViewModel
    ///     {
    ///         ...
    ///     }
    ///     </code>
    /// </example>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This event should be invoked to notify the view every time some property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Execute this method when some property in your viewmodel changes.
        /// </summary>
        /// <param name="propertyName">Property that changes.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
