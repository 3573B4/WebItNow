<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Expediente.aspx.cs" Inherits="WebItNow.Expediente" %>
<%--<%@ Page Title="" Language="C#" MasterPageFile="~/3secciones.Master" AutoEventWireup="true" CodeBehind="Expediente.aspx.cs" Inherits="WebItNow.Expediente" %>--%>

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

        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress1.ClientID %>");
            updateProgress.style.display = "block";
        }

    </script>

    <script type="text/javascript">
        var _validFilejpeg = [".jpeg", ".jpg", ".bmp", ".pdf", ".zip", ".rar", ".msg"];

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

    <style type="text/css">
        .Initial
        {
          display: block;
          padding: 4px 18px 4px 18px;
          float: left;
          background: url("../Images/InitialImage.png") no-repeat right top;
          color: Black;
          font-weight: bold;
        }
        .Initial:hover
        {
          color: White;
          background: url("../Images/SelectedButton.png") no-repeat right top;
        }
        .Clicked
        {
          float: left;
          display: block;
          background: url("../Images/SelectedButton.png") no-repeat right top;
          padding: 4px 18px 4px 18px;
          color: Black;
          font-weight: bold;
          color: White;
        }

        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .8em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left:10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
          width: 150px !important;    
        }
        #divwidth div
       {
        width: 150px !important;   
       }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <%--    initializeRequest y endRequest  --%>
    <script type="text/javascript">  

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        // Agregar initializeRequest y endRequest
        prm.add_initializeRequest(prm_InitializeRequest);
        prm.add_endRequest(prm_EndRequest);

        // Llamado cuando comienza la devolución de datos asíncrona
        function prm_InitializeRequest(sender, args) {
            // Obtener el divImage y configurarlo en visible
            var panelProg = $get('divImage');
            panelProg.style.display = '';

            // Deshabilitar el botón que provocó una devolución de datos
            $get(args._postBackElement.id).disabled = true;
        }

        // Llamado cuando finaliza la devolución de datos asincrónica
        function prm_EndRequest(sender, args) {
            // obtener el divImage y ocultarlo de nuevo
            var panelProg = $get('divImage');
            panelProg.style.display = 'none';

            // Habilitar el botón que provocó una devolución de datos
            // $get(sender._postBackSettings.sourceElement.id).disabled = false;
        }

    </script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <br />
    
    <div class="container col-md-8 col-lg-7"">
        <div class="d-grid gap-4 d-flex justify-content-lg-start mt-2 mb-3">
            <asp:Button ID="BtnNotas" runat="server" Text="Notas" OnClick="BtnNotas_Click" CssClass="btn btn-outline-primary px-4" Enabled="false" />
            <asp:Button ID="BtnDocumentos" runat="server" Text="Documentos" OnClick="BtnDocumentos_Click" CssClass="btn btn-outline-primary" Enabled="false" />
            <asp:Button ID="BtnFotos" runat="server" Text="Fotos" OnClick="BtnFotos_Click" CssClass="btn btn-outline-primary" Enabled="false" />
        </div>
        <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
            <h6 class="h6 fw-normal my-1"> </h6>
        </div>
    </div>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="viewReport" runat="server">
            <div class="container col-md-8 col-lg-7">
                <div class="row mt-3">
                    <div class="row col-lg-8 col-md-7 col-sm-7 py-0">
                        <div class="col-3 py-0 pt-2">
                            <asp:Label ID="LblReferencia" runat="server" Text="Referencia" CssClass="form-label"></asp:Label>
                        </div>
                        <div class="col-4">
                            <div class="input-group">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="TxtReferencia" runat="server" OnTextChanged="TxtReferencia_TextChanged" AutoPostBack="true" CssClass="form-control form-control-sm" placeholder="Busqueda" MaxLength="12" autocomplete="off"  Style="text-transform: uppercase" ></asp:TextBox>
                                        <asp:Panel ID="pnlElementosLista" runat="server" Width="600px" style="background-color:#FFFFFF;">
                                            <asp:GridView ID="grdPanelBusqueda" runat="server" AutoGenerateColumns="False" GridLines="None" Width="600px"
                                                AllowPaging="True" CssClass="table table-hover table-sm " PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
                                                PageSize="10" OnRowCommand="grdPanelBusqueda_RowCommand" OnPageIndexChanging="grdPanelBusqueda_PageIndexChanging"
                                                OnRowDataBound="grdPanelBusqueda_RowDataBound" OnSelectedIndexChanged="grdPanelBusqueda_SelectedIndexChanged">
                                                <AlternatingRowStyle CssClass="table" />
                                                <Columns>
                                                    <asp:BoundField DataField="UsReferencia" HeaderText="Referencia" />
                                                    <asp:BoundField DataField="Num_Siniestro" HeaderText="No de Siniestro" />
                                                    <asp:BoundField DataField="Num_Reporte" HeaderText="No de Reporte" />
                                                    <asp:BoundField DataField="Nombre" HeaderText="Asegurado" />
                                                    <asp:BoundField DataField="Num_Poliza" HeaderText="No de Poliza" />
                                                </Columns>
                                                <PagerStyle CssClass="pgr" />
                                            </asp:GridView>
                                        </asp:Panel>
                                        <ajaxToolkit:DropDownExtender ID="DropDownExtender1" runat="server" DropDownControlID="pnlElementosLista" TargetControlID="txtReferencia" ></ajaxToolkit:DropDownExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-4">
                        <asp:UpdatePanel ID="UpdatePanel" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="BtnBuscar" runat="server" Text="&#128270;" Font-Bold="True" OnClick="BtnBuscar_Click" CssClass="btn btn-primary btn-sm" />
                                <%--<asp:ImageButton ID="btnImgBuscar" runat="server" ImageUrl="Images/icon_buscar.png" Font-Bold="True" OnClick="BtnBuscar_Click" CssClass="btn btn-primary btn-sm" />--%>
                                <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" CssClass="btn btn-outline-secondary  btn-sm" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row col-lg-4 col-md-5 col-sm-5 me-0 pe-0 mt-2">
                        <div class="col-5 py-0 my-0">
                            <asp:Label ID="LblBusquedaAsegurado" runat="server" Text="Asegurado: " CssClass="form-label"></asp:Label>
                        </div>
                        <div class="col-7">
                            <asp:TextBox ID="TxtBusqNombre" runat="server" BorderStyle="None" Font-Size="Small" ></asp:TextBox>
                        </div>
                    </div>
                </div>

<%--
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div class="row mb-3">
                        <div class="form-group">
                            <div class="d-grid col-2 mx-auto">
                                <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" Font-Bold="True" OnClick="BtnBuscar_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
--%>

            <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
                <h6 class="h6 fw-normal my-1" >Aseguradora</h6>
            </div>

            <div class="row mt-3">
                <div class=" col-lg-6 col-md-6">
                    <div class="mb-2">
                        <asp:Label ID="LblAseguradora" runat="server" Text="Aseguradora" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlAseguradora" runat="server" CssClass="form-select" >
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblDivision" runat="server" Text="División" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnNewDivision" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">

                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblAnalista" runat="server" Text="Analista" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtAnalista" runat="server" CssClass="form-control form-control-sm" placeholder="Analista" ></asp:TextBox>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblAtencion" runat="server" Text="Atención a" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlAtencion" runat="server" CssClass="form-select" >
                        </asp:DropDownList>
                        <asp:Button ID="btnNewAtencion" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>

            </div>

            <div class="row mt-3">
                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblEventCatastrofico" runat="server" Text="Evento Catastrofico" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlEventCatastrofico" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:Button ID="btnNewEventCatastrofico" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblProceso" runat="server" Text="Proceso" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlProceso" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlProceso_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnNewProceso" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblSubProceso" runat="server" Text="SubProceso" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlSubProceso" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSubProceso_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnNewSubProceso" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblCobertura" runat="server" Text="Cobertura" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlCobertura" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:Button ID="btnNewCobertura" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 ">
                    <div class="mb-2">
                        <asp:Label ID="LblRiesgo" runat="server" Text="Riesgo" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlRiesgo" runat="server" CssClass="form-select" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:Button ID="btnNewRiesgo" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">

                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblNumSiniestro" runat="server" Text="No. Siniestro" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNumSiniestro" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>

                <div class="col-lg-4 col-md-4">
                    <div class="mb-2">
                        <asp:Label ID="LblNumReporte" runat="server" Text="No. Reporte" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNumReporte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>

                <div class="col-lg-4 col-md-4">
                    <div class="mb-2">
                        <asp:Label ID="LblFechaAseguradora" runat="server" Text="Fecha a Aseguradora" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtFechaAseguradora" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" id="BtnFechaAseguradora">
                            <span class="visually-hidden">Toggle Dropdown</span>
                        </button>
                        <ajaxToolkit:CalendarExtender ID="dateAseguradora" runat="server" TargetControlID="TxtFechaAseguradora" PopupButtonID="BtnFechaAseguradora" Format="dd/MM/yyyy" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-lg-6 col-md-6">
                    <div class="mb-2">
                        <asp:Label ID="LblDelSiniestro" runat="server" Text="Fecha del Siniestro" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtFechaSiniestro" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        <button type="button" id="BtnFechaSiniestro" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false">
                            <span class="visually-hidden">Toggle Dropdown</span>
                        </button>
                        <ajaxToolkit:CalendarExtender ID="dateSiniestro" runat="server" TargetControlID="TxtFechaSiniestro" PopupButtonID="BtnFechaSiniestro"  Format="dd/MM/yyyy"/>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6">
                    <div class="mb-2">
                        <asp:Label ID="LblRefAsegurado" runat="server" Text="Referencia del Asegurado" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtRefAsegurado" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>

            </div>

            <div class="row mt-3">

                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblReporto" runat="server" Text="Quién Reportó?" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtReporto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>

                <div class="col-lg-4 col-md-4">
                    <div class="mb-2">
                        <asp:Label ID="LblFechAsignacion" runat="server" Text="Fecha de Asignación" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtFechaAsignacion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        <button type="button" id="BtnFechaAsignacion" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false" >
                            <span class="visually-hidden">Toggle Dropdown</span>
                        </button>
                        <ajaxToolkit:CalendarExtender ID="dateAsignacion" runat="server" TargetControlID="TxtFechaAsignacion" PopupButtonID="BtnFechaAsignacion"  Format="dd/MM/yyyy"/>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4">
                    <div class="mb-2">
                        <asp:Label ID="LblHorAsignacion" runat="server" Text="Hora de Asignación" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtHorAsignacion" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox>
                        <asp:Label ID="LblHoras" runat="server" Text="Hrs" CssClass="form-label mx-3 mt-2 mb-0"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblNumPoliza" runat="server" Text="No. Póliza" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtNumPoliza" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblIniciaVigencia" runat="server" Text="Inicia Vigencia" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtIniciaVigencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        <button type="button" id="BtnIniciaVigencia" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false">
                            <span class="visually-hidden">Toggle Dropdown</span>
                        </button>
                        <ajaxToolkit:CalendarExtender ID="dateIniciaVigencia" runat="server" TargetControlID="TxtIniciaVigencia" PopupButtonID="BtnIniciaVigencia"  Format="dd/MM/yyyy"/>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 ">
                    <div class="mb-2">
                        <asp:Label ID="LblTerminaVigencia" runat="server" Text="Termina Vigencia" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtTerminaVigencia" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        <button type="button" id="BtnTerminaVigencia" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown" aria-expanded="false">
                            <span class="visually-hidden">Toggle Dropdown</span>
                        </button>
                        <ajaxToolkit:CalendarExtender ID="dateTerminaVigencia" runat="server" TargetControlID="TxtTerminaVigencia" PopupButtonID="BtnTerminaVigencia"  Format="dd/MM/yyyy"/>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12 col-md-12 ">
                    <div class="mb-2">
                        <asp:Label ID="LblCausaSiniestro" runat="server" Text="Tipo de daño / Causa del Siniestro" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtCausaSiniestro" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-7 col-md-7 ">
                    <div class="mb-2">
                        <asp:Label ID="LblEstimacion" runat="server" Text="Estimación:" CssClass="form-label form-label-sm my-2 me-3"></asp:Label>
                    </div>
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtEstimacion" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-5 col-md-5">
                    <div class="input-group input-group-sm">
                        <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="form-select form-select-sm mt-4">
                        </asp:DropDownList>
                        <asp:Button ID="BtnNewMoneda" runat="server" Text="Nuevo" CssClass="btn btn-outline-secondary mt-4" />
                    </div>
                </div>
            </div>

            <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
                <h6 class="h6 fw-normal my-1" >Asegurado</h6>
            </div>

                <div class="row mb-3">
                    <div class="col-lg-6 col-md-6">
                        <div class ="mb-2">
                            <asp:Label ID="LblAsegurado" runat="server" Text="Asegurado"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlAsegurado" runat="server" CssClass="btn btn-outline-secondary text-start mt-1" AutoPostBack="true" OnSelectedIndexChanged="ddlAsegurado_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6">
                        <div class ="mb-2">
                            <asp:Label ID="LblSegundoAsegurado" runat="server" Text="Segundo Asegurado"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlSegundoAsegurado" runat="server" CssClass="btn btn-outline-secondary text-start mt-1" AutoPostBack="true" OnSelectedIndexChanged="ddlSegundoAsegurado_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTelefono" runat="server" Text="Telefonos" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="">
                            <asp:TextBox ID="TxtTelefono_1" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Telefono 1" AutoComplete="off" MaxLength="10" ></asp:TextBox>
                            <asp:TextBox ID="TxtTelefono_2" runat="server" CssClass="form-control form-control-sm" placeholder="Telefono 2" AutoComplete="off" MaxLength="10" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCelular" runat="server" Text="Celular o Radio" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCelular" runat="server" CssClass="form-control form-control-sm" placeholder="Celular o Radio" AutoComplete="off" MaxLength="10" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblGrupo" runat="server" Text="Grupo"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlGrupo" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
                    <h6 class="h6 fw-normal my-1" >Dirección</h6>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCodigoPostal" runat="server" Text="Codigo Postal" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="">
                            <asp:TextBox ID="TxtCodigoPostal" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Codigo Postal" AutoComplete="off" MaxLength="5" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblEstado" runat="server" Text="Estado" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblDelegacion" runat="server" Text="Deleg/Mpio"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlDelegacion" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlDelegacion_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblColonia" runat="server" Text="Colonia"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtColonia" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Colonia" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCalle" runat="server" Text="Calle"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCalle" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Calle" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
                    <h6 class="h6 fw-normal my-1" >Dirección Afectada</h6>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCodigoPostalAfectado" runat="server" Text="Codigo Postal" CssClass="control-label col-sm-2" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="">
                            <asp:TextBox ID="TxtCodigoPostalAfectado" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Codigo Postal" AutoComplete="off" MaxLength="5" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblEstadoAfectado" runat="server" Text="Estado" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlEstadoAfectado" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoAfectado_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblDelegacionAfectada" runat="server" Text="Deleg/Mpio"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlDelegacionAfectada" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlDelegacionAfectada_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblColoniaAfectada" runat="server" Text="Colonia"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtColonia_Afectada" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Colonia" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCalleAfectada" runat="server" Text="Calle"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCalle_Afectada" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Calle" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-12 col-md-12">
                        <div class ="mb-2">
                            <asp:Label ID="LblObservaciones" runat="server" Text="Observaciones del domicilio"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtObservaciones" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Entre que calles, cerca de..." AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblEntrevistar" runat="server" Text="Entrevistar a"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEntrevistar" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Entrevistar a" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblPuesto_1" runat="server" Text="Puesto"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPuesto_1" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Puesto" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTel_1" runat="server" Text="Teléfono"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTel_1" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Telefono" AutoComplete="off" MaxLength="10" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblEmail_1" runat="server" Text="Email"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEmail_1" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Email" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblDocumentara" runat="server" Text="¿Quién Documentará?"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtDocumentara" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblPuesto_2" runat="server" Text="Puesto"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtPuesto_2" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Puesto" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblTel_2" runat="server" Text="Teléfono"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtTel_2" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Telefono" AutoComplete="off" MaxLength="10" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblEmail_2" runat="server" Text="Email"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtEmail_2" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Email" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblLegal" runat="server" Text="Rep. Legal"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtLegal" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Legal" AutoComplete="off" MaxLength="50" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
                    <h6 class="h6 fw-normal my-1" >Ajustador</h6>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblLocal" runat="server" Text="Local / Foráneo" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlLocal" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlLocal_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblEmpresa" runat="server" Text="Empresa"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblOficina" runat="server" Text="Oficina" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlOficina" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlOficina_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="lblAjustador" runat="server" Text="Ajustador"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlAjustador" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlAjustador_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblCorredor" runat="server" Text="Corredor o Agente"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlCorredor" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlCorredor_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblAgente" runat="server" Text="Agente Contacto"></asp:Label>
                        </div>
                        <div class="form-floating">
                            <asp:DropDownList ID="ddlAgente" runat="server" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlAgente_SelectedIndexChanged" Width="100%" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-12 col-md-12">
                        <div class ="mb-2">
                            <asp:Label ID="LblObservaciones_2" runat="server" Text="Observaciones especiales del caso"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtObservaciones_2" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-lg-12 col-md-12">
                        <div class ="mb-2">
                            <asp:Label ID="LblAntecedentes" runat="server" Text="Antecedentes del Siniestro"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtAntecedentes" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-lg-12 col-md-12">
                        <div class ="mb-2">
                            <asp:Label ID="LblCausa" runat="server" Text="Causa"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtCausa" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-lg-12 col-md-12">
                        <div class ="mb-2">
                            <asp:Label ID="LblBienes" runat="server" Text="Bienes Afectados"></asp:Label>
                        </div>
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="TxtBienes" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="" AutoComplete="off" MaxLength="2500" Columns="12" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                <ContentTemplate>
                    <div class="row">
                        <div class="form-group">
                            <div class="d-grid col-2 mx-auto">
                                <asp:Button ID="BtnActualizar" runat="server" Text="Actualizar" Font-Bold="True" OnClick="BtnActualizar_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:View>
    </asp:MultiView>
        <%--mb-4 pb-5 al div de abajo --%>
    <div class="from-group ">
        <div class="d-grid col-6 mx-auto">
            <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link" Visible="false" />
        </div>
    </div>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="BtnBuscar" />
        <asp:PostBackTrigger ControlID="BtnActualizar" />
    </Triggers>
</asp:UpdatePanel>

    <div class="form-group">
        <div class="d-grid col-6 mx-auto">
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlMensaje"
                TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
            </ajaxToolkit:ModalPopupExtender>
            <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" />
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
                    </div>
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
        <div id="divImage" class ="loading">
            <div class="center">
                <img alt="Processing..." src="Images\ajax-loader.gif" />
            </div>
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
