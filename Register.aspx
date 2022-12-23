<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebItNow.Register" %>
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
    }, 120000);

    function acceso() {
        location.href = '/Login.aspx';
    }

    function mpeMensajeOnOk() {
        //
    }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />

<div class="container well contenedorLogin">
        <div class="row">
            <div class="col-xs-12">
                <h2> Registrar</h2>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="LblUsu" runat="server" Text="Usuario" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"   
                ControlToValidate="TxtUsu" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="Usuario"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="LblEmail" runat="server" Text="Correo electrónico" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                ControlToValidate="TxtEmail" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" placeholder="Ingresa tu e-mail" MaxLength="50" TextMode="Email"></asp:TextBox>
            </div>
        </div>        

        <div class="form-group">
            <asp:Label ID="LblPass" runat="server" Text="Contraseña" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"   
                ControlToValidate="TxtPass" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtPass" runat="server" CssClass="form-control" placeholder="Crea una contraseña" MaxLength="10" TextMode="Password"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False" ></asp:Label>
            </div>
        </div>        
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Iniciar Sesión" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnEnviar" runat="server" Text="Registrar" Font-Bold="True" OnClick="BtnEnviar_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
            </div>
        </div>
    </div>

    <br />
    <asp:Panel ID="pnlExpira" runat="server" CssClass="CajaDialogo" style="display: none;">
    <table border="0" width="275px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
        <tr>
            <td align="left">
                <asp:Label ID="Label6" runat="server" Text="I t n o w" />
            </td>
            <td></td>
        </tr>
    </table>

    <div>
        <br />
        <table border="0" width="275px" style="margin: 0px; padding: 0px;" >
            <tr>
                <td><asp:Label ID="LblExpira" runat="server" Text="" /></td>
                <td></td>
            </tr>
        </table>
    </div>

    <div>
        <br />
        <table border="0" width="275px" style="margin: 0px; padding: 0px;">
            <tr>
                <td align="center"><asp:Button ID="BtnClose_Expira" OnClientClick="acceso(); return false;" runat="server" Text="Cerrar" /></td>
                <td></td>
            </tr>
        </table>
    </div>

    </asp:Panel>
    <br />
    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="display: none;">

    <table border="0" width="287px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
        <tr>
            <td align="left">
                <asp:Label ID="Label2" runat="server" Text="I t n o w" />
            </td>
            <td>
                <!--   <asp:ImageButton ID="btnCerrar" runat="server" Style="vertical-align: top;" ImageAlign="Right" /> -->
            </td>
        </tr>
    </table>

        <div>
            <!-- <asp:Image ID="imgIcono" runat="server" ImageUrl="Exclama.jpg" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" ImageAlign="Middle" /> -->
            <br />
            <table border="0" width="275px" style="margin: 0px; padding: 0px;" >
                <tr>
                    <td><asp:Label ID="LblMessage" runat="server" Text="" /></td>
                    <td></td>
                </tr>
            </table>
        </div>

        <div>
            <br />
            <table border="0" width="275px" style="margin: 0px; padding: 0px;">
                <tr>
                    <td align="center"><asp:Button ID="btnClose" OnClick="btnClose_Click" runat="server" Text="Cerrar" /></td>
                    <td></td>
                </tr>
            </table>
        </div>

    </asp:Panel>

    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td><asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" /></td>
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

</asp:Content>
