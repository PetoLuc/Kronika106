<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumControll.ascx.cs" Inherits="Kronika106.ForumControll" %>

<asp:HiddenField ID="hiddenCommentID" runat="server" />
<asp:HiddenField ID="hiddenSliderId" runat="server" />
<%--<asp:HiddenField ID="hiddenVideoPosition" runat="server" />--%>
<script>
    function showModalClick(e) {
        var commentDBKey = e.id.split("_")[1];

        //nastavenie DB ID
        var commentID = document.getElementById('<% =hiddenCommentID.ClientID%>');
        commentID.value = commentDBKey;
        var commentKey = "comment_" + commentDBKey;
        var commentTag = document.getElementById(commentKey);
        var commentData = commentTag.outerHTML;
        //var modalData = document.getElementById('oldContent');
        oldContent.innerHTML = commentData;
        deleteLinkButtons(oldContent);
        //var modalTitle = document.getElementById("myModalTitle");                

        //nastavenie controlov
        switch (e.className) {
            case "ReplyLink":
                myModalTitle.textContent = "Odpoveď na komentár";
                textBox.style.display = '';
                replyButton.style.display = '';
                deleteButtons.style.display = 'none';
                editButton.style.display = 'none';

                break;
            case "EditLink":
                myModalTitle.textContent = "Uprav komentár";
                textBox.style.display = '';
                replyButton.style.display = 'none';
                deleteButtons.style.display = 'none';
                editButton.style.display = '';
                break;
            case "DeleteLink":
                myModalTitle.textContent = "Naozaj chceš zmazať komentár :-(";
                textBox.style.display = 'none';
                replyButton.style.display = 'none';
                deleteButtons.style.display = '';
                editButton.style.display = 'none';
                break;
        }
    }

    function deleteLinkButtons(modalData) {
        var linkButton = modalData.getElementsByClassName("ReplyLink");
        if (linkButton != null && linkButton.item(0) != null) {
            linkButton.item(0).outerHTML = "";
        }

        linkButton = modalData.getElementsByClassName("EditLink");
        if (linkButton != null && linkButton.item(0) != null) {
            linkButton.item(0).outerHTML = "";
        }

        linkButton = modalData.getElementsByClassName("DeleteLink");
        if (linkButton != null && linkButton.item(0) != null) {
            linkButton.item(0).outerHTML = "";
        }
    }


    function limitText(limitField) {
        var limitCount = 0;
        var limitNum = 2500;
        if (limitField.value.length > limitNum) {
            limitField.value = limitField.value.substring(0, limitNum);
        }
        else {
            if (limitField == '') {
                limitCount = limitNum - 0;
            }
            else {
                limitCount = limitNum - limitField.value.length;
            }
        }
        if (limitCount == 0) {
            document.getElementById('replyErrorLabel').innerHTML = "Maximálna dĺžka odpovede je 2500 znakov";
        }
        if (limitCount != 0) {
            document.getElementById('replyErrorLabel').innerHTML = '';
        }
    }
    function getDetailForComment() {
        if (typeof jssor_1_slider != 'undefined') {
            var element = document.getElementById('<%=hiddenSliderId.ClientID %>');
            element.value = null;

            var index = jssor_1_slider.$CurrentIndex();
            var key = 'slideData' + index;
            var sliderData = document.getElementById(key);
            var data = sliderData.innerHTML;
            element.value = data;
        }

       <%-- var vid = document.getElementById("videoControll");
        if (vid != null && vid != "undefined") {
            var element = document.getElementById('<%=hiddenVideoPosition.ClientID %>');
            element.value = vid.currentTime;
        }   --%>
    }
    function showModalImageClick(e) {        
        var imgModal = document.getElementById("imgModal");
        if (imgModal != null && imgModal != "undefined") {
            imgModal.src = "";
           imgModal.src = e.children[0].src.replace("/Thumbs", "");
            $('#bigPhotoModal').modal('show');
        }
    }
    $(document).ready(function () {
        $('#bigPhotoModal').bind('contextmenu', function () { return false; });
    });

    $(window).bind("load", function () {        
        var element = document.getElementById('<%=hiddenCommentID.ClientID %>');
        if (element.value != null) {
            var controll = document.getElementById(element.value);
            if (controll != 'undefined' && controll != null) {
                $('html, body').animate({
                    scrollTop: $("#" + element.value).offset().top
                }, 300);
            }
            element.value = null;
        }
    });
 
        

</script>

<div class="container">
    <div class="row">
        <div class="col-md-12">
            <br />
            <asp:UpdatePanel ID="UpdatePanelAddComment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset>
                        <div class="container">
                            <div class="row" runat="server" id="addCommentRow">
                                <%--   <div class="col-md-10 col-md-offset-1">--%>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:TextBox runat="server" Rows="4" ID="txtNewComment" CssClass="form-control" TextMode="MultiLine" Style="/*font-size: 10px; */ line-height: 1.2" MaxLength="2500" />
                                        <asp:RegularExpressionValidator CssClass="text-danger" Display="Dynamic" ControlToValidate="txtNewComment" ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{0,2500}$" runat="server" ErrorMessage="Maximálne 2500 znakov.">
                                        </asp:RegularExpressionValidator>
                                        <asp:ModelErrorMessage runat="server" ModelStateKey="txtNewComment" CssClass="text-error" />
                                        <asp:Button ID="btnSend" runat="server" Text="Pridaj komentár" OnClick="btnSend_Click" UseSubmitBehavior="false" OnClientClick="javascript: getDetailForComment(); this.disabled=true;" CssClass="btn btn-default pull-left" />
                                        <br />
                                        <br />
                                    </div>
                                    <%--</div>--%>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Timer ID="TimerRefreshForum" runat="server" OnTick="TimerRefreshForum_Tick" Enabled="False">
            </asp:Timer>
            <asp:Panel runat="server" ID="CommentsPanel">
                <asp:UpdatePanel ID="UpdatePanelComments" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TimerRefreshForum" EventName="Tick" />
                    </Triggers>
                    <ContentTemplate>
                        <fieldset>
                            <div class="container" runat="server" id="commentsContainer">
                                <div class="inner-content">
                                    <div runat="server" id="phComments">
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>
</div>

<asp:UpdatePanel ID="modalUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <fieldset>
            <!-- Trigger the modal with a button -->
            <%--<button type="button" class="btn btn-info btn-lg btnModal" data-toggle="modal"  data-target="#myModal">Open Modal</button>--%>
            <!-- Modal -->
            <div class="modal fade" tabindex="-1" id="myModal" role="dialog">
                <div class="modal-dialog" role="dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title" id="myModalTitle">Odpoveď na komentár</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12" id="oldContent">
                                </div>
                            </div>
                            <div class="row" id="textBox">
                                <div class="col-md-12">
                                    <label id="replyErrorLabel" style="color: red"></label>
                                    <asp:TextBox runat="server" Rows="5" ID="reply" TextMode="MultiLine" CssClass="form-control" Style="/*font-size: 10px; */line-height: 1.2; resize: vertical; max-height: 200px" MaxLength="2500" onkeyup="limitText(this);" onkeypress="limitText(this);" onkeydown="limitText(this);" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="replyButton">
                                        <asp:Button ID="btnCommitComment" runat="server" UseSubmitBehavior="false" Text="Odpovedaj" CssClass="btn btn-default pull-left" OnClick="btnSendReply_Click" data-dismiss="modal" />
                                    </div>


                                    <div id="editButton">
                                        <asp:Button ID="btnCommintUpdate" runat="server" UseSubmitBehavior="false" Text="Ulož" CssClass="btn btn-default pull-left" OnClick="btnCommintUpdate_Click" data-dismiss="modal" />
                                    </div>
                                    <div id="deleteButtons">
                                        <asp:Button ID="btnComitDelete" runat="server" UseSubmitBehavior="false" Text="Áno" CssClass="btn btn-default pull-left" OnClick="btnComitDelete_Click" data-dismiss="modal" />
                                        <asp:Button ID="btnNo" runat="server" UseSubmitBehavior="false" Text="Nie" CssClass="btn btn-default pull-left" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel runat="server" ID="updatePanelBigPhoto" UpdateMode="Conditional">
    <ContentTemplate>        
<div class="modal fade" tabindex="-1" id="bigPhotoModal" role="dialog">
    <div class="modal-dialog modal-lg" role="dialog">

        <%--  <div class="modal-header">
                
            </div>--%>
        <div class="modal-body" style="background:url(Content/Images/deer.gif) no-repeat center;" >
            <button type="button" class="close" style="color: black" data-dismiss="modal">×</button>
            <br>          
            <%--<img src="Content/Images/loading.gif" />--%>  
            <img id="imgModal" src=""/>
        </div>
    </div>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>

