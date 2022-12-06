<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Carga_Nomina.aspx.cs" Inherits="WebFONAC.Carga_Nomina" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web"%>

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

            filter: alpha(opacity=70);

            opacity: 0.7;

        }


        .auto-style1 {
            height: 34px;
        }
    </style>
    
    <script language="javascript" type="text/javascript">

    var timer = setTimeout(function () {
        document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
        var modalId = '<%=mpeExpira.ClientID%>';
        var modal = $find(modalId);
            modal.show();
        //alert("La sesión ha expirado.");
        //location.href = '/Acceso.aspx';
    }, 120000);

    function acceso() {
        location.href = '/Acceso.aspx';
    }

    function mpeMensajeOnOk()

    {
        Txt_Casos_B1.Focus();
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

    <br />
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td colspan="3" rowspan="1"><asp:Label ID="Label6" runat="server" Width="1000px" CssClass="txtAlign"
                Text="Dirección General de Recursos Humanos y Organización" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" rowspan="1"><asp:Label ID="Label7" runat="server" Width="1000px" CssClass="txtAlign"
                Text="Dirección de Pagos" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" rowspan="1"><asp:Label ID="Label8" runat="server" width="1000px" CssClass="txtAlign" 
                Text="Totales de FONAC por Movimiento de Nómina" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <br />
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td class="auto-style1"></td>
            <td colspan="2" align="center" rowspan="1" class="auto-style1"><asp:Label ID="lblBase" runat="server"
                Text="Base" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td class="auto-style1">&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td colspan="2" align="center" rowspan="1" class="auto-style1"><asp:Label ID="lblConfianza" runat="server"
                Text="Confianza" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td class="auto-style1"></td>
            <td class="auto-style1"></td>
        </tr>
        <tr>
            <td class="auto-style1"></td>
            <td align="center" class="auto-style1"><asp:Label ID="lblCasos" runat="server"
                Text="Casos" Font-Names="Bookman Old Style" Font-Bold="True"></asp:Label></td>
            <td align="center" class="auto-style1"><asp:Label ID="lblAportaciones" runat="server"
                Text="Aportaciones" Font-Names="Bookman Old Style" Font-Bold="True"></asp:Label></td>
            <td class="auto-style1">&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td align="center" class="auto-style1"><asp:Label ID="lblCasos_2" runat="server"
                Text="Casos" Font-Names="Bookman Old Style" Font-Bold="True"></asp:Label></td>
            <td align="center" class="auto-style1"><asp:Label ID="lblAportaciones_2" runat="server"
                Text="Aportaciones" Font-Names="Bookman Old Style" Font-Bold="True"></asp:Label></td>
            <td class="auto-style1"></td>
            <td class="auto-style1"></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:TextBox ID="Txt_Casos_B1" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_B1" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server"
                Text="Primer Archivo de Nómina" Font-Bold="True" Font-Names="Bookman Old Style" Width="286px"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Casos_C1" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_C1" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:TextBox ID="Txt_Casos_B2" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_B2" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label2" runat="server"
                Text="Segundo Archivo de Nómina" Font-Bold="True" Font-Names="Bookman Old Style" Width="286px"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Casos_C2" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_C2" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:TextBox ID="Txt_Casos_B3" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_B3" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label3" runat="server"
                Text="Tercer Archivo de Nómina" Font-Bold="True" Font-Names="Bookman Old Style" Width="286px"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Casos_C3" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_C3" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:TextBox ID="Txt_Casos_B4" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_B4" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label4" runat="server"
                Text="Definitivo" Font-Bold="True" Font-Names="Bookman Old Style" Width="286px"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Casos_C4" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Aportaciones_C4" runat="server" AutoComplete="off" Enabled="False"></asp:TextBox></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <br />
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:FileUpload ID="FileUpload1" runat="server" AutoComplete="off"  />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnPrimerCarga" />
                        <asp:PostBackTrigger ControlID="BtnSegundaCarga" />
                        <asp:PostBackTrigger ControlID="BtnTercerCarga" />
                        <asp:PostBackTrigger ControlID="BtnDefinitivo" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
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
    </table>
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="BtnPrimerCarga" runat="server" Text="Primer Carga" Font-Bold="True" Height="52px" OnClick="BtnPrimerCarga_Click" Width="160px" />
                <asp:Button ID="BtnSegundaCarga" runat="server" Text="Segunda Carga" Font-Bold="True" Height="52px" OnClick="BtnSegundaCarga_Click" Width="160px" />
            </td>
            <td>
                <asp:Button ID="BtnTercerCarga" runat="server" Text="Tercer Carga" Font-Bold="True" Height="52px" OnClick="BtnTercerCarga_Click" Width="160px" />
                <asp:Button ID="BtnDefinitivo" runat="server" Text="Definitivo" Font-Bold="True" Height="52px" OnClick="BtnDefinitivo_Click" Width="160px" />
            </td>
            <td>
                <asp:Button ID="BtnLimpiar" runat="server" Text="Limpiar" Font-Bold="True" Height="52px" OnClick="BtnLimpiar_Click" Width="160px" />
                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Font-Bold="True" Height="52px" OnClick="BtnSalir_Click" Width="160px" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" align="center" rowspan="1"><asp:Label ID="Lbl_Mensaje" runat="server"></asp:Label></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td><asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" /></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                    TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td class="style3"><asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" /></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="display: none;">

    <table border="0" width="275px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
        <tr>
            <td align="center">
                <asp:Label ID="Label9" runat="server" Text="F O N A C" />
            </td>
            <td>
                <!--<asp:ImageButton ID="btnCerrar" runat="server" Style="vertical-align: top;" ImageAlign="Right" /> -->
            </td>
        </tr>
    </table>

        <div>
            <!--<asp:Image ID="imgIcono" runat="server" ImageUrl="Exclama.jpg" BorderColor="Black"
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
    <asp:Panel ID="pnlExpira" runat="server" CssClass="CajaDialogo" style="display: none;">
    <table border="0" width="275px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
        <tr>
            <td align="center">
                <asp:Label ID="Label32" runat="server" Text="F O N A C" />
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
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
