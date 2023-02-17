<%@ Page Title="" Language="C#" EnableViewState="false" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Review_Document.aspx.cs" Inherits="WebItNow.Review_Document" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>

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

        function clearTextBox() {
            var elements = [];
            elements = document.getElementsByClassName("form-control");

            for (var i = 0; i < elements.length; i++) {
                elements[i].value = "";
            }
        }

        function setFormSubmitToFalse() {
            setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
            return true;
        }

        function timedRefresh(timeoutPeriod) {
            setTimeout("location.reload(true);", timeoutPeriod);
        }

        function realizarPostBack() {
            __doPostBack('', '');
        }

        function openPageDescargas() {

            window.open('Descargas.aspx');
            return true;
        }

        $(function () {
            $('[id*=GridView1]').footable();
        });

        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
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

    <link rel="stylesheet" type="text/css" href="loading-bar.css"/>
    <script type="text/javascript" src="loading-bar.js"></script>

    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <link href="~/Scripts/footable.min.js" rel="stylesheet" type="text/javascript" />
    <link href="~/Styles/footable.min.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        .modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            /*filter: alpha(opacity=80);*/
            min-height: 100%;
            width: 100%;
        }

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

        .loader {
            left: 50%;
            margin-left: -4em;
            font-size: 10px;
            border: .8em solid rgba(218, 219, 223, 1);
            border-left: .8em solid rgba(58, 166, 165, 1);
            animation: spin 1.1s infinite linear;
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
    </style>

    <style type="text/css">
        .overlay  
        {
          position: fixed;
          z-index: 98;
          top: 0px;
          left: 0px;
          right: 0px;
          bottom: 0px;
          background-color: #aaa; 
        /*filter: alpha(opacity=80);*/ 
          opacity: 0.8; 
        }
        .overlayContent
        {
          background-color:transparent;
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

        .hide {
            display: none!important;
        }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>

<%--    initializeRequest y endRequest  --%>
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
        // $get(sender._postBackSettings.sourceElement.id).disabled = false;
    }

</script>
    
<asp:UpdatePanel ID="UpdatePanel" runat="server" >
    <ContentTemplate>
    <br />
    <div class="container col-md-6">
        <h2 class="h2 mb-3 fw-normal">Aprobación de Documentos</h2>
        <br />
        <h2 class="h2 mb-5 fw-normal">Archivos Recibidos</h2>
        <br />

        <div style="overflow-x: auto; overflow-y:hidden">
            <asp:GridView ID="GrdEstadoDocumento"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="900px"
                    AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
                    OnPageIndexChanging="GrdEstadoDocumento_OnPageIndexChanging" OnRowCommand="GrdEstadoDocumento_RowCommand"
                    OnSelectedIndexChanged="GrdEstadoDocumento_SelectedIndexChanged"
                    OnRowDataBound ="GrdEstadoDocumento_RowDataBound" DataKeyNames="Referencia" >
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgAceptado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/aceptar.ico" OnClick="ImgAceptado_Click" Enabled="false" />
                            </ItemTemplate> 
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgRechazado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/cancelar.ico" OnClick="ImgRechazado_Click" Enabled="false" />
                            </ItemTemplate> 
                        </asp:TemplateField>

                        <asp:BoundField DataField="Referencia" HeaderText="Referencia" >
                        <ItemStyle Width="150px" /> 
                        </asp:BoundField>
                        <asp:BoundField DataField="Desc_Status" HeaderText="Estatus" >
                        <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Tipo de Documento" >
                        <ItemStyle Width="850px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nom_Imagen" HeaderText="Nombre de Archivo" >
                        <ItemStyle Width="850" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Url_Imagen"  >
                        <ItemStyle Width="0px" Font-Size="0pt" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IdTipoDocumento"  >
                        <ItemStyle Width="0px" Font-Size="0pt" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IdDescarga"  >
                        <ItemStyle Width="0px" Font-Size="0pt" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UsAsegurado"  >
                        <ItemStyle Width="0px" Font-Size="0pt" />
                        </asp:BoundField>
                        <%--ItemStyle-CssClass="hide"--%>
                    </Columns>
                
                    <PagerStyle CssClass="pgr" />
            </asp:GridView>
        </div>

        <div class="">
            <asp:HiddenField ID="hdfValorGrid" runat="server" Value=""/>
            <br />
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row g-3 mb-3">
                <div class="col-lg-4 col-md-5 col-sm-4">
                    <asp:Label ID="LblRef" runat="server" Text="Referencia Seleccionada" CssClass="control-label" Font-Size="Small"></asp:Label>
                    <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                </div>
                <asp:TextBox ID="TxtTpoDocumento" runat="server" CssClass="form-control" placeholder="Tipo de Documento" ReadOnly="True" Visible="false"></asp:TextBox>
                <asp:TextBox ID="TxtUrl_Imagen" runat="server" CssClass="form-control" placeholder="Ruta del archivo" Enabled="False" Visible="false" ></asp:TextBox>
                <div class="col-lg-7 col-md-5 col-sm-6">
                    <asp:Label ID="LblNomArchivo" runat="server" Text="Archivo Seleccionado" CssClass="control-label" Font-Size="Small"></asp:Label>
                    <asp:TextBox ID="TxtNomArchivo" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                </div>
                <div class="col-lg-1 col-md-2 col-sm-2 text-center">
                    <asp:Label ID="LblDescarga" runat="server" Text="Descargar" CssClass="control-label" Font-Size="Small"></asp:Label>
                    <asp:ImageButton ID="imgDescarga" runat="server" CssClass="btn p-0" ImageUrl="~/Images/descargar.png" Height="35px" Width="35px" OnClick="ImgDescarga_Click" Enabled ="false" />
                </div>
            </div>
            <div class="row g-3">
                <div class="col-lg-4 col-md-5 col-sm-4 mt-3 ">
                    <asp:Label ID="lblAsegurado" runat="server" Text="Nombre de Cliente :" CssClass="control-label" Font-Size="Small"></asp:Label>
                </div>
                <div class="col-lg-8 col-md-7 col-sm-8">
                    <asp:TextBox ID="TxtAsegurado" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                </div>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>

<%-- 
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnDescargas" runat="server" Text="  Descarga Azure  " OnClick="BtnDescargas_Click" CausesValidation="false"  CssClass="btn btn-outline-primary" Visible="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
--%>

<%--        
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnDownload" runat="server" Text="Descarga Dispositivo" OnClick="BtnDownload_Click" CausesValidation="false"  CssClass="btn btn-outline-primary" Visible="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
--%>
        <br />
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header"> </div>
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

    </div>
    <br />
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
    <br />
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
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td>
                <div class="form-group">
                    <div class="d-grid col-6 mx-auto">
                        <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                            TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                    </div>
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                    TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td class="style3"><asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
            </td>
            <td></td>
            <td></td>
        </tr>
    </table>

    </ContentTemplate>

    <Triggers>
        <asp:PostBackTrigger ControlID="grdEstadoDocumento" />
<%--    <asp:PostBackTrigger ControlID="BtnDescargas" />    --%>
<%--    <asp:PostBackTrigger ControlID="imgDescarga" />     --%>
    </Triggers>
</asp:UpdatePanel>

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
