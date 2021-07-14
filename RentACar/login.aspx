<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RentACar.login" %>
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
            Login
        </p>
        <p>
            Please enter a valid username and password.
        </p>
        <br />
        <p>
            Username&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TextBoxUser" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxUser" ErrorMessage="Enter username." ForeColor="#89692B">*</asp:RequiredFieldValidator>
        </p>
        <p>
            Password&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxPassword" ErrorMessage="Enter password." ForeColor="#89692B">*</asp:RequiredFieldValidator>
        </p>
        <br />
        <p>
            <asp:Button ID="ButtonLogin" runat="server" Text="Login" OnClick="ButtonLogin_Click" />
        </p>
        <p>
            <a class="hyper" runat="server" href="~/register.aspx">Register</a>
        </p>
        <p>
            <a class="hyper" runat="server" href="~/passrecover.aspx">Forgot Password</a>
        </p>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="#89692B" />
        <p>
        </p>
        <p>
            <asp:Label ID="LabelMessage" runat="server" ForeColor="#89692B"></asp:Label>
        </p>
        <br />
    </div>
</asp:Content>
