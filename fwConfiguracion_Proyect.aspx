<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" EnableViewState="true" AutoEventWireup="true" CodeBehind="fwConfiguracion_Proyect.aspx.cs" Inherits="WebItNow_Peacock.fwConfiguracion_Proyect" %>
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

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        function mpeNewDocumento() {
            //
        }

        function mpeNewProceso() {

        }

        function mpeExpiraOnOk() {

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

        function CerrarPanel() {
            // 1️⃣ Ocultar el panel al instante
            var pnl = document.getElementById('<%= PnlDocProceso.ClientID %>');
            if (pnl) pnl.style.display = 'none';

            // 2️⃣ Llamar al WebMethod para limpiar la variable
            if (typeof PageMethods !== "undefined") {
                PageMethods.LimpiarVariable(onSuccess, onError);
            }

            function onSuccess() {
                console.log('Variable limpiada correctamente.');
            }
            function onError(err) {
                console.error('Error al limpiar variable: ', err);
            }
        }

    </script>
    
    <style type="text/css">
        .disabled-checkbox {
            opacity: 0.7;               /* Cambia la opacidad para dar un efecto visual similar */
            background-color: #f5f5f5;  /* Fondo más claro */
            color: #333;                /* Color de texto oscuro */
            pointer-events: none;       /* Evita que los usuarios interactúen con él */
            cursor: not-allowed;        /* Cambia el cursor para indicar que no se puede hacer clic */
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row mb-2 py-4">
                    <div class="col-lg-4 col-md-4 ">
                    </div>
                    <div class="col-lg-8 col-md-8">
                        <div class="input-group input-group-sm">
                            <%--<h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Configuración de Categorías al Proyecto</h2>--%>
                            <asp:Label ID="lblTitulo_Categoria_Proyecto" runat="server" CssClass="h2 mb-3 fw-normal mt-4 align-content-center" style="display:block; text-align:center;" ></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-1" style="font-size:small">&nbsp;</h5>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCliente" runat="server" Text="<%$ Resources:GlobalResources, LblCiaSeguros %>" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <%--<asp:DropDownList ID="ddlCliente" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged" Width="100%">--%>
                            <%--</asp:DropDownList>--%>
                            <asp:TextBox ID="TxtCliente" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, LblCiaSeguros %>" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblProyecto" runat="server" Text="<%$ Resources:GlobalResources, lblNomProyecto %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <%--<asp:DropDownList ID="ddlProyecto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" Width="100%">--%>
                            <%--</asp:DropDownList>--%>
                            <asp:TextBox ID="TxtNomProyecto" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblNomProyecto %>" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTpoAsunto" runat="server" Text="<%$ Resources:GlobalResources, lblTipoAsunto %>" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoAsunto" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblTipoAsunto %>" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                </div>


                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-1" style="font-size:small">&nbsp;</h5>
                </div>


                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:Button ID="BtnAgregarDatos" runat="server" Text="<%$ Resources:GlobalResources, btnAgregar %>" OnClick="BtnAgregarDatos_Click" CssClass="btn btn-primary" Visible="true" TabIndex="0" />
                    <asp:Button ID="btnEditar" runat="server" Text="<%$ Resources:GlobalResources, btnEditar %>" OnClick="BtnEditar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="1" />
                    <asp:Button ID="BtnGrabar" runat="server" Text="<%$ Resources:GlobalResources, btnGrabar %>" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2" />
                    <asp:Button ID="BtnRegresar" runat="server" Text="<%$ Resources:GlobalResources, btnRegresar %>" OnClick="BtnRegresar_Click" CssClass="btn btn-primary" />

                    <%--<asp:Button ID="btnEliminarDatos" runat="server" Text="Eliminar" OnClick="btnEliminarDatos_Click" CssClass="btn btn-primary" Enabled="false" />--%>
                    <%--<asp:Button ID="btnEditarDatos" runat="server" Text="Editar Datos" OnClick="btnEditarDatos_Click" CssClass="btn btn-primary" Enabled="false" />--%>
                </div>
                <br />

                <%--controles pnl1--%>
                <div class="row" style="background-color:mediumturquoise; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl1" runat="server" Text="<%$ Resources:GlobalResources, hdrTituloDocumentos %>" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
<%--                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                    <asp:Button ID="btnShowPanel1" runat="server" Text="&#9660;" OnClick="btnShowPanel1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
<%--                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>

                <div>
                    <asp:Panel ID="pnl1" runat="server" Visible="true">
<%--
                        <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                            <h5 class="h1 fw-normal my-1" style="font-size:small">ASEGURADOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)</h5>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl2" runat="server" Text="<%$ Resources:GlobalResources, hdrTituloAsegurados %>" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion1" runat="server" Text="+" OnClick="BtnSeccion1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_1" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                            AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_1_RowDataBound" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Columna1" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_1_1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna2" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_1_2" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna3" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_1_3" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_1" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2 mt-2">
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                                <div class=" input-group input-group-sm">
                                    <asp:Button ID="BtnPnl1Seleccionar_1" runat="server" Text="<%$ Resources:GlobalResources, btnSeleccionar %>" OnClick="BtnPnl1Seleccionar_1_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

<%--
                        <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                            <h5 class="h1 fw-normal my-1" style="font-size:small">ASUNTOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)</h5>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl3" runat="server" Text="<%$ Resources:GlobalResources, hdrTituloRiesgos %>" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion2" runat="server" Text="+" OnClick="BtnSeccion2_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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

                        <div class="row mb-2 mt-2">
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                                <div class=" input-group input-group-sm">
                                    <asp:Button ID="BtnPnl1Seleccionar_2" runat="server" Text="<%$ Resources:GlobalResources, btnSeleccionar %>" OnClick="BtnPnl1Seleccionar_2_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

<%--
                        <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                            <h5 class="h1 fw-normal my-1" style="font-size:small">CUADERNOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)</h5>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl4" runat="server" Text="<%$ Resources:GlobalResources, hdrTituloEstatus %>" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion3" runat="server" Text="+" OnClick="BtnSeccion3_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_3" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                            AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_3_RowDataBound" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Columna1" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_3_1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna2" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_3_2" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna3" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_3_3" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_3" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2 mt-2">
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                                <div class=" input-group input-group-sm">
                                    <asp:Button ID="BtnPnl1Seleccionar_3" runat="server" Text="<%$ Resources:GlobalResources, btnSeleccionar %>" OnClick="BtnPnl1Seleccionar_3_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

<%--
                        <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                            <h5 class="h1 fw-normal my-1" style="font-size:small">BIENES A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)</h5>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl5" runat="server" Text="<%$ Resources:GlobalResources, hdrTituloBienes %>" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion4" runat="server" Text="+" OnClick="BtnSeccion4_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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

                        <div class="row mb-2 mt-2">
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                                <div class=" input-group input-group-sm">
                                    <asp:Button ID="BtnPnl1Seleccionar_4" runat="server" Text="<%$ Resources:GlobalResources, btnSeleccionar %>" OnClick="BtnPnl1Seleccionar_4_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblEtiquetaPnl6" runat="server" Text="<%$ Resources:GlobalResources, hdrTituloOtrosDetalles %>" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion5" runat="server" Text="+" OnClick="BtnSeccion5_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
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

                        <div class="row mb-2 mt-2">
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                            </div>
                            <div class="col-lg-4 col-md-4">
                                <div class=" input-group input-group-sm">
                                    <asp:Button ID="BtnPnl1Seleccionar_5" runat="server" Text="<%$ Resources:GlobalResources, btnSeleccionar %>" OnClick="BtnPnl1Seleccionar_5_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </asp:Panel>
                </div>
                <br />
                <!-- Puedes agregar más botones y paneles según sea necesario -->
            </div>

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
                    <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="<%$ Resources:GlobalResources, btnCerrar %>" CssClass="btn btn-outline-primary"/>
                </div>
            </asp:Panel>
            <br />
            <asp:Panel ID="PnlDocProceso" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;">
                <div class="row justify-content-end" data-bs-theme="dark">
                    <div class="col-1">
                        <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server" >
                    <ContentTemplate>
                        <div class="container">
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div>
                                <br />
                                <div class="d-flex flex-row mx-4 m-0 p-0">
                                    <asp:Label ID="LblDescCategoria" runat="server" Text="<%$ Resources:GlobalResources, lblDescCategoria %>" CssClass="form-label my-0 p-0" />
                                </div>
                                <div class="col-sm-6 mx-3">
                                    <asp:Panel runat="server" DefaultButton="BtnBuscar">
                                        <asp:TextBox ID="TxtDescripcion" runat="server" placeholder="<%$ Resources:GlobalResources, lblDescCategoria %>" CssClass="form-control form-control-sm" onkeyup="mayus(this);"></asp:TextBox>
                                        <asp:Button ID="BtnBuscar" runat="server" OnClick="BtnBuscar_Click" Style="display: none" />
                                    </asp:Panel>
                                </div>
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                        <asp:GridView ID="grdPnlBusqProceso" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="False" CssClass="table table-responsive table-light table-striped table-hover align-middle"
                                            AlternatingRowStyle-CssClass="alt" OnRowCommand="grdPnlBusqProceso_RowCommand" 
                                            OnRowDataBound="grdPnlBusqProceso_RowDataBound"
                                            Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IdDocumento" HeaderText="" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:GlobalResources, col_DescCategoria %>" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkQuitar" runat="server" CommandName="Quitar" CommandArgument='<%# Container.DataItemIndex %>' Text="<%$ Resources:GlobalResources, lblEliminar %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <asp:Button ID="btnAgregar_Proceso" runat="server" Text="<%$ Resources:GlobalResources, btnAgregar %>" OnClick="btnAgregar_Proceso_Click" CssClass="btn btn-outline-primary btn-sm" />
                                <asp:Button ID="btnClose_Proceso" runat="server" OnClick="btnClose_Proceso_Click" Text="<%$ Resources:GlobalResources, btnCerrar %>" CssClass="btn btn-outline-secondary btn-sm" />
                            </div>
                            <br />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdPnlBusqProceso" />
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
                        </div>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <ajaxToolkit:ModalPopupExtender ID="mpeNewProceso" runat="server" PopupControlID="PnlDocProceso"
                            TargetControlID="LblOculto1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProceso()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="LblOculto1" runat="server" Text="Label" Style="display: none;" />
                    </td>
                </tr>
            </table>
            <br />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="BtnAgregarDatos" />
        <asp:PostBackTrigger ControlID="btnAgregar_Proceso" />
        <asp:PostBackTrigger ControlID="BtnGrabar" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
