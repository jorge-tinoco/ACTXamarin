using System;
using System.Linq;
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
            //CleanDB();
            connection.CreateTableAsync<Vehicle>().Wait(); // Only one item should be stored in DB. 
            connection.CreateTableAsync<Driver>().Wait();
            connection.CreateTableAsync<Rejection>().Wait();
            InitRejections();
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

        /// <summary>
        /// Insert the initial rejections that app should contain.
        /// </summary>
        internal async void InitRejections()
        {
            IEnumerable<Rejection> RejectionsInDb = await  Rejections();
            if (RejectionsInDb.Count() == 0)
            {
                await AddRejection("Paquetón Mal Armado", 54) ;
                await AddRejection("Material Revuelto (OP, Colada, EE, Etc.)", 55) ;
                await AddRejection("Exceso de Tonelaje", 56) ;
                await AddRejection("Plana Dañada", 57) ;
                await AddRejection("Mantenimiento Programado", 41) ;
                await AddRejection("Mantenimiento Correctivo", 42) ;
                await AddRejection("Cambio de Turno", 43) ;
                await AddRejection("Comida", 44) ;
                await AddRejection("Carga Combustible", 45) ;
                await AddRejection("Falta Equipo Descarga", 46) ;
                await AddRejection("Espera BME: Plana no Lista", 47) ;
                await AddRejection("Comunicaciones (Radio Saturada, Etc)", 48) ;
                await AddRejection("Leer Tarjeta/Falta Tarjeta", 49) ;
                await AddRejection("Falta de Demanda", 50) ;
                await AddRejection("Falta de Conductor", 51) ;
            }
        }

        private async void CleanDB()
        {
            connection.DropTableAsync<Vehicle>().Wait(); // Only one item should be stored in DB. 
            //connection.DropTableAsync<Driver>().Wait();
            connection.DropTableAsync<Rejection>().Wait();
        }
    }
}
