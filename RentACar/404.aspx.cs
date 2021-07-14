using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class _404 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            Response.AddHeader("REFRESH", "3;URL=vehicles.aspx");
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
