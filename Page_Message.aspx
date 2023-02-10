<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Page_Message.aspx.cs" Inherits="WebItNow.Page_Message" %>

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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <ContentTemplate>

    <div class="container col-md-4">
        <h2 class="h2 mt-3 mb-5 fw-normal">Tu petición a sido procesada.</h2>

<%--        
        <div class="form-group">
            <div class="d-grid col-4 mx-auto py-1">
                <asp:Label ID="lblUsuario" runat="server" Font-Size="X-Large" ></asp:Label>
            </div>
        </div>
--%>

        <div class="form-group my-3 ">
            <asp:Label ID="LblMotivo" runat="server" Text="" Font-Size="Large"></asp:Label>
        </div>

        <div class="form-group my-3">
            <textarea rows="6" cols="64" id="TxtAreaMensaje" runat="server" class="form-control"/>
        </div>
<%--    <div class="form-group">
            <asp:Button ID="BtnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="BtnEnviar_Click"/>
        </div>--%>

        <div class="form-group mt-4">
            <div class="col-12">                
                <div class="d-grid gap-4 d-flex justify-content-center">
                    <%--CssClass="btn btn-outline-success p-1"--%>
                    <asp:ImageButton ID="imgWhatsApp" runat="server" ImageUrl="~/Images/whatsapp-logo_icon.png"  Height ="45px" Width="45px" OnClick="BtnEnviarWhats_Click" Enabled ="false" />
                    <%--CssClass="btn btn-outline-primary p-1"--%> 
                    <asp:ImageButton ID="imgEmail" runat="server" ImageUrl="~/Images/email-icon.png" Height="45px" Width="45px" OnClick="BtnEnviarEmail_Click" />

                    <%--<asp:Button ID="BtnEnviar" runat="server" Text="Enviar"  Font-Bold="True"  CssClass="btn btn-primary me-md-2" OnClick="BtnEnviar_Click"/>--%>
                    <asp:Button ID="BtnRegresar" runat="server" Font-Bold="True" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-outline-primary"/>
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
            <br />
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
                <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
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
