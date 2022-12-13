<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SubirArchivo.aspx.cs" Inherits="WebItNow.SubirArchivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <ContentTemplate>
    <div class="container well contenedorLogin">
        <div class="row">
            <div class="col-xs-12">
                <h2>Subir Archivo</h2>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-4 mx-auto">
                <asp:Label ID="indicaciones" runat="server" Text="Click para subir archivos" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            </div>
        </div>
        
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="estilo-foto">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icono-Subir-Archivo-morado.png" ></asp:Image>
                    <!--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/icono-Subir-Archivo-morado.png" For="FileUpload1"></asp:ImageButton>-->
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control"></asp:FileUpload>
                    
                </div>
            </div>
            

        </div>
        
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
        </div>
        <br />
        <div class="form-group">
            <div class="col-sm-12"> 
            <div class="d-grid gap-2 d-md-flex justify-content-center">
                <asp:Button ID="BtnSalir"  runat="server" Font-Bold="True" Text="    Salir     " OnClick="BtnSalir_Click" CssClass="btn btn-outline-primary"/>
                <asp:Button ID="BtnEnviar" runat="server" Font-Bold="True" Text="    Subir     " OnClick="BtnEnviar_Click" CssClass="btn btn-primary me-md-2" />
                
            </div>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                <asp:Label ID="LblMessage" runat="server" Text="" />
            </div>
        </div>
    </div>
    <br />
    
    
    </ContentTemplate>
</asp:Content>
