<%@ Page Title="Prihlásenie" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Kronika106.Account.Login" Async="true" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container">
        <div class="col-md-12">
            <a class="templatemo_logo">
                <h2><%: Title %>.</h2>
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <div class="alert alert-danger" runat="server" id="FailtureText">
                        <asp:Literal runat="server" ID="FailureText" />
                    </div>
                </asp:PlaceHolder>
            </a>
        </div>
    </div>


    <div class="form-horizontal">
        <div class="row">
            <h4>Prihlás sa účtom vytvoreným na tejto stránke</h4>
            <hr />
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" MaxLength="256" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="Email je povinný." />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Heslo</asp:Label>
                <div class="col-md-8">
                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" MaxLength="200">                                    
                    </asp:TextBox>
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled" CssClass="pull-left" ForeColor="#009933">Zabudol si heslo?</asp:HyperLink>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Heslo je povinné." />

                </div>
            </div>
            <div class="form-group">
                <div class="col-md-8 col-md-offset-2">
                    <asp:Button runat="server" OnClick="LogIn" Text="Prihlásiť" CssClass="btn btn-default" />
                    <asp:Button runat="server" ID="ResendConfirm" OnClick="SendEmailConfirmationToken" Text="Preposlať potvrdzovací mail" Visible="false" CssClass="btn btn-default" />
                </div>
            </div>
            <div class="col-md-8 col-md-offset-2">
                <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled" CssClass="font" ForeColor="#009933">Vytvorenie nového účtu</asp:HyperLink>
            </div>
            <div class="form-group">
                <div class="col-md-8 col-md-offset-2">                    
                        <asp:CheckBox runat="server" ID="RememberMe" CssClass="checkbox checkbox-inline" Text="Neodhlasovať" Checked="true"/>                    
                </div>
            </div>
        </div>
    </div>                                   
</asp:Content>
