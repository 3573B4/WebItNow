<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwGM_Alta_Proyecto.aspx.cs" Inherits="WebItNow_Peacock.fwGM_Alta_Proyecto" %>
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

    <style type="text/css">
        .group-container {
            border: 1px solid #ccc;
        padding: 10px;
        margin-bottom: 10px;
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
                        <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Alta de Proyecto</h2>
                    </div>
                </div>
            </div>
        </div>
        <div class="container col-md-8 col-lg-10 mt-4">
            <div class="row mb-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblCliente" runat="server" Text="Cliente"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4">
                    <div class ="mb-2">
                        <asp:Label ID="LblTpoAsunto" runat="server" Text="Tipo de Asunto" ></asp:Label>
                    </div>
                    <div class=" input-group input-group-sm">
                        <asp:DropDownList ID="ddlTpoAsunto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoAsunto_SelectedIndexChanged" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblProyecto" runat="server" Text="Nombre del Proyecto" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNomProyecto" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Nombre del Proyecto" Text="NINGUNO" AutoComplete="off" MaxLength="25"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblGerente" runat="server" Text="Gerente Responsable" ></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlGerentes" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlGerentes_SelectedIndexChanged" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                    </div>
                    <div class="input-group input-group-sm">
                    </div>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-lg-8 col-md-8 ">
                    <div class ="mb-2">
                        <asp:Label ID="LblDescProyecto" runat="server" Text="Descripción del Proyecto"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtDescProyecto" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-lg-4 col-md-4">
                    <div class="group-container">
                        <asp:RadioButton ID="rbGestionCasos1" runat="server" Text="&nbsp; Un solo Asegurado" OnCheckedChanged="rbGestionCasos1_CheckedChanged" AutoPostBack="true" GroupName="GrupoCasos" />
                        <br />
                        <br />
                        <asp:RadioButton ID="rbGestionCasos2" runat="server" Text="&nbsp; Multiples Asegurado" OnCheckedChanged="rbGestionCasos2_CheckedChanged" AutoPostBack="true" GroupName="GrupoCasos" />
                    </div>
                </div>
            </div>

            <div class="row mb-3">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PnlGestionCasos" runat="server" Visible="false">
                            <!-- Controles dentro del Panel -->
                            <div class="row mb-3">
                                <div class="col-lg-4 col-md-4 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNumPoliza" runat="server" Text="Número de Póliza"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNumPoliza" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Número de Póliza" AutoComplete="off" MaxLength="18"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblNomAsegurado" runat="server" Text="Nombre del Asegurado"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtNomAsegurado" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Nombre del Asegurado" AutoComplete="off" MaxLength="80"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblTpoAsegurado" runat="server" Text="Tipo de Asegurado"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:DropDownList ID="ddlTpoAsegurado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoAsegurado_SelectedIndexChanged" visible="true" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-lg-4 col-md-4 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblIniVigencia" runat="server" Text="Inicio de Vigencia"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtIniVigencia" runat="server" TextMode="Date" placeholder="Select date" style="font-size:16px;" class="form-control form-control-sm" TabIndex="1" />
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 ">
                                    <div class="mb-2">
                                        <asp:Label ID="LblFinVigencia" runat="server" Text="Fin de Vigencia"></asp:Label>
                                    </div>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="TxtFinVigencia" runat="server" TextMode="Date" placeholder="Select date" style="font-size:16px;" class="form-control form-control-sm" TabIndex="1" />
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 ">
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <%--<asp:Button ID="BtnAgregar" runat="server" Text="Agregar" OnClick="BtnAgregar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="11"/>--%>
                        <asp:Button ID="BtnSiguiente" runat="server" Text="Siguiente" OnClick="BtnSiguiente_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="11"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="container col-12 mt-4">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">Consulta de Proyectos</h6>
                </div>

                <%-- Catalogo de proyectos --%>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="GrdProyectos"  runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%" 
                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdProyectos_PageIndexChanging" OnRowCommand="GrdProyectos_RowCommand" OnPreRender="GrdProyectos_PreRender"
                            OnSelectedIndexChanged="GrdProyectos_SelectedIndexChanged" OnRowDataBound="GrdProyectos_RowDataBound" 
                            DataKeyNames="IdProyecto" PageSize="10" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt autoWidth" />
                            <Columns>
                                <asp:TemplateField HeaderText="Nombre del proyecto">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkProyecto" runat="server" UseSubmitBehavior="true" style="display: block; text-align: left; text-decoration: none;" CommandName="SelectProyecto" CommandArgument='<%# Eval("Descripcion") + "|" + Eval("IdProyecto") + "|" + Eval("NomCliente") + "|" + Eval("DescAsunto")  + "|" + Eval("IdCliente") + "|" + Eval("IdTpoEvento") %>'><%# Eval("Descripcion") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
<%--
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción del proyecto" >
                                </asp:BoundField>
--%>
                                <asp:BoundField DataField="NomCliente" HeaderText="Cliente" >
                                </asp:BoundField>
                                <asp:BoundField DataField="DescAsunto" HeaderText="Tipo de Asunto" >
                                </asp:BoundField>
                                <asp:BoundField DataField="NumPoliza" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdProyecto" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdCliente" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdTpoEvento" >
                                </asp:BoundField>
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
            <%--<asp:PostBackTrigger ControlID="BtnAgregar" />--%>
            <asp:PostBackTrigger ControlID="BtnSiguiente" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
