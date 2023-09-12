<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Document_Fotos.aspx.cs" Inherits="WebItNow.Document_Fotos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        var timer = setTimeout(function () {
            document.getElementById('<%=LblExpira.ClientID %>').innerHTML = 'La sesión ha expirado.';
            var modalId = '<%=mpeExpira.ClientID%>';
            var modal = $find(modalId);
            modal.show();
        }, 3600000);

        function acceso() {
            location.href = '/Login.aspx';
        }

        function mpeMensajeOnOk() {
            //
        }

        function mpeSbrArchivo() {
            //
        }

    </script>

    <script type="text/javascript">
        var _validFilejpeg = [".jpeg", ".jpg", ".bmp"];

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
    </script>

    <script language="javascript" type="text/javascript">
        $(function () {
            $("#rondellCarousel").rondell({
                preset: "carousel",
            });
        });

        $(function() {
            // Create a rondell with the 'carousel' preset and set an option
            // to disable the rondell while the lightbox is displayed
            $('#rondellSlider> *').rondell({
                preset:"slider",
            });
        });

        //$('.myContainer > .myItem').rondell([opciones][, callback]);

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="container col-md-8 col-lg-7">
            <div class="row mt-4">
                <div class="row col-lg-8 col-md-7 col-sm-7 py-0">
                    <div class="col-2 py-0 pt-2">
                        <asp:Label ID="LblReferencia" runat="server" Text="Referencia" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-4">
                        <div class="input-group">
                            <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control form-control-sm" MaxLength="12" ReadOnly="true" autocomplete="off" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-2 py-0 pt-2">
                        <asp:Label ID="LblAsegurado" runat="server" Text="Asegurado: " CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-4">
                        <asp:TextBox ID="TxtAsegurado" runat="server" CssClass="form-control form-control-sm" Font-Size="Small" ReadOnly="true" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row mb-3 mt-4" style="background-color:mediumturquoise;">
                <h6 class="h6 fw-normal my-1"> </h6>
            </div>
            <div class="d-flex gap-4 justify-content-center">
                <asp:Button ID="btnSubir" runat="server" Text="Subir Foto" CssClass="btn btn-primary btn-sm" OnClick="btnSubir_Click" />
                <asp:Button ID="btnDescargarZip" runat="server" Text="Descargar Zip" OnClick="btnDescargarZip_Click" CssClass="btn btn-secondary btn-sm" />
            </div>
            <br />
            <div class="row mb-3 mt-4" >
                <div class="col-6 mt-4">
                    <asp:DataList ID="DataList1" runat="server" RepeatColumns="5" CellPadding="5" CellSpacing="5" HorizontalAlign="Left" RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <div class="card-group"> 
                                <div class="card d-flex align-items-center m-2" >
                                    <div class="d-flex align-items-center" style="height:200px; width:200px;">
                                        <asp:Image ID="Image1" ImageAlign="AbsBottom" CssClass="card-img" ImageUrl='<%# Eval("Value") %>' runat="server" />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </div>
        </div>
        <br />
        <div class="from-group mb-4 pb-5">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"  />
            </div>
        </div>

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" />
            </div>
        </div>
        <br />
        <asp:Panel ID="pnlSubirArchivo" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 650px; background-color:#FFFFFF;">
            <div class="d-flex bd-highlight">
                <div class="p-2 w-100 bd-highlight d-flex flex-row ">
                    <asp:Label ID="lblPnlTitulo" runat="server" Text="Subir fotos" Font-Size="Medium"></asp:Label>
                </div>
                <div class="p-2 flex-shrink-1 bd-highlight">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>
            <div>
                    <br />
                <hr class="dropdown-divider" />
            </div>
            <div class="mx-3">
                <asp:Label ID="LblPnlIndicaciones" runat="server" Text="Puede seleccionar varios archivos a la vez dejando presionada la tecla <control> y dando click con el mouse en la ventana de selección de archivos sobre cada uno de ellos."></asp:Label>
            </div>
            <div>
                    <br />
                <hr class="dropdown-divider" />
            </div>
            <div class="form-group d-flex">
                <asp:Label ID="lblPnlFotosEnviar" runat="server" Text="Fotos a enviar"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
<%--                
                    <div class="input-group mx-2" style="width:400px;">
                        <asp:FileUpload ID="UploadFilePnl" runat="server" AllowMultiple="true"></asp:FileUpload>
                    </div>
--%>
                    <div class="input-group mx-auto pt-3">
                        <input id="oFile" type="file" name="oFile" multiple="multiple" onchange="validateForSize(this,1,122880);" class="form-control" />
                    </div>
<%--                    
                    <div class="input-group mx-2" >
                        <asp:Label ID="Span1" runat="server" Text="Label"></asp:Label>
                    </div>
--%>
                    <div class="form-group">
                        <div class="row">
                            <asp:Label ID="lblPnlMensageError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <div class="row">
                            <asp:Label ID="lblpnltpoDoc" runat="server" Text="Tipo de imagenes permitidas: jpeg, jpg"></asp:Label>
                        </div>
                        <div class="row">
                            <asp:Label ID="lblPnlTamMax" runat="server" Text="Tamaño máximo de archivo: 4Mb"></asp:Label>
                        </div>
<%--                        
                        <div class="row">
                            <asp:Label ID="lblPnlListasubidos" runat="server" Text="g"></asp:Label>
                        </div>
--%>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
<%--
                            <div class="from-group">
                                <asp:Button ID="btnPnlSubir" runat="server" Text="Enviar" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnPnlSubir_Click" />
                            </div>
--%>
                            <div class="d-grid gap-4 d-flex justify-content-center mb-2">
                                <asp:Button ID="btnPnlSubir" runat="server" Text="Enviar" OnClick="btnPnlSubir_Click" CssClass="btn btn-outline-primary" />
                                <asp:Button ID="btnClose_FotosUnload" runat="server" Text="Cerrar" OnClick="btnClose_FotosUnload_Click" CssClass="btn btn-outline-secondary" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </ContentTemplate>
            </asp:UpdatePanel>
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

        <div class="form-group">
            <div class="d-grid col-6 mx-auto">
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
                
                <ajaxToolkit:ModalPopupExtender ID="mpeSubirArchivo" runat="server" PopupControlID="pnlSubirArchivo"
                    TargetControlID="lblOcultoSbr" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeSbrArchivo()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblOcultoSbr" runat="server" Text="Label" Style="display: none;" />

                <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                    TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
            </div>
        </div>
        <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPnlSubir" />
            <asp:PostBackTrigger ControlID="btnDescargarZip" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<asp:Content ID="cntSec" ContentPlaceHolderID="cphSec" Runat="Server">--%>
</asp:Content>
