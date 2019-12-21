using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SeventhServices.QQRobot.Extensions
{
    public static class DbContextExtension
    {

        /// <summary>
        /// Detach all entry in dbContext.  
        /// </summary>
        /// <param name="dbContext"></param>
        public static void DetachAll(this DbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

        }
    }
}
