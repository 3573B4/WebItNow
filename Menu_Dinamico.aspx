<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Menu_Dinamico.aspx.cs" Inherits="WebItNow.Menu_Dinamico" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
<%--    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:Menu ID="MyMenu" runat="server" Height="32px" Orientation="Vertical" Width="380px"
        MaximumDynamicDisplayLevels ="2" BackColor="#FFFBD6"
        DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em"
        ForeColor="#990000" StaticSubMenuIndent="10px">
        
        <DynamicHoverStyle BackColor="#990000" ForeColor="White" />
        <DynamicMenuItemStyle HorizontalPadding="10px" VerticalPadding="4px" />
        <DynamicMenuStyle BackColor="#FFFBD6" />
        <DynamicSelectedStyle BackColor="#FFCC66" />

        <StaticHoverStyle BackColor="#990000" ForeColor="White" />
        <StaticMenuItemStyle HorizontalPadding="10px" VerticalPadding="4px" />
        <StaticSelectedStyle BackColor="#FFCC66" />
    </asp:Menu>--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
