<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="Kronika106.Search" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="navbar-brand navbar-form navbar-right" role="search" runat="server">
            <div class="input-group" runat="server">
                <asp:TextBox runat="server" ID="txtSearch" class="form-control search" MaxLength="50" placeholder="Hľadaj"  ></asp:TextBox>
                <div class="input-group-btn">
                    <asp:LinkButton runat="server" ID="btnSearch" class="btn btn-default" type="submit" OnClick="btnSearch_Click"><span class="glyphicon glyphicon-search"></span></asp:LinkButton>
                </div>                 
            </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
