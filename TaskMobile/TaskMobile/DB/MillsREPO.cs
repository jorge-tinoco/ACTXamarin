using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMobile.Models;

namespace TaskMobile.DB
{
    /// <summary>
    /// Mill repository. Used for supporting multi MILLS in Task Mobile application.
    /// </summary>
    /// <remarks>
    /// TODO: connecting database and implementing business logic for every mill.
    /// </remarks>
    public class MillsREPO
    {
        /// <summary>
        /// Get all the supported mills for task mobile.
        /// </summary>
        /// <returns>Collection of mills.</returns>
        public Task< IEnumerable<Mill> > SupportedMills()
        {
            Task<IEnumerable<Mill>> task = new Task<IEnumerable<Mill>>(obj =>
            {
                Mill mill = new Mill { Key = "PROT", Value = "Protectores" };
                Mill[] Mills = new Mill[] { mill };
                return Mills;
            }, 300);
            task.Start();
            return task;
        }
    }
}
