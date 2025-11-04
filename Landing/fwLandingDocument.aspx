<%@ Page Title="" Language="C#" MasterPageFile="~/Landing/Landing.Master" AutoEventWireup="true" CodeBehind="fwLandingDocument.aspx.cs" Inherits="WebItNow_Peacock.Landing.fwLandingDocument" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        if (history.forward(1)) {
            location.replace(history.forward(1))
        }

        function mayus(e) {
            e.value = e.value.toUpperCase();
        }

        $(document).ready(function () {
            $("input").attr("autocomplete", "off");
        });

        window.onload = function () {
            // Inicia un temporizador para ejecutar después de 30 minutos (1800000 milisegundos)
            var timer = setTimeout(function () {
                // Actualiza el contenido del elemento para mostrar que la sesión ha expirado
                document.getElementById('<%=LblExpira.ClientID %>').innerHTML = '<%$ Resources:GlobalResources, msgSesionExpirada %>';
        
                // Encuentra el modal y lo muestra
                var modalId = '<%=mpeExpira.ClientID%>';
                var modal = $find(modalId);
                modal.show();

                // Inicia otro temporizador para recargar la página después de 30 minutos
                setTimeout(function () {
                    location.reload();
                }, 1800000);

            }, 1800000);
        };

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        function mpeNewEnvio() {
            //
        }

        function mpeAddArchivo() {
            //
        }

        function mpeBuscador() {
            //
        }

        document.onkeydown = function (evt) {
            return (evt ? evt.which : event.keyCode) != 13;
        }

        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress1.ClientID %>");
            updateProgress.style.display = "block";
        }

        function cerrarPestania() {
            // Intento de cerrar la pestaña actual
            window.open('', '_self', '');
            window.close();
        }

    </script>

    <script type="text/javascript">
        var _validFilejpeg = [".jpeg", ".jpg", ".bmp", ".pdf", ".zip", ".rar"];

        function validateForSize(oInput, minSize, maxSizejpeg) {
            //if there is a need of specifying any other type, just add that particular type in var  _validFilejpeg
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFilejpeg.length; j++) {
                        var sCurExtension = _validFilejpeg[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length)
                            .toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            break;
                        }
                    }

                    if (!blnValid) {

                        document.getElementById('<%=LblMessage.ClientID %>').innerHTML = 'El archivo tiene un formato invalido';
                        $find('bMpeModalCargando').show();

                        //alert("Sorry, this file is invalid, allowed extension is: " + _validFilejpeg.join(", "));
                        oInput.value = "";
                        return false;
                    }
                }
            }

            fileSizeValidatejpeg(oInput, minSize, maxSizejpeg);

        }

        function fileSizeValidatejpeg(fdata, minSize, maxSizejpeg) {
            if (fdata.files && fdata.files[0]) {
                var fsize = fdata.files[0].size / 1024; //The files property of an input element returns a FileList. fdata is an input element,fdata.files[0] returns a File object at the index 0.
                //alert(fsize)
                if (fsize > maxSizejpeg) {

                    document.getElementById('<%=LblMessage.ClientID %>').innerHTML = 'El documento Excede los 120 MB';
                    $find('bMpeModalCargando').show();

                    //alert('This file size is: ' + fsize.toFixed(2) +
                    //    "KB. Files should be in " + (minSize) + " to " + (maxSizejpeg) + " KB ");

                    fdata.value = ""; //so that the file name is not displayed on the side of the choose file button
                    return false;
                } else if (fsize < minSize){
                    document.getElementById('<%=LblMessage.ClientID %>').innerHTML = 'El archivo esta dañado';
                    $find('bMpeModalCargando').show();

                    fdata.value = ""; // para que el nombre del archivo no se muestre al lado del botón de elegir archivo
                    return false;
                } else {
                    console.log("");
                }
            }
        }

        // Función para deshabilitar los CheckBox
        function disableCheckBoxes() {
            // Seleccionar todos los CheckBox dentro del GridView
            var checkBoxes = document.querySelectorAll('#<%= GrdArch_Solicitados.ClientID %> input[type="checkbox"]');

            // Iterar sobre cada CheckBox
            checkBoxes.forEach(function (checkBox) {
                // Prevenir el cambio de estado
                checkBox.addEventListener('click', function (event) {
                    event.preventDefault();
                    return false;
                });

                // Añadir un estilo para indicar que está deshabilitado (opcional)
                checkBox.parentNode.style.opacity = '0.9';          // Reducir la opacidad para indicar que está deshabilitado
                checkBox.parentNode.style.pointerEvents = 'none';   // Deshabilitar eventos de puntero para prevenir clics
            });
        }

        // Llamar a la función cuando la página esté completamente cargada
        document.addEventListener('DOMContentLoaded', function () {
            disableCheckBoxes();
        });

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row mb-2 py-4">
                    <div class="col-lg-4 col-md-4 col-sm-12">
                    </div>
                    <div class="col-lg-12 col-md-12">
                        <div class="input-group input-group-sm  justify-content-center">
                            <h2 class="h2 mb-3 fw-normal mt-4">Ahora puede cargar documentación de su siniestro.</h2>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row ">
                    <div class="row mb-3">
                        <div class="col-lg-3 col-md-3 col-sm-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblRef" runat="server" Text="Referencia" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="row">
                                <div class="col-12">
                                    <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Referencia" AutoComplete="off" MaxLength="15" Enabled="false" ReadOnly="True" ></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblNomCliente" runat="server" Text="Cliente" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
			                    <asp:TextBox ID="TxtNomCliente" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Nombre del cliente" AutoComplete="off" MaxLength="50" Enabled="false" ReadOnly="True" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            
                <div class="row mb-2">
                    <div class="col-lg-4 col-md-4 col-sm-12">
                    </div>
                    <div class="col-lg-8 col-md-8">
                        <div class="input-group input-group-sm">
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Agregar Archivos</h2>
                        </div>
                    </div>
                </div>
                
                <div class="row" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblSolicitados" runat="server" Text="Documentos Solicitados" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                </div>

                <%-- Archivos Solicitados --%>
                <div style="overflow-y: auto; max-height: 400px; ">
                    <asp:GridView ID="GrdArch_Solicitados"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="99%"
                            CssClass="table table-responsive table-light table-striped table-hover align-middle" AlternatingRowStyle-CssClass="alt" 
                            OnPageIndexChanging="GrdArch_Solicitados_PageIndexChanging" OnRowCommand="GrdArch_Solicitados_RowCommand"
                            OnSelectedIndexChanged="GrdArch_Solicitados_SelectedIndexChanged" OnRowDataBound ="GrdArch_Solicitados_RowDataBound"
                            DataKeyNames="IdTpoDocumento" Font-Size="Smaller" >
                            <AlternatingRowStyle CssClass="alt" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgAccept_Solicitados" runat="server" Height="24px" Width="24px" ImageUrl="~/Images/aceptar_new.png" OnClick="ImgAccept_Solicitados_Click" Enabled="false" />
                                    </ItemTemplate> 
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgDecline_Solicitados" runat="server" Height="24px" Width="24px" ImageUrl="~/Images/rechazar_new.png" OnClick="ImgDecline_Solicitados_Click" Enabled="false" />
                                    </ItemTemplate> 
                                </asp:TemplateField>
                                <asp:BoundField DataField="TpoArchivo" HeaderText="Tipo de Archivo" >
                                <ItemStyle Width="75px" /> 
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" >
                                <ItemStyle Width="300px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fec_Entrega" DataFormatString="{0:d}" HeaderText="Fecha de entrega" >
                                <ItemStyle Width="125px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Nom_Archivo" HeaderText="Nombre de Archivo"  >
                                <ItemStyle Width="450px" />
                                </asp:BoundField>
                                
                                <asp:TemplateField HeaderText="Entregado" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                    <ItemStyle Width="150px" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChBoxRow" runat="server" Checked='<%# Convert.ToBoolean(Eval("IdDescarga")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Url_Archivo" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdUsuario" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdTpoDocumento" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdConclusion" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdDocumento" >
                                </asp:BoundField>
                                <asp:BoundField DataField="Id_Directorio" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IdStatus" >
                                </asp:BoundField>
                            </Columns>
                    </asp:GridView>
                </div>

                <div class="">
                    <asp:HiddenField ID="hdfValorGrid" runat="server" Value=""/>
                    <br />

                    <div class="d-grid col-6 mx-auto">
                        <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>

                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1"></h6>
                </div>

                <div class="container mt-2 mb-3">
                    <div class="row justify-content-center">
                        <div class="col-auto">
                            <asp:Button ID="BtnCloseWindows" runat="server" Text="Enviar Documentos" OnClick="BtnCloseWindows_Click" CssClass="btn btn-primary px-4" Style="width: 200px;"/>
                        </div>
                    </div>
                </div>

            </div>
            <asp:Panel ID="PnlEnvioArchivos" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; height: 180px; width: 600px; background-color:#FFFFFF;">
                <div class=" row justify-content-end" data-bs-theme="dark">
                    <div class="col-1">
                        <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="input-group mx-auto pt-3">
                        <input id="oFile" type="file" name="oFile" runat="server" onchange="validateForSize(this,1,122880);" class="form-control" />
                    </div>
                </div>
                <div>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnEnviar" runat="server" OnClick="BtnEnviar_Click" Text="Enviar" OnClientClick="showProgress()" CssClass="btn btn-outline-primary" />
                            <asp:Button ID="BtnCerrar" runat="server" OnClick="BtnCerrar_Click" Text="Cerrar" CssClass="btn btn-outline-secondary" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <br />
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
                    <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary"/>
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

                                <ajaxToolkit:ModalPopupExtender ID="mpeNewEnvio" runat="server" PopupControlID="PnlEnvioArchivos"
                                    TargetControlID="LblOculto2" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewEnvio()" >
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="LblOculto2" runat="server" Text="Label" Style="display: none;" />
                            </div>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
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
            <br />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3" DynamicLayout="true">
                <ProgressTemplate>
                <div id="divImage" class ="loading">
                    <div class="center">
                        <img alt="Processing..." src="Images\ajax-loader.gif" />
                    </div>
                </div>
                </ProgressTemplate>
            </asp:UpdateProgress>            
            <br />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GrdArch_Solicitados" />
            <asp:PostBackTrigger ControlID="BtnEnviar" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
