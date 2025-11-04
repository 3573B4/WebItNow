<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwAgenda_Inspecciones.aspx.cs" Inherits="WebItNow_Peacock.fwAgenda_Inspecciones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>

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

        <div class="container col-md-8 col-lg-7 py-5">
            <div class="row ">
                <div class="row mb-3">
                    <div class="col-lg-4 col-md-4 ">
                        <div class ="mb-2">
                            <asp:Label ID="LblFechaProgramada" runat="server" Text="Fecha Programada" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="md-form md-outline input-with-post-icon datepicker">
                            <asp:TextBox ID="DateInput" runat="server" TextMode="Date" AutoPostBack="true" placeholder="Select date" style="font-size:16px;" class="form-control form-control-sm" OnTextChanged="DateInput_TextChanged" />
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <asp:Label ID="LblResponsable" runat="server" Text="Responsable Inspección" CssClass="control-label" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="input-group">
                            <asp:DropDownList ID="ddlResponsable" runat="server" style="font-size:16px;" CssClass="btn btn-outline-secondary text-start" AutoPostBack="true" OnSelectedIndexChanged="ddlResponsable_SelectedIndexChanged" visible="true" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class ="mb-2">
                            <br />
                        </div>
                        <div class="input-group">
                            <asp:Button ID="BtnAgendarCita" runat="server" Text="Agendar Citas" OnClick="BtnAgendarCita_Click" CssClass="btn btn-primary btn-sm px-4 " />
                        </div>
                    </div>
                </div>

            </div>

            <div class="row mb-3 mt-4" style="background-color:#96E7D9;">
                <h6 class="h6 fw-normal my-1" style="font-size:small">CONTROL DE INSPECCIONES</h6>
            </div>

            <%-- Agenda control de inspecciones  --%>
            <div style="overflow-x: auto; overflow-y:hidden">
                <asp:GridView ID="GrdArch_Agenda"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%"
                        AllowPaging="True" CssClass="table table-responsive table-light table-striped table-hover align-middle" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                        OnPageIndexChanging="GrdArch_Agenda_PageIndexChanging" OnRowCommand="GrdArch_Agenda_RowCommand"
                        OnSelectedIndexChanged="GrdArch_Agenda_SelectedIndexChanged"
                        OnRowDataBound ="GrdArch_Agenda_RowDataBound" DataKeyNames="Fecha_Programada" PageSize="8" Font-Size="Smaller" Visible="false" >
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:BoundField DataField="Fecha_Programada" HeaderText="Fecha" >
                            <ItemStyle Width="150px" /> 
                            </asp:BoundField>
                            <asp:BoundField DataField="Hora" HeaderText="Horario" >
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Ref_Siniestro" HeaderText="Referncia / Siniestro" >
                            <ItemStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nom_Responsable" HeaderText="Responsable Inspección" >
                            <ItemStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NomTpo_Inspeccion" HeaderText="Tipo Inspección" >
                            <ItemStyle Width="300px" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>

            <div>
                <daypilot:daypilotcalendar 
	                id="DayPilotCalendar1" 
	                runat="server" 
                    DataStartField="eventstart" 
	                DataEndField="eventend"
	                DataTextField="Ref_Siniestro" 
	                DataValueField="Id_Inspecciones" 
	                Days="7"
	                EventMoveHandling="Disabled"
	                ClientObjectName="dpc" 
                    HourFontSize="10pt" 
                    LoadingLabelFontSize="8pt" 
                    BusinessBeginsHour="08" 
                    BusinessEndsHour="23" 
                    HeaderDateFormat="dd/MM/yyyy" 
                    CellHeight="30"
                    HeightSpec="BusinessHoursNoScroll" >
                </daypilot:daypilotcalendar>
            </div>

        </div>
        </ContentTemplate>
        <Triggers>

        </Triggers>
    </asp:UpdatePanel>
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
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
