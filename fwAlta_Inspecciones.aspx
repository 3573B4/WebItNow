<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwAlta_Inspecciones.aspx.cs" Inherits="WebItNow_Peacock.fwAlta_Inspecciones" %>
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

        function mpeNewProceso() {

        }

        function mpeExpiraOnOk() {

        }

        document.onkeydown = function (evt) {
            return (evt ? evt.which : event.keyCode) != 13;
        }

        function Campo_Obligatorio(source, arguments) {
            var valor = arguments.value;
            if (valor == "-- Seleccionar --") {
                arguments.IsValid = false;
                return;
            } else {
                arguments.IsValid = true;
                return;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="container col-md-4 py-5">
            <div class="form-floating mt-3">
                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">ALTA DE INSPECCIONES</h6>
                </div>

                <div class="form-group mt-4">
                <asp:Panel runat="server" DefaultButton="ImgBusReference">
                    <div class="row">
                        <div class ="mb-2">
                            <asp:Label ID="LblRef_Siniestro" runat="server" Text="Número de Referencia ó Siniestro" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvRef_Siniestro" runat="server"
                            ControlToValidate="TxtRef_Siniestro" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-11">
                            <asp:TextBox ID="TxtRef_Siniestro" runat="server" CssClass="form-control" placeholder="Número Referencia ó Siniestro" AutoComplete="off" onkeyup="mayus(this);" MaxLength="15" TabIndex="1" ></asp:TextBox>
                        </div>
                        <div class="col-1 justify-content-start ps-0 ms-0">
                            <asp:ImageButton ID="ImgBusReference" runat="server" CssClass="p-1" ImageUrl="~/Images/search_find.png" Height="32px" Width="32px" OnClick="ImgBusReference_Click" CausesValidation="false" />
                        </div>
                    </div>
                </asp:Panel>
                </div>

<%--                
                <div class="form-group mt-4">
                    <asp:Label ID="LblRef_Siniestro" runat="server" Text="Número de Referencia ó Siniestro" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvRef_Siniestro" runat="server"
                        ControlToValidate="TxtRef_Siniestro" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtRef_Siniestro" runat="server" CssClass="form-control" placeholder="Número Referencia ó Siniestro" AutoComplete="off" onkeyup="mayus(this);" MaxLength="12" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
--%>

                <div class="form-group mt-4">
                    <asp:Label ID="LblAsegurado" runat="server" Text="Nombre del Asegurado" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
<%--                        
                        <asp:RequiredFieldValidator ID="rfvAsegurado" runat="server"
                        ControlToValidate="TxtNomAsegurado" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
--%>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtNomAsegurado" runat="server" CssClass="form-control" placeholder="Nombre del asegurado" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" TabIndex="2"></asp:TextBox>
                    </div>
                </div>

<%--
                <div class="form-group mt-4">
                    <asp:Label ID="LblSiniestro" runat="server" Text="Numero de Siniestro" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvSiniestro" runat="server"
                        ControlToValidate="TxtSiniestro" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtSiniestro" runat="server" CssClass="form-control" placeholder="Numero de Siniestro" AutoComplete="off" onkeyup="mayus(this);" MaxLength="15" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
--%>

                <div class="form-group mt-4">
                    <asp:Label ID="LblRiesgo" runat="server" Text="Riesgo" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvRiesgo" runat="server"
                        ControlToValidate="TxtRiesgo" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtRiesgo" runat="server" CssClass="form-control" placeholder="Riesgo" AutoComplete="off" onkeyup="mayus(this);" MaxLength="150" TabIndex="4"></asp:TextBox>
                    </div>
                </div>

                <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                    <h6 class="h6 fw-normal my-1" style="font-size:small">DATOS DE LA INSPECCIÓN</h6>
                </div>

                <div class="form-group mt-4">
                    <asp:Label ID="LblFechaProgramada" runat="server" Text="Fecha Programada" CssClass="control-label" Font-Size="Small"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvFechaInput" runat="server"
                        ControlToValidate="TxtFechaInput" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12 md-form md-outline input-with-post-icon datepicker">
                        <asp:TextBox ID="TxtFechaInput" runat="server" TextMode="Date" placeholder="Select date" style="font-size:16px;" class="form-control form-control-sm" TabIndex="5" />
                    </div>
                </div>

                <div class="form-floating mt-3">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="LblHoras" runat="server" Text="Hora Programada"></asp:Label>
                            <asp:CustomValidator ID="cvHoraProgramada" runat="server" ControlToValidate="ddlHoraProgramada" 
                                OnServerValidate="cvHoraProgramada_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic">
                            </asp:CustomValidator>
                            <asp:DropDownList ID="ddlHoraProgramada" runat="server" CssClass="btn btn-outline-secondary text-start mt-1" Width="100%" TabIndex="6">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="LblMinutes" runat="server" Text="Minuto Programado"></asp:Label>
                            <asp:CustomValidator ID="cvMinutes" runat="server" ControlToValidate="ddlMinutes" 
                                OnServerValidate="cvMinutes_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic">
                            </asp:CustomValidator>
                            <asp:DropDownList ID="ddlMinutes" runat="server" CssClass="btn btn-outline-secondary text-start mt-1" Width="100%" TabIndex="7">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <asp:Label ID="LblCalle" runat="server" Text="Calle" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvCalle" runat="server"
                        ControlToValidate="TxtCalle" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtCalle" runat="server" CssClass="form-control" placeholder="Calle" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" TabIndex="8"></asp:TextBox>
                    </div>
                </div>

                <div class="form-floating mt-3">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="LblNumExt" runat="server" Text="Num. Exterior"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvNumExt" runat="server"
                            ControlToValidate="TxtNumExt" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="TxtNumExt" runat="server" CssClass="form-control" placeholder="Num. Exterior" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" TabIndex="9"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="LblNumInt" runat="server" Text="Num. Interior"></asp:Label>
<%--
                            <asp:RequiredFieldValidator ID="rfvNumInt" runat="server"
                            ControlToValidate="TxtNumInt" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
--%>
                            <asp:TextBox ID="TxtNumInt" runat="server" CssClass="form-control" placeholder="Num. Interior" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" TabIndex="10"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <asp:Label ID="LblEstado" runat="server" Text="Estado" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:CustomValidator ID="cvEstado" runat="server" ControlToValidate="ddlEstado" 
                        OnServerValidate ="cvEstado_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="11">
                    </asp:DropDownList>
                </div>

                <div class="form-group mt-4">
                    <asp:Label ID="LblDelegacion" runat="server" Text="Delegación / Municipio" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:CustomValidator ID="cvMunicipios" runat="server" ControlToValidate="ddlMunicipios" 
                        OnServerValidate ="cvMunicipios_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>
                    <asp:DropDownList ID="ddlMunicipios" runat="server" CssClass="btn btn-outline-secondary text-start" OnSelectedIndexChanged="ddlMunicipios_SelectedIndexChanged" AutoPostBack="true" Width="100%" TabIndex="12">
                    </asp:DropDownList>
                </div>

                <div class="row mb-1 mt-4">
                    <div class="col-10" style="padding-left: 14px;">
                        <asp:Label ID="LblCodigoPostal" runat="server" Text="Código Postal" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server"
                        ControlToValidate="TxtCodigoPostal" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-2" style="display:flex; justify-content: end;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="BtnAddCodigoPostal" runat="server" Text="+" OnClick="BtnAddCodigoPostal_Click" Height="24px" Width="24px" Enabled="false" CssClass="btn btn-primary btn-sm justify-content-center align-items-center p-0 m-0" CausesValidation="false"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtCodigoPostal" runat="server" CssClass="form-control" placeholder="Código Postal" AutoComplete="off" onkeyup="mayus(this);" MaxLength="5" Enabled="false" TabIndex="13"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <asp:Label ID="LblColonia" runat="server" Text="Colonia" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvColonia" runat="server"
                        ControlToValidate="TxtColonia" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtColonia" runat="server" CssClass="form-control" placeholder="Colonia" AutoComplete="off" onkeyup="mayus(this);" MaxLength="50" Enabled="false" TabIndex="14"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group mt-4">
                    <asp:Label ID="LblObservaciones_Domicilio" runat="server" Text="Observaciones del domicilio" CssClass="control-label co-sm-2" Font-Size="Small" Font-Bold="False"></asp:Label>
<%--                        
                        <asp:RequiredFieldValidator ID="rfvObservacionesDomicilio" runat="server"
                        ControlToValidate="TxtObservacionesDomicilio" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
--%>
                    <div class="col-sm-12">
                        <asp:TextBox ID="TxtObservacionesDomicilio" runat="server" CssClass="form-control" placeholder="Observaciones del domicilio" AutoComplete="off" onkeyup="mayus(this);" MaxLength="250" TabIndex="15"></asp:TextBox>
                    </div>
                </div>

                <div class="form-floating mt-3">
                    <asp:Label ID="LblTipoInspeccion" runat="server" Text="Tipo de Inspección" ></asp:Label>
<%--                        
                        <asp:RequiredFieldValidator ID="rfvTipoInspeccion" runat="server"
                        ControlToValidate="ddlTipoInspeccion" ErrorMessage="*" Display="Dynamic" ForeColor="Red">
                        </asp:RequiredFieldValidator>
--%>
                        <asp:CustomValidator ID="cvTipoInspeccion" runat="server" ControlToValidate="ddlTipoInspeccion" 
                        OnServerValidate ="cvTipoInspeccion_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>
                    <asp:DropDownList ID="ddlTipoInspeccion" runat="server" CssClass="btn btn-outline-secondary text-start" Width="100%" TabIndex="16">
                    </asp:DropDownList>
                </div>

                <div class="form-floating mt-3">
                    <asp:Label ID="LblResponsable" runat="server" Text="Responsable de la Inspección" ></asp:Label>
<%--                        
                        <asp:RequiredFieldValidator ID="rfvResponsableInspeccion" runat="server"
                        ControlToValidate="ddlResponsableInspeccion" ErrorMessage="*" Display="Dynamic" ForeColor="Red">
                        </asp:RequiredFieldValidator>
--%>
                        <asp:CustomValidator ID="cvResponsableInspeccion" runat="server" ControlToValidate="ddlResponsableInspeccion" 
                        OnServerValidate ="cvResponsableInspeccion_ServerValidate" ErrorMessage="*" ForeColor="Red" Display="Dynamic" >
                        </asp:CustomValidator>

                    <asp:DropDownList ID="ddlResponsableInspeccion" runat="server" CssClass="btn btn-outline-secondary text-start" Width="100%" TabIndex="17">
                    </asp:DropDownList>
                </div>
                <br />
                <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnEnviar" runat="server" Text="Enviar" OnClick="BtnEnviar_Click" CausesValidation="true" CssClass="btn btn-primary px-4" TabIndex="18"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnEnviar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBusReference" />
            <asp:PostBackTrigger ControlID="BtnAddCodigoPostal" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
        <div class=" row justify-content-end" data-bs-theme="dark">
            <div class="col-1">
                <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" CausesValidation="false" />
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
            <asp:Button ID="BtnClose" runat="server" OnClick="BtnClose_Click" Text="Cerrar" CssClass="btn btn-outline-primary" CausesValidation="false"/>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlMensaje_1" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 400px; background-color:#FFFFFF;">
        <div class=" row justify-content-end" data-bs-theme="dark">
            <div class="col-1">
                <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" CausesValidation="false"/>
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
            <asp:Button ID="BtnAceptar" runat="server" OnClick="BtnAceptar_Click" Text="Crear Referencia" CssClass="btn btn-outline-primary mx-1" CausesValidation="false"/>
            <asp:Button ID="BtnCancelar" runat="server" OnClick="BtnCancelar_Click" Text="Cerrar" CssClass="btn btn-outline-secondary mx-1" CausesValidation="false" />
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="PnlCodigoPostal" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; max-width: 100%; width: 800px; background-color:#FFFFFF;">
            <div class="row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" >
                <ContentTemplate>
                    <div class="container">
                        <div>
                            <br />
                            <hr class="dropdown-divider" />
                        </div>
                        <div>
                            <br />
                            <div class="d-flex flex-row mx-4 m-0 p-0">
                                <asp:Label ID="LblCPostal" runat="server" Text="Código Postal" CssClass="form-label my-0 p-0" />
                            </div>
                            <div class="col-sm-6 mx-3">
                                <asp:Panel runat="server" DefaultButton="BtnBuscar">
                                    <asp:TextBox ID="txtCPostal" runat="server" placeholder="Buscar/Código Postal" CssClass="form-control form-control-sm" CausesValidation="false"></asp:TextBox>
                                    <asp:Button ID="BtnBuscar" runat="server" OnClick="BtnBuscar_Click" Style="display: none" CausesValidation="false"/>
                                </asp:Panel>
                            </div>
                            <div class="mb-2">
                                <div style="overflow-x: hidden; overflow-y: auto; max-height: 275px;">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" >
                                    <ContentTemplate>
                                    <asp:GridView ID="grdCodigoPostal" runat="server" AutoGenerateColumns="false" GridLines="None" Width="100%"
                                    CssClass="table table-responsive table-light table-striped table-hover align-middle"
                                    AlternatingRowStyle-CssClass="alt" OnRowCommand="grdCodigoPostal_RowCommand"
                                    Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" >
                                        <AlternatingRowStyle CssClass="alt autoWidth" />
                                        <Columns>
<%--
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChBoxRow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
--%>
                                            <asp:TemplateField HeaderText="Código Postal">
                                                <ItemTemplate>
                                                <%--<asp:LinkButton ID="lnkCPostal" runat="server" CommandArgument='<%# Eval("d_codigo") %>' OnClick="lnkCPostal_Click" UseSubmitBehavior="true" style="display: block; text-align: left;"><%# Eval("d_codigo") %></asp:LinkButton>--%>
                                                    <asp:LinkButton ID="lnkCPostal" runat="server" UseSubmitBehavior="true" style="display: block; text-align: left; text-decoration: none;" CommandName="SelectPostalCode" CausesValidation="false" CommandArgument='<%# Eval("d_codigo") + "|" + Eval("d_asenta") %>'><%# Eval("d_codigo") %></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
<%--                                            
                                            <asp:BoundField DataField="d_codigo" HeaderText="Código Postal" >
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
--%>
                                            <asp:BoundField DataField="d_asenta" HeaderText="Colonia" >
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="d_tipo_asenta" HeaderText="Tpo. Asentamiento" >
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <%--<PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />--%>
                                    </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdCodigoPostal" />
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
                            <%--<asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" CssClass="btn btn-outline-primary btn-sm" />--%>
                            <%--<asp:Button ID="btnClose_Proceso" runat="server" OnClick="btnClose_Proceso_Click" Text="Cerrar" CssClass="btn btn-outline-secondary btn-sm" />--%>
                            <asp:Button ID="btnClose_Proceso" runat="server" OnClick="btnClose_Proceso_Click" Text="Cerrar" CssClass="btn btn-outline-primary btn-sm" CausesValidation="false"/>
                        </div>
                        <br />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnClose_Proceso" />
                </Triggers>
            </asp:UpdatePanel>
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
        </tr>
        <tr>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeExpira" runat="server" PopupControlID="pnlExpira"
                    TargetControlID="lblHide" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeExpiraOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td class="style3"><asp:Label ID="lblHide" runat="server" Text="Label" Style="display: none;" />
            </td>
            <td>
                <ajaxToolkit:ModalPopupExtender ID="mpeNewProceso" runat="server" PopupControlID="PnlCodigoPostal"
                    TargetControlID="LblOculto1" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewProceso()" >
                </ajaxToolkit:ModalPopupExtender>
                <asp:Label ID="LblOculto1" runat="server" Text="Label" Style="display: none;" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
