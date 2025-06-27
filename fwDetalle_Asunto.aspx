<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwDetalle_Asunto.aspx.cs" Inherits="WebItNow_Peacock.fwDetalle_Asunto" %>
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
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Detalles del Caso</h2>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Información General</h6>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblSubReferencia" runat="server" Text="No. Referencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSubReferencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblTpo_Asunto" runat="server" Text="Tipo de Asunto" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTpo_Asunto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblProyecto" runat="server" Text="Nombre de Proyecto" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtProyecto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblFecha_Asignacion" runat="server" Text="Fecha Asignación" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtFecha_Asignacion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblNomActor" runat="server" Text="Nombre Actor" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomActor" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblNomDemandado" runat="server" Text="Nombre Demandado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomDemandado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblNomAsegurado" runat="server" Text="Nombre Asegurado" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtNomAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblSeguro_Cia" runat="server" Text="Compañia de Seguros" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSeguro_Cia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblResp_Tecnico" runat="server" Text="Responsable Tecnico" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtResp_Tecnico" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            <asp:Label ID="LblResp_Administrativo" runat="server" Text="Responsable Administrativo" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtResp_Administrativo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="mb-2">
                            <asp:Label ID="LblEstatus" runat="server" Text="Estatus" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEstatus" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
<%--                
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Configuración del Siniestro</h6>
                </div>
--%>

<%--                <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl1" runat="server" Text="Configuración del Siniestro " CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnShowPanel1" runat="server" Text="&#9660;" OnClick="btnShowPanel1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <br />
                <div>
                <asp:Panel ID="pnl1" runat="server" Visible="false">
                    <div class="row mb-3">
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblTpoAsegurado" runat="server" Text="Tipo de Asegurado" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlTpoAsegurado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoAsegurado_SelectedIndexChanged" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblTpoEstatus" runat="server" Text="Estatus" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlConclusion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlConclusion_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <br />                
                    <div class="row mb-2">
                        <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                            <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <asp:GridView ID="GrdCategorias" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                    AllowPaging="False" CssClass="table table-responsive table-light table-striped table-hover align-middle"
                                    AlternatingRowStyle-CssClass="alt" OnRowCommand="GrdCategorias_RowCommand" 
                                    OnRowDataBound="GrdCategorias_RowDataBound"
                                    Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                    <AlternatingRowStyle CssClass="alt autoWidth" />
                                    <Columns>
                                        <asp:BoundField DataField="IdDocumento" />
                                        <asp:BoundField DataField="IdSeccion" />
                                        <asp:BoundField DataField="IdCategoria" />
                                        <asp:BoundField DataField="DescCategoria" HeaderText="Descripción de Categoría" >
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DescSeccion" HeaderText="Categoría Por" >
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Seleccionado">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                <asp:Label ID="IdSeccionLbl" runat="server" Text='<%# Eval("IdSeccion") %>' Visible="false" />
                                                <asp:Label ID="IdDocumentoLbl" runat="server" Text='<%# Eval("IdDocumento") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </asp:Panel>
                </div>--%>
                <br />
                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--<asp:Button ID="BtnGrabar" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" TabIndex="1"/>--%>
                            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-secondary" TabIndex="2"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </ContentTemplate>
    <Triggers>
        <%--<asp:PostBackTrigger ControlID="BtnGrabar" />--%>
        <%--<asp:PostBackTrigger ControlID="btnShowPanel1" />--%>
    </Triggers>
    </asp:UpdatePanel>
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
                        <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                            TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
                    </div>
                </div>
            </td>
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

            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
