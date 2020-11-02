<%@ Page Title="Kronika" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gallery.aspx.cs" Inherits="Kronika106.Gallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content products">        
            <div class="row">
                <asp:Repeater runat="server" ID="rptYears" SelectMethod="LoadYears" ItemType="Kronika106.FileSystemModel.Year">
                    <ItemTemplate>
                        <div class="col-md-4 col-sm-6">
                            <div class="product-item">                                
                                <a class="product-title" href="<%#string.Format(@"RokAkcie.aspx?ID={0}",Item.Rok)%>" title="Akcie v roku <%#Eval("Rok")%>">
                                    <img src="<%#Item.PathFotka%>" /><%#Item.Rok%></a>                                    
                                <p style="overflow-wrap: break-word; overflow: no-display; line-height: 1.1; text-align: justify; height: 25px"><i>Komentárov: <%#Item.PocetKomentarov %></i></p>                                                                
                                <p style="overflow-wrap: break-word; overflow: no-display; line-height: 1.1; text-align: justify; height: 25px"><%#Item.Popis%> ...</p>                                                                
                                <br>                                
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>        
    </div>
</asp:Content>
