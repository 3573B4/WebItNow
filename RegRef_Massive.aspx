<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RegRef_Massive.aspx.cs" Inherits="WebItNow.RegRef_Massive" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">

    var timer = setTimeout(function () {
        document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
        var modalId = '<%=mpeExpira.ClientID%>';
        var modal = $find(modalId);
        modal.show();

    }, 600000);

    function acceso() {
        location.href = '/Login.aspx';
    }

    function mpeMensajeOnOk() {
        //
    }

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
        <h2 class="h2 mb-3 fw-normal mt-4">Carga de Datos</h2>
        <div class="input-group">
            <asp:Label ID="LblExcel" CssClass="form-control col-md-5 p-2" runat="server" Text="Descarga plantilla Excel" Font-Size="Medium"></asp:Label>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
            <asp:ImageButton ID="imgExcel" runat="server" CssClass="col-md-2 mx-1" Height="40px" Width="40px" ImageUrl="~/Images/excel.png" OnClick="imgExcel_Click" OnClientClick="showProgress()" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="input-group pt-2 ">
            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="BtnCargaExcel" runat="server" Font-Bold="True" Text="Subir" OnClick="BtnCargaExcel_Click" OnClientClick="showProgress()" CssClass="btn btn-outline-secondary" BorderColor="#CCCCCC" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="from-group mt-2">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
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
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td><asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" /></td>
            <td></td>
            <td></td>
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
        <asp:PostBackTrigger ControlID="imgExcel" />
        <asp:PostBackTrigger ControlID="BtnCargaExcel" />
    </Triggers>
</asp:UpdatePanel>

<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
    <ProgressTemplate>
    <div id="divImage" class ="loading">
        <div class="center">
                <img alt="Processing..." src="Images\ajax-loader.gif" />
        </div>
    </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2" DynamicLayout="true">
    <ProgressTemplate>
    <div id="divImage" class ="loading">
        <div class="center">
                <img alt="Processing..." src="Images\ajax-loader.gif" />
        </div>
    </div>
    </ProgressTemplate>
</asp:UpdateProgress>

</asp:Content>

