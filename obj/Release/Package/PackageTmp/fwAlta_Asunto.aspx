<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwAlta_Asunto.aspx.cs" Inherits="WebItNow_Peacock.fwAlta_Asunto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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

        window.onload = function () {
            // Inicia un temporizador para ejecutar después de 30 minutos (1800000 milisegundos)
            var timer = setTimeout(function () {
                // Actualiza el contenido del elemento para mostrar que la sesión ha expirado
                document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
        
                // Encuentra el modal y lo muestra
                var modalId = '<%=mpeExpira.ClientID%>';
                var modal = $find(modalId);
                modal.show();

                // Inicia otro temporizador para recargar la página después de 30 minutos
                setTimeout(function () {
                    location.reload();
                }, 1800000);

            }, 1800000);
        };

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        function mpeNewEnvio() {
            //
        }

        function mpeAddArchivo() {
            //
        }

        document.onkeydown = function (evt) {
            return (evt ? evt.which : event.keyCode) != 13;
        }

        function Campo_Obligatorio(source, arguments) {
            var valor = arguments.value;
            if (valor == "-- Seleccionar --") {
                arguments.IsValid = false;
                return;
            } else {
                arguments.IsValid = true;
                return;
            }
        }

        function cerrarPanel() {
            // Ocultar el panel cuando se hace clic en el botón de cierre
            document.getElementById('<%= pnlMensaje.ClientID %>').style.display = 'none';
            // Redirigir a la misma página
            window.location.href = window.location.href;
            return false; // Evitar que el botón cause un postback
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

        <div class="container col-md-4 " >
            <div class="form-floating mt-3 py-5">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h5 class="h6 fw-normal my-1" style="font-size:small">ALTA DE NUEVA REFERENCIA</h5>
                </div>

                <div class="form-floating mt-3">
                    <asp:Label ID="LblCiaSeguros" runat="server" Text="Compañia de Seguros"></asp:Label>
                        <asp:CustomValidator ID="cvCiaSeguros" runat="server" ControlToValidate="ddlCiaSeguros" 
                        OnServerValidate ="cvCiaSeguros_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>
                    <asp:DropDownList ID="ddlCiaSeguros" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlCiaSeguros_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="2">
                    </asp:DropDownList>
                </div>

                <div class="form-floating mt-3">
                    <asp:Label ID="LblProyecto" runat="server" Text="Proyecto" CssClass="control-label" Font-Size="Small"></asp:Label>
                        <asp:CustomValidator ID="cvProyecto" runat="server" ControlToValidate="ddlProyecto" 
                        OnServerValidate ="cvProyecto_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>
                    <asp:DropDownList ID="ddlProyecto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" Width="100%">
                    </asp:DropDownList>
                </div>

                <div class="form-group mt-4">
                <div id="divCliente" runat="server" visible="false">
                    <asp:Label ID="LblNomCliente" runat="server" Text="Nombre del Cliente" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False" ></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvCliente" runat="server"
                        ControlToValidate="TxtNomCliente" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtNomCliente" runat="server" CssClass="form-control" placeholder="Nombre del Cliente" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
                </div>  

                <div class="form-floating mt-3">
                    <asp:Label ID="LblTipoAsunto" runat="server" Text="Tipo de Asunto" ></asp:Label>
                        <asp:CustomValidator ID="cvTipoAsunto" runat="server" ControlToValidate="ddlTipoAsunto" 
                        OnServerValidate ="cvTipoAsunto_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>
                    <asp:DropDownList ID="ddlTipoAsunto" runat="server" CssClass="btn btn-outline-secondary text-start"  OnSelectedIndexChanged="ddlTipoAsunto_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="4">
                    </asp:DropDownList>
                </div>

                <div class="form-group mt-4">
                    <asp:Label ID="LblFechaAsignacion" runat="server" Text="Fecha de Asignación" CssClass="control-label" Font-Size="Small"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvFechaInput" runat="server"
                        ControlToValidate="TxtFechaInput" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12 md-form md-outline input-with-post-icon datepicker">
                        <asp:TextBox ID="TxtFechaInput" runat="server" TextMode="Date" placeholder="Select date" style="font-size:16px;" class="form-control form-control-sm" TabIndex="1" />
                    </div>
                </div>

                <div class="form-group mt-4">
                    <div id="divNomActor" runat="server" visible="true">
                        <asp:Label ID="LblNomActor" runat="server" Text="Nombre del Actor" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvNomActor" runat="server"
                            ControlToValidate="TxtNomActor" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:TextBox ID="TxtNomActor" runat="server" CssClass="form-control" placeholder="Nombre del Actor" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" TabIndex="5"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <div id="divDemandado" runat="server" visible="true">
                        <asp:Label ID="LblDemandado" runat="server" Text="Nombre del Demandado" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvDemandado" runat="server"
                            ControlToValidate="TxtDemandado" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:TextBox ID="TxtDemandado" runat="server" CssClass="form-control" placeholder="Nombre del Demandado" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" TabIndex="6"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <div id="divNumSiniestro" runat="server" visible="true">
                        <asp:Label ID="LblNumSiniestro" runat="server" Text="Número de Siniestro (Notificación)" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvNumSiniestro" runat="server"
                            ControlToValidate="TxtNumSiniestro" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:TextBox ID="TxtNumSiniestro" runat="server" CssClass="form-control" placeholder="Número de Siniestro" AutoComplete="off" onkeyup="mayus(this);" MaxLength="250" TabIndex="7"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <div id="divNumPoliza" runat="server" visible="true">
                        <asp:Label ID="LblNumPoliza" runat="server" Text="Número de Póliza" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvNumPoliza" runat="server"
                            ControlToValidate="TxtNumPoliza" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:TextBox ID="TxtNumPoliza" runat="server" CssClass="form-control" placeholder="Número de Póliza" AutoComplete="off" onkeyup="mayus(this);" MaxLength="20" TabIndex="8"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <div id="divNumReporte" runat="server" visible="false">
                        <asp:Label ID="LblNumReporte" runat="server" Text="Número de Reporte" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvNumReporte" runat="server"
                            ControlToValidate="TxtNumReporte" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:TextBox ID="TxtNumReporte" runat="server" CssClass="form-control" placeholder="Número de Reporte" AutoComplete="off" onkeyup="mayus(this);" MaxLength="20" TabIndex="9"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <div id="divNomAsegurado" runat="server" visible="true">
                        <asp:Label ID="LblNomAsegurado" runat="server" Text="Nombre del Asegurado" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvNomAsegurado" runat="server"
                            ControlToValidate="TxtNomAsegurado" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:TextBox ID="TxtNomAsegurado" runat="server" CssClass="form-control" placeholder="Nombre del Asegurado" AutoComplete="off" onkeyup="mayus(this);" MaxLength="100" TabIndex="10"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-floating mt-3">
                    <div id="divTecResponsable" runat="server" visible="true">
                        <asp:Label ID="LblRespTecnico" runat="server" Text="Responsable Técnico" ></asp:Label>
                            <asp:CustomValidator ID="cvTecResponsable" runat="server" ControlToValidate="ddlRespTecnico" 
                            OnServerValidate ="cvTecResponsable_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                            </asp:CustomValidator>
                        <asp:DropDownList ID="ddlRespTecnico" runat="server" CssClass="btn btn-outline-secondary text-start" Width="100%" TabIndex="11">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-floating mt-3">
                    <asp:Label ID="LblRespAdministrativo" runat="server" Text="Responsable Administrativo" ></asp:Label>
                        <asp:CustomValidator ID="cvAdminResponsable" runat="server" ControlToValidate="ddlRespAdministrativo" 
                        OnServerValidate ="cvAdminResponsable_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>
                    <asp:DropDownList ID="ddlRespAdministrativo" runat="server" CssClass="btn btn-outline-secondary text-start" Width="100%" TabIndex="12">
                    </asp:DropDownList>
                </div>

                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnEnviar" runat="server" Text="Generar" OnClick="BtnEnviar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="13"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--<asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-primary" TabIndex="11"/>--%>
                </div>
            </div>
        </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnEnviar" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
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
            <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary" CausesValidation="false"/>
            <%--OnClientClick="cerrarPanel();" --%>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlMensaje_1" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
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
            <asp:Label ID="LblMessage_1" runat="server" Text="" />
        </div>
        <div>
            <br />
            <hr class="dropdown-divider" />
        </div>
        
        <div class="d-flex justify-content-center mb-3">
            <br />
            <asp:Button ID="BtnContinuar" runat="server" OnClick="BtnContinuar_Click" Text="Continuar" CssClass="btn btn-outline-primary mx-1" />
            <asp:Button ID="BtnCancelar" runat="server" OnClick="BtnCancelar_Click" Text="Cancelar" CssClass="btn btn-outline-secondary mx-1" />
            <asp:Button ID="BtnCerrar" runat="server" OnClick="BtnCerrar_Click" Text="Cerrar" Visible="false" CssClass="btn btn-outline-primary"/>
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
            <td>
                <div class="form-group">
                    <div class="d-grid col-6 mx-auto">
                        <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                            TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                    TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td class="style3"><asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
