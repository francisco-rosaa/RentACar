<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="userarea.aspx.cs" Inherits="RentACar.userarea" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .loginDiv {
            font-size: 0.85rem;
            width: 17rem;
            margin: auto;
            text-align: center;
        }
        .loginTitle {
            font-size: 1.5rem;
            font-weight: bold;
            color: rgb(137, 105, 43);
        }
        .contentDiv {
            text-align: left;
        }
        .userName {
            font-weight: bold;
            color: rgb(137, 105, 43);
        }
        .userInfo {
            font-weight: bold;
            color: rgb(96, 96, 96);
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
    <div class="loginDiv">
        <br />
        <p class="loginTitle">
            User Area
        </p>
        <p>
            Welcome <asp:Label ID="LabelUsername" CssClass="userName" runat="server"></asp:Label>
        </p>
        <br />
        <div class="contentDiv">
            <p>
                Name:&nbsp;&nbsp;&nbsp;<asp:Label ID="LabelName" CssClass="userInfo" runat="server"></asp:Label>
            </p>
            <p>
                Email:&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="LabelEmail" CssClass="userInfo" runat="server"></asp:Label>
            </p>
            <p>
                Profile:&nbsp;&nbsp;&nbsp;<asp:Label ID="LabelUserType" CssClass="userInfo" runat="server"></asp:Label>
            </p>
            <br />
        </div>
        <p>
            <asp:Button ID="ButtonLogout" runat="server" Text="Logout" OnClick="ButtonLogout_Click" />
        </p>
        <p>
            <a class="hyper" runat="server" href="~/passchange.aspx">Password Change</a>
        </p>
        <br />
    </div>
</asp:Content>
