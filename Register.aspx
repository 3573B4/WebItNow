<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebItNow.Register" %>
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
        <h2 class="h2 mb-3 fw-normal">Registrar</h2>
        <div class="form-group">
            <asp:Label ID="LblUsu" runat="server" Text="Usuario" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"   
                ControlToValidate="TxtUsu" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="Usuario" onkeyup="mayus(this);" OnTextChanged="TxtUsu_TextChanged" AutoPostBack="true" MaxLength="16" ></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="LblEmail" runat="server" Text="Correo electrónico" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                ControlToValidate="TxtEmail" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator id="regEmail" ControlToValidate="TxtEmail" Text="" Runat="server" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" /> 
            <div class="col-sm-12">
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" placeholder="Ingresa tu e-mail" MaxLength="50" OnTextChanged="TxtEmail_TextChanged" AutoPostBack="true" TextMode="Email"></asp:TextBox>
            </div>
        </div>        

        <div class="form-group">
            <asp:Label ID="LblPass" runat="server" Text="Contraseña" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"   
                ControlToValidate="TxtPass" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtPass" runat="server" CssClass="form-control" placeholder="Crea una contraseña" onkeyup="mayus(this);" MaxLength="16" TextMode="Password"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False"  Width="280px" ></asp:Label>
            </div>
        </div>        
        
        <div class="form-group mt-3">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnEnviar" runat="server" Text="Registrar" Font-Bold="True" OnClick="BtnEnviar_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <div class="from-group mt-2">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Iniciar Sesión" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
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
