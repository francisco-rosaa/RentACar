<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="RentACar._404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style>
        .mainDiv {
            font-size: 0.85rem;
            text-align: center;
        }
        .title {
            font-size: 1.5rem;
            font-weight: bold;
            color: rgb(137, 105, 43);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainDiv">
        <br />
        <p class="title">
            404
        </p>
        <p>
            Page not found.
        </p>
        <br />
    </div>
</asp:Content>
