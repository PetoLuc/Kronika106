<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminMaster.aspx.cs" Inherits="Kronika106.Admin.AdminMaster" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Admin konzola</h2>
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
        <h4>Odosielanie hromadných mailov (užívateľom, ktorí odoberajú novinky)</h4>
        <hr />
        <div class="form-group row" id="grpSubject" runat="server">
            <asp:Label runat="server" AssociatedControlID="subject" CssClass="col-md-2 control-label">Predmet</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="subject" CssClass="form-control" MaxLength="300" TextMode="singleline" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="subject"
                    Display="Dynamic" CssClass="text-danger" ErrorMessage="Subjekt je povinný" />
                <asp:ModelErrorMessage runat="server" ModelStateKey="subject" CssClass="text-error" />
            </div>
        </div>

        <div class="form-group row" id="grpBody" runat="server">
            <asp:Label runat="server" AssociatedControlID="body" CssClass="col-md-2 control-label">Obsah</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="body" CssClass="form-control" MaxLength="10000" TextMode="MultiLine" Rows="4" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="body"
                    Display="Dynamic" CssClass="text-danger" ErrorMessage="Telo emailu povinné" />
                <asp:ModelErrorMessage runat="server" ModelStateKey="body" CssClass="text-error" />
            </div>
        </div>                            
        <div class="form-group">
            <div class="row">
                <div class="col-md-offset-2 col-md-8">
                    <asp:CheckBox runat ="server" ID="chSendToAll" Text="Odošli všetkým (aj tým čo nechcú maily)" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-offset-2 col-md-8">
                    <asp:Button runat="server" Text="Odošli hromadne" OnClick="Unnamed_Click" CssClass="btn btn-default" />
                </div>
            </div>
        </div>        
        <div class="container">
            <h4>Proces odosielania</h4>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="col-md-2"></div>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="progress" CssClass="form-control" MaxLength="10000" TextMode="MultiLine" Rows="5" Enabled="false" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br>
        </div>
          
        <div class="container">
            <h2>Užívatelia</h2>
            <br>
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>FirstName</th>
                        <th>LastName</th>
                        <th>ScoutNickName</th>
                        <th>Email</th>
                        <th>EmailConfirmed</th>
                        <th>SendNewsMail</th>
                        <th>Comments</th>
                        <th>LastLogin</th>
                        <th>LoginCount</th>                        
                    </tr>
                </thead>
                <tbody class="text-center">
                    <asp:Repeater ID="Users" SelectMethod="LoadUsers" ItemType="Kronika106.Admin.UserRecord" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Item.FirstName%>
                                </td>
                                <td>
                                    <%#Item.LastName%>
                                </td>
                                <td>
                                    <%#Item.ScoutNickName%>
                                </td>
                                <td >
                                    <%#Item.Email%>
                                </td>
                                <td class="bs-checkbox">
                                    <%#Item.EmailConfirmed%>
                                </td>
                                <td>
                                    <%#Item.SendNewsMail%>
                                </td>
                                <td>
                                    <%#Item.Comments%>
                                </td>
                                <td>
                                    <%#Item.LastLogin%>
                                </td>
                                <td>
                                    <%#Item.LoginCount%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <div class="container">
            <h2>Štatistika hľadania</h2>
            <br>
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>FirstName</th>
                        <th>LastName</th>
                        <th>ScoutNickName</th>                                                                    
                        <th>SearchPattern</th>
                    </tr>
                </thead>
                <tbody class="text-center">
                    <asp:Repeater ID="Repeater1" SelectMethod="LoadSearch" ItemType="Kronika106.Admin.UserRecord" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Item.FirstName%>
                                </td>
                                <td>
                                    <%#Item.LastName%>
                                </td>
                                <td>
                                    <%#Item.ScoutNickName%>
                                </td>
                                <td>
                                    <%#Item.SearchPattrern%>
                                </td>
                              
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <div class="container">
            <h2>Štatistika browsovania </h2>
            <br>
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>FirstName</th>
                        <th>LastName</th>
                        <th>ScoutNickName</th>                                                                    
                        <th>URL</th>
                    </tr>
                </thead>
                <tbody class="text-center">
                    <asp:Repeater ID="Repeater2" SelectMethod="LoadBrowse" ItemType="Kronika106.Admin.UserRecord" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Item.LastAccess%>
                                </td>
                                <td>
                                    <%#Item.FirstName%>
                                </td>
                                <td>
                                    <%#Item.LastName%>
                                </td>
                                <td>
                                    <%#Item.ScoutNickName%>
                                </td>
                                <td>
                                    <%#Item.BrowseURL%>
                                </td>
                              
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

    </div>
</asp:Content>
