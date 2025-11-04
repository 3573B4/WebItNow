<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwCatalog_SLA_Protocolos_bk.aspx.cs" Inherits="WebItNow_Peacock.fwCatalog_SLA_Protocolos_bk" %>
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
                        <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Catálogo de Protocolos (SLA)</h2>
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
                                <asp:Label ID="LblProtocolos" runat="server" Text="Nombre del Protocolo (SLA)" ></asp:Label>
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
                                <asp:Button ID="btnMostrarPanel" runat="server" Text="Agregar Protocolo" OnClick="btnMostrarPanel_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                            </div>
                        </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblTarea" runat="server" Text="Nombre de la Tarea"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTarea" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre de la Tarea" AutoComplete="off" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2 ">
                        <div class="mb-2">
                            <asp:Label ID="LblPlazo" runat="server" Text="Plazo" ></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPlazo" runat="server" CssClass="form-control form-control-sm" placeholder="Plazo" AutoComplete="off" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2 ">
                        <div class="mb-2">
                            <asp:Label ID="LblTiempo" runat="server" Text="Medida de tiempo" ></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlUnidadTiempo" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlUnidadTiempo_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-2">
                            &nbsp;
                        </div>
                        <div class="input-group input-group-sm checkbox-container">
                            <asp:CheckBox ID="chkTaskInterno" runat="server" /> <span> &nbsp;&nbsp; Tarea Interna</span>
                        </div>
                    </div>
                </div>


<%--                
            <div class="row mb-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblProceso" runat="server" Text="Nombre de Tarea"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNomProceso" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre del Proceso" AutoComplete="off" MaxLength="60"></asp:TextBox>
                    </div>
                </div>

                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblTiempoRespuesta" runat="server" Text="Tiempo de Respuesta" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtTiempoRespuesta" runat="server" CssClass="form-control form-control-sm" placeholder="Tiempo de Respuesta" AutoComplete="off" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
            </div>
--%>

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
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Tareas</h6>
                </div>

                <%-- Catalogo de Tareas --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdTasks"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdTasks_PageIndexChanging" OnRowCommand="GrdTasks_RowCommand" OnPreRender="GrdTasks_PreRender"
                            OnSelectedIndexChanged="GrdTasks_SelectedIndexChanged" OnRowDataBound="GrdTasks_RowDataBound" 
                            DataKeyNames="IdTarea" PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="NomTarea" HeaderText="Descripción Tarea a realizar" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdTarea" >
                                </asp:BoundField>
                                <asp:BoundField DataField="DocInterno">
                                </asp:BoundField>
                                <asp:BoundField DataField="Plazo" HeaderText="Plazo">
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Medida de tiempo">
                                </asp:BoundField>
                                <asp:BoundField DataField="TpoInterno" HeaderText="Tpo. Tarea">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UnidadTiempo" HeaderText="">
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
                <%-- Catalogo de Procesos --%>
<%--                
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdProcesos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdProcesos_PageIndexChanging" OnRowCommand="GrdProcesos_RowCommand" OnPreRender="GrdProcesos_PreRender"
                            OnSelectedIndexChanged="GrdProcesos_SelectedIndexChanged" OnRowDataBound="GrdProcesos_RowDataBound" 
                            DataKeyNames="IdSeguros" PageSize="10" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="IdProceso" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdSeguros" HeaderText="Clave" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdSLA" >
                                </asp:BoundField>
                                <asp:BoundField DataField="TiempoRespuesta" HeaderText="Tiempo de Respuesta" >
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
--%>

            </asp:Panel>

            <!-- Panel oculto para agregar protocolos -->
            <asp:Panel ID="PanelAgregarProtocolo" runat="server" Visible="false">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Agregar Nuevo Protocolo (SLA)</h6>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblSLA_Protocolo" runat="server" Text="Nombre del Protocolo (SLA)"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtSLA_Protocolo" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre del Protocolo (SLA)" AutoComplete="off" MaxLength="60"></asp:TextBox>
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
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Protocolos (SLA)</h6>
                </div>

                <%-- Catalogo de LineaNegocios --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdProtocolos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdProtocolos_PageIndexChanging" OnRowCommand="GrdProtocolos_RowCommand" OnPreRender="GrdProtocolos_PreRender"
                            OnSelectedIndexChanged="GrdProtocolos_SelectedIndexChanged" OnRowDataBound="GrdProtocolos_RowDataBound" 
                            DataKeyNames="IdSLA" PageSize="10" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="IdSLA" >
                                </asp:BoundField>
                                <asp:BoundField DataField="NomProtocolo" HeaderText="Nombre del Protocolo (SLA)" >
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
            <asp:PostBackTrigger ControlID="BtnAddProtocolo" />
            <asp:PostBackTrigger ControlID="BtnCancelProtocolo" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
