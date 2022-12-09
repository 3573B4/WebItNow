<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebItNow.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container well contenedorLogin">
        <div class="row">
            <div class="col-xs-12">
                <h2> Iniciar Sesión</h2>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="LblUsu" runat="server" Text="Usuario" CssClass="control-label co-sm-3"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="Usuario"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Contraseña" CssClass="control-label col-sm-2"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="Contraseña"></asp:TextBox>
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
            <div class="col-sm-12">
                <asp:TextBox runat="server" ID="txtVerificationCode" placeholder="Código de verificación" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                
                    <button <%--onclick="location.href='Forgot-Password.aspx'" --%> class="btn btn-link">Olvidé mi contraseña</button>
                
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid gap-2 d-md-flex justify-content-center">
                <asp:Button ID="BtnAceptar" runat="server" Text="Iniciar sesión"  Font-Bold="True"  CssClass="btn btn-primary me-md-2"/>
                <asp:Button ID="BtnRegistrarse" runat="server" Font-Bold="True" Text="Registrarse" CssClass="btn btn-outline-primary"/>
            </div>
        </div>

    </div>
</asp:Content>
