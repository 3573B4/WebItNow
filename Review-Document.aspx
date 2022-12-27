<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Review-Document.aspx.cs" Inherits="WebItNow.Review_Document" %>
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

        function getFolder() {
            return showModalDialog("folderDialog.HTA", "", "width:400px;height:400px;resizeable:yes;");
        }

        function SelCarpeta() {
            var objShell = new ActiveXObject("Shell.Application");

            var objFolder = objShell.BrowseForFolder(0, "SELECCIONE LA RUTA DONDE DESEA GUARDAR EL ARCHIVO", 0, 0);

            if (objFolder != null) {
                var objFolderItem = objFolder.Items().Item();
                var objPath = objFolderItem.Path;
                var foldername = objPath;
                document.forms.aspnetForm.ctl00_ContentPlaceHolder1_txtrutaID.value = foldername;
                return false;
            }
        }

        function fnShellBrowseForFolderJ() {
            var objShell = new ActiveXObject("shell.application");
            var ssfWINDOWS = 36;
            var objFolder;

            objFolder = objShell.BrowseForFolder(0, "Example", 0, ssfWINDOWS);
            if (objFolder != null) {
                // Add code here.
            }

            function showDirectory() {
                document.all.TxtPathDownload.value = window.showModalDialog("browseDirectory.aspx", 'jain', "dialogHeight: 560px; dialogWidth: 360px; edge: Raised; center: Yes; help: Yes; resizable: Yes; status: No;");
                return false;
            }

        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<br />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
<div class="container well contenedorLogin">
        <div class="row">
            <div class="col-xs-12">
                <h2> Validación de Documentos</h2>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="LblUsu" runat="server" Text="Usuario" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="Usuario" Enabled="False"></asp:TextBox>
            </div>
        </div>
        <br />        
        <div class="form-group">
            <asp:Label ID="LblTpoDocumento" runat="server" Text="Tpo. de Documento" CssClass="control-label col-sm-2"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtTpoDocumento" runat="server" CssClass="form-control" placeholder="Tipo de Documento" Enabled="False" ></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="form-group">
            <asp:Label ID="LblPathDownload" runat="server" Text="Ruta Descarga" CssClass="control-label col-sm-2"></asp:Label>
            <div class="col-sm-12">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <div class="d-grid gap-2 d-md-flex justify-content-center">
                        <asp:TextBox ID="TxtPathDownload" runat="server" CssClass="form-control" placeholder="Ruta descarga archivo" Enabled="False" ></asp:TextBox>
                        <asp:ImageButton ID="imgDownload" runat="server" ImageUrl="~/Images/search_find.png" OnClientClick="showDirectory();" />
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
    <!--
                <input type="button" value="Examinar" class="botones" onclick="getFolder();" align="bottom"/>
                <input type="text" name="txtruta" style="width: 289px" id="txtrutaID" /> 

                <div class="form-group">
                    <input type="file" id="fileLoader" name="files" title="Load File" />
                </div>
    -->
        </div>
        <br />
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnUnLoad" runat="server" Text="Descarga Archivo(s)" Font-Bold="True" OnClick="BtnUnLoad_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <br />
        <!--
        <div class="row">
            <div class="col-xs-12">
                <h2> Pendientes</h2>
            </div>
        </div>
        -->
        <div class="form-group">
            <asp:GridView ID="grdEstadoDocumento"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="586px"
                AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                PageSize="5" OnSelectedIndexChanged="grdEstadoDocumento_SelectedIndexChanged" Caption="Pendientes" CaptionAlign="Top" >
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEditar" runat="server" CommandName="Select" ImageUrl="~/Images/edit.jpg" Height="22px" Width="22px" />
                        </ItemTemplate> 
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdUsuario" HeaderText="Id. Usuario" />
                    <asp:BoundField DataField="IdTipoDocumento" HeaderText="Id. Tipo Documento" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Tipo de Documento" />
                    <asp:BoundField DataField="Desc_Status" HeaderText="Id. Status" />
                </Columns>
                
                <PagerStyle CssClass="pgr" />
            </asp:GridView>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False" ></asp:Label>
            </div>
        </div>        

        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
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
                <!--   <asp:ImageButton ID="BtnCerrar" runat="server" Style="vertical-align: top;" ImageAlign="Right" /> -->
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
                    <td align="center"><asp:Button ID="BtnClose" OnClick="BtnClose_Click" runat="server" Text="Cerrar" /></td>
                    <td></td>
                </tr>
            </table>
        </div>

    </asp:Panel>

    <table cellspacing="1" cellpadding="1" border="0">
        <tr>
            <td>
                <div class="form-group">
                    <div class="d-grid col-6 mx-auto">
                        <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                            TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                    </div>
                </div>
            </td>
            <td></td>
            <td></td>
        </tr>
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
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="imgDownload" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</asp:Content>
