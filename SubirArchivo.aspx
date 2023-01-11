
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SubirArchivo.aspx.cs" Inherits="WebItNow.SubirArchivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        function openModal(event) {
            $(document).ready(function () {

                var ref = $('#exampleModal').modal();
                ref = false;
                return false;

            });
        }
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

        function mpeMensajeOnOk() {
            //
        }

    </script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />

    
    <ContentTemplate>
    
        <!-- Modal -->
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content modal-dialog-centered">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="exampleModalLabel">Modal title</h1>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        ...
                        <asp:Label ID="Lbl_M_Message" runat="server" Text="" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary">Save changes</button>
                    </div>
                </div>
            </div>
        </div>

    <div class="container">
        <div class="row justify-content-center">
            <div class="row">
                <div class="col-xs-12">
                    <h2>Carga segura de documentos</h2>
                </div>
            </div> 
            <div class="col-sm-4">
                <div class="row">
                        
                    <div class="form-group">
                        <div class="d-grid col-4 mx-auto py-1">
                            <asp:Label ID="lblUsuario" runat="server" Font-Size="XX-Large" ></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="d-grid col-4 mx-auto py-1">
                            <asp:Label ID="indicaciones" runat="server" Text="Seleccione el archivo a subir" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="form-group">
                        <div class="d-grid col-12 mx-auto py-3">
                            <div class="dropdown">
                                <asp:DropDownList ID="ddlDocs" runat="server" CssClass="btn btn-outline-secondary"  AutoPostBack="true" OnSelectedIndexChanged="ddlDocs_SelectedIndexChanged" Width="400">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <!-- empieza el acordion -->
                    <div class="row">
                        <div class="accordion accordion" id="accordionExample">
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="headingOne">
                                    <!--<asp:Button ID="btnAcordTitle" runat="server" Text="" class="accordion-button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne"></asp:Button>-->
                                    <button id="btnAcrdTitle" class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="false" aria-controls="collapseOne">
                                        Instrucciones</button>
                                </h2>
                                <div id="collapseOne" class="accordion-collapse collapse" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
                                    <div class="accordion-body">
                                        <asp:Label ID="LblDescrpBrev" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- termina el acordion -->
                    
                    

                    <div class="form-group">
                        <div class="input-group mx-auto pt-3">
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control"></asp:FileUpload>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto pt-3">
                            <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <div class="col-sm-12">
                            <div class="d-grid gap-4 d-md-flex justify-content-center">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Text="    Salir     " OnClick="BtnSalir_Click" CssClass="btn btn-outline-primary" />
                                <asp:Button ID="BtnEnviar" runat="server" Font-Bold="True" Text="    Subir     " OnClick="BtnEnviar_Click"  CssClass="btn btn-primary me-md-2" />
                            </div>
                        </div>
                    </div>
                    
                </div>
            </div>
            <div class="row justify-content-center">
                <div class="col-5 mt-5">
                    <div class="row">
                        <asp:GridView ID="gvEstadoDocs" runat="server" AutoGenerateColumns="False" GridLines="None" Width="380px"
                            AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            PageSize="7" OnSelectedIndexChanged="gvEstadoDocs_SelectedIndexChanged" >
                            <AlternatingRowStyle CssClass="table" />
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Tipo Documento" />
                                <asp:BoundField DataField="Nom_Imagen" HeaderText="Documento" />
                                <asp:BoundField DataField="Desc_status" HeaderText="Status" />
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                        </asp:GridView>
                    </div>
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                                TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                        </div>
                        <br />
                        <div class="d-grid col-6 mx-auto">
                            <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                                TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
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

    </ContentTemplate>

    
</asp:Content>
