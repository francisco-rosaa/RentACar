<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="RentACar.register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .registerDiv {
            font-size: 0.85rem;
            width: 18rem;
            margin: auto;
            text-align: center;
        }
        .registerTitle {
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
    <div class="registerDiv">
        <br />
        <p class="registerTitle">
            Register
        </p>
        <p>
            Please fill out the registration form.
        </p>
        <br />
        <div class="contentDiv">
            <p>
                Username&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxUser" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxUser" ErrorMessage="Enter username." ForeColor="#89692B">*</asp:RequiredFieldValidator>
            </p>
            <p>
                Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxName" ErrorMessage="Enter name." ForeColor="#89692B">*</asp:RequiredFieldValidator>
            </p>
            <p>
                Email&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxEmail" ErrorMessage="Enter email." ForeColor="#89692B">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxEmail" ErrorMessage="Invalid email." ForeColor="#89692B" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
            </p>
            <p>
                Profile&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="DropDownListProfiles" runat="server" DataSourceID="SqlDataSourceProfiles" DataTextField="id_profile" DataValueField="id_profile"></asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSourceProfiles" ConnectionString='<%$ ConnectionStrings:RentACarConnectionString %>' SelectCommand="SELECT [id_profile] FROM [profiles]"></asp:SqlDataSource>
            </p>
            <br />
            <p>
                Password&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBoxPassword" ErrorMessage="Enter password." ForeColor="#89692B">*</asp:RequiredFieldValidator>
            </p>
            <p>
                Confirm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxConfirm" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TextBoxConfirm" ErrorMessage="Confirm password." ForeColor="#89692B">*</asp:RequiredFieldValidator>
            </p>
        </div>
        <br />
        <p>
            <asp:Button ID="ButtonRegister" runat="server" Text="Register" OnClick="ButtonRegister_Click" />
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
