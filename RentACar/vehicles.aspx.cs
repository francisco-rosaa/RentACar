using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RentACar.Entities;

namespace RentACar
{
    public partial class vehicles : System.Web.UI.Page
    {
        private List<Vehicle> vehiclesList;
        private CssStyle cssStyle;

        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            vehiclesList = new List<Vehicle>();
            cssStyle = new CssStyle();

            if (Session["UserType"] == null)
            {
                Session["UserType"] = "normal";
            }

            if (Session["ReservesList"] == null)
            {
                Session["ReservesList"] = new List<Cart>();
            }

            if (!IsPostBack)
            {
                DropDownListCategories.Items.Add(new ListItem("All", "All"));
                DropDownBrands.Items.Add(new ListItem("All", "All"));
            }

            if (vehiclesList.Count == 0)
            {
                vehiclesList = GetAllVehicles();
            }

            PreparePresentation();

            RepeaterVehiclesList.DataSource = vehiclesList;
            RepeaterVehiclesList.DataBind();
        }

        private void PreparePresentation()
        {
            if (Session["UserType"].ToString() != "regular")
            {
                foreach (Vehicle vehicle in vehiclesList)
                {
                    vehicle.Style.BoxPriceDiscounted = "boxPriceOff";
                }
            }

            if (Session["UserType"].ToString() == "regular")
            {
                foreach (Vehicle vehicle in vehiclesList)
                {
                    vehicle.Style.BoxPrice = "boxPriceSmallLine";
                    vehicle.Style.BoxPriceDiscounted = string.Empty;
                }
            }

            if (CheckBoxAvailability.Checked)
            {
                ShowAvailability();
            }
        }

        private List<Vehicle> GetAllVehicles()
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RentACarConnectionString"].ConnectionString);

            SqlCommand myCommand = new SqlCommand("get_all_vehicles", myConn);
            myCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                myConn.Open();
                SqlDataReader dataReader = myCommand.ExecuteReader();

                List<Vehicle> vehicles = new List<Vehicle>();

                while (dataReader.Read())
                {
                    Vehicle vehicle = new Vehicle
                    {
                        Id = Convert.ToInt32(dataReader["id_vehicle"]),
                        Category = dataReader["id_category"].ToString(),
                        Brand = dataReader["brand"].ToString(),
                        Model = dataReader["model"].ToString(),
                        Description = dataReader["description"].ToString(),
                        Price = Convert.ToDecimal(dataReader["price"]),
                        PriceDiscounted = Convert.ToDecimal(dataReader["pricediscounted"]),
                        IsAvailable = Convert.ToBoolean(dataReader["available"]),
                        ImageVehicle = $"data:{dataReader["imagetype"]};base64,{Convert.ToBase64String(dataReader["image"] as byte[])}",
                        Style = cssStyle
                    };

                    vehicles.Add(vehicle);
                }

                dataReader.Close();
                myConn.Close();

                return vehicles;
            }
            catch (Exception ex)
            {
                Session["Message"] = ex.Message;
                Response.Redirect("error.aspx");
            }

            return null;
        }

        protected void DropDownListCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownBrands.SelectedValue = "All";
            DropDownListOrderBy.SelectedValue = string.Empty;

            List<Vehicle> tempVehiclesList = new List<Vehicle>();

            if (DropDownListCategories.SelectedValue != null)
            {
                foreach (Vehicle vehicle in vehiclesList)
                {
                    if (vehicle.Category == DropDownListCategories.SelectedValue.ToString())
                    {
                        tempVehiclesList.Add(vehicle);
                    }
                }
            }

            if (DropDownListCategories.SelectedValue.ToString() == "All")
            {
                tempVehiclesList = vehiclesList;
            }

            RepeaterVehiclesList.DataSource = tempVehiclesList;
            RepeaterVehiclesList.DataBind();
        }

        protected void DropDownBrands_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListCategories.SelectedValue = "All";
            DropDownListOrderBy.SelectedValue = string.Empty;

            List<Vehicle> tempVehiclesList = new List<Vehicle>();

            if (DropDownBrands.SelectedValue != null)
            {
                foreach (Vehicle vehicle in vehiclesList)
                {
                    if (vehicle.Brand == DropDownBrands.SelectedValue.ToString())
                    {
                        tempVehiclesList.Add(vehicle);
                    }
                }
            }

            if (DropDownBrands.SelectedValue.ToString() == "All")
            {
                tempVehiclesList = vehiclesList;
            }

            RepeaterVehiclesList.DataSource = tempVehiclesList;
            RepeaterVehiclesList.DataBind();
        }

        protected void DropDownListOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListCategories.SelectedValue = "All";
            DropDownBrands.SelectedValue = "All";

            if (DropDownListOrderBy.SelectedValue != null)
            {
                if (DropDownListOrderBy.SelectedValue == "categoryasc")
                {
                    vehiclesList = vehiclesList.OrderBy(x => x.Category).ToList();
                }

                if (DropDownListOrderBy.SelectedValue == "categorydesc")
                {
                    vehiclesList = vehiclesList.OrderByDescending(x => x.Category).ToList();
                }

                if (DropDownListOrderBy.SelectedValue == "priceasc")
                {
                    vehiclesList = vehiclesList.OrderBy(x => x.Price).ToList();
                }

                if (DropDownListOrderBy.SelectedValue == "pricedesc")
                {
                    vehiclesList = vehiclesList.OrderByDescending(x => x.Price).ToList();
                }
            }

            RepeaterVehiclesList.DataSource = vehiclesList;
            RepeaterVehiclesList.DataBind();
        }

        protected void CheckBoxAvailability_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxAvailability.Checked)
            {
                DropDownListCategories.SelectedValue = "All";
                DropDownBrands.SelectedValue = "All";
                DropDownListOrderBy.SelectedValue = string.Empty;
            }
            else
            {
                DropDownListCategories.SelectedValue = "All";
                DropDownBrands.SelectedValue = "All";
                DropDownListOrderBy.SelectedValue = string.Empty;
            }
        }

        private void ShowAvailability()
        {
            foreach (Vehicle vehicle in vehiclesList)
            {
                CssStyle cssStyle = new CssStyle();

                if (vehicle.IsAvailable)
                {
                    cssStyle.BoxButtonAvailable = "seeAvailabilityTrue";
                }
                else
                {
                    cssStyle.BoxButtonAvailable = "seeAvailabilityFalse";
                }

                cssStyle.BoxPrice = vehicle.Style.BoxPrice;
                cssStyle.BoxPriceDiscounted = vehicle.Style.BoxPriceDiscounted;
                vehicle.Style = cssStyle;
            }
        }

        protected void RepeaterVehiclesList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("ButtonReserve"))
            {
                RegisterReservationCart(Convert.ToInt32(e.CommandArgument));

                PresentShoppingCart();
            }
        }

        private void RegisterReservationCart(int id)
        {
            Cart cartTemp = new Cart();

            foreach (Vehicle vehicle in vehiclesList)
            {
                if (vehicle.Id == id)
                {
                    cartTemp = new Cart
                    {
                        Id = vehicle.Id,
                        Category = vehicle.Category,
                        Brand = vehicle.Brand,
                        Model = vehicle.Model,
                        Price = vehicle.Price,
                        PriceDiscounted = vehicle.PriceDiscounted
                    };

                    List<Cart> reservesCart = Session["ReservesList"] as List<Cart>;
                    reservesCart.Add(cartTemp);
                    Session["ReservesList"] = reservesCart;

                    Session["TotalReserves"] = Convert.ToInt32(Session["TotalReserves"]) + 1;
                    Session["TotalCost"] = Convert.ToDecimal(Session["TotalCost"]) + cartTemp.Price;
                    Session["TotalCostDiscounted"] = Convert.ToDecimal(Session["TotalCostDiscounted"]) + cartTemp.PriceDiscounted;
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
