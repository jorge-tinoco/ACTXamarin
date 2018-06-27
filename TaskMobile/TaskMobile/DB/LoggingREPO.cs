using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.DB
{
    /// <summary>
    /// Used for store application log in database. 
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// This is an asyncrhonus repository. This means this methods are fire and forget. 
    /// Is not neccesary wait or set and await operator.
    /// TO DO: Connect with database..
    ///     - Set the business logic.
    ///     - Stablish debugging method.
    ///     - Inherit from <see cref="DB.Database"/>
    /// </remarks>
    public class LoggingREPO
    {
        public LoggingREPO()
        {
            // TO DO: initializes table  and  get the connection
        }

        public async void Debug(string text)
        {
            await Task.Delay(Random());
            // TO DO: Do some debugging stuff and store in db
        }

        public async void Debug(string text, params object[] parameters)
        {
            await Task.Delay(Random());
            // TO DO: Do some debugging stuff and store in db
        }

        public async void Info(string text)
        {
            await Task.Delay(Random());
            // TO DO: Do some info stuff and store in db
        }

        public async void Info(string text, params object[] parameters)
        {
            await Task.Delay(Random());
            // TO DO: Do some info stuff and store in db
        }

        public async void Error(string text)
        {
            await Task.Delay( Random() );
            // TO DO: Do some error stuff and store in db
        }

        public async void Error(Exception exception)
        {
            await Task.Delay(Random());
            // TO DO: Do some error stuff and store in db
        }
        public async void Error(string text, Exception exception)
        {
            await Task.Delay( Random() );
            // TO DO: Do some error stuff and store in db
        }

        public async void Error(string text, Exception exception, params object[] parameters)
        {
            await Task.Delay( Random() );
            // TO DO: Do some error stuff and store in db
        }

        private int Random()
        {
            Random random = new Random();
            Double ToConvert = random.NextDouble() * (2000 - 1000) + 1000;
            return (int)ToConvert;
        }
    }
}
