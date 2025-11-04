<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Catalog_Lineas.aspx.cs" Inherits="WebItNow_Peacock.fwGM_Catalog_Lineas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    
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
        

        document.onkeydown = function (evt) {
            return (evt ? evt.which : event.keyCode) != 13;
        }

        function cerrarModal() {
            //$("[id*='mpeVerIntegComis']").css("display", "none");
            $("#pnlMensaje").css("display", "none");

            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="container col-lg-7 col-md-8 col-sm-8">
            <div class="row mb-2 py-4">
                <div class="col-lg-4 col-md-4 ">
                </div>
                <div class="col-lg-8 col-md-8">
                    <div class="input-group input-group-sm">
                        <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Catálogo de Líneas - Estaciones</h2>
                    </div>
                </div>
            </div>
        </div>

        <div class="container col-md-8 col-lg-10 mt-4">
            <!-- Formulario principal -->
            <asp:Panel ID="MainPanel" runat="server" Visible="true">

                <div class="row mb-3">
                        <div class="col-lg-4 col-md-4">
                            <div class ="mb-2">
                                <asp:Label ID="LblLinea_Ocurrencia" runat="server" Text="Nombre Línea Ocurrencia" ></asp:Label>
                            </div>
                            <div class=" input-group input-group-sm">
                                <asp:DropDownList ID="ddlProtocolos" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProtocolos_SelectedIndexChanged" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4">
                            <div class ="mb-2">
                                &nbsp;&nbsp;
                            </div>
                            <div class=" input-group input-group-sm">
                                <asp:Button ID="btnMostrarPanel" runat="server" Text="Agregar Línea" OnClick="btnMostrarPanel_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                            </div>
                        </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblNomEstacion" runat="server" Text="Nombre Estación"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTarea" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre de la Estación" AutoComplete="off" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                </div>

            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAnular" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnular_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                        <asp:Button ID="btnEditar" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="BtnEditar_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                        <asp:Button ID="BtnGrabar" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                        <asp:Button ID="BtnAgregar" runat="server" Text="Agregar" OnClick="BtnAgregar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="container col-12 mt-4">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Estaciones</h6>
                </div>

                <%-- Catalogo de Tareas --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdTasks"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdTasks_PageIndexChanging" OnRowCommand="GrdTasks_RowCommand" OnPreRender="GrdTasks_PreRender"
                            OnSelectedIndexChanged="GrdTasks_SelectedIndexChanged" OnRowDataBound="GrdTasks_RowDataBound" 
                            DataKeyNames="IdEstacion" PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="NomEstacion" HeaderText="Descripción Estación Ocurrencia" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdEstacion" >
                                </asp:BoundField>
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
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
            </asp:Panel>

            <!-- Panel oculto para agregar protocolos -->
            <asp:Panel ID="PanelAgregarProtocolo" runat="server" Visible="false">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Agregar Línea de Ocurrencia</h6>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblNomLinea" runat="server" Text="Nombre Línea de Ocurrencia"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSLA_Protocolo" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre Línea de Ocurrencia" AutoComplete="off" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAddProtocolo"    runat="server" Text="Agregar&nbsp;"  OnClick="BtnAddProtocolo_Click" CssClass="btn btn-primary px-4" TabIndex="0"/>
                        <asp:Button ID="BtnCancelProtocolo" runat="server" Text="Cancelar" OnClick="BtnCancelProtocolo_Click" CssClass="btn btn-secondary px-4" TabIndex="1"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>

            <div class="container col-12 mt-4">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Lineas de Ocurrencia</h6>
                </div>

                <%-- Catalogo de LineaNegocios --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdProtocolos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdProtocolos_PageIndexChanging" OnRowCommand="GrdProtocolos_RowCommand" OnPreRender="GrdProtocolos_PreRender"
                            OnSelectedIndexChanged="GrdProtocolos_SelectedIndexChanged" OnRowDataBound="GrdProtocolos_RowDataBound" 
                            DataKeyNames="IdLinea" PageSize="10" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="IdLinea" >
                                </asp:BoundField>
                                <asp:BoundField DataField="NomLinea" HeaderText="Nombre Línea de Ocurrencia" >
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgEliminarSLA" runat="server" OnClick="ImgEliminarSLA_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
            </asp:Panel>

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
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                                TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnAgregar" />
            <asp:PostBackTrigger ControlID="btnEditar" />
            <asp:PostBackTrigger ControlID="BtnAnular" />
            <asp:PostBackTrigger ControlID="BtnGrabar" />
            <asp:PostBackTrigger ControlID="BtnAddProtocolo" />
            <asp:PostBackTrigger ControlID="BtnCancelProtocolo" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
