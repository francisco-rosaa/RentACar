<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="vehicles.aspx.cs" Inherits="RentACar.vehicles"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #vehiclesMenu {
            font-size: 0.85rem;
        }
        #vehiclesList {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            column-gap: 0.5rem;
            row-gap: 0.5rem;
        }
        .vehicleBox {
            display: flex;
            flex-direction: column;
            border: solid;
            border-width: 0.05rem;
            border-radius: 0.3rem;
            padding: 1rem;
            text-align: center;
            background-color: rgb(240, 240,240);
        }
        .boxCategory {
            font-size: 0.8rem;
            margin: 0 0 0.5rem 0;
        }
        .boxImage {
            width: auto;
            height: 6.25rem;
            margin: 0 auto 0.75rem auto;
        }
        .boxBrand {
            font-size: 1.2rem;
            font-weight: bold;
            color: rgb(137, 105, 43);
            margin: 0 0 0.25rem 0;
        }
        .boxModel {
            font-size: 1rem;
            font-weight: bold;
            color: rgb(96, 96, 96);
            margin: 0 0 0.5rem 0;
        }
        .boxDescription {
            font-size: 0.8rem;
            margin: 0 0 0.5rem 0;
        }
        .boxPrice, .boxPriceDiscounted {
            font-size: 1.25rem;
            color: rgb(96, 96, 96);
            margin: 0 0 0.5rem 0;
        }
        .dummySpace {
            flex-grow: 1;
        }
        .boxButton {
            border: solid;
            background-color: rgb(137, 105, 43);
            border-width: 0;
            border-radius: 0.3rem;
        }
        .boxButtonText {
            color: white;
            text-align: center;
            font-size: 0.9rem;
            margin: 0.6rem 0 0.6rem 0;
        }
        .aspButton {
            background-color: transparent;
            border: none;
            cursor: pointer;
        }
        .boxPriceSmallLine {
            font-size: 0.9rem;
            text-decoration-line: line-through;
            color: darkred;
        }
        .boxPriceOff {
            display: none;
        }
        .seeAvailabilityTrue {
            background-color: darkgreen;
        }
        .seeAvailabilityFalse {
            background-color: darkred;
        }
        @media (max-width: 1100px){
            #vehiclesList {
                grid-template-columns: repeat(3, 1fr);
            }
        }
        @media (max-width: 850px){
            #vehiclesList {
                grid-template-columns: repeat(2, 1fr);
            }
        }
        @media (max-width: 600px){
            #vehiclesList {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div id="vehiclesMenu">
        Category&nbsp;
        <asp:DropDownList ID="DropDownListCategories" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceCategories" DataTextField="id_category" DataValueField="id_category" AppendDataBoundItems="true" OnSelectedIndexChanged="DropDownListCategories_SelectedIndexChanged"></asp:DropDownList>
        <asp:SqlDataSource runat="server" ID="SqlDataSourceCategories" ConnectionString='<%$ ConnectionStrings:RentACarConnectionString %>' SelectCommand="SELECT [id_category] FROM [categories]"></asp:SqlDataSource>
        &nbsp;&nbsp;&nbsp;&nbsp; 
        Brand&nbsp;
        <asp:DropDownList ID="DropDownBrands" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceBrands" DataTextField="brand" DataValueField="brand" AppendDataBoundItems="true" OnSelectedIndexChanged="DropDownBrands_SelectedIndexChanged"></asp:DropDownList>
        <asp:SqlDataSource runat="server" ID="SqlDataSourceBrands" ConnectionString='<%$ ConnectionStrings:RentACarConnectionString %>' SelectCommand="SELECT DISTINCT [brand] FROM [vehicles]"></asp:SqlDataSource>
        &nbsp;&nbsp;&nbsp;&nbsp; 
        OrderBy&nbsp;
        <asp:DropDownList ID="DropDownListOrderBy" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListOrderBy_SelectedIndexChanged">
            <asp:ListItem></asp:ListItem>
            <asp:ListItem Value="categoryasc">Category Asc</asp:ListItem>
            <asp:ListItem Value="categorydesc">Category Desc</asp:ListItem>
            <asp:ListItem Value="priceasc">Price Asc</asp:ListItem>
            <asp:ListItem Value="pricedesc">Price Desc</asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp; 
        Availability
        <asp:CheckBox ID="CheckBoxAvailability" style="vertical-align:middle;" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBoxAvailability_CheckedChanged" />
    </div>
    <br />
    <div id="vehiclesList">
        <asp:Repeater ID="RepeaterVehiclesList" runat="server" OnItemCommand="RepeaterVehiclesList_ItemCommand">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="vehicleBox">
                    <p class="boxCategory">
                        <%# Eval("Category") %>
                    </p>
                    <img class="boxImage" src="<%# Eval("ImageVehicle") %>" />
                    <p class="boxBrand">
                        <%# Eval("Brand") %>
                    </p>
                    <p class="boxModel">
                        <%# Eval("Model") %>
                    </p>
                    <p class="boxDescription">
                        <%# Eval("Description") %>
                    </p>
                    <p class="boxPrice <%# Eval("Style.BoxPrice") %>">
                        <%# Eval("Price") %>€
                    </p>
                    <p class="boxPriceDiscounted <%# Eval("Style.BoxPriceDiscounted") %>">
                        <%# Eval("PriceDiscounted") %>€
                    </p>
                    <div class="dummySpace">
                    </div>
                    <div class="boxButton <%# Eval("Style.BoxButtonAvailable") %>" >
                        <asp:Button ID="ButtonReserve" class="boxButton boxButtonText aspButton" runat="server" Text="RESERVE" 
                            UseSubmitBehavior="False" CommandName="ButtonReserve" CommandArgument='<%# Eval("Id") %>' />
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
