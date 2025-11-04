<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Configuracion_Document.aspx.cs" Inherits="WebItNow_Peacock.fwGM_Configuracion_Document" %>
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
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Configuración de Documentos por Categoría</h2>
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

                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTpoAsunto" runat="server" Text="Tipo de Asunto" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoAsunto" runat="server" CssClass="form-control form-control-sm" placeholder="Tipo de Asunto" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-1" style="font-size:small">&nbsp;</h5>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblSecciones" runat="server" Text="Documentos Requeridos Por" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlSecciones" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlSecciones_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCategorias" runat="server" Text="Categorias" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlCategorias" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCategorias_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                </div>

                <!-- agregar más paneles según sea necesario -->
            <div class="container col-12 mt-4">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblDocumentos" runat="server" Text="Documentos de la Categoría" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnDocumentos" runat="server" Text="+" OnClick="BtnDocumentos_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <%-- Documentos por categoria --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdDocumentos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdDocumentos_PageIndexChanging" OnRowCommand="GrdDocumentos_RowCommand" OnPreRender="GrdDocumentos_PreRender"
                            OnSelectedIndexChanged="GrdDocumentos_SelectedIndexChanged" OnRowDataBound="GrdDocumentos_RowDataBound" 
                            DataKeyNames="IdDoc_Categoria" PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción del documento" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdDoc_Categoria" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdProyecto" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdSeccion" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdCategoria" >
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
                                    <asp:Label ID="LblCarpetas" runat="server" Text="Carpeta en la cual desea trabajar" CssClass="form-label my-0 p-0" />
                                </div>
                                <div class="col-sm-6 mx-3">
                                    <asp:DropDownList ID="ddlCarpetas" runat="server" CssClass="form-select form-control-sm" OnSelectedIndexChanged="ddlCarpetas_SelectedIndexChanged" AutoPostBack ="true" >
                                    </asp:DropDownList>
                                </div>
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">  
                                        <ContentTemplate>
                                        <asp:GridView ID="GrdDocCarpeta" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" 
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GrdDocCarpeta_PageIndexChanging" OnRowDataBound="GrdDocCarpeta_RowDataBound"
                                            PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IdConsecutivo" HeaderText="" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción del documento" >
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
                                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" CssClass="btn btn-outline-primary btn-sm" />
                                <asp:Button ID="btnClose_Proceso" runat="server" Text="Cerrar" OnClick="btnClose_Proceso_Click" CssClass="btn btn-outline-secondary btn-sm" />
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
    <Triggers>
        <asp:PostBackTrigger ControlID="BtnDocumentos" />
        <asp:PostBackTrigger ControlID="btnAgregar" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
