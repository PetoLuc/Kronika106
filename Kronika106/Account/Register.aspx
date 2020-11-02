<%@ Page Title="Registrácia" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Kronika106.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
        <div class="alert alert-danger" runat="server" id="FailtureText">
            <asp:Literal runat="server" ID="FailtureTextContent" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="InfoMessage" Visible="false">
        <div class="alert alert-info" runat="server" id="InfoText">
            <asp:Literal runat="server" ID="InfoTextContent" />
        </div>
    </asp:PlaceHolder>

    <div class="form-horizontal">
        <h4>Vytvor si svoj nový účet</h4>
        <hr />
        <div class="form-group" id="grpEmail" runat="server">
            <asp:Label runat="server" AssociatedControlID="email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="email" CssClass="form-control" MaxLength="256" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="email"
                    Display="Dynamic" CssClass="text-danger" ErrorMessage="Email je povinný" />
                <asp:ModelErrorMessage runat="server" ModelStateKey="email" CssClass="text-error" />
            </div>
        </div>

        <div class="form-group" id="grpName" runat="server">
            <asp:Label runat="server" AssociatedControlID="firstame" CssClass="col-md-2 control-label">Meno</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="firstame" CssClass="form-control" MaxLength="50" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="firstame"
                    Display="Dynamic" CssClass="text-danger" ErrorMessage="Meno je povinné" />
                <asp:ModelErrorMessage runat="server" ModelStateKey="firstame" CssClass="text-error" />
            </div>
        </div>

        <div class="form-group" id="grplastname" runat="server">
            <asp:Label runat="server" AssociatedControlID="lastname" CssClass="col-md-2 control-label">Priezvisko</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="lastname" CssClass="form-control" MaxLength="50" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="lastname"
                    Display="Dynamic" CssClass="text-danger" ErrorMessage="Priezvisko je povinné" />
                <asp:ModelErrorMessage runat="server" ModelStateKey="lastname" CssClass="text-error" />
            </div>
        </div>

        <div class="form-group" id="grpNick" runat="server" visible="true">
            <asp:Label runat="server" AssociatedControlID="nick" CssClass="col-md-2 control-label">Ak si skaut, vyplň svoju prezývku</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="nick" CssClass="form-control" MaxLength="50" TextMode="SingleLine" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Heslo</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" MaxLength="200" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="Heslo je povinné." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Potvrdenie hesla</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" MaxLength="200" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Potvrdenie hesla je povinné." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Hesla sa nezhodujú" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="chSendMail" ID="labelSendMail" CssClass="col-md-2 control-label">Informovať o novinkách</asp:Label>
            <div class="col-md-1">
                <asp:CheckBox runat="server" ID="chSendMail" Checked="true" CssClass="checkbox"/>
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-2 col-md-8">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Registruj" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
