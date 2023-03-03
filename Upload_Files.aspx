<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Upload_Files.aspx.cs" Inherits="WebItNow.Upload_Files" %>
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
            location.href = '/Access.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        $(function () {
            $('[id*=GridView1]').footable();
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <ContentTemplate>
    
    <div class="container col-md-8">
        <h2 class="h2 mb-3 fw-normal">CARGA SEGURA DE DOCUMENTOS</h2>

        <div class="form-group ">
            <div class="d-grid col-12 justify-content-center mx-auto py-1">
                <asp:Label ID="lblReferencia" runat="server" Font-Size="X-Large" ></asp:Label>
            </div>
        </div>
        <br />
        <br />

        <div class="form-group">
            <div class="col-12">
            <div class="d-grid gap-2 d-flex justify-content-center">
                <div style="overflow-x: auto; overflow-y:hidden">
                    <asp:GridView ID="grdEstadoDocs" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%"
                        AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
                        PageSize="10" OnRowCommand="grdEstadoDocs_RowCommand"
                        OnPageIndexChanging="grdEstadoDocs_OnPageIndexChanging" OnRowDataBound="grdEstadoDocs_RowDataBound" DataKeyNames="IdTipoDocumento">
                        <AlternatingRowStyle CssClass="table" />
                        <Columns>
                            <asp:BoundField DataField="IdTipoDocumento" HeaderText="Id" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Documento" />
                            <asp:BoundField DataField="Nom_Imagen" HeaderText="Nombre de archivo" />
                            <asp:BoundField DataField="Desc_status" HeaderText="Estatus" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <asp:Button ID="BtnCargaDocumento" runat="server" Text="Carga Documento" Font-Bold="True" CommandName="CargaDocumento" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CssClass="btn btn-primary" />
                            </ItemTemplate> 
                        </asp:TemplateField>

                        </Columns>
                        <PagerStyle CssClass="pgr" />
                    </asp:GridView>
                </div>
            </div>
            </div>
        </div>

        <div class="from-group">
            <div class="d-grid col-2 mx-auto">
                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Font-Bold="True" OnClick="BtnSalir_Click" CssClass="btn btn-outline-primary mt-3 mb-4 pb-5"/>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
            </div>

            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                    TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()">
                </ajaxToolkit:ModalPopupExtender>
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

</asp:Content>
