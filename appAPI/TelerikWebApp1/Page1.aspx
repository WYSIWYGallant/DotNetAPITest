<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="Page1.aspx.cs" Inherits="TelerikWebApp1.Page1" %>  
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">  
    <script src="https://unpkg.com/react@17/umd/react.development.js" crossorigin="anonymous"></script>  
    <script src="https://unpkg.com/react-dom@17/umd/react-dom.development.js" crossorigin="anonymous"></script>  
</asp:Content>  
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div id="reactGrid"></div>
        <script>
        </script>  
</asp:Content>
