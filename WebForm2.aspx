<%@ Page Title="generar QR´s" Language="C#" MasterPageFile="~/3secciones.Master" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WebItNow.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">

        const img = document.querySelector("img"),
            input = document.querySelector("input"),
            button = document.querySelector("button"),
            api = 'https://api.qrserver.com/v1/',
            api2 = 'create-qr-code/?size=150x150&data=';

        button.addEventListener("click", () => {
            img.src = '${api}${api2}${input.value}';
        });

        function generarQR() {
            const img = document.querySelector("img"),
                input = document.querySelector("input"),
                button = document.querySelector("button"),
                api = 'https://api.qrserver.com/v1/',
                api2 = 'create-qr-code/?size=150x150&data=';

            img.src = '${api}${api2}${input.value}';
        }

    </script>
    
            <div class="contenedor">
                <h1>3 secciones</h1>
                <h3>Generar QR´s</h3>
                <div class="QR">
                    <img src="" width="155" height="155" />
                </div>
                <div class="input_Text">
                    <input type="text" />
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="Btn_Generator">
                            <button runat="server" onclick="generarQR">Generator</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        
</asp:Content>
