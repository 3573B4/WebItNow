<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwReporte_Inspecciones.aspx.cs" Inherits="WebItNow_Peacock.fwReporte_Inspecciones" %>
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
            <div class="row ">
                <div class="row mb-3 py-5">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblResponsable" runat="server" Text="Responsable Inspección" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group">
                            <asp:DropDownList ID="ddlResponsable" runat="server" style="font-size:16px;" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlResponsable_SelectedIndexChanged" visible="true" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <br />
                        </div>
                        <div class="input-group">
                            <asp:Button ID="BtnAgendaInspecciones" runat="server" Text="Calendario" OnClick="BtnAgendaInspecciones_Click" CssClass="btn btn-primary btn-sm px-4 " />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container col-12 mt-4">
            <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta General de Inspecciones</h6>
            </div>

            <%-- Altas de Inspecciones --%>
            <div style="overflow-x: auto; overflow-y:hidden">
                <asp:GridView ID="GrdAlta_Inspeccion"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                        OnPageIndexChanging="GrdAlta_Inspeccion_PageIndexChanging" OnRowCommand="GrdAlta_Inspeccion_RowCommand" OnPreRender="GrdAlta_Inspeccion_PreRender"
                        OnSelectedIndexChanged="GrdAlta_Inspeccion_SelectedIndexChanged" OnRowDataBound="GrdAlta_Inspeccion_RowDataBound" 
                        DataKeyNames="Id_Inspecciones" PageSize="10" Font-Size="Smaller" >
                        <AlternatingRowStyle CssClass="alt autoWidth" />
                        <Columns>
                            <asp:BoundField DataField="Dia_Semana" HeaderText="DIA" >
                            <%--<ItemStyle Width="300px" />--%> 
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha_Programada" DataFormatString="{0:d}" HeaderText="FECHA" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Hora_Minutos" HeaderText="HORA" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Ref_Siniestro" HeaderText="Referencia ó Siniestro" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="IdResponsable_Inspeccion" HeaderText="Responsable de Inspeccion" >
                            <%--<ItemStyle Width="3000px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="IdTpo_Inspeccion" HeaderText="Tipo de Inspección" >
                            <%--<ItemStyle Width="250px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Lugar_Inspeccion" HeaderText="Lugar de Inspeccion" >
                            <%--<ItemStyle Width="250px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones"  >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="IdStatus" HeaderText="Estatus" >
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>

            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAltaInspeccion" runat="server" Text="Alta Inspección" OnClick="BtnAltaInspeccion_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="10"/>
                        <asp:Button ID="BtnExportarExcel" runat="server" Text="Exportar Excel" OnClick="BtnExportarExcel_Click" CssClass="btn btn-primary" TabIndex="11"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        </ContentTemplate>
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
                            <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                                TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
                </td>
                <td>&nbsp;</td>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
