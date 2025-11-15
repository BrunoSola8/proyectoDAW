<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin_Permisos.aspx.cs" Inherits="MenuAdmin_Permisos" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />
    <title>PA - Gestión de Permisos</title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        html, body {
            margin: 0;
            padding: 0;
            height: 100vh;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            overflow-x: hidden;
            background-color: #f4f4f4;
        }

        .contenedor {
            padding: 0;
            max-width: 100%;
            margin: 0;
            box-sizing: border-box;
            min-height: 100vh;
            display: flex;
            flex-direction: column;
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

        .contenido-principal {
            flex: 1;
            padding: 20px;
            background-color: #f4f4f4;
            display: flex;
            flex-direction: column;
            gap: 20px;
            width: 100%;
            box-sizing: border-box;
            min-height: calc(100vh - 60px);
        }

        h2 {
            margin: 0 0 20px 0;
            color: #2c3e50;
        }

        .seccion-superior {
            display: flex;
            gap: 20px;
        }

        .columna {
            flex: 1;
            background: white;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 6px;
            display: flex;
            flex-direction: column;
        }

            .columna h3 {
                margin-top: 0;
                margin-bottom: 15px;
                color: #2c3e50;
                font-size: 16px;
            }

        .campo-select {
            width: 100%;
            padding: 8px;
            margin-bottom: 10px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .boton {
            padding: 8px 12px;
            border: none;
            border-radius: 4px;
            color: white;
            cursor: pointer;
            margin-bottom: 8px;
            font-size: 14px;
            transition: opacity 0.3s ease;
        }

            .boton:disabled {
                opacity: 0.5;
                cursor: not-allowed;
            }

        .boton-rojo {
            background-color: #c0392b;
        }

            .boton-rojo:hover:not(:disabled) {
                background-color: #a93226;
            }

        .boton-azul {
            background-color: #2980b9;
        }

            .boton-azul:hover:not(:disabled) {
                background-color: #1c598a;
            }

        .lista-permisos {
            border: 1px solid #ccc;
            border-radius: 4px;
            padding: 10px;
            min-height: 274px;
            max-height: 274px;
            overflow-y: auto;
            background: white;
        }

        .item-permiso {
            padding: 5px;
            margin-bottom: 5px;
            display: flex;
            align-items: center;
            gap: 8px;
        }

            .item-permiso input[type="checkbox"] {
                cursor: pointer;
            }

            .item-permiso label {
                cursor: pointer;
                flex: 1;
            }

        .arbol-permisos {
            border: 1px solid #ccc;
            border-radius: 4px;
            padding: 10px;
            min-height: 274px;
            max-height: 274px;
            overflow-y: auto;
            background: white;
        }

        .nodo-arbol {
            margin-left: 20px;
            margin-bottom: 5px;
        }

            .nodo-arbol.raiz {
                margin-left: 0;
            }

        .campo-texto {
            width: 100%;
            padding: 8px;
            margin-bottom: 10px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .seccion-inferior {
            display: flex;
            gap: 20px;
        }

        .seccion-botones {
            flex: 1;
            background: white;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 6px;
        }

        .boton-grande {
            width: 100%;
            padding: 12px;
            margin-bottom: 10px;
        }

        .boton-verde {
            background-color: #27ae60;
        }

            .boton-verde:hover:not(:disabled) {
                background-color: #229954;
            }

        .boton-ayuda {
            position: fixed;
            bottom: 20px;
            right: 20px;
            width: 50px;
            height: 50px;
            border-radius: 50%;
            background-color: #2980b9;
            color: white;
            border: none;
            font-size: 24px;
            font-weight: bold;
            cursor: pointer;
            box-shadow: 0 4px 8px rgba(0,0,0,0.3);
            transition: background-color 0.3s ease;
        }

            .boton-ayuda:hover {
                background-color: #1c598a;
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

            .seccion-superior,
            .seccion-inferior {
                flex-direction: column;
            }
        }
    </style>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="contenedor">
            <div class="navbar">
                <div class="logo">
                    <i class="fa-solid fa-cogs"></i>
                    Panel de administración
                </div>
                <div class="menu">
                    <asp:LinkButton ID="btnInicio" runat="server" OnClick="btnInicio_Click" CssClass="menu-button">
                        <i class="fa fa-home"></i> Inicio
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnUsuarios" runat="server" OnClick="btnUsuarios_Click" CssClass="menu-button">
                        <i class="fa fa-users"></i> Usuarios
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnRubrosIdiomas" runat="server" OnClick="btnRubrosIdiomas_Click" CssClass="menu-button">
                        <i class="fa fa-language"></i> Rubros e Idiomas
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnVolverALanding" runat="server" OnClick="btnVolverALanding_Click" CssClass="menu-button">
                        <i class="fa fa-arrow-left"></i> Volver
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnCerrarSesion" runat="server" OnClick="btnCerrarSesion_Click" CssClass="menu-button">
                        <i class="fa fa-sign-out-alt"></i> Cerrar Sesión
                    </asp:LinkButton>
                </div>
            </div>

            <div class="contenido-principal">
                <h2>Gestión de Permisos</h2>

                <div class="seccion-superior">
                    <div class="columna">
                        <h3>Roles y Grupos</h3>
                        <asp:DropDownList ID="ddlRolesGrupos" runat="server" CssClass="campo-select" AutoPostBack="true" OnSelectedIndexChanged="ddlRolesGrupos_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar seleccionado" CssClass="boton boton-rojo" Enabled="false" OnClick="btnEliminar_Click" />
                        <asp:Button ID="btnModificarNombre" runat="server" Text="Modificar Nombre" CssClass="boton boton-azul" Enabled="false" OnClick="btnModificarNombre_Click" />
                    </div>

                    <asp:UpdatePanel ID="upPermisosAsignados" runat="server" UpdateMode="Conditional" class="columna">
                        <ContentTemplate>
                            <h3>Permisos Asignados</h3>
                            <div class="lista-permisos">
                                <asp:CheckBoxList ID="chkListPermisos" runat="server"
                                    CssClass="lista-permisos-checkbox"
                                    AutoPostBack="true" OnSelectedIndexChanged="chkListPermisos_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlRolesGrupos" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="upVisualizacion" runat="server" UpdateMode="Conditional" class="columna">
                        <ContentTemplate>
                            <h3>Visualización</h3>
                            <div class="arbol-permisos">
                                <asp:TreeView ID="treeViewPermisos" runat="server" CssClass="arbol-permisos-tree">
                                </asp:TreeView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlRolesGrupos" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="seccion-inferior">
                    <div class="seccion-botones">
                        <h3>Nuevo rol o grupo de permisos</h3>
                        <asp:TextBox ID="txtNuevoNombre" runat="server" CssClass="campo-texto" placeholder="Ingrese el nuevo nombre"></asp:TextBox>
                        <asp:Button ID="btnCrearRol" runat="server" Text="Crear Rol" CssClass="boton boton-verde boton-grande" OnClick="btnCrearRol_Click" />
                        <asp:Button ID="btnCrearGrupo" runat="server" Text="Crear Grupo de Permisos" CssClass="boton boton-verde boton-grande" OnClick="btnCrearGrupo_Click" />
                    </div>

                    <div class="seccion-botones">
                        <h3>Acciones</h3>
                        <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar Cambios" CssClass="boton boton-azul boton-grande" Enabled="true" OnClick="btnGuardarCambios_Click" />
                    </div>
                </div>
            </div>
            <asp:Button ID="btnConfirmarCambio" runat="server" OnClick="btnConfirmarCambio_Click" Style="display:none;" />
            <asp:HiddenField ID="hfNuevoNombre" runat="server" />
            <asp:Button ID="btnEliminarConfirmar" runat="server" 
            OnClick="btnEliminarConfirmar_Click" 
            Style="display:none;" />



        </div>
    </form>
    <script>
        document.addEventListener("DOMContentLoaded", function () {

            const ddl = document.getElementById("<%= ddlRolesGrupos.ClientID %>");
            const btnEliminar = document.getElementById("<%= btnEliminar.ClientID %>");
            const btnModificar = document.getElementById("<%= btnModificarNombre.ClientID %>");

            ddl.addEventListener("change", function () {
                const haySeleccion = ddl.value !== "";

                btnEliminar.disabled = !haySeleccion;
                btnModificar.disabled = !haySeleccion;
            });

        });
</script>
</body>
</html>
