<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Acceso.aspx.cs" Inherits="WebItNow.Acceso" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .txtAlign { Text-Align: right }

        .CajaDialogo

        {

            background-color: lightcyan;

            border-width: 4px;

            border-style: outset;

            border-color: black;

            padding: 0px;

            width: 275px;

            font-weight: bold;

            font-style: italic;

        }

        .CajaDialogo div

        {

            margin: 7px;

            text-align: center;

        }

        .FondoAplicacion

        {

            background-color: Gray;

            /* filter: alpha(opacity=70); */

            opacity: 0.7;

        }
    </style>

    <script language="javascript" type="text/javascript">

    //var timer = setTimeout(function () {
    //    alert("La sesión ha expirado.");
    //    location.href = '/Acceso.aspx';
    //}, 120000);

    function mpeMensajeOnOk()

    {
        //var txtNombre = document.getElementById("txtNombre");
        //var txtClave = document.getElementById("txtClave");

        //txtNombre.value = "";
        //txtClave.value = "";

        //txtNombre.focus();
    }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Fiscalía General de la República" 
                    Width="1000px" CssClass="txtAlign" Font-Bold="True" 
                    Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label2" runat="server" 
                    Text="Dirección General de Recursos Humanos" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label3" runat="server" 
                    Text="Dirección General Adjunta de Administración" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Dirección de Pagos" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Subdirección de Nómina" 
                    Width="1000px" CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        </table>
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td class="style4" colspan="3" align="center" rowspan="1"><asp:Label ID="lblEstatus" runat="server"
                    Text="Sistema Integral de Información del FONAC (SIIF)" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="lblUsu" runat="server" Text="Usuario" Width="148px" CssClass="txtAlign"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtUsu" runat="server" AutoComplete="off"
                    ToolTip="TECLEA TU USUARIO EN MAYÚSCULAS" Width="148px"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Label ID="lblCve" runat="server" Text="Clave" Width="148px" CssClass="txtAlign"></asp:Label></td>
            <td><asp:TextBox ID="TxtCve" runat="server" TextMode="Password" AutoComplete="off"
                    ToolTip="TECLEA TU CLAVE EN MAYÚSCULAS" Width="148px"></asp:TextBox></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td><asp:Image ID="ImgCaptcha" runat="server" Height="55px" ImageUrl="~/Captcha.aspx" Width="186px" />  
                <br />  
                <asp:Label runat="server" ID="lblCaptchaMessage"></asp:Label></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="lblVerificacion" runat="server" CssClass="txtAlign" Text="Código de verificación" Width="148px"></asp:Label>
            </td>
            <td><asp:TextBox runat="server" ID="txtVerificationCode" Width="148px" AutoComplete="off" ></asp:TextBox></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="BtnAceptar" runat="server" Text="Aceptar" OnClick="BtnAceptar_Click" Width="87px" />
                </td>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td><asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" /></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="display: none;">

        <table border="0" width="275px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
            <tr>
                <td align="center">
                    <asp:Label ID="Label6" runat="server" Text="F O N A C" />
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
                    <td align="center"><asp:Button ID="btnClose" runat="server" Text="Cerrar" /></td>
                    <td></td>
                </tr>
            </table>
        </div>

    </asp:Panel>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
