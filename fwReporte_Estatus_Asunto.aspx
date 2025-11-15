<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwReporte_Estatus_Asunto.aspx.cs" Inherits="WebItNow_Peacock.fwReporte_Estatus_Asunto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sortablejs@1.15.0/Sortable.min.js"></script>

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
            .stage-block {
                cursor: pointer;
            transition: all 0.2s ease-in-out;
            }

            .stage-block:hover {
                background - color: #f8f9fa;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
            transform: scale(1.02);
            }

            .etapa-card:hover {
                background-color: #f8f9fa;
                transform: translateY(-3px);
                transition: all 0.2s ease;
            }

            .etapa-card {
                min-height: 120px;
                display: flex;
                align-items: center;
                justify-content: center;
                flex-direction: column;
            }

        </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="updPnlMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row justify-content-center mb-2 py-4">
                    <div class="col-lg-8 col-md-8">
                        <div class="input-group input-group-sm justify-content-center">
                            <%--<h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Consulta por Etapas/Protocolos</h2>--%>
                            <asp:Label ID="lblTitulo_Cat_Etapas" runat="server" CssClass="h2 mb-3 fw-normal mt-4 align-content-center" style="display:block; text-align:center;" ></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container col-md-8 col-lg-10 mt-4">
                <!-- Panel principal -->
                <asp:Panel ID="MainPanel" runat="server" Visible="true" >
                     <div class="row mb-4">
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-3">
                                <asp:Label ID="LblAseguradora" runat="server" Text="<%$ Resources:GlobalResources, lblCiaSeguros %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlAseguradora" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlAseguradora_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-3">
                                <asp:Label ID="LblServicio" runat="server" Text="<%$ Resources:GlobalResources, lblProyecto %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlProyecto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-3">
                                <asp:Label ID="LblCategoria" runat="server" Text="<%$ Resources:GlobalResources, lblTipoAsunto %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" Width="100%" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    
                    </div>
                    
                    <!-- opcion con ListView -->
                    <div class="">
                        <asp:ListView ID="lvEtapas" runat="server" OnItemCommand="lvEtapas_ItemCommand">
                            <LayoutTemplate>
                                <div class="container">
                                    <div class="row" id="etapasContainer">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                    </div>
                                </div>
                            </LayoutTemplate>

                            <ItemTemplate>
                                <div class="col-6 col-md-4 col-lg-3 mb-4">
                                    <asp:LinkButton ID="btnEtapa" runat="server" 
                                        CssClass="card etapa-card text-center shadow-sm border-0 w-100" 
                                        CommandName="VerReferencias" CommandArgument='<%# Eval("IdEtapa_fk") %>' 
                                         >  
                                        <div class="card-body">
                                            <h6 class="card-title fw-bold text-primary mb-2">
                                                <%# Eval("NombreEtapa") %>
                                            </h6>
                                            <div class="ref-count bg-light rounded-pill px-3 py-1 d-inline-block">
                                                <%# Eval("NumeroReferencias") %> referencias
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>

                </asp:Panel>

                <!-- Panel Secundary Tabla Grid -->
                <asp:Panel ID="ConsultPanel" runat="server" Visible="true" >
                    
                    <div class="d-grid gap-4 d-flex justify-content-end mt-2 mb-3">
                        <asp:UpdatePanel ID="updtPnlBtnRegresar" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="BtnRegresarPnlMain" runat="server" Text="<%$ Resources:GlobalResources, btnRegresar %>" OnClick="BtnRegresarPnlMain_Click" CssClass="btn btn-secondary px-4" TabIndex="1"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:Label ID="lblIdEtapaSelect" runat="server" Text="" />
                    <%-- Altas de Asunto --%>
                    <div style="overflow-x: auto; overflow-y:hidden">
                        <asp:UpdatePanel ID="updtPnlGrdReferenciasEtapa" runat="server">
                        <ContentTemplate>
                        <asp:GridView ID="GrdReferenciasEtapa"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                                AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                
                                  PageSize="10" Font-Size="Smaller" >
                                <AlternatingRowStyle CssClass="alt autoWidth" />
                                <Columns>
                                    
                                    <asp:BoundField DataField="Referencia_Siniestro" HeaderText="Referencia Siniestro" >
                                    <%--<ItemStyle Width="300px" />--%>
                                    </asp:BoundField>

                                    <%--<asp:TemplateField HeaderText="Referencia Siniestro">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReferencia" runat="server" UseSubmitBehavior="true" style="display: block; text-align: left; text-decoration: none;" CommandName="SelectReferencia" >
                                            
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                
                                    <asp:BoundField DataField="NumSiniestro" HeaderText="<%$ Resources:GlobalResources, col_NumSiniestro %>" >
                                    <%--<ItemStyle Width="300px" />--%>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NumPoliza" HeaderText="<%$ Resources:GlobalResources, col_NumPoliza %>" >
                                    <%--<ItemStyle Width="300px" />--%>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NomProyecto" HeaderText="<%$ Resources:GlobalResources, col_NomProyecto %>" >
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
                                    
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                        </asp:GridView>
                        </ContentTemplate>
                        </asp:UpdatePanel>
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
                    <asp:Button ID="BtnAceptar" runat="server"  Text="Aceptar" CssClass="btn btn-outline-primary mx-1" />
                    <asp:Button ID="BtnCancelar" runat="server"  Text="Cancelar" CssClass="btn btn-outline-secondary mx-1" />
                    <asp:Button ID="BtnCerrar" runat="server"  Text="Cerrar" CssClass="btn btn-outline-primary"/>
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

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnRegresarPnlMain" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
