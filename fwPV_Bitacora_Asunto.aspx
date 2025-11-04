<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwPV_Bitacora_Asunto.aspx.cs" Inherits="WebItNow_Peacock.fwPV_Bitacora_Asunto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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

        function showError(message) {
            // Aquí puedes definir cómo quieres mostrar los mensajes de error
            var errorDiv = document.getElementById('errorDiv');
            if (errorDiv) {
                errorDiv.innerHTML = message;
                errorDiv.style.display = 'block';
            }

            // O puedes usar un alert
            // alert(message);
        }

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        // Evitar el envío del formulario con la tecla Enter
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

        // Función para deshabilitar los CheckBox
<%--
        function disableCheckBoxes() {
            // Seleccionar todos los CheckBox dentro del GridView
            var checkBoxes = document.querySelectorAll('#<%= GrdDocumentos.ClientID %> input[type="checkbox"]');

            // Iterar sobre cada CheckBox
            checkBoxes.forEach(function (checkBox) {
                // Prevenir el cambio de estado
                checkBox.addEventListener('click', function (event) {
                    event.preventDefault();
                    return false;
                });

                // Añadir un estilo para indicar que está deshabilitado (opcional)
                checkBox.parentNode.style.opacity = '0.9';          // Reducir la opacidad para indicar que está deshabilitado
                checkBox.parentNode.style.pointerEvents = 'none';   // Deshabilitar eventos de puntero para prevenir clics
            });
        }

        // Llamar a la función cuando la página esté completamente cargada
        document.addEventListener('DOMContentLoaded', function () {
            disableCheckBoxes();
        });
--%>

    </script>

    <style type="text/css">
        .disabled-checkbox {
            opacity: 0.7;               /* Cambia la opacidad para dar un efecto visual similar */
            background-color: #f5f5f5;  /* Fondo más claro */
            color: #333;                /* Color de texto oscuro */
            pointer-events: none;       /* Evita que los usuarios interactúen con él */
            cursor: not-allowed;        /* Cambia el cursor para indicar que no se puede hacer clic */
        }

        .uniform-controls .form-control {
            height: 38px; /* Altura uniforme para todos los TextBox */
            margin-bottom: 10px; /* Espaciado uniforme entre controles */
        }

        .uniform-controls {
            margin-bottom: 20px; /* Espaciado uniforme entre filas */
        }

        .match-input-height {
            height: 28.6px; /* altura real de form-control-sm */
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
            line-height: 1.5;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container col-lg-8 col-md-9 col-sm-10 py-5">
                <div class="row mb-4 mt-3 " style="background-color:#96E7D9; align-items: center; display: flex; height: 24px; ">
                    <div class="col-12" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl2" runat="server" Text="INFORMACIÓN GENERAL" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-6 col-md-6">
                        <div class="mb-2">
                            <asp:Label ID="LblSeguro_Cia" runat="server" Text="Compañia de Seguros" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSeguro_Cia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblSubReferencia" runat="server" Text="No. Referencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSubReferencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblNumSiniestro" runat="server" Text="No. Siniestro (Notificación)" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumSiniestro" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblNumReporte" runat="server" Text="No. Reporte" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumReporte" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblNumPoliza" runat="server" Text="No. Póliza" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumPoliza" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-6 col-md-6">
                        <div class="mb-2">
                            <asp:Label ID="LblNomCliente" runat="server" Text="Nombre del Cliente" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomCliente" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6">
                        <div class="mb-2">
                            <asp:Label ID="LblNomAsegurado" runat="server" Text="Nombre Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomAsegurado" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" onkeyup="mayus(this);" MaxLength="80"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoSiniestro" runat="server" Text="Tipo de Siniestro" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoSiniestro" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblFechaOcurrencia" runat="server" Text="Fecha Ocurrencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtFechaOcurrencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaOcurrencia">
                                <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ajaxToolkit:CalendarExtender ID="dateOcurrencia" runat="server" TargetControlID="TxtFechaOcurrencia" PopupButtonID="BtnFechaOcurrencia" Format="dd/MM/yyyy" />
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblFechaContacto" runat="server" Text="Fecha Contacto" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtFechaContacto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaContacto">
                                <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ajaxToolkit:CalendarExtender ID="dateContacto" runat="server" TargetControlID="TxtFechaContacto" PopupButtonID="BtnFechaContacto" Format="dd/MM/yyyy" />
                        </div>
                    </div>

                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblFechaInspeccion" runat="server" Text="Fecha Inspeccion" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtFechaInspeccion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaInspeccion">
                                <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ajaxToolkit:CalendarExtender ID="dateInspeccion" runat="server" TargetControlID="TxtFechaInspeccion" PopupButtonID="BtnFechaInspeccion" Format="dd/MM/yyyy" />
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblHoraInspeccion" runat="server" Text="Hora Inspección" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtHoraInspeccion" runat="server" CssClass="form-control form-control-sm" 
                                    onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                    oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                    MaxLength ="5"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-12 col-md-12">
                        <div class ="mb-2">
                            <asp:Label ID="LblDetalleReporte" runat="server" Text="Detalle del Reporte"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtDetalleReporte" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-8 col-md-8">
                        <div class ="mb-2">
                            <asp:Label ID="LblCalle" runat="server" Text="Calle" ></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCalle" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2">
                        <div class ="mb-2">
                            <asp:Label ID="LblNumExterior" runat="server" Text="Num. Exterior" ></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumExterior" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2">
                        <div class ="mb-2">
                            <asp:Label ID="LblNumInterior" runat="server" Text="Num. Interior" ></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumInterior" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblEstado" runat="server" Text="Estado" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblDelegacion" runat="server" Text="Delegación / Municipio" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlMunicipios" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipios_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblColonia" runat="server" Text="Colonia" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtColonia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-1 col-md-1 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblCodigoPostal" runat="server" Text="C.Postal" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCodigoPostal" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-4 mt-3 " style="background-color:#96E7D9; align-items: center; display: flex; height: 24px; ">
                    <div class="col-12" style="padding-left: 14px;">
                        <asp:Label ID="Label1" runat="server" Text="DATOS DE CONTACTO" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-6 col-md-6">
                        <div class="mb-2">
                            <asp:Label ID="LblEmailContacto1" runat="server" Text="Correo Electronico del Contacto" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEmailContacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblTel1_Contacto1" runat="server" Text="Telefono 1" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTel1_Contacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-3 col-md-3 ">
                        <div class="mb-2">
                            <asp:Label ID="LblTel2_Contacto1" runat="server" Text="Telefono 2" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTel2_Contacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="d-grid gap-2 gap-md-3 d-md-flex justify-content-md-center mt-5 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="BtnAnularPnl2" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl2_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                            <asp:Button ID="btnEditarPnl2" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl2_Click" CssClass="btn btn-primary" TabIndex="1"/>
                            <asp:Button ID="btnActualizarPnl2" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl2_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                            <asp:Button ID="BtnRegresar" runat="server" Text="&nbsp;&nbsp;&nbsp;Regresar&nbsp;&nbsp;&nbsp;" OnClick="BtnRegresar_Click" CssClass="btn btn-secondary" CausesValidation="false" TabIndex="3"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="row mb-4 mt-3 " style="background-color:#96E7D9; align-items: center; display: flex; height: 24px; ">
                    <div class="col-12" style="padding-left: 14px;">
                        <asp:Label ID="LbDescGeneral" runat="server" Text="DESCRIPCIÓN GENERAL" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblEquipo" runat="server" Text="Equipo" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEquipo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblMarca" runat="server" Text="Marca" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtMarca" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblSerie" runat="server" Text="No. Serie" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSerie" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblModelo" runat="server" Text="Modelo" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtModelo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                </div>

                <div class="row mt-3">
                    <div class="col-lg-12 col-md-12">
                        <div class="mb-2">
                            <asp:Label ID="LblCondiciones" runat="server" Text="Condiciones del Equipo" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCondiciones" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div  class="row mt-3">
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblContraseña" runat="server" Text="Contraseña" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtContraseña" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblTpoFolio" runat="server" Text="Tipo de Folio" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlTpoFolio" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoFolio_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2 ">
                        <div class ="mb-2">
                            <asp:Label runat="server" CssClass="form-label">&nbsp;</asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:Button ID="BtnNvoFolio" runat="server" Text="Nuevo Folio" OnClick="BtnNvoFolio_Click" CssClass="btn btn-primary match-input-height" />
                        </div>
                    </div>
                </div>

                <!-- Botón Guardar y Actualizar para el panel 1 -->
                <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="BtnAnularPnl1" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl1_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                            <asp:Button ID="btnEditarPnl1" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl1_Click" CssClass="btn btn-primary" TabIndex="1"/>
                            <asp:Button ID="btnActualizarPnl1" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl1_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="row mt-5">
                    <%-- Consulta de Folios --%>
                    <div style="overflow-x: auto; overflow-y:hidden">
                        <asp:GridView ID="GrdFolios"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                OnPageIndexChanging="GrdFolios_PageIndexChanging" OnRowCommand="GrdFolios_RowCommand" OnPreRender="GrdFolios_PreRender"
                                OnSelectedIndexChanged="GrdFolios_SelectedIndexChanged" OnRowDataBound="GrdFolios_RowDataBound" 
                                DataKeyNames="IdConsecutivo" PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                <AlternatingRowStyle CssClass="alt autoWidth" />
                                <Columns>
                                    <asp:BoundField DataField="Foliar" HeaderText="Folio" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Equipo" HeaderText="Equipo" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Marca" HeaderText="Marca" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Serie" HeaderText="No. Serie" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Modelo" HeaderText="Modelo">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Condiciones" HeaderText="Condiciones del Equipo">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Contraseña" HeaderText="Contraseña">
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgSubRef_Add" runat="server" OnClick="ImgSubRef_Add_Click" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png" Enabled="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgSubRef_Del" runat="server" OnClick="ImgSubRef_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png" Enabled="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Folio" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SubFolio" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TpoFolio" >
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                </div>

                <div class="row mb-4 mt-3 " style="background-color:#96E7D9; align-items: center; display: flex; height: 24px; ">
                    <div class="col-12" style="padding-left: 14px;">
                        <asp:Label ID="LblDocumentacion" runat="server" Text="DOCUMENTACIÓN" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <div class="d-grid gap-2 gap-md-3 d-md-flex justify-content-md-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="BtnDeclaracion"  runat="server" Text="Declaración" OnClick="BtnDeclaracion_Click" CssClass="btn btn-primary" TabIndex="1"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
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
                <asp:Button ID="BtnAceptar" runat="server" OnClick="BtnAceptar_Click" Text="Aceptar" CssClass="btn btn-outline-primary mx-1" />
                <asp:Button ID="BtnCancelar" runat="server" OnClick="BtnCancelar_Click" Text="Cancelar" CssClass="btn btn-outline-secondary mx-1" />
                <asp:Button ID="BtnCerrar" runat="server" OnClick="BtnCerrar_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlMensaje_2" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
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
                <asp:Label ID="LblMessage_2" runat="server" Text="" />
            </div>
            <div>
                <br />
                <hr class="dropdown-divider" />
            </div>
        
            <div class="d-flex justify-content-center mb-3">
                <br />
                <asp:Button ID="BtnAceptar_Del_Doc" runat="server" OnClick="BtnAceptar_Del_Doc_Click" Text="Aceptar" CssClass="btn btn-outline-primary mx-1" />
                <asp:Button ID="BtnCancelar_Del_Doc" runat="server" OnClick="BtnCancelar_Del_Doc_Click" Text="Cancelar" CssClass="btn btn-outline-secondary mx-1" />
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlMensaje_3" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
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
                <asp:Label ID="LblMessage_3" runat="server" Text="" />
            </div>
            <div>
                <br />
                <hr class="dropdown-divider" />
            </div>
        
            <div class="d-flex justify-content-center mb-3">
                <br />
                <asp:Button ID="BtnAceptar_CrearCuaderno" runat="server" OnClick="BtnAceptar_CrearCuaderno_Click" Text="Aceptar" CssClass="btn btn-outline-primary mx-1" />
                <asp:Button ID="BtnCancelar_CrearCuaderno" runat="server" OnClick="BtnCancelar_CrearCuaderno_Click" Text="Cancelar" CssClass="btn btn-outline-secondary mx-1" />
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
                        <div>
                            <div class="form-group">
                                <div class="d-grid col-6 mx-auto">
                                    <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                                        TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="form-group">
                                <div class="d-grid col-6 mx-auto">
                                    <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_2" runat="server" PopupControlID="pnlMensaje_2"
                                        TargetControlID="lblOculto_2" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Label ID="lblOculto_2" runat="server" Text="Label" Style="display: none;" />
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="form-group">
                                <div class="d-grid col-6 mx-auto">
                                    <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_3" runat="server" PopupControlID="pnlMensaje_3"
                                        TargetControlID="lblOculto_3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Label ID="lblOculto_3" runat="server" Text="Label" Style="display: none;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>

                </td>
                <td class="style3">
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="BtnAnularPnl1" />
        <asp:PostBackTrigger ControlID="btnEditarPnl1" />
        <asp:PostBackTrigger ControlID="btnActualizarPnl1" />

        <asp:PostBackTrigger ControlID="BtnAnularPnl2" />
        <asp:PostBackTrigger ControlID="btnEditarPnl2" />
        <asp:PostBackTrigger ControlID="btnActualizarPnl2" />

        <asp:PostBackTrigger ControlID="BtnDeclaracion" />
        
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
