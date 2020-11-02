<%@ Page Title="Potvrdenie registrácie" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Kronika106.Account.Confirm" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>

    <div>
        <%--<asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true">
            <p>
                Tvoj účet bol overený. Klikni <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login">sem</asp:HyperLink>  pre prihlásenie.             
            </p>
        </asp:PlaceHolder>--%>
        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
            <div class="alert alert-danger" runat="server" id="FailtureText">
                <asp:Literal runat="server" ID="FailtureTextContent" />
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="SuccesMessage" Visible="false">
            <div class="alert alert-success" runat="server" id="SuccesText">
                <p>
                     Tvoj účet bol overený. Klikni <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login"><b>SEM</b></asp:HyperLink>  pre prihlásenie. 
                </p>
            </div>
        </asp:PlaceHolder>

    </div>
</asp:Content>
