<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Review-Document.aspx.cs" Inherits="WebItNow.Review_Document" %>
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

        function realizarPostBack() {
            __doPostBack('', '');
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
    <br />
    <div class="container col-md-4">
                        <h2 class="h2 mb-3 fw-normal">Validación de Documentos</h2>
            <div class="form-group">
                <asp:Label ID="LblUsu" runat="server" Text="Usuario" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
                <div class="col-sm-12">
                    <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="Usuario" ReadOnly="True"></asp:TextBox>
                </div>
            </div>
            <br />        
            <div class="form-group">
                <asp:Label ID="LblTpoDocumento" runat="server" Text="Tpo. de Documento" CssClass="control-label col-sm-2"></asp:Label>
                <div class="col-sm-12">
                    <asp:TextBox ID="TxtTpoDocumento" runat="server" CssClass="form-control" placeholder="Tipo de Documento" ReadOnly="True"></asp:TextBox>
                    <asp:TextBox ID="TxtUrl_Imagen" runat="server" CssClass="form-control" placeholder="Ruta del archivo" Enabled="False" Visible="false" ></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="form-group">
                <asp:Label ID="LblNomArchivo" runat="server" Text="Archivo para descargar" CssClass="control-label col-sm-2"></asp:Label>
                <div class="col-12">
                        <div class="d-grid gap-2 d-flex justify-content-center">
                            <asp:TextBox ID="TxtNomArchivo" runat="server" CssClass="form-control" placeholder="Archivo para descargar" ReadOnly="True"></asp:TextBox>
                            <asp:ImageButton ID="imgDescarga" runat="server" ImageUrl="~/Images/descargar.png" Height="35px" Width="35px" OnClick="ImgDescarga_Click" OnClientClick="clearTextBox()" />
                        </div>
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="d-grid col-6 mx-auto">
                    <asp:Button ID="BtnUnLoad" runat="server" Text="Descarga Archivo" Font-Bold="True" OnClick="BtnUnLoad_Click" CssClass="btn btn-primary" Visible="False" />
                </div>
            </div>
            <br />
            <div class="from-group">
                <div class="d-grid col-6 mx-auto">
                    <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
                </div>
            </div>
            <br />
            <h2 class="h2 mb-3 fw-normal">Pendientes</h2>
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="grdEstadoDocumento"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="686px"
                            AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            PageSize="5" OnSelectedIndexChanged="grdEstadoDocumento_SelectedIndexChanged" OnRowDataBound ="grdEstadoDocumento_RowDataBound" DataKeyNames="IdUsuario" >
                            <AlternatingRowStyle CssClass="alt" />
                            <Columns>
                                <asp:BoundField DataField="IdUsuario" HeaderText="Id. Usuario" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Tipo de Documento" />
                                <asp:BoundField DataField="Nom_Imagen" HeaderText="Nombre de Archivo" />
                                <asp:BoundField DataField="Desc_Status" HeaderText="Id. Status" />
                                <asp:BoundField DataField="Url_Imagen" HeaderText="Url_Imagen" />
                                <asp:BoundField DataField="IdTipoDocumento" HeaderText="Id. de Documento" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                         <asp:Button ID="BtnRechazado" runat="server" Text="Rechazado" OnClick ="BtnRechazado_Click" />
                                    </ItemTemplate> 
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="BtnAceptado" runat="server" Text="Aceptado" OnClick ="BtnAceptado_Click" />
                                    </ItemTemplate> 
                                </asp:TemplateField>
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
            <td></td>
            <td></td>
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
        <asp:PostBackTrigger ControlID="imgDescarga" />
    </Triggers>
</asp:UpdatePanel> 
</asp:Content>
