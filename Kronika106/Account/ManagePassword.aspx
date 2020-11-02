<%@ Page Title="Správa hesla" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagePassword.aspx.cs" Inherits="Kronika106.Account.ManagePassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <section id="passwordForm">
            <asp:PlaceHolder runat="server" ID="setPassword" Visible="false">
                <p>
                    Nemáš lokálne heslo pre túto stránku. Ak si pridáš lokálne heslo, budeš sa vedieť príhlásiť na stránku aj bez Facebook účtu.                    
                </p>              
                        <div class="form-horizontal">
                            <h3>Nastaviť nové heslo</h3>
                            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                            <hr />

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="password" CssClass="col-md-2 control-label">Heslo</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="password" TextMode="Password" CssClass="form-control"  MaxLength="200"/>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="password"
                                        CssClass="text-danger" ErrorMessage="The password field is required."
                                        Display="Dynamic" ValidationGroup="SetPassword" />
                                    <asp:ModelErrorMessage runat="server" ModelStateKey="NewPassword" AssociatedControlID="password"
                                        CssClass="text-danger" SetFocusOnError="true" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="confirmPassword" CssClass="col-md-2 control-label">Potvrenie hesla</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="confirmPassword" TextMode="Password" CssClass="form-control" MaxLength="200" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="confirmPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required."
                                        ValidationGroup="SetPassword" />
                                    <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="confirmPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match."
                                        ValidationGroup="SetPassword" />

                                </div>
                            </div>
                            <div class="form-group">
                                <div class="ccol-md-8 col-md-offset-2">
                                    <asp:Button runat="server" Text="Nastav heslo" ValidationGroup="SetPassword" OnClick="SetPassword_Click" CssClass="btn btn-default" />
                                </div>
                            </div>
                        </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder runat="server" ID="changePasswordHolder" Visible="false">      
                        <div class="form-horizontal">
                            <h3>Zmeniť heslo</h3>
                            <hr />
                            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                            <div class="form-group">
                                <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword" CssClass="col-md-2 control-label">Aktuálne heslo</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" MaxLength="200" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                                        CssClass="text-danger" ErrorMessage="Aktuálne helo je povinné."
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword" CssClass="col-md-2 control-label">Nové heslo</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" MaxLength="200"/>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                                        CssClass="text-danger" ErrorMessage="Nové heslo je povinné."
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Potvrdenie nového hesla</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" MaxLength="200" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Potvrdenie nového hesla je povinné."
                                        ValidationGroup="ChangePassword" />
                                    <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Nové heslo a jeho potvrdenie sa nezhodujú."
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-8 col-md-offset-2">
                                    <asp:Button runat="server" Text="Zmeniť heslo" ValidationGroup="ChangePassword" OnClick="ChangePassword_Click" CssClass="btn btn-default" />
                                </div>
                            </div>
                        </div>                  
            </asp:PlaceHolder>
        </section>
    </div>
</asp:Content>
