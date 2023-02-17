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
            location.href = '/Access.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        $(function () {
            $('[id*=GridView1]').footable();
        });

        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress1.ClientID %>");
            updateProgress.style.display = "block";
        }

        $('form').live("submit", function () {
            ShowProgress();
        });

    </script>

    <style type="text/css">
        .loading {
            position: fixed;
            top: 0; right: 0;
            bottom: 0; left: 0;
            /*background: #fff;*/

            z-index: 98;
            background-color: #fafafa; 
        /*filter: alpha(opacity=80);*/ 
            opacity: 0.8; 
        }

        .center img{
            height: 128px;
            width:  128px;
            background-color:transparent;
            opacity: 0.8;
            position: fixed;
            z-index: 98;
            top:45%;
            left: 45%;
            position: absolute;
        }
        .overlay  
        {
          position: fixed;
          z-index: 98;
          top: 0px;
          left: 0px;
          right: 0px;
          bottom: 0px;
          background-color: #aaa; 
       /* filter: alpha(opacity=80);*/ 
          opacity: 0.8; 
        }
        .overlayContent
        {
          z-index: 99;
          margin: 250px auto;
          width: 80px;
          height: 80px;
        }
        .overlayContent h2
        {
            font-size: 18px;
            font-weight: bold;
            color: #000;
        }
        .overlayContent img
        {
          width: 80px;
          height: 80px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    
    <script type="text/javascript">  

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        // Agregar initializeRequest y endRequest
        prm.add_initializeRequest(prm_InitializeRequest);
        prm.add_endRequest(prm_EndRequest);

        // Llamado cuando comienza la devolución de datos asíncrona
        function prm_InitializeRequest(sender, args) {
            // Obtener el divImage y configurarlo en visible
            var panelProg = $get('divImage');
            panelProg.style.display = '';

            // Deshabilitar el botón que provocó una devolución de datos
            $get(args._postBackElement.id).disabled = true;
        }

        // Llamado cuando finaliza la devolución de datos asincrónica
        function prm_EndRequest(sender, args) {
            // obtener el divImage y ocultarlo de nuevo
            var panelProg = $get('divImage');
            panelProg.style.display = 'none';

            // Habilitar el botón que provocó una devolución de datos
            $get(sender._postBackSettings.sourceElement.id).disabled = false;
        }

    </script>

<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
    <div class="container col-md-4">
        <h2 class="h2 mb-3 fw-normal">CARGA SEGURA DE DOCUMENTOS</h2>
        <br />

        <div class="form-group ">
            <div class="d-grid col-12 justify-content-center mx-auto py-1">
                <asp:Label ID="lblUsuario" runat="server" Font-Size="X-Large" ></asp:Label>
            </div>
        </div>

<%--        
        <div class="form-group">
            <div class="d-grid col-4 mx-auto py-1">
                <asp:Label ID="indicaciones" runat="server" Text="Seleccione el archivo a subir" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            </div>
        </div>
--%>

        <br />
        <div class="form-floating">
                <div class="dropdown">
                    <asp:DropDownList ID="ddlDocs" runat="server" CssClass="btn btn-outline-secondary"  AutoPostBack="true" OnSelectedIndexChanged="DdlDocs_SelectedIndexChanged" Width="100%">
                    </asp:DropDownList>
                </div>
        </div>
        <br />
        <br />

        <div class="form-group">
            <asp:Label ID="LblInstrucciones" runat="server" Text="Instrucciones" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <asp:Button ID="BtnEnviar" runat="server" Font-Bold="True" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subir&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" OnClick="BtnEnviar_Click" OnClientClick="showProgress()" CssClass="btn btn-primary" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                    <asp:Button ID="BtnRegresar" runat="server" Font-Bold="True" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Regresar&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" OnClick="BtnRegresar_Click" CssClass="btn btn-outline-primary me-md-2" />
                </div>
            </div>
        </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnEnviar" />
        </Triggers>
    </asp:UpdatePanel>

        <!-- Modal -->
        <div class="modal" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        
                    </div>
                    <div class="modal-body">
                        
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

<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
    <ProgressTemplate>
    <div id="divImage" class ="loading">
        <div class="center">
    <%--    <asp:Image ID="imgLoading" runat="server" ImageUrl="Images\ajax-loader.gif" Width="34px" />  --%>
            <img alt="Processing..." src="Images\ajax-loader.gif" />
        </div>
    </div>
    </ProgressTemplate>
</asp:UpdateProgress>

</asp:Content>
