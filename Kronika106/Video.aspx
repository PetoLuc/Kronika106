<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Video.aspx.cs" Inherits="Kronika106.Video" Async="true" %>
<%@ Register Src="~/ForumControll.ascx" TagPrefix="uc1" TagName="ForumControll" %>
<asp:Content ID="VideoDetail" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('#videoControll').bind('contextmenu', function () { return false; });
        });
    </script>
    <div class="container center">
        <h1 runat="server" id="eventName">bla bla</h1>
    </div>

    <div class="container center">
        <div class="row">
            <div class="col-md-12">
                <div class="inner-content" id="textArea">
                    <div class="text-justify" id="tab1">
                        <p runat="server" id="VideoPopis">
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <br>
        <div class="row">
            <div class="col-md-12">
                <asp:Repeater ID="videoItem" runat="server" ItemType="Kronika106.FileSystemModel.Video" SelectMethod="GetVideoDetail">
                    <ItemTemplate>
                        <div style="margin: 0 auto; position: relative; max-width: 800px;">
                            <video id="videoControll" controls="controls" poster="<%#Item.Poster %>" autoplay="autoplay" <%--poster="uvod.jpg"--%> style="width: 100%" title="Video">
                                <source src="<%#Item.WEBM%>" type='video/webm'>
                                <source src="<%#Item.MP4%>" type='video/mp4'>
                                <source src="<%#Item.OGV%>" type='video/ogg'>
                            </video>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>    
    <br />
    <uc1:ForumControll runat="server" ID="ForumControll" forumType="EventVideoGallery" />
</asp:Content>
