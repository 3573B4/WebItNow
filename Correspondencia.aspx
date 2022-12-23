<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"  EnableEventValidation="false"
    CodeBehind="Correspondencia.aspx.cs" Inherits="WebItNow.Correspondencia" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

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

        .mGrid {
            width: 100%;
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }

            .mGrid td {
                padding: 2px;
                border: solid 1px #c1c1c1;
                color: #717171;
            }

            .mGrid th {
                padding: 4px 2px;
                color: #fff;
                background: #424242 url(grd_head.png) repeat-x top;
                border-left: solid 1px #525252;
                font-size: 0.9em;
            }

            .mGrid .alt {
                background: #fcfcfc url(grd_alt.png) repeat-x top;
            }

            .mGrid .pgr {
                background: #424242 url(grd_pgr.png) repeat-x top;
            }

            .mGrid .pgr table {
                margin: 5px 0;
            }

            .mGrid .pgr td {
                border-width: 0;
                padding: 0 6px;
                border-left: solid 1px #666;
                font-weight: bold;
                color: #fff;
                line-height: 12px;
            }

            .mGrid .pgr a {
                color: #666;
                text-decoration: none;
            }

            .mGrid .pgr a:hover {
                color: #000;
                text-decoration: none;
            }

        .scrolling-table-container {
            height: 262px;
            overflow-y: scroll;
            overflow-x: hidden;
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
        Txt_Rfc.Focus();
    }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td colspan="3" rowspan="1"><asp:Label ID="Label6" runat="server" width="1000px" CssClass="txtAlign" 
                Text="Dirección General de Recursos Humanos y Organización" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" rowspan="1"><asp:Label ID="Label7" runat="server" width="1000px" CssClass="txtAlign" 
                Text="Dirección de Pagos" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" rowspan="1"><asp:Label ID="Label8" runat="server" width="1000px" CssClass="txtAlign" 
                Text="Correspondencia de Fonac" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
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
            <td><asp:Button ID="hBtnRfc" runat="server" style="display:none;" /></td>
            <td><asp:Label ID="LblRFC" runat="server"
                Text="Rfc:" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td><asp:TextBox ID="Txt_Rfc" runat="server" Style="text-transform: uppercase" AutoComplete="off" MaxLength="13"></asp:TextBox>
                <asp:ImageButton ID="BuscadorRfc" runat="server" BorderStyle="None" ImageUrl="~/Images/search_find.png" 
                              Width="16px" OnClick="BuscadorRfc_Click" />
            </td>
            <td><asp:TextBox ID="Txt_Nombre" runat="server" Width="320px" Enabled="False" AutoComplete="off"></asp:TextBox></td>
            <td><asp:Label ID="LblEstatus" runat="server"
                Text="Estatus:" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Estatus" runat="server" Enabled="False" AutoComplete="off"></asp:TextBox></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td><asp:Label ID="LblCiclo" runat="server"
                Text="Ciclo:" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td>
                <asp:DropDownList ID="Cbo_Ciclo" runat="server" Width="128px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="LblPeriodo" runat="server"
                Text="Periodo:" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td>
                <asp:DropDownList ID="Cbo_Periodo" runat="server" Width="128px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="LblMotivo" runat="server"
                Text="Motivo:" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label></td>
            <td>
                <asp:DropDownList ID="Cbo_Motivos" runat="server" Width="224px">
                </asp:DropDownList>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td colspan="3" align="center" rowspan="1"><asp:Label ID="lblSeguimiento" runat="server"
                Text="Seguimiento" Font-Bold="True" Font-Names="Bookman Old Style"></asp:Label>
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
            <td><asp:Label ID="LblOficio" runat="server" Text="Oficio"></asp:Label></td>
            <td><asp:Label ID="LblFolio" runat="server" Text="Folio"></asp:Label></td>
            <td><asp:Label ID="LblObservaciones" runat="server" Text="Observaciones"></asp:Label></td>
            <td></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label1" runat="server" Text="1:"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Oficio_1" runat="server" AutoComplete="off" Width="320px"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Folio_1" runat="server" AutoComplete="off" Width="64px" TextMode="Number" ></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Observaciones_1" runat="server" AutoComplete="off" Width="418px"></asp:TextBox></td>
            <td></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label2" runat="server" Text="2:"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Oficio_2" runat="server" AutoComplete="off" Width="320px"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Folio_2" runat="server" AutoComplete="off" Width="64px" TextMode="Number" ></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Observaciones_2" runat="server" AutoComplete="off" Width="418px"></asp:TextBox></td>
            <td></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label3" runat="server" Text="3:"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Oficio_3" runat="server" AutoComplete="off" Width="320px"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Folio_3" runat="server" AutoComplete="off" Width="64px" TextMode="Number" ></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Observaciones_3" runat="server" AutoComplete="off" Width="418px"></asp:TextBox></td>
            <td></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label4" runat="server" Text="4:"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Oficio_4" runat="server" AutoComplete="off" Width="320px"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Folio_4" runat="server" AutoComplete="off" Width="64px" TextMode="Number" ></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Observaciones_4" AutoComplete="off" runat="server" Width="418px"></asp:TextBox></td>
            <td></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label5" runat="server" Text="5:"></asp:Label></td>
            <td><asp:TextBox ID="Txt_Oficio_5" runat="server" AutoComplete="off" Width="320px"></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Folio_5" runat="server" AutoComplete="off" Width="64px" TextMode="Number" ></asp:TextBox></td>
            <td><asp:TextBox ID="Txt_Observaciones_5" runat="server" AutoComplete="off" Width="418px"></asp:TextBox></td>
            <td></td>
        </tr>
        </table>
        <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>
                <asp:TextBox ID="Txt_Mensaje" runat="server" Width="418px"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        </table>
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
                <asp:Button ID="BtnActivar" runat="server" Text="Activar" Font-Bold="True" Height="52px" OnClick="BtnActivar_Click" Width="160px" Visible="False" /><asp:Button ID="BtnLimpiar" runat="server" Text="Limpiar" Font-Bold="True" Height="52px" OnClick="BtnLimpiar_Click" Width="160px" /></td>
            <td>
                <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" Font-Bold="True" Height="52px" OnClick="BtnBuscar_Click" Width="160px" /><asp:Button ID="BtnRegistrar" runat="server" Text="Registrar" Font-Bold="True" Height="52px" OnClick="BtnRegistrar_Click" Width="160px" /></td>
            <td>
                <asp:Button ID="BtnActualizar" runat="server" Text="Actualizar" Font-Bold="True" Height="52px" OnClick="BtnActualizar_Click" Width="160px" /><asp:Button ID="BtnSalir" runat="server" Text="Salir" Font-Bold="True" Height="52px" OnClick="BtnSalir_Click" Width="160px" />
            </td>
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
    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td></td>
            <td>
            <div style="text-align:left;">
                <asp:Panel ID="pnlRfc" runat="server" Width="600px" BorderStyle="Solid" BorderWidth="2px" BackColor="White">
                    <table cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td></td>
                        <td>
                            <asp:TextBox ID="Txt_BuscadorRfc" runat="server" AutoComplete="off" Style="text-transform: uppercase" MaxLength="13" Width="552px"></asp:TextBox>
                            <asp:Button ID="BtnBuscador" runat="server" Text="..." Height="22px" OnClick="BtnBuscador_Click" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                        <div class="scrolling-table-container">
                            <asp:GridView ID="GrdRfc" runat="server"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" GridLines="None"
                                AutoGenerateColumns="False" AutoGenerateSelectButton="false" Width="586px" 
                                 OnSelectedIndexChanged="GrdRfc_SelectedIndexChanged" OnRowDataBound="GrdRfc_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="IdUsuario" HeaderText="IdUsuario" />
                                <asp:BoundField DataField="UsPrivilegios" HeaderText="UsPrivilegios" />
                            </Columns>
                            </asp:GridView>
                        </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="center"><asp:Button ID="btnCloseRfc" runat="server" Text="Close" /></td>
                        <td></td>
                    </tr>
                    </table>
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="mpePopup" runat="server" TargetControlId="hBtnRfc" 
                                        PopupControlID="pnlRfc" CancelControlID="btnCloseRfc" />
            </div>
            </td>
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
                    <td align="center"><asp:Button ID="btnClose" OnClientClick="acceso(); return false;" runat="server" Text="Cerrar" /></td>
                    <td></td>
                </tr>
            </table>
        </div>

    </asp:Panel>
    <asp:Panel ID="pnlExpira" runat="server" CssClass="CajaDialogo" style="display: none;">
    <table border="0" width="275px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
        <tr>
            <td align="center">
                <asp:Label ID="Label10" runat="server" Text="F O N A C" />
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
