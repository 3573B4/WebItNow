<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Consulta_Document.aspx.cs" Inherits="WebItNow.Consulta_Document" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        
        var timer = setTimeout(function () {
            document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
            var modalId = '<%=mpeExpira.ClientID%>';
            var modal = $find(modalId);
            modal.show();

            //alert("La sesión ha expirado.");
            //location.href = '/Login.aspx';
        }, 600000);

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        $(function () {
            $('[id*=GridView1]').footable();
        });


    </script>

    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <link href="~/Scripts/footable.min.js" rel="stylesheet" type="text/javascript" />
    <link href="~/Styles/footable.min.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <div class="container col-lg-6">
            <h2 class="h2 mb-3 fw-normal mt-4">Reporte de archivos por estatus</h2>
            
<%--            <div class="form-group">
                <div class="d-grid col-4 mx-auto py-1">
                    <asp:Label ID="lblUsuario" runat="server" Font-Size="XX-Large" ></asp:Label>
                </div>
            </div>--%>

            <div class="form-group mb-4">
                <div class="d-grid col-12 justify-content-center mx-auto py-1">
                    <asp:Label ID="lblRef" runat="server" Font-Size="X-Large" ></asp:Label>
                 </div>
            </div>

            <div class="form-floating my-3">
                <div class="dropdown">
                    <asp:DropDownList ID="ddlStatusDocumento" runat="server" CssClass="btn btn-outline-secondary text-start mt-1" AppendDataBoundItems="true" AutoPostBack="true" Width="100%">
                        <asp:ListItem Value="0"> -- Seleccionar todos -- </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div style="overflow-x: auto; overflow-y:hidden">
                <asp:GridView ID="grdConsultaDocumento" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%"
                    AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt"
                    OnPageIndexChanging="grdConsultaDocumento_PageIndexChanging" PageSize="8" >
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--<asp:ImageButton ID="imgAceptado" runat="server" ImageUrl="~/Images/aceptar.png" Height="35px" Width="35px" OnClick="imgAceptado_Click" Enabled="false" />   --%>
                                <%--<asp:ImageButton ID="imgRechazado" runat="server" ImageUrl="~/Images/cancelar.png" Height="35px" Width="35px" OnClick="imgRechazado_Click" Enabled="false" />   --%>
                                <%--aqui va el boton de descarga--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="IdUsuario" HeaderText="Usuario" />   --%>
                        <asp:BoundField DataField="Referencia" HeaderText="Referencia" />
                        <asp:BoundField DataField="Fec_Envio" HeaderText="Recepción" />
                        <%--<asp:BoundField DataField="Nom_Imagen" HeaderText="Archivo" />  --%>
                        <asp:BoundField DataField="Desc_status" HeaderText="Estatus" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Tipo de Documento" />
                        <asp:BoundField DataField="Fec_Aceptado" HeaderText="Aprobación" />
                        <asp:BoundField DataField="Fec_Rechazado" HeaderText="Rechazo" />
                        <asp:BoundField DataField="Url_Imagen" HeaderText="Directorio" />
                        <asp:BoundField DataField="IdDescarga" HeaderText="Descargas" />
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>
            <div class="form-group d-grid col-md-3 mx-auto pt-3">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link mb-4 pb-5" Visible="false" />
            </div>
            <div class="form-group">
                <div class="d-grid col-6 mx-auto">
                    <ajaxtoolkit:modalpopupextender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                        TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()">
                    </ajaxtoolkit:modalpopupextender>
                    <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                </div>
                <br />
                <div class="d-grid col-6 mx-auto">
                    <ajaxtoolkit:modalpopupextender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                        TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()">
                    </ajaxtoolkit:modalpopupextender>
                    <asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
                </div>
            </div>
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
            <asp:Button ID="btnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
        </div>


    </asp:Panel>

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
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="grdConsultaDocumento" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
