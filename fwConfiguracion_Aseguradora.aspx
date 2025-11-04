<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwConfiguracion_Aseguradora.aspx.cs" Inherits="WebItNow_Peacock.fwConfiguracion_Aseguradora" %>
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

        function mpeExpiraOnOk() {

        }

        function mpeNewProcesoOnOK() {
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
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Configuración de Aseguradora</h2>
                        </div>
                    </div>
                </div>

                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-1" style="font-size:small">&nbsp;</h5>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblNomCliente" runat="server" Text="Nombre del Cliente" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtNomCliente" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre del Cliente" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>

<%--
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblProyectos" runat="server" Text="Proyecto" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlProyecto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
--%>

                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblContactos" runat="server" Text="Contacto" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlContactos" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlContactos_SelectedIndexChanged" onchange="this.form.submit();" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-1" style="font-size:small">&nbsp;</h5>
                </div>

                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnAgregarDatos" runat="server" Text="Agregar" OnClick="BtnAgregarDatos_Click" CssClass="btn btn-primary" Visible="true" TabIndex="0" />
                            <asp:Button ID="btnEditar" runat="server" Text="Editar Datos" OnClick="BtnEditar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="1" />
                            <asp:Button ID="BtnGrabar" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2" />
                            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-primary" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblLineaNegocios" runat="server" Text="LINEA DE NEGOCIOS" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <div class="row mb-2">
                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
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

<%--
                <br />
                <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblProtocolos" runat="server" Text="PROTOCOLO DE SERVICIO (SLA)" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <div class="row mb-2">
                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
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
--%>
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
            <table cellspacing="1" cellpadding="1" border="0">
                <tr>
                    <td>
                        <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                            TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                    </td>
                    <td>
                        <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                            TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <ajaxToolkit:ModalPopupExtender ID="mpeNewProceso" runat="server" PopupControlID="PnlDocProceso"
                            TargetControlID="LblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProcesoOnOK()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="LblOculto3" runat="server" Text="Label" Style="display: none;" />
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
