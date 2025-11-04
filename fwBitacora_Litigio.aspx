<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwBitacora_Litigio.aspx.cs" Inherits="WebItNow_Peacock.fwBitacora_Litigio" %>
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
                document.getElementById('<%=LblExpira.ClientID %>').innerHTML = document.getElementById('<%=litSesionExpirada.ClientID %>').innerHTML;
        
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
                            <asp:Label ID="LblAntReferencia" runat="server" Text="Referencia Anterior" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtAntReferencia" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div id="divSiniestro_Proyect" runat="server" class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblSiniestro_Proyect" runat="server" Text="Siniestro" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSiniestro_Proyect" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblExpediente_Proyect" runat="server" Text="Expediente" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtExpediente_Proyect" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblNumSiniestro" runat="server" Text="No. Siniestro" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumSiniestro" runat="server" CssClass="form-control form-control-sm" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
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
                            <asp:Label ID="LblFechaReporte" runat="server" Text="Fecha Reporte" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtFechaReporte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaReporte">
                                <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ajaxToolkit:CalendarExtender ID="dateFecReporte" runat="server" TargetControlID="TxtFechaReporte" PopupButtonID="BtnFechaReporte" Format="dd/MM/yyyy" />
                        </div>
                    </div>

                    <div id="divFecAviso_Proyect" runat="server" class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblFechaAviso" runat="server" Text="Fecha Aviso a la GAAR" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtFechaAviso" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaAviso">
                                <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ajaxToolkit:CalendarExtender ID="dateFecAviso" runat="server" TargetControlID="TxtFechaAviso" PopupButtonID="BtnFechaAviso" Format="dd/MM/yyyy" />
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblVigencia" runat="server" Text="Vigencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtVigencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnVigencia">
                                <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ajaxToolkit:CalendarExtender ID="dateFecVigencia" runat="server" TargetControlID="TxtVigencia" PopupButtonID="BtnVigencia" Format="dd/MM/yyyy" />
                        </div>
                    </div>

                    <div class="col-lg-8 col-md-8">
                        <div class="mb-2">
                            <asp:Label ID="LblNomAjustador" runat="server" Text="Nombre del Ajustador" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomAjustador" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                    <div class="col-lg-1 col-md-1 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblCodigoPostal" runat="server" Text="C.Postal" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCodigoPostal" runat="server" CssClass="form-control form-control-sm" MaxLength="5"></asp:TextBox>
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
                </div>

                <div class="row mt-3">
                    <div class="col-lg-8 col-md-8">
                        <div class="mb-2">
                            <asp:Label ID="LblNomAfectado" runat="server" Text="Nombre del Afectado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomAfectado" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" onkeyup="mayus(this);" MaxLength="80"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-8 col-md-8">
                        <div class="mb-2">
                            <asp:Label ID="LblNomActor" runat="server" Text="Nombre del Actor" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomActor" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" onkeyup="mayus(this);" MaxLength="80"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-8 col-md-8">
                        <div class="mb-2">
                            <asp:Label ID="LblNomDemandado" runat="server" Text="Nombre del Demandado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomDemandado" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" onkeyup="mayus(this);" MaxLength="80"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div>
                <asp:Panel ID="pnl2" runat="server" Visible="true">
                    <div class="row mt-3">
                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
                                <asp:Label ID="LblTribunal" runat="server" Text="Tribunal" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtTribunal" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
                                <asp:Label ID="LblExpLitigio" runat="server" Text="Expediente del Litigio" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtExpLitigio" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4">
                            <div class="mb-2">
			                    <asp:Label ID="LblTpoJuicio" runat="server" Text="Tipo de Procedimiento" CssClass="form-label"></asp:Label>
                            </div>
<%--
                            <div class="input-group input-group-sm">
			                    <asp:TextBox ID="TxtTpoJuicio" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
--%>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlProcedimiento" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProcedimiento_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <!-- Botón Guardar y Actualizar para el panel 2 -->
                    <div class="d-grid gap-4 d-flex justify-content-center mt-3 mb-3">
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
                            <asp:Button ID="btnShowPanel1" runat="server" Text="&#9660;" OnClick="btnShowPanel1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
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
                                <asp:Label ID="LblTpoEstatus" runat="server" Text="Estatus" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlConclusion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlConclusion_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                &nbsp;
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="ImgDel_Documento" runat="server" OnClick="ImgDel_Documento_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
                                        <span class="center-text"> &nbsp;&nbsp; Eliminar Documento(s)</span>
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
                                    OnPageIndexChanging="GrdDocumentos_PageIndexChanging" OnRowCommand="GrdDocumentos_RowCommand"
                                    OnSelectedIndexChanged="GrdDocumentos_SelectedIndexChanged" OnRowDataBound ="GrdDocumentos_RowDataBound"
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
                                    <%--<PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />--%>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="d-grid gap-4 d-flex justify-content-center mt-4 mb-3">
                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnCartaSolicitud" runat="server" Text="Carta Solicitud" OnClick="BtnCartaSolicitud_Click" CssClass="btn btn-primary" TabIndex="8"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </asp:Panel>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgDel_Documento" />

            <asp:PostBackTrigger ControlID="BtnGraba_Categorias" />
            <asp:PostBackTrigger ControlID="BtnCartaSolicitud" />
            
            <asp:PostBackTrigger ControlID="BtnAnularPnl2" />
            <asp:PostBackTrigger ControlID="btnEditarPnl2" />
            <asp:PostBackTrigger ControlID="btnActualizarPnl2" />

        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlExpira" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
        <div class=" row justify-content-end" data-bs-theme="dark">
            <div class="col-1">
                <asp:Literal ID="litSesionExpirada" runat="server" Visible="false" Text="<%$ Resources:GlobalResources, msgSesionExpirada %>" />
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
                                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_2" runat="server" PopupControlID="pnlMensaje_2"
                                    TargetControlID="lblOculto_2" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="lblOculto_2" runat="server" Text="Label" Style="display: none;" />
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
