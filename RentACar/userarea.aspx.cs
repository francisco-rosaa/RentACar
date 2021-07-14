using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentACar
{
    public partial class userarea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PresentShoppingCart();

            if (Session["SuccessLogin"] == null)
            {
                Response.Redirect("login.aspx");
            }

            PresentUserAreaInfo();
        }

        private void PresentUserAreaInfo()
        {
            if (
                Session["UserName"] != null &&
                Session["Name"] != null &&
                Session["Email"] != null &&
                Session["UserType"] != null
                )
            {
                LabelUsername.Text = Session["UserName"].ToString();
                LabelName.Text = Session["Name"].ToString();
                LabelEmail.Text = Session["Email"].ToString();
                LabelUserType.Text = Session["UserType"].ToString();
            }
        }

        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("logout.aspx");
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
