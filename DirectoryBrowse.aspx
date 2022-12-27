<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DirectoryBrowse.aspx.cs" Inherits="WebItNow.DirectoryBrowse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">

		function SelectAndClose()
			{
				txtValue = document.getElementById('_browseTextBox');
		    
				window.returnValue = txtValue.value;
				window.close();
				return false;
			}
		
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table width="100%" border="0">
            <tr>
                <td>
                    <span class="text"><b>Browse directories:</b></span></td>
            </tr>
            <tr>
                <td>
                    <span class="text">example: c:\ or \\server\share or http://server/directory browsing
                        share</span></td>
            </tr>
            <tr>
                <td><asp:Label ID="error" runat="server" CssClass="errorMsg" ></asp:Label>
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="_browseTextBox" runat="server" CssClass="toolbar" Width="300px" /><asp:ImageButton ID="_browseButton" ImageUrl="Images/Go.gif" runat="server" OnClick="_browseButton_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <div class="tableOutlineWt" style="width: 313px; height: 371px; background-color: white">
                        <table cellspacing="0" cellpadding="4" width="100%" bgcolor="#ffffff" border="0">
                            <tr>
                                <td>
                                        
                                        <asp:TreeView ID="TreeView1" runat="server" Height="326px" ImageSet="XPFileExplorer"
                                            NodeIndent="15" Width="292px">
                                            <ParentNodeStyle Font-Bold="False" />
                                            <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                            <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                                                VerticalPadding="0px" />
                                            <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                                                NodeSpacing="0px" VerticalPadding="2px" />
                                            <LeafNodeStyle ImageUrl="~/Images/folder.gif" />
                                        </asp:TreeView>
                                       
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
                <tr>
                <td align="left">
                    <asp:ImageButton ID="_selectButton" ImageUrl="Images/ok.jpg" OnClientClick="SelectAndClose();"
                        runat="server" />
                </td>
            </tr>
        </table>
          
    </div>
</asp:Content>
