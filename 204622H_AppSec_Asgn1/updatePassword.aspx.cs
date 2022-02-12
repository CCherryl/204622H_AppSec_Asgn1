using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _204622H_AppSec_Asgn1
{
    
    public partial class updatePassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    // u put ur message or sth for Home page to display
                }

            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

        }
        protected void btn_Change(object sender, EventArgs e) {


            // string pwd = get value from your textbox
            string pwd = tb_newpwd.Text.ToString().Trim();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            if (validation() == 1) {
                changes();
            }
            
        }
        protected int validation()
        {
            int check = 0;
            if (tb_oldpwd.Text == "")
            {
                lbl_old.ForeColor = Color.Red;
                lbl_old.Text = "Field is required!";
             
            }
            if (tb_newpwd.Text == "") {
                pwd_checker.Text = "Field is required!";
                pwd_checker.ForeColor = Color.Red;
            }
            if (tb_Cnewpwd.Text == "")
            {
                lbl_new.Text = "Field is required!";
                lbl_new.ForeColor = Color.Red;
            }





            string pwd = tb_oldpwd.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash();
                string dbSalt = getDBSalt();

                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash == getDBHash())
                    {
                        lbl_old.Text = "";
                        string pwdWithSalt2 = tb_newpwd.Text.ToString().Trim() + dbSalt;
                        byte[] hashWithSalt2 = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt2));
                        string userHash2 = Convert.ToBase64String(hashWithSalt2);

                        if (userHash == userHash2)
                        {
                            lbl_Response.Text = "Old Password cannot be reused!";
                            lbl_Response.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (tb_newpwd.Text == tb_Cnewpwd.Text)
                            {
                            lbl_old.Text = "";
                            lbl_new.Text = "";
                            pwd_checker.Text = "";
                            lbl_Response.Text = "Password has been successfully changed";
                                lbl_Response.ForeColor = Color.Green;
                                check = 1;
                                changes();

                            }
                            else
                            {
                                lbl_new.Text = "Password does not match!";
                                lbl_new.ForeColor = Color.Red;
                                check = 0;
                            }
                        }
                    }
                    else
                    {
                        lbl_old.Text = "Incorrect old password!";
                        lbl_old.ForeColor = Color.Red;
                    }
                

            }
            
            return check;
        }

        protected void changes() {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Accounts SET PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt, Password=@Password WHERE EmailAddress = @EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PasswordHash", finalHash);
            command.Parameters.AddWithValue("@PasswordSalt", salt);
            command.Parameters.AddWithValue("@Password", tb_newpwd.Text);
            command.Parameters.AddWithValue("@EmailAddress", Session["LoggedIn"]);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            
        }
        protected string getDBHash()
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Accounts WHERE EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", Session["LoggedIn"]);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected string getDBSalt()
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt FROM ACCOUNTS WHERE EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress",Session["LoggedIn"]);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
    }

}