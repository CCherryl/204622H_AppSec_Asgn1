using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _204622H_AppSec_Asgn1
{
    public partial class Login : System.Web.UI.Page
    {
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        string MYDBConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

        byte[] Key;
        byte[] IV;

        

        protected void btn_Login(object sender, EventArgs e)
        {
            if (tb_email.Text == "" || tb_password.Text == "")
            {
                if (tb_email.Text == "")
                {
                    lbl_email.ForeColor = Color.Red;
                    lbl_email.Text = "Field is required!";

                }
                if (tb_password.Text == "")
                {
                    pwd_checker.Text = "Field is required!";
                    pwd_checker.ForeColor = Color.Red;
                }
            }
            else {
                lbl_email.Text = "";
                pwd_checker.Text = "";
                string pwd = tb_password.Text.ToString().Trim();
            string userid = tb_email.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
                // int count += 1;
                if (ValidateCaptcha())
                {
                    selectDate();
                    if (selectCurrentStatus() == 0 || UpdateCurrentStatus() == 0)
                    {

                        try
                        {


                            if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                            {
                                string pwdWithSalt = pwd + dbSalt;
                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                string userHash = Convert.ToBase64String(hashWithSalt);
                                if (userHash.Equals(dbHash))
                                {
                                    logging();
                                    Session["LoggedIn"] = tb_email.Text.Trim();

                                    string guid = Guid.NewGuid().ToString();
                                    Session["AuthToken"] = guid;

                                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                    Response.Redirect("Home.aspx", false);

                                }
                                else
                                {
                                    lbl_error.Text = "Incorrect password or username";
                                    lbl_error.ForeColor = Color.Red;
                                    CountIncrement();

                                    //lbl_error.Text = "Help ur account died 1";
                                    if (getCount() > 2)
                                    {
                                        lbl_error.Text = "Please wait 1 minute to recover your account!";
                                        lbl_error.ForeColor = Color.Red;
                                        timeLocked();
                                        UpdateCurrentStatus();

                                    }
                                }

                            }

                            // lbl_error.Text = "incorrect password or username";
                            else
                            {

                                lbl_error.Text = "Please wait 1 minute to recover your account!";
                                lbl_error.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(message: ex.ToString());
                        }


                    }

                }
                    
                
               

            }

        }
        protected void logging()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmdd = new SqlCommand("INSERT INTO Logs VALUES(@Email,@Action,@Occurence)"))
                    {
                        Console.WriteLine("testing");
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmdd.CommandType = CommandType.Text;
                            cmdd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmdd.Parameters.AddWithValue("@Action", "Login");
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
        protected DateTime timeLocked() {

            DateTime locked = DateTime.Now.AddMinutes(1);
            //now.AddSeconds(300);
            try
            {
               

                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmdd = new SqlCommand("UPDATE Accounts SET lockOutTimer=@lockOutTimer where EmailAddress=@EmailAddress"))
                    {
                        Console.WriteLine("testing");
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmdd.CommandType = CommandType.Text;
                            cmdd.Parameters.AddWithValue("@EmailAddress", tb_email.Text.Trim());
                            cmdd.Parameters.AddWithValue("@lockOutTimer", DateTime.Now.AddMinutes(1));
                            



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
            return locked;
        }
        protected int selectDate() {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT lockOutTimer FROM Accounts WHERE EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", tb_email.Text);

            int stat = 0;
            try
            {
                // string selectSQL = "select Status FROM Accounts WHERE EmailAddress=@EmailAddress";
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lockOutTimer"] != null)
                        {
                            if (Convert.ToDateTime(reader["lockOutTimer"]) <= DateTime.Now && selectCurrentStatus() == 1)
                            {
                                stat = 0;
                            }
                            else  {
                                stat = 1;
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

            return stat;

        }
      
        protected int selectCurrentStatus()
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT Status FROM Accounts WHERE EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", tb_email.Text);

            int stat = 0;
            try
            {
                // string selectSQL = "select Status FROM Accounts WHERE EmailAddress=@EmailAddress";
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Status"] != null)
                        {
                            //if (selectCounter() == 0)
                            //{
                            //    return stat = -1;
                            //}
                            
                                stat = Convert.ToInt32(reader["Status"]);
                            
                           
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }

            return stat;

        }
       
        protected int UpdateCurrentStatus()
        {
           
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Accounts SET Status = @Status WHERE EmailAddress = @EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", tb_email.Text);
            //  command.Parameters.AddWithValue("@LockoutTimer", 30);
            if (selectDate() == 0)
            {
                CountUpdate();
                command.Parameters.AddWithValue("@Status", 0);
                
            }
           else { 
                command.Parameters.AddWithValue("@Status", selectCurrentStatus() + 1);
                
            }
            
           


            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return selectCurrentStatus();
        }

        // Get the smount of time user tries to login
        protected int getCount()
        {
            int x = 0;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Count FROM Accounts WHERE EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", tb_email.Text);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Count"] != null)
                        {
                            x = Convert.ToInt32(reader["Count"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return x;


        }


        protected int CountIncrement()
        {
            int x = getCount();

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Accounts SET Count=@Count WHERE EmailAddress = @EmailAddress"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                          cmd.Parameters.AddWithValue("@Count", getCount() + 1);

                         cmd.Parameters.AddWithValue("@EmailAddress", tb_email.Text);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return x;
        }

        protected int CountUpdate()
        {
            int x = getCount();

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Accounts SET Count=@Count WHERE EmailAddress = @EmailAddress"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {

                            if (selectDate() == 0)
                            {
                                cmd.Parameters.AddWithValue("@Count", 0);
                            }
                            cmd.Parameters.AddWithValue("@EmailAddress", tb_email.Text);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return x;
        }

        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Accounts WHERE EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", userid);
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

        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt FROM ACCOUNTS WHERE EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", userid);
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

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret= &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);
                    }

                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
    }