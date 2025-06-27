<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGenerar_Formatos.aspx.cs" Inherits="WebItNow_Peacock.fwGenerar_Formatos" %>
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

        <div class="container col-md-8 col-lg-10 mt-4">
            <div class="row mb-3 py-5">
                <div class="col-lg-3 col-md-3">
                    <div class ="mb-2">
                        <asp:Label ID="LblReporte" runat="server" Text="Reporte / Siniestro" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                    </div>
                    <asp:Panel runat="server" DefaultButton="ImgBusReporte">
                        <div class="row">
                            <div class="col-11">
                                <asp:TextBox ID="TxtReporte" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Reporte ó Siniestro" AutoComplete="off" MaxLength="12" ></asp:TextBox>
                            </div>
                            <div class="col-1 justify-content-start ps-0 ms-0">
                                <asp:ImageButton ID="ImgBusReporte" runat="server" CssClass="p-1" ImageUrl="~/Images/search_find.png" Height="32px" Width="32px" OnClick="ImgBusReporte_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="col-lg-4 col-md-4 ">
<%--
                    <div class ="mb-2">
                        <asp:Label ID="LblCiaSeguros" runat="server" Text="Compañia de Seguros"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlCiaSeguros" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlCiaSeguros_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                    </div>
--%>
            </div>
        </div>

        <div class="container col-12 mt-4">
            <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                <h6 class="h6 fw-normal my-1" style="font-size:small">Información del Reporte</h6>
            </div>

            <%-- Consulta de Asunto --%>
            <div style="overflow-x: auto; overflow-y:hidden">
                <asp:GridView ID="GrdConsulta_Datos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                        OnPageIndexChanging="GrdConsulta_Datos_PageIndexChanging" OnRowCommand="GrdConsulta_Datos_RowCommand" OnPreRender="GrdConsulta_Datos_PreRender"
                        OnSelectedIndexChanged="GrdConsulta_Datos_SelectedIndexChanged" OnRowDataBound="GrdConsulta_Datos_RowDataBound" 
                        DataKeyNames="Id_Expediente" PageSize="10" Font-Size="Smaller" >
                        <AlternatingRowStyle CssClass="alt autoWidth" />
                        <Columns>
                            <asp:BoundField DataField="Referencia" HeaderText="Número Referencia" >
                            <%--<ItemStyle Width="300px" />--%> 
                            </asp:BoundField>
                            <asp:BoundField DataField="Num_Reporte" HeaderText="Número Reporte" >
                            <%--<ItemStyle Width="300px" />--%> 
                            </asp:BoundField>
                            <asp:BoundField DataField="Num_Siniestro" HeaderText="Número Siniestro" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fec_Asignacion" DataFormatString="{0:d}" HeaderText="Fecha de Asignación" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Riesgo" HeaderText="Riesgo Materializado" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Det_Reporte" HeaderText="Detalles del Reporte" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Asegurado_1" HeaderText="Nombre del Asegurado" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField> 
                            <asp:BoundField DataField="IdStatus" >
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgPagina" runat="server" OnClick="ImgPagina_Click" Height="32px" Width="64px" ImageUrl="~/Images/editar.png"  Enabled="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>

        </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GrdConsulta_Datos" />
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
                <td>

                </td>
                <td class="style3">
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
