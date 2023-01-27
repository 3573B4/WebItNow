﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Page_Message.aspx.cs" Inherits="WebItNow.Page_Message" %>

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
            <h2 class="h2 mb-3 fw-normal">Pantalla de mensaje</h2>
            <div class="form-group my-3">
                <asp:Label ID="lblUsuario" runat="server" Text=""></asp:Label>
            </div>
            <div class="form-group my-3">
                <asp:Label ID="LblMotivo" runat="server" Text="Motivo de rechazo/aceptado/Enviado" Font-Size="Large"></asp:Label>
            </div>
            <div class="form-group my-3">
                <textarea rows="6" cols="64" id="TxtAreaMensaje" runat="server" class="form-control"/>
            </div>
            <div class="form-group">
                <asp:Button ID="BtnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="BtnEnviar_Click"/>
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