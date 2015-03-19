using System.Data;
using System.Data.SqlClient;
using System.Threading;
using MiscUtils.Framework;

namespace MiscUtils.Data
{
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Associate the current user and locale with the SQL connection.
        /// The user and locale are inferred from the current thread.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public static void SetContextInfo(this SqlConnection connection)
        {
            connection.ThrowIfNull("connection");

            // Just be careful here.
            if (Thread.CurrentPrincipal == null ||
                Thread.CurrentPrincipal.Identity == null ||
                Thread.CurrentThread.CurrentUICulture == null
                )
                return;

            SetContextInfo
                (
                connection,
                Thread.CurrentPrincipal.Identity.Name,
                Thread.CurrentThread.CurrentUICulture.ToString()
                );
        }

        /// <summary>
        /// Associate the specified user and locale with the SQL connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="locale">The .net locale, e.g. "en-gb".</param>
        public static void SetContextInfo(this SqlConnection connection, string userId, string locale)
        {
            connection.ThrowIfNull("conn");
            userId.ThrowIfNullOrWhiteSpace("userId");

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.SetContextInfo";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Locale", locale);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
