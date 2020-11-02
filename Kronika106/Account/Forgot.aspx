<%@ Page Title="Zabudnuté heslo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forgot.aspx.cs" Inherits="Kronika106.Account.ForgotPassword" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
        <div class="alert alert-danger" runat="server" id="FailtureText">
            <asp:Literal runat="server" ID="FailtureTextContent" />
        </div>
    </asp:PlaceHolder>
    <div class="form-horizontal">
        <asp:PlaceHolder ID="loginForm" runat="server">
            <h4>Pre obnovu hesla je potrebné zadať prihlasovací email.</h4>
            <hr />
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" MaxLength ="256" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                        CssClass="text-danger" ErrorMessage="The email field is required." />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-8 col-md-offset-2">
                    <asp:Button runat="server" OnClick="Forgot" Text="Poslať do emailu link pre obnovu hesla" CssClass="btn btn-default " />
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="DisplayEmail" Visible="false">
            <div class="alert alert-info" runat="server" id="InfoText">
                <asp:Literal runat="server" ID="InfoTextContent" Text="Všetko potrebné na zmenu hesla bolo odoslané na Tvoj email." />
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
