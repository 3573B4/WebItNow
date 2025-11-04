<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwReporte_Alta_Asunto.aspx.cs" Inherits="WebItNow_Peacock.fwReporte_Alta_Asunto" %>
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

        .radio-button-horizontal label {
            margin-left: 6px; /* separa el texto del botón */
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
                        
                        <asp:Label ID="LblRef" runat="server" Text="<%$ Resources:GlobalResources, LblRef %>" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                    </div>
<%--                    
                    <div class="col-12">
                        <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Referencia ó Siniestro" AutoComplete="off" MaxLength="12" ></asp:TextBox>
                    </div>
--%>
 
                    <asp:Panel runat="server" DefaultButton="ImgBusReference">
                        <div class="row">
                            
                            <div class="col-11">
                                <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="<%$ Resources:GlobalResources, LblRef %>" AutoComplete="off" MaxLength="15" ></asp:TextBox>
                            </div>

                           <div class="col-1 justify-content-start ps-0 ms-0">
                                <asp:ImageButton ID="ImgBusReference" runat="server" CssClass="p-1" ImageUrl="~/Images/search_find.png" Height="32px" Width="32px" OnClick="ImgBusReference_Click" />
                            </div>
                        </div>
                    </asp:Panel>

                </div>

                <div class="col-lg-3 col-md-3 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblColumnas" runat="server" Text="<%$ Resources:GlobalResources, LblColumnas %>"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlColumnas" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlColumnas_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-lg-3 col-md-3 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblFiltros" runat="server" Text="<%$ Resources:GlobalResources, LblFiltros %>" Visible="false" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlFiltros" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlFiltros_SelectedIndexChanged" AutoPostBack="true" Width="100%" Visible="false" TabIndex ="2">
                        </asp:DropDownList>
                    </div>  
                </div>

            </div>

            <div class="group-container-horizontal">
                <asp:RadioButton ID="rbAsuntos" runat="server" CssClass="radio-button-horizontal" Text="<%$ Resources:GlobalResources, LblAsuntos %>" OnCheckedChanged="rbAsuntos_CheckedChanged" AutoPostBack="true" GroupName="GrupoCasos" Checked="True" />
                <asp:RadioButton ID="rbProyectos" runat="server" CssClass="radio-button-horizontal" Text="<%$ Resources:GlobalResources, LblProyectos %>" OnCheckedChanged="rbProyectos_CheckedChanged" AutoPostBack="true" GroupName="GrupoCasos" />
            </div>
        </div>

        <div class="container col-12 mt-4">
            <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                <%--<h6 class="h6 fw-normal my-1" style="font-size:small">Consulta General de Referencias</h6>--%>
                <h6 class="h6 fw-normal my-1" style="font-size:small">
                    <asp:Label ID="LblConsultaRef" runat="server" Text="<%$ Resources:GlobalResources, lblConsultaRef %>"></asp:Label>
                </h6>
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
                                    <asp:ImageButton ID="ImgCheckList" runat="server" OnClick="ImgCheckList_Click" Height="24px" Width="24px" ImageUrl="~/Images/checklist.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:GlobalResources, col_Referencia %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkReferencia" runat="server" UseSubmitBehavior="true" style="display: block; text-align: left; text-decoration: none;" CommandName="SelectReferencia" 
                                    CommandArgument='<%# Eval("Referencia_Sub") + "|" + Eval("Referencia") + "|" + Eval("SubReferencia")  + "|" + 
                                                         Eval("IdProyecto") + "|" + Eval("IdSeguros")  + "|" + Eval("IdTpoAsunto") %>'><%# Eval("Referencia_Sub") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                        
                            <asp:BoundField DataField="Referencia_Sub" HeaderText="Referencia Siniestro" >
                            <ItemStyle Width="300px" />
                            </asp:BoundField>
--%>                        
                            <asp:BoundField DataField="NumSiniestro" HeaderText="<%$ Resources:GlobalResources, col_NumSiniestro %>" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NumPoliza" HeaderText="<%$ Resources:GlobalResources, col_NumPoliza %>" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomProyecto" HeaderText="<%$ Resources:GlobalResources, col_NomProyecto %>" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NumReporte" HeaderText="<%$ Resources:GlobalResources, col_NumReporte %>" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha_Asignacion" DataFormatString="{0:d}" HeaderText="<%$ Resources:GlobalResources, col_Fecha_Asignacion %>" >
                            <%--<ItemStyle Width="300px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Seguro_Cia" HeaderText="<%$ Resources:GlobalResources, col_Seguro_Cia %>" >
                            <%--<ItemStyle Width="3000px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Tpo_Asunto" HeaderText="<%$ Resources:GlobalResources, col_Tpo_Asunto %>" >
                            <%--<ItemStyle Width="250px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomActor" HeaderText="<%$ Resources:GlobalResources, col_NomActor %>"  >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomDemandado" HeaderText="<%$ Resources:GlobalResources, col_NomDemandado %>" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="NomAsegurado" HeaderText="<%$ Resources:GlobalResources, col_NomAsegurado %>" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Resp_Tecnico" HeaderText="<%$ Resources:GlobalResources, col_Resp_Tecnico %>" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Resp_Administrativo" HeaderText="<%$ Resources:GlobalResources, col_Resp_Administrativo %>" >
                            <%--<ItemStyle Width="600px" />--%>
                            </asp:BoundField>
                            <asp:BoundField DataField="Referencia_Anterior" HeaderText="<%$ Resources:GlobalResources, col_Referencia_Anterior %>" >
                            <%--<ItemStyle Width="300px" />--%> 
                            </asp:BoundField>
                            <asp:BoundField DataField="IdStatus" HeaderText="<%$ Resources:GlobalResources, col_IdStatus %>" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Referencia" >
                            </asp:BoundField>
                            <asp:BoundField DataField="SubReferencia" >
                            </asp:BoundField>
                            <asp:BoundField DataField="IdProyecto" >
                            </asp:BoundField>
                            <asp:BoundField DataField="IdSeguros" > 
                            </asp:BoundField>
                            <asp:BoundField DataField="IdTpoAsunto" >
                            </asp:BoundField>
                            <asp:BoundField DataField="IdTpoProyecto" > 
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgSubRef_Add" runat="server" OnClick="ImgSubRef_Add_Click" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png"  Enabled="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgSubRef_Del" runat="server" OnClick="ImgSubRef_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
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
                        <asp:Button ID="BtnCargaDatosIA" runat="server" Text="<%$ Resources:GlobalResources, btnCargaDatosIA %>" OnClick="BtnCargaDatosIA_Click" CssClass="btn btn-secondary" Visible="false" TabIndex="9"/>
                        <asp:Button ID="BtnAltaAsunto" runat="server" Text="<%$ Resources:GlobalResources, btnAltaAsunto %>" OnClick="BtnAltaAsunto_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="10"/>
                        <asp:Button ID="BtnExportarExcel" runat="server" Text="<%$ Resources:GlobalResources, btnExportarExcel %>" OnClick="BtnExportarExcel_Click" CssClass="btn btn-primary" TabIndex="11"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
                <asp:Label ID="LblMessage" runat="server" Text=""  />
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
        <asp:Panel ID="pnlMensaje_2" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 500px; background-color:#FFFFFF;">
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
                <asp:Label ID="LblMessageIA" runat="server" Text="" Mode="PassThrough" />
            </div>
            <div>
                <br />
                <hr class="dropdown-divider" />
            </div>
        
            <div>
                <br />
                <asp:Button ID="BtnCloseIA" runat="server" OnClick="BtnCloseIA_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlEnvioArchivos" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; height: 180px; width: 600px; background-color:#FFFFFF;">
            <div class=" row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>

            <div class="form-group">
                <div class="input-group mx-auto pt-3">
                    <input id="oFile" type="file" name="oFile" runat="server" accept=".xlsx" onchange="validateForSize(this,1,122880);" class="form-control" />
                </div>
            </div>

            <div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnEnviar" runat="server" OnClick="BtnEnviar_Click" Text="Aceptar" OnClientClick="showProgress()" CssClass="btn btn-outline-primary" />
                        <asp:Button ID="BtnCerrarIA" runat="server" OnClick="BtnCerrarIA_Click" Text="Cerrar" CssClass="btn btn-outline-secondary" />
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
                            <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                                TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_2" runat="server" PopupControlID="pnlMensaje_2"
                                TargetControlID="lblOculto_3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="lblOculto_3" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
                </td>
                <td>
                    <ajaxToolkit:ModalPopupExtender ID="mpeNewEnvio" runat="server" PopupControlID="PnlEnvioArchivos"
                        TargetControlID="LblOculto2" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewEnvio()" >
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Label ID="LblOculto2" runat="server" Text="Label" Style="display: none;" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBusReference" />
            <asp:PostBackTrigger ControlID="GrdAlta_Asunto" />
            <asp:PostBackTrigger ControlID="BtnCargaDatosIA" />
            <asp:PostBackTrigger ControlID="BtnAltaAsunto" />
            <asp:PostBackTrigger ControlID="BtnExportarExcel" />
            <asp:PostBackTrigger ControlID="BtnEnviar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
