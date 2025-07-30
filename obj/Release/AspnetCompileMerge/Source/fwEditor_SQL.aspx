<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="fwEditor_SQL.aspx.cs" Inherits="WebItNow_Peacock.fwEditor_SQL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablepageMethods="true">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="container col-lg-8 col-md-9 col-sm-10 py-5">
                <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
                <br /> <br />

                <asp:Button ID="btnExecute" runat="server" Text="Ejecutar Query" Font-Bold="True" OnClick="btnExecute_Click" CssClass="btn btn-primary" TabIndex="0"/>
                <br /><br />

                <asp:GridView ID="gvResults"  runat="server" AutoGenerateColumns="true" GridLines="None" Width="99%"
                    CssClass="table table-responsive table-light table-striped table-hover align-middle" AlternatingRowStyle-CssClass="alt" 
                    Font-Size="Smaller" >
                    <AlternatingRowStyle CssClass="alt" />
                </asp:GridView>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
