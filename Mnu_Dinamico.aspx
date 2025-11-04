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
                    <%--<h2 class="h2 mb-3 fw-normal mt-4 align-content-center">SINIESTROS DAÑOS</h2>--%>
                    <asp:Label ID="lblTitulo_Mnu_Dinamico" runat="server" CssClass="h2 mb-3 fw-normal mt-4" style="display:block; text-align:center;" ></asp:Label>
                </div>
            </div>
        </div>
        <br />
        <div class="d-flex flex-row justify-content-center">
            <div class="mx-3 text-center" style="width: 10rem;">
                <a href="fwAlta_Asunto.aspx">
                    <img src="/Images/alta_referencia.png" class="img-fluid" alt="Alta de Asunto" />
                </a>
                <div class="mt-2 fw-bold">
                    <asp:Label ID="lblAltaReferencia" runat="server" Text="<%$ Resources:GlobalResources, lblAltaReferencia %>"></asp:Label>
                </div>
            </div>

            <div class="mx-3 text-center" style="width: 10rem;">
                <a href="fwReporte_Alta_Asunto.aspx">
                    <img src="/Images/consultas.png" class="img-fluid" alt="Consultas" />
                </a>
                <div class="mt-2 fw-bold">
                    <asp:Label ID="lblConsultas" runat="server" Text="<%$ Resources:GlobalResources, lblConsultas %>"></asp:Label>
                </div>
            </div>

            <div class="mx-3 text-center" style="width: 10rem;">
                <a id="lnkAlta_Proyecto" runat="server" href="fwAlta_Proyecto.aspx">
                    <img src="/Images/alta_proyecto.png" class="img-fluid" alt="Alta de Proyecto" />
                </a>
                <div class="mt-2 fw-bold">
                    <asp:Label ID="lblAltaProyecto" runat="server" Text="<%$ Resources:GlobalResources, lblAltaProyecto %>"></asp:Label>
                </div>
            </div>

            <div class="mx-3 text-center" style="width: 10rem;">
                <a id="lnkAlta_Inspeccion" runat="server" href="fwAlta_Inspecciones.aspx">
                    <img src="/Images/alta_inspeccion.png" class="img-fluid" alt="Alta de Inspección" />
                </a>
                <div class="mt-2 fw-bold">
                    <asp:Label ID="lblAltaInspeccion" runat="server" Text="<%$ Resources:GlobalResources, lblAltaInspeccion %>"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Agrega más elementos similares aquí con las imágenes deseadas -->
        <br />

        <div class="d-flex flex-row justify-content-center">
            <div class="mx-3 text-center" style="width: 10rem;">
                <%--<a href="fwModulo_Operaciones.aspx">--%>
                <a>
                    <img src="/Images/operaciones.png" class="img-fluid" alt="Operaciones" />
                </a>
                <div class="mt-2 fw-bold">
                    <asp:Label ID="lblOperaciones" runat="server" Text="<%$ Resources:GlobalResources, lblOperaciones %>"></asp:Label>
                </div>
            </div>

            <div class="mx-3 text-center" style="width: 10rem;">
                <%--<a href="fwModulo_Correspondencia.aspx">--%>
                <a>
                    <img src="/Images/correspondencia.png" class="img-fluid" alt="Correspondencia" />
                </a>
                <div class="mt-2 fw-bold">
                    <asp:Label ID="lblCorrespondencia" runat="server" Text="<%$ Resources:GlobalResources, lblCorrespondencia %>"></asp:Label>
                </div>
            </div>

            <div class="mx-3 text-center" style="width: 10rem;">
                <a href="fwPV_Mnu_Dinamico.aspx">
                    <img src="/Images/proveedor.png" class="img-fluid" alt="Proveedores" />
                </a>
                <div class="mt-2 fw-bold">
                    <asp:Label ID="lblProveedores" runat="server" Text="<%$ Resources:GlobalResources, lblProveedores %>"></asp:Label>
                </div>
            </div>

            <div class="mx-3 text-center" style="width: 10rem;">
                <a></a>
                <div class="mt-2 fw-bold">&nbsp;</div>
            </div>
        </div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>

