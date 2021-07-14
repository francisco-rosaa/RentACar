using System;

namespace RentACar
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["IdUser"] = null;
            Session["UserName"] = null;
            Session["Name"] = null;
            Session["Email"] = null;
            Session["UserType"] = "normal";
            Session["SuccessLogin"] = null;
            Session["Message"] = null;

            Response.Redirect("vehicles.aspx");
        }
    }
}
