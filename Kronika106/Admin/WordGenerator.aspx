<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WordGenerator.aspx.cs" Inherits="Kronika106.Admin.WordGenerator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2>Kronika2Word</h2>
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
        <h4>Generovanie stránky do docx</h4>
        <hr />
        <div class="form-group">
            <div class="row">
                <div class="col-md-offset-2 col-md-8">
                    <asp:Button runat="server" Text="Generuj" OnClick="Unnamed_Click" CssClass="btn btn-default" />
                </div>
            </div>
        </div>                                  
    </div>

</asp:Content>
