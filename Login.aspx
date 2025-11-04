<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebItNow_Peacock.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <meta name="description" content="@Html.Raw(ViewBag.description)" />

    <title></title>
    <script language="javascript" type="text/javascript">

        setInterval(mantenerSesionActiva, 5 * 60 * 1000);  // 5 minutos en milisegundos

        function mantenerSesionActiva() {
            // Realizar una solicitud al servidor para mantener la sesión activa
            var xhr = new XMLHttpRequest();

            // Especificar el método y la URL del servidor (usar Login.aspx)
            /*xhr.open("GET", "Login.aspx?accion=MantenSesion", true);*/
            xhr.open("GET", "Login.aspx", true);

            // Manejar la respuesta del servidor
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    // La solicitud fue exitosa
                    console.log("Sesión actualizada correctamente.");
                }
            };

            // Enviar la solicitud
            xhr.send();
        }

        function EnterEvent(e, ctrl) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == 13 && ctrl.value.length > 2) {
                /*return false;*/
                $('[id$=BtnAceptar]').click();
            }
            else {
                //return true;
                return false;
            }
        }

        if (history.forward(1)) {
            location.replace(history.forward(1))
        }

        function mayus(e) {
            e.value = e.value.toUpperCase();
        }

        $(document).ready(function () {
            $("input").attr("autocomplete", "off");
        });


        function mpeMensajeOnOk() {
            //
        }

        function mpeNewEnvio() {
            //
        }

        function mpeAddArchivo() {
            //
        }

        //document.onkeydown = function (evt) {
        //    return (evt ? evt.which : event.keyCode) != 13;
        //}

        function setIdioma(codigoIdioma) {
            __doPostBack('ddlIdioma', codigoIdioma);
        }

    </script>

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

    <!-- Estilo para Agregar THEAD y TBODY a GridView. -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/css/footable.min.css" rel="stylesheet" type="text/css" />
    <!-- CSS de Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- JS de Bootstrap -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <link href="~/Styles/center_controls.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/default.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/panel_message.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/GridView.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/fuentes.css" rel="stylesheet" type="text/css" />
    <link href="Styles/pagination-ys.css" rel="stylesheet" type="text/css" />
</head>

<body style="overflow: hidden;">
    <form id="form1" runat="server">
<%-- 
        <!-- Dropdown de idioma superior derecho -->
       <div style="position:absolute; top:10px; right:10px; z-index:9999;">
            <asp:DropDownList ID="ddlIdioma" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIdioma_SelectedIndexChanged">
                <asp:ListItem Text="Español" Value="es-MX"></asp:ListItem>
                <asp:ListItem Text="Português" Value="pt-BR"></asp:ListItem>
                <asp:ListItem Text="English" Value="en-US"></asp:ListItem>
            </asp:DropDownList>
        </div>
--%>

        <!-- Dropdown de idioma con ícono -->
        <div class="dropdown" style="position:absolute; top:10px; right:10px; z-index:9999;">
          <button class="btn btn-light border dropdown-toggle d-flex align-items-center shadow-sm" 
                  type="button" id="ddlIdiomaBtn" data-bs-toggle="dropdown" aria-expanded="false">
            <i class="bi bi-globe me-2"></i> <% = GetGlobalResourceObject("GlobalResources", "lblIdioma") %>
          </button>
          <ul class="dropdown-menu dropdown-menu-end shadow" aria-labelledby="ddlIdiomaBtn">
            <li>
              <a class="dropdown-item d-flex align-items-center" href="javascript:void(0);" onclick="setIdioma('es-MX')">
                <img src="/Images/es.png" width="20" class="me-2" /> Español
              </a>
            </li>
            <li>
              <a class="dropdown-item d-flex align-items-center" href="javascript:void(0);" onclick="setIdioma('pt-BR')">
                <img src="/Images/pt.png" width="20" class="me-2" /> Português
              </a>
            </li>
            <li>
              <a class="dropdown-item d-flex align-items-center" href="javascript:void(0);" onclick="setIdioma('en-US')">
                <img src="/Images/en.png" width="20" class="me-2" /> English
              </a>
            </li>
          </ul>
        </div>

<%--
        <div class="dropdown" style="position:absolute; top:10px; right:10px; z-index:9999;">
          <button class="btn btn-outline-light dropdown-toggle" type="button" id="ddlIdiomaBtn" data-bs-toggle="dropdown" aria-expanded="false">
            🌐 <% = GetGlobalResourceObject("GlobalResources", "lblIdioma") %>
          </button>
          <ul class="dropdown-menu dropdown-menu-dark dropdown-menu-end" aria-labelledby="ddlIdiomaBtn">
            <li><a class="dropdown-item" href="javascript:void(0);" onclick="setIdioma('es-MX')">🇲🇽 Español</a></li>
            <li><a class="dropdown-item" href="javascript:void(0);" onclick="setIdioma('pt-BR')">🇧🇷 Português</a></li>
            <li><a class="dropdown-item" href="javascript:void(0);" onclick="setIdioma('en-US')">🇺🇸 English</a></li>
          </ul>
        </div>
--%>

<%--
        <div style="position:absolute; top:10px; right:10px; z-index:9999;">
          <div class="btn-group">
            <button type="button" class="btn btn-outline-primary dropdown-toggle rounded-pill" 
                    data-bs-toggle="dropdown" aria-expanded="false">
              🌍  <% = GetGlobalResourceObject("GlobalResources", "lblIdioma") %>
            </button>
            <ul class="dropdown-menu dropdown-menu-end shadow-sm">
              <li><a class="dropdown-item" href="javascript:void(0);" onclick="setIdioma('es-MX')">Español</a></li>
              <li><a class="dropdown-item" href="javascript:void(0);" onclick="setIdioma('pt-BR')">Português</a></li>
              <li><a class="dropdown-item" href="javascript:void(0);" onclick="setIdioma('en-US')">English</a></li>
            </ul>
          </div>
        </div>
--%>

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Banderas de idioma -->
<%--
        <div style="text-align:right; margin:10px;">
            <asp:ImageButton ID="btnEspañol" runat="server" ImageUrl="~/Images/es.png"
                ToolTip="Español" OnClick="btnEspañol_Click" CausesValidation="false" />
            <asp:ImageButton ID="btnPortugues" runat="server" ImageUrl="~/Images/pt.png"
                ToolTip="Português" OnClick="btnPortugues_Click" CausesValidation="false" />
        </div>
--%>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div id="contenedor">
                <div id="panelSuperior" class="container col-md-4">
                    <%--<h2 class="h2 mb-3 fw-normal mt-4"> Iniciar Sesión</h2>--%>
                    <asp:Label ID="lblTitulo" runat="server" CssClass="h2 mb-3 fw-normal mt-4" style="display:block; text-align:center;" ></asp:Label>

                    <div class="form-group mb-2">
                        <asp:Label ID="lblUsuario" runat="server" Text="<%$ Resources:GlobalResources, lblUsuario %>" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"   
                            ControlToValidate="TxtUsu" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:Panel ID="pnlUsu" runat="server" DefaultButton="BtnAceptar">
                                <asp:TextBox ID="TxtUsu" runat="server" CssClass="form-control" placeholder="<%$ Resources:GlobalResources, lblUsuario %>"  MaxLength="20" ></asp:TextBox>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="form-group mb-2">
                        <asp:Label ID="LblPass" runat="server" Text="<%$ Resources:GlobalResources, lblContraseña %>" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"   
                            ControlToValidate="TxtPass" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:Panel ID="pnlPass" runat="server" DefaultButton="BtnAceptar">
                                <asp:TextBox ID="TxtPass" runat="server" CssClass="form-control" placeholder="<%$ Resources:GlobalResources, lblContraseña %>" MaxLength="20" TextMode="Password"></asp:TextBox>
                            </asp:Panel>
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
                        <asp:Label ID="lblVerificacion" runat="server" CssClass="control-label col-sm-2" Text="<%$ Resources:GlobalResources, lblCodigoVerificacion %>" Font-Size="Small"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"   
                            ControlToValidate="txtVerificationCode" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        <div class="col-sm-12">
                            <asp:Panel ID="pnlCaptcha" runat="server" DefaultButton="BtnAceptar">
                                <asp:TextBox runat="server" ID="txtVerificationCode" placeholder="<%$ Resources:GlobalResources, lblCodigoVerificacion %>" onkeyup="mayus(this);" CssClass="form-control"></asp:TextBox>
                            </asp:Panel>

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
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnAceptar" runat="server" Text="<%$ Resources:GlobalResources, btnIniciarSesion %>" Font-Bold="True" CssClass="btn btn-success me-md-2" OnClick="BtnAceptar_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
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
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnEspañol" />--%>
                <%--<asp:PostBackTrigger ControlID="btnPortugues" />--%>
                <asp:PostBackTrigger ControlID="BtnAceptar" /> 
            </Triggers>
        </asp:UpdatePanel>

<%--        
        <script language="javascript" type="text/javascript">
            setInterval('MantenSesion()', <%= (int) (0.9 * (Session.Timeout * 30000)) %>);
        </script>
--%>
    </form>
</body>
</html>
