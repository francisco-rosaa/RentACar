using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class passrecover : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            if (Session["SuccessLogin"] != null)
            {
                Response.Redirect("userarea.aspx");
            }
        }

        protected void ButtonSend_Click(object sender, EventArgs e)
        {
            List<string> bdResponse = RecoverPassword();

            if (Convert.ToInt32(bdResponse[0]) == 1)
            {
                SendEmail(GenerateEmail(bdResponse[1]), TextBoxEmail.Text);

                Session["Message"] = "Check your email.";
                Response.Redirect("login.aspx");
            }
            else
            {
                LabelMessage.Text = "This email is not registered.";
            }
        }

        private List<string> RecoverPassword()
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();

            myCommand.Parameters.AddWithValue("@email", TextBoxEmail.Text);

            SqlParameter response = new SqlParameter();
            response.ParameterName = "@response";
            response.Direction = ParameterDirection.Output;
            response.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(response);

            SqlParameter pass = new SqlParameter();
            pass.ParameterName = "@username";
            pass.Direction = ParameterDirection.Output;
            pass.SqlDbType = SqlDbType.VarChar;
            pass.Size = 50;
            myCommand.Parameters.Add(pass);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "password_recover";
            myCommand.Connection = myConn;

            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();

                List<string> bdResponse = new List<string>();
                bdResponse.Add(Convert.ToString(myCommand.Parameters["@response"].Value));
                bdResponse.Add(Convert.ToString(myCommand.Parameters["@username"].Value));

                myConn.Close();

                return bdResponse;
            }
            catch (Exception ex)
            {
                Session["Message"] = ex.Message;
                Response.Redirect("error.aspx");
            }

            return null;
        }

        private List<string> GenerateEmail(string inputUser)
        {
            string pathSite = ConfigurationManager.AppSettings.Get("PathSite");

            string subject = "Recover your password at Mr. Wills!";

            string content =
                $"Hello {inputUser}.<br/>" +
                "To change the password click " +
                $"<a href='{pathSite}passchange.aspx?user={EncryptString(inputUser)}'>here<a>.<br/>" +
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
                ("____________________", "_____________________");
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
