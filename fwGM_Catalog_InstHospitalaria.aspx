<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Catalog_InstHospitalaria.aspx.cs" Inherits="WebItNow_Peacock.fwGM_Catalog_InstHospitalaria" %>
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
                        <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Catálogo de Instituciones Hospitalarias</h2>
                    </div>
                </div>
            </div>
        </div>

        <div class="container col-md-8 col-lg-10 mt-4">

            <div class="row mb-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblNomInstitucion" runat="server" Text="Nombre Institución Hospitalaria" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNomInstitucion" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre Institución Hospitalaria" AutoComplete="off" onkeyup="mayus(this);" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAnular" runat="server" Text="Cancelar" Font-Bold="True" OnClick="BtnAnular_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                        <asp:Button ID="BtnEditar" runat="server" Text="Editar Datos" Font-Bold="True" OnClick="BtnEditar_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                        <asp:Button ID="BtnGrabar" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                        <asp:Button ID="BtnAgregar" runat="server" Text="Agregar" OnClick="BtnAgregar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="container col-12 mt-4">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Instituciones Hospitalarias</h6>
                </div>

                <%-- Catalogo de clientes --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdInstitucion"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdInstitucion_PageIndexChanging" OnRowCommand="GrdInstitucion_RowCommand" OnPreRender="GrdInstitucion_PreRender"
                            OnSelectedIndexChanged="GrdInstitucion_SelectedIndexChanged" OnRowDataBound="GrdInstitucion_RowDataBound" 
                            DataKeyNames="Id_Institucion" PageSize="10" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="Id_Institucion" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción Institución Hospitalaria" >
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
