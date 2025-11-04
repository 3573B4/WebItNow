<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwCatalog_SLA_Protocolos.aspx.cs" Inherits="WebItNow_Peacock.fwCatalog_SLA_Protocolos" %>
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
        function mpeNewsProtocolos() {
            //
        }
        function mpeNewTask() {
            //
        }
        function mpeOrderTask() {
            //
        }
        function mpeOrderSLA() {
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

        //empieza las funciones para ordenar con droag & drop en tareas
        var sortableInstance = null;

        function initSortable() {
            // destruir si ya existe
            if (sortableInstance) {
                try { sortableInstance.destroy(); } catch (e) { }
                sortableInstance = null;
            }

            var el = document.getElementById('olItems');
            if (!el) return;

            sortableInstance = Sortable.create(el, {
                animation: 150,
                ghostClass: 'bg-light',
                onEnd: function () {
                    saveOrderToHidden();
                },
                handle: ".list-group-item" // obliga a que el arrastre sea en todo el <li>
            });

            // guardar el orden inicial
            saveOrderToHidden();
        }

        function saveOrderToHidden() {
            var arr = [];
            var items = document.querySelectorAll('#olItems > li');
            items.forEach(function (li, idx) {
                var id = li.getAttribute('data-id');
                arr.push({ id: parseInt(id, 10), pos: idx + 1 });
            });
            // coloca el JSON en el HiddenField
            var hdn = document.getElementById('<%= hdnOrdenTask.ClientID %>');
            if (hdn) hdn.value = JSON.stringify(arr);
            console.log("Orden actual:", JSON.stringify(arr));
        }

        //empieza las funciones para ordenar con drag &drop en protocolos 
        var sortableInstanceP = null;

        function initSortableP() {
            // destruir si ya existe
            if (sortableInstanceP) {
                try { sortableInstanceP.destroy(); } catch (e) { }
                sortableInstanceP = null;
            }

            var elP = document.getElementById('olItemsP');
            if (!elP) return;

            sortableInstanceP = Sortable.create(elP, {
                animation: 150,
                ghostClass: 'bg-light',
                onEnd: function () {
                    saveOrderToHiddenP();
                },
                handle: ".list-group-item" // obliga a que el arrastre sea en todo el <li>
            });

            // guardar el orden inicial
            saveOrderToHiddenP();
        }

        function saveOrderToHiddenP() {
            var arrP = [];
            var items = document.querySelectorAll('#olItemsP > li');
            items.forEach(function (li, idx) {
                var id = li.getAttribute('data-id');
                arrP.push({ id: parseInt(id, 10), pos: idx + 1 });
            });
            // coloca el JSON en el HiddenField
            var hdnP = document.getElementById('<%= hdnOrdenSLA.ClientID %>');
            if (hdnP) hdnP.value = JSON.stringify(arrP);
            //console.log("Orden actual:", JSON.stringify(arrP));
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
        .list-group-numbered { padding-left: 0; }
        .list-group-item { 
            user-select: none; /* evita que subraye el texto */
            cursor: grab;      /* mano abierta */
        }
        .list-group-item:active {
            cursor: grabbing;  /* mano cerrada cuando arrastras */
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
                        <%--<h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Catálogo de Protocolos (SLA)</h2>--%>
                        <asp:Label ID="lblTitulo_Cat_Protocolos" runat="server" CssClass="h2 mb-3 fw-normal mt-4 align-content-center" style="display:block; text-align:center;" ></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="container col-md-8 col-lg-10 mt-4">
            <!-- Formulario principal -->
            <asp:Panel ID="MainPanel" runat="server" Visible="true">
               
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
                            <asp:DropDownList ID="ddlServicio" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlServicio_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-3">
                            <asp:Label ID="LblTipoAsunto" runat="server" Text="<%$ Resources:GlobalResources, lblTipoAsunto %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                </div>

                <%-- &nbsp;&nbsp; --%>
                
                               
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="row mb-2">
                            <div class="col-10">
                                <asp:Label ID="LblProtocolos" runat="server" Text="<%$ Resources:GlobalResources, lblProtocolos %>" ></asp:Label>
                            </div>
                            <div class="col-2 p-0 justify-content-end">
                                <asp:Button ID="btnMostrarPanel" runat="server" Text="+" OnClick="btnMostrarPanel_Click" CausesValidation="true" CssClass="btn btn-primary btn-sm py-0 px-2" TabIndex="3"/>
                            </div>
                            
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlProtocolos" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProtocolos_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4 ">
                        <div class="mb-3">
                            <asp:Label ID="LblTaskPadre" runat="server" Text="<%$ Resources:GlobalResources, lblTarea %>" ></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlTaskPadre" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTaskPadre_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                

                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCancelGrdTask" runat="server" Text="<%$ Resources:GlobalResources, btnCancelar %>" Font-Bold="True"  CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                            <asp:Button ID="BtnEditGrdTask" runat="server" Text="<%$ Resources:GlobalResources, btnEditar %>" Font-Bold="True" OnClick="BtnEditGrdTask_Click" CssClass="btn btn-primary" Enabled="false" TabIndex="1"/>
                            <asp:Button ID="BtnSaveGrdTask" runat="server" Text="<%$ Resources:GlobalResources, btnGrabar %>" Font-Bold="True" OnClick="BtnSaveGrdTask_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="container col-12 mt-4">
                    <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                        <div class="col-10" style="padding-left:14px;">
                            <%--<h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Tareas</h6>--%>
                            <h6 class="h6 fw-normal my-1" style="font-size:small">
                                <asp:Label ID="LblConsultaTareas" runat="server" Text="<%$ Resources:GlobalResources, lblConsultaTareas %>"></asp:Label>
                            </h6>
                        </div>
                        <div class="col-2" style="display:flex; justify-content: end;">
                            <asp:UpdatePanel ID="UpdPModalAddTask" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnModalAddTask" runat="server" Text="+" OnClick="BtnModalAddTask_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="BtnEditGrdTask" />
                                    <asp:PostBackTrigger ControlID="BtnSaveGrdTask" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <%-- tabla para ver las conexciones y si estan habilitadas --%>
                    <div class="row mb-2">
                        <div style="overflow-x: hidden; overflow-y: auto; max-height: 310px;">
                            <asp:UpdatePanel ID="updPnlGrdTaskConx" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>   
                                    <asp:GridView ID="grdTaskConx" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                        AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdTaskConx_RowDataBound" >
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:BoundField DataField="Columna1" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Columna2" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Columna3" >
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxSeccion_3" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grdTaskConx" />
                                    <asp:PostBackTrigger ControlID="BtnSaveGrdTask" />
                                    <asp:PostBackTrigger ControlID="btnPnlGuardarOrdenTask" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
               
                    <div class="row mb-2 mt-2">
                        <div class="col-lg-4 col-md-4">
                        </div>
                        <div class="col-lg-4 col-md-4">
                        </div>
                        <div class="col-lg-4 col-md-4">
                            <div class=" input-group input-group-sm">
                                <asp:Button ID="BtnModalOrdenarTask" runat="server" Text="<%$ Resources:GlobalResources, btnOrdenarTask %>" OnClick="BtnModalOrdenarTask_Click" Enabled="false" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>

                    <%-- Catalogo de Tareas --%>
                    <div style="overflow-x: auto; overflow-y:hidden">
                        <asp:GridView ID="GrdTasks"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" Visible="false"
                                AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                                OnPageIndexChanging="GrdTasks_PageIndexChanging" OnRowCommand="GrdTasks_RowCommand" OnPreRender="GrdTasks_PreRender"
                                OnSelectedIndexChanged="GrdTasks_SelectedIndexChanged" OnRowDataBound="GrdTasks_RowDataBound" 
                                DataKeyNames="IdTarea" PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                <AlternatingRowStyle CssClass="alt autoWidth" />
                                <Columns>
                                    <asp:BoundField DataField="NomTarea" HeaderText="<%$ Resources:GlobalResources, col_DescTarea %>" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IdTarea" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DocInterno">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Plazo" HeaderText="<%$ Resources:GlobalResources, col_Plazo %>">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:GlobalResources, col_MedidaTiempo %>">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TpoInterno" HeaderText="<%$ Resources:GlobalResources, col_TpoInterno %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UnidadTiempo" HeaderText="<%$ Resources:GlobalResources, col_UnidadTiempo %>">
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
                    <div class="col-10" style="padding-left:14px;">
                        <%--<h6 class="h6 fw-normal my-1" style="font-size:small">Agregar Nuevo Protocolo (SLA)</h6>--%>
                        <h6 class="h6 fw-normal my-1" style="font-size:small">
                            <asp:Label ID="lblAddNewProtocolo" runat="server" Text="<%$ Resources:GlobalResources, lblAddNewProtocolo %>"></asp:Label>
                        </h6>
                    </div>
                    
                </div>
                 
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblAseguradoraAddProt" runat="server" Text="<%$ Resources:GlobalResources, lblCiaSeguros %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlAseguradoraAddProt" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlAseguradoraAddProt_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblServicioAddProt" runat="server" Text="<%$ Resources:GlobalResources, lblProyecto %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlServicioAddProt" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlServicioAddProt_SelectedIndexChanged"  Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblCategoriaAddProt" runat="server" Text="<%$ Resources:GlobalResources, lblTipoAsunto %>" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlCategoriaAddProt" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoriaAddProt_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <%-- 
                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:Button ID="BtnAgregarDatos" runat="server" Text="Agregar" OnClick="BtnAgregarDatos_Click" CssClass="btn btn-primary" Visible="true" TabIndex="0" />
                    <asp:Button ID="BtnEditarConxProt" runat="server" Text="Editar Datos" OnClick="" CssClass="btn btn-primary" Visible="false" TabIndex="1" />
                    
                    <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-primary" />

                </div>
                --%>
                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:Button ID="BtnGrabarProts" runat="server" Text="<%$ Resources:GlobalResources, btnGrabar %>" Font-Bold="True" OnClick="BtnGrabarProts_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2" />
                    <asp:Button ID="BtnEditarConxProt" runat="server" Text="<%$ Resources:GlobalResources, btnEditar %>" OnClick="BtnEditarConxProt_Click" CssClass="btn btn-primary" Visible="false" TabIndex="1" />
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCancelProtocolo" runat="server" Text="<%$ Resources:GlobalResources, btnRegresar %>" OnClick="BtnCancelProtocolo_Click" CssClass="btn btn-secondary px-4" TabIndex="1"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            <div class="container col-12 mt-4">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <div class="col-10" style="padding-left:14px;">
                        <%--<h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Protocolos (SLA)</h6>--%>
                        <h6 class="h6 fw-normal my-1" style="font-size:small">
                            <asp:Label ID="lblConsultaProtocolo" runat="server" Text="<%$ Resources:GlobalResources, lblConsultaProtocolo %>"></asp:Label>
                        </h6>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <asp:UpdatePanel ID="UpdPnlAddProtocolos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnPnlAddProtocolos" runat="server" Text="+" OnClick="BtnPnlAddProtocolos_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="row mb-2">
                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                        <asp:UpdatePanel ID="upGrdProtConx" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>   
                                <asp:GridView ID="grdProtConx" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                    AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                    AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdProtConx_RowDataBound" >
                                    <AlternatingRowStyle CssClass="alt autoWidth" />
                                    <Columns>
                                        <asp:BoundField DataField="Columna1" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_1" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Columna2" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_2" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Columna3" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_3" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="grdProtConx" />
                                <asp:PostBackTrigger ControlID="BtnGrabarProts" />
                                <asp:PostBackTrigger ControlID="btnPnlGuardarOrdenTask" /> 
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="row mb-2 mt-2">
                    <div class="col-lg-4 col-md-4">
                    </div>
                    <div class="col-lg-4 col-md-4">
                        
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class=" input-group input-group-sm">
                            
                            <div class=" input-group input-group-sm">
                                <asp:Button ID="BtnModalOrdenarProtocolo" runat="server" Text="Ordenar Lista" OnClick="BtnModalOrdenarProtocolo_Click" Enabled="false" CssClass="btn btn-primary" />
                            </div> 
                            
                        </div>
                    </div>
                </div>

                <%-- Catalogo de LineaNegocios --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdProtocolos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" Visible="false"
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdProtocolos_PageIndexChanging" OnRowCommand="GrdProtocolos_RowCommand" OnPreRender="GrdProtocolos_PreRender"
                            OnSelectedIndexChanged="GrdProtocolos_SelectedIndexChanged" OnRowDataBound="GrdProtocolos_RowDataBound" 
                            DataKeyNames="IdSLA" PageSize="10" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:BoundField DataField="IdSLA" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Aseguradora" HeaderText="Aseguradora" >
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
        <asp:Panel ID="PnlAddProtocolos" runat="server" CssClass="CajaDialogo mt-5" 
            style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;" >
            <div class="row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>
            <asp:UpdatePanel ID="UdPnlModalAddProtc" runat="server" >
                <ContentTemplate>
                    <div class="container">
                        <div> <br /> <hr class="dropdown-divider" /> </div>
                        <div>
                            <br />
                                
                            <%--                                
                            <div class="row">
                                <div class="col-sm-8 mx-3">
                                    <div class ="mb-2">
                                        <asp:Label ID="LblSLA_Protocolo" runat="server" Visible="false" Text="Nombre del Protocolo (SLA)"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtSLA_Protocolo" runat="server" Visible="false" CssClass="form-control form-control-sm" placeholder="Nombre del Protocolo (SLA)" AutoComplete="off" MaxLength="60"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3 justify-content-end">
                                        
                                            <asp:Button ID="BtnAddProtocolo" runat="server" Text="+"  OnClick="BtnAddProtocolo_Click" Visible="false" CssClass="btn btn-primary" TabIndex="0"/>
                                           
                                </div>
                            </div>
                            --%>


                            <div class="mb-2">
                                <%-- Catalogo con checkList para las LineaNegocios &nbsp; --%>
                                <%-- style="overflow-x: hidden; overflow-y:auto; max-height: 225px;" 
                                        style="overflow-x: auto; overflow-y:hidden"
                                --%>
                                <asp:UpdatePanel ID="updPnlGrdAllProtocolosModal" runat="server">
                                    <ContentTemplate>
                                        <div style="overflow-x: hidden; overflow-y:auto; max-height: 410px;" >
                                            <asp:GridView ID="grdChBoxAddProtocolos" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                                AllowPaging="false" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                                AlternatingRowStyle-CssClass="alt" OnRowCommand="grdChBoxAddProtocolos_RowCommand"
                                                OnRowDataBound="grdChBoxAddProtocolos_RowDataBound" 
                                                Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                                <AlternatingRowStyle CssClass="alt autoWidth" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" Width="25px" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="IdDocumento" />
                                                    <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:GlobalResources, col_Desc_Etapa %>" >
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%-- Agregar funcionalidad al boton de eliminar --%>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkQuitar" runat="server" CommandName="Quitar" CommandArgument='<%# Container.DataItemIndex %>' Text="<%$ Resources:GlobalResources, lblEliminar %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div><br /><hr class="dropdown-divider" /></div>
                        <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                            <%-- boton para agregar a la tabla 97 --%>
                            <asp:Button ID="btnAgregarProts" runat="server" Text="<%$ Resources:GlobalResources, btnAgregar %>" OnClick="btnAgregarProts_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnCerrarModal" runat="server" Text="<%$ Resources:GlobalResources, btnCerrar %>" CssClass="btn btn-outline-secondary" /> 
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdChBoxAddProtocolos" />
                    <asp:PostBackTrigger ControlID="btnAgregarProts" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlAddTask" runat="server" CssClass="CajaDialogo mt-5" 
            style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;" >
            <div class="row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>
            <asp:UpdatePanel ID="updPnlModalAddTask" runat="server">
                <ContentTemplate>
                    <div class="container px-0">
                        <div> <br /> <hr class="dropdown-divider" /> </div>
                        <div class="ps-0">
                            <br />
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 160px;" >
                                <div class="row mb-1">
                                        <div class="col-lg-8 col-md-8 ">
                                        <div class ="mb-2">
                                            <asp:Label ID="LblTarea" runat="server" Text="<%$ Resources:GlobalResources, lblNomTarea %>"></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtTarea" runat="server" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblNomTarea %>" AutoComplete="off" MaxLength="60"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 ">
                                        <div class="input-group input-group-sm checkbox-container mb-2">
                                            <asp:CheckBox ID="chbxTaskAutom" runat="server" />
                                            <span>&nbsp;&nbsp; <asp:Literal runat="server" Text="<%$ Resources:GlobalResources, lblTareaAutomatica %>" /> </span>
                                        </div>

                                        <div class="input-group input-group-sm checkbox-container">
                                            <asp:CheckBox ID="chkTaskInterno" runat="server" />
                                            <span>&nbsp;&nbsp; <asp:Literal runat="server" Text="<%$ Resources:GlobalResources, lblTareaInterna %>" /> </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mt-1">
                                    <div class="col-lg-4 col-md-4 ">
                                        <div class="mb-2">
                                            <asp:Label ID="LblPlazo" runat="server" Text="<%$ Resources:GlobalResources, lblPlazo %>" ></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="TxtPlazo" runat="server" TextMode="Number" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblPlazo %>" AutoComplete="off" MaxLength="60"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4 ">
                                        <div class ="mb-2">
                                                <asp:Label ID="LblTimeSleep" runat="server" Text="<%$ Resources:GlobalResources, lblTimeSleep %>" ></asp:Label>
                                        </div>
                                        <div class=" input-group input-group-sm">
                                            <asp:TextBox ID="TxtTimeSleep" runat="server" TextMode="Number" CssClass="form-control form-control-sm" placeholder="<%$ Resources:GlobalResources, lblTimeSleep %>" AutoComplete="off" MaxLength="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3 ">
                                        <div class="mb-2">
                                            <asp:Label ID="LblTiempo" runat="server" Text="<%$ Resources:GlobalResources, lblMedidaTiempo %>" ></asp:Label>
                                        </div>
                                        <div class="input-group input-group-sm">
                                            <asp:DropDownList ID="ddlUnidadTiempo" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlUnidadTiempo_SelectedIndexChanged" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <%-- boton para agregar a la tabla 54 --%>
                                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAnular" runat="server" Text="<%$ Resources:GlobalResources, btnAnular %>" Font-Bold="True" OnClick="BtnAnular_Click" CssClass="btn btn-primary" Visible="false" TabIndex="0"/>
                                            <asp:Button ID="BtnEditar" runat="server" Text="<%$ Resources:GlobalResources, btnEditar %>" Font-Bold="True" OnClick="BtnEditar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="1"/>
                                            <asp:Button ID="BtnGrabar" runat="server" Text="<%$ Resources:GlobalResources, btnGrabar %>" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2"/>
                                            <asp:Button ID="BtnAgregar" runat="server" Text="<%$ Resources:GlobalResources, btnAgregar %>" Font-Bold="True" OnClick="BtnAgregar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="3"/>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                
                            </div>
                            <div class="mb-2" style="overflow-x: hidden; overflow-y:auto; max-height: 225px; ">
                                <asp:GridView ID="grdChkBxPnlAddTask" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                        AllowPaging="False" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                        AlternatingRowStyle-CssClass="alt" OnRowCommand="grdChkBxPnlAddTask_RowCommand"
                                        OnRowDataBound="grdChkBxPnlAddTask_RowDataBound"
                                        Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left"  >
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" Width="25px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IdTarea" />
                                            <asp:BoundField DataField="NomTarea" HeaderText="<%$ Resources:GlobalResources, col_NomTarea %>" >
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Plazo" HeaderText="<%$ Resources:GlobalResources, col_Plazo %>">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:GlobalResources, col_MedidaTiempo %>">
                                            </asp:BoundField>
                                            <%-- Agregar funcionalidad al boton de eliminar --%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkQuitar" runat="server" CommandName="Quitar" CommandArgument='<%# Container.DataItemIndex %>' Text="<%$ Resources:GlobalResources, lblEliminar %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div><br /><hr class="dropdown-divider" /></div>
                        <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                            <asp:Button ID="BtnInsertConxTask" runat="server" Text="<%$ Resources:GlobalResources, btnAgregar %>" OnClick="BtnInsertConxTask_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="BtnCerrarModalTask" runat="server" Text="<%$ Resources:GlobalResources, btnCerrar %>" CssClass="btn btn-outline-secondary" /> 
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdChkBxPnlAddTask" />
                    <asp:PostBackTrigger ControlID="BtnInsertConxTask" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlOrderTask" runat="server" CssClass="CajaDialogo mt-5" 
            style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;" >
            <div class="card">
                <div class="card-header">
                    <div class="row justify-content-end" data-bs-theme="dark">
                        <div class="col-1">
                            <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdPnlOrderTask" runat="server">
                    <ContentTemplate>
                            
                        <div class="card-body ps-0" style="overflow-x: hidden; overflow-y: auto; max-height: 300px;">
                            <div class="card-text">
                                <asp:HiddenField ID="hdnOrdenTask" runat="server" />
                                <asp:ListView ID="lvOrderTaskItem" runat="server" ClientIDMode="Static">
                                    <LayoutTemplate>
                                        <ol id="olItems" class="list-group list-group-numbered">
                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        </ol>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li class="list-group-item" data-id='<%# Eval("IdRelaciones") %>'>
                                            <%# Eval("NomTarea") %>
                                        </li>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                                    
                        </div>
                        <div class="card-footer d-grid gap-4 d-flex justify-content-center mb-2">
                            <asp:Button ID="btnPnlGuardarOrdenTask" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:GlobalResources, btnGuardarOrden %>" OnClick="btnPnlGuardarOrdenTask_Click"  />
                            <asp:Button ID="BtnPnlCerrarOrderTask" runat="server" CssClass="btn btn-outline-secondary" Text="<%$ Resources:GlobalResources, btnCerrar %>" OnClientClick="$find('<%= mpeOrderTask.ClientID %>').hide(); return false;" />
                        </div>
                            
                    </ContentTemplate>
                    <Triggers>

                    </Triggers>
                </asp:UpdatePanel>
            </div>

        </asp:Panel>
        <br />
        <asp:Panel ID="PnlOrderProtocolos" runat="server" CssClass="CajaDialogo mt-5" 
            style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;" >
            <div class="card">
                <div class="card-header">
                    <div class="row justify-content-end" data-bs-theme="dark">
                        <div class="col-1">
                            <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdPnlOrderSLA" runat="server" >
                    <ContentTemplate>
                        <div class="card-body ps-0" style="overflow-x: hidden; overflow-y: auto; max-height: 300px;" >
                            <div class="card-text">
                                <asp:HiddenField ID="hdnOrdenSLA" runat="server" />
                                <asp:ListView ID="lvOrderSLAItem" runat="server" ClientIDMode="Static">
                                    <LayoutTemplate>
                                        <ol id="olItemsP" class="list-group list-group-numbered">
                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        </ol>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li class="list-group-item" data-id='<%# Eval("IdRelacion") %>'>
                                            <%# Eval("Descripcion") %>
                                        </li>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                        <div class="card-footer d-grid gap-4 d-flex justify-content-center mb-2">
                            <asp:Button ID="btnPnlGuardarOrdenSLA" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:GlobalResources, btnGuardarOrden %>" OnClick="btnPnlGuardarOrdenSLA_Click" />
                            <asp:Button ID="BtnPnlCerrarOrderSLA" runat="server" CssClass="btn btn-outline-secondary" Text="<%$ Resources:GlobalResources, btnCerrar %>" OnClientClick="$find('<%= mpeOrderTask.ClientID %>').hide(); return false;" />
                        </div>
                    </ContentTemplate>
                    <Triggers>

                    </Triggers>
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
                <td>&nbsp;</td>
                <td>
                    <ajaxToolkit:ModalPopupExtender ID="mpeNewsProtocolos" runat="server" PopupControlID="PnlAddProtocolos"
                        TargetControlID="LblOcultPnlProtocolos" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewsProtocolos()">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Label ID="LblOcultPnlProtocolos" Text="Lbloculto" runat="server" Style="display: none;" />
                </td>
                <td>&nbsp;</td>
                <td>
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeNewsTask" runat="server" PopupControlID="PnlAddTask"
                                TargetControlID="LblOcultPnlAddTask" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewTask()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="LblOcultPnlAddTask" Text="LblocultoPnlAddTask" runat="server" Style="display: none;" />
                        </div>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeOrderTask" runat="server" 
                                PopupControlID="PnlOrderTask" TargetControlID="LblocultoPnlOrderTask" 
                                BackgroundCssClass="FondoAplicacion" OnOkScript="mpeOrderTask()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="LblocultoPnlOrderTask" Text="LblocultoPnlOrderTask" runat="server" Style="display: none;" />
                        </div>
                    </div>
                </td>
                <td>&nbsp;</td>
                <td>
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeOrderSLA" runat="server" 
                                PopupControlID="PnlOrderProtocolos" TargetControlID="LblOcultoPnlOrderSLA" 
                                BackgroundCssClass="FondoAplicacion" OnOkScript="mpeOrderSLA()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="LblOcultoPnlOrderSLA" Text="LblocultoPnlOrderSLA" runat="server" Style="display: none;" />
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
            <%--<asp:PostBackTrigger ControlID="BtnAddProtocolo" />--%>
            <asp:PostBackTrigger ControlID="BtnCancelProtocolo" />
            <asp:PostBackTrigger ControlID="BtnPnlAddProtocolos" />
            <asp:PostBackTrigger ControlID="btnAgregarProts" />
            <asp:PostBackTrigger ControlID="btnCerrarModal" />
            <asp:PostBackTrigger ControlID="BtnCerrarModalTask" />
            <asp:PostBackTrigger ControlID="BtnInsertConxTask" />
            <asp:PostBackTrigger ControlID="BtnPnlCerrarOrderTask" />
            <asp:PostBackTrigger ControlID="btnPnlGuardarOrdenTask" />
            <asp:PostBackTrigger ControlID="BtnPnlCerrarOrderSLA" />
            <asp:PostBackTrigger ControlID="btnPnlGuardarOrdenSLA" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
