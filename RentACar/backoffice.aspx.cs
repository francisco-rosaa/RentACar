using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class backoffice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            if (Session["SuccessLogin"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Session["UserType"].ToString() != "admin")
            {
                Response.Redirect("vehicles.aspx");
            }
        }

        protected void RepeaterVehicles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;

                (e.Item.FindControl("LabelIdVehicle") as Label).Text = dr.Row["id_vehicle"].ToString();
                (e.Item.FindControl("DropDownListCategories") as DropDownList).SelectedValue = dr.Row["id_category"].ToString();
                (e.Item.FindControl("TextBoxBrand") as TextBox).Text = dr.Row["brand"].ToString();
                (e.Item.FindControl("TextBoxModel") as TextBox).Text = dr.Row["model"].ToString();
                (e.Item.FindControl("TextBoxDescription") as TextBox).Text = dr.Row["description"].ToString();
                (e.Item.FindControl("TextBoxPrice") as TextBox).Text = dr.Row["price"].ToString();
                (e.Item.FindControl("CheckBoxAvailable") as CheckBox).Checked = Convert.ToBoolean(dr.Row["available"]);
                (e.Item.FindControl("ImageVehicle") as Image).ImageUrl =
                    dr.Row["image"].ToString() != string.Empty
                    ? "data:image/png;base64," + Convert.ToBase64String(dr.Row["image"] as byte[])
                    : string.Empty;

                (e.Item.FindControl("ButtonUploadImage") as Button).CommandArgument = dr.Row["id_vehicle"].ToString();
                (e.Item.FindControl("ButtonDeleteImage") as Button).CommandArgument = dr.Row["id_vehicle"].ToString();
                (e.Item.FindControl("ButtonSaveVehicle") as Button).CommandArgument = dr.Row["id_vehicle"].ToString();
                (e.Item.FindControl("ButtonDeleteVehicle") as Button).CommandArgument = dr.Row["id_vehicle"].ToString();
            }
        }

        protected void RepeaterVehicles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = myConn;

            if (e.CommandName.Equals("ButtonUploadImage"))
            {
                if ((e.Item.FindControl("FileUploadImage") as FileUpload).HasFile)
                {
                    Stream imgStream = (e.Item.FindControl("FileUploadImage") as FileUpload).PostedFile.InputStream;
                    string imageType = (e.Item.FindControl("FileUploadImage") as FileUpload).PostedFile.ContentType;

                    int imgLength = (e.Item.FindControl("FileUploadImage") as FileUpload).PostedFile.ContentLength;
                    byte[] imgBinary = new byte[imgLength];
                    imgStream.Read(imgBinary, 0, imgLength);

                    myCommand.Parameters.AddWithValue("@id_vehicle", (e.Item.FindControl("ButtonUploadImage") as Button).CommandArgument);
                    myCommand.Parameters.AddWithValue("@imagetype", imageType);
                    myCommand.Parameters.AddWithValue("@image", imgBinary);

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "upload_vehicle_image";
                }
            }

            if (e.CommandName.Equals("ButtonDeleteImage"))
            {
                if ((e.Item.FindControl("ImageVehicle") as Image).ImageUrl != string.Empty)
                {
                    myCommand.Parameters.AddWithValue("@id_vehicle", (e.Item.FindControl("ButtonUploadImage") as Button).CommandArgument);

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "delete_vehicle_image";
                }
            }

            if (e.CommandName.Equals("ButtonSaveVehicle"))
            {
                if (
                    (e.Item.FindControl("DropDownListCategories") as DropDownList).SelectedValue != null &&
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxBrand") as TextBox).Text) &&
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxModel") as TextBox).Text) &&
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxDescription") as TextBox).Text) &&
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxPrice") as TextBox).Text)
                    )
                {
                    myCommand.Parameters.AddWithValue("@id_vehicle", (e.Item.FindControl("ButtonUploadImage") as Button).CommandArgument);
                    myCommand.Parameters.AddWithValue("@id_category", (e.Item.FindControl("DropDownListCategories") as DropDownList).SelectedItem.ToString());
                    myCommand.Parameters.AddWithValue("@brand", (e.Item.FindControl("TextBoxBrand") as TextBox).Text);
                    myCommand.Parameters.AddWithValue("@model", (e.Item.FindControl("TextBoxModel") as TextBox).Text);
                    myCommand.Parameters.AddWithValue("@description", (e.Item.FindControl("TextBoxDescription") as TextBox).Text);
                    myCommand.Parameters.AddWithValue("@price", (Convert.ToDecimal((e.Item.FindControl("TextBoxPrice") as TextBox).Text)).ToString(CultureInfo.InvariantCulture));
                    myCommand.Parameters.AddWithValue("@available", (e.Item.FindControl("CheckBoxAvailable") as CheckBox).Checked.ToString());

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "save_vehicle";
                }
            }

            if (e.CommandName.Equals("ButtonDeleteVehicle"))
            {
                if (!string.IsNullOrEmpty((e.Item.FindControl("LabelIdVehicle") as Label).Text))
                {
                    myCommand.Parameters.AddWithValue("@id_vehicle", (e.Item.FindControl("ButtonDeleteVehicle") as Button).CommandArgument);

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "delete_vehicle";
                }
            }

            if (myCommand.CommandText != string.Empty)
            {
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
                    Response.Redirect("backoffice.aspx", false);
                }
                catch (Exception ex)
                {
                    Session["Message"] = ex.Message;
                    Response.Redirect("error.aspx");
                }
            }
        }

        protected void RepeaterUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;

                (e.Item.FindControl("LabelIdUser") as Label).Text = dr.Row["id_user"].ToString();
                (e.Item.FindControl("TextBoxUsername") as TextBox).Text = dr.Row["username"].ToString();
                (e.Item.FindControl("TextBoxName") as TextBox).Text = dr.Row["name"].ToString();
                (e.Item.FindControl("TextBoxEmail") as TextBox).Text = dr.Row["email"].ToString();
                (e.Item.FindControl("DropDownListProfiles") as DropDownList).SelectedValue = dr.Row["id_profile"].ToString();
                (e.Item.FindControl("CheckBoxActivated") as CheckBox).Checked = Convert.ToBoolean(dr.Row["activated"]);

                (e.Item.FindControl("ButtonSaveUser") as Button).CommandArgument = dr.Row["id_user"].ToString();
                (e.Item.FindControl("ButtonDeleteUser") as Button).CommandArgument = dr.Row["id_user"].ToString();
            }
        }

        protected void RepeaterUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = myConn;

            if (e.CommandName.Equals("ButtonSaveUser"))
            {
                if (
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxUsername") as TextBox).Text) &&
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxName") as TextBox).Text) &&
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxEmail") as TextBox).Text) &&
                    (e.Item.FindControl("DropDownListProfiles") as DropDownList).SelectedValue != null
                    )
                {
                    myCommand.Parameters.AddWithValue("@id_user", (e.Item.FindControl("ButtonSaveUser") as Button).CommandArgument);
                    myCommand.Parameters.AddWithValue("@username", (e.Item.FindControl("TextBoxUsername") as TextBox).Text);
                    myCommand.Parameters.AddWithValue("@name", (e.Item.FindControl("TextBoxName") as TextBox).Text);
                    myCommand.Parameters.AddWithValue("@email", (e.Item.FindControl("TextBoxEmail") as TextBox).Text);
                    myCommand.Parameters.AddWithValue("@id_profile", (e.Item.FindControl("DropDownListProfiles") as DropDownList).SelectedItem.ToString());
                    myCommand.Parameters.AddWithValue("@activated", (e.Item.FindControl("CheckBoxActivated") as CheckBox).Checked.ToString());

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "save_user";
                }
            }

            if (e.CommandName.Equals("ButtonDeleteUser"))
            {
                if (!string.IsNullOrEmpty((e.Item.FindControl("LabelIdUser") as Label).Text))
                {
                    myCommand.Parameters.AddWithValue("@id_user", (e.Item.FindControl("ButtonDeleteUser") as Button).CommandArgument);

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "delete_user";
                }
            }

            if (myCommand.CommandText != string.Empty)
            {
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
                    Response.Redirect("backoffice.aspx", false);
                }
                catch (Exception ex)
                {
                    Session["Message"] = ex.Message;
                    Response.Redirect("error.aspx");
                }
            }
        }

        protected void RepeaterReservations_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;

                (e.Item.FindControl("LabelIdReservation") as Label).Text = dr.Row["id_reservation"].ToString();
                (e.Item.FindControl("DropDownReservationVehicles") as DropDownList).SelectedValue = dr.Row["id_vehicle"].ToString();
                (e.Item.FindControl("DropDownListReservationUsers") as DropDownList).SelectedValue = dr.Row["id_user"].ToString();
                (e.Item.FindControl("TextBoxReservationDate") as TextBox).Text = dr.Row["reservationdate"].ToString();

                (e.Item.FindControl("ButtonSaveReservation") as Button).CommandArgument = dr.Row["id_reservation"].ToString();
                (e.Item.FindControl("ButtonDeleteReservation") as Button).CommandArgument = dr.Row["id_reservation"].ToString();
            }
        }

        protected void RepeaterReservations_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = myConn;

            if (e.CommandName.Equals("ButtonSaveReservation"))
            {
                if (
                    (e.Item.FindControl("DropDownReservationVehicles") as DropDownList).SelectedValue != null &&
                    (e.Item.FindControl("DropDownListReservationUsers") as DropDownList).SelectedValue != null &&
                    !string.IsNullOrEmpty((e.Item.FindControl("TextBoxReservationDate") as TextBox).Text)
                    )
                {
                    if (DateTime.TryParse((e.Item.FindControl("TextBoxReservationDate") as TextBox).Text, out _))
                    {
                        myCommand.Parameters.AddWithValue("@id_reservation", (e.Item.FindControl("ButtonSaveReservation") as Button).CommandArgument);
                        myCommand.Parameters.AddWithValue("@id_vehicle", (e.Item.FindControl("DropDownReservationVehicles") as DropDownList).SelectedValue.ToString());
                        myCommand.Parameters.AddWithValue("@id_user", (e.Item.FindControl("DropDownListReservationUsers") as DropDownList).SelectedValue.ToString());
                        myCommand.Parameters.AddWithValue("@reservationdate", Convert.ToDateTime((e.Item.FindControl("TextBoxReservationDate") as TextBox).Text));
                        
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "save_reservation";
                    }
                }
            }

            if (e.CommandName.Equals("ButtonDeleteReservation"))
            {
                if (!string.IsNullOrEmpty((e.Item.FindControl("LabelIdReservation") as Label).Text))
                {
                    myCommand.Parameters.AddWithValue("@id_reservation", (e.Item.FindControl("ButtonDeleteReservation") as Button).CommandArgument);

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "delete_reservation";
                }
            }

            if (myCommand.CommandText != string.Empty)
            {
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
                    Response.Redirect("backoffice.aspx", false);
                }
                catch (Exception ex)
                {
                    Session["Message"] = ex.Message;
                    Response.Redirect("error.aspx");
                }
            }
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
