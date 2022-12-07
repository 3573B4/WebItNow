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
                <asp:Label ID="Label1" runat="server" Text="" 
                    Width="1000px" CssClass="txtAlign" Font-Bold="True" 
                    Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label2" runat="server" 
                    Text="" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label3" runat="server" 
                    Text="" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="" 
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
                    Text="Ingresa tus datos" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
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
            <td><!--
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"   
                ControlToValidate="TxtUsu" ErrorMessage="* Este campo es obligatorio" ForeColor="Red">
                </asp:RequiredFieldValidator> -->
            </td>
            <td></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="lblCve" runat="server" Text="Clave" Width="148px" CssClass="txtAlign"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtPass" runat="server" TextMode="Password" AutoComplete="off"
                    ToolTip="TECLEA TU CLAVE EN MAYÚSCULAS" Width="148px"></asp:TextBox>
            </td>
            <td><!--
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                ControlToValidate="TxtPass" ErrorMessage="* Este campo es obligatorio" ForeColor="Red">
                </asp:RequiredFieldValidator> -->
            </td>
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
            <td></td>
            <td></td>
            <td>
                <button onclick="location.href='Forgot-Password.aspx'">Olvidé mi contraseña</button>
                <!-- 
                <p>
                    <a href="Forgot-Password.aspx">Olvidé mi contraseña</a>.
                </p>
                <asp:Label ID="lblCotraseña" runat="server" CssClass="txtAlign" Text="¿Primer acceso o te olvidaste tu contraseña?"></asp:Label>-->
            </td>
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
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="BtnAceptar" runat="server" Text="Iniciar sesión"  Font-Bold="True" OnClick="BtnAceptar_Click" Width="160px" Height="52px"/>
                </td>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Button ID="BtnRegistrarse" runat="server" Font-Bold="True" Height="52px" OnClick="BtnRegistrarse_Click" Text="Registrarse" Width="160px" />
            </td>
            <td><asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" /></td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td> <!--
                <asp:Label ID="Label7" runat="server" Text="© 2022 Todos los derechos reservados" 
                    Width="1000px" CssClass="txtAlign" Font-Bold="True" 
                    Font-Names="Bookman Old Style"></asp:Label> -->
            </td>
            <td></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="display: none;">

    <table border="0" width="275px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
        <tr>
            <td align="left">
                <asp:Label ID="Label6" runat="server" Text="I t n o w" />
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
