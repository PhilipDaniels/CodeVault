using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Web.Configuration;
using System.Web.UI;
using System.Configuration;

namespace IdentityTester
{
    public partial class Identify : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DisplayIdentity();
        }

        protected void btnChangeImpersonation_Click(object sender, EventArgs e)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            IdentitySection isec = (IdentitySection)config.GetSection("system.web/identity");
            isec.Impersonate = !isec.Impersonate;
            config.Save();
            DisplayIdentity();
        }

        private void DisplayIdentity()
        {
            AuthenticationSection asec = (AuthenticationSection)WebConfigurationManager.GetSection("system.web/authentication");
            IdentitySection isec = (IdentitySection)WebConfigurationManager.GetSection("system.web/identity");
            litAuthenticationMode.Text = asec.Mode.ToString();
            litImpersonate.Text = isec.Impersonate.ToString();

            litPageUserIdentity.Text = Page.User.Identity.Name;
            litWindowsCurrentIdentity.Text = WindowsIdentity.GetCurrent().Name;
            litThreadCurrentIdentity.Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = WebConfigurationManager.ConnectionStrings[0].ConnectionString;
            litConnStr.Text = conn.ConnectionString;



            // In block.
            WindowsIdentity id = (WindowsIdentity)Page.User.Identity;
            using (WindowsImpersonationContext ctx = id.Impersonate())
            {
                conn.Open();
                litDatabaseInBlock.Text = GetDatabaseIdentity(conn);
            }

            // After block, using connection opened above.
            try
            {
                litDatabaseAfterBlock.Text = GetDatabaseIdentity(conn);
            }
            catch (Exception ex)
            {
                litDatabaseAfterBlock.Text = "Exception: " + ex.Message;
            }
            finally
            {
                conn.Close();
            }



            // Outside block, using a newly opened connection.
            try
            {
                conn.Open();
                litDatabaseOutsideBlock.Text = GetDatabaseIdentity(conn);
            }
            catch (Exception ex)
            {
                litDatabaseOutsideBlock.Text = "Exception: " + ex.Message;
            }
            finally
            {
                conn.Close();
            }

            string host = Request.ServerVariables["SERVER_SOFTWARE"];
            if (String.IsNullOrEmpty(host))
                host = "(unknown)";
            litHostSoftware.Text = host;
        }

        private string GetDatabaseIdentity(SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand()) 
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT SUSER_SNAME()";
                string dbid = (string)cmd.ExecuteScalar();
                return dbid;
            }
        }
    }
}
