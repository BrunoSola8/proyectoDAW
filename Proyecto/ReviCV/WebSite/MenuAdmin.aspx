<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin.aspx.cs" Inherits="MenuAdmin" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <title>Panel de Administración</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />
    <style>
        body {
            margin: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f4f4;
        }

        .navbar {
            background-color: #2c3e50;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 30px;
            height: 60px;
            color: white;
            box-shadow: 0 2px 8px rgba(0,0,0,0.2);
        }

        .navbar .logo {
            font-size: 20px;
            font-weight: bold;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .navbar .menu {
            display: flex;
            gap: 20px;
        }

        .menu-button {
            background: none;
            border: none;
            color: white;
            font-size: 15px;
            cursor: pointer;
            display: flex;
            align-items: center;
            gap: 6px;
            padding: 10px;
            transition: background 0.3s ease;
            text-decoration: none;
        }

        .menu-button:hover {
            background-color: #34495e;
            border-radius: 4px;
        }

        .contenido {
    display: flex;
    justify-content: center;
    align-items: center;
    height: calc(100vh - 60px);
    text-align: center;
}

.centrado h2 {
    font-size: 30px;
    color: #2c3e50;
    margin-bottom: 15px;
}

.centrado p {
    font-size: 18px;
    color: #555;
}

        .contenido h2 {
            color: #2c3e50;
            font-size: 28px;
            margin-bottom: 10px;
        }

        .contenido p {
            color: #555;
            font-size: 16px;
        }

        @media (max-width: 768px) {
            .navbar {
                flex-direction: column;
                align-items: flex-start;
                height: auto;
                padding: 10px 20px;
            }

            .navbar .menu {
                flex-direction: column;
                width: 100%;
            }

            .menu-button {
                width: 100%;
                justify-content: flex-start;
            }

            .contenido {
                padding: 20px;
            }
            .logo span {
    margin-left: 8px;
    font-weight: 600;
    color: #333;
}
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="navbar">
          <div class="logo">
    <i class="fa-solid fa-cogs"></i>
    <span runat="server" data-key="PanelDeAdministracion">Panel de administración</span>
</div>
            <div class="menu">
                <asp:LinkButton ID="btnInicio" runat="server" OnClick="btnInicio_Click" CssClass="menu-button" data-key="Inicio">
                    <i class="fa fa-home"></i> Inicio
                </asp:LinkButton>
                <asp:LinkButton ID="btnUsuarios" runat="server" OnClick="btnUsuarios_Click" CssClass="menu-button" data-key="Usuarios">
                    <i class="fa fa-users"></i> Usuarios
                </asp:LinkButton>
                <asp:LinkButton ID="btnRubrosIdiomas" runat="server" OnClick="btnRubrosIdiomas_Click" CssClass="menu-button" data-key="RubrosIdiomas">
                    <i class="fa fa-language"></i> Rubros e Idiomas
                </asp:LinkButton>
                <asp:LinkButton ID="btnVolverALanding" runat="server" OnClick="btnVolverALanding_Click" CssClass="menu-button" data-key="Volver">
                    <i class="fa fa-arrow-left"></i> Volver
                </asp:LinkButton>
                <asp:LinkButton ID="btnVerPerfilUsuario" runat="server" OnClick="btnVerPerfilUsuario_Click" CssClass="menu-button" data-key="VerPerfilUsuario">
                    <i class="fa fa-user"></i> Ver perfil de usuario
                </asp:LinkButton>
                <asp:LinkButton ID="btnCerrarSesion" runat="server" OnClick="btnCerrarSesion_Click" CssClass="menu-button" data-key="CerrarSesion">
                    <i class="fa fa-sign-out-alt"></i> Cerrar Sesión
                </asp:LinkButton>
            </div>
        </div>
<div class="contenido">
    <div class="centrado">
        <h2 data-key="BienvenidoPanelAdmin" runat="server">Bienvenido al panel de administración</h2>
        <p data-key="SeleccionaUnaOpcDelMenuParaComenzar" runat="server">Seleccioná una opción del menú para comenzar</p>
    </div>
</div>
    </form>
</body>
</html>
