<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="backoffice.aspx.cs" Inherits="RentACar.backoffice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
       .backOfficeTitle {
            font-size: 1.5rem;
            font-weight: bold;
            color: rgb(137, 105, 43);
        }
        .backOfficeSubTitle {
            font-size: 0.8rem;
            font-weight: bold;
            color: rgb(96, 96, 96);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <p class="backOfficeTitle">
        Vehicles
    </p>
    <hr>
    <asp:Repeater ID="RepeaterVehicles" runat="server" DataSourceID="SqlDataSourceVehicles" OnItemDataBound="RepeaterVehicles_ItemDataBound" OnItemCommand="RepeaterVehicles_ItemCommand">
        <HeaderTemplate>
            <table>
                <tr class="backOfficeSubTitle">
                    <td>Id</td>
                    <td>Category</td>
                    <td>Brand</td>
                    <td>Model</td>
                    <td>Description</td>
                    <td>Price</td>
                    <td>Avl.</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="LabelIdVehicle" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListCategories" runat="server" DataSourceID="SqlDataSourceCategories" DataTextField="id_category" DataValueField="id_category"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSourceCategories" ConnectionString='<%$ ConnectionStrings:RentACarConnectionString %>' SelectCommand="SELECT [id_category] FROM [categories]"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxBrand" style="width:5rem;" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxModel" style="width:8rem;" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxDescription" style="width:10rem;" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxPrice" style="width:3rem; text-align:right;" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBoxAvailable" runat="server" />  
                    </td>
                    <td>
                        <asp:Button ID="ButtonSaveVehicle" runat="server" Text="Save" CommandName="ButtonSaveVehicle" />
                    </td>
                    <td>
                        <asp:Button ID="ButtonDeleteVehicle" runat="server" Text="Delete" CommandName="ButtonDeleteVehicle" />
                    </td>
                    <td>
                        <asp:Image ID="ImageVehicle" style="width:6rem; height:4rem; height:auto;" runat="server" />
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUploadImage" runat="server" />
                        <asp:Button ID="ButtonUploadImage" runat="server" Text="Upload" CommandName="ButtonUploadImage" />
                        <asp:Button ID="ButtonDeleteImage" runat="server" Text="Delete" CommandName="ButtonDeleteImage" />
                    </td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <br />
    <p class="backOfficeTitle">
        Users
    </p>
    <hr>
    <asp:Repeater ID="RepeaterUsers" runat="server" DataSourceID="SqlDataSourceUsers" OnItemDataBound="RepeaterUsers_ItemDataBound" OnItemCommand="RepeaterUsers_ItemCommand">
        <HeaderTemplate>
            <table>
                <tr class="backOfficeSubTitle">
                    <td>Id</td>
                    <td>Username</td>
                    <td>Name</td>
                    <td>Email</td>
                    <td>Profile</td>
                    <td>Act.</td>
                    <td></td>
                    <td></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="LabelIdUser" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxUsername" style="width:5rem;"  runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListProfiles" runat="server" DataSourceID="SqlDataSourceProfiles" DataTextField="id_profile" DataValueField="id_profile"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSourceProfiles" ConnectionString='<%$ ConnectionStrings:RentACarConnectionString %>' SelectCommand="SELECT [id_profile] FROM [profiles]"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBoxActivated" runat="server" />  
                    </td>
                    <td>
                        <asp:Button ID="ButtonSaveUser" runat="server" Text="Save" CommandName="ButtonSaveUser" />
                    </td>
                    <td>
                        <asp:Button ID="ButtonDeleteUser" runat="server" Text="Delete" CommandName="ButtonDeleteUser" />
                    </td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <br />
    <br />
    <p class="backOfficeTitle">
        Reservations
    </p>
    <hr>
    <asp:Repeater ID="RepeaterReservations" runat="server" DataSourceID="SqlDataSourceReservations" OnItemDataBound="RepeaterReservations_ItemDataBound" OnItemCommand="RepeaterReservations_ItemCommand">
        <HeaderTemplate>
            <table>
                <tr class="backOfficeSubTitle">
                    <td>Id</td>
                    <td>Vehicle</td>
                    <td>User</td>
                    <td>Date</td>
                    <td></td>
                    <td></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="LabelIdReservation" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownReservationVehicles" runat="server" DataSourceID="SqlDataSourceReservationVehicles" DataTextField="id_brand_model" DataValueField="id_vehicle"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSourceReservationVehicles" ConnectionString='<%$ ConnectionStrings:RentACarConnectionString %>' SelectCommand="SELECT [id_vehicle], CONCAT(id_vehicle, ' | ', brand, ' | ', model) as id_brand_model FROM [vehicles]"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListReservationUsers" runat="server" DataSourceID="SqlDataSourceReservationUsers" DataTextField="id_name" DataValueField="id_user"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSourceReservationUsers" ConnectionString='<%$ ConnectionStrings:RentACarConnectionString %>' SelectCommand="SELECT [id_user], CONCAT(id_user, ' | ', name) as id_name FROM [users]"></asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxReservationDate" style="width:8rem;" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="ButtonSaveReservation" runat="server" Text="Save" CommandName="ButtonSaveReservation" />
                    </td>
                    <td>
                        <asp:Button ID="ButtonDeleteReservation" runat="server" Text="Delete" CommandName="ButtonDeleteReservation" />
                    </td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSourceVehicles" runat="server" ConnectionString="<%$ ConnectionStrings:RentACarConnectionString %>" SelectCommand="SELECT * FROM [vehicles]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceUsers" runat="server" ConnectionString="<%$ ConnectionStrings:RentACarConnectionString %>" SelectCommand="SELECT * FROM [users]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceReservations" runat="server" ConnectionString="<%$ ConnectionStrings:RentACarConnectionString %>" SelectCommand="SELECT * FROM [reservations]"></asp:SqlDataSource>
</asp:Content>
