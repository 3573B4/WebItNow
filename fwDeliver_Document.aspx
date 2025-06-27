<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwDeliver_Document.aspx.cs" Inherits="WebItNow_Peacock.fwDeliver_Document" %>
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

    </script>
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
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Validación de Entrega de Documentos</h2>
                        </div>
                    </div>
                </div>
            </div>

        <div class="container col-lg-7 col-md-8 col-sm-8">
            <div class="row ">
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblReferencia" runat="server" Text="Referencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control form-control-sm" MaxLength="12" ReadOnly="true" autocomplete="off" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblSiniestro" runat="server" Text="Siniestro" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSiniestro" runat="server" CssClass="form-control form-control-sm" Font-Size="Small" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblPoliza" runat="server" Text="Poliza" CssClass="form-label"></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtPoliza" runat="server" CssClass="form-control form-control-sm" Font-Size="Small" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblAsegurado" runat="server" Text="Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtAsegurado" runat="server" CssClass="form-control form-control-sm" Font-Size="Small" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2 ">
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-3 mt-4">
            <div class="form-group">
                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server"  UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="BtnDocument_Proceso" runat="server" Text="Documentos del Proceso" Font-Bold="True" OnClick="BtnDocument_Proceso_Click" CssClass="btn btn-secondary" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="container col-12 mt-4">
            <div class="row mb-3" style="background-color:#96E7D9; align-items: baseline;">
                <div class="col-10" style="padding-left: 14px;">
                    <asp:Label ID="LblEtiquetaCheck" runat="server" Text="Check List" CssClass="control-label" Font-Size="small"></asp:Label>
                </div>
                <div class="col-2" style="display:flex; justify-content: end;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="BtnAddDocument" runat="server" Text="+" OnClick="BtnAddDocument_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <%-- Check List --%>
            <div style="overflow-x: auto; overflow-y:hidden;">
                <asp:GridView ID="GrdCheck_List"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                        OnPageIndexChanging="GrdCheck_List_PageIndexChanging" OnRowCommand="GrdCheck_List_RowCommand" OnPreRender="GrdCheck_List_PreRender"
                        OnSelectedIndexChanged="GrdCheck_List_SelectedIndexChanged" OnRowDataBound="GrdCheck_List_RowDataBound" 
                        DataKeyNames="Id_CheckList" PageSize="10" Font-Size="Smaller" >
                        <AlternatingRowStyle CssClass="alt autoWidth" />
                        <Columns>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción de Documento" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Fec_Entrega" DataFormatString="{0:d}" HeaderText="Fecha Entrega" >
                            <ItemStyle Width="850" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Entregado" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkEntregado" runat="server" Checked='<%# Eval("Entregado") %>' OnCheckedChanged="ChkEntregado_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                </ItemTemplate> 
                            </asp:TemplateField>
                            <asp:BoundField DataField="IdStatus" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Id_CheckList" >
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgEliminar" runat="server" OnClick="ImgEliminar_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>

            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="10"/>
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
        <asp:Panel ID="PnlNewDocumentos" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width:100%; width:800px; background-color: #FFFFFF;">
            <div class="row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>

            <div class="d-flex flex-row mx-4 m-0 p-0">
                <asp:Label ID="LblTpoDocumento" runat="server" Text="Tipo de documento" CssClass="form-label my-0 p-0 col-sm-3 col-lg-2 text-lg-end"></asp:Label>
            </div>
            <div class="col-sm-4 col-lg-6 mx-3">
                <%--<div class="input-group input-group-sm">--%>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTpoDocumento" runat="server" CssClass="form-select form-control-sm" OnSelectedIndexChanged="ddlTpoDocumento_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <%--</div>--%>
            </div>

            <div class="d-flex flex-row mx-4 m-0 p-0">
                <asp:Label ID="LblDescripcion" runat="server" Text="Descripción del documento" CssClass="form-label my-0 p-0 col-sm-3"></asp:Label>
            </div>
            <div class="col-11">
                <div class="input-group input-group-sm mx-3">
                    <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>

            <div class="row mt-3 justify-content-center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAceptar_New" runat="server" OnClick="BtnAceptar_New_Click" Text="Enviar" CssClass="btn btn-outline-primary me-2" />
                        <asp:Button ID="BtnCerrar_New" runat="server" OnClick="BtnCerrar_New_Click" Text="Cerrar" CssClass="btn btn-outline-secondary" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlDocProceso" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;">
            <div class="row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" >
                <ContentTemplate>
                    <div class="container">
                        <div>
                            <br />
                            <hr class="dropdown-divider" />
                        </div>
                        <div>
                            <br />
                            <div class="d-flex flex-row mx-4 m-0 p-0">
                                <asp:Label ID="LblDocumentos" runat="server" Text="Documentos" CssClass="form-label my-0 p-0" />
                            </div>
                            <div class="col-sm-6 mx-3">
                                <asp:Panel runat="server" DefaultButton="BtnBuscar">
                                    <asp:TextBox ID="txtPnlBusqProceso" runat="server" placeholder="Buscar/Proceso" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <asp:Button ID="BtnBuscar" runat="server" OnClick="BtnBuscar_Click" Style="display: none" />
                                </asp:Panel>
                            </div>
                            <div class="mb-2">
                                <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                    <asp:GridView ID="grdPnlBusqProceso" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" 
                                        AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grdPnlBusqProceso_PageIndexChanging"
                                        PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left">
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Desc_Proceso" HeaderText="Proceso" >
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción del documento" >
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
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
                        <br />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:PostBackTrigger ControlID="grdPnlBusqProceso" />--%>
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
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                                TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeNewDocumento" runat="server" PopupControlID="PnlNewDocumentos"
                                TargetControlID="LblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewDocumento()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="LblOculto3" runat="server" Text="Label" Style="display: none;" />

                            <ajaxToolkit:ModalPopupExtender ID="mpeNewProceso" runat="server" PopupControlID="PnlDocProceso"
                                TargetControlID="LblOculto1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProceso()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="LblOculto1" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
                </td>
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
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GrdCheck_List" />
            <asp:PostBackTrigger ControlID="BtnAceptar_New" />
            <asp:PostBackTrigger ControlID="btnAgregar" />
            <asp:PostBackTrigger ControlID="BtnAceptar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
