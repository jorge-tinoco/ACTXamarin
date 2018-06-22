using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMobile.Models;

namespace TaskMobile.DB
{
    /// <summary>
    /// Languages repository. Used for supporting multilanguage in Task Mobile application.
    /// </summary>
    /// <remarks>
    /// TODO: connecting database and implementing multilanguage support. 
    /// </remarks>
    public class LanguagesREPO
    {
        /// <summary>
        /// Getting the application supported languages.
        /// </summary>
        /// <returns>Collection of languages.</returns>
        public Task<IEnumerable<Language> > SupportedLanguages()
        {
            Task<IEnumerable<Language>> task = new Task<IEnumerable<Language>>(obj =>
            {
                Language Lang = new Language { Key = "Spanish", ShortName = "Español MX", Value = "Español" };
                Language[] languages = new Language[] { Lang};
                return languages;
            }, 300);
            task.Start(); 
            return task;
        }
    }
}
