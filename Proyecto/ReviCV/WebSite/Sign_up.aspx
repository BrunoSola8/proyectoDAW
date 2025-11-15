<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sign_up.aspx.cs" Inherits="Sign_up" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Registro de Usuario - ReviCV</title>
    <link href="Estilos_css\signup.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Registro de Usuario</h2>

            <div class="form-group">
                <label for="TbDNI">DNI</label>
                <asp:TextBox ID="TbDNI" runat="server" />
                <asp:RequiredFieldValidator ID="rfvDNI" runat="server"
                    ControlToValidate="TbDNI"
                    ErrorMessage="* Campo obligatorio"
                    ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="TbNombre">Nombre</label>
                <asp:TextBox ID="TbNombre" runat="server" />
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                    ControlToValidate="TbNombre"
                    ErrorMessage="* Campo obligatorio"
                    ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="TbApellido">Apellido</label>
                <asp:TextBox ID="TbApellido" runat="server" />
                <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                    ControlToValidate="TbApellido"
                    ErrorMessage="* Campo obligatorio"
                    ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="TbUserName">Username</label>
                <asp:TextBox ID="TbUserName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvUserName" runat="server"
                    ControlToValidate="TbUserName"
                    ErrorMessage="* Campo obligatorio"
                    ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="TbMail">Mail</label>
                <asp:TextBox ID="TbMail" runat="server" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvMail" runat="server"
                    ControlToValidate="TbMail"
                    ErrorMessage="* Campo obligatorio"
                    ForeColor="Red" Display="Dynamic" />
                
            </div>
          <div class="form-group">
   <label for="ddlIdioma">Idioma</label>
<asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdown-input">
        <asp:ListItem Text="Español" Value="es" />
        <asp:ListItem Text="Inglés" Value="en" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvIdioma" runat="server"
        ControlToValidate="DropDownList1"
        InitialValue=""
        ErrorMessage="* Campo obligatorio"
        ForeColor="Red" Display="Dynamic" />
</div>

            <div class="form-group">
                <label for="TbPassw1">Contraseña</label>
                <asp:TextBox ID="TbPassw1" runat="server" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvPass1" runat="server"
                    ControlToValidate="TbPassw1"
                    ErrorMessage="* Campo obligatorio"
                    ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="TbPassw2">Repetir Contraseña</label>
                <asp:TextBox ID="TbPassw2" runat="server" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvPass2" runat="server"
                    ControlToValidate="TbPassw2"
                    ErrorMessage="* Campo obligatorio"
                    ForeColor="Red" Display="Dynamic" />

                <asp:CompareValidator ID="cvPass" runat="server"
                    ControlToValidate="TbPassw2"
                    ControlToCompare="TbPassw1"
                    ErrorMessage="* Las contraseñas no coinciden"
                    ForeColor="Red" Display="Dynamic" />

                <asp:Label ID="ErrorContraseñasLB" runat="server" Text="" />
            </div>

            <div class="form-group">
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Confirmar" />
            </div>

<div class="form-group">
    <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" CausesValidation="false" />
</div>
        </div>
    </form>
</body>
</html>