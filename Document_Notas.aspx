<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Document_Notas.aspx.cs" Inherits="WebItNow.Document_Notas" %>
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

        function mpeNewEnvio() {
            //
        }

        function mpeNewNota() {
            //
        }

        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress1.ClientID %>");
            updateProgress.style.display = "block";
        }

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
            <div class="row mb-3 mt-4"style="background-color:mediumturquoise;">
                <h6 class="h6 fw-normal my-1"> </h6>
            </div>
            <div class="container col-md-8 col-lg-7"">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                <ContentTemplate>
                    <div class="row mb-3 mt-4">
                        <div class="form-group">
                            <div class="d-grid gap-4 d-flex justify-content-center mt-2 mb-3">
                                <asp:Button ID="BtnDocument_Nuevo" runat="server" Text="Nuevo Documento" Font-Bold="True" OnClick="BtnDocument_Nuevo_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
            <div style="overflow-x: auto; overflow-y:hidden">
                <asp:GridView ID="GrdNotas"  runat="server" AutoGenerateColumns="False" GridLines="None" Width="1050px"
                        AllowPaging="True" CssClass="footable" PagerStyle-CssClass="pagination-ys" AlternatingRowStyle-CssClass="alt" 
                        OnPageIndexChanging="GrdNotas_PageIndexChanging" OnRowCommand="GrdNotas_RowCommand"
                        OnSelectedIndexChanged="GrdNotas_SelectedIndexChanged"
                        OnRowDataBound ="GrdNotas_RowDataBound" DataKeyNames="IdTpoNota" PageSize="8" >
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgEnvioArchivo" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/envia_doc.png" OnClick="ImgEnvioArchivo_Click" Enabled="true" />
                                </ItemTemplate> 
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgVerArchivo" runat="server" Height="26px" Width="26px" ImageUrl="~/Images/visor_doc.jpg" OnClick="ImgVerArchivo_Click" Enabled="true" />
                                </ItemTemplate> 
                            </asp:TemplateField>
                            <asp:BoundField DataField="Desc_Nota" HeaderText="Tipo de Nota" >
                            <ItemStyle Width="150px" /> 
                            </asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Notas" >
                            <ItemStyle Width="450px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fec_Entrega" DataFormatString="{0:d}" HeaderText="Fecha de entrega" >
                            <ItemStyle Width="150" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IdUsuario" HeaderText="Usuario" >
                            <ItemStyle Width="300" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Url_Imagen"  >
                            <ItemStyle Width="0px" Font-Size="0pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nom_Imagen"  >
                            <ItemStyle Width="0px" Font-Size="0pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IdNota"  >
                            <ItemStyle Width="0px" Font-Size="0pt" />
                            </asp:BoundField>
                            
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>
            <div class="">
                <asp:HiddenField ID="hdfValorGrid" runat="server" Value=""/>
                <br />
                <div class="d-grid col-6 mx-auto">
                    <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
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
        <asp:Panel ID="PnlEnvioArchivos" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; height: 180px; width: 600px; background-color:#FFFFFF;">
            <div class=" row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>

            <div class="form-group">
                <div class="input-group mx-auto pt-3">
                    <input id="oFile" type="file" name="oFile" onchange="validateForSize(this,1,122880);" class="form-control" />
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
        <asp:Panel ID="PnlNewNotas" runat="server" CssClass="CajaDialogo" style="display: none; border: none; border-radius: 10px; width: 600px; background-color:#FFFFFF; text-justify:auto;">
            <div class=" row justify-content-end" data-bs-theme="dark">
                <div class="col-1">
                    <asp:Button runat="server" type="button" class="btn-close" aria-label="Close" />
                </div>
            </div>
                <div class="d-flex flex-row mx-4 m-0 p-0">
                    <asp:Label ID="LblTpoDocumento" runat="server" Text="Tipo de nota" CssClass="form-label my-0 p-0"></asp:Label>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                    <div class="col-10">
                        <div class="input-group input-group-sm">
                            <asp:DropDownList ID="ddlTpoNota" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlTpoNota_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="d-flex flex-row mx-4 m-0 p-0">
                    <asp:Label ID="LblNota" runat="server" Text="Nota" CssClass="form-label my-0 p-0"></asp:Label>
                </div>
                <div class="col-10">
                    <div class="input-group input-group-sm">
                        <asp:TextBox ID="TxtDescNota" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox>
                    </div>
                </div>
            <div class="row mt-0">
                <br />
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnAceptar_New" runat="server" OnClick="BtnAceptar_New_Click" Text="Enviar" CssClass="btn btn-outline-primary" />
                        <asp:Button ID="BtnCerrar_New" runat="server" OnClick="BtnCerrar_New_Click" Text="Cerrar" CssClass="btn btn-outline-secondary" />
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

                            <ajaxToolkit:ModalPopupExtender ID="mpeNewNota" runat="server" PopupControlID="PnlNewNotas"
                                TargetControlID="LblOculto3" BackgroundCssClass="FondoAplicacion" OnOkScript="mpeNewNota()" >
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Label ID="LblOculto3" runat="server" Text="Label" Style="display: none;" />

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
            <asp:PostBackTrigger ControlID="GrdNotas" />
            <asp:PostBackTrigger ControlID="BtnEnviar" />
            <asp:PostBackTrigger ControlID="BtnAceptar_New" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
