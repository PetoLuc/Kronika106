<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RokAkcie.aspx.cs" Inherits="Kronika106.RokAkcie" %>

<asp:Content ID="RokAkcieContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="yearDetail" runat="server" ItemType="Kronika106.FileSystemModel.Year" SelectMethod="yearDetail_GetItem">
        <ItemTemplate>
            <div class="container">
                <div class="text-center">
                    <a class="templatemo_logo">
                        <h2><%#:Item.Rok%></h2>
                        <br />
                    </a>
                </div>
            </div>
            <div class="inner-content">
                <div class="text-justify" id="tab1">
                    <%--<h2 class="page-title">Stalo sa</h2>--%>
                    <p><%#Item.Popis%></p>
                </div>
            </div>            
                <div class="content products">
                    <div class="container">
                        <div class="row">
                            <asp:Repeater runat="server" ID="rptEvents" SelectMethod="AkcieGetForYear" ItemType="Kronika106.FileSystemModel.Akcia">
                                <ItemTemplate>
                                    <div class="col-md-4 col-sm-6" <%--style="max-width: 400px; max-height: 300px;--%>>
                                        <div class="product-item">
                                            <a class="product-title" href="AkciaPopis.aspx?ID=<%#Item.URL%>" title="Prejdi na akciu: <%#Item.Nazov %>">
                                            <img src="<%#Item.PathFotka%>"><%#Item.Nazov %></a>    
                                            <p style="overflow-wrap: break-word; overflow: no-display; line-height: 1.1; text-align: justify; height: 25px"><i>Komentárov: <%#Item.PocetKomentarov %></i></p>
                                            <p style="overflow-wrap:break-word; overflow:no-display; line-height:1.1; text-align:justify; height: 25px"><%#Item.Popis%> ...</p>
                                            <br>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>                
            </div>
        </ItemTemplate>
    </asp:FormView>
</asp:Content>


