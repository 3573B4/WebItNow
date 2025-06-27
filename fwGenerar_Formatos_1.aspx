<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGenerar_Formatos_1.aspx.cs" Inherits="WebItNow_Peacock.fwGenerar_Formatos_1" %>
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

        $(document).ready(function () {
            var timer = setTimeout(function () {
                document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
                var modalId = '<%=mpeExpira.ClientID%>';
                var modal = $find(modalId);
                modal.show();
            }, 3600000);
        });

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container col-lg-7 col-md-8 col-sm-8 py-5">
                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h6 fw-normal my-1" style="font-size:small">INFORMACIÓN GENERAL</h5>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblReferencia" runat="server" Text="No. Referencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblNumReporte" runat="server" Text="No. Reporte" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumReporte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblNumSiniestro" runat="server" Text="No. Siniestro" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumSiniestro" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblNumPoliza" runat="server" Text="No. Póliza" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumPoliza" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
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
                            <asp:Label ID="LblRiesgo" runat="server" Text="Riesgo Materializado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtRiesgo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoContratante" runat="server" Text="Tipo Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                <div class="row mb-3">
                    <div class="col-lg-12 col-md-12">
                        <div class ="mb-2">
                            <asp:Label ID="LblLugarSiniestro" runat="server" Text="Lugar del Siniestro"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtLugarSiniestro" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h6 fw-normal my-1" style="font-size:small">DATOS PERSONALES</h5>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblAsegurado_1" runat="server" Text="Asegurado 1" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtAsegurado_1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblAsegurado_2" runat="server" Text="Asegurado 2" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtAsegurado_2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblAsegurado_3" runat="server" Text="Asegurado 3" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtAsegurado_3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTelReporte" runat="server" Text="Telefono Casa Reporte" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTelReporte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblTelOficina" runat="server" Text="Telefono Oficina Reporte" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTelOficina" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCelReporte" runat="server" Text="Celular Reporte" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCelReporte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTelAsegPoliza" runat="server" Text="Telefono Asegurado Póliza" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTelAsegPoliza" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblNomContratante" runat="server" Text="Nombre Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTelContratante" runat="server" Text="Telefono Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTelContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblMailContratante" runat="server" Text="Mail Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtMailContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblCelContratante" runat="server" Text="Celular Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCelContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblBeneficiario" runat="server" Text="Beneficiario" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtBeneficiario" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h6 fw-normal my-1" style="font-size:small">INFORMACIÓN PÓLIZA</h5>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCalleBienAsegurado" runat="server" Text="Calle Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCalleBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblColoniaBienAsegurado" runat="server" Text="Colonia Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtColoniaBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblPoblacionBienAsegurado" runat="server" Text="Poblacion Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPoblacionBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblEstadoBienAsegurado" runat="server" Text="Estado Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEstadoBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblMunicipioBienAsegurado" runat="server" Text="Municipio Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtMunicipioBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCPostalBienAsegurado" runat="server" Text="C.Postal Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCPostalBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoProducto" runat="server" Text="Tipo Producto" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoProducto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoPlan" runat="server" Text="Tipo Plan" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoPlan" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoMoneda" runat="server" Text="Tipo Moneda" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoMoneda" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblNumCertificado" runat="server" Text="No. Certificado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNumCertificado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblPlazo" runat="server" Text="Plazo" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPlazo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCanalVentas" runat="server" Text="Canal de Ventas" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCanalVentas" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
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
                            <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFecFinVigencia">
                                <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ajaxToolkit:CalendarExtender ID="dateFecFinVigencia" runat="server" TargetControlID="TxtFechaFinVigencia" PopupButtonID="BtnFecFinVigencia" Format="dd/MM/yyyy" />
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCalleAsegurado" runat="server" Text="Calle Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCalleAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblColoniaAsegurado" runat="server" Text="Colonia Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtColoniaAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblPoblacionAsegurado" runat="server" Text="Poblacion Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPoblacionAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblEstadoAsegurado" runat="server" Text="Estado Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEstadoAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblMunicipioAsegurado" runat="server" Text="Municipio Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtMunicipioAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCPostalAsegurado" runat="server" Text="C.Postal Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCPostalAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCalleContratante" runat="server" Text="Calle Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCalleContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblColoniaContratante" runat="server" Text="Colonia Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtColoniaContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblPoblacionContratante" runat="server" Text="Poblacion Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPoblacionContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblEstadoContratante" runat="server" Text="Estado Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEstadoContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblMunicipioContratante" runat="server" Text="Municipio Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtMunicipioContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblCPostalContratante" runat="server" Text="C.Postal Contratante" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCPostalContratante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoTecho" runat="server" Text="Tipo Techo" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoTecho" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoVivienda" runat="server" Text="Tipo Vivienda" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoVivienda" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTpoMuro" runat="server" Text="Tipo Muro" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoMuro" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblPisosBienAsegurado" runat="server" Text="Pisos Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPisosBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblPisosDelBienAsegurado" runat="server" Text="Pisos del Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPisosDelBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblLocalesComerciales" runat="server" Text="Locales_Comerciales" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtLocalesComerciales" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblDetalleBienAsegurado" runat="server" Text="Detalles Bien Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtDetallesBienAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblReglaSumaAsegurada" runat="server" Text="Regla Suma Asegurada" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtReglaSumaAsegurada" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                        </div>
                        <div class="input-group input-group-sm">
                        </div>
                    </div>
                </div>
                <br />
                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="BtnEditar" runat="server" Text="Editar Datos" OnClick="BtnEditar_Click" CssClass="btn btn-primary" TabIndex="0"/>
                            <asp:Button ID="BtnGrabar" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="1"/>
                            <asp:Button ID="BtnGeneraDocumento" runat="server" Text="Genera Formato" Font-Bold="True" OnClick="BtnGeneraDocumento_Click" CssClass="btn btn-primary" TabIndex="2"/>
                            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-secondary" TabIndex="3"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnEditar" />
            <asp:PostBackTrigger ControlID="BtnGrabar" /> 
            <asp:PostBackTrigger ControlID="BtnGeneraDocumento" />
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
