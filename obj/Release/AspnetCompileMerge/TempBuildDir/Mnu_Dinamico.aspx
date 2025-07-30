<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Mnu_Dinamico.aspx.cs" Inherits="WebItNow_Peacock.Mnu_Dinamico" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    <div class="container col-lg-7 col-md-8 col-sm-8">
        <div class="row mb-2 py-4">
            <div class="col-lg-4 col-md-4 ">
            </div>
            <div class="col-lg-8 col-md-8">
                <div class="input-group input-group-sm">
                    <%-- Selecciona un tipo de Operación --%>
                    <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">SINIESTROS DAÑOS</h2>
                </div>
            </div>
        </div>
        <br />
        <div class="d-flex flex-row">
            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <a href="fwAlta_Asunto.aspx">
                    <img src="/Images/alta_referencia.png" class="img-fluid" alt="..." />
                </a>
            </div>

            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <a href="fwReporte_Alta_Asunto.aspx">
                    <img src="/Images/consultas.png" class="img-fluid" alt="..." />
                </a>
            </div>

            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <a id="lnkAlta_Proyecto" runat="server" href="fwAlta_Proyecto.aspx">
                    <img src="/Images/alta_proyecto.png" class="img-fluid" alt="..." />
                </a>
            </div>

            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <a id="lnkAlta_Inspeccion" runat="server" href="fwAlta_Inspecciones.aspx">
                    <img src="/Images/alta_inspeccion.png" class="img-fluid" alt="..." />
                </a>
            </div>
        </div>
        <!-- Agrega más elementos similares aquí con las imágenes deseadas -->
        <br />
        <div class="d-flex flex-row">
            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <%--<a href="fwModulo_Operaciones.aspx">--%>
                <a>
                   <img src="/Images/operaciones.png" class="img-fluid" alt="..." />
                </a>
                <%--</a>--%>
            </div>

            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <%--<a href="fwModulo_Correspondencia.aspx">--%>
                <a>
                   <img src="/Images/correspondencia.png" class="img-fluid" alt="..." />
                </a>
                <%--</a>--%>
            </div>

            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <a href="fwPV_Mnu_Dinamico.aspx">
                    <img src="/Images/proveedor.png" class="img-fluid" alt="..." />
                </a>
            </div>

            <div class="mx-2" style="width: 10rem; height: 10rem;">
                <a> </a>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>

