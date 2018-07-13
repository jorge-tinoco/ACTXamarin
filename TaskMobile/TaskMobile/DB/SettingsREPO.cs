using System;
using System.Collections.Generic;
using Thread = System.Threading.Tasks;
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
            connection.CreateTableAsync<Rejection>().Wait();
        }

        /// <summary>
        /// Returns the current vehicle.
        /// </summary>
        /// <remarks>
        /// Only one vehicle should be stored in db.
        /// </remarks>
        /// <returns>Current <see cref="TaskMobile.Models.Vehicle"/> or null if not set.</returns>
        public async Thread.Task <Vehicle> CurrentVehicle()
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
        public async Thread.Task <bool> SetVehicle (Vehicle currentVehicle)
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
                App.LogToDb.Error(e);
                return false;
            }
        }

        /// <summary>
        /// Get all available reasons for rejecting an activity.
        /// </summary>
        /// <returns>Set of <see cref="RejectReasons"/>.</returns>
        public async Thread.Task <IEnumerable<Rejection >> Rejections()
        {
            try
            {
                return await connection.Table<Rejection>().ToListAsync();
            }
            catch (Exception e)
            {
                App.LogToDb.Error(e);
                return new List<Rejection>();
            }
        }

        /// <summary>
        /// Add new rejection  reason to DB.
        /// </summary>
        /// <param name="reason">Rejection  reason.</param>
        /// <param name="number">Custom number to add to the rejection.</param>
        public async Thread.Task AddRejection(string reason, int number = 0)
        {
            try
            {
                int ReasonsFound = await connection.Table<Rejection>().
                                            Where(x => x.Reason == reason).
                                            CountAsync();
                if (ReasonsFound > 0)
                    throw new Exception(string.Format( "Ya existe la razón de rechazo '{0}' en la BD, pruebe con otra.", reason) );
                else
                {
                    await connection.InsertAsync( new Rejection(reason, number) ); 
                }
            }
            catch (Exception e)
            {
                App.LogToDb.Error(e);
                throw  e;
            }
        }
    }
}
