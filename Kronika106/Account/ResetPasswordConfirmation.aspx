<%@ Page Title="Heslo zmenené" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPasswordConfirmation.aspx.cs" Inherits="Kronika106.Account.ResetPasswordConfirmation" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    
         <asp:PlaceHolder runat="server" ID="SuccesMessage" Visible="true">
            <div class="alert alert-success" runat="server" id="SuccesText">
                <p>
                     Tvoje heslo bolo zmenené. Klikni <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login"><b>SEM</b></asp:HyperLink> pre prihlásenie
                </p>
            </div>
        </asp:PlaceHolder>
</asp:Content>
