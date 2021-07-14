using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using RentACar.Entities;

namespace RentACar
{
    public partial class shoppingcart : System.Web.UI.Page
    {
        private List<Cart> reservesCart;

        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            PresentShoppingCartDetails();
        }

        private void PresentShoppingCartDetails()
        {
            reservesCart = new List<Cart>();

            if (Session["ReservesList"] != null)
            {
                reservesCart = Session["ReservesList"] as List<Cart>;

                if (Session["UserType"].ToString() == "regular")
                {
                    CalculateDiscount(reservesCart);
                }

                LabelDateToday.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LabelTotalCost.Text = CaculateTotal(reservesCart).ToString() + "€";

                if (reservesCart.Count == 0)
                {
                    LabelTotalCost.Text = "0,00€";
                    LabelMessage.Text = "Your shopping cart is currently empty.";
                }

                RepeaterShoppingCart.DataSource = reservesCart;
                RepeaterShoppingCart.DataBind();
            }
            else
            {
                LabelDateToday.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LabelTotalCost.Text = "0,00€";
                LabelMessage.Text = "Your shopping cart is currently empty.";
            }
        }

        private void CalculateDiscount(List<Cart> reservesCart)
        {
            foreach (Cart reserve in reservesCart)
            {
                reserve.Price = reserve.PriceDiscounted;
            }
        }

        private decimal CaculateTotal(List<Cart> reservesCart)
        {
            decimal total = 0;

            foreach (Cart reserve in reservesCart)
            {
                total += reserve.Price;
            }

            return total;
        }

        protected void RepeaterShoppingCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("CartDelete"))
            {
                DeleteReservation(Convert.ToInt32(e.CommandArgument));

                PresentShoppingCart();
                PresentShoppingCartDetails();
            }
        }

        private void DeleteReservation(int id)
        {
            Cart reserveToRemove = new Cart();

            foreach (Cart reserve in reservesCart)
            {
                if (reserve.Id == id)
                {
                    reserveToRemove = reserve;
                }
            }

            reservesCart.Remove(reserveToRemove);
            Session["ReservesList"] = reservesCart;

            Session["TotalReserves"] = Convert.ToInt32(Session["TotalReserves"]) - 1;
            Session["TotalCost"] = Convert.ToDecimal(Session["TotalCost"]) - reserveToRemove.Price;
            Session["TotalCostDiscounted"] = Convert.ToDecimal(Session["TotalCostDiscounted"]) - reserveToRemove.PriceDiscounted;
        }

        // Button Finalize ////////////////
        protected void ButtonFinalize_Click(object sender, EventArgs e)
        {
            if (reservesCart.Count == 0)
            {
                LabelMessage.Text = "Your shopping is empty.";
                return;
            }

            if (Session["SuccessLogin"] == null)
            {
                Session["Message"] = "Please login to finalize your reservation.";
                Response.Redirect("login.aspx");
            }

            foreach (Cart reserve in reservesCart)
            {
                if (SaveReserve(reserve.Id) != 1)
                {
                    LabelMessage.Text = "There was a problem trying to finalize the reservation.";
                    return;
                }
            }

            LabelMessage.Text = "Reservation saved successfully.";

            string newPdfPath = GeneratePdf();
            List<string> message = GenerateEmail();

            SendEmail(message, newPdfPath);
            LabelMessage.Text = "Information sent by email. Thank you for your preference.";

            ClearShoppingCart();
            Response.AddHeader("REFRESH", "3;URL=shoppingcart.aspx");
        }

        private int SaveReserve(int id_vehicle)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();

            myCommand.Parameters.AddWithValue("@id_vehicle", id_vehicle);
            myCommand.Parameters.AddWithValue("@id_user", Session["IdUser"].ToString());
            myCommand.Parameters.AddWithValue("@reservationdate", DateTime.Today);

            SqlParameter response = new SqlParameter();
            response.ParameterName = "@response";
            response.Direction = ParameterDirection.Output;
            response.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(response);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "register_reservation";
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

        private string GeneratePdf()
        {
            string pathSite = ConfigurationManager.AppSettings.Get("PathSite");
            string pathPdfFiles = ConfigurationManager.AppSettings.Get("PathPdfFiles");
            string pdfTemplate = pathPdfFiles + "Templates\\template.pdf";

            Guid nameRandom = Guid.NewGuid();
            string namePdf = nameRandom + ".pdf";
            string newPdf = pathPdfFiles + namePdf;

            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newPdf, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            string content = string.Empty;
            decimal total = 0;

            if (Session["UserType"].ToString() == "regular")
            {
                foreach (Cart reserve in reservesCart)
                {
                    content += $"{reserve.Category}  |  {reserve.Brand} {reserve.Model}  |  {reserve.PriceDiscounted}€\n\r";
                    total += reserve.PriceDiscounted;
                }

                content += $"\n\rTOTAL:  {total}€";
            }
            else
            {
                foreach (Cart reserve in reservesCart)
                {
                    content += $"{reserve.Category}  |  {reserve.Brand} {reserve.Model}  |  {reserve.Price}€\n\r";
                    total += reserve.Price;
                }

                content += $"\n\rTOTAL:  {total}€";
            }

            pdfFormFields.SetField("date", DateTime.Today.ToString("dd/MM/yyyy"));
            pdfFormFields.SetField("name", $"Dear {Session["Name"]}");
            pdfFormFields.SetField("content", content);
            pdfFormFields.SetField("message", "Thank you for your preference and happy driving.");

            pdfStamper.Close();

            string generatedPdfPath = $"{pathSite}Resources/Pdfs/{namePdf}";
            ClientScript.RegisterStartupScript(this.GetType(), "open", "window.open('" + generatedPdfPath + "','_blank' );", true);

            return newPdf;
        }

        private List<string> GenerateEmail()
        {
            string subject = "Reserve confirmed by Mr. Wills!";

            string content =
                $"Hello {Session["Name"]}!<br/>" +
                $"We confirm your reservation, please consult the attached document.<br/>" +
                "Thank you for your preference and happy driving.";

            List<string> message = new List<string>();
            message.Add(subject);
            message.Add(content);

            return message;
        }

        private void SendEmail(List<string> messageInput, string newPdfPath)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            Attachment attachment = new Attachment(newPdfPath);
            message.Attachments.Add(attachment);

            message.From = new MailAddress("____________________");
            message.To.Add(new MailAddress(Session["Email"].ToString()));
            message.Subject = messageInput[0];

            message.IsBodyHtml = true;
            message.Body = messageInput[1];

            smtp.Host = "smtp.office365.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential
                ("____________________", "____________________");
            smtp.EnableSsl = true;

            smtp.Send(message);
        }
        
        private void ClearShoppingCart()
        {
            reservesCart.Clear();
            Session["ReservesList"] = reservesCart;
            Session["TotalReserves"] = 0;
            Session["TotalCost"] = 0;
            Session["TotalCostDiscounted"] = 0;
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
