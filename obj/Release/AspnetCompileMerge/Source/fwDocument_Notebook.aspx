<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwDocument_Notebook.aspx.cs" Inherits="WebItNow_Peacock.fwDocument_Notebook" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
                document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
        
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row mb-2 py-4">
                    <div class="col-lg-4 col-md-4 col-sm-12">
                    </div>
                    <div class="col-lg-8 col-md-8">
                        <div class="input-group input-group-sm">
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Alta de Cuaderno</h2>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row ">
                    <div class="row mb-3">
                        <div class="col-lg-4 col-md-4 col-sm-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblRef" runat="server" Text="Referencia" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                            </div>
                            <asp:Panel runat="server" DefaultButton="ImgBusReference">
                                <div class="row">
                                    <div class="col-12">
                                        <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Referencia" AutoComplete="off" MaxLength="15" ></asp:TextBox>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblSiniestro" runat="server" Text="Siniestro" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                            </div>
                            <asp:Panel runat="server" DefaultButton="ImgBusReference">
                                <div class="row">
                                    <div class="col-11">
                                        <asp:TextBox ID="TxtSiniestro" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Siniestro" AutoComplete="off" MaxLength="14" ></asp:TextBox>
                                    </div>
                                    <div class="col-1 justify-content-start ps-0 ms-0">
                                        <asp:ImageButton ID="ImgBusReference" runat="server" CssClass="p-1" ImageUrl="~/Images/search_find.png" Height="32px" Width="32px" OnClick="ImgBusReference_Click" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblReporte" runat="server" Text="Reporte" CssClass="control-label col-sm-2" Font-Size="Small" Visible="false"></asp:Label>
                            </div>
                            <asp:Panel runat="server" DefaultButton="ImgBusReference">
                                <div class="row">
                                    <div class="col-12">
                                        <asp:TextBox ID="TxtReporte" runat="server" CssClass="form-control form-control-sm" placeholder="Reporte" AutoComplete="off" MaxLength="15" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-lg-4 col-md-4 col-sm-12">
                            <div class="mb-2">
                                <asp:Label ID="LblNomProyecto" runat="server" Text="Nombre del Proyecto"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtNomProyecto" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Nombre del Proyecto" AutoComplete="off" MaxLength="25" Enabled="false" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-7 col-md-7">
                            <div class ="mb-2">
                                <asp:Label ID="LblNomCliente" runat="server" Text="Nombre del Cliente"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="TxtNomCliente" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Nombre del Cliente" AutoComplete="off" MaxLength="60"  Enabled="false" ReadOnly="True" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-lg-4 col-md-4 col-sm-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblTpoAsunto" runat="server" Text="Tipo de Asunto"></asp:Label>
                            </div>
                            <div class=" input-group input-group-sm">
                                <asp:TextBox ID="TxtTpoAsunto" runat="server" CssClass="form-control form-control-sm" onkeyup="mayus(this);" placeholder="Tipo de Asunto" AutoComplete="off" MaxLength="50"  Enabled="false" ReadOnly="True" ></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-lg-7 col-md-7 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblNomAsegurado" runat="server" Text="Nombre del Asegurado" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
			                    <asp:TextBox ID="TxtNomAsegurado" runat="server" CssClass="form-control form-control-sm" onkeydown = "return (event.keyCode!=13);" onkeyup="mayus(this);" placeholder="Nombre del asegurado" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-lg-1 col-md-1 ps-0  ms-0 " style="display:flex; align-items:end;">
                            <asp:ImageButton ID="ImgEditar" runat="server" CssClass="p-1" ImageUrl="~/Images/edit.jpg" Height="36px" Width="36px" OnClick="ImgEditar_Click" Enabled="true" visible="true" />
                            <asp:ImageButton ID="ImgAceptar" runat="server" CssClass="p-1" ImageUrl="~/Images/aceptar_new.png" Height="36px" Width="36px" OnClick="ImgAceptar_Click" visible="false" />
                            <asp:ImageButton ID="ImgCancelar" runat="server" CssClass="p-1" ImageUrl="~/Images/rechazar_new.png" Height="36px" Width="36px" OnClick="ImgCancelar_Click" visible="false" />
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-lg-4 col-md-4 col-sm-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblConclusion" runat="server" Text="Estatus" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlConclusion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlConclusion_SelectedIndexChanged" visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12">
                            <div class ="mb-2">
                                <asp:Label ID="LblRegimen" runat="server" Text="Tipo de Asegurado" CssClass="control-label" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlRegimen" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlRegimen_SelectedIndexChanged" visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

<%--                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                        <asp:Button ID="BtnRegresar" runat="server" Text="&nbsp;&nbsp;&nbsp;Regresar&nbsp;&nbsp;&nbsp;" OnClick="BtnRegresar_Click" CssClass="btn btn-secondary" TabIndex="1"/>
                    </div>--%>
                </div>

                <div class="container mt-2 mb-3">
                    <div class="row justify-content-center">
                        <div class="col-auto">
                            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" OnClick="BtnRegresar_Click" CssClass="btn btn-secondary px-4" Style="width: 200px;" TabIndex="1" />
                        </div>
                    </div>
                </div>

                <div class="row mb-2">
                    <div class="col-lg-4 col-md-4 col-sm-12">
                    </div>
                    <div class="col-lg-8 col-md-8">
                        <div class="input-group input-group-sm">
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Agregar Archivos de Trabajo</h2>
                        </div>
                    </div>
                </div>

                <div class="row" style="background-color:#96E7D9; align-items: baseline;">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblSolicitados" runat="server" Text="Documentos Solicitados" CssClass="control-label" Font-Size="small"></asp:Label>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnCorresp_Solicitados" runat="server" Text="+" OnClick="BtnCorresp_Solicitados_Click" Height="24px" Width="24px" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <%-- Archivos Solicitados --%>
                <%--<div style="overflow-x: auto; overflow-y:hidden">--%>
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
                                
                                <%--<asp:BoundField DataField="IdDescarga" HeaderText="Entregado" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">--%>
                                <%--<ItemStyle Width="150px" />--%>
                                <%--</asp:BoundField>--%>

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
                            <%--<PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />--%>
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

            </div>
            <br />

<%--        <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                <asp:Button ID="BtnGenerarArch" runat="server" Text="Generar Cuaderno" OnClick="BtnGenerarArch_Click" CssClass="btn btn-primary px-4" />
            </div>--%>

            <div class="container mt-2 mb-3">
                <div class="row justify-content-center">
                    <div class="col-auto">
                        <asp:Button ID="BtnGenerarArch" runat="server" Text="Generar Cuaderno" OnClick="BtnGenerarArch_Click" CssClass="btn btn-primary px-4" Style="width: 200px;"/>
                    </div>
                </div>
            </div>

            <br />
            <asp:Panel ID="PnlAgregarArchivo" runat="server" CssClass="CajaDialogo" Style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
                <div class="mt-3">
                <table cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="LblEtiqueta" runat="server" style="font-size:small" CssClass="align-content-center" Text="&nbsp;&nbsp;&nbspSeleccionar el tipo de documento" ></asp:Label>
                        </td>
                    </tr>
                </table>
                </div>
                <div class="" style="display:flex; flex-direction:column; justify-content:start;">
                    <div class="mt-2">
                        <asp:DropDownList ID="ddlSegmento" runat="server" CssClass="btn btn-outline-secondary text-start" Width="100%">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <hr class="dropdown-divider" />
                    </div>
                    <div class="mt-2">
                        <asp:TextBox ID="TxtNomArch" runat="server" CssClass="form-control form-control-sm" placeholder="Nombre del archivo" Height="38px" AutoComplete="off" MaxLength="60" ></asp:TextBox>
                    </div>
                    <div>
                        <hr class="dropdown-divider" />
                    </div>
                    <%--<div class="mb-0 pb-0" style="align-items:start; justify-content:start;">--%>
                        <%--<input id="oFileArch" type="file" name="oFileArch" runat="server" onchange="validateForSize(this,1,122880);" class="form-control" />--%>
                    <%--</div>--%>
                </div>
                <div>
                    <hr class="dropdown-divider " />
                </div>
                <div class="" style="display:flex; justify-content:space-evenly;">
                    <asp:Button ID="BtnPnlMsgAceptar" runat="server" Text="Aceptar" OnClick="BtnPnlMsgAceptar_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="BtnClousePnlAdd" OnClick="BtnClousePnlAdd_Click" runat="server" Text="Cerrar" CssClass="btn btn-outline-secondary" />
                        
                </div>
            </asp:Panel>
            <br />
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
            <asp:Panel ID="pnlMensaje_1" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
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
                    <asp:Label ID="LblMessage_1" runat="server" Text="" />
                </div>
                <div>
                    <br />
                    <hr class="dropdown-divider" />
                </div>
        
                <div class="d-flex justify-content-center mb-3">
                    <br />
                    <asp:Button ID="BtnAceptar" runat="server" OnClick="BtnAceptar_Click" Text="Crear Referencia" CssClass="btn btn-outline-primary mx-1" />
                    <asp:Button ID="BtnCancelar" runat="server" OnClick="BtnCancelar_Click" Text="Cerrar" CssClass="btn btn-outline-secondary mx-1" />
                </div>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlMensaje_2" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
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
                    <asp:Label ID="LblMessage_2" runat="server" Text="" />
                </div>
                <div>
                    <br />
                    <hr class="dropdown-divider" />
                </div>
        
                <div class="d-flex justify-content-center mb-3">
                    <br />
                    <asp:Button ID="BtnAceptar_Del_Doc" runat="server" OnClick="BtnAceptar_Del_Doc_Click" Text="Aceptar" CssClass="btn btn-outline-primary mx-1" />
                    <asp:Button ID="BtnCancelar_Del_Doc" runat="server" OnClick="BtnCancelar_Del_Doc_Click" Text="Cancelar" CssClass="btn btn-outline-secondary mx-1" />
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
            <asp:Panel ID="PnlBuscador" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 500px; background-color:#FFFFFF;">
                <div class="row justify-content-end" data-bs-theme="dark">
                    <div class="col-1">
                        <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container">
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>

                            <div>
                            <br />
                                <div class="mb-2">
                                    <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:GridView ID="GrdBuscador" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                            AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" 
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GrdBuscador_PageIndexChanging" OnRowCommand="GrdBuscador_RowCommand" 
                                            PageSize="10" Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                            <AlternatingRowStyle CssClass="alt autoWidth" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Referencia">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkReferencia" runat="server" UseSubmitBehavior="true" style="display: block; text-align: left; text-decoration: none;" CommandName="SelectBuscador" CommandArgument='<%# Eval("Referencia") %>'><%# Eval("Referencia") %></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NumSiniestro" HeaderText="Número de Siniestro" >
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="GrdBuscador" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <br />
                                <hr class="dropdown-divider" />
                            </div>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                                    <ContentTemplate>
                                        <asp:Button ID="btnClose_Proceso" runat="server" OnClick="btnClose_Proceso_Click" Text="Cerrar" CssClass="btn btn-outline-secondary btn-sm" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <br />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnClose_Proceso" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnClose_Proceso" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>
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

                                <ajaxToolkit:ModalPopupExtender ID="mpeAddArchivo" runat="server" PopupControlID="PnlAgregarArchivo"
                                    TargetControlID="lblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeAddArchivo()">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="lblOculto3" runat="server" Text="Label" Style="display: none;" />

                                <ajaxToolkit:ModalPopupExtender ID="mpeBuscador" runat="server" PopupControlID="PnlBuscador"
                                    TargetControlID="lblOculto1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeBuscador()">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="lblOculto1" runat="server" Text="Label" Style="display: none;" />

                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <div class="d-grid col-6 mx-auto">
                                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_1" runat="server" PopupControlID="pnlMensaje_1"
                                    TargetControlID="lblOculto_1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="lblOculto_1" runat="server" Text="Label" Style="display: none;" />
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <div class="d-grid col-6 mx-auto">
                                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje_2" runat="server" PopupControlID="pnlMensaje_2"
                                    TargetControlID="lblOculto_2" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Label ID="lblOculto_2" runat="server" Text="Label" Style="display: none;" />
                            </div>
                        </div>
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

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBusReference" />
            <asp:PostBackTrigger ControlID="GrdArch_Solicitados" />
            <asp:PostBackTrigger ControlID="BtnEnviar" />
            <asp:PostBackTrigger ControlID="BtnCorresp_Solicitados" />
            <asp:PostBackTrigger ControlID="BtnPnlMsgAceptar" />

            <%--<asp:PostBackTrigger ControlID="btnCrearRef" />--%>

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>

