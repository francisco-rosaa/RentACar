<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="passchange.aspx.cs" Inherits="RentACar.passchange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mainDiv {
            font-size: 0.85rem;
            width: 18rem;
            margin: auto;
            text-align: center;
        }
        .title {
            font-size: 1.5rem;
            font-weight: bold;
            color: rgb(137, 105, 43);
        }
        .contentDiv {
            text-align: left;
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
            Password Change
        </p>
        <br />
        <div class="contentDiv">
            <p>
                New Password&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxNewPass" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxNewPass" ErrorMessage="Enter new password." ForeColor="#89692B">*</asp:RequiredFieldValidator>
            </p>
            <p>
                Confirm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxConfirm" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxConfirm" ErrorMessage="Confirm new password." ForeColor="#89692B">*</asp:RequiredFieldValidator>
            </p>
        </div>
        <br />
        <p>
            <asp:Button ID="ButtonConfirm" runat="server" Text="Confirm" OnClick="ButtonConfirm_Click" />
        </p>
        <p>
            <a class="hyper" runat="server" href="~/userarea.aspx">Back</a>
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
