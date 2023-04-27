using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace FPTV.Services.BackupDBService
{
    /// <summary>
    /// This class is used to create a backup of a database.
    /// </summary>
    public class BackupBD
    {
        /// <summary>
        /// Backup a whole database to the specified file.
        /// </summary>
        /// <remarks>
        /// The database must not be in use when backing up
        /// The folder holding the file must have appropriate permissions given
        /// </remarks>
        public static void BackupDatabase()
        {
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Backup.bak");

            ServerConnection con = new ServerConnection(@"Server=(localdb)\\mssqllocaldb;Database=FPTV;Trusted_Connection=True;MultipleActiveResultSets=true");
            Server server = new Server(con);
            Backup source = new Backup();
            source.Action = BackupActionType.Database;
            source.Database = "FPTV";
            BackupDeviceItem destination = new BackupDeviceItem(startupPath, DeviceType.File);
            source.Devices.Add(destination);
            source.SqlBackup(server);
            con.Disconnect();
        }
        /// <summary>
        /// Restore a whole database from a backup file.
        /// </summary>
        /// <remarks>
        /// The database must not be in use when backing up
        /// The folder holding the file must have appropriate permissions given
        /// </remarks>
        public static void RestoreDatabase()
        {
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Backup.bak");

            ServerConnection con = new ServerConnection(@"Server=(localdb)\\mssqllocaldb;Database=FPTV;Trusted_Connection=True;MultipleActiveResultSets=true");
            Server server = new Server(con);
            Restore destination = new Restore();
            destination.Action = RestoreActionType.Database;
            destination.Database = "MyDataBaseName";
            BackupDeviceItem source = new BackupDeviceItem(startupPath, DeviceType.File);
            destination.Devices.Add(source);
            destination.ReplaceDatabase = true;
            destination.SqlRestore(server);
        }
    }
}
