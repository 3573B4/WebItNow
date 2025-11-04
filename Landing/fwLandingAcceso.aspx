<%@ Page Title="Portal de Siniestros" Language="C#" MasterPageFile="~/Landing/Landing.Master" AutoEventWireup="true" CodeBehind="fwLandingAcceso.aspx.cs" Inherits="WebItNow_Peacock.Landing.fwLandingAcceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        body, html {
            height: 100%;
            margin: 0;
        }

        #contenedor {
            position: relative;
            width: 100%;
            height: 100vh;;
            display: flex;
            justify-content: center;
            align-items: center;
            background-size: cover;
            background-image: url('/Images/fondo_inicio.jpeg');
        }

        #panelSuperior {
            position: absolute;
            top: 25%;           	
            width: 90%;         	
            max-width: 500px;   	
            border-radius:4%;
            background-color: white;
            padding: 20px;
        }

        @media (max-width: 768px) {
            #panelSuperior {
                width: 90%;
                top: 5%; 	
            }
        }
    </style>

    <div class="landing-container">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="contenedor">
                    <div id="panelSuperior" class="container col-md-4">
                        <h2 class="h2 mb-4 fw-normal mt-3">Portal de Siniestros</h2>

                        <!-- Panel de Login -->
                        <asp:Panel ID="PnlLogin" runat="server" Visible="true">
                            <div class="form-group">
                                <asp:Label ID="LblPassword" runat="server" Text="Contraseña temporal:" CssClass="control-label col-sm-2 mb-3" Font-Size="Small"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                                    ControlToValidate="TxtPassword" ErrorMessage="*" ForeColor="Red">
                                    </asp:RequiredFieldValidator>
                                <div class="col-sm-12">
                                    <asp:Panel ID="pnlPass" runat="server" DefaultButton="BtnLogin">
                                        <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control mt-2 mb-3" placeholder="Contraseña" MaxLength="20" TextMode="Password"></asp:TextBox>
                                    </asp:Panel>
                                </div>
                            </div>

                            <!-- Mensaje de alerta -->
                            <asp:Label ID="LblMensaje" runat="server" CssClass="mensaje" />

                            <div class="form-group mt-4">
                                <div class="col-12">                
                                    <div class="d-grid gap-2 d-flex justify-content-center">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnLogin" runat="server" Text="Iniciar sesión" Font-Bold="True" CssClass="btn btn-success me-md-2" OnClick="BtnLogin_Click"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>

                            <%--<div class="form-group">--%>
                                <%--<asp:Label ID="LblPassword" runat="server" Text="Contraseña temporal:" AssociatedControlID="TxtPassword"></asp:Label>--%>
                                <%--<asp:TextBox ID="TxtPassword" runat="server" CssClass="input-text" TextMode="Password"></asp:TextBox>--%>
                            <%--</div>--%>

                            <%--<asp:Button ID="BtnLogin" runat="server" Text="Ingresar" CssClass="btn" OnClick="BtnLogin_Click" />--%>
                        </asp:Panel>
                    </div>
                </div>

                <!-- Panel para cargar información -->
                <asp:Panel ID="PnlFormulario" runat="server" Visible="false">
                    <div class="form-group">
                        <asp:Label ID="LblComentarios" runat="server" Text="Comentarios:" AssociatedControlID="TxtComentarios"></asp:Label>
                        <asp:TextBox ID="TxtComentarios" runat="server" CssClass="input-text" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <asp:Label ID="LblDocumentos" runat="server" Text="Subir archivos:" AssociatedControlID="FileUpload1"></asp:Label>
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="file-upload" AllowMultiple="true" />
                    </div>

                    <asp:Button ID="BtnEnviar" runat="server" Text="Enviar información" CssClass="btn" OnClick="BtnEnviar_Click" />
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BtnLogin" /> 
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>


