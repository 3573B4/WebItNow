<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwCatalog_Contacts.aspx.cs" Inherits="WebItNow_Peacock.fwCatalog_Contacts" %>
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
                        <%--<h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Catálogo de Contactos</h2>--%>
                        <asp:Label ID="lblTitulo_Cat_Contactos" runat="server" CssClass="h2 mb-3 fw-normal mt-4 align-content-center" style="display:block; text-align:center;" ></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="container col-md-8 col-lg-10 mt-4">

            <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCliente" runat="server" Text="<%$ Resources:GlobalResources, lblCiaSeguros %>" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
            </div>

            <div class="row mb-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblNomContacto" runat="server" Text="<%$ Resources:GlobalResources, lblNomContacto %>"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNomContacto" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblNomContacto %>" AutoComplete="off" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblCorreoContacto" runat="server" Text="<%$ Resources:GlobalResources, lblCorreoContacto %>" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtCorreoContacto" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblCorreoContacto %>" AutoComplete="off" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblTelefono" runat="server" Text="<%$ Resources:GlobalResources, lblTelefono %>" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblTelefono %>" AutoComplete="off" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblNumExt" runat="server" Text="<%$ Resources:GlobalResources, lblNumExtencion %>" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtExtencionTel" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblNumExtencion %>" AutoComplete="off" MaxLength="5"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblTelMovil" runat="server" Text="Teléfono Celular" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtTelMovil" runat="server" CssClass="form-control form-control-sm" placeholder="Teléfono Celular" AutoComplete="off" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
            </div>
            <br />                
            <div class="row mb-3 mt-4" style="background-color:#C6D541; align-items: baseline;">
                <div class="col-10" style="padding-left: 14px;">
                    <asp:Label ID="LblLineaNegocios" runat="server" Text="<%$ Resources:GlobalResources, lblLineaNegocios %>" CssClass="control-label" Font-Size="small"></asp:Label>
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

            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAnular" runat="server" Text="<%$ Resources:GlobalResources, btnAnular %>" Font-Bold="True" OnClick="BtnAnular_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                        <asp:Button ID="BtnEditar" runat="server" Text="<%$ Resources:GlobalResources, btnEditar %>" Font-Bold="True" OnClick="BtnEditar_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                        <asp:Button ID="BtnGrabar" runat="server" Text="<%$ Resources:GlobalResources, btnGrabar %>" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                        <asp:Button ID="BtnAgregar" runat="server" Text="<%$ Resources:GlobalResources, btnAgregar %>" Font-Bold="True" OnClick="BtnAgregar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="container col-12 mt-4">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <%--<h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Contactos</h6>--%>
                    <h6 class="h6 fw-normal my-1" style="font-size:small">
                        <asp:Literal runat="server" Text="<%$ Resources:GlobalResources, hdrConsultaContactos %>" />
                    </h6>
                </div>

                <%-- Catalogo de clientes --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdContactos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdContactos_PageIndexChanging" OnRowCommand="GrdContactos_RowCommand" OnPreRender="GrdContactos_PreRender"
                            OnSelectedIndexChanged="GrdContactos_SelectedIndexChanged" OnRowDataBound="GrdContactos_RowDataBound" 
                            DataKeyNames="IdSeguros" PageSize="10" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="IdContacto" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdSeguros" HeaderText="<%$ Resources:GlobalResources, col_IdSeguros %>" >
                                </asp:BoundField>
                                <asp:BoundField DataField="NomContacto" HeaderText="<%$ Resources:GlobalResources, col_NomContacto %>" >
                                </asp:BoundField>
                                <asp:BoundField DataField="EmailContacto" HeaderText="<%$ Resources:GlobalResources, col_CorreoContacto %>" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Telefono" HeaderText="<%$ Resources:GlobalResources, col_NomContacto %>" >
                                </asp:BoundField>
                                <asp:BoundField DataField="TelExtencion" HeaderText="<%$ Resources:GlobalResources, col_TelExtencion %>" >
                                </asp:BoundField>
                                <asp:BoundField DataField="TelCelular" HeaderText="<%$ Resources:GlobalResources, col_TelMovil %>" >
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Líneas de Negocio">
                                    <ItemTemplate>
                                        <asp:Repeater ID="rptLineasNegocio" runat="server">
                                            <ItemTemplate>
                                                <div><%# Eval("NomLineaNegocio") %></div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="<%$ Resources:GlobalResources, btnCerrar %>" CssClass="btn btn-outline-primary"/>
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
                <asp:Button ID="BtnAceptar" runat="server" OnClick="BtnAceptar_Click" Text="<%$ Resources:GlobalResources, btnAceptar %>" CssClass="btn btn-outline-primary mx-1" />
                <asp:Button ID="BtnCancelar" runat="server" OnClick="BtnCancelar_Click" Text="<%$ Resources:GlobalResources, btnCancelar %>" CssClass="btn btn-outline-secondary mx-1" />
                <asp:Button ID="BtnCerrar" runat="server" OnClick="BtnCerrar_Click" Text="<%$ Resources:GlobalResources, btnCerrar %>" CssClass="btn btn-outline-primary"/>
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
            <asp:PostBackTrigger ControlID="BtnEditar" />
            <asp:PostBackTrigger ControlID="BtnAnular" />
            <asp:PostBackTrigger ControlID="BtnGrabar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
