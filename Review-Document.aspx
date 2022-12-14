<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Review-Document.aspx.cs" Inherits="WebItNow.Fotos" %>
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
            Selecciona la galería que deseas ver:<br />
            <asp:DropDownList ID="cboGallery" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboGallery_SelectedIndexChanged" Width="250px">
            </asp:DropDownList>
            <br />
            <br />
            <asp:DataList ID="dlsGallery" runat="server" RepeatColumns="6" OnSelectedIndexChanged="dlsGallery_SelectedIndexChanged">
            <ItemTemplate>
                <table>
                    <tr>
                        <td style="border-style:solid; border-width:1px; width:150px; height:150px; text-align:center;">
                        <a href="Directorio/USUARIO3/<%#Eval("IdUsuario")%>/<%#Eval("FileName")%>" target="_blank">
                       
                             
                        <asp:Image ID="imgGallery" runat="server" Height="200px" Width="200px"
                             ImageUrl='<%#fnFilePath(DataBinder.Eval(Container.DataItem,"IdUsuario").ToString(),DataBinder.Eval(Container.DataItem,"FileName").ToString()) %>' />
                         </a>
                        <p style="text-align:center;">
                            <a href="Directorio/USUARIO3/<%#Eval("IdUsuario")%>/<%#Eval("FileName")%>" target="_blank">
                        <!--<asp:Label ID="lblFileDesc" runat="server" Text='<%#Eval("FileDescription")%>'></asp:Label>-->
                            </a>
                        </p>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            </asp:DataList>
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
 </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
