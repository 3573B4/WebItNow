﻿
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SubirArchivo.aspx.cs" Inherits="WebItNow.SubirArchivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <script src="https://code.jquery.com/jquery-3.6.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js"></script>
    
    <ContentTemplate>
    <div class="container well contenedorLogin">
        <div class="row">
            <div class="col-xs-12">
                <h2>Carga segura de documentos</h2>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-4 mx-auto">
                <asp:Label ID="lblUsuario" runat="server" Font-Size="XX-Large"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="d-grid col-4 mx-auto">
                <asp:Label ID="indicaciones" runat="server" Text="Seleccione el archivo a subir" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            </div>
        </div>
        <br />
        <div class="form-group">
            <div class="d-grid col-2 mx-auto">
                <div class="dropdown">
                    <asp:DropDownList ID="ddlDocs" runat="server" CssClass="btn btn-outline-secondary" OnSelectedIndexChanged="ddlDocs_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <br />
        <!-- empieza el acordion -->
        <div class="row">
            <div class="accordion" id="accordionExample">
                <div class="accordion-item">
                    <h2 class="accordion-header" id="headingOne">
                        <!--<asp:Button ID="btnAcordTitle" runat="server" Text="" class="accordion-button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne"></asp:Button>-->
                        <button id="btnAcrdTitle" class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                            Instrucciones
                        </button>
                    </h2>
                    <div id="collapseOne" class="accordion-collapse collapse show" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <strong>Subir Archivo menor de 40 MB </strong> Tomar foto o escanear el documento que desee subir. subir en PNG, JPG, PDF o ZIP.<!--It is shown by default, until the collapse plugin adds the appropriate classes that we use to style each element. These classes control the overall appearance, as well as the showing and hiding via CSS transitions. You can modify any of this with custom CSS or overriding our default variables. It's also worth noting that just about any HTML can go within the <code>.accordion-body</code>, though the transition does limit overflow.-->
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- termina el acordion -->
        
        <br />
        <div class="form-group">
            <div class="input-group mb-12">
                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control"></asp:FileUpload>
                <!--<ajaxToolkit:AjaxFileUpload ID="AjaxFileUpload1" runat="server"></ajaxToolkit:AjaxFileUpload>-->
                <!--<ajaxToolkit:AsyncFileUpload ID="AfuFileUpload" runat="server" OnUploadedComplete="AfuFileUpload_UploadedComplete" 
                    OnUploadedFileError="AfuFileUpload_UploadedFileError" OnClientUploadComplete="uploadComplete" Width="400px" UploaderStyle="Modern" 
                    UploadingBackColor="#CCFFFF" ThrobberID="myThrobber">
                </ajaxToolkit:AsyncFileUpload>-->
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
        <div class="row">
            <asp:GridView ID="gvEstadoDocs" runat="server" AutoGenerateColumns="False" GridLines="None" Width="380px"
                AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                PageSize="7" >
                <AlternatingRowStyle CssClass="table" />
                <Columns>
                    <asp:BoundField DataField="Descripcion" HeaderText="Tipo Documento"/>
                    <asp:BoundField DataField="Desc_status" HeaderText="Status"/>
                </Columns>
                <PagerStyle CssClass="pgr" />
            </asp:GridView>
        </div>
        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
            </div>
        </div>
    </div>
    <br />

    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="display: none;">

        <table border="0" width="287px" style="margin: 0px; padding: 0px; background-color: #0033CC; color: #FFFFFF;">
            <tr>
                <td align="left">
                    <asp:Label ID="Label6" runat="server" Text="I t n o w" />
                </td>
                <td>
                </td>
            </tr>
        </table>

        <div>
            <br />
            <table border="0" width="275px" style="margin: 0px; padding: 0px;" >
                <tr>
                    <td><asp:Label ID="LblMessage" runat="server" Text="" /></td>
                    <td></td>
                </tr>
            </table>
        </div>

        <div>
            <br />
            <table border="0" width="275px" style="margin: 0px; padding: 0px;">
                <tr>
                    <td align="center"><asp:Button ID="btnClose" runat="server" Text="Cerrar" /></td>
                    <td></td>
                </tr>
            </table>
        </div>

    </asp:Panel>
    
    
    </ContentTemplate>
</asp:Content>
