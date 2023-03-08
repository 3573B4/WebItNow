<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Catalog_TpoDocument.aspx.cs" Inherits="WebItNow.Catalog_TpoDocument" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">

        var timer = setTimeout(function () {
            document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
        var modalId = '<%=mpeExpira.ClientID%>';
        var modal = $find(modalId);
        modal.show();

    }, 600000);

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

    <div class="container col-md-4">
        
        <h2 class="h2 my-3 fw-normal mt-4">Alta de documento</h2>

        <asp:Label ID="LblDescripcion" runat="server" CssClass="" Text="Inserte el nombre del archivo a pedir y en seguida las instrucciones de como se debe subir el archivo a pedir."></asp:Label>
        
        <div class="form-floating mt-3">
            <asp:Label ID="LblProceso" runat="server" Text="Proceso"></asp:Label>
            <asp:DropDownList ID="ddlProceso" runat="server" CssClass="btn btn-outline-secondary text-start mt-1" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" Width="100%">
           
            </asp:DropDownList>
        </div>
        <div class="form-floating my-3">
            <asp:Label ID="LblSubProceso" runat="server" Text="Sub Proceso"></asp:Label>
            <asp:DropDownList ID="ddlSubProceso" runat="server" CssClass="btn btn-outline-secondary text-start mt-1" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlSubProceso_SelectedIndexChanged" >

            </asp:DropDownList>
        </div>
        <div class="mt-3">
            <asp:Label ID="LblNameDoc" runat="server" Text="Nombre del Documento"></asp:Label>
            <asp:TextBox ID="TxtNameDoc" runat="server" CssClass="form-control mt-1" placeholder="Nombre del Archivo"></asp:TextBox>
        </div>
        <asp:TextBox type="hidden" ID="TxtIdTpoDocumento" runat="server" CssClass="form-control" ></asp:TextBox>
        <div class="justify-content-center mt-3">
            <asp:Label ID="LblInstrucciones" runat="server" Text="Instrucciones"></asp:Label>
            <textarea rows="2" cols="64" id="TxtAreaMensaje" runat="server" class="form-control mt-1"/>
        </div>

        <div class="d-grid gap-4 d-flex justify-content-center mt-3">
            <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False"  Width="280px"></asp:Label>
        </div>
        <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
            <asp:Button ID="BtnAgregar" runat="server" Text="Agregar" OnClick="BtnAgregar_Click" CssClass="btn btn-primary px-4" />
            <asp:Button ID="BtnUpdate" runat="server" Text="Actualizar" OnClick="BtnUpdate_Click" CssClass="btn btn-primary" />
            <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" CssClass="btn btn-outline-danger" />
        </div>
    </div>

    <div class="container col-md-6">
        <div style="overflow-x: auto; overflow-y:hidden">
            <asp:GridView ID="grdTpoDocumento" runat="server" Width="100%" GridLines="None" AllowPaging="true" CssClass="footable" 
                OnSelectedIndexChanged="grdTpoDocumento_SelectedIndexChanged" OnRowDataBound="grdTpoDocumento_RowDataBound" 
                OnPageIndexChanging="grdTpoDocumento_PageIndexChanging" AutoGenerateColumns="false" >
            <Columns>
                <asp:BoundField DataField="IdTpoDocumento" HeaderText="Id Documento" />
                <asp:BoundField DataField="Descripcion" HeaderText="Tipo documento" />
                <asp:BoundField DataField="DescrpBrev" HeaderText="Instrucciones" />
                <asp:BoundField DataField="IdProceso" >
                    <ItemStyle Width="0px" Font-Size="0pt" />
                </asp:BoundField>
                <asp:BoundField DataField="IdSubProceso" >
                    <ItemStyle Width="0px" Font-Size="0pt" />
                </asp:BoundField>
            </Columns>
            </asp:GridView>
        </div>

        <div class="d-grid gap-4 d-flex justify-content-center mt-1 mb-5 pb-4">
            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link" />
        </div>
    </div>
    <br />
    <div class="d-grid col-6 mx-auto">
        <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
            TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
    </div>
    <br />
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
    <br />
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
    <br />
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
