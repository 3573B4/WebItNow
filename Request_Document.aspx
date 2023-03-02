    <%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Request_Document.aspx.cs" Inherits="WebItNow.Request_Document" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script language="javascript" type="text/javascript">

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

    $(function () {
        $('[id*=GridView1]').footable();
    });


    document.onkeydown = function (evt) { return (evt ? evt.which : event.keyCode) != 13; }

</script>

    <link href="~/Scripts/footable.min.js" rel="stylesheet" type="text/javascript" />
    <link href="~/Styles/footable.min.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <div class="container col-md-6">
        <h2 class="h2 mb-5 fw-normal">Solicitud de Documentos</h2>
        <div class="form-group mt-3">
            <div class="row">
            <div class="col-md-6">
                <asp:Label ID="LblRef" runat="server" Text="Referencia:" CssClass="control-label col-sm-2"></asp:Label>
                <div class="input-group mt-1 mb-3">
                    <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control" Style="text-transform: uppercase"  AutoComplete="off" MaxLength="12" ></asp:TextBox>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="ImgBusReference" runat="server" CssClass="p-1" ImageUrl="~/Images/search_find.png" Height="32px" Width="32px" OnClick="ImgBusReference_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="col-md-6">
                <asp:Label ID="LblCliente" runat="server" Text="Cliente:" CssClass="control-label col-sm-2"></asp:Label>            
                <div class="input-group mt-1 mb-3">
                    <asp:TextBox ID="TxtCliente" runat="server" CssClass="form-control" Style="text-transform: uppercase"  AutoComplete="off" MaxLength="12" ></asp:TextBox>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="ImgBusCustomer" runat="server" CssClass="p-1" ImageUrl="~/Images/search_find.png" Height="32px" Width="32px" OnClick="ImgBusCustomer_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            </div>
        </div>
        <asp:Panel ID="pnlRef" runat="server" >
            <div style="overflow-x: hidden; overflow-y: auto; height:225px;">
                <asp:GridView ID="GrdRef" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%" AllowPaging = "False"
                    CssClass="footable" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    OnSelectedIndexChanged="GrdRef_SelectedIndexChanged" OnRowDataBound="GrdRef_RowDataBound" >
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgSelect" runat="server" Height="24px" Width="24px" ImageUrl="~/Images/aceptar.ico" OnClick="ImgSelect_Click" />
                            </ItemTemplate> 
                        </asp:TemplateField>

                        <asp:BoundField DataField="UsReferencia" HeaderText="Referencia" ></asp:BoundField>
                        <asp:BoundField DataField="Aseguradora" HeaderText="Aseguradora" ></asp:BoundField>
                        <asp:BoundField DataField="Siniestro" HeaderText="Siniestro" ></asp:BoundField>
                        <asp:BoundField DataField="UsEmail" HeaderText="Email" ></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
    <div class="container col-md-4">
        <div class="form-group  mt-3">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False"  Width="280px" ></asp:Label>
            </div>
        </div>        
        
        <div class="form-group mt-3">
            <div class="d-grid col-6 mx-auto">
            <%--<asp:Button ID="BtnNewReference" runat="server" Text="Referencia Nueva" Font-Bold="True" OnClick="BtnNewReference_Click" CssClass="btn btn-primary" />--%>
            <%--<asp:Button ID="BtnExistReference" runat="server" Text="Referencia Existente" Font-Bold="True" OnClick="BtnExistReference_Click" CssClass="btn btn-primary" />--%>
            </div>
        </div>

        <div class="from-group">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link mb-5 pb-4"/>
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
                <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td><asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" /></td>
            <td></td>
            <td></td>
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
</ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="GrdRef" />
        <asp:PostBackTrigger ControlID="ImgBusReference" />
        <asp:PostBackTrigger ControlID="ImgBusCustomer" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
