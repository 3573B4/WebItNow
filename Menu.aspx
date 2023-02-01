<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="WebItNow.Menu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js" type="text/javascript"></script>
    <style type="text/css">
        .txtAlign { Text-Align: right }

        .CajaDialogo

        {

            background-color: lightcyan;

            border-width: 4px;

            border-style: outset;

            border-color: black;

            padding: 0px;

            width: 275px;

            font-weight: bold;

            font-style: italic;

        }

        .CajaDialogo div

        {

            margin: 7px;

            text-align: center;

        }

        .FondoAplicacion

        {

            background-color: Gray;

            filter: alpha(opacity=70);

            opacity: 0.7;

        }
			
		    * {
			    margin:0px;
			    padding:0px;
		    }
			
		    #header {
			    margin:auto;
			    width:100%;
			    background:black;
			    position:fixed;
			    font-family:Arial, Helvetica, sans-serif;
		    }
			
		    ul, ol {
			    list-style:none;
		    }
			
		    .nav {
			    width:750px;   /*Le establecemos un ancho*/
			    margin:0 auto; /*Centramos automaticamente*/
		    }

		    .nav > li {
			    float:left;
		    }
			
		    .nav li a {
			    background-color:#000;
			    color:#fff;
			    text-decoration:none;
			    padding:10px 12px;
			    display:block;
		    }
			
		    .nav li a:hover {
			    background-color:#434343;
		    }
			
		    .nav li ul {
			    display:none;
			    position:absolute;
			    min-width:140px;
		    }
			
		    .nav li:hover > ul {
			    display:block;
		    }
			
		    .nav li ul li {
			    position:relative;
		    }
			
		    .nav li ul li ul {
			    right:-140px;
			    top:0px;
		    }

    </style>

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

        function mpeMensajeOnOk()
        {
            //
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary" data-bs-theme="dark">
        <div class="container-fluid">
            <a class="navbar-brand" href="Menu.aspx">&nbsp</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <%--    <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="#">Home</a>
                            </li>   --%>
                    <%--    <li class="nav-item">
                            <a class="nav-link" href="#">Link</a>
                            </li>   --%>
                    <li class="nav-item dropdown my-1">
                        <a class="nav-link btn btn-primary dropdown-toggle text-start" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <font size="4">Documentos</font>
                        </a>
                        <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="Review_Document.aspx">Revision Documento</a></li>
                            <li><hr class="dropdown-divider"/></li>
                            <li><a class="dropdown-item" href="Request_Document.aspx">Solicitar Documento</a></li>
                            <li><hr class="dropdown-divider"/></li>
                            <li><a class="dropdown-item" href="Consulta_Document.aspx">Consultas</a></li>
                        </ul>
                    </li>
                    <li class="nav-item my-1">
                        <a class="nav-link btn btn-primary text-start" href="Login.aspx">
                            
                            <font size="4">Salir</font>
                            <%--<asp:ImageButton ID="ImgSalir" runat="server" ImageUrl="~/Images/logout-d.png" Height="45px" Width="52px" OnClick="ImgSalir_Click" CssClass="btn btn-primary my-1"/>--%>
                        </a>
                    </li>
                </ul>
                
            </div>
        </div>
    </nav>

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
