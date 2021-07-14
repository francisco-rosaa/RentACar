<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="shoppingcart.aspx.cs" Inherits="RentACar.shoppingcart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #shoppingCartContent {
            width: 32rem;
            margin: auto;
        }
       .shoppingCartTitle {
            font-size: 1.5rem;
            font-weight: bold;
            color: rgb(115, 179, 47);
            text-align: center;
        }
        .shoppingCartContent {
            font-size: 0.8rem;
            font-weight: bold;
            color: rgb(96, 96, 96);
        }
        .shoppingCartBox {
            display: flex;
            align-items: baseline;
        }
        .dummySpace {
            flex-grow: 1;
        }
        .total {
            font-size: 1rem;
        }
        .shoppingCartFinalize {
            text-align: center;
        }
        .hyper, .message {
            font-size: 0.85rem;
        }
        .hyper:link, .hyper:visited {
            color: rgb(0, 0, 0);
        }
        .hyper:hover {
            color: rgb(137, 105, 43);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="shoppingCartContent">
        <br />
        <p class="shoppingCartTitle">
            Shopping Cart 
        </p>
        <hr>
        <br />
        <div class="shoppingCartContent">
            <asp:Repeater ID="RepeaterShoppingCart" runat="server" OnItemCommand="RepeaterShoppingCart_ItemCommand">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="shoppingCartBox">
                        <p>
                            <div>
                                <%# Eval("Category") %>&nbsp;&nbsp;|&nbsp;&nbsp;
                                <%# Eval("Brand") %>&nbsp;&nbsp;
                                <%# Eval("Model") %>
                            </div>
                            <div class="dummySpace"></div>
                            <div>
                                <%# Eval("Price") %>€&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="CartDelete" runat="server" Text="Delete" UseSubmitBehavior="False" 
                                    CommandName="CartDelete" CommandArgument='<%# Eval("Id") %>' />
                            </div>
                        </p>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <br />
        <div class="shoppingCartContent shoppingCartBox">
            <div>
                <asp:Label ID="LabelDateToday" runat="server" Text=""></asp:Label>
            </div>
            <div class="dummySpace"></div>
            <div>
                <asp:Label ID="LabelTotalCost" CssClass="total" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <br />
        <div class="shoppingCartFinalize">
            <p>
                <asp:Button ID="ButtonFinalize" runat="server" Text="FINALIZE" OnClick="ButtonFinalize_Click" />
            </p>
            <p>
                <a class="hyper" runat="server" href="~/userarea.aspx">User Area</a>
            </p>
            <p>
                <asp:Label ID="LabelMessage" CssClass="message" runat="server" ForeColor="#89692B"></asp:Label>
            </p>
            <br />
        </div>
    </div>
</asp:Content>
