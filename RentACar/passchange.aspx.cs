using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class passchange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            string userUrl = Request.QueryString["user"];

            if (string.IsNullOrEmpty(userUrl))
            {
                if (Session["SuccessLogin"] == null)
                {
                    Response.Redirect("vehicles.aspx");
                }
            }
            else
            {
                Session["UserName"] = DecryptString(userUrl);
            }
        }

        protected void ButtonConfirm_Click(object sender, EventArgs e)
        {
            ChangePassword(Session["UserName"].ToString());
        }

        private void ChangePassword(string inputUser)
        {
            if (TextBoxNewPass.Text == TextBoxConfirm.Text)
            {
                if (IsPasswordStrong(TextBoxConfirm.Text))
                {
                    if (ChangePassword(inputUser, TextBoxConfirm.Text) == 1)
                    {
                        Session["IdUser"] = null;
                        Session["UserName"] = null;
                        Session["Name"] = null;
                        Session["Email"] = null;
                        Session["UserType"] = "normal";
                        Session["SuccessLogin"] = null;

                        Session["Message"] = "Password changed successfully.";
                        Response.Redirect("login.aspx");
                    }
                    else
                    {
                        Session["Message"] = "There was a problem trying to change the password. Try again.";
                        Response.Redirect("error.aspx");
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

        private int ChangePassword(string inputUser, string inputPassword)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();

            myCommand.Parameters.AddWithValue("@username", inputUser);
            myCommand.Parameters.AddWithValue("@password", EncryptString(inputPassword));

            SqlParameter response = new SqlParameter();
            response.ParameterName = "@response";
            response.Direction = ParameterDirection.Output;
            response.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(response);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "password_change";
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

        internal string DecryptString(string Message)
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

            Message = Message.Replace("KKK", "+");
            Message = Message.Replace("JJJ", "/");
            Message = Message.Replace("III", "\\");

            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);
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
