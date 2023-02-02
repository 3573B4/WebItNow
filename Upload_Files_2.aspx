<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Upload_Files_2.aspx.cs" Inherits="WebItNow.Upload_Files_2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="Scripts/jquery-3.4.1.min.js"></script>
    <script language="javascript" type="text/javascript">
        
        var timer = setTimeout(function () {
            document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
            var modalId = '<%=mpeExpira.ClientID%>';
            var modal = $find(modalId);
            modal.show();

            //alert("La sesión ha expirado.");
            //location.href = '/Login.aspx';
        }, 600000);

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        $(function () {
            $('[id*=GridView1]').footable();
        });

        function ShowProgress() {
            setTimeout(function () {
                //var modal = $('<div />');
                //modal.addClass("modal");
                //$('body').append(modal);
                //$('modal-dialog-centered').append(modal);
                //modal.addClass("modal-dialog-centered");
                var loading = $(".modal");
                //loading.addClass("modal-dialog-centered");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function () {
            ShowProgress();
        });


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <ContentTemplate>

    <div class="container col-md-4">
        <h2 class="h2 mb-3 fw-normal">Carga segura de documentos</h2>
        <br />
        <div class="form-group">
            <div class="d-grid col-4 mx-auto py-1">
                <asp:Label ID="lblUsuario" runat="server" Font-Size="XX-Large" ></asp:Label>
            </div>
        </div>

<%--        <div class="form-group">
            <div class="d-grid col-4 mx-auto py-1">
                <asp:Label ID="indicaciones" runat="server" Text="Seleccione el archivo a subir" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            </div>
        </div>--%>

        <br />
        <div class="form-floating">
                <div class="dropdown">
                    <asp:DropDownList ID="ddlDocs" runat="server" CssClass="btn btn-outline-secondary"  AutoPostBack="true" OnSelectedIndexChanged="ddlDocs_SelectedIndexChanged" Width="100%">
                    </asp:DropDownList>
                </div>
        </div>
        <br />
        <br />

        <div class="form-group">
            <asp:Label ID="LblInstrucciones" runat="server" Text="Instrucciones" CssClass="control-label co-sm-3 " Font-Bold="False"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtDescrpBrev" runat="server" CssClass="form-control mt-2" placeholder="Usuario" ReadOnly="True"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="input-group mx-auto pt-3">
                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control"></asp:FileUpload>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto pt-3">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
        </div>

        <div class="form-group my-2">
            <div class="col-12">
                <div class="d-grid gap-4 d-flex justify-content-center">
                    <asp:Button ID="BtnEnviar" runat="server" Font-Bold="True" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subir&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" OnClick="BtnEnviar_Click" OnClientClick="ShowProgress()" CssClass="btn btn-primary" />
                    <asp:Button ID="BtnRegresar" runat="server" Font-Bold="True" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Regresar&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" OnClick="BtnRegresar_Click" CssClass="btn btn-outline-primary me-md-2" />
                </div>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <%--<h1 class="modal-title fs-5" id="exampleModalLabel">Modal Espera</h1>--%>
                    </div>
                    <div class="modal-body">
                        <%--<div class="Text-center">
                            <img src="Images/loader.gif" alt="" />
                        </div>--%>
                        <div class="text-center">
                            <div class="spinner-border" role="status">
                                <span class="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
            </div>

            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                    TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
            </div>
        </div>

    </div>

    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
        <div class=" row justify-content-end" data-bs-theme="dark">
            <div class="col-1">
                <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
            </div>
        </div>
        <div>
             <br />
            <hr class="dropdown-divider" />
        </div>
        
        <div>
            <br />
            <asp:Label ID="LblMessage" runat="server" Text="" />
        </div>
        <div>
            <br />
            <hr class="dropdown-divider" />
        </div>
        
        <div>
            <br />
            <asp:Button ID="btnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
        </div>


    </asp:Panel>

    <asp:Panel ID="pnlExpira" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
        <div class=" row justify-content-end" data-bs-theme="dark">
            <div class="col-1">
                <asp:Button runat="server" OnClientClick="acceso(); return false;" type="button" class="btn-close" aria-label="Close" />
            </div>
        </div>
        <div>
             <br />
            <hr class="dropdown-divider" />
        </div>
        <div>
             <br />
            <hr class="dropdown-divider" />
        </div>
        <div>
            <asp:Label ID="LblExpira" runat="server" Text="" />
        </div>
        <div>
            <br />
            <hr class="dropdown-divider" />
        </div>
        <div>
            <br />
                <asp:Button ID="BtnClose_Expira" OnClientClick="acceso(); return false;" runat="server" Text="Cerrar" CssClass="btn btn-outline-primary"/>
        </div>

    </asp:Panel>   

    </ContentTemplate>
</asp:Content>
