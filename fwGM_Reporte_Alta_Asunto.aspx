<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Reporte_Alta_Asunto.aspx.cs" Inherits="WebItNow_Peacock.GastosMedicos.fwGM_Reporte_Alta_Asunto" %>
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

        function mpeDetalleReferencia() {
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

    <style type="text/css" >
        .group-container-horizontal {
            display: flex;
            align-items: center;
        }

        .radio-button-horizontal {
            margin-right: 40px; /* Espacio entre los RadioButton */
        }

        /* Elimina el margen derecho del último RadioButton */
        .radio-button-horizontal:last-child {
            margin-right: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

        <div class="container col-md-8 col-lg-10 mt-4">
            <div class="row mb-0 py-5">
                <div class="col-lg-3 col-md-3">
                    <div class ="mb-2">
                        <asp:Label ID="LblRef" runat="server" Text="Buscar General de Referencias" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                    </div>
<%--                    
                    <div class="col-12">
                        <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Referencia ó Siniestro" AutoComplete="off" MaxLength="12" ></asp:TextBox>
                    </div>
--%>
 
                    <asp:Panel runat="server" DefaultButton="ImgBusReference">
                        <div class="row">
                            
                            <div class="col-11">
                                <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Buscar General de Referencias" AutoComplete="off" MaxLength="15" ></asp:TextBox>
                            </div>

                           <div class="col-1 justify-content-start ps-0 ms-0">
                                <asp:ImageButton ID="ImgBusReference" runat="server" CssClass="p-1" ImageUrl="~/Images/search_find.png" Height="32px" Width="32px" OnClick="ImgBusReference_Click" />
                            </div>
                        </div>
                    </asp:Panel>

                </div>

                <div class="col-lg-3 col-md-3 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblColumnas" runat="server" Text="Columna a buscar"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlColumnas" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlColumnas_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-lg-3 col-md-3 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblFiltros" runat="server" Text="Filtros" Visible="false" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlFiltros" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlFiltros_SelectedIndexChanged" AutoPostBack="true" Width="100%" Visible="false" TabIndex ="2">
                        </asp:DropDownList>
                    </div>
                </div>
            
            </div>

            <div class="group-container-horizontal">
                <asp:RadioButton ID="rbAsuntos" runat="server" CssClass="radio-button-horizontal" Text="&nbsp; Asuntos" OnCheckedChanged="rbAsuntos_CheckedChanged" AutoPostBack="true" GroupName="GrupoCasos" Checked="True" />
                <asp:RadioButton ID="rbProyectos" runat="server" CssClass="radio-button-horizontal" Text="&nbsp; Proyectos" OnCheckedChanged="rbProyectos_CheckedChanged" AutoPostBack="true" GroupName="GrupoCasos" />
            </div>

        </div>

        <div class="container col-12 mt-4">
            <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta General de Referencias</h6>
            </div>

            <%-- Altas de Asunto --%>
            <div style="overflow-x: auto; overflow-y:hidden">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <asp:GridView ID="GrdAlta_Asunto"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                        OnPageIndexChanging="GrdAlta_Asunto_PageIndexChanging" OnRowCommand="GrdAlta_Asunto_RowCommand" OnPreRender="GrdAlta_Asunto_PreRender"
                        OnSelectedIndexChanged="GrdAlta_Asunto_SelectedIndexChanged" OnRowDataBound="GrdAlta_Asunto_RowDataBound" 
                        DataKeyNames="IdAsunto" PageSize="10" Font-Size="Smaller" >
                        <AlternatingRowStyle CssClass="alt autoWidth" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgCheckList" runat="server" OnClick="ImgCheckList_Click" Height="24px" Width="24px" ImageUrl="~/Images/checklist.png" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referencia Siniestro">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReferencia" runat="server" UseSubmitBehavior="true" style="display: block; text-align: left; text-decoration: none;" CommandName="SelectReferencia" 
                                    CommandArgument='<%# Eval("Referencia_Sub") + "|" + Eval("Referencia") + "|" + Eval("SubReferencia")  + "|" + 
                                                         Eval("IdProyecto") + "|" + Eval("IdSeguros")  + "|" + Eval("IdTpoEvento") %>'><%# Eval("Referencia_Sub") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NumSiniestro" HeaderText="Número Siniestro" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NumPoliza" HeaderText="Número Póliza" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomProyecto" HeaderText="Nombre Proyecto" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NumReporte" HeaderText="Número Reporte" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha_Asignacion" DataFormatString="{0:d}" HeaderText="Fecha de Asignación" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Hora_Asignacion" DataFormatString="{0:HH:mm}" HeaderText="Hora de Asignación" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Seguro_Cia" HeaderText="Compañia de Seguros" >
                            <%--<ItemStyle Width="3000px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Tpo_Evento" HeaderText="Tipo de Evento" >
                            <%--<ItemStyle Width="250px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomActor" HeaderText="Nombre del Actor"  >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomDemandado" HeaderText="Nombre del Demandado" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomAjustador" HeaderText="Nombre del Ajustador" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Resp_Tecnico" HeaderText="Responsable Medico" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Estatus_Caso" HeaderText="Estatus del Caso" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="IdStatus" HeaderText="Estatus" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Referencia" >
                            </asp:BoundField>
                            <asp:BoundField DataField="SubReferencia" >
                            </asp:BoundField>
                            <asp:BoundField DataField="IdProyecto" >
                            </asp:BoundField>
                            <asp:BoundField DataField="IdSeguros" > 
                            </asp:BoundField>
                            <asp:BoundField DataField="IdTpoEvento" >
                            </asp:BoundField>
                            <asp:BoundField DataField="IdTpoProyecto" > 
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgSubRef_Add" runat="server" OnClick="ImgSubRef_Add_Click" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png"  Enabled="true" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgSubRef_Del" runat="server" OnClick="ImgSubRef_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAltaAsunto" runat="server" Text="Alta Asunto" OnClick="BtnAltaAsunto_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="10"/>
                        <asp:Button ID="BtnExportarExcel" runat="server" Text="Exportar Excel" OnClick="BtnExportarExcel_Click" CssClass="btn btn-primary" TabIndex="11"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBusReference" />
            <asp:PostBackTrigger ControlID="GrdAlta_Asunto" />
            <asp:PostBackTrigger ControlID="BtnAltaAsunto" />
            <asp:PostBackTrigger ControlID="BtnExportarExcel" />
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
    <asp:Panel ID="pnlMensaje_1" runat="server" CssClass="CajaDialogo shadow-sm" 
                style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
        <!-- Botón de cierre en la esquina superior derecha -->
        <div class="row justify-content-end">
            <div class="col-auto">
                <asp:Button runat="server" CssClass="btn-close" aria-label="Cerrar" />
            </div>
        </div>
    
        <!-- Línea divisora -->
        <div class="border-bottom mx-3 mb-3"></div>
    
        <!-- Mensaje -->
        <div class="text-center mx-3 mb-3">
            <asp:Label ID="LblMessage_1" runat="server" CssClass="fw-bold" Text=""></asp:Label>
        </div>
    
        <!-- Dropdown List condicional -->
        <div class="mx-3 mb-3" style="min-height: 40px;">
            <asp:DropDownList ID="ddlProcedimiento" runat="server" CssClass="form-select form-select-sm w-100" OnSelectedIndexChanged="ddlTpoJuicio_SelectedIndexChanged" 
                style="display: none;" >
            </asp:DropDownList>
        </div>
    
        <!-- Botones de acción -->
        <div class="d-flex justify-content-center">
            <asp:Button ID="BtnAceptar" runat="server" OnClick="BtnAceptar_Click" Text="Aceptar" CssClass="btn btn-outline-primary mx-2" />
            <asp:Button ID="BtnCancelar" runat="server" OnClick="BtnCancelar_Click" Text="Cancelar" CssClass="btn btn-outline-secondary mx-2" />
            <asp:Button ID="BtnCerrar" runat="server" OnClick="BtnCerrar_Click" Text="Cerrar" CssClass="btn btn-outline-primary" />
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
            <td>
                <div class="form-group">
                    <div class="d-grid col-6 mx-auto">
                        <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                            TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
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
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
