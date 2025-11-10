<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaginaPerfilUsuario.aspx.cs" Inherits="PaginaPerfilUsuario" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <title>Tu Perfil - ReviCV</title>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        html, body {
            height: 100%;
            width: 100%;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(to bottom, #a3d4ff, #b3e0ff);
            overflow-x: hidden;
        }

        .contenedor {
            display: flex;
            height: 100%;
            width: 100%;
        }

        .curriculums {
            flex: 1;
            display: flex;
            flex-direction: column;
            gap: 15px;
            padding: 20px;
            overflow-y: auto;
        }

        .curriculum-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background-color: #e8e8e8;
            padding: 15px 20px;
            border-radius: 30px;
            width: 100%;
            height: 80px;
            min-height: 80px;
        }

        .curriculum-titulo {
            font-weight: bold;
            flex-grow: 1;
            padding-left: 10px;
        }

        .agregar-curriculum {
            display: flex;
            align-items: center;
            justify-content: center;
            border: 4px solid white;
            border-radius: 30px;
            height: 80px;
            min-height: 80px;
            cursor: pointer;
            color: orange;
            font-size: 52px;
            font-weight: bold;
            transition: all 0.3s;
        }

            .agregar-curriculum:hover {
                background-color: rgba(255, 255, 255, 0.1);
                transform: scale(1.02);
            }

        .tarjeta-perfil {
            height: 745px;
            width: 450px;
            min-width: 400px;
            background: #f8f9ff;
            margin: 20px;
            border-radius: 25px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
            overflow: hidden;
            display: flex;
            flex-direction: column;
        }

        .encabezado-perfil {
            background: #667eea;
            color: white;
            padding: 30px;
            text-align: center;
            position: relative;
        }

        .nombre-perfil {
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 5px;
            position: relative;
            z-index: 1;
        }

        .usuario-perfil {
            font-size: 14px;
            opacity: 0.9;
            position: relative;
            z-index: 1;
        }

        .contenido-perfil {
            padding: 30px;
            flex: 1;
            display: flex;
            flex-direction: column;
            gap: 25px;
        }

        .tabs-perfil {
            display: flex;
            background-color: #f0f4f8;
            border-radius: 12px;
            margin-bottom: 20px;
        }

        .boton-tab {
            flex: 1;
            padding: 10px 16px;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 500;
            transition: all 0.3s;
        }

            .boton-tab.active {
                background: white;
                color: #667eea;
                box-shadow: 0 2px 8px rgba(102, 126, 234, 0.15);
            }

        .contenido-tab {
            display: none;
        }

            .contenido-tab.active {
                display: block;
            }

        .grupo-input {
            margin-bottom: 20px;
        }

        .form-label {
            display: block;
            margin-bottom: 6px;
            font-weight: 500;
            color: #374151;
            font-size: 14px;
        }

        .form-input {
            width: 100%;
            padding: 12px 16px;
            border: 2px solid #e5e7eb;
            border-radius: 10px;
            font-size: 14px;
            transition: all 0.3s;
            background-color: #ffffff;
        }

        .txt-input {
            position: relative;
        }

        .btn {
            width: 100%;
            padding: 12px 24px;
            border: none;
            border-radius: 10px;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.3s;
            font-size: 14px;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
        }

        .btn-guardar {
            background: #667eea;
            color: white;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
        }

            .btn-guardar:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
            }

        .btn-cancelar {
            background-color: #f3f4f6;
            color: #374151;
            border: 1px solid #d1d5db;
        }

            .btn-cancelar:hover {
                background-color: #e5e7eb;
            }

        .btn-rojo {
            background: #ef4444;
            color: white;
        }

            .btn-rojo:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 15px rgba(239, 68, 68, 0.3);
            }

        .botones-accion {
            display: flex;
            gap: 12px;
            margin-top: 110px;
            margin-bottom: -110px;
        }

        .botones-inferiores {
            padding: 20px;
            display: flex;
            flex-direction: column;
            gap: 12px;
            margin: 0 20px 20px 20px;
        }

        .nombre-perfil,
        .usuario-perfil {
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="contenedor">
            <div class="curriculums scrollbar-hidden">
                <div style="margin-bottom: 10px;">
                    <asp:DropDownList ID="ddlIdiomas" runat="server" Width="160px" Height="45px" CssClass="form-input" />
                    <asp:DropDownList ID="ddlRubros" runat="server" Width="160px" Height="45px" CssClass="form-input" />
                </div>

                <asp:FileUpload ID="fileUpload" runat="server" Style="display: none;" />
                <asp:HiddenField ID="hfNombreArchivo" runat="server" />
                <asp:Button ID="btnSubirArchivo" runat="server" OnClick="btnSubirArchivo_Click" Style="display: none;" />
                <asp:LinkButton ID="btnAbrirCarga" runat="server" OnClientClick="abrirDialogoArchivo(); return false;" CssClass="agregar-curriculum">+</asp:LinkButton>

                <asp:PlaceHolder ID="phCurriculums" runat="server" />
            </div>

            <div class="contenedor-perfil">
                <div class="tarjeta-perfil">
                    <div class="encabezado-perfil">
                        <asp:Label ID="lblNombrePerfil" runat="server" CssClass="nombre-perfil" Text="Juan Gomez" />
                        <asp:Label ID="lblUsuarioPerfil" runat="server" CssClass="usuario-perfil" Text="@UsuarioPrueba123" />
                    </div>


                    <div class="contenido-perfil">

                        <div class="tabs-perfil">
                            <button type="button" class="boton-tab active" onclick="switchTab('profile')">Perfil</button>
                            <button type="button" class="boton-tab" onclick="switchTab('security')">Seguridad</button>
                        </div>


                        <div id="profile-tab" class="contenido-tab active">
                            <asp:Panel ID="profilePanel" runat="server">
                                <div class="grupo-input">
                                    <label class="form-label">Nombre de usuario</label>
                                    <div class="txt-input">
                                        <asp:TextBox ID="username" CssClass="form-input" runat="server" Text="UsuarioPrueba123" />
                                    </div>
                                </div>

                                <div class="grupo-input">
                                    <label class="form-label">Nombre</label>
                                    <div class="txt-input">
                                        <asp:TextBox ID="firstName" CssClass="form-input" runat="server" Text="Juan" />
                                    </div>
                                </div>

                                <div class="grupo-input">
                                    <label class="form-label">Apellido</label>
                                    <div class="txt-input">
                                        <asp:TextBox ID="lastName" CssClass="form-input" runat="server" Text="Gomez" />
                                    </div>
                                </div>

                                <div class="grupo-input">
                                    <label class="form-label">Email</label>
                                    <div class="txt-input">
                                        <asp:TextBox ID="email" CssClass="form-input" runat="server" Text="prueba@gmail.com" TextMode="Email" />
                                    </div>
                                </div>

                                <div class="grupo-input">
    <label class="form-label">Idioma</label>
    <div class="txt-input">
        <asp:DropDownList ID="ddlIdioma" runat="server" CssClass="form-input">
            <asp:ListItem Text="Español" Value="es" />
            <asp:ListItem Text="Inglés" Value="en" />
            <asp:ListItem Text="Portugués" Value="pt" />
        </asp:DropDownList>
    </div>
</div>

                                <div class="botones-accion">
                                    <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-guardar" Text="💾 Guardar Cambios" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-cancelar" Text="✖️ Cancelar" OnClick="btnCancelar_Click" CausesValidation="False" UseSubmitBehavior="false" />
                                </div>
                            </asp:Panel>
                        </div>


                        <div id="security-tab" class="contenido-tab">
                            <asp:Panel ID="securityPanel" runat="server">
                                <div class="grupo-input">
                                    <label class="form-label">Nueva contraseña</label>
                                    <asp:TextBox ID="newPassword" CssClass="form-input" runat="server" TextMode="Password" Placeholder="Ingresa tu nueva contraseña" />
                                </div>

                                <div class="grupo-input">
                                    <label class="form-label">Confirmar nueva contraseña</label>
                                    <asp:TextBox ID="confirmPassword" CssClass="form-input" runat="server" TextMode="Password" Placeholder="Confirma tu nueva contraseña" />
                                </div>

                                <div class="botones-accion">
                                    <asp:Button ID="btnCambiarPassword" runat="server" CssClass="btn btn-rojo" Text="🔑 Cambiar Contraseña" OnClick="btnCambiarPassword_Click" />
                                </div>
                            </asp:Panel>
                        </div>

                    </div>
                </div>

                <div class="botones-inferiores">
                    <asp:Button ID="btnVolverPrincipal" class="btn btn-guardar" runat="server" Text="Volver a la pagina principal" OnClick="btnVolverPrincipal_Click" />
                    <asp:Button ID="btnCerrarSesion" class="btn btn-guardar" runat="server" Text="Cerrar sesión" OnClick="btnCerrarSesion_Click" />
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hfOriginalUsername" runat="server" />
        <asp:HiddenField ID="hfOriginalFirstName" runat="server" />
        <asp:HiddenField ID="hfOriginalLastName" runat="server" />
        <asp:HiddenField ID="hfOriginalEmail" runat="server" />
        <asp:HiddenField ID="hfOriginalDNI" runat="server" />
        <asp:HiddenField ID="hfOriginalRol" runat="server" />

    </form>


    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script>

        function switchTab(tabName) {

            document.querySelectorAll('.boton-tab').forEach(btn => btn.classList.remove('active'));
            document.querySelectorAll('.contenido-tab').forEach(content => content.classList.remove('active'));

            event.target.classList.add('active');
            document.getElementById(tabName + '-tab').classList.add('active');
        }

        function abrirDialogoArchivo() {
            const input = document.getElementById("<%= fileUpload.ClientID %>");
            input.click();

            input.onchange = function () {
                const file = input.files[0];
                if (!file) return;

                const valid = file.type === "application/pdf" || file.type.startsWith("image/");
                if (!valid) {
                    Swal.fire("Archivo inválido", "Solo se permiten PDF o imágenes", "error");
                    input.value = "";
                    return;
                }

                Swal.fire({
                    title: "Nombre del archivo",
                    input: "text",
                    inputLabel: "¿Cómo querés llamarlo?",
                    inputPlaceholder: "Ej: Curriculum Abogacía Blanco y Negro - Inglés",
                    showCancelButton: true,
                    confirmButtonText: "Subir"
                }).then(result => {
                    if (result.isConfirmed && result.value.trim() !== "") {
                        document.getElementById("<%= hfNombreArchivo.ClientID %>").value = result.value.trim();
                            document.getElementById("<%= btnSubirArchivo.ClientID %>").click();
                        } else {
                            input.value = "";
                        }
                    });
            };
        }


    </script>
</body>
</html>
