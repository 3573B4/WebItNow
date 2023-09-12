<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Document_Files.aspx.cs" Inherits="WebItNow.Document_Files" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script language="javascript" type="text/javascript">
        var timer = setTimeout(function () {
            document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
            var modalId = '<%=mpeExpira.ClientID%>';
            var modal = $find(modalId);
            modal.show();
        }, 3600000);

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        function mpeNewProceso() {
            //
        }

        function mpeNewEnvio() {
            //
        }

        function mpeNewDocumento() {
            //
        }
        
        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress1.ClientID %>");
            updateProgress.style.display = "block";
        }

    </script>

    <script type="text/javascript">
        var _validFilejpeg = [".jpeg", ".jpg", ".bmp", ".pdf", ".zip", ".rar"];

        function validateForSize(oInput, minSize, maxSizejpeg) {
            //if there is a need of specifying any other type, just add that particular type in var  _validFilejpeg
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFilejpeg.length; j++) {
                        var sCurExtension = _validFilejpeg[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length)
                            .toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            break;
                        }
                    }

                    if (!blnValid) {

                        document.getElementById('<%=LblMessage.ClientID %>').innerHTML = 'El archivo tiene un formato invalido';
                        $find('bMpeModalCargando').show();

                        //alert("Sorry, this file is invalid, allowed extension is: " + _validFilejpeg.join(", "));
                        oInput.value = "";
                        return false;
                    }
                }
            }

            fileSizeValidatejpeg(oInput, minSize, maxSizejpeg);
        }

        function fileSizeValidatejpeg(fdata, minSize, maxSizejpeg) {
            if (fdata.files && fdata.files[0]) {
                var fsize = fdata.files[0].size / 1024; //The files property of an input element returns a FileList. fdata is an input element,fdata.files[0] returns a File object at the index 0.
                //alert(fsize)
                if (fsize > maxSizejpeg) {

                    document.getElementById('<%=LblMessage.ClientID %>').innerHTML = 'El documento Excede los 120 MB';
                    $find('bMpeModalCargando').show();

                    //alert('This file size is: ' + fsize.toFixed(2) +
                    //    "KB. Files should be in " + (minSize) + " to " + (maxSizejpeg) + " KB ");

                    fdata.value = ""; //so that the file name is not displayed on the side of the choose file button
                    return false;
                } else if (fsize < minSize){
                    document.getElementById('<%=LblMessage.ClientID %>').innerHTML = 'El archivo esta dañado';
                    $find('bMpeModalCargando').show();

                    fdata.value = ""; // para que el nombre del archivo no se muestre al lado del botón de elegir archivo
                    return false;
                } else {
                    console.log("");
                }
            }
        }
    </script>
        <style type="text/css">
        .ajax_calendar_container
        {
            position: relative;
            z-index:1000;
        }
        .Initial
        {
          display: block;
          padding: 4px 18px 4px 18px;
          float: left;
          background: url("../Images/InitialImage.png") no-repeat right top;
          color: Black;
          font-weight: bold;
        }
        .Initial:hover
        {
          color: White;
          background: url("../Images/SelectedButton.png") no-repeat right top;
        }
        .Clicked
        {
          float: left;
          display: block;
          background: url("../Images/SelectedButton.png") no-repeat right top;
          padding: 4px 18px 4px 18px;
          color: Black;
          font-weight: bold;
          color: White;
        }

        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .8em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left:10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
          width: 150px !important;    
        }
        #divwidth div
       {
        width: 150px !important;   
       }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

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
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="container col-md-8 col-lg-7">
        <div class="row mt-4">
            <div class="row col-lg-8 col-md-7 col-sm-7 py-0">
                <div class="col-2 py-0 pt-2">
                    <asp:Label ID="LblReferencia" runat="server" Text="Referencia" CssClass="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <div class="input-group">
                        <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control form-control-sm" MaxLength="12" ReadOnly="true" autocomplete="off" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-2 py-0 pt-2">
                    <asp:Label ID="LblAsegurado" runat="server" Text="Asegurado: " CssClass="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="TxtAsegurado" runat="server" CssClass="form-control form-control-sm" Font-Size="Small" ReadOnly="true" ></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
            <h6 class="h6 fw-normal my-1"> </h6>
        </div>
    </div>
    <div class="container col-md-8 col-lg-7"">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
            <ContentTemplate>
                <div class="row mb-3 mt-4">
                    <div class="form-group">
                        <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                            <asp:Button ID="BtnDocument_Nuevo" runat="server" Text="Nuevo Documento" Font-Bold="True" OnClick="BtnDocument_Nuevo_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="BtnDocument_Proceso" runat="server" Text="Documentos del Proceso" Font-Bold="True" OnClick="BtnDocument_Proceso_Click" CssClass="btn btn-secondary" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div style="overflow-x: auto; overflow-y:hidden">
            <asp:GridView ID="GrdTpoDocumento"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="900px"
                    AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                    OnPageIndexChanging="GrdTpoDocumento_PageIndexChanging" OnRowCommand="GrdTpoDocumento_RowCommand"
                    OnSelectedIndexChanged="GrdTpoDocumento_SelectedIndexChanged"
                    OnRowDataBound ="GrdTpoDocumento_RowDataBound" DataKeyNames="IdTpoDocumento" PageSize="8" >
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgEnvioArchivo" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/envia_doc.png" OnClick="ImgEnvioArchivo_Click" Enabled="true" />
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgVerArchivo" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/visor_doc.jpg" OnClick="ImgVerArchivo_Click" Enabled="true" />
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Entregado" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkEntregado" runat="server" onclick="return false" Checked='<%# Eval("Entregado") %>' ></asp:CheckBox>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Tipo de documento" >
                        <ItemStyle Width="150px" /> 
                        </asp:BoundField>
                        <asp:BoundField DataField="Desc_Documento" HeaderText="Descripción del documento" >
                        <ItemStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Fec_Entrega" DataFormatString="{0:d}" HeaderText="Fecha de entrega" >
                        <ItemStyle Width="850" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IdTpoDocumento"  >
                        <ItemStyle Width="0px" Font-Size="0pt" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Url_Imagen"  >
                        <ItemStyle Width="0px" Font-Size="0pt" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nom_Imagen"  >
                        <ItemStyle Width="0px" Font-Size="0pt" />
                        </asp:BoundField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
            </asp:GridView>
        </div>

        <div class="">
            <asp:HiddenField ID="hdfValorGrid" runat="server" Value=""/>
            <br />
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>    
    </div>

    <div class="from-group mb-4 pb-5">
        <div class="d-grid col-6 mx-auto">
            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"  />
        </div>
    </div>

    <div class="form-group">
        <div class="d-grid col-6 mx-auto">
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlMensaje"
                TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
            </ajaxToolkit:ModalPopupExtender>
            <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" />
        </div>
    </div>
    <br />
    <asp:Panel ID="PnlDocProceso" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 800px; background-color:#FFFFFF;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div class="container">
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
                        <div class="d-flex flex-row mb-2">
                            <asp:Label ID="LblDocumentos" runat="server" Text="Documentos" />
                        </div>
                        <div class="col-lg-8 mb-3">
                                <asp:Panel runat="server" DefaultButton="BtnBuscar">
                                    <asp:TextBox ID="txtPnlBusqProceso" runat="server" placeholder="Buscar/Proceso" CssClass="form-control form-control-sm ms-2" ></asp:TextBox>
                                    <asp:Button ID="BtnBuscar" runat="server" OnClick="BtnBuscar_Click" Style="display: none" />
                                </asp:Panel>
                        </div>
                        <div class="">
                            <asp:GridView ID="grdPnlBusqProceso" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                    AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
                                    PageSize="10" >
                                <AlternatingRowStyle CssClass="table" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChBoxRow" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Desc_Proceso" HeaderText="Proceso" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion del documento" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div>
                        <br />
                        <hr class="dropdown-divider" />
                    </div>
                    <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" CssClass="btn btn-outline-primary btn-sm" />
                        <asp:Button ID="btnClose_Proceso" runat="server" OnClick="btnClose_Proceso_Click" Text="Cerrar" CssClass="btn btn-outline-secondary btn-sm" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdPnlBusqProceso" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <asp:Panel ID="PnlEnvioArchivos" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; height: 180px; width: 600px; background-color:#FFFFFF;">
        <div class=" row justify-content-end" data-bs-theme="dark">
            <div class="col-1">
                <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
            </div>
        </div>

        <div class="form-group">
            <div class="input-group mx-auto pt-3">
                <input id="oFile" type="file" name="oFile" onchange="validateForSize(this,1,122880);" class="form-control" />
            </div>
        </div>
        <div>
            <br />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Button ID="BtnEnviar" runat="server" OnClick="BtnEnviar_Click" Text="Enviar" OnClientClick="showProgress()" CssClass="btn btn-outline-primary" />
                    <asp:Button ID="BtnCerrar" runat="server" OnClick="BtnCerrar_Click" Text="Cerrar" CssClass="btn btn-outline-secondary" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="PnlNewDocumentos" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 600px; background-color:#FFFFFF; text-justify:auto;">
        <div class=" row justify-content-end" data-bs-theme="dark">
            <div class="col-1">
                <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
            </div>
        </div>
            <div class="d-flex flex-row mx-4 m-0 p-0">
                <asp:Label ID="LblTpoDocumento" runat="server" Text="Tipo de documento" CssClass="form-label my-0 p-0"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                <div class="col-10">
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlTpoDocumento" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlTpoDocumento_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="d-flex flex-row mx-4 m-0 p-0">
                <asp:Label ID="LblDescripcion" runat="server" Text="Descripción del documento" CssClass="form-label my-0 p-0"></asp:Label>
            </div>
            <div class="col-10">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox>
                </div>
            </div>
<%--            
            <div class="d-flex flex-row mx-4 m-0 p-0">
                <asp:Label ID="LblFecEntrega" runat="server" Text="Fecha de Entrega" CssClass="form-label my-0 p-0"></asp:Label>
            </div>
            <div class="col-10">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="TxtFechaEntregado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaEntregado">
                        <span class="visually-hidden">Toggle Dropdown</span>
                    </button>
                    <ajaxToolkit:CalendarExtender ID="dateEntregado" runat="server" TargetControlID="TxtFechaEntregado" PopupButtonID="BtnFechaEntregado" Format="dd/MM/yyyy" />
                </div>
            </div>
            <div class="col-10">
                <div class="input-group input-group-sm m-0 p-2 ">
                    <asp:CheckBox ID="chkEntregado" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;Entregado" CssClass="" />
                </div>
            </div>
--%>
        <div class="row mt-0">
            <br />
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:Button ID="BtnAceptar_New" runat="server" OnClick="BtnAceptar_New_Click" Text="Enviar" CssClass="btn btn-outline-primary" />
                    <asp:Button ID="BtnCerrar_New" runat="server" OnClick="BtnCerrar_New_Click" Text="Cerrar" CssClass="btn btn-outline-secondary" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
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
            <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
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

                        <ajaxToolkit:ModalPopupExtender ID="mpeNewProceso" runat="server" PopupControlID="PnlDocProceso"
                            TargetControlID="LblOculto1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProceso()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="LblOculto1" runat="server" Text="Label" Style="display: none;" />

                        <ajaxToolkit:ModalPopupExtender ID="mpeNewEnvio" runat="server" PopupControlID="PnlEnvioArchivos"
                            TargetControlID="LblOculto2" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewEnvio()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="LblOculto2" runat="server" Text="Label" Style="display: none;" />

                        <ajaxToolkit:ModalPopupExtender ID="mpeNewDocumento" runat="server" PopupControlID="PnlNewDocumentos"
                            TargetControlID="LblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewDocumento()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="LblOculto3" runat="server" Text="Label" Style="display: none;" />
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
    <br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3" DynamicLayout="true">
        <ProgressTemplate>
        <div id="divImage" class ="loading">
            <div class="center">
                <img alt="Processing..." src="Images\ajax-loader.gif" />
            </div>
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="GrdTpoDocumento" />
        <asp:PostBackTrigger ControlID="BtnAceptar_New" />
        <asp:PostBackTrigger ControlID="BtnEnviar" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
