<%@ Page Title="Najnovšie komentáre" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LastComments.aspx.cs" Inherits="Kronika106.LastComments"  Async="true"%>

<%@ Register Src="~/ForumControll.ascx" TagPrefix="uc1" TagName="ForumControll" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        hr {
            display: block;
            height: 1px;
            border: 0;
            border-top: 1px solid #ccc;
            margin: 0.5em 0;
            padding: 0;
        }
    </style>
    <div class="container">
    <h2 runat="server" id="title"></h2>
    <asp:UpdatePanel ID="UpdLastComments" runat="server">
        <ContentTemplate>
            <%--<div class="container">--%>
                <%--<div class="container inner-content">
                    <div runat="server" id="phComments">
                    </div>--%>
                    <uc1:ForumControll runat="server" ID="ForumControll" forumType="CommentList" forumMode="Append" />                
                    <asp:Button ID="btnLoadNextComments" CssClass="btn btn-default  btn-xs" runat="server" Text="Staršie" OnClick="Button1_Click" />
                    <%--</div>--%>
          <%--      </div>                
            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
        </div>
</asp:Content>
