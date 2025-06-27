<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwRepository_Document.aspx.cs" Inherits="WebItNow_Peacock.fwRepository_Document" %>
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

        function mpeNewProceso() {

        }

        function mpeNewDocumento() {
            //
        }

        function mpeExpira() {

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
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Asignación de Documentos a Carpeta</h2>
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
                            <asp:Label ID="LblCliente" runat="server" Text="Cliente" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblProyecto" runat="server" Text="Proyecto" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlProyecto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
<%--                    
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblAccion" runat="server" Text="Acción" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlAccion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlAccion_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
--%>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTpoAsunto" runat="server" Text="Tipo de Asunto" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
<%--
                            <asp:DropDownList ID="ddlTpoAsunto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoAsunto_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
--%>
                            <asp:TextBox ID="TxtTpoAsunto" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Tipo de Asunto" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTpoEstatus" runat="server" Text="Tipo de Estatus" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlTpoEstatus" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoStatus_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                </div>

                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-0" style="font-size:small">&nbsp;</h5>
                </div>

                <div class="row mb-3">
<%--                    
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblReferencia" runat="server" Text="Referencia" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Referencia" AutoComplete="off" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblSiniestro" runat="server" Text="Siniestro" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtSiniestro" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Siniestro" AutoComplete="off" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
--%>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblPoliza" runat="server" Text="Póliza" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtPoliza" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Póliza" AutoComplete="off" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblAsegurado" runat="server" Text="Asegurado" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtAsegurado" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Asegurado" AutoComplete="off" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                </div>

                <div class="row mb-3 mt-4">
                    <div class="form-group">
                        <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server"  UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnDocument_Proceso" runat="server" Text="Documentos del Estatus" Font-Bold="True" OnClick="BtnDocument_Proceso_Click" CssClass="btn btn-secondary" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
<%--
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                </div>
--%>

                <div class="container col-12 mt-4">
                    <div class="row mb-3" style="background-color:#96E7D9; align-items: baseline;">
                        <div class="col-10" style="padding-left: 14px;">
                            <asp:Label ID="LblEtiquetaCheck" runat="server" Text="" CssClass="control-label" Font-Size="small"></asp:Label>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAddDocument" runat="server" Text="+" OnClick="BtnAddDocument_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <%-- Documentos --%>
                    <div style="overflow-x: auto; overflow-y:hidden;">
                        <asp:GridView ID="GrdDocumentos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                OnPageIndexChanging="GrdDocumentos_PageIndexChanging" OnRowCommand="GrdDocumentos_RowCommand" OnPreRender="GrdDocumentos_PreRender"
                                OnSelectedIndexChanged="GrdDocumentos_SelectedIndexChanged" OnRowDataBound="GrdDocumentos_RowDataBound" 
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
                </div>
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
                    <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
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
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GrdDocumentos_PageIndexChanging"
                                            PageSize="10" Font-Size="Smaller">
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Desc_Proceso" HeaderText="Proceso">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción del documento">
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
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="GrdDocumentos" />
        <asp:PostBackTrigger ControlID="BtnAceptar_New" />
        <asp:PostBackTrigger ControlID="btnAgregar" />
        <%--<asp:PostBackTrigger ControlID="BtnAceptar" />--%>
    </Triggers>
</asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
