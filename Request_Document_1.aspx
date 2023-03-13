<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Request_Document_1.aspx.cs" Inherits="WebItNow.Request_Document_1" %>
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<br />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

<div class="container col-md-4">
        <h2 class="h2 mb-3 fw-normal mt-4">Solicitud de Documentos</h2>
    
        <div class="form-group mt-3">
            <asp:Label ID="LblReferencia" runat="server" Text="Referencia" CssClass="control-label col-sm-2"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control" placeholder="Referencia" ReadOnly="true" ></asp:TextBox>
            </div>
        </div>

        <div class="form-group mt-3">
            <asp:Label ID="LblEmail" runat="server" Text="Correo electrónico" CssClass="control-label col-sm-2"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" placeholder="Correo electrónico" ReadOnly="true" ></asp:TextBox>
            </div>
        </div>   

        <div class="form-group mt-3">
            <asp:Label ID="LblNom" runat="server" Text="Nombre de Cliente o Asegurado" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtNom" runat="server" CssClass="form-control" placeholder="Nombre de Cliente o Destinatario" ReadOnly="true" ></asp:TextBox>
            </div>
        </div>

        <div class="form-group mt-3">
            <asp:Label ID="LblProceso" runat="server" Text="Proceso" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtProceso" runat="server" CssClass="form-control" placeholder="Proceso" ReadOnly="true" ></asp:TextBox>
            </div>
        </div>

        <div class="form-group mt-3">
            <asp:Label ID="LblSubProceso" runat="server" Text="SubProceso" CssClass="control-label co-sm-3" Font-Bold="False"></asp:Label>
            <div class="col-sm-12">
                <asp:TextBox ID="TxtSubProceso" runat="server" CssClass="form-control" placeholder="SubProceso" ReadOnly="true" ></asp:TextBox>
            </div>
        </div>
        
        <div class="form-group border-bottom mt-3 mb-4">
            <asp:Label ID="lblNewDocumento" runat="server" Text="Agregar nuevo documento" CssClass="control-label co-sm-3 mb-1" Font-Bold="False"></asp:Label>
            <div class="col-sm-12 mt-1">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlTpoDocumentNew" runat="server" >
                    <div style="overflow-x: hidden; overflow-y: auto; height:120px;">
                    <asp:GridView ID="GrdTpoDocumentNew" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%" AllowPaging = "False"
                    CssClass="footable" AlternatingRowStyle-CssClass="alt" OnRowDataBound="GrdTpoDocumentNew_RowDataBound" >
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgAgregar" runat="server" Height="20px" Width="20px" ImageUrl="~/Images/add-new-page_icon.png" OnClick="ImgAgregar_Click" />
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:BoundField DataField="IdTpoDocumento" HeaderText="Id" ></asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" ></asp:BoundField>
                    </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
        </div>

        <div class="form-group border-bottom mt-4 mb-3">
            <div class="col-sm-12">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:Panel ID="pnlTpoDocumento" runat="server" >
                <div style="overflow-x: hidden; overflow-y: auto; height:120px;">
                    <asp:GridView ID="GrdTpoDocumento" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%"
                        CssClass="footable" AlternatingRowStyle-CssClass="alt" >
                        <%--OnRowDataBound="GrdTpoDocumento_RowDataBound" OnRowDeleting="GrdTpoDocumento_RowDeleting" >--%>
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgEliminar" runat="server" Height="20px" Width="20px" ImageUrl="~/Images/delete-page-icon.png" OnClick="ImgEliminar_Click" />
                                </ItemTemplate> 
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkTpoDocumento" runat="server" Checked='<%# Convert.ToBoolean(Eval("IdStatus")) %>' OnCheckedChanged="chkTpoDocumento_CheckedChanged" AutoPostBack="True" />
                                </ItemTemplate> 
                            </asp:TemplateField>

                            <asp:BoundField DataField="IdTpoDocumento" HeaderText="Id" ></asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" ></asp:BoundField>
                            <asp:BoundField DataField="IdProceso" >
                                <ItemStyle Width="0px" Font-Size="0pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IdSubProceso" >
                                <ItemStyle Width="0px" Font-Size="0pt" />
                            </asp:BoundField>
                            <%--<asp:CommandField ShowDeleteButton="True" ButtonType="Button" />--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            </ContentTemplate>
            </asp:UpdatePanel>
        </ div>
        </div>
        <div class="form-group mt-3">
            <div class="d-grid col-6 mx-auto">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Visible="False"  Width="280px" ></asp:Label>
            </div>
        </div>
    
<%--        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="form-group mt-3">
                    <div class="d-grid col-6 mx-auto">
                        <asp:Button ID="BtnEnviar" runat="server" Text="Solicitar" Font-Bold="True" OnClick="BtnEnviar_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
<%--            </ContentTemplate>
        </asp:UpdatePanel>--%>

        <div class="from-group mb-4 pb-5">
            <div class="d-grid col-6 mx-auto">
                <asp:Button ID="BtnRegresar" runat="server" Text="Regresar" Font-Bold="True" OnClick="BtnRegresar_Click" CssClass="btn btn-link"/>
            </div>
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
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlMensaje"
                    TargetControlID="lblOculto" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeMensajeOnOk()" >
                </ajaxToolkit:ModalPopupExtender>
            </td>
            <td><asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;" /></td>
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

    </ContentTemplate>
    <Triggers>
        <%--<asp:PostBackTrigger ControlID="GrdTpoDocumentNew" />--%>
        <%--<asp:PostBackTrigger ControlID="GrdTpoDocumento" />--%>
        <%--<asp:PostBackTrigger ControlID="BtnEnviar" />--%>
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
