<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VideoList.aspx.cs" Inherits="Kronika106.VideoList" %>
<asp:Content ID="VideoListContent" ContentPlaceHolderID="MainContent" runat="server">                 
            <div class="container">
                <div class="text-center">
                    <a class="templatemo_logo">
                        <h2 runat="server" id="VideoListTitle"></h2>
                        <br />
                    </a>
                </div>
            </div>          
          
                <div class="content products">
                    <div class="container">
                        <div class="row">
                            <asp:Repeater runat="server" ID="rptVideos" SelectMethod="GetVideosForAkcie" ItemType="Kronika106.FileSystemModel.Video">
                                <ItemTemplate>
                                    <div class="col-md-6<%--col-sm-6--%>" <%--style="max-width: 400px; max-height: 300px;--%>>
                                        <div class="product-item">
                                            <a class="product-title" href="Video.aspx?ID=<%#Item.DetailURL%>" title="Prejdi na video: <%#Item.Nazov %>">
                                            <img src="<%#Item.Poster%>"><%#Item.Nazov %></a>                                            
                                            <p style="overflow-wrap:break-word; overflow:no-display; line-height:1.1; text-align:justify; height: 25px"><%#Item.Popis%> ...</p>
                                            <br>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>                
            </div>        
</asp:Content>
