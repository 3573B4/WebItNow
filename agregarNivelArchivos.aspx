<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="agregarNivelArchivos.aspx.cs" Inherits="WebItNow.agregarNivelArchivos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <meta name="description" content="@Html.Raw(ViewBag.description)" />
    

    <title>Personalizar Cuadernillo</title>
    <link rel="website icon" type="png" href="Images/banner_Codice_mediano.png" />

    <script language="javascript" type="text/javascript">
        if (history.forward(1)) {
            location.replace(history.forward(1))
        }

        function mayus(e) {
            e.value = e.value.toUpperCase();
        }

        $(document).ready(function () {
            $("input").attr("autocomplete", "off");
        });

        <%--var timer = setTimeout(function () {
            document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
            var modalId = '<%=mpeExpira.ClientID%>';
            var modal = $find(modalId);
            modal.show();
        }, 3600000);--%>

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }
        
        function mpeAddMensajeOnOk() {
            //
        }

        function mpeNewEnvio() {
            //
        }

        function mpeNewNota() {
            //
        }

        document.onkeydown = function (evt) {
            return (evt ? evt.which : event.keyCode) != 13;
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

    <!-- Estilo para Agregar THEAD y TBODY a GridView. -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/css/footable.min.css" rel="stylesheet" type="text/css" />

    <%--<link href="~/Styles/center_controls.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/Styles/default.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/panel_message.css" rel="stylesheet" type="text/css" />
    <%--<link href="~/Styles/GridView.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/Styles/fuentes.css" rel="stylesheet" type="text/css" />
    <link href="Styles/pagination-ys.css" rel="stylesheet" type="text/css" />

</head>
<body >
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="col-12 w-100" style="/*background-color: white;*/ 
                                              box-sizing: border-box; font-family: 'poppons', sans-serif;
                                              width:100%; height:100vh; display:flex; justify-content:center; align-content:center;">
                    <div class="container col-lg-9 col-md-11 col-sm-11">
                        <div class="row col-lg-9 col-md-11 mb-3 ps-3" style="width: 100%; padding-top:60px;">
                            <div class="col-md-6 mb-3"  >
                                <div class="row">
                                    <div class="col-10">
                                        <asp:Label ID="LblAseguradora" runat="server" Text="Aseguradora" CssClass="control-label"></asp:Label>
                                        <asp:DropDownList ID="ddlAseguradora" runat="server" OnSelectedIndexChanged="ddlAseguradora_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-2" style="display:flex; align-items:end; justify-content:end;">
                                            <asp:Button ID="BtnAddAseguradora" runat="server" Text="+" OnClick="BtnAddAseguradora_Click" CssClass="btn btn-primary btn-sm" />
                                        <%-- <asp:Button ID="BtnAddAseguradora" runat="server" Text="+" OnClick="BtnAddAseguradora_Click" CssClass="btn btn-primary btn-sm" /> --%>
                                    </div>
                                </div>
                                
                            </div>
                            <div class="col-md-6 mb-3">
                                <div class="row">
                                    <div class="col-10">
                                        <asp:Label ID="LblConclusion" runat="server" Text="Conclusion" CssClass="control-label"></asp:Label>
                                        <asp:DropDownList ID="ddlConclusion" runat="server" OnSelectedIndexChanged="ddlConclusion_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-2" style="display:flex; align-items:end; justify-content:end;">
                                        <asp:Button ID="BtnAddConclusion" runat="server" Text="+"  CssClass="btn btn-primary btn-sm" />
                                    </div>
                                </div>
                                
                            </div>
                        </div>

                        <div class="row col-lg-9 col-md-11 mb-3 ps-3" style="width:100%">
                            <div class="col-md-6 mb-3">
                                <div class="row py-2" style="align-items: baseline;">  <%--center--%>
                                    <div class="col-10" style="padding-left: 14px;">
                                        <asp:Label ID="LblNivel1" runat="server" Text="Nivel 1" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                    </div>
                                    <div class="col-2" style="display:flex; justify-content: end;">
                                        <asp:Button ID="BtnAddNivel1" runat="server" Text="+"  CssClass="btn btn-primary btn-sm" />
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div style="overflow-x:auto; overflow-y:hidden;">
                                        <asp:GridView ID="GrdNivel1" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="true" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt"
                                            PageSize="5" DataKeyNames="IdNivel1" 
                                            OnPageIndexChanging="GrdNivel1_PageIndexChanging" OnRowCommand="GrdNivel1_RowCommand" 
                                            OnSelectedIndexChanged="GrdNivel1_SelectedIndexChanged" OnRowDataBound="GrdNivel1_RowDataBound">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdNivel1" HeaderText="Id1">
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="nomNivel1" HeaderText="Nombre Nivel 1">
                                                    <ItemStyle Width="250px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgAceptado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/aceptar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgRechazado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/rechazar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row pt-3 pb-2" style="align-items: baseline;">  <%--center--%>
                                    <div class="col-10" style="padding-left: 14px;">
                                        <asp:Label ID="LblNivel2" runat="server" Text="Nivel 2" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                    </div>
                                    <div class="col-2" style="display:flex; justify-content: end;">
                                        <asp:Button ID="BtnAddNivel2" runat="server" Text="+"  CssClass="btn btn-primary btn-sm" />
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div style="overflow-x:auto; overflow-y:hidden;">
                                        <asp:GridView ID="GrdNivel2" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="true" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt"
                                            PageSize="5" DataKeyNames="IdNivel2"
                                            OnRowDataBound="GrdNivel2_RowDataBound" OnRowCommand="GrdNivel2_RowCommand" OnSelectedIndexChanged="GrdNivel2_SelectedIndexChanged">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdNivel2" HeaderText="Id2">
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="nomNivel2" HeaderText="Nombre Nivel 2">
                                                    <ItemStyle Width="250px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgAceptado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/aceptar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgRechazado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/rechazar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row pt-3 pb-2" style="align-items: baseline;">  <%--center--%>
                                    <div class="col-10" style="padding-left: 14px;">
                                        <asp:Label ID="LblNivel3" runat="server" Text="Nivel 3" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                    </div>
                                    <div class="col-2" style="display:flex; justify-content: end;">
                                        <asp:Button ID="BtnAddNivel3" runat="server" Text="+"  CssClass="btn btn-primary btn-sm" />
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div style="overflow-x:auto; overflow-y:hidden;">
                                        <asp:GridView ID="GrdNivel3" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="true" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt"
                                            PageSize="5">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdNivel3" HeaderText="Id2">
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="nomNivel3" HeaderText="Nombre Nivel 3">
                                                    <ItemStyle Width="250px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgAceptado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/aceptar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgRechazado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/rechazar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row pt-3 pb-2" style="align-items: baseline;">  <%--center--%>
                                    <div class="col-10" style="padding-left: 14px;">
                                        <asp:Label ID="LblNivel4" runat="server" Text="Nivel 4" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                    </div>
                                    <div class="col-2" style="display:flex; justify-content: end;">
                                        <asp:Button ID="BtnAddNivel4" runat="server" Text="+"  CssClass="btn btn-primary btn-sm" />
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div style="overflow-x:auto; overflow-y:hidden;">
                                        <asp:GridView ID="GrdNivel4" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="true" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt"
                                            PageSize="5">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdNivel4" HeaderText="Id2">
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="nomNivel4" HeaderText="Nombre Nivel 4">
                                                    <ItemStyle Width="250px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgAceptado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/aceptar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgRechazado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/rechazar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 mb-3">
                                <div class="row py-2" style="align-items: baseline;">  <%--center--%>
                                    <div class="col-10" style="padding-left: 14px;">
                                        <asp:Label ID="LblArchivos" runat="server" Text="Archivos" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                    </div>
                                    <div class="col-2" style="display:flex; justify-content: end;">
                                        <asp:Button ID="BtnArchivos" runat="server" Text="+"  CssClass="btn btn-primary btn-sm" />
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                    </div>
                                </div>
                                <div style="overflow-x: auto; overflow-y:hidden;">
                                    <asp:GridView ID="GridArchivosPaq" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="true" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt"
                                            PageSize="10">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:BoundField DataField="IdArchivo" HeaderText="Id" >
                                                <ItemStyle Width="50" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nomArchivo" HeaderText="Nombre" >
                                                <ItemStyle Width="250" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Extencion" HeaderText="Extencion" >
                                                <ItemStyle Width="100" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgAceptado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/aceptar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgRechazado" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/rechazar_new.png" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="d-grid col-6 mx-auto">
                                <ajaxtoolkit:modalpopupextender id="ModalPopupExtender1" runat="server" popupcontrolid="pnlMensaje"
                                    targetcontrolid="lblOculto" backgroundcssclass="FondoAplicacion" onokscript="mpeMensajeOnOk()">
                                </ajaxtoolkit:modalpopupextender>
                                <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>


                
                <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" Style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
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
                        <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlExpira" runat="server" CssClass="CajaDialogo" Style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
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
                        <asp:Button ID="BtnClose_Expira" OnClientClick="acceso(); return false;" runat="server" Text="Cerrar" CssClass="btn btn-outline-primary" />
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlAdd" runat="server" CssClass="CajaDialogo" Style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
                    <div class=" row justify-content-end" data-bs-theme="dark">
                        <asp:Label ID="LblTitlePnlAdd" runat="server" Text=""></asp:Label>
                        <div class="col-1">
                            <asp:Button runat="server" OnClick="Unnamed_Click" type="button" class="btn-close" aria-label="Close" />
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
                        <asp:Label ID="LblMsgPnlAdd" runat="server" Text="" />
                    </div>
                    <div>
                        <br />
                        <hr class="dropdown-divider" />
                    </div>
                    <div>
                        <br />
                        <asp:Button ID="BtnClousePnlAdd" OnClientClick="Unnamed_Click" runat="server" Text="Cerrar" CssClass="btn btn-outline-primary" />
                    </div>
                </asp:Panel>

                <div class="form-group">
                    <div class="d-grid col-6 mx-auto">
                        <ajaxtoolkit:modalpopupextender id="mpeMensaje" runat="server" popupcontrolid="pnlMensaje"
                            targetcontrolid="lblOculto" backgroundcssclass="FondoAplicacion" onokscript="mpeMensajeOnOk()">
                        </ajaxtoolkit:modalpopupextender>
                        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />

                        <ajaxtoolkit:modalpopupextender id="mpeNewEnvio" runat="server" popupcontrolid="PnlEnvioArchivos"
                            targetcontrolid="LblOculto2" backgroundcssclass="FondoAplicacion" onokscript="mpeNewEnvio()">
                        </ajaxtoolkit:modalpopupextender>
                        <asp:Label ID="LblOculto2" runat="server" Text="Label" Style="display: none;" />
                        <ajaxToolkit:ModalPopupExtender ID="mpeAddMensaje" runat="server" PopupControlID="pnlAdd"
                            TargetControlID="lblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeAddMensajeOnOk">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblOculto3" runat="server" Text="label" Style="display: none;" />
                        <%--
                            <ajaxToolkit:ModalPopupExtender ID="mpeNewNota" runat="server" PopupControlID="PnlNewNotas"
                                TargetControlID="LblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewNota()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="LblOculto3" runat="server" Text="Label" Style="display: none;" />
                        --%>
                    </div>
                </div>

                <%--<ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                        TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                    </ajaxToolkit:ModalPopupExtender>--%>
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
                <asp:PostBackTrigger ControlID="BtnAddAseguradora" />
            </Triggers>
        </asp:UpdatePanel>
        
    </form>
</body>
</html>
