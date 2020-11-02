<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AkciaPopis.aspx.cs" Inherits="Kronika106.AkciaPopis" Async="true" %>

<%@ Register Src="~/ForumControll.ascx" TagPrefix="uc1" TagName="ForumControll" %>

<asp:Content ID="EventDetailContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("iframe").css("max-height", "300px");
            $("iframe").css("max-width", "400px");
            $("iframe").css("min-widt", "120px");
            $("iframe").css("min-height", "80px");
            SetIframeSize();
            var o = window.orientation;
        });

        // resize on window resize
        $(window).on('resize', function () {

            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                return;
            }
            SetIframeSize();
        });

        $(window).on('orientationchange', function () {
            var mapFrame = document.getElementById("map");
            if (mapFrame) {
                var oldW = $("iframe").width();
                var oldH = $("iframe").height();
                if ($(window).height() < $(window).width()) {
                    var newW = $(window).height() - 80;
                    var newH = newW / 4 * 3;
                }
                else {//toto
                    var newW = $(window).width() - 40;
                    var newH = newW / 4 * 3;
                }
                if (newW == oldW) {
                    return;
                }
                if (Math.abs(oldW - newW) > 30) {

                    $("iframe").width(newW);
                    $("iframe").height(newH);
                    mapFrame.src = mapFrame.src;
                }
            }
        });

        function SetIframeSize() {

            var mapFrame = document.getElementById("map");
            if (mapFrame) {
                var oldW = $("iframe").width();
                var oldH = $("iframe").height();
                var newW = $(window).width() - 110;
                var newH = newW / 4 * 3;

                if (newW == oldW) {
                    return;
                }

                if (Math.abs(oldW - newW) > 50) {
                    $("iframe").width(newW);
                    $("iframe").height(newH);
                    mapFrame.src = mapFrame.src;
                }
            }
        }
    </script>
    <div class="container center">
        <div class="row">
            <div class="col-md-12">
                <%--  --%>
                <div class="container center">
                    <div class="text-center">
                        <h2 runat="server" id="header"></h2>
                        <div class="row" runat="server" id="mnPhoto">
                            <div runat="server" id="colLeftOrder">
                            </div>
                            <div runat ="server" class="col-md-2" style="padding: 0 5px 5px;" id="boxGallery" visible ="false">
                                <button runat="server" id="btnLinkGallery" class="btn btn-primary" onserverclick="linkToGallery_Click" title="Prezrieť fotky" visible="false"><i class="fa fa-camera fa-2x"></i> Fotogaléria</button>
                            </div>
                            <div runat="server" class="col-md-2" style="padding: 0 5px 5px;" id= "boxVideo" visible ="false">
                                <button runat="server" id="btnLinkToVideo" class="btn btn-primary" onserverclick="linkToVideoGallery_Click" title="Prezrieť videá" visible="false"><i class="fa fa-video-camera fa-2x"></i> Videogaléria</button>
                            </div>
                            <div class="col-md-4
                                " id="idMapa" runat="server">
                            </div>
                        </div>
                    </div>
                </div>

                <div class="container center">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="inner-content" id="textArea">
                                <div class="text-justify" id="tab1">
                                    <p runat="server" id="EventPopis">
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc1:ForumControll runat="server" ID="ForumControll" forumType="Event" ControlPageLoad="False" />
</asp:Content>
