using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _204622H_AppSec_Asgn1
{

    public partial class Home : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null) {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else { 
                    // u put ur message or sth for Home page to display
                }
                
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

        }
        protected void btn_LogOut(object sender, EventArgs e)
        {
            LogoutAction();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null) {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.Net_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            
        }
        //protected void logging()
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
        //        {
        //            using (SqlCommand cmdd = new SqlCommand("INSERT INTO Logs VALUES(@Email,@Action,@Occurence)"))
        //            {
        //                Console.WriteLine("testing");
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    cmdd.CommandType = System.Data.CommandType.Text;
        //                    //cmdd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
        //                    cmdd.Parameters.AddWithValue("@Action", "Registration");
        //                    cmdd.Parameters.AddWithValue("@Occurence", DateTime.Now);



        //                    cmdd.Connection = conn;
        //                    conn.Open();
        //                    cmdd.ExecuteNonQuery();
        //                    conn.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.ToString());
        //    }

        //}
        protected void LogoutAction()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmdd = new SqlCommand("UPDATE Logs SET Actions=@Actions WHERE Email=@Email"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {

                            cmdd.CommandType = CommandType.Text;
                            cmdd.Parameters.AddWithValue("@Email", Session["LoggedIn"]);
                            cmdd.Parameters.AddWithValue("@Actions", "Logout");
                            cmdd.Parameters.AddWithValue("@Occurence", DateTime.Now);
                            cmdd.Connection = conn;
                            conn.Open();
                            cmdd.ExecuteNonQuery();
                            conn.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}