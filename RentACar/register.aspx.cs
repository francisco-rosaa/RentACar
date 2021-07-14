using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            if (Session["SuccessLogin"] != null)
            {
                Response.Redirect("userarea.aspx");
            }
        }

        protected void ButtonRegister_Click(object sender, EventArgs e)
        {
            if (IsUserValid(TextBoxUser.Text))
            {
                if (TextBoxPassword.Text == TextBoxConfirm.Text)
                {
                    if (IsPasswordStrong(TextBoxPassword.Text))
                    {
                        if (RegisterUser() == 1)
                        {
                            SendEmail(GenerateEmail(TextBoxUser.Text), TextBoxEmail.Text);

                            Session["Message"] = "Registration successful. Check your email to activate the account.";
                            Response.Redirect("login.aspx");
                        }
                        else
                        {
                            LabelMessage.Text = "This user or email already exists.";
                        }
                    }
                    else
                    {
                        LabelMessage.Text = "Password must contain:<br/><br/>" +
                            "Between 6 and 32 characters<br/>" +
                            "At least one capital letter<br/>" +
                            "At least one lowercase letter<br/>" +
                            "At least one number<br/>" +
                            "At least one special character<br/>" +
                            "Zero quotes";
                    }
                }
                else
                {
                    LabelMessage.Text = "The passwords entered are not the same.";
                }
            }
            else
            {
                LabelMessage.Text = "Username must contain:<br/><br/>" +
                    "At least four characters<br/>" +
                    "Only special characters ._-<br/>" +
                    "Zero quotes";
            }
        }

        private bool IsUserValid(string inputUser)
        {
            Regex special = new Regex(@"[^a-zA-Z0-9\._-]");
            Regex quote = new Regex("'");

            if (inputUser.Length < 4)
            {
                return false;
            }

            if (special.Matches(inputUser).Count > 0)
            {
                return false;
            }

            if (quote.Matches(inputUser).Count > 0)
            {
                return false;
            }

            return true;
        }

        private bool IsPasswordStrong(string inputPassword)
        {
            Regex capital = new Regex("[A-Z]");
            Regex lowercase = new Regex("[a-z]");
            Regex numbers = new Regex("[0-9]");
            Regex special = new Regex("[^a-zA-Z0-9]");
            Regex quote = new Regex("'");

            if (inputPassword.Length < 6 || inputPassword.Length > 32)
            {
                return false;
            }

            if (capital.Matches(inputPassword).Count < 1)
            {
                return false;
            }

            if (lowercase.Matches(inputPassword).Count < 1)
            {
                return false;
            }

            if (numbers.Matches(inputPassword).Count < 1)
            {
                return false;
            }

            if (special.Matches(inputPassword).Count < 1)
            {
                return false;
            }

            if (quote.Matches(inputPassword).Count > 0)
            {
                return false;
            }

            return true;
        }

        private int RegisterUser()
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();

            myCommand.Parameters.AddWithValue("@username", TextBoxUser.Text);
            myCommand.Parameters.AddWithValue("@password", EncryptString(TextBoxPassword.Text));
            myCommand.Parameters.AddWithValue("@name", TextBoxName.Text);
            myCommand.Parameters.AddWithValue("@email", TextBoxEmail.Text);
            myCommand.Parameters.AddWithValue("@id_profile", DropDownListProfiles.SelectedValue.ToString());

            SqlParameter response = new SqlParameter();
            response.ParameterName = "@response";
            response.Direction = ParameterDirection.Output;
            response.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(response);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "user_register";
            myCommand.Connection = myConn;

            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();

                int bdResponse = Convert.ToInt32(myCommand.Parameters["@response"].Value);

                myConn.Close();

                return bdResponse;
            }
            catch (Exception ex)
            {
                Session["Message"] = ex.Message;
                Response.Redirect("error.aspx");
            }

            return 0;
        }

        private List<string> GenerateEmail(string inputUser)
        {
            string pathSite = ConfigurationManager.AppSettings.Get("PathSite");

            string subject = "Welcome to Mr. Wills!";

            string content =
                $"Hello {inputUser}!<br/>" +
                "Welcome to Mr. Wills.<br/>" +
                "To activate your account please click " +
                $"<a href='{pathSite}activate.aspx?user={EncryptString(inputUser)}'>here<a>.<br/>" +
                "Thank you for your preference and happy driving.";

            List<string> message = new List<string>();
            message.Add(subject);
            message.Add(content);

            return message;
        }

        private void SendEmail(List<string> inputMessage, string inputDestity)
        {
            MailMessage eMail = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            eMail.From = new MailAddress("____________________");
            eMail.To.Add(new MailAddress(inputDestity));

            eMail.Subject = inputMessage[0];
            eMail.IsBodyHtml = true;
            eMail.Body = inputMessage[1];

            smtp.Host = "smtp.office365.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential
                ("____________________", "____________________");
            smtp.EnableSsl = true;

            smtp.Send(eMail);
        }

        public static string EncryptString(string Message)
        {
            string Passphrase = "mrwills";
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            string enc = Convert.ToBase64String(Results);
            enc = enc.Replace("+", "KKK");
            enc = enc.Replace("/", "JJJ");
            enc = enc.Replace("\\", "III");
            return enc;
        }

        private void PresentShoppingCart()
        {
            if (
                Session["TotalReserves"] != null &&
                Session["TotalCost"] != null &&
                Session["TotalCostDiscounted"] != null
                )
            {
                HtmlForm form1 = this.Master.FindControl("form1") as HtmlForm;
                Label totalReserves = form1.FindControl("LabelTotalReserves") as Label;
                Label totalCost = form1.FindControl("LabelTotalCost") as Label;
                totalReserves.Text = Session["TotalReserves"].ToString();

                if (Session["UserType"] == null)
                {
                    totalCost.Text = Session["TotalCost"].ToString();
                }
                else
                {
                    if (Session["UserType"].ToString() == "regular")
                    {
                        totalCost.Text = Session["TotalCostDiscounted"].ToString();
                    }
                    else
                    {
                        totalCost.Text = Session["TotalCost"].ToString();
                    }
                }
            }
        }
    }
}
