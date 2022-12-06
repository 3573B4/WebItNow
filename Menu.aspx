<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="WebItNow.Menu" %>
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

        filter: alpha(opacity=70);

        opacity: 0.7;

    }
			
		* {
			margin:0px;
			padding:0px;
		}
			
		#header {
			margin:auto;
			width:100%;
			background:black;
			position:fixed;
			font-family:Arial, Helvetica, sans-serif;
		}
			
		ul, ol {
			list-style:none;
		}
			
		.nav {
			width:750px;   /*Le establecemos un ancho*/
			margin:0 auto; /*Centramos automaticamente*/
		}

		.nav > li {
			float:left;
		}
			
		.nav li a {
			background-color:#000;
			color:#fff;
			text-decoration:none;
			padding:10px 12px;
			display:block;
		}
			
		.nav li a:hover {
			background-color:#434343;
		}
			
		.nav li ul {
			display:none;
			position:absolute;
			min-width:140px;
		}
			
		.nav li:hover > ul {
			display:block;
		}
			
		.nav li ul li {
			position:relative;
		}
			
		.nav li ul li ul {
			right:-140px;
			top:0px;
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
        //
    }

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <table cellspacing="1" cellpadding="1" border="0" style="background-color:Black" width="100%">
        <tr>
            <td style="width:20%"></td>
            <td style="width:70%"> 
                <asp:Menu ID="Menu1" runat="server" Height="25px" Orientation="Horizontal" Width="100%"
                    MaximumDynamicDisplayLevels ="2" BackColor="Black"
                    DynamicHorizontalOffset="2" Font-Names="Arial,Helvetica,sans-serif" Font-Size="Medium"
                    ForeColor="White" StaticSubMenuIndent="10px" OnMenuItemClick="MyMenu_MenuItemClick" Font-Bold="False">
                    <DynamicHoverStyle BackColor="Gray" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="30px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="Black" />
                    <DynamicSelectedStyle BackColor="Silver" />
                    <StaticHoverStyle BackColor="Gray" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="30px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="Silver" />
                    <Items>
<%--                    
                        <asp:MenuItem Text="Configuracion">
                            <asp:MenuItem Text="Usuarios" NavigateUrl="~/Usuarios.aspx"></asp:MenuItem>
                            <asp:MenuItem Text="Bitacora" NavigateUrl="~/Bitacora.aspx"></asp:MenuItem>
                        </asp:MenuItem>
--%>
<%--
                        <asp:MenuItem Text="Itnow">
                            <asp:MenuItem Text="Correspondencia" NavigateUrl="~/Correspondencia.aspx"></asp:MenuItem>
                            <asp:MenuItem Text="Consulta" NavigateUrl="~/Consulta.aspx"></asp:MenuItem>
                            <asp:MenuItem Text="Totales para desincorporados" NavigateUrl="~/Carga_Nomina.aspx"></asp:MenuItem>
                        </asp:MenuItem>
--%>
                        <asp:MenuItem Text="Salir" NavigateUrl="~/Acceso.aspx">
                        </asp:MenuItem>
                    </Items>
                </asp:Menu>
            </td>
            <td style="width:10%"></td>
        </tr>
    </table>
	<br />
	<br />
	<br />
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="" Width="1000px" 
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
                <asp:Label ID="Label5" runat="server" Text="" Width="1000px" 
                    CssClass="txtAlign" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
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
    <br />

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
            <td></td>
            <td>
                <%-- <asp:Button ID="BtnCorrespondencia" runat="server" Text="Correspondencia" Font-Bold="True" Height="52px" OnClick="BtnCorrespondencia_Click" Width="160px" /><asp:Button ID="BtnConsulta" runat="server" Text="Consulta" Font-Bold="True" Height="52px" OnClick="BtnConsulta_Click" Width="160px" /> --%>
            </td>
            <td>
                <%-- <asp:Button ID="BtnCarga_Nomina" runat="server" Text="Totales p/Desincorp." Font-Bold="True" Height="52px" OnClick="BtnCarga_Nomina_Click" Width="160px" /><asp:Button ID="BtnSalir" runat="server" Text="Salir" Font-Bold="True" Height="52px" OnClick="BtnSalir_Click" Width="160px" />  --%>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td><asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" /></td>
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
                <asp:Label ID="Label6" runat="server" Text="F O N A C" />
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
</asp:Content>
