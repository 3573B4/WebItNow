<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Request_Document.aspx.cs" Inherits="WebItNow.Request_Document" %>
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
    <br />

<div class="container col-md-4">
        <h2 class="h2 mb-3 fw-normal">Solicitud de Documento</h2>
        <div class="form-group">
            <asp:Label ID="LblNom" runat="server" Text="Nombre" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"   
                ControlToValidate="TxtNom" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtNom" runat="server" CssClass="form-control" placeholder="Nombre" onkeyup="mayus(this);" ></asp:TextBox>
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
            <asp:Label ID="LblReferencia" runat="server" Text="Referencia" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"   
                ControlToValidate="TxtReferencia" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control" placeholder="Referencia" onkeyup="mayus(this);" MaxLength="10" ></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="LblTpoDocumento" runat="server" Text="Tpo. Documento" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"   
                ControlToValidate="TxtTpoDocumento" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtTpoDocumento" runat="server" CssClass="form-control" placeholder="Tipo de Documento" onkeyup="mayus(this);" MaxLength="10" ></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False"  Width="280px" ></asp:Label>
            </div>
        </div>        
        
        <div class="form-group mt-3">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnEnviar" runat="server" Text="Solicitud" Font-Bold="True" OnClick="BtnEnviar_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
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
            <asp:Button ID="btnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
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
