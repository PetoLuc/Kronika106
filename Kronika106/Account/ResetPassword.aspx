<%@ Page Title="Obnova hesla" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Kronika106.Account.ResetPassword" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
        <div class="alert alert-danger" runat="server" id="FailtureText">
            <asp:Literal runat="server" ID="FailureTextContent" />
        </div>
    </asp:PlaceHolder>

    <div class="form-horizontal">
        <h4>Zadaj nové heslo</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" MaxLength ="256" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                    CssClass="text-danger" ErrorMessage="Email je povinný." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PasswordToReset" CssClass="col-md-2 control-label">Heslo</asp:Label>
            <div class="col-md-8"> 
                <asp:TextBox runat="server" ID="PasswordToReset" TextMode="Password" CssClass="form-control" MaxLength="200" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PasswordToReset"
                    CssClass="text-danger" ErrorMessage="Heslo je povinné." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPasswordToReset" CssClass="col-md-2 control-label">Potvrdenie hesla</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="ConfirmPasswordToReset" TextMode="Password" CssClass="form-control"  MaxLength="200"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPasswordToReset"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Potvrdenie hesla je povinné." />
                <asp:CompareValidator runat="server" ControlToCompare="PasswordToReset" ControlToValidate="ConfirmPasswordToReset"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Heslo a potvdenie hesla sa nezhodujú" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-8 col-md-offset-2">
                <asp:Button runat="server" OnClick="Reset_Click" Text="Obnov heslo" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
