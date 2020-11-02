<%@ Page Title="Správa účtu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="Kronika106.Account.Manage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <div>
        <asp:PlaceHolder runat="server" ID="SuccesMessage" Visible="false">
            <div class="alert alert-success" runat="server" id="SuccesText">
                <p>
                    <%: SuccessMessage %>
                </p>
            </div>
        </asp:PlaceHolder>
    </div>


    <div class="form-horizontal">
        <hr />

        <%--<p> <%: LoginsCount>0 ? "Pre prihlasovanie používaš Facebook": null %> </p>--%>
        <%--    <asp:HyperLink NavigateUrl="/Account/ManageLogins" Text="[Spravuj]" runat="server" />--%>
        <div class="form-group" id="grpEmail" runat="server">
            <asp:Label runat="server" AssociatedControlID="email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="email" CssClass="form-control" TextMode="Email" Enabled="false" />
            </div>
        </div>

        <div class="form-group" id="grpName" runat="server">
            <asp:Label runat="server" AssociatedControlID="firstame" CssClass="col-md-2 control-label">Meno</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="firstame" CssClass="form-control" TextMode="SingleLine" Enabled="false" />
            </div>
        </div>

        <div class="form-group" id="grplastname" runat="server">
            <asp:Label runat="server" AssociatedControlID="lastname" CssClass="col-md-2 control-label">Priezvisko</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="lastname" CssClass="form-control" TextMode="SingleLine" Enabled="false" />
            </div>
        </div>

        <div class="form-group" id="grpNick" runat="server" visible="true">
            <asp:Label runat="server" AssociatedControlID="nick" CssClass="col-md-2 control-label">Skautská prezývka</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="nick" CssClass="form-control" TextMode="SingleLine" Enabled="false" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="chSendMail" ID="labelSendMail" CssClass="col-md-2 control-label">Informovať o novinkách</asp:Label>
            <div class="col-md-1">
                <asp:CheckBox runat="server" ID="chSendMail" CssClass="checkbox"/>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-2"></div>
            <div class="col-md-1">
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click"  Text="Uložiť zmeny" CssClass="btn btn-sm" />
            </div>
        </div>


        <div class="form-group row">
            <asp:Label runat="server" AssociatedControlID="ChangePassword" ID="label1" CssClass="col-md-2 control-label">Heslo</asp:Label>
            <div class="col-md-1">
                <asp:HyperLink NavigateUrl="~/Account/ManagePassword" Text="[Zmeň]" Visible="false" ID="ChangePassword" runat="server" CssClass="col-md-2 control-label" />
            </div>
        </div>
    </div>
</asp:Content>
