<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Access.aspx.cs" Inherits="WebItNow.Access" %>
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
        <h2 class="h2 mb-3 fw-normal mt-4"> Iniciar Sesión</h2>

        <div class="form-group">
            <asp:Label ID="LblEmail" runat="server" Text="Correo electrónico" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                ControlToValidate="TxtEmail" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" placeholder="Ingresa tu e-mail" MaxLength="50" ></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="LblRef" runat="server" Text="Referencia" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"   
                ControlToValidate="TxtRef" ErrorMessage="*" ForeColor="Red">
                </asp:RequiredFieldValidator>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control" placeholder="Referencia" onkeyup="mayus(this);" MaxLength="12" ></asp:TextBox>
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
            <asp:Label ID="lblVerificacion" runat="server" CssClass="control-label col-sm-2" Text="Código de verificación" Font-Size="Small"></asp:Label>
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
                    <%--<asp:Button ID="BtnRegistrarse" runat="server" Font-Bold="True" Text="Registrarse" OnClick="BtnRegistrarse_Click" CssClass="btn btn-outline-primary"/>--%>
                </div>
            </div>
        </div>
<%--        <div class="from-group">
            <div class="d-grid mx-auto">
                <button onclick="location.href='Forgot-Password.aspx'" class="btn btn-link">Olvidé mi contraseña</button>
            </div>
        </div>--%>
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
