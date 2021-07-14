using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class activate : System.Web.UI.Page
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
                ActivateUser(DecryptString(userUrl));
            }
        }

        private void ActivateUser(string inputUser)
        {
            if (ActivateAccount(inputUser) == 1)
            {
                Session["Message"] = "Account activated successfully.";
                Response.AddHeader("REFRESH", "3;URL=login.aspx");
            }
            else
            {
                Session["Message"] = "There was a problem trying to activate the account.";
                Response.Redirect("error.aspx");
            }
        }

        private int ActivateAccount(string inputUser)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();

            myCommand.Parameters.AddWithValue("@username", inputUser);

            SqlParameter response = new SqlParameter();
            response.ParameterName = "@response";
            response.Direction = ParameterDirection.Output;
            response.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(response);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "user_activate";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();

            int bdResponse = Convert.ToInt32(myCommand.Parameters["@response"].Value);

            myConn.Close();

            return bdResponse;
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
