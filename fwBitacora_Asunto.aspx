<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwBitacora_Asunto.aspx.cs" Inherits="WebItNow_Peacock.fwBitacora_Asunto" %>
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

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Elemento HTML para mostrar errores -->
            <div id="errorDiv" style="display:none; color:red;"></div>

            <%--<div class="container col-lg-7 col-md-8 col-sm-8 py-5">--%>
            <div class="container col-lg-8 col-md-9 col-sm-10 py-5">
                <div class="row mb-4 mt-3 " style="background-color:#96E7D9; align-items: center; display: flex; height: 24px; ">
                    <div class="col-12" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl2" runat="server" Text="INFORMACIÓN GENERAL" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblSeguro_Cia" runat="server" Text="Compañia de Seguros" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSeguro_Cia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblSubReferencia" runat="server" Text="No. Referencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSubReferencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblProducto" runat="server" Text="Producto" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <%--<asp:TextBox ID="TxtProducto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>--%>
                                <asp:DropDownList ID="ddlProductos" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProductos_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblAntReferencia" runat="server" Text="Referencia Anterior" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtAntReferencia" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblNumReporte" runat="server" Text="No. Reporte" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumReporte" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
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
                    <div class="col-lg-3 col-md-3">
                        <div class="mb-2">
                            <asp:Label ID="LblNumPoliza" runat="server" Text="No. Póliza" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumPoliza" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnlEstadoOcurrencia" runat="server">
                    <div class="row mt-3">
                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
                                <asp:Label ID="LblEstOcurrencia" runat="server" Text="Estado de Ocurrencia" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtEstOcurrencia" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-5 col-md-5">
                            <div class="mb-2">
                                <asp:Label ID="LblDescMote" runat="server" Text="Descripción Mote" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtDescMote" runat="server" CssClass="form-control form-control-sm" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <div class="row mt-3">
                    <div class="col-lg-6 col-md-6">
                        <div class="mb-2">
                            <asp:Label ID="LblNomAsegurado" runat="server" Text="Nombre Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomAsegurado" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" onkeyup="mayus(this);" MaxLength="80"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div>
                <asp:Panel ID="pnl2" runat="server" Visible="true">
                    <div class="row mt-3">
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
                                <asp:Label ID="LblFechaReporte" runat="server" Text="Fecha Reporte" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtFechaReporte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaReporte">
                                    <span class="visually-hidden">Toggle Dropdown</span>
                                </button>
                                <ajaxToolkit:CalendarExtender ID="dateReporte" runat="server" TargetControlID="TxtFechaReporte" PopupButtonID="BtnFechaReporte" Format="dd/MM/yyyy" />
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
                                <asp:Label ID="LblFechaAsignacion" runat="server" Text="Fecha Asignacion" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtFechaAsignacion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaAsignacion">
                                    <span class="visually-hidden">Toggle Dropdown</span>
                                </button>
                                <ajaxToolkit:CalendarExtender ID="dateAsignacion" runat="server" TargetControlID="TxtFechaAsignacion" PopupButtonID="BtnFechaAsignacion" Format="dd/MM/yyyy" />
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
                                <asp:Label ID="LblHoraAsignacion" runat="server" Text="Hora Asignación" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtHoraAsignacion" runat="server" CssClass="form-control form-control-sm" 
                                      onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                      oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                      MaxLength ="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
                            </div>
                            <div class="input-group input-group-sm">
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

                    <br />

                    <div>
                        <asp:Panel ID="pnlDependencia" runat="server" >
                                <div class="row mb-4 mt-3 " style="background-color:#96E7D9; align-items: center; display: flex; height: 24px; ">
                                    <div class="col-10" style="padding-left: 14px;">
                                        <asp:Label ID="LblEtiquetaPnl18" runat="server" Text="DETALLES" CssClass="control-label" Font-Size="small"></asp:Label>
                                    </div>
                                    <div class="col-2" style="display:flex; justify-content: end;">
                                        <div>
                                            <asp:Button ID="btnShowPanel18" runat="server" Text="&#9660;" OnClick="btnShowPanel18_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                        </div>
                                    </div>
                                </div>
                        </asp:Panel>

                        <%-- AZTECA --%>
                        <asp:Panel ID="pnl18" runat="server" Visible="false" >
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNomRep_1" runat="server" Text="Nombre del Representante (1)" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNomRep_1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-6 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPtoRep_1" runat="server" Text="Puesto del Representante (1)" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPtoRep_1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNomRep_2" runat="server" Text="Nombre del Representante (2)" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNomRep_2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-6 col-md-6 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPtoRep_2" runat="server" Text="Puesto del Representante (2)" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPtoRep_2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-12 col-md-12">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblDetDivision" runat="server" Text="División Gubernamental"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtDetDivision" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNomAfectado_1" runat="server" Text="Nombre Afectado (1)" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNomAfectado_1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

<%--
                                <div class="col-lg-6 col-md-6 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNomAfectado_2" runat="server" Text="Nombre Afectado (2)" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNomAfectado_2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
--%>

                                <div class="col-lg-6 col-md-6">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblNomProveedor" runat="server" Text="Nombre de Proveedor"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNomProveedor" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblDependencia" runat="server" Text="Dependencia"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtDependencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>

                        <%-- BERKLEY --%>
                        <asp:Panel ID="pnl19" runat="server" Visible="false" >
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNomBeneficiario" runat="server" Text="Nombre Beneficiario" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNomBeneficiario" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-6 ">
                                    <div class="mb-2">
                                    </div>
                                    <div class="input-group input-group-sm">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>

                    <!-- Botón Guardar y Actualizar para el panel 2 -->
                    <%--<div class="d-grid gap-4 d-flex justify-content-center mt-4 mb-4">--%>
                    <div class="d-grid gap-2 gap-md-3 d-md-flex justify-content-md-center mt-2 mb-3">
                        <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnAnularPnl2" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl2_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                <asp:Button ID="btnEditarPnl2" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl2_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                <asp:Button ID="btnActualizarPnl2" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl2_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                <asp:Button ID="BtnRegresar" runat="server" Text="&nbsp;&nbsp;&nbsp;Regresar&nbsp;&nbsp;&nbsp;" OnClick="BtnRegresar_Click" CssClass="btn btn-secondary" CausesValidation="false" TabIndex="3"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </asp:Panel>
                </div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl1" runat="server" Text="CONFIGURACIÓN DEL SINIESTRO" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">--%>
                                <%--<ContentTemplate>--%>
                                    <asp:Button ID="btnShowPanel1" runat="server" Text="&#9660;" OnClick="btnShowPanel1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                <%--</ContentTemplate>--%>
                            <%--</asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>
                <div>
                <asp:Panel ID="pnl1" runat="server" Visible="true">
                    <div class="row mb-3">
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblTpoAsegurado" runat="server" Text="Tipo de Asegurado" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlTpoAsegurado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoAsegurado_SelectedIndexChanged" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblStatusSiniestro" runat="server" Text="Estatus del Siniestro" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlEstSiniestro" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstSiniestro_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblEtapas" runat="server" Text="Estatus Etapas" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlConclusion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlConclusion_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                            <!-- Aquí se agrega el UpdatePanel debajo de ddlConclusion -->
                            <div class="mt-2">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="d-flex align-items-center">
                                        <asp:ImageButton ID="ImgDel_Documento" runat="server" OnClick="ImgDel_Documento_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png" Enabled="true" />
                                        <span class="ms-2">Eliminar Documento(s)</span>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <br />                
                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblRiesgos" runat="server" Text="RIESGOS" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                    </div>

                    <div class="row mb-2">
                        <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="grdSeccion_2" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                        AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_2_RowDataBound" >
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:BoundField DataField="Columna1" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_2_1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Columna2" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_2_2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Columna3" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_2_3" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grdSeccion_2" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblBienes" runat="server" Text="BIENES" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                    </div>

                    <div class="row mb-2">
                        <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="grdSeccion_4" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                        AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_4_RowDataBound">
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:BoundField DataField="Columna1" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_4_1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Columna2" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_4_2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Columna3" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_4_3" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grdSeccion_4" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                        <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblOtros" runat="server" Text="OTROS DETALLES" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_5" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                    AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                    AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_5_RowDataBound" >
                                    <AlternatingRowStyle CssClass="alt autoWidth" />
                                    <Columns>
                                        <asp:BoundField DataField="Columna1" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_5_1" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Columna2" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_5_2" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Columna3" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_5_3" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_5" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnGraba_Categorias" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGraba_Categorias_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                <asp:Button ID="BtnCrear_Cuaderno" runat="server" Text="Crear Cuaderno" Font-Bold="True" OnClick="BtnCrear_Cuaderno_Click" CssClass="btn btn-secondary" TabIndex="2"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                </div>
                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl0" runat="server" Text="DOCUMENTACIÓN" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <%--<asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">--%>
                                <%--<ContentTemplate>--%>
                                    <asp:Button ID="btnShowPanel0" runat="server" Text="&#9660;" OnClick="btnShowPanel0_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                <%--</ContentTemplate>--%>
                            <%--</asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>
                <div>
                <asp:Panel ID="pnl0" runat="server" Visible="false">
                    <div class="row mb-2">
                        <%--<div style="overflow-x: auto; overflow-y:hidden">--%>
<%--                            
                            <asp:GridView ID="GrdDocumentos"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="99%"
                                    AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                    OnPageIndexChanging="GrdDocumentos_PageIndexChanging" OnRowCommand="GrdDocumentos_RowCommand"
                                    OnSelectedIndexChanged="GrdDocumentos_SelectedIndexChanged" OnRowDataBound ="GrdDocumentos_RowDataBound"
                                    DataKeyNames="IdDoc_Categoria" PageSize="8" Font-Size="Smaller" >
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:BoundField DataField="TpoArchivo" HeaderText="Tipo de Archivo" >
                                        <ItemStyle Width="125px" /> 
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción del documento" >
                                        <ItemStyle Width="825px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fec_Entrega" DataFormatString="{0:d}" HeaderText="Fecha de entrega" >
                                        <ItemStyle Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DocInterno" >
                                        </asp:BoundField>    
                                        <asp:TemplateField HeaderText="Entregado" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                            <ItemStyle Width="125px" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxRow" runat="server" Checked='<%# Convert.ToBoolean(Eval("IdDescarga")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                            </asp:GridView>
--%>
                        <div style="overflow-y: auto; max-height: 330px; ">

                            <asp:GridView ID="GrdDocumentos"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="99%"
                                    CssClass="table table-responsive table-light table-striped table-hover align-middle" AlternatingRowStyle-CssClass="alt" 
                                    OnRowCommand="GrdDocumentos_RowCommand" OnSelectedIndexChanged="GrdDocumentos_SelectedIndexChanged" OnRowDataBound ="GrdDocumentos_RowDataBound"
                                    DataKeyNames="IdDoc_Categoria" Font-Size="Smaller" >
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:BoundField DataField="TpoArchivo" HeaderText="Tipo de Archivo" >
                                        <ItemStyle Width="125px" /> 
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción del documento" >
                                        <ItemStyle Width="825px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fec_Entrega" DataFormatString="{0:d}" HeaderText="Fecha de entrega" >
                                        <ItemStyle Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DocInterno" >
                                        </asp:BoundField>    
                                        <asp:TemplateField HeaderText="Entregado" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                            <ItemStyle Width="125px" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxRow" runat="server" Checked='<%# Convert.ToBoolean(Eval("IdDescarga")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>

                        </div>
                    </div>

                    <%--<div class="d-grid gap-4 d-flex justify-content-center mt-4 mb-3">--%>
                    <div class="d-grid gap-2 gap-md-3 d-md-flex justify-content-md-center mt-2 mb-3">
                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnCartaSolicitud"  runat="server" Text="Carta Solicitud" OnClick="BtnCartaSolicitud_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                <asp:Button ID="BtnGeneraDocumento" runat="server" Text="Informe Preliminar" OnClick="BtnInformePreliminar_Click" CssClass="btn btn-primary" TabIndex="2"/>
                                <asp:Button ID="BtnConvenioAjuste" runat="server" Text="Convenio Ajuste" OnClick="BtnConvenioAjuste_Click" CssClass="btn btn-primary" TabIndex="3" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </asp:Panel>
                </div>
                <div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl3" runat="server" Text="DATOS PERSONALES" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">--%>
                                <%--<ContentTemplate>--%>
                                    <asp:Button ID="btnShowPanel3" runat="server" Text="&#9660;" OnClick="btnShowPanel3_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                <%--</ContentTemplate>--%>
                            <%--</asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnl3" runat="server" Visible="false">
                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl5" runat="server" Text="CONTACTO 1" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel5" runat="server" Text="&#9660;" OnClick="btnShowPanel5_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <div>
                    <asp:Panel ID="pnl5" runat="server" Visible="true">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomContacto1" runat="server" Text="Nombre" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomContacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-6 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoContacto1" runat="server" Text="Tipo de Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoContacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
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

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel3_Contacto1" runat="server" Text="Telefono 3" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel3_Contacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel4_Contacto1" runat="server" Text="Telefono 4" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel4_Contacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
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
                            <div class="col-lg-6 col-md-6">
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class="mb-2">
                                    <asp:Label ID="LblDetalleContacto1" runat="server" Text="Detalle del Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDetalleContacto1" runat="server" CssClass="form-control form-control-sm" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    </div>

                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl6" runat="server" Text="CONTACTO 2" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel6" runat="server" Text="&#9660;" OnClick="btnShowPanel6_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                   </div>

                    <div>
                    <asp:Panel ID="pnl6" runat="server" Visible="false">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomContacto2" runat="server" Text="Nombre" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomContacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-6 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoContacto2" runat="server" Text="Tipo de Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoContacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel1_Contacto2" runat="server" Text="Telefono 1" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel1_Contacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel2_Contacto2" runat="server" Text="Telefono 2" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel2_Contacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel3_Contacto2" runat="server" Text="Telefono 3" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel3_Contacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel4_Contacto2" runat="server" Text="Telefono 4" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel4_Contacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblEmailContacto2" runat="server" Text="Correo Electronico del Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEmailContacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6">
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class="mb-2">
                                    <asp:Label ID="LblDetalleContacto2" runat="server" Text="Detalle del Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDetalleContacto2" runat="server" CssClass="form-control form-control-sm" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    </div>

                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl7" runat="server" Text="CONTACTO 3" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel7" runat="server" Text="&#9660;" OnClick="btnShowPanel7_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <div>
                    <asp:Panel ID="pnl7" runat="server" Visible="false">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomContacto3" runat="server" Text="Nombre" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomContacto3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-6 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoContacto3" runat="server" Text="Tipo de Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoContacto3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel1_Contacto3" runat="server" Text="Telefono 1" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel1_Contacto3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel2_Contacto3" runat="server" Text="Telefono 2" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel2_Contacto3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel3_Contacto3" runat="server" Text="Telefono 3" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel3_Contacto3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel4_Contacto3" runat="server" Text="Telefono 4" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel4_Contacto3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblEmailContacto3" runat="server" Text="Correo Electronico del Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEmailContacto3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6">
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class="mb-2">
                                    <asp:Label ID="LblDetalleContacto3" runat="server" Text="Detalle del Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDetalleContacto3" runat="server" CssClass="form-control form-control-sm" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    </div>

                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl8" runat="server" Text="CONTACTO 4" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel8" runat="server" Text="&#9660;" OnClick="btnShowPanel8_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <div>
                    <asp:Panel ID="pnl8" runat="server" Visible="false">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomContacto4" runat="server" Text="Nombre" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomContacto4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-6 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoContacto4" runat="server" Text="Tipo de Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoContacto4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel1_Contacto4" runat="server" Text="Telefono 1" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel1_Contacto4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel2_Contacto4" runat="server" Text="Telefono 2" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel2_Contacto4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel3_Contacto4" runat="server" Text="Telefono 3" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel3_Contacto4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTel4_Contacto4" runat="server" Text="Telefono 4" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTel4_Contacto4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblEmailContacto4" runat="server" Text="Correo Electronico del Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEmailContacto4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6">
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class="mb-2">
                                    <asp:Label ID="LblDetalleContacto4" runat="server" Text="Detalle del Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDetalleContacto4" runat="server" CssClass="form-control form-control-sm" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    </div>

                    <!-- Botón Guardar y Actualizar para el panel 3 -->
                    <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnAnularPnl3" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl3_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                <asp:Button ID="btnEditarPnl3" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl3_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                <asp:Button ID="btnActualizarPnl3" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl3_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                </div>
                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl4" runat="server" Text="INFORMACIÓN PÓLIZA" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">--%>
                                <%--<ContentTemplate>--%>
                                    <asp:Button ID="btnShowPanel4" runat="server" Text="&#9660;" OnClick="btnShowPanel4_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                <%--</ContentTemplate>--%>
                            <%--</asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>
                <div>
                <%-- Detalles Poliza --%>
                <asp:Panel ID="pnl4" runat="server" Visible="false">

                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl9" runat="server" Text="DETALLES PÓLIZA" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel9" runat="server" Text="&#9660;" OnClick="btnShowPanel9_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnl9" runat="server" Visible="false">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoProducto" runat="server" Text="Tipo Producto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoProducto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaEmision" runat="server" Text="Fecha Emision" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaEmision" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaEmision">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateEmision" runat="server" TargetControlID="TxtFechaEmision" PopupButtonID="BtnFechaEmision" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaContacto" runat="server" Text="Fecha Contacto" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaContacto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFecContacto">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateFecContacto" runat="server" TargetControlID="TxtFechaContacto" PopupButtonID="BtnFecContacto" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaIniVigencia" runat="server" Text="Fecha Inicio Vigencia" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaIniVigencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaIniVigencia">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateFechaIniVigencia" runat="server" TargetControlID="TxtFechaIniVigencia" PopupButtonID="BtnFechaIniVigencia" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaFinVigencia" runat="server" Text="Fecha Final Vigencia" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaFinVigencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFecFinVigencia">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateFecFinVigencia" runat="server" TargetControlID="TxtFechaFinVigencia" PopupButtonID="BtnFecFinVigencia" Format="dd/MM/yyyy" />
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblNumCertificado" runat="server" Text="No. Certificado" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNumCertificado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoMoneda" runat="server" Text="Tipo Moneda" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoMoneda" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoPlan" runat="server" Text="Tipo Plan" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoPlan" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblPlazo" runat="server" Text="Plazo" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtPlazo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblCanalVentas" runat="server" Text="Canal de Ventas" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtCanalVentas" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblNumRenovacion" runat="server" Text="No. Renovación" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNumRenovacion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblGiro" runat="server" Text="Giro" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtGiro_Asegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl9" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl9_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl9" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl9_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl9" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl9_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </asp:Panel>

                    <%-- Detalles Contratante --%>
                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl10" runat="server" Text="DETALLES CONTRATANTE" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel10" runat="server" Text="&#9660;" OnClick="btnShowPanel10_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnl10" runat="server" Visible="false">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomContratante" runat="server" Text="Nombre Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblTpoContratante" runat="server" Text="Tipo Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTpoContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblCalleContratante" runat="server" Text="Calle Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtCalleContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblColoniaContratante" runat="server" Text="Colonia Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtColoniaContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblPoblacionContratante" runat="server" Text="Poblacion Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtPoblacionContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblEstadoContratante" runat="server" Text="Estado Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEstadoContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblMunicipioContratante" runat="server" Text="Municipio Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtMunicipioContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblCPostalContratante" runat="server" Text="C.Postal Contratante" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtCPostalContratante" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl10" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl10_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl10" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl10_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl10" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl10_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>

                    <%-- Datos Asegurado --%>
                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl11" runat="server" Text="ASEGURADO(S)" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel11" runat="server" Text="&#9660;" OnClick="btnShowPanel11_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnl11" runat="server" Visible="false">
                        <%-- Asegurado 1 --%>
                        <div class="row mb-3 mt-4" style="background-color:#AED6F1; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl12" runat="server" Text="ASEGURADO 1" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <%--<asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">--%>
                                        <%--<ContentTemplate>--%>
                                            <asp:Button ID="btnShowPanel12" runat="server" Text="&#9660;" OnClick="btnShowPanel12_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                        <%--</ContentTemplate>--%>
                                    <%--</asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="pnl12" runat="server" Visible="false">
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:CheckBox ID="chkCopiarDatos" runat="server" AutoPostBack="True" OnCheckedChanged="chkCopiarDatos_CheckedChanged" Text="&nbsp;&nbsp;&nbsp;Copiar datos de CONTRATANTE" TextAlign="Right" />
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNombreAsegurado1" runat="server" Text="Nombre Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNombreAsegurado1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblTipoAsegurado1" runat="server" Text="Tipo Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtTpoAsegurado1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCalleAsegurado1" runat="server" Text="Calle Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCalleAsegurado1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblColoniaAsegurado1" runat="server" Text="Colonia Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtColoniaAsegurado1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPoblacionAsegurado1" runat="server" Text="Poblacion Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPoblacionAsegurado1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblEstadoAsegurado1" runat="server" Text="Estado Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtEstadoAsegurado1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblMunicipioAsegurado1" runat="server" Text="Municipio Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMunicipioAsegurado1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCPostalAsegurado1" runat="server" Text="C.Postal Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCPostalAsegurado1" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    <%-- Asegurado 2 --%>
                        <div class="row mb-3 mt-4" style="background-color:#AED6F1; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl13" runat="server" Text="ASEGURADO 2" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <%--<asp:UpdatePanel ID="UpdatePanel23" runat="server" UpdateMode="Conditional">--%>
                                        <%--<ContentTemplate>--%>
                                            <asp:Button ID="btnShowPanel13" runat="server" Text="&#9660;" OnClick="btnShowPanel13_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                        <%--</ContentTemplate>--%>
                                    <%--</asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="pnl13" runat="server" Visible="false">
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNombreAsegurado2" runat="server" Text="Nombre Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNombreAsegurado2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblTipoAsegurado2" runat="server" Text="Tipo Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtTpoAsegurado2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCalleAsegurado2" runat="server" Text="Calle Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCalleAsegurado2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblColoniaAsegurado2" runat="server" Text="Colonia Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtColoniaAsegurado2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPoblacionAsegurado2" runat="server" Text="Poblacion Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPoblacionAsegurado2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">


                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblEstadoAsegurado2" runat="server" Text="Estado Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtEstadoAsegurado2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblMunicipioAsegurado2" runat="server" Text="Municipio Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMunicipioAsegurado2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCPostalAsegurado2" runat="server" Text="C.Postal Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCPostalAsegurado2" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    <%-- Asegurado 3 --%>
                        <div class="row mb-3 mt-4" style="background-color:#AED6F1; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl14" runat="server" Text="ASEGURADO 3" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <%--<asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">--%>
                                        <%--<ContentTemplate>--%>
                                            <asp:Button ID="btnShowPanel14" runat="server" Text="&#9660;" OnClick="btnShowPanel14_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                        <%--</ContentTemplate>--%>
                                    <%--</asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="pnl14" runat="server" Visible="false">
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNombreAsegurado3" runat="server" Text="Nombre Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNombreAsegurado3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblTipoAsegurado3" runat="server" Text="Tipo Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtTpoAsegurado3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCalleAsegurado3" runat="server" Text="Calle Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCalleAsegurado3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblColoniaAsegurado3" runat="server" Text="Colonia Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtColoniaAsegurado3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPoblacionAsegurado3" runat="server" Text="Poblacion Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPoblacionAsegurado3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblEstadoAsegurado3" runat="server" Text="Estado Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtEstadoAsegurado3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblMunicipioAsegurado3" runat="server" Text="Municipio Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMunicipioAsegurado3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCPostalAsegurado3" runat="server" Text="C.Postal Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCPostalAsegurado3" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl11" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl11_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl11" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl11_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl11" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl11_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </asp:Panel>

                    <%-- Bien Asegurado --%>
                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl15" runat="server" Text="BIEN ASEGURADO" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel15" runat="server" Text="&#9660;" OnClick="btnShowPanel15_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnl15" runat="server" Visible="false">
                        <%-- Ubicacion Asegurada --%>
                        <div class="row mb-3 mt-4" style="background-color:#AED6F1; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl16" runat="server" Text="UBICACIÓN ASEGURADA" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <%--<asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Conditional">--%>
                                        <%--<ContentTemplate>--%>
                                            <asp:Button ID="btnShowPanel16" runat="server" Text="&#9660;" OnClick="btnShowPanel16_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                        <%--</ContentTemplate>--%>
                                    <%--</asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="pnl16" runat="server" Visible="false">
                            <div class="row mt-3">
                                <div class="col-lg-6 col-md-6">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCalleBienAsegurado" runat="server" Text="Calle Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCalleBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblColoniaBienAsegurado" runat="server" Text="Colonia Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtColoniaBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPoblacionBienAsegurado" runat="server" Text="Poblacion Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPoblacionBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblEstadoBienAsegurado" runat="server" Text="Estado Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtEstadoBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblMunicipioBienAsegurado" runat="server" Text="Municipio Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMunicipioBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblCodigoBienAsegurado" runat="server" Text="C.Postal Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtCodigoBienAsegurado" runat="server" CssClass="form-control form-control-sm" MaxLength="5" ></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblTpoTecho" runat="server" Text="Tipo Techo" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtTpoTecho" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblTpoVivienda" runat="server" Text="Tipo Vivienda" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtTpoVivienda" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblTpoMuro" runat="server" Text="Tipo Muro" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtTpoMuro" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPisosBienAsegurado" runat="server" Text="Pisos Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPisosBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblPisosDelBienAsegurado" runat="server" Text="Pisos del Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtPisosDelBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblLocalesComerciales" runat="server" Text="Locales_Comerciales" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtLocalesComerciales" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-6 col-md-6">
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-lg-12 col-md-12">
                                    <div class="mb-2">
                                        <asp:Label ID="LblDetalleBienAsegurado" runat="server" Text="Detalles Bien Asegurado" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtDetallesBienAsegurado" runat="server" CssClass="form-control form-control-sm" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                                <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnAnularPnl15" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl15_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                        <asp:Button ID="btnEditarPnl15" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl15_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                        <asp:Button ID="btnActualizarPnl15" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl15_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </asp:Panel>

                    </asp:Panel>

                    <%-- Datos Coberturas --%>
                    <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaPnl17" runat="server" Text="COBERTURAS" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel30" runat="server" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>--%>
                                        <asp:Button ID="btnShowPanel17" runat="server" Text="&#9660;" OnClick="btnShowPanel17_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    <%--</ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnl17" runat="server" Visible="false">
                        <div class="row mb-3">
                            <div class="col-lg-4 col-md-4">
                                <div class ="mb-2">
                                    <asp:Label ID="LblCoberturas" runat="server" Text="Cobertura" ></asp:Label>
                                </div>
                                <div class=" input-group input-group-sm">
                                    <asp:DropDownList ID="ddlCoberturas" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCoberturas_SelectedIndexChanged" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-1 col-md-1">
                                <div class="mb-2">
                                    &nbsp;
                                </div>
                                <div class="input-group input-group-sm">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4">
                                <div class="mb-2">
                                    <asp:Label ID="LblSumaAsegurada" runat="server" Text="Suma Asegurada" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtSumaAsegurada" runat="server" MaxLength="50" CssClass="form-control form-control-sm" placeholder="Suma Asegurada"></asp:TextBox>
<%--                                    <asp:TextBox ID="TxtSumaAsegurada" runat="server" CssClass="form-control form-control-sm" MaxLength ="18"
                                        oninput="this.value = this.value.replace(/[^0-9.]/g, ''); if ((this.value.match(/\./g) || []).length > 1) this.value = this.value.replace(/\.$/, '');">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revSumaAsegurada" runat="server" ControlToValidate="TxtSumaAsegurada" 
                                        ValidationExpression="^\d+(\.\d{1,2})?$" ErrorMessage="&nbsp;&nbsp;&nbsp; Por favor, ingresa un valor válido." CssClass="error" Display="Dynamic" />--%>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-9 col-md-9">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomCobertura" runat="server" Text="Nombre Cobertura" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomCobertura" runat="server" MaxLength="100" CssClass="form-control form-control-sm" placeholder="Ingrese Cobertura"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-9 col-md-9">
                                <div class="mb-2">
                                    <asp:Label ID="LblRiesgo" runat="server" Text="Riesgo" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtRiesgo" runat="server" MaxLength="100" CssClass="form-control form-control-sm" placeholder="Ingrese Riesgo"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-9 col-md-9">
                                <div class="mb-2">
                                    <asp:Label ID="LblSublimite" runat="server" Text="Sublimite" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtSublimite" runat="server" MaxLength="100" CssClass="form-control form-control-sm" placeholder="Ingrese Sublimite"></asp:TextBox>
                                </div>
                            </div>
			            </div>
                        <div class="row mt-3">
                            <div class="col-lg-9 col-md-9">
                                <div class="mb-2">
                                    <asp:Label ID="LblDeducible" runat="server" Text="Deducible" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDeducible" runat="server" MaxLength="100" CssClass="form-control form-control-sm" placeholder="Ingrese Deducible"></asp:TextBox>
                                </div>
                            </div>
			            </div>
                        <div class="row mt-3">
                            <div class="col-lg-9 col-md-9">
                                <div class="mb-2">
                                    <asp:Label ID="LblCoaseguro" runat="server" Text="Coaseguro" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtCoaseguro" runat="server" MaxLength="100" CssClass="form-control form-control-sm" placeholder="Ingrese Coaseguro"></asp:TextBox>
                                </div>
                            </div>
			            </div>

<%--
                        <asp:Repeater ID="Repeater1" runat="server">
                            <HeaderTemplate>
                                <table style="width: 100%; border-collapse: collapse;">
                                    <thead>
                                        <tr>
                                            <th>&nbsp;</th>
                                            <th style="font-size: 12px;">NOMBRE COBERTURA</th>
                                            <th style="font-size: 12px;">RIESGO</th>
                                            <th style="font-size: 12px;">SUMA ASEGURADA</th>
                                            <th style="font-size: 12px;">SUBLIMITE</th>
                                            <th style="font-size: 12px;">DEDUCIBLE</th>
                                            <th style="font-size: 12px;">COASEGURO</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="border-bottom: 1px solid #ddd;">
                                    <td style="width: 200px;">
                                        <div style="display: flex; justify-content: space-between; align-items: center;">
                                            <div style="flex: 1; text-align: left;">
                                                <asp:Label ID="Label" runat="server" Text='<%# Eval("Label") %>' AssociatedControlID="CheckBox" />
                                            </div>
                                            <div style="flex: 0; text-align: right; margin-right: 10px;">
                                                <asp:CheckBox ID="CheckBox" runat="server" Checked='<%# Convert.ToBoolean(Eval("Cob_Habilitado")) %>' />
                                            </div>
                                        </div>
                                    </td>
                                    <td><asp:TextBox ID="TextBox1" runat="server" Width="95%" Text='<%# Eval("Cob_Nombre") %>' /></td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" Width="95%" Text='<%# Eval("Cob_Riesgo") %>' /></td>
                                    <td>
                                        <asp:TextBox ID="TextBox3" runat="server" Width="95%" Text='<%# Eval("Cob_Suma") %>' oninput="this.value = this.value.replace(/[^0-9.]/g, '');" />
                                        <asp:RegularExpressionValidator ID="revTextBox3" runat="server" ControlToValidate="TextBox3" ValidationExpression="^\d+(\.\d{1,2})?$" CssClass="error" Display="Dynamic" />
                                    </td>
                                    <td><asp:TextBox ID="TextBox4" runat="server" Width="95%" Text='<%# Eval("Cob_Sublimite") %>' /></td>
                                    <td><asp:TextBox ID="TextBox5" runat="server" Width="95%" Text='<%# Eval("Cob_Deducible") %>' /></td>
                                    <td><asp:TextBox ID="TextBox6" runat="server" Width="95%" Text='<%# Eval("Cob_Coaseguro") %>' /></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
--%>

                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl17" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl17_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl17" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl17_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl17" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl17_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                    <asp:Button ID="BtnAgregarPnl17" runat="server" Text="Agregar" OnClick="BtnAgregarPnl17_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="container col-12 mt-4">
                            <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                                <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Cobertura(s)</h6>
                            </div>

                            <%-- Reporte de Cobertura --%>
                            <div style="overflow-x: auto; overflow-y:hidden">
                                <asp:GridView ID="GrdCoberturas"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                        AllowPaging="false" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                        OnPageIndexChanging="GrdCoberturas_PageIndexChanging" OnRowCommand="GrdCoberturas_RowCommand" OnPreRender="GrdCoberturas_PreRender"
                                        OnSelectedIndexChanged="GrdCoberturas_SelectedIndexChanged" OnRowDataBound="GrdCoberturas_RowDataBound" 
                                        DataKeyNames="IdCobertura" PageSize="10" Font-Size="Smaller" >
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgEditar" runat="server" OnClick="ImgEditar_Click" Height="24px" Width="24px" ImageUrl="~/Images/editar_new.png"  Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgEliminar" runat="server" OnClick="ImgEliminar_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IdCobertura" />
                                            <asp:BoundField DataField="DescCobertura" />
                                            <asp:BoundField DataField="Cob_Nombre" HeaderText="Nombre Cobertura" />
                                            <asp:BoundField DataField="Cob_Riesgo"  HeaderText="Riesgo" />
                                            <asp:BoundField DataField="Cob_Suma" HeaderText="Suma Asegurada" />
                                            <asp:BoundField DataField="Cob_Sublimite"  HeaderText="Sublimite" />
                                            <asp:BoundField DataField="Cob_Deducible" HeaderText="Deducible" />
                                            <asp:BoundField DataField="Cob_Coaseguro" HeaderText="Coaseguro" />
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>

                    </asp:Panel>

                </asp:Panel>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgDel_Documento" />

            <asp:PostBackTrigger ControlID="BtnCrear_Cuaderno" />
            <asp:PostBackTrigger ControlID="BtnGraba_Categorias" />
            <asp:PostBackTrigger ControlID="BtnCartaSolicitud" />
            <asp:PostBackTrigger ControlID="BtnGeneraDocumento" />
            <asp:PostBackTrigger ControlID="BtnConvenioAjuste" />
            
            <asp:PostBackTrigger ControlID="BtnAnularPnl2" />
            <asp:PostBackTrigger ControlID="btnEditarPnl2" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl2" />
            
            <asp:PostBackTrigger ControlID="BtnAnularPnl3" />
            <asp:PostBackTrigger ControlID="btnEditarPnl3" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl3" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl9" />
            <asp:PostBackTrigger ControlID="btnEditarPnl9" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl9" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl10" />
            <asp:PostBackTrigger ControlID="btnEditarPnl10" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl10" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl11" />
            <asp:PostBackTrigger ControlID="btnEditarPnl11" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl11" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl15" />
            <asp:PostBackTrigger ControlID="btnEditarPnl15" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl15" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl17" />
            <asp:PostBackTrigger ControlID="btnEditarPnl17" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl17" /> 
            <asp:PostBackTrigger ControlID="BtnAgregarPnl17" />

            <asp:PostBackTrigger ControlID="GrdCoberturas" />
        </Triggers>
    </asp:UpdatePanel>

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
            <td>
                <div class="form-group">
                    <div class="d-grid col-6 mx-auto">
                        <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                            TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
                    </div>
                </div>
            </td>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
