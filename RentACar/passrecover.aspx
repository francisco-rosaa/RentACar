<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="passrecover.aspx.cs" Inherits="RentACar.passrecover" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mainDiv {
            font-size: 0.85rem;
            width: 17rem;
            margin: auto;
            text-align: center;
        }
        .title {
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
        <div class="mainDiv">
        <br />
        <p class="title">
            Password Recovery
        </p>
        <p>
            Please enter a valid email.
        </p>
        <br />
        <p>
            Email&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxEmail" ErrorMessage="Enter email." ForeColor="#89692B">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxEmail" ErrorMessage="Invalid email." ForeColor="#89692B" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
        </p>
        <br />
        <p>
            <asp:Button ID="ButtonSend" runat="server" Text="Send" OnClick="ButtonSend_Click" />
        </p>
        <p>
            <a class="hyper" runat="server" href="~/login.aspx">Login</a>
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
