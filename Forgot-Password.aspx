<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Forgot-Password.aspx.cs" Inherits="WebItNow.Forgot_Password" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container well contenedorLogin">
        <div class="row">
            <div class="col-xs-12">
                <h2> Registrar</h2>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="LblUsu" runat="server" Text="Usuario" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"   
                ControlToValidate="TxtUsu" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="Usuario"></asp:TextBox>
            </div>
        </div>
        
        <div class="form-group">
            <asp:Label ID="LblPass" runat="server" Text="Contraseña" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"   
                ControlToValidate="TxtPass" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtPass" runat="server" CssClass="form-control" placeholder="Contraseña" MaxLength="10" TextMode="Password"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Confirmar contraseña" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                ControlToValidate="TxtConfPass" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtConfPass" runat="server" CssClass="form-control" placeholder="Confirmar contraseña" MaxLength="10" TextMode="Password"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False" ></asp:Label>
            </div>
        </div>        
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Iniciar Sesión" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnEnviar" runat="server" Text="Registrar" Font-Bold="True" OnClick="BtnEnviar_Click" CssClass="btn btn-primary" />
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

</asp:Content>
