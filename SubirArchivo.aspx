
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
        
        <div>
            <br />
            <asp:Button ID="btnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
        </div>


    </asp:Panel>

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
    </div>
    <br />
    
    
    </ContentTemplate>

    
</asp:Content>
