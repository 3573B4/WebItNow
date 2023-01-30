<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebItNow.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <br />
    <div class="container col-md-4">
        <h2 class="h2 mb-3 fw-normal"> Iniciar Sesión</h2>
        <div class="form-group">
            <asp:Label ID="LblUsu" runat="server" Text="Usuario" CssClass="control-label co-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"   
                ControlToValidate="TxtUsu" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="Usuario" onkeyup="mayus(this);" MaxLength="16" ></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="LblPass" runat="server" Text="Contraseña" CssClass="control-label col-sm-2"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                ControlToValidate="TxtPass" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtPass" runat="server" CssClass="form-control" placeholder="Contraseña" onkeyup="mayus(this);" MaxLength="16" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="from-group">
                <div class="d-grid col-4 mx-auto">
                    <br />
                    <asp:Image ID="ImgCaptcha" runat="server" Height="55px" ImageUrl="~/Captcha.aspx" Width="186px" />
                    <br />
                    <asp:Label runat="server" ID="lblCaptchaMessage"></asp:Label>
                </div>
        </div>
        <div class="from-group">
            <asp:Label ID="lblVerificacion" runat="server" CssClass="control-label col-sm-2" Text="Código de verificación" ></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"   
                ControlToValidate="txtVerificationCode" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox runat="server" ID="txtVerificationCode" placeholder="Código de verificación" onkeyup="mayus(this);" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False"  Width="280px" ></asp:Label>
            </div>
        </div>
        <div class="form-group mt-3">
            <div class="col-12">                
                <div class="d-grid gap-2 d-flex justify-content-center">
                    <asp:Button ID="BtnAceptar" runat="server" Text="Iniciar sesión"  Font-Bold="True"  CssClass="btn btn-primary me-md-2" OnClick="BtnAceptar_Click"/>
                    <asp:Button ID="BtnRegistrarse" runat="server" Font-Bold="True" Text="Registrarse" OnClick="BtnRegistrarse_Click" CssClass="btn btn-outline-primary"/>
                </div>
            </div>
        </div>
        <div class="from-group">
            <div class="d-grid mx-auto">
                <button onclick="location.href='Forgot-Password.aspx'" class="btn btn-link">Olvidé mi contraseña</button>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid gap-2 d-md-flex justify-content-center">
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
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
