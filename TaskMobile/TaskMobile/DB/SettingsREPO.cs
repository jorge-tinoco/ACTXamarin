using System;
using System.Threading.Tasks;
using TaskMobile.Models;

namespace TaskMobile.DB
{
    /// <summary>
    /// Used for manage variable settings stored in database.
    /// </summary>
    public class SettingsREPO: Database
    {
        public SettingsREPO()
        {
            connection.CreateTableAsync<Vehicle>().Wait(); // Only one item should be stored in DB. 
            connection.CreateTableAsync<Driver>().Wait();
        }

        /// <summary>
        /// Returns the current vehicle.
        /// </summary>
        /// <remarks>
        /// Only one vehicle should be stored in db.
        /// </remarks>
        /// <returns>Current <see cref="TaskMobile.Models.Vehicle"/> or null if not set.</returns>
        public async Task<Vehicle> CurrentVehicle()
        {
            int VehicleQuantity = await connection.Table<Vehicle>().CountAsync();
            if (VehicleQuantity > 0)
            {
                return await connection.Table<Vehicle>().FirstAsync();
            }else
                return null;
        }

        /// <summary>
        /// Set one vehicle as current. 
        /// </summary>
        /// <param name="currentVehicle">Vehicle to set as current.</param>
        /// <returns>True if all was ok.</returns>
        public async Task<bool> SetVehicle(Vehicle currentVehicle)
        {
            try
            {
                int VehicleQuantity = await connection.Table<Vehicle>().CountAsync();
                if (VehicleQuantity > 0)
                {
                    var OldVehicle = await connection.Table<Vehicle>().FirstAsync();
                    if (OldVehicle != null)
                        await connection.DeleteAsync(OldVehicle);
                }
                await connection.InsertAsync(currentVehicle);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
      
    }
}
