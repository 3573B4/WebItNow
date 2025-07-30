<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Configuracion_Proyect.aspx.cs" Inherits="WebItNow_Peacock.fwGM_Configuracion_Proyect" %>
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

        function mpeNewDocumento() {
            //
        }

        function mpeNewProceso() {

        }

        function mpeExpiraOnOk() {

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
    
    <style type="text/css">
        .disabled-checkbox {
            opacity: 0.7;               /* Cambia la opacidad para dar un efecto visual similar */
            background-color: #f5f5f5;  /* Fondo más claro */
            color: #333;                /* Color de texto oscuro */
            pointer-events: none;       /* Evita que los usuarios interactúen con él */
            cursor: not-allowed;        /* Cambia el cursor para indicar que no se puede hacer clic */
        }
    </style>

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
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Configuración de Categorías al Proyecto</h2>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-1" style="font-size:small">&nbsp;</h5>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCliente" runat="server" Text="Nombre del Cliente" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <%--<asp:DropDownList ID="ddlCliente" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged" Width="100%">--%>
                            <%--</asp:DropDownList>--%>
                            <asp:TextBox ID="TxtCliente" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre del Cliente" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblProyecto" runat="server" Text="Proyecto" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <%--<asp:DropDownList ID="ddlProyecto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" Width="100%">--%>
                            <%--</asp:DropDownList>--%>
                            <asp:TextBox ID="TxtNomProyecto" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre del Proyecto" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTpoAsunto" runat="server" Text="Tipo de Asunto" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="TxtTpoAsunto" runat="server" CssClass="form-control form-control-sm" placeholder="Tipo de Asunto" AutoComplete="off" MaxLength="30" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                </div>

<%--  
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTpoEstatus" runat="server" Text="Tipo de Estatus" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlTpoEstatus" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoStatus_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                </div>
--%>
                <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                    <h5 class="h1 fw-normal my-1" style="font-size:small">&nbsp;</h5>
                </div>

<%--                
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCarpetas" runat="server" Text="Carpeta en la cual desea trabajar" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:DropDownList ID="ddlCarpetas" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCarpetas_SelectedIndexChanged" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                    <div class="col-lg-4 col-md-4">
                    </div>
                </div>
--%>

                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:Button ID="BtnAgregarDatos" runat="server" Text="Agregar" OnClick="BtnAgregarDatos_Click" CssClass="btn btn-primary" Visible="true" TabIndex="0" />
                    <asp:Button ID="BtnEditar" runat="server" Text="Editar Datos" OnClick="BtnEditar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="1" />
                    <asp:Button ID="BtnGrabar" runat="server" Text="Aplicar Cambios" Font-Bold="True" OnClick="BtnGrabar_Click" CssClass="btn btn-primary" Visible="false" TabIndex="2" />
                    <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-primary" />

                    <%--<asp:Button ID="btnEliminarDatos" runat="server" Text="Eliminar" OnClick="btnEliminarDatos_Click" CssClass="btn btn-primary" Enabled="false" />--%>
                    <%--<asp:Button ID="btnEditarDatos" runat="server" Text="Editar Datos" OnClick="btnEditarDatos_Click" CssClass="btn btn-primary" Enabled="false" />--%>
                </div>
                <br />
<%--
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="lblArchivo" runat="server" Text="ruta archivo" ></asp:Label>
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:TextBox ID="txtFilePath" runat="server" CssClass="form-control form-control-sm" placeholder="ruta archivo" AutoComplete="off" MaxLength="200"  ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            &nbsp;
                        </div>
                        <div class=" input-group input-group-sm">
                            <asp:Button ID="btnReadExcel" runat="server" Text="Leer Excel" OnClick="btnReadExcel_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
--%>
                <%--controles pnl1--%>
                <div class="row" style="background-color:mediumturquoise; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblEtiquetaPnl1" runat="server" Text="DOCUMENTOS REQUERIDOS POR " CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <div>
<%--                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                    <asp:Button ID="btnShowPanel1" runat="server" Text="&#9660;" OnClick="btnShowPanel1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
<%--                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>

                <div>
                    <asp:Panel ID="pnl1" runat="server" Visible="true">
<%--
                        <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                            <h5 class="h1 fw-normal my-1" style="font-size:small">ASEGURADOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)</h5>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblAsegurados" runat="server" Text="ASEGURADOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion1" runat="server" Text="+" OnClick="BtnSeccion1_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_1" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                            AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_1_RowDataBound" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Columna1" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_1_1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna2" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_1_2" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna3" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_1_3" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_1" />
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
                                    <asp:Button ID="BtnPnl1Seleccionar_1" runat="server" Text="SELECCIONAR TODOS" OnClick="BtnPnl1Seleccionar_1_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

<%--
                        <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                            <h5 class="h1 fw-normal my-1" style="font-size:small">ASUNTOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)</h5>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblAsuntos" runat="server" Text="RIESGOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion2" runat="server" Text="+" OnClick="BtnSeccion2_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_2" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                            AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_2_RowDataBound" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Columna1" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_2_1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna2" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_2_2" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna3" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_2_3" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_2" />
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
                                    <asp:Button ID="BtnPnl1Seleccionar_2" runat="server" Text="SELECCIONAR TODOS" OnClick="BtnPnl1Seleccionar_2_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

<%--
                        <div class="row mb-3 mt-4 " style="background-color:#96E7D9;">
                            <h5 class="h1 fw-normal my-1" style="font-size:small">CUADERNOS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)</h5>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblCuadernos" runat="server" Text="ESTATUS A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion3" runat="server" Text="+" OnClick="BtnSeccion3_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_3" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                            AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_3_RowDataBound" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Columna1" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_3_1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna2" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_3_2" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna3" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_3_3" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_3" />
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
                                    <asp:Button ID="BtnPnl1Seleccionar_3" runat="server" Text="SELECCIONAR TODOS" OnClick="BtnPnl1Seleccionar_3_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

<%--
                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="LblBienes" runat="server" Text="BIENES A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion4" runat="server" Text="+" OnClick="BtnSeccion4_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_4" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                            AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_4_RowDataBound">
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="Columna1" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_4_1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna2" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_4_2" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Columna3" >
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxSeccion_4_3" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_4" />
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
                                    <asp:Button ID="BtnPnl1Seleccionar_4" runat="server" Text="SELECCIONAR TODOS" OnClick="BtnPnl1Seleccionar_4_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
--%>

                        <div class="row mb-3 mt-4" style="background-color:#96E7D9; align-items: baseline;">
                            <div class="col-10" style="padding-left: 14px;">
                                <asp:Label ID="Label1" runat="server" Text="OTROS DETALLES A LOS QUE APLICA(N) ESTE/OS DOCUMENTO(S)" CssClass="control-label" Font-Size="small"></asp:Label>
                            </div>
                            <div class="col-2" style="display:flex; justify-content: end;">
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnSeccion5" runat="server" Text="+" OnClick="BtnSeccion5_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div style="overflow-x: hidden; overflow-y: auto; max-height: 295px;">
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdSeccion_5" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                    AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" 
                                    AlternatingRowStyle-CssClass="alt" Font-Size="Smaller" ShowHeader="false" OnRowDataBound="grdSeccion_5_RowDataBound" >
                                    <AlternatingRowStyle CssClass="alt autoWidth" />
                                    <Columns>
                                        <asp:BoundField DataField="Columna1" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_5_1" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Columna2" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_5_2" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Columna3" >
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChBoxSeccion_5_3" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdSeccion_5" />
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
                                    <asp:Button ID="BtnPnl1Seleccionar_5" runat="server" Text="SELECCIONAR TODOS" OnClick="BtnPnl1Seleccionar_5_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </asp:Panel>
                </div>
                <br />
                <!-- Puedes agregar más botones y paneles según sea necesario -->
            </div>

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
            <asp:Panel ID="PnlDocProceso" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;">
                <div class="row justify-content-end" data-bs-theme="dark">
                    <div class="col-1">
                        <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server" >
                    <ContentTemplate>
                        <div class="container">
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div>
                                <br />
                                <div class="d-flex flex-row mx-4 m-0 p-0">
                                    <asp:Label ID="LblDocumentos" runat="server" Text="Descripción de Categoría" CssClass="form-label my-0 p-0" />
                                </div>
                                <div class="col-sm-6 mx-3">
                                    <asp:Panel runat="server" DefaultButton="BtnBuscar">
                                        <asp:TextBox ID="TxtDescripcion" runat="server" placeholder="Descripción de Categoría" CssClass="form-control form-control-sm" onkeyup="mayus(this);"></asp:TextBox>
                                        <asp:Button ID="BtnBuscar" runat="server" OnClick="BtnBuscar_Click" Style="display: none" />
                                    </asp:Panel>
                                </div>
<%--                                
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                        <asp:GridView ID="grdPnlBusqProceso" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" 
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grdPnlBusqProceso_PageIndexChanging"
                                            PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:BoundField DataField="IdDocumento" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Categoría" >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgDocumento_Del" runat="server" OnClick="ImgDocumento_Del_Click" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png"  Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>
--%>
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                        <asp:GridView ID="grdPnlBusqProceso" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="False" CssClass="table table-responsive table-light table-striped table-hover align-middle"
                                            AlternatingRowStyle-CssClass="alt" OnRowCommand="grdPnlBusqProceso_RowCommand" 
                                            OnRowDataBound="grdPnlBusqProceso_RowDataBound"
                                            Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IdDocumento" HeaderText="" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción de Categoría" >
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkQuitar" runat="server" CommandName="Quitar" CommandArgument='<%# Container.DataItemIndex %>' Text="eliminar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <asp:Button ID="btnAgregar_Proceso" runat="server" Text="Agregar" OnClick="btnAgregar_Proceso_Click" CssClass="btn btn-outline-primary btn-sm" />
                                <asp:Button ID="btnClose_Proceso" runat="server" OnClick="btnClose_Proceso_Click" Text="Cerrar" CssClass="btn btn-outline-secondary btn-sm" />
                            </div>
                            <br />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdPnlBusqProceso" />
                    </Triggers>
                </asp:UpdatePanel>
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
                </tr>
                <tr>
                    <td>
                        <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                            TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                    </td>
                    <td class="style3"><asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
                    </td>
                    <td>
                        <ajaxToolkit:ModalPopupExtender ID="mpeNewProceso" runat="server" PopupControlID="PnlDocProceso"
                            TargetControlID="LblOculto1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProceso()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="LblOculto1" runat="server" Text="Label" Style="display: none;" />
                    </td>
                </tr>
            </table>
            <br />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="BtnAgregarDatos" />
        <asp:PostBackTrigger ControlID="btnAgregar_Proceso" />
        <asp:PostBackTrigger ControlID="BtnGrabar" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
