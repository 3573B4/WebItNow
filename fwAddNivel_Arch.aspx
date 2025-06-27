<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwAddNivel_Arch.aspx.cs" Inherits="WebItNow_Peacock.fwAddNivel_Arch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    
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
        
        function mpeAddMensajeOnOk() {
            //
        }

        function mpeNewEnvio() {
            //
        }


        document.onkeydown = function (evt) {
            return (evt ? evt.which : event.keyCode) != 13;
        }

        function cerrarModal() {
            //$("[id*='mpeVerIntegComis']").css("display", "none");
            $("#pnlMensaje").css("display", "none");

            return false;
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
        //graficar con google
        // Load the Visualization API and the corechart package.
        google.charts.load('current', { 'packages': ['corechart'] });

        // Set a callback to run when the Google Visualization API is loaded.
        google.charts.setOnLoadCallback(drawChart);

        // Callback that creates and populates a data table,
        // instantiates the pie chart, passes in the data and
        // draws it.
        function drawChart() {

            // Create the data table.
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Topping');
            data.addColumn('number', 'Slices');
            data.addRows([
                ['Mushrooms', 4],
                ['Onions', 8],
                ['Olives', 10],
                ['Zucchini', 11],
                ['Pepperoni', 20]
            ]);

            // Set chart options
            var options = {
                'title': 'How Much Pizza I Ate Last Night'/*,
                'width': 900,
                'height': 700*/
            };

            // Instantiate and draw our chart, passing in some options.
            var chart = new google.visualization.PieChart(document.getElementById('chart'));
            chart.draw(data, options);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container col-lg-7 col-md-8 col-sm-8">
                <div class="row mb-2 py-4">
                    <div class="col-lg-4 col-md-4 ">
                    </div>
                    <div class="col-lg-8 col-md-8">
                        <div class="input-group input-group-sm">
                            <h2 class="h2 mb-3 fw-normal mt-4 align-content-center">Personalizar Cuadernos</h2>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 w-100" style="/*background-color: white;*/ 
                 box-sizing: border-box; font-family: 'poppons', sans-serif;
                 width:100%; height:100vh; display:flex; justify-content:center; align-content:center;">

                <div class="container col-lg-7 col-md-8 col-sm-8">

                    <div class="row mb-2 py-4">
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblAseguradora" runat="server" Text="Aseguradora" CssClass="control-label" Font-Size="Medium" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlAseguradora" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlAseguradora_SelectedIndexChanged" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>  

                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblConclusion" runat="server" Text="Tipo de Cuaderno" CssClass="control-label" Font-Size="Medium" ></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlConclusion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlConclusion_SelectedIndexChanged" visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblRegimen" runat="server" Text="Tipo de Asegurado" CssClass="control-label" Font-Size="Medium"></asp:Label>
                            </div>
                            <div class="input-group input-group-sm">
                                <asp:DropDownList ID="ddlRegimen" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlRegimen_SelectedIndexChanged" visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4 py-0">
                        <div class="col-lg-4 col-md-4 ">
                            <div class ="mb-2">
                                <asp:Label ID="LblTpoAsunto" runat="server" Text="Tipo de Asunto" CssClass="control-label" Font-Size="Medium" ></asp:Label>
                            </div>
                            <div class=" input-group input-group-sm">
                                <asp:DropDownList ID="ddlTpoAsunto" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlTpoAsunto_SelectedIndexChanged" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
<%--
                    <div class="row col-lg-9 col-md-11 mb-3 ps-3" style="width: 100%; padding-top:60px;">
                        <div class="col-md-6 mb-3"  >
                            <div class="row">
                                <div class="col-12 mb-2">
                                    <asp:Label ID="LblAseguradora" runat="server" Text="Aseguradora" CssClass="control-label mb-3" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-12">
                                    <asp:DropDownList ID="ddlAseguradora" runat="server" OnSelectedIndexChanged="ddlAseguradora_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-2" style="display:flex; align-items:end; justify-content:end;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddAseguradora" runat="server" Text="+" OnClick="BtnAddAseguradora_Click" CssClass="btn btn-primary btn-sm" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mb-3">
                            <div class="row">
                                <div class="col-12 mb-2">
                                    <asp:Label ID="LblConclusion" runat="server" Text="Tipo de Cuaderno" CssClass="control-label mb-2" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-12">
                                    <asp:DropDownList ID="ddlConclusion" runat="server" OnSelectedIndexChanged="ddlConclusion_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%" >
                                    </asp:DropDownList>
                                </div>
                                <div class="col-2" style="display:flex; align-items:end; justify-content:end;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddConclusion" runat="server" Text="+" OnClick="BtnAddConclusion_Click" CssClass="btn btn-primary btn-sm" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
--%>
                    <div class="row col-lg-9 col-md-11 mb-0 ps-0" style="width:100%">
                        <div class="col-md-6 mb-3">
                            <div class="row py-2" style="align-items: baseline;">  <%--center--%>
                                <div class="col-8" style="padding-left: 14px;">
                                    <asp:Label ID="LblNivel1" runat="server" Text="Crear carpeta primaria" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-4" style="display:flex; justify-content: end;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddNivel1" runat="server" Text="+" OnClick="BtnAddNivel1_Click" Height="30px" Width="25px" CssClass="btn btn-primary btn-sm" />
                                            <asp:Button ID="BtnDelNivel1" runat="server" Text="-" OnClick="BtnDelNivel1_Click" Height="30px" Width="25px" CssClass="btn btn-primary btn-sm" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="col-12">
                                <asp:DropDownList ID="ddlNivel1" runat="server" OnSelectedIndexChanged="ddlNivel1_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>
                                
                            <div class="row pt-3 pb-2" style="align-items: baseline;">  <%--center--%>
                                <div class="col-8" style="padding-left: 14px;">
                                    <asp:Label ID="LblNivel2" runat="server" Text="Crear carpeta secundaria" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-4" style="display:flex; justify-content: end;">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddNivel2" runat="server" Text="+" OnClick="BtnAddNivel2_Click" Height="30px" Width="25px" CssClass="btn btn-primary btn-sm" />
                                            <asp:Button ID="BtnDelNivel2" runat="server" Text="-" OnClick="BtnDelNivel2_Click" Height="30px" Width="25px" CssClass="btn btn-primary btn-sm" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="col-12">
                                <asp:DropDownList ID="ddlNivel2" runat="server" OnSelectedIndexChanged="ddlNivel2_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>
<%--                                
                            <div class="row pt-3 pb-2" style="align-items: baseline;">
                                <div class="col-10" style="padding-left: 14px;">
                                    <asp:Label ID="LblNivel3" runat="server" Text="Nivel 3" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-2" style="display:flex; justify-content: end;">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>     
                                            <asp:Button ID="BtnAddNivel3" runat="server" Text="+" OnClick="BtnAddNivel3_Click" CssClass="btn btn-primary btn-sm" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="col-12">
                                <asp:DropDownList ID="ddlNivel3" runat="server" OnSelectedIndexChanged="ddlNivel3_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>

                            
                            <div class="row pt-3 pb-2" style="align-items: baseline;">
                                <div class="col-10" style="padding-left: 14px;">
                                    <asp:Label ID="LblNivel4" runat="server" Text="Nivel 4" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-2" style="display:flex; justify-content: end;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddNivel4" runat="server" Text="+" OnClick="BtnAddNivel4_Click" CssClass="btn btn-primary btn-sm" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-12">
                                <asp:DropDownList ID="ddlNivel4" runat="server" OnSelectedIndexChanged="ddlNivel4_SelectedIndexChanged" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" Visible="true" Width="100%">
                                </asp:DropDownList>
                            </div>
--%>
                            <div id="chartPBA" class="col-12" style="width:100%; height:100px;"></div>
                        </div>

                        <div class="col-md-6 mb-3">
                            <div class="row py-2" style="align-items: baseline;">  <%--center--%>
                                <div class="col-10" style="padding-left: 14px;">
                                    <asp:Label ID="LblArchivos" runat="server" Text="Agregar tipo de documento a carpeta" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-2" style="display:flex; justify-content: end;">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnArchivos" runat="server" Text="+" OnClick="BtnArchivos_Click" CssClass="btn btn-primary btn-sm" />
                                            <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div style="overflow-x: auto; overflow-y:hidden;">
                                <asp:GridView ID="GridArchivosPaq" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                        AllowPaging="true" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt"
                                        PageSize="10"
                                        OnDataBound="GridArchivosPaq_DataBound" OnRowDataBound="GridArchivosPaq_RowDataBound" OnSelectedIndexChanged="GridArchivosPaq_SelectedIndexChanged"
                                        OnSelectedIndexChanging="GridArchivosPaq_SelectedIndexChanging">

                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:BoundField DataField="IdArchivo" HeaderText="Id" >
                                            <ItemStyle Width="50" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="nomArchivo" HeaderText="Nombre" >
                                            <ItemStyle Width="250" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Tipo Documento" >
                                            <ItemStyle Width="100" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DrescRegimen" HeaderText="Regimen" >
                                            <ItemStyle Width="100" />
                                        </asp:BoundField>

                                        <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgAceptado" runat="server" OnClick="ImgAceptado_Click" Height="26px" Width="26px" ImageUrl="~/Images/aceptar_new.png" Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgRechazado" runat="server" OnClick="ImgRechazado_Click" Height="26px" Width="26px" ImageUrl="~/Images/rechazar_new.png" Enabled="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:BoundField DataField="IdDirectorio">
                                            <%--<ItemStyle Width="0px" Font-Size="0pt" />--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IdRegimen">
                                            <%--<ItemStyle Width="0px" Font-Size="0pt" />--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IdSeccion">
                                            <%--<ItemStyle Width="0px" Font-Size="0pt" />--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IdTpoArchivo">
                                            <%--<ItemStyle Width="0px" Font-Size="0pt" />--%>
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="row py-2" style="align-items: baseline;">  <%--center--%>
                                <div class="col-10" style="padding-left: 14px;">
                                    <asp:Label ID="LblAddRegimen" runat="server" Text="Regimen" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-2" style="display:flex; justify-content: end;">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddRegimen" runat="server" Text="+" OnClick="BtnAddRegimen_Click" CssClass="btn btn-primary btn-sm" />
                                            <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="row py-2" style="align-items: baseline;">  <%--center--%>
                                <div class="col-10" style="padding-left: 14px;">
                                    <asp:Label ID="LblAddCarpeta" runat="server" Text="Carpeta" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="col-2" style="display:flex; justify-content: end;">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddDirectorio" runat="server" Text="+" OnClick="BtnAddDirectorio_Click" CssClass="btn btn-primary btn-sm" />
                                            <%--<asp:ImageButton ID="ImageButton1" runat="server" />--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>
                    </div>
<%--                        
                    <div class="form-group">
                        <div class="d-grid col-6 mx-auto">
                            <ajaxtoolkit:modalpopupextender id="ModalPopupExtender1" runat="server" popupcontrolid="pnlMensaje"
                                targetcontrolid="lblOculto" backgroundcssclass="FondoAplicacion" onokscript="mpeMensajeOnOk()">
                            </ajaxtoolkit:modalpopupextender>
                            <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" />
                        </div>
                    </div>
--%>
                </div>

            </div>

            <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" Style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
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
                    <asp:Button ID="BtnClose" runat="server" OnClientClick="return cerrarModal();" Text="Cerrar" CssClass="btn btn-outline-primary" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlExpira" runat="server" CssClass="CajaDialogo" Style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
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
                <div >
                    <asp:Button ID="BtnClose_Expira" OnClientClick="acceso(); return false;" runat="server" Text="Cerrar" CssClass="btn btn-outline-primary" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlAdd" runat="server" CssClass="CajaDialogo" Style="display: none; border: none; border-radius: 10px; width: 400px; background-color: #FFFFFF;">
                <div class="row" data-bs-theme="dark">
                    <div class="col-9" style="display:flex;">
                        <asp:Label ID="LblTitlePnlAdd" runat="server" Text="" Visible="true" ></asp:Label>
                    </div>
                    <div class="col-2 me-o pe-0" style="display:flex; justify-content:end;">
                        <asp:Button runat="server" OnClick="Unnamed_Click" type="button" class="btn-close" aria-label="Close" />
                    </div>
                </div>
                <div>
                    <br />
                    <hr class="dropdown-divider" />
                </div>
                <div class="" style="display:flex; flex-direction:column; justify-content:start;">
<%--
                    <div class="mb-0 pb-0" style="align-items:start; justify-content:start;">
                        <asp:Label ID="LblPnlMsgFiscal" runat="server" Text="Regimen fiscal" Visible="false"></asp:Label>
                    </div>
                    <div class="mt-1">
                        <asp:DropDownList ID="ddlPnlMsgFiscal" runat="server" CssClass="btn btn-outline-secondary text-start" Visible="false" Width="100%">
                        </asp:DropDownList>
                    </div>
--%>
                    <div class="mb-0 pb-0" style="align-items:start; justify-content:start;">
                        <asp:Label ID="LblDdlPnlMsgAdd" runat="server" Text="Drow drop List" Visible="false"></asp:Label>
                    </div>
                    <div class="mt-1">
                        <%--AutoPostBack="true"--%>
                        <asp:DropDownList ID="ddlPnlMsAdd" runat="server" CssClass="btn btn-outline-secondary text-start" Visible="false" Width="100%">

                        </asp:DropDownList>
                    </div>
                    <div class="mb-0 pb-0" style="align-items:start; justify-content:start;">
                        <asp:Label ID="LblDdlPnlMsgTpDoc" runat="server" Text="Tipo de documento" Visible="false"></asp:Label>
                    </div>
                    <div class="mt-1">
                        <asp:DropDownList ID="ddlPnlMsgTpDoc" runat="server" CssClass="btn btn-outline-secondary text-start" Visible="false" Width="100%">

                        </asp:DropDownList>
                    </div>
                    <div class="mb-0 pb-0" style="align-items:start; justify-content:start;">
                        <asp:Label ID="LblPnlMsgSeccion" runat="server" Text="Seccion a mostrar" Visible="false"></asp:Label>
                    </div>
                    <div class="mt-1">
                        <asp:DropDownList ID="ddlPnlMsgSeccion" runat="server" CssClass="btn btn-outline-secondary text-start" Visible="false" Width="100%">

                        </asp:DropDownList>
                    </div>
                    <div class="mb-0 pb-0">
                        <asp:Label ID="LblMsgPnlAdd" runat="server" Text="" />
                    </div>
                    <div class="mt-1">
                        <asp:TextBox ID="TxtPnlMsgAdd" runat="server" CssClass="form-control form-control-sm" AutoComplete="off" ></asp:TextBox>
                    </div>
                    <div class="mb-0 pb-0">
                        <asp:Label ID="LblPnlAdd" runat="server" Text="" Visible="false" />
                    </div>
                </div>
                <div>
                    <br />
                    <hr class="dropdown-divider" />
                </div>
                <div class="" style="display:flex; justify-content:space-evenly;">
                    <asp:Button ID="BtnPnlMsgAceptar" runat="server" Text="Aceptar" OnClick="BtnPnlMsgAceptar_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="BtnClousePnlAdd" OnClientClick="Unnamed_Click" runat="server" Text="Cerrar" CssClass="btn btn-outline-secondary" />
                        
                </div>
            </asp:Panel>

            <div class="form-group">
                <div class="d-grid col-6 mx-auto">
                    <ajaxtoolkit:modalpopupextender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                        TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()">
                    </ajaxtoolkit:modalpopupextender>
                    <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />

<%--                        
                    <ajaxtoolkit:modalpopupextender ID="mpeNewEnvio" runat="server" PopupControlID="PnlEnvioArchivos"
                        TargetControlID="LblOculto2" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewEnvio()">
                    </ajaxtoolkit:modalpopupextender>
                    <asp:Label ID="LblOculto2" runat="server" Text="Label" Style="display: none;" />
--%>

                    <ajaxToolkit:ModalPopupExtender ID="mpeAddMensaje" runat="server" PopupControlID="pnlAdd"
                        TargetControlID="lblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeAddMensajeOnOk">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Label ID="lblOculto3" runat="server" Text="label" Style="display: none;" />
                    <%--
                        <ajaxToolkit:ModalPopupExtender ID="mpeNewNota" runat="server" PopupControlID="PnlNewNotas"
                            TargetControlID="LblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewNota()" >
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Label ID="LblOculto3" runat="server" Text="Label" Style="display: none;" />
                    --%>
                </div>
            </div>

            <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
            </ajaxToolkit:ModalPopupExtender>
            <asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
            
            <%--
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3" DynamicLayout="true">
                <ProgressTemplate>
                <div id="divImage" class ="loading">
                    <div class="center">
                        <img alt="Processing..." src="Images\ajax-loader.gif" />
                    </div>
                </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            --%>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="BtnAddAseguradora" />--%>
            <%--<asp:PostBackTrigger ControlID="BtnAddConclusion" />--%>
            <asp:PostBackTrigger ControlID="BtnAddNivel1" />
            <asp:PostBackTrigger ControlID="BtnAddNivel2" />
            <%--<asp:PostBackTrigger ControlID="BtnAddNivel3" />--%>
            <%--<asp:PostBackTrigger ControlID="BtnAddNivel4" />--%>
            <asp:PostBackTrigger ControlID="BtnDelNivel1" />
            <asp:PostBackTrigger ControlID="BtnDelNivel2" />

            <asp:PostBackTrigger ControlID="BtnArchivos" />
            <asp:PostBackTrigger ControlID="GridArchivosPaq" />
            <asp:PostBackTrigger ControlID="BtnPnlMsgAceptar" />
            <asp:PostBackTrigger ControlID="BtnAddRegimen" />
            <asp:PostBackTrigger ControlID="BtnAddDirectorio" />

            <%--<asp:PostBackTrigger ControlID="BtnLogin" />--%>
            <%--<asp:PostBackTrigger ControlID="BtnCardLogout" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
