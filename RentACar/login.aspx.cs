using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            if (Session["Message"] != null)
            {
                LabelMessage.Text = Session["Message"].ToString();
                Session["Message"] = null;
            }

            if (Session["SuccessLogin"] != null)
            {
                if (Session["UserType"].ToString() == "admin")
                {
                    Response.Redirect("backoffice.aspx");
                }
                else
                {
                    Response.Redirect("vehicles.aspx");
                }
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            List<string> bdResponse = UserLogin();

            if (Convert.ToInt32(bdResponse[0]) == 0)
            {
                LabelMessage.Text = "User and/or password not valid.";
            }
            else if (Convert.ToInt32(bdResponse[0]) == 1)
            {
                LabelMessage.Text = "User not yet activated. Verify your email.";
            }
            else if (Convert.ToInt32(bdResponse[0]) == 2)
            {
                if (bdResponse[4] == "normal")
                {
                    Session["IdUser"] = bdResponse[1];
                    Session["UserName"] = TextBoxUser.Text;
                    Session["Name"] = bdResponse[2];
                    Session["Email"] = bdResponse[3];
                    Session["UserType"] = "normal";
                    Session["SuccessLogin"] = "Yes";
                    Response.Redirect("vehicles.aspx");
                }
                else if (bdResponse[4] == "regular")
                {
                    Session["IdUser"] = bdResponse[1];
                    Session["UserName"] = TextBoxUser.Text;
                    Session["Name"] = bdResponse[2];
                    Session["Email"] = bdResponse[3];
                    Session["UserType"] = "regular";
                    Session["SuccessLogin"] = "Yes";
                    Response.Redirect("vehicles.aspx");
                }
                else if (bdResponse[4] == "admin")
                {
                    Session["IdUser"] = bdResponse[1];
                    Session["UserName"] = TextBoxUser.Text;
                    Session["Name"] = bdResponse[2];
                    Session["Email"] = bdResponse[3];
                    Session["UserType"] = "admin";
                    Session["SuccessLogin"] = "Yes";
                    Response.Redirect("backoffice.aspx");
                }
                else
                {
                    Session["Message"] = "Error in authentication.";
                    Response.Redirect("error.aspx");
                }
            }
            else
            {
                Session["Message"] = "Error in authentication.";
                Response.Redirect("error.aspx");
            }
        }

        private List<string> UserLogin()
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();

            myCommand.Parameters.AddWithValue("@username", TextBoxUser.Text);
            myCommand.Parameters.AddWithValue("@password", EncryptString(TextBoxPassword.Text));

            SqlParameter response = new SqlParameter();
            response.ParameterName = "@response";
            response.Direction = ParameterDirection.Output;
            response.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(response);

            SqlParameter id_user = new SqlParameter();
            id_user.ParameterName = "@id_user";
            id_user.Direction = ParameterDirection.Output;
            id_user.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(id_user);

            SqlParameter name = new SqlParameter();
            name.ParameterName = "@name";
            name.Direction = ParameterDirection.Output;
            name.SqlDbType = SqlDbType.VarChar;
            name.Size = 100;
            myCommand.Parameters.Add(name);

            SqlParameter email = new SqlParameter();
            email.ParameterName = "@email";
            email.Direction = ParameterDirection.Output;
            email.SqlDbType = SqlDbType.VarChar;
            email.Size = 50;
            myCommand.Parameters.Add(email);

            SqlParameter id_profile = new SqlParameter();
            id_profile.ParameterName = "@id_profile";
            id_profile.Direction = ParameterDirection.Output;
            id_profile.SqlDbType = SqlDbType.VarChar;
            id_profile.Size = 50;
            myCommand.Parameters.Add(id_profile);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "user_login";
            myCommand.Connection = myConn;

            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();

                List<string> bdResponse = new List<string>();
                bdResponse.Add(Convert.ToString(myCommand.Parameters["@response"].Value));
                bdResponse.Add(Convert.ToString(myCommand.Parameters["@id_user"].Value));
                bdResponse.Add(Convert.ToString(myCommand.Parameters["@name"].Value));
                bdResponse.Add(Convert.ToString(myCommand.Parameters["@email"].Value));
                bdResponse.Add(Convert.ToString(myCommand.Parameters["@id_profile"].Value));

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
