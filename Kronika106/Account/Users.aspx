<%@ Page Title="Kto je tu :-)" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Kronika106.Users" %>

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
        <h2>Kto je tu</h2>
        <div id="userList">
            <div class="container">
                <div class=" container inner-content">
                    <asp:Repeater runat="server" ID="rptUsers" SelectMethod="LoadUsers" ItemType="Kronika106.Admin.UserRecord">
                        <ItemTemplate>
                            <div class="row" id="<%#Item.UserId%>">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <p id="comment_5690" style="line-height: 1.1; text-align: justify; word-wrap: break-word;">
                                            <strong><%#string.IsNullOrEmpty(Item.ScoutNickName)? string.Format("{0} {1}", Item.FirstName, Item.LastName): Item.ScoutNickName%></strong><%#string.IsNullOrEmpty(Item.ScoutNickName)? "": Item.FirstName+ " " +Item.LastName%>                                                                                        
                                                                                                                                    
                                            <a <%# Item.Comments>0 ? "href=/LastComments?ID=" +Server.UrlEncode(Item.UserId):""%>>Komentáre (<%#Item.Comments %>)</a>
                                                <%--href='<%#$"/LastComments?ID={Server.UrlEncode(Item.UserId)}" %>'>Komentáre (<%#Item.Comments %>)</a>--%>
                                                                                        
                                            <br>
                                            <%--<img src="AllPhotos/1999/Pustý hrad/Thumbs/366.JPG" class="img_left" style="max-height: 90px; max-width: 120px; width: auto; height: auto">Siskovi trčí hlava z mosta,  čo ide v ZV popod Pustý hrad(ponad geto- Balkán).  Ten most je zvnútra dutý, my sme doň vliezli a chodili sme vo vnútri. Je to jeden veľký tunel porozdeľovaný priečkami.--%>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <hr>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
