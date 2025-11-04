<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Catalog_Proveedores.aspx.cs" Inherits="WebItNow_Peacock.fwGM_Catalog_Proveedores" %>
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
                        <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Catálogo de Proveedores</h2>
                    </div>
                </div>
            </div>
        </div>

        <div class="container col-md-8 col-lg-10 mt-4">
            <div class="row mb-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblTpoServicio" runat="server" Text="Tipo de Servicio" CssClass="control-label" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlTpoServicio" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoServicio_SelectedIndexChanged" Width="100%">
                        </asp:DropDownList>
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
                        <asp:Label ID="LblNomRepresentante" runat="server" Text="Nombre del representante" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNomRepresentante" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-lg-4 col-md-4">
                    <div class="mb-2">
                        <asp:Label ID="LblNacionalidad_Empresa" runat="server" Text="Nacionalidad de la empresa" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNacionalidad_Empresa" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4">
                    <div class="mb-2">
                        <asp:Label ID="LblRFC" runat="server" Text="RFC" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtRFC" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblMunicipioProveedor" runat="server" Text="Delegación / Municipio" CssClass="control-label" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlMunicipios" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipios_SelectedIndexChanged" Width="100%" >
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
                <div class="col-lg-6 col-md-6">
                    <div class="mb-2">
                        <asp:Label ID="LblEmail" runat="server" Text="Correo electrónico" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
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
                <div class="col-lg-6 col-md-6">
                    <div class="mb-2">
                        <asp:Label ID="LblResponsable" runat="server" Text="Responsable comunicaciones" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtResponsable" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3">
                    <div class="mb-2">
                        <asp:Label ID="LblContraseñaQR" runat="server" Text="Contraseña QR" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtContraseñaQR" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Proveedores</h6>
                </div>

                <%-- Catalogo de proveedores --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdProveedores" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdProveedores_PageIndexChanging" OnRowCommand="GrdProveedores_RowCommand" OnPreRender="GrdProveedores_PreRender"
                            OnSelectedIndexChanged="GrdProveedores_SelectedIndexChanged" OnRowDataBound="GrdProveedores_RowDataBound" 
                            DataKeyNames="Id_Proveedor" PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="Id_Proveedor" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Tpo_Servicio" HeaderText="Tipo de Servicio" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Nom_Empresa" HeaderText="Nombre de la empresa" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Nom_Representante" HeaderText="Nombre del representante" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Nacionalidad_Empresa" HeaderText="Nacionalidad de la empresa" >
                                </asp:BoundField>
                                <asp:BoundField DataField="RFC_Empresa" HeaderText="RFC" >
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
                                <asp:BoundField DataField="Email_Empresa" HeaderText="Correo Electronico" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Tel_Contacto_1" HeaderText="Tel. Contacto 1" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Tel_Contacto_2" HeaderText="Tel. Contacto 2" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Nom_Responsable" HeaderText="Nombre del reponsable" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Contraseña_QR" HeaderText="Contraseña_QR" >
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
            <asp:PostBackTrigger ControlID="GrdProveedores" />
            <asp:PostBackTrigger ControlID="BtnAgregar" />
            <asp:PostBackTrigger ControlID="btnEditar" />
            <asp:PostBackTrigger ControlID="BtnAnular" />
            <asp:PostBackTrigger ControlID="BtnGrabar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
