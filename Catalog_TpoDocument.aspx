<%@ Page Title="" Language="C#" EnableViewState="false" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Catalog_TpoDocument.aspx.cs" Inherits="WebItNow.Catalog_TpoDocument" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="Scripts/jquery-3.4.1.min.js"></script>
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

       <%-- function showProgress() {
            var updateProgress = $get("<%= UpdateProgress1.ClientID %>");
            updateProgress.style.display = "block";
        }

        $('form').live("submit", function () {
            ShowProgress();
        });--%>

    </script>

    <link href="~/Scripts/footable.min.js" rel="stylesheet" type="text/javascript" />
    <link href="~/Styles/footable.min.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional">
    <ContentTemplate>
    <div class="container col-md-4">

        <h2 class="h2 my-3 fw-normal">Alta de documento</h2>

        <asp:Label ID="LblDescripcion" runat="server" CssClass="" Text="Incerte el nombre del archivo a pedir y en seguida las instrucciones de como se debe subir el archivo a pedir"></asp:Label>

        <div class="mt-3">
            <asp:Label ID="LblNameDoc" runat="server" Text="Nombre del Documento"></asp:Label>
            <asp:TextBox ID="TxtNameDoc" runat="server" CssClass="form-control mt-1" placeholder="Nombre del Archivo"></asp:TextBox>
        </div>
        <asp:TextBox type="hidden" ID="TxtIdTpoDocumento" runat="server" CssClass="form-control" ></asp:TextBox>
        <div class="justify-content-center mt-3">
            <asp:Label ID="LblInstrucciones" runat="server" Text="Instrucciones"></asp:Label>
            <textarea rows="2" cols="64" id="TxtAreaMensaje" runat="server" class="form-control mt-1"/>
        </div>

        <div class="d-grid gap-4 d-flex justify-content-center">
            <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False"  Width="280px"></asp:Label>
        </div>
        
        <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
            <asp:Button ID="BtnAgregar" runat="server" Text="Agregar" OnClick="BtnAgregar_Click" CssClass="btn btn-primary px-4" />
            <asp:Button ID="BtnUpdate" runat="server" Text="Actualizar" OnClick="BtnUpdate_Click" CssClass="btn btn-primary" />
            <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" CssClass="btn btn-outline-danger" />
        </div>


        <div style="overflow-x: auto; overflow-y:hidden">
            <asp:GridView ID="GrdTpoDocumento" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%"
                AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
                OnSelectedIndexChanged="GrdTpoDocumento_SelectedIndexChanged" OnRowDataBound="GrdTpoDocumento_RowDataBound"
                OnPageIndexChanging="GrdTpoDocumento_PageIndexChanging" DataKeyNames="IdTpoDocumento">
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="IdTpoDocumento" HeaderText="Id documento" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Tipo documento" />
                    <asp:BoundField DataField="DescrpBrev" HeaderText="Instrucciones" />
                </Columns>
                <PagerStyle CssClass="pgr" />
            </asp:GridView>
        </div>

        <div class="d-grid gap-4 d-flex justify-content-center mt-1 mb-5 pb-5">
            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-link" />
        </div>
        
        <!-- Modal -->
        <div class="modal" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        
                    </div>
                    <div class="modal-body">
                        
                        <div class="text-center">
                            <div class="spinner-border" role="status">
                                <span class="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
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
</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID ="GrdTpoDocumento" />
        </Triggers>
</asp:UpdatePanel>

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

</asp:Content>
