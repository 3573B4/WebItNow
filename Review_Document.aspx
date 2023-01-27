<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Review_Document.aspx.cs" Inherits="WebItNow.Review_Document" %>
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

        function clearTextBox() {
            var elements = [];
            elements = document.getElementsByClassName("form-control");

            for (var i = 0; i < elements.length; i++) {
                elements[i].value = "";
            }
        }

        function setFormSubmitToFalse() {
            setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
            return true;
        }

        function timedRefresh(timeoutPeriod) {
            setTimeout("location.reload(true);", timeoutPeriod);
        }

        function realizarPostBack() {
            __doPostBack('', '');
        }

        function openPageDescargas() {

            window.open('Descargas.aspx');
            return true;
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
    <br />
    <div class="container col-md-4">
        <h2 class="h2 mb-3 fw-normal">Validación de Documentos</h2>
        <br />
        <h2 class="h2 mb-3 fw-normal">Pendientes</h2>
        <br />
        <div style="overflow-x: auto; overflow-y:hidden">
            <asp:GridView ID="grdEstadoDocumento"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="1400px"
                    AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grdEstadoDocumento_OnPageIndexChanging"
                    PageSize="10" OnRowDataBound ="grdEstadoDocumento_RowDataBound" DataKeyNames="IdUsuario" >
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgAceptado" runat="server" ImageUrl="~/Images/aceptar.png" Height="35px" Width="35px" OnClick="imgAceptado_Click" Enabled="false" />
                            </ItemTemplate> 
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgRechazado" runat="server" ImageUrl="~/Images/cancelar.png" Height="35px" Width="35px" OnClick="imgRechazado_Click" Enabled="false" />
                            </ItemTemplate> 
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDescarga" runat="server" ImageUrl="~/Images/descargar.png" Height="35px" Width="35px" OnClick="ImgDescarga_Click" Enabled="true" />
                            </ItemTemplate> 
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="IdUsuario" HeaderText="Id. Usuario" >
                        <ItemStyle Width="600px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Desc_Status" HeaderText="Id. Status" >
                        <ItemStyle Width="600px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Tipo de Documento" >
                        <ItemStyle Width="1250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nom_Imagen" HeaderText="Nombre de Archivo" >
                        <ItemStyle Width="1750px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Url_Imagen" HeaderText="Url_Imagen" >
                        <ItemStyle Width="600px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IdTipoDocumento" HeaderText="Id. de Documento" >
                        <ItemStyle Width="600px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IdDescarga" HeaderText="Descarga(s)" >
                        <ItemStyle Width="200px" />
                        </asp:BoundField>
                                

                    </Columns>
                
                    <PagerStyle CssClass="pgr" />
            </asp:GridView>
        </div>
        <div class="">
            <asp:HiddenField ID="hdfValorGrid" runat="server" Value=""/>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" ></asp:Label>
            </div>
        </div>
        <br />
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
                <asp:Button ID="BtnPruebas" runat="server" Text="Pruebas" OnClick="BtnPruebas_Click" />
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
            <asp:Button ID="btnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
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
            <td></td>
            <td></td>
        </tr>
    </table>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="BtnPruebas"/>
    </Triggers>
</asp:UpdatePanel> 
</asp:Content>
