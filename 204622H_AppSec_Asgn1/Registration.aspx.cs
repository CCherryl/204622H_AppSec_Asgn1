using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _204622H_AppSec_Asgn1
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_Validate(object sender, EventArgs e)
        {

            // Fname
            //checkName(tb_Fname.Text);
            lbl_Fname.ForeColor = Color.Red;
            lbl_Fname.Text = checkName(tb_Fname.Text);

            // Lname
            lbl_Lname.ForeColor = Color.Red;
            lbl_Lname.Text = checkLname(tb_Lname.Text);

            // Dob
            lbl_Dob.ForeColor = Color.Red;
            lbl_Dob.Text = checkDob(tb_dob.Text);

            // Email
            lbl_Email.ForeColor = Color.Red;
            lbl_Email.Text = checkEmail(tb_email.Text);

            // Credit Card
            lbl_Card.ForeColor = Color.Red;
            lbl_Card.Text = checkCard(tb_card.Text);

            // Photo

            // Password
            if (tb_Password.Text == " ")
            {
                pwd_checker.ForeColor = Color.Red;
                pwd_checker.Text = "Field is required!";
            }
            else {
                pwd_checker.Text = " ";
            }
            int scores = checkPassword(tb_Password.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            pwd_checker2.Text = "Status : " + status;
            if (scores < 4)
            {
                pwd_checker2.ForeColor = Color.Red;
                return;
            }
            else {
                pwd_checker2.ForeColor = Color.Green;
            }

            // string pwd = get value from your textbox
            string pwd = tb_Password.Text.ToString().Trim();
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
            if (lbl_Lname.Text != "Field is required!" && lbl_Fname.Text != "Field is required!" && lbl_Email.Text != "Field is required!" && lbl_Email.Text != "Email has been registered please try another email!") {
                logging();
                createAccount();
                
            }

            

        }



        private string checkName(string Fname)
        {
            if (Fname == "")
            {
                return "Field is required!";
            }
            else if (Regex.IsMatch(Fname, "[0-9]"))
            {
                return "Field must not contain Numbers";
            }
            return " ";

        }
        private string checkLname(string Lname)
        {
            if (Lname == "")
            {
                return "Field is required!";
            }
            else if (Regex.IsMatch(Lname, "[0-9]"))
            {
                return "Field must not contain Numbers";
            }
            return " ";

        }
        private string checkDob(string Dob) {
            if (Dob == null) {
                return "Field is required!";
            }
            return " ";
        }
        private string checkEmail(string Email) {
            
            if (Email == "")
            {
                return "Field is required!";
            }
            else if (Regex.IsMatch(Email, @"[[^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$]]")){
                return "Invalid Email format";
            }
            else {
               return uniqueEmail();
            }
                }

        private string checkCard(string Card) {
            if (Card == "")
            {
                return "Field is required!";
            }
            else if (Card.Length < 16 || Card.Length > 16)
            {
                return "Invalid card length!";
            }
            else {
                return " ";
            }
           
                }

        private string checkPhoto(string photo) {
            return " ";
        }

        private int checkPassword(string password)
        {
            int score = 0;

            // include your implementation here

            // Score 0 is very weak!
            // if length of password is less than 8 characters

            if (password.Length < 8)
            {
                return 1;
            }

            else
            {
                score = 1;
            }

            // Score 2 Weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            // Score 3 Medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            // Score 4 Strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            // Score 5 Excellent
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }
            return score;
        }
        private String uniqueEmail() {
            
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT EmailAddress FROM Accounts Where EmailAddress=@EmailAddress";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EmailAddress", tb_email.Text);

            string comment = "";
            try
            {
                // string selectSQL = "select Status FROM Accounts WHERE EmailAddress=@EmailAddress";
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["EmailAddress"] != null)
                        {
                            if (reader["EmailAddress"].ToString() == tb_email.Text)
                            {
                                comment = "Email has been registered please try another email!";
                            }
                            else {
                                comment = "";
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


            return comment ;

        }
        protected void logging() {
            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmdd = new SqlCommand("INSERT INTO Logs VALUES(@Email,@Action,@Occurence)"))
                    {
                        Console.WriteLine("testing");
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmdd.CommandType = System.Data.CommandType.Text;
                            cmdd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmdd.Parameters.AddWithValue("@Action", "Registration");
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

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts VALUES(@FirstName,@LastName,@DateOfBirth,@EmailAddress,@CreditCard,@Photo,@Password,@PasswordHash,@PasswordSalt,@IV,@Key,@Status,@Count,@LockoutTimer)"))
                    {
                        Console.WriteLine("testing");
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_Fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_Lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@DateOfBirth", tb_dob.Text.Trim());
                            cmd.Parameters.AddWithValue("@EmailAddress", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCard", Convert.ToBase64String(encryptData(tb_card.Text.Trim())));
                            if (tb_photo.HasFile)
                            {
                                string fileName = tb_photo.FileName.ToString();
                                tb_photo.PostedFile.SaveAs(Server.MapPath("~/Image/") + fileName);
                                cmd.Parameters.AddWithValue("@Photo", fileName);

                            }
                            else {
                                cmd.Parameters.AddWithValue("@Photo", DBNull.Value);
                            }

                            // cmd.Parameters.AddWithValue("@Photo", tb_photo.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", tb_Password.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@Status", false);
                            cmd.Parameters.AddWithValue("@Count", 0);
                            cmd.Parameters.AddWithValue("@LockoutTimer", 0);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex) {
              
                throw new Exception(ex.ToString());
            }
            Response.Redirect("Login.aspx");
        }

        protected byte[] encryptData(string data) {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
               
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;

        }


    }
}
    