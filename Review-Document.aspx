<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" EnableEventValidation = "false" CodeBehind="Review-Document.aspx.cs" Inherits="WebItNow.Fotos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<br />
<asp:UpdatePanel ID="uplMain" runat="server">
<ContentTemplate>
    <table>
    <tr>
        <td>
            <asp:Label ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="BuscadorUser" runat="server" Text="..." OnClick="BtnBuscadorUser_Click"/>
            <asp:Button ID="hBtnUser" runat="server" style="display:none;" Text="..." />
        </td>
    <tr>
        <td>
            <asp:Label ID="lblTipoDocumento" runat="server" Text="Id. Tipo Documento"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtTpoDocumento" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblEstatus" runat="server" Text="Id. Estatus"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtEstatus" runat="server"></asp:TextBox>
        </td>
    </tr>
    </table>
    <br />
    <table>
    <tr>
        <td>
            Seleccione el Tipo de Documento :<br />
            <asp:DropDownList ID="cboTpoDocumento" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboTpoDocumento_SelectedIndexChanged" Width="250px" Visible="False">
            </asp:DropDownList>
            <br />
            <asp:GridView ID="grdEstadoDocumento" runat="server" AutoGenerateColumns="False" GridLines="None" Width="586px"
                AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                PageSize="7" OnSelectedIndexChanged="grdEstadoDocumento_SelectedIndexChanged">
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:ButtonField ButtonType="Link" CommandName="Select" Text="Select" />
                    <asp:BoundField DataField="IdUsuario" HeaderText="Id. Usuario" />
                    <asp:BoundField DataField="IdTipoDocumento" HeaderText="Id. Tipo Documento" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Tipo de Documento" />
                    <asp:BoundField DataField="Desc_Status" HeaderText="Id. Status" />
                </Columns>
                <PagerStyle CssClass="pgr" />
            </asp:GridView>
            <br />
            <div class="container well contenedorLogin">
            <asp:DataList ID="dblstTpoDocumento" runat="server" RepeatColumns="6" OnSelectedIndexChanged="dblstTpoDocumento_SelectedIndexChanged">
            <ItemTemplate>
                <table>
                    <tr>
                        <td style="border-style:solid; border-width:1px; width:150px; height:150px; text-align:center;">
                        <a href="Directorio/USUARIO3/<%#Eval("IdUsuario")%>/<%#Eval("FileName")%>" target="_blank">
                             
                        <asp:Image ID="imgGallery" runat="server" Height="200px" Width="200px"
                             ImageUrl='<%#fnFilePath(DataBinder.Eval(Container.DataItem,"IdUsuario").ToString(), DataBinder.Eval(Container.DataItem,"FileName").ToString()) %>' />
                         </a>
                        <p style="text-align:center;">
                            <a href="Directorio/USUARIO3/<%#Eval("IdUsuario")%>/<%#Eval("FileName")%>" target="_blank">
                   <!-- <asp:Label ID="lblFileDesc" runat="server" Text='<%#Eval("FileDescription")%>'></asp:Label> -->
                            </a>
                        </p>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            </asp:DataList>
            </div>
            <br />
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    </table>

    <div class="container well contenedorLogin">
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-outline-primary"/>
            </div>
        </div>
    </div>

    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td></td>
            <td></td>
            <td>
            <div style="text-align:left;">
                <asp:Panel ID="pnlUsuarios" runat="server" Width="600px" BorderStyle="Solid" BorderWidth="2px" BackColor="White">
                    <table cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td></td>
                        <td>
                            <asp:TextBox ID="Txt_BuscadorUser" runat="server" AutoComplete="off" Style="text-transform: uppercase" MaxLength="13" Width="552px"></asp:TextBox>
                            <asp:Button ID="BtnBuscador" runat="server" Text="..." Height="22px" OnClick="BtnBuscador_Click" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                        <div class="scrolling-table-container">
                            <asp:GridView ID="GrdUsuarios" runat="server"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" GridLines="None"
                                AutoGenerateColumns="False" AutoGenerateSelectButton="false" Width="586px" 
                                 OnSelectedIndexChanged="GrdUsuarios_SelectedIndexChanged" OnRowDataBound="GrdUsuarios_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="IdUsuario" HeaderText="Usuario" />
                                <asp:BoundField DataField="UsPrivilegios" HeaderText="Privilegios" />
                            </Columns>
                            </asp:GridView>
                        </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="center"><asp:Button ID="btnCloseUser" runat="server" Text="Close" /></td>
                        <td></td>
                    </tr>
                    </table>
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="mpePopup" runat="server" TargetControlId="hBtnUser"
                                                PopupControlID="pnlUsuarios" CancelControlID="btnCloseUser" />
            </div>
            </td>
            <td></td>
        </tr>
    </table>
    <br />
 </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
