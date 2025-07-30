<%@ Page Title="" MaintainScrollPositionOnPostBack="true" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Bitacora_Asunto.aspx.cs" Inherits="WebItNow_Peacock.fwGM_Bitacora_Asunto" %>
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

        function mpeNewProcesoOnOK() {
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

        .custom-dropdown {
            border: 1px solid #ced4da;
            border-radius: .25rem;
            background-color: #f8f9fa; /* color como botón claro */
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
            height: calc(1.5em + .5rem + 2px); /* igual que btn-sm */
            font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif; /* misma fuente que Bootstrap */
        }

        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true" EnablePartialRendering="true">
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
                            <asp:Label ID="LblNumSiniestro" runat="server" Text="No. Siniestro" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumSiniestro" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblNumPoliza" runat="server" Text="No. Póliza" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumPoliza" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
                        <div class="col-lg-4 col-md-4">
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
                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
                                <asp:Label ID="LblFechaFinVigencia" runat="server" Text="Fecha Final Vigencia" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtFechaFinVigencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaFinVigencia">
                                    <span class="visually-hidden">Toggle Dropdown</span>
                                </button>
                                <ajaxToolkit:CalendarExtender ID="dateFechaFinVigencia" runat="server" TargetControlID="TxtFechaFinVigencia" PopupButtonID="BtnFechaFinVigencia" Format="dd/MM/yyyy" />
                            </div>
                        </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblEstatusCaso" runat="server" Text="Estatus del Caso" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlEstatusCaso" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstatusCaso_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-8 col-md-8">
                        <div class="mb-2">
                            <asp:Label ID="LblNomAjustador" runat="server" Text="Nombre Ajustador" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomAjustador" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" onkeyup="mayus(this);" MaxLength="80"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblEmailAjustador" runat="server" Text="Correo Electronico Ajustador" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEmailAjustador" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblTelAjustador" runat="server" Text="Telefono Celular Ajustador" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTelAjustador" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                                <asp:Label ID="LblHoraOcurrencia" runat="server" Text="Hora Ocurrencia" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtHoraOcurrencia" runat="server" CssClass="form-control form-control-sm" 
                                      onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                      oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                      MaxLength ="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
                                <asp:Label ID="LblHoraRecepcion" runat="server" Text="Hora Recepcion" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtHoraRecepcion" runat="server" CssClass="form-control form-control-sm" 
                                        onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                        oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                        MaxLength ="5"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row mt-3">
                        <div class="col-lg-12 col-md-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblDetalleReporte" runat="server" Text="Descripción del Accidente"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtDetalleReporte" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>

<%--                    
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
--%>

                </asp:Panel>
                    <!-- Botón Guardar y Actualizar para el panel 2 -->
                    <%--<div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">--%>
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
                </div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl6" runat="server" Text="LINEA / ESTACIÓN DE OCURRENCIA" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel6" runat="server" Text="&#9660;" OnClick="btnShowPanel6_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>

                <div>
                    <asp:Panel ID="pnl6" runat="server" Visible="true">
                        <div class="row mb-3">
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblLineaOcurrencia" runat="server" Text="Línea de Ocurrencia" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlLineaOcurrencia" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlLineaOcurrencia_SelectedIndexChanged" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblEstacionOcurrencia" runat="server" Text="Estación de Ocurrencia" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlEstacionOcurrencia" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstacionOcurrencia_SelectedIndexChanged" Width="100%" >
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblRef_Ubicacion" runat="server" Text="Referencias de la Ubicación"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtRef_Ubicacion" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Botón Guardar y Actualizar para el panel 6 -->
                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl6" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl6_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl6" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl6_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl6" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl6_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </asp:Panel>
                </div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl3" runat="server" Text="INFORMACIÓN GENERAL DEL BENEFICIARIO" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel3" runat="server" Text="&#9660;" OnClick="btnShowPanel3_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="pnl3" runat="server" Visible="true">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomLesionado" runat="server" Text="Nombre Completo del Beneficiario" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFecNacimiento" runat="server" Text="Fecha de Nacimiento" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFecNacimiento" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFecNacimiento">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateNacimiento" runat="server" TargetControlID="TxtFecNacimiento" PopupButtonID="BtnFecNacimiento" Format="dd/MM/yyyy" />
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblSexo" runat="server" Text="Sexo" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtSexo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblEmailLesionado" runat="server" Text="Correo Electronico Beneficiario" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEmailLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTelLesionado" runat="server" Text="Telefono Beneficiario" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTelLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblEdadLesionado" runat="server" Text="Edad" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEdadLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblRFC_Lesionado" runat="server" Text="RFC Beneficiario" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtRFC_Lesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblClasifLesiones" runat="server" Text="Clasificación de las lesiones" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlTipoEvento" runat="server" CssClass="btn btn-outline-secondary text-start"  OnSelectedIndexChanged="ddlTipoEvento_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="4">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblDescLesiones" runat="server" Text="Descripción de las Lesiones"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDescLesiones" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    <div class="row mt-3">
                        <div class="col-lg-8 col-md-8">
                            <div class ="mb-2">
                                <asp:Label ID="LblCalleLesionado" runat="server" Text="Calle" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtCalleLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-2">
                            <div class ="mb-2">
                                <asp:Label ID="LblNumExtLesionado" runat="server" Text="Num. Exterior" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtNumExtLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-2">
                            <div class ="mb-2">
                                <asp:Label ID="LblNumIntLesionado" runat="server" Text="Num. Interior" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtNumIntLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row mt-3">
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblEstadoLesionado" runat="server" Text="Estado" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlEstadoLesionado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoLesionado_SelectedIndexChanged" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblMunicipiosLesionado" runat="server" Text="Delegación / Municipio" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlMunicipiosLesionado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipiosLesionado_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblColoniaLesionado" runat="server" Text="Colonia" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtColoniaLesionado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-1 col-md-1 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblCPostalLesionado" runat="server" Text="C.Postal" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtCPostalLesionado" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
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
                            <asp:Label ID="LblEtiquetaPnl4" runat="server" Text="INFORMACIÓN GENERAL DEL RESPONSABLE" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <div>
                                <asp:Button ID="btnShowPanel4" runat="server" Text="&#9660;" OnClick="btnShowPanel4_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                            </div>
                        </div>
                    </div>
                <div>
                    <asp:Panel ID="pnl4" runat="server" Visible="true">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblNomResponsable" runat="server" Text="Nombre Completo del Tercer Responsable" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNomResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblParentesco" runat="server" Text="Parentesco" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtParentesco" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblEdadResponsable" runat="server" Text="Edad" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEdadResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTelResponsable" runat="server" Text="Telefono Responsable" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTelResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblRFC_Responsable" runat="server" Text="RFC Responsable" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtRFC_Responsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-5 col-md-5 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblEmailResponsable" runat="server" Text="Correo Electronico Responsable" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEmailResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-8 col-md-8">
                                <div class ="mb-2">
                                    <asp:Label ID="LblCalleResponsable" runat="server" Text="Calle" ></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtCalleResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-2">
                                <div class ="mb-2">
                                    <asp:Label ID="LblNumExtResponsable" runat="server" Text="Num. Exterior" ></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNumExtResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-2">
                                <div class ="mb-2">
                                    <asp:Label ID="LblNumIntResponsable" runat="server" Text="Num. Interior" ></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtNumIntResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblEstadoResponsable" runat="server" Text="Estado" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlEstadoResponsable" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoResponsable_SelectedIndexChanged" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblMunicipioResponsable" runat="server" Text="Delegación / Municipio" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlMunicipiosResponsable" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipiosResponsable_SelectedIndexChanged" Width="100%" >
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblColoniaResponsable" runat="server" Text="Colonia" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtColoniaResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-1 col-md-1 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblCPostalResponsable" runat="server" Text="C.Postal" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtCPostalResponsable" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <!-- Botón Guardar y Actualizar para el panel 4 -->
                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl4" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl4_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl4" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl4_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl4" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl4_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>

                </div>

<%--
                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl5" runat="server" Text="CONFIGURACIÓN DE LA RECLAMACIÓN" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel1" runat="server" Text="&#9660;" OnClick="btnShowPanel1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>
--%>

                <div>
                <asp:Panel ID="pnl1" runat="server" Visible="false">
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
<%--                    
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
--%>

                    <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnGraba_Categorias" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGraba_Categorias_Click" CssClass="btn btn-primary" TabIndex="1" />
                                <asp:Button ID="BtnCrear_Cuaderno" runat="server" Text="Crear Cuaderno" Font-Bold="True" OnClick="BtnCrear_Cuaderno_Click" CssClass="btn btn-secondary" TabIndex="2" Enabled="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                </div>
                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl0" runat="server" Text="GENERADOR DE DOCUMENTOS" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel0" runat="server" Text="&#9660;" OnClick="btnShowPanel0_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>
                <div>
                <asp:Panel ID="pnl0" runat="server" Visible="false">
                    <div class="row mb-2">
                        <%--<div style="overflow-x: auto; overflow-y:hidden">--%>
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
                </asp:Panel>

                <div>
                    <asp:Panel ID="pnl5" runat="server" Visible="true">
                        <div class="d-grid gap-2 gap-md-3 d-md-flex justify-content-md-center mt-2 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnOne" runat="server" Text="P. Medico STC." OnClick="BtnOne_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="BtnTwo"  runat="server" Text="1ra Respuesta" OnClick="BtnTwo_Click" CssClass="btn btn-primary" TabIndex="2"/>
                                    <asp:Button ID="BtnThree" runat="server" Text="C. Autorización" OnClick="BtnThree_Click" CssClass="btn btn-primary" TabIndex="3" />
                                    <asp:Button ID="BtnFour" runat="server" Text="D. Médico" OnClick="BtnFour_Click" CssClass="btn btn-primary" TabIndex="4"/>
                                    <asp:Button ID="BtnFive"  runat="server" Text="I. Preliminar" OnClick="BtnFive_Click" CssClass="btn btn-primary" TabIndex="5"/>
                                    <asp:Button ID="BtnSix" runat="server" Text="P.Médico Benef" OnClick="BtnSix_Click" CssClass="btn btn-primary" TabIndex="6" />
                                    <asp:Button ID="BtnSeven" runat="server" Text="P.Médico Ajust." OnClick="BtnSeven_Click" CssClass="btn btn-primary" TabIndex="7" />
                                    <%--<asp:Button ID="BtnEight" runat="server" Text="P. Médico QRB." OnClick="BtnEight_Click" CssClass="btn btn-primary" TabIndex="7" />--%>
                                    <%--<asp:Button ID="BtnNine" runat="server" Text="P. Médico QRR." OnClick="BtnNine_Click" CssClass="btn btn-primary" TabIndex="7" />--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>
                </div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl7" runat="server" Text="PROVEEDOR(ES)" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel7" runat="server" Text="&#9660;" OnClick="btnShowPanel7_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="pnl7" runat="server" Visible="false">

                        <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblProveedor" runat="server" Text="DATOS DE PROVEEDOR" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <asp:Button ID="btnShowPanel9" runat="server" Text="&#9660;" OnClick="btnShowPanel9_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </div>
                            </div>
                        </div>

                        <div>
                            <asp:Panel ID="pnl9" runat="server" Visible="true">
                                <%--Campos *--%>
                                <div class="row mb-3">
                                    <div class="col-lg-6 col-md-6">
                                        <div class="mb-2">
                                            <asp:Label ID="LblTpoServicio" runat="server" Text="Tipo de Servicio" CssClass="control-label" Font-Size="Small"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:DropDownList ID="ddlTpoServicio" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoServicio_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-2">
                                        <div class="mb-2">
                                            <asp:Label runat="server" CssClass="form-label">&nbsp;</asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:Button ID="BtnProveedor" runat="server" Text="Buscar Proveedor" OnClick="BtnProveedor_Click" CssClass="btn btn-primary w-100" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-3">
                                    <div class="col-lg-6 col-md-6">
                                        <div class="mb-2">
                                            <asp:Label ID="LblNomEmpresa" runat="server" Text="Nombre de la empresa" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtNomEmpresa" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6">
                                        <div class="mb-2">
                                            <asp:Label ID="LblEmailEmpresa" runat="server" Text="Correo electrónico" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtEmailEmpresa" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-lg-8 col-md-8">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblCalleProveedor" runat="server" Text="Calle" ></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtCalleProveedor" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-2">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblNumExtProveedor" runat="server" Text="Num. Exterior" ></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtNumExtProveedor" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-2">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblNumIntProveedor" runat="server" Text="Num. Interior" ></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtNumIntProveedor" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-lg-4 col-md-4 ">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblEstadoProveedor" runat="server" Text="Estado" CssClass="control-label" Font-Size="Small"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:DropDownList ID="ddlEstadoProveedor" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoProveedor_SelectedIndexChanged" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4 ">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblMunicipioProveedor" runat="server" Text="Delegación / Municipio" CssClass="control-label" Font-Size="Small"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:DropDownList ID="ddlMunicipioProveedor" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipioProveedor_SelectedIndexChanged" Width="100%" >
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3 ">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblColoniaProveedor" runat="server" Text="Colonia" CssClass="control-label" Font-Size="Small"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtColoniaProveedor" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-1 col-md-1 ">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblCPostalProveedor" runat="server" Text="C.Postal" CssClass="control-label" Font-Size="Small"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtCPostalProveedor" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblTelContacto1" runat="server" Text="Teléfono de contacto 1" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtTelContacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblTelContacto2" runat="server" Text="Teléfono de contacto 2" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtTelContacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblHoraSolicitud" runat="server" Text="Hora de solicitud" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtHoraSolicitud" runat="server" CssClass="form-control form-control-sm" 
                                                    onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                                    oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                                    MaxLength ="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblHoraArribo" runat="server" Text="Hora de Arribo" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtHoraArribo" runat="server" CssClass="form-control form-control-sm" 
                                                    onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                                    oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                                    MaxLength ="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblHoraSalida" runat="server" Text="Hora de Salida" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtHoraSalida" runat="server" CssClass="form-control form-control-sm" 
                                                    onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                                    oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                                    MaxLength ="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblHoraLlegada" runat="server" Text="Hora de Llegada" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtHoraLlegada" runat="server" CssClass="form-control form-control-sm" 
                                                    onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                                    oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                                    MaxLength ="5"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblProvMontoAutorizado" runat="server" Text="Monto Autorizado" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
<%--
                                            <asp:TextBox ID="TxtProvMontoAutorizado" runat="server" CssClass="form-control form-control-sm"
                                              onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                              oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength ="11"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revProvMontoAutorizado" runat="server" ControlToValidate="TxtProvMontoAutorizado" 
                                                ValidationExpression="^\d+(\.\d{0,2})?$" Display="None" EnableClientScript="true"/>
--%>
                                            <asp:TextBox ID="TxtProvMontoAutorizado" runat="server" CssClass="form-control form-control-sm"
                                                onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                                oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength="11"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <div class="mb-2">
                                            <asp:Label ID="LblNumUnidad" runat="server" Text="No. Unidad" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtNumUnidad" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6">
                                        <div class="mb-2">
                                            <asp:Label ID="LblResponsable" runat="server" Text="Responsable" CssClass="form-label"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <!-- Botón Guardar y Actualizar para el panel 7 -->
                                <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                                <asp:Button ID="BtnAnularPnl7" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl7_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                                <asp:Button ID="btnEditarPnl7" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl7_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                                                <asp:Button ID="btnActualizarPnl7" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl7_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                                                <asp:Button ID="btnAgregarPnl7" runat="server" Text="Agregar" OnClick="btnAgregarPnl7_Click" CausesValidation="true" CssClass="btn btn-primary px-4"  TabIndex="3"/>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <%-- Proveedores --%>
                                <div style="overflow-x: auto; overflow-y:hidden">
                                    <asp:GridView ID="GrdProveedores"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                            OnPageIndexChanging="GrdProveedores_PageIndexChanging" OnRowCommand="GrdProveedores_RowCommand" OnPreRender="GrdProveedores_PreRender"
                                            OnSelectedIndexChanged="GrdProveedores_SelectedIndexChanged" OnRowDataBound="GrdProveedores_RowDataBound" 
                                            DataKeyNames="Id_Proveedor" PageSize="5" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Id_Proveedor" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Tpo_Servicio" HeaderText="Tpo. Servicio" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Nom_Empresa" HeaderText="Nombre Empresa" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Hora_Solicitud" HeaderText="Hora Solicitud" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Hora_Arribo" HeaderText="Hora Arribo" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Hora_Salida" HeaderText="Hora Salida" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Hora_Llegada" HeaderText="Hora Llegada" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_Autorizado" HeaderText="Monto Autorizado" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Calle" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Num_Exterior" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Num_Interior" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estado" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Delegacion" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Colonia" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Codigo_Postal" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Tel_Contacto_1" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Tel_Contacto_2" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Email_Empresa" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Num_Unidad" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Responsable">
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgProveedor_Add" runat="server" OnClick="ImgProveedor_Add_Click" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png" Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgProveedor_Del" runat="server" OnClick="ImgProveedor_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png" Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </div>

                    </asp:Panel>
                </div>

                <%--PAQUETES MEDICOS--%>
		        <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl16" runat="server" Text="PAQUETES MEDICOS" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel16" runat="server" Text="&#9660;" OnClick="btnShowPanel16_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>

                <div>
                    <asp:Panel ID="pnl16" runat="server" Visible="false">
                            <div class="row mb-3">
                                <div class="col-lg-4 col-md-4">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblInstituciones" runat="server" Text="Institución Hospitalaria" ></asp:Label>
                                    </div>
                                    <div class=" input-group input-group-sm">
                                        <asp:DropDownList ID="ddlInstituciones" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlInstituciones_SelectedIndexChanged" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-5 col-md-5">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNomPaquete" runat="server" Text="Nombre del Paquete" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <%--<asp:TextBox ID="TxtNomPaquete" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlPaquetes_MD" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlPaquetes_MD_SelectedIndexChanged" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-lg-3 col-md-3">
                                    <div class="mb-2">
                                        <asp:Label ID="LblIdPaquete" runat="server" Text="ID del Paquete" CssClass="form-label"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtIdPaquete" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblMontoMinimo" runat="server" Text="Monto Minimo" ></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMontoMinimo" runat="server" CssClass="form-control form-control-sm"
                                          onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                          oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength ="11"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revMontoMinimo" runat="server" ControlToValidate="TxtMontoMinimo" 
                                            ValidationExpression="^\d+(\.\d{0,2})?$" Display="None" EnableClientScript="true"/>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblMontoMaximo" runat="server" Text="Monto Maximo" ></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMontoMaximo" runat="server" CssClass="form-control form-control-sm"
                                          onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                          oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength ="11"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revMontoMaximo" runat="server" ControlToValidate="TxtMontoMaximo" 
                                            ValidationExpression="^\d+(\.\d{0,2})?$" Display="None" EnableClientScript="true"/>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblMontoUtilizado" runat="server" Text="Monto Utilizado" ></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMontoUtilizado" runat="server" CssClass="form-control form-control-sm"
                                          onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                          oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength ="11"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revMontoUtilizado" runat="server" ControlToValidate="TxtMontoUtilizado" 
                                            ValidationExpression="^\d+(\.\d{0,2})?$" Display="None" EnableClientScript="true"/>
                                    </div>
                                </div>

                            </div>
                            <div class="row mb-3">
                                <div class="col-lg-3 col-md-3">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblMontoRestante" runat="server" Text="Monto Restante" ></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMontoRestante" runat="server" CssClass="form-control form-control-sm"
                                          onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                          oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength ="11"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revMontoRestante" runat="server" ControlToValidate="TxtMontoRestante" 
                                            ValidationExpression="^\d+(\.\d{0,2})?$" Display="None" EnableClientScript="true"/>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblMontoSuperado" runat="server" Text="Monto Superado" ></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtMontoSuperado" runat="server" CssClass="form-control form-control-sm"
                                          onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                          oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength ="11"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revMontoSuperado" runat="server" ControlToValidate="TxtMontoSuperado" 
                                            ValidationExpression="^\d+(\.\d{0,2})?$" Display="None" EnableClientScript="true"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-lg-12 col-md-12">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblObservaciones_PM" runat="server" Text="Observaciones / Aplicación"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtObservaciones_PM" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <!-- Botón Guardar y Actualizar para el panel 16 -->
                            <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnAnularPnl16" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl16_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                        <asp:Button ID="btnEditarPnl16" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl16_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                                        <asp:Button ID="btnActualizarPnl16" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl16_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                        <asp:Button ID="BtnAgregarPnl16" runat="server" Text="Agregar" OnClick="BtnAgregarPnl16_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row mt-5">
                                <%-- Consulta de Paquete Medico --%>
                                <div style="overflow-x: auto; overflow-y:hidden">
                                    <asp:GridView ID="GrdPaqueteMedico"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                            OnPageIndexChanging="GrdPaqueteMedico_PageIndexChanging" OnRowCommand="GrdPaqueteMedico_RowCommand" OnPreRender="GrdPaqueteMedico_PreRender"
                                            OnSelectedIndexChanged="GrdPaqueteMedico_SelectedIndexChanged" OnRowDataBound="GrdPaqueteMedico_RowDataBound" 
                                            DataKeyNames="Id_Institucion" PageSize="5" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Id_Consecutivo" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Id_Institucion" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Id_Paquete_Medico" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Institución Hospitalaria">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ID_Paquete" HeaderText="ID del Paquete" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_Minimo" HeaderText="Monto Minimo" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_Maximo" HeaderText="Monto Maximo" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_Utilizado" HeaderText="Monto Utilizado" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_Restante" HeaderText="Monto Restante" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_Superado" HeaderText="Monto Superado" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Observaciones" >
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgPaquete_Add" runat="server" OnClick="ImgPaquete_Add_Click" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png" Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgPaquete_Del" runat="server" OnClick="ImgPaquete_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png" Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>

                    </asp:Panel>
                </div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl8" runat="server" Text="INFORMACIÓN MEDICA" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel8" runat="server" Text="&#9660;" OnClick="btnShowPanel8_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="pnl8" runat="server" Visible="false">
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblAlergias" runat="server" Text="Alergias"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtAlergias" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblEnfermedades" runat="server" Text="Enfermedades preexistentes / Cronicas"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtEnfermedades" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblMedicamentos" runat="server" Text="Tratamientos"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtMedicamentos" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblAlcohol" runat="server" Text="Intoxicación por Alcohol"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtAlcohol" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblSustancias" runat="server" Text="Intoxicación por Sustancias"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtSustancias" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblObservaciones" runat="server" Text="Observaciones de primera atención"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtObservaciones_PA" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblDiagnosticoPreliminar" runat="server" Text="Diagnostico Preliminar"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDiagnosticoPreliminar" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblComentariosMedicos" runat="server" Text="Comentarios Médicos"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtComentariosMedicos" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl8" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl8_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl8" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl8_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl8" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl8_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblICD" runat="server" Text="Diagnostico Final" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <asp:Button ID="btnShowPanel10" runat="server" Text="&#9660;" OnClick="btnShowPanel10_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnl10" runat="server" Visible="false">
                        <div class="row mb-3">
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblBuscarICD" runat="server" Text="ICD" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtBuscarICD" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);"></asp:TextBox>
                                        </div>
                            </div>
                            <div class="col-lg-2 col-md-2 ">
                                <div class ="mb-2">
                                    <asp:Label runat="server" CssClass="form-label">&nbsp;</asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:Button ID="BtnICD" runat="server" Text="Buscar ICD" OnClick="BtnICD_Click" CssClass="btn btn-primary match-input-height" Enabled="false" />
                                </div>
                            </div>
                        </div>

                        <%--  Diagnostico Final --%>
                        <div style="overflow-x: auto; overflow-y:hidden">
                            <asp:GridView ID="GrdDiagnosticoFinal"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                    AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                    OnPageIndexChanging="GrdDiagnosticoFinal_PageIndexChanging" OnRowCommand="GrdDiagnosticoFinal_RowCommand" OnPreRender="GrdDiagnosticoFinal_PreRender"
                                    OnSelectedIndexChanged="GrdDiagnosticoFinal_SelectedIndexChanged" OnRowDataBound="GrdDiagnosticoFinal_RowDataBound" 
                                    DataKeyNames="Id_ICD" PageSize="5" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                    <AlternatingRowStyle CssClass="alt autoWidth" />
                                    <Columns>
                                        <asp:BoundField DataField="Id_ICD" >
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cve_ICD" HeaderText="Clave ICD" >
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Desc_ICD" HeaderText="Descripción" >
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Referencia" >
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SubReferencia" >
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgEliminar_ICD" runat="server" OnClick="ImgEliminar_ICD_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                            </asp:GridView>
                        </div>
                        
<%--
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblDiagnosticoFinal" runat="server" Text="Diagnostico Final"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDiagnosticoFinal" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
--%>
                        </asp:Panel>
                        <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblCPT" runat="server" Text="Tratamientos Realizados" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <asp:Button ID="btnShowPanel11" runat="server" Text="&#9660;" OnClick="btnShowPanel11_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnl11" runat="server" Visible="false">
                            <div class="row mb-3">
                                <div class="col-lg-4 col-md-4 ">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblBuscarCPT" runat="server" Text="CPT" CssClass="control-label" Font-Size="Small"></asp:Label>
                                    </div>
                                            <div class="input-group input-group-sm">
                                                <asp:TextBox ID="TxtBuscarCPT" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);"></asp:TextBox>
                                            </div>
                                </div>
                                <div class="col-lg-2 col-md-2 ">
                                    <div class ="mb-2">
                                        <asp:Label runat="server" CssClass="form-label">&nbsp;</asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:Button ID="BtnCPT" runat="server" Text="Buscar CPT" OnClick="BtnCPT_Click" CssClass="btn btn-primary match-input-height" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                            <%--  Tratamientos Realizados --%>
                            <div style="overflow-x: auto; overflow-y:hidden">
                                <asp:GridView ID="GrdTratamientos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                        OnPageIndexChanging="GrdTratamientos_PageIndexChanging" OnRowCommand="GrdTratamientos_RowCommand" OnPreRender="GrdTratamientos_PreRender"
                                        OnSelectedIndexChanged="GrdTratamientos_SelectedIndexChanged" OnRowDataBound="GrdTratamientos_RowDataBound" 
                                        DataKeyNames="Id_CPT" PageSize="5" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_CPT" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cve_CPT" HeaderText="Clave CPT" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Desc_CPT" HeaderText="Descripción" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Referencia" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubReferencia" >
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgEliminar_CPT" runat="server" OnClick="ImgEliminar_CPT_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
<%--                        
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblTratamientosRealizados" runat="server" Text="Tratamientos Realizados"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTratamientosRealizados" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
--%>
                        </asp:Panel>
                        <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblLine1" runat="server" Text="Estudios Realizados" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <asp:Button ID="btnShowPanel12" runat="server" Text="&#9660;" OnClick="btnShowPanel12_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnl12" runat="server" Visible="false">
                            <div class="row mt-3">
                                <div class="col-lg-12 col-md-12">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblEstudiosRealizados" runat="server" Text="Estudios Realizados"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtEstudiosRealizados" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <!-- Botón Guardar y Actualizar para el panel 9 -->
                            <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                                <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnAnularPnl9" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl9_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                        <asp:Button ID="btnEditarPnl9" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl9_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                                        <asp:Button ID="btnActualizarPnl9" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl9_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                                        <asp:Button ID="BtnAgregarPnl9" runat="server" Text="Agregar" OnClick="BtnAgregarPnl9_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row mt-5">
                                <%-- Estudios Realizados --%>
                                <div style="overflow-x: auto; overflow-y:hidden">
                                    <asp:GridView ID="GrdEstudios"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                            OnPageIndexChanging="GrdEstudios_PageIndexChanging" OnRowCommand="GrdEstudios_RowCommand" OnPreRender="GrdEstudios_PreRender"
                                            OnSelectedIndexChanged="GrdEstudios_SelectedIndexChanged" OnRowDataBound="GrdEstudios_RowDataBound" 
                                            DataKeyNames="Id_Estudio" PageSize="5" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Id_Estudio" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Desc_Estudio" HeaderText="Estudios Realizados" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgEstudios_Add" runat="server" OnClick="ImgEstudios_Add_Click" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png" Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgEstudios_Del" runat="server" OnClick="ImgEstudios_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png" Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>
<%--                        
                        <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblLine2" runat="server" Text="Comentarios Médicos" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <div>
                                    <asp:Button ID="btnShowPanel13" runat="server" Text="&#9660;" OnClick="btnShowPanel13_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnl13" runat="server" Visible="false">
                        </asp:Panel>
--%>

                    </asp:Panel>
                </div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl14" runat="server" Text="DATOS DE ATENCIÓN" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel14" runat="server" Text="&#9660;" OnClick="btnShowPanel14_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="pnl14" runat="server" Visible="false">
                        <div class="row mb-3">
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblZonas" runat="server" Text="Zonas" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlZonas" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlZonas_SelectedIndexChanged" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblHospitales" runat="server" Text="Institución Medica" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlHospitales" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlHospitales_SelectedIndexChanged" Width="100%" >
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblTpoAtencion" runat="server" Text="Tipo de Atención" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlTpoAtencion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoAtencion_SelectedIndexChanged" Width="100%" >
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-lg-6 col-md-6">
                                <div class="mb-2">
                                    <asp:Label ID="LblCorreoElectronico" runat="server" Text="Correo Electronico" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtCorreoElectronico" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTelAtencionContacto1" runat="server" Text="Telefono Contacto 1" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTelAtencionContacto1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 ">
                                <div class="mb-2">
                                    <asp:Label ID="LblTelAtencionContacto2" runat="server" Text="Telefono Contacto 2" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtTelAtencionContacto2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaIngreso" runat="server" Text="Fecha Ingreso" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaIngreso" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaIngreso">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateIngreso" runat="server" TargetControlID="TxtFechaIngreso" PopupButtonID="BtnFechaIngreso" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblHoraIngreso" runat="server" Text="Hora Ingreso" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtHoraIngreso" runat="server" CssClass="form-control form-control-sm" 
                                            onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                            oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                            MaxLength ="5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaRecepcionNM" runat="server" Text="Fecha Recepción Notas Medicas" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaRecepcionNM" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaRecepcionNM">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateRecepcion" runat="server" TargetControlID="TxtFechaRecepcionNM" PopupButtonID="BtnFechaRecepcionNM" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblHoraRecepcionNM" runat="server" Text="Hora Recepción Notas Medicas" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtHoraRecepcionNM" runat="server" CssClass="form-control form-control-sm" 
                                            onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                            oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                            MaxLength ="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaAlta" runat="server" Text="Fecha Alta Hospitalaria" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaAlta" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaAlta">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateAlta" runat="server" TargetControlID="TxtFechaAlta" PopupButtonID="BtnFechaAlta" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblHoraAlta" runat="server" Text="Hora Alta Hospitalaria" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtHoraAlta" runat="server" CssClass="form-control form-control-sm" 
                                            onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                            oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                            MaxLength ="5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaEnvio" runat="server" Text="Fecha Envio Expediente" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaEnvio" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaEnvio">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateEnvio" runat="server" TargetControlID="TxtFechaEnvio" PopupButtonID="BtnFechaEnvio" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblHoraEnvio" runat="server" Text="Hora Envio Expediente" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtHoraEnvio" runat="server" CssClass="form-control form-control-sm" 
                                            onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                            oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                            MaxLength ="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-lg-4 col-md-4">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaVigencia" runat="server" Text="Vigencia de la carta hasta" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaVigencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaVigencia">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateVigencia" runat="server" TargetControlID="TxtFechaVigencia" PopupButtonID="BtnFechaVigencia" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblMontoAutorizado" runat="server" Text="Monto Autorizado" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
<%--                                    
                                    <asp:TextBox ID="TxtMontoAutorizado" runat="server" CssClass="form-control form-control-sm"
                                      onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                      oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength ="11"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revMontoAutorizado" runat="server" ControlToValidate="TxtMontoAutorizado" 
                                        ValidationExpression="^\d+(\.\d{0,2})?$" Display="None" EnableClientScript="true"/>
--%>
                                    <asp:TextBox ID="TxtMontoAutorizado" runat="server" CssClass="form-control form-control-sm"
                                        onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                        oninput="this.value = this.value.replace(/[^0-9.]/g, '');" MaxLength="11"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    <div class="row mt-3">
                        <div class="col-lg-8 col-md-8">
                            <div class ="mb-2">
                                <asp:Label ID="LblCalleAtencion" runat="server" Text="Calle" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtCalleAtencion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-2">
                            <div class ="mb-2">
                                <asp:Label ID="LblNumExtAtencion" runat="server" Text="Num. Exterior" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtNumExtAtencion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-2">
                            <div class ="mb-2">
                                <asp:Label ID="LblNumIntAtencion" runat="server" Text="Num. Interior" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtNumIntAtencion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row mt-3">
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblEstadoAtencion" runat="server" Text="Estado" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlEstadoAtencion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoAtencion_SelectedIndexChanged" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblMunicipiosAtencion" runat="server" Text="Delegación / Municipio" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlMunicipiosAtencion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipiosAtencion_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblColoniaAtencion" runat="server" Text="Colonia" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtColoniaAtencion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-1 col-md-1 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblCPostalAtencion" runat="server" Text="C.Postal" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtCPostalAtencion" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblDiagnostico" runat="server" Text="Diagnostico"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDiagnostico" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblObservaciones_DA" runat="server" Text="Observaciones"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtObservaciones_DA" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblPlanTratamiento" runat="server" Text="Plan de Tratamiento"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtPlanTratamiento" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <!-- Botón Guardar y Actualizar para el panel 13 -->
                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl13" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl13_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl13" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl13_Click" CssClass="btn btn-primary" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl13" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl13_Click" CssClass="btn btn-secondary" visible="false" TabIndex="2"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>
                </div>

                <div class="row mb-4 mt-3" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl15" runat="server" Text="OTROS SERVICIOS" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:Button ID="btnShowPanel15" runat="server" Text="&#9660;" OnClick="btnShowPanel15_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="pnl15" runat="server" Visible="false">
                        <div class="row mb-3">
                            <div class="col-lg-4 col-md-4 ">
                                <div class ="mb-2">
                                    <asp:Label ID="LblServicios" runat="server" Text="Servicios" CssClass="control-label" Font-Size="Small"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="ddlServicios" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlServicios_SelectedIndexChanged" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblFechaServicio" runat="server" Text="Fecha Servicio" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtFechaServicio" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaServicio">
                                        <span class="visually-hidden">Toggle Dropdown</span>
                                    </button>
                                    <ajaxToolkit:CalendarExtender ID="dateFechaServicio" runat="server" TargetControlID="TxtFechaServicio" PopupButtonID="BtnFechaServicio" Format="dd/MM/yyyy" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <div class="mb-2">
                                    <asp:Label ID="LblHoraServicio" runat="server" Text="Hora Servicio" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtHoraServicio" runat="server" CssClass="form-control form-control-sm" 
                                            onkeydown="if(event.key === 'Enter'){event.preventDefault(); return false;}"
                                            oninput="this.value = this.value.replace(/[^0-9:]/g, '');"  
                                            MaxLength ="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-lg-12 col-md-12">
                                <div class ="mb-2">
                                    <asp:Label ID="LblDescServicio" runat="server" Text="Descripción Servicio"></asp:Label>
                                </div>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="TxtDescServicio" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="1250" Columns="12" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <!-- Botón Guardar y Actualizar para el panel 14 -->
                        <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAnularPnl14" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnularPnl14_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                    <asp:Button ID="btnEditarPnl14" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="btnEditarPnl14_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                                    <asp:Button ID="btnActualizarPnl14" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="btnActualizarPnl14_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                                    <asp:Button ID="BtnAgregarPnl14" runat="server" Text="Agregar" OnClick="BtnAgregarPnl14_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="row mt-5">
                            <%-- Consulta de Servicios Autorizados --%>
                            <div style="overflow-x: auto; overflow-y:hidden">
                                <asp:GridView ID="GrdServicios"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                        OnPageIndexChanging="GrdServicios_PageIndexChanging" OnRowCommand="GrdServicios_RowCommand" OnPreRender="GrdServicios_PreRender"
                                        OnSelectedIndexChanged="GrdServicios_SelectedIndexChanged" OnRowDataBound="GrdServicios_RowDataBound" 
                                        DataKeyNames="IdConsecutivo" PageSize="5" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:BoundField DataField="IdConsecutivo" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IdServicio" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Descripcion" HeaderText="Servicio" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FechaServicio" HeaderText="Fecha Servicio" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Hora_Servicio" HeaderText="Hora Servicio" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Desc_Servicio" HeaderText="Descripción" >
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgServicio_Add" runat="server" OnClick="ImgServicio_Add_Click" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png" Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgServicio_Del" runat="server" OnClick="ImgServicio_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png" Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>

                </div>

            </div>

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
            <asp:Panel ID="pnlCveProveedor" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;">
                <div class="row justify-content-end" data-bs-theme="dark">
                    <div class="col-1">
                        <asp:Button runat="server" class="btn-close" aria-label="Close" />
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel17" runat="server" >
                    <ContentTemplate>
                        <div class="container">
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div>
                                <br />
        <%--
                                <div class="d-flex flex-row mx-4 m-0 p-0">
                                    <asp:Label ID="LblCarpetas" runat="server" Text="Carpeta en la cual desea trabajar" CssClass="form-label my-0 p-0" />
                                </div>
                                <div class="col-sm-6 mx-3">
                                    <asp:DropDownList ID="ddlCarpetas" runat="server" CssClass="form-select form-control-sm" OnSelectedIndexChanged="ddlCarpetas_SelectedIndexChanged" AutoPostBack ="true" >
                                    </asp:DropDownList>
                                </div>
        --%>
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">  
                                        <ContentTemplate>
                                        <asp:GridView ID="GrdDatosProveedor" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" 
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GrdDatosProveedor_PageIndexChanging" OnRowDataBound="GrdDatosProveedor_RowDataBound"
                                            PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Tpo_Servicio" >
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Nom_Empresa" HeaderText="Nombre Empresa" >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Email_Empresa" HeaderText="Correo Electronico" >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Calle" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Num_Exterior" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Num_Interior" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estado" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Delegacion" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Colonia" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Codigo_Postal" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Tel_Contacto_1" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Tel_Contacto_2" >
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <asp:Button ID="btnAgregar_Proveedor" runat="server" Text="Agregar" OnClick="btnAgregar_Proveedor_Click" CssClass="btn btn-outline-primary btn-sm" />
                                <asp:Button ID="btnClose_Proveedor" runat="server" Text="Cerrar" OnClick="btnClose_Proveedor_Click" CssClass="btn btn-outline-secondary btn-sm" />
                            </div>
                            <br />
                        </div>
                    </ContentTemplate>
                    <Triggers>

                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlCveICD" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;">
                <div class="row justify-content-end" data-bs-theme="dark">
                    <div class="col-1">
                        <asp:Button runat="server" class="btn-close" aria-label="Close" />
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel8" runat="server" >
                    <ContentTemplate>
                        <div class="container">
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div>
                                <br />
        <%--
                                <div class="d-flex flex-row mx-4 m-0 p-0">
                                    <asp:Label ID="LblCarpetas" runat="server" Text="Carpeta en la cual desea trabajar" CssClass="form-label my-0 p-0" />
                                </div>
                                <div class="col-sm-6 mx-3">
                                    <asp:DropDownList ID="ddlCarpetas" runat="server" CssClass="form-select form-control-sm" OnSelectedIndexChanged="ddlCarpetas_SelectedIndexChanged" AutoPostBack ="true" >
                                    </asp:DropDownList>
                                </div>
        --%>
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">  
                                        <ContentTemplate>
                                        <asp:GridView ID="GrdICD" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" 
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GrdICD_PageIndexChanging" OnRowDataBound="GrdICD_RowDataBound"
                                            PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Cve_ICD" HeaderText="Cve." >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Desc_ICD" HeaderText="Descripción ICD" >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <asp:Button ID="btnAgregar_ICD" runat="server" Text="Agregar" OnClick="btnAgregar_ICD_Click" CssClass="btn btn-outline-primary btn-sm" />
                                <asp:Button ID="btnClose_ICD" runat="server" Text="Cerrar" OnClick="btnClose_ICD_Click" CssClass="btn btn-outline-secondary btn-sm" />
                            </div>
                            <br />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:PostBackTrigger ControlID="GrdDocCarpeta" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlCveCPT" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;">
                <div class="row justify-content-end" data-bs-theme="dark">
                    <div class="col-1">
                        <asp:Button runat="server" class="btn-close" aria-label="Close" />
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel14" runat="server" >
                    <ContentTemplate>
                        <div class="container">
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div>
                                <br />
        <%--
                                <div class="d-flex flex-row mx-4 m-0 p-0">
                                    <asp:Label ID="LblCarpetas" runat="server" Text="Carpeta en la cual desea trabajar" CssClass="form-label my-0 p-0" />
                                </div>
                                <div class="col-sm-6 mx-3">
                                    <asp:DropDownList ID="ddlCarpetas" runat="server" CssClass="form-select form-control-sm" OnSelectedIndexChanged="ddlCarpetas_SelectedIndexChanged" AutoPostBack ="true" >
                                    </asp:DropDownList>
                                </div>
        --%>
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">  
                                        <ContentTemplate>
                                        <asp:GridView ID="GrdCPT" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" 
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GrdCPT_PageIndexChanging" OnRowDataBound="GrdCPT_RowDataBound"
                                            PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Cve_CPT" HeaderText="Cve." >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Desc_CPT" HeaderText="Descripción CPT" >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <asp:Button ID="btnAgregar_CPT" runat="server" Text="Agregar" OnClick="btnAgregar_CPT_Click" CssClass="btn btn-outline-primary btn-sm" />
                                <asp:Button ID="btnClose_CPT" runat="server" Text="Cerrar" OnClick="btnClose_CPT_Click" CssClass="btn btn-outline-secondary btn-sm" />
                            </div>
                            <br />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:PostBackTrigger ControlID="GrdDocCarpeta" />--%>
                    </Triggers>
                </asp:UpdatePanel>
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
                        <div class="form-group">
                            <div class="d-grid col-6 mx-auto">
                                <ajaxToolkit:ModalPopupExtender ID="mpeNewICD" runat="server" PopupControlID="pnlCveICD"
                                    TargetControlID="LblOculto4" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProcesoOnOK()" >
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="LblOculto4" runat="server" Text="Label" Style="display: none;" />
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <div class="d-grid col-6 mx-auto">
                                <ajaxToolkit:ModalPopupExtender ID="mpeNewCPT" runat="server" PopupControlID="pnlCveCPT"
                                    TargetControlID="LblOculto5" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProcesoOnOK()" >
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="LblOculto5" runat="server" Text="Label" Style="display: none;" />
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <div class="d-grid col-6 mx-auto">
                                <ajaxToolkit:ModalPopupExtender ID="mpeNewProveedor" runat="server" PopupControlID="pnlCveProveedor"
                                    TargetControlID="LblOculto6" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProcesoOnOK()" >
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="LblOculto6" runat="server" Text="Label" Style="display: none;" />
                            </div>
                        </div>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
        <Triggers>

            <asp:PostBackTrigger ControlID="BtnGraba_Categorias" />
            <asp:PostBackTrigger ControlID="BtnOne" />
            <asp:PostBackTrigger ControlID="BtnTwo" />
            <asp:PostBackTrigger ControlID="BtnThree" />
            <asp:PostBackTrigger ControlID="BtnFour" />
            <asp:PostBackTrigger ControlID="BtnFive" />
            <asp:PostBackTrigger ControlID="BtnSix" />
            <asp:PostBackTrigger ControlID="BtnSeven" />
            <%--<asp:PostBackTrigger ControlID="BtnEight" />--%>
            <%--<asp:PostBackTrigger ControlID="BtnNine" />--%>

            <asp:PostBackTrigger ControlID="BtnProveedor" />
            <asp:PostBackTrigger ControlID="BtnICD" />
            <asp:PostBackTrigger ControlID="BtnCPT" />
            

            <asp:PostBackTrigger ControlID="btnAgregar_Proveedor" />
            <asp:PostBackTrigger ControlID="btnAgregar_ICD" />
            <asp:PostBackTrigger ControlID="btnAgregar_CPT" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl2" />
            <asp:PostBackTrigger ControlID="btnEditarPnl2" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl2" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl3" />
            <asp:PostBackTrigger ControlID="btnEditarPnl3" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl3" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl4" />
            <asp:PostBackTrigger ControlID="btnEditarPnl4" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl4" />

<%--
            <asp:PostBackTrigger ControlID="BtnAnularPnl5" />
            <asp:PostBackTrigger ControlID="btnEditarPnl5" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl5" />
--%>

            <asp:PostBackTrigger ControlID="BtnAnularPnl6" />
            <asp:PostBackTrigger ControlID="btnEditarPnl6" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl6" />

            <asp:PostBackTrigger ControlID="BtnAgregarPnl7" />
            <asp:PostBackTrigger ControlID="BtnAnularPnl7" />
            <asp:PostBackTrigger ControlID="btnEditarPnl7" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl7" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl8" />
            <asp:PostBackTrigger ControlID="btnEditarPnl8" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl8" />

            <asp:PostBackTrigger ControlID="BtnAgregarPnl9" />
            <asp:PostBackTrigger ControlID="BtnAnularPnl9" />
            <asp:PostBackTrigger ControlID="btnEditarPnl9" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl9" />

            <asp:PostBackTrigger ControlID="BtnAnularPnl13" />
            <asp:PostBackTrigger ControlID="btnEditarPnl13" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl13" />

            <asp:PostBackTrigger ControlID="BtnAgregarPnl14" />
            <asp:PostBackTrigger ControlID="BtnAnularPnl14" />
            <asp:PostBackTrigger ControlID="btnEditarPnl14" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl14" />

            <asp:PostBackTrigger ControlID="BtnAgregarPnl16" />
            <asp:PostBackTrigger ControlID="BtnAnularPnl16" />
            <asp:PostBackTrigger ControlID="btnEditarPnl16" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl16" />

        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
