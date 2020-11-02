<%@ Page Title="ˇVyhľadávanie" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResult.aspx.cs" Inherits="Kronika106.SearchResult" %>

<%@ Register Src="~/ForumControll.ascx" TagPrefix="uc1" TagName="ForumControll" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 runat="server" id="title"></h2>        
        <uc1:ForumControll runat="server" ID="ForumControll" forumMode="Reload" forumType="CommentList" />
    </div>
</asp:Content>
