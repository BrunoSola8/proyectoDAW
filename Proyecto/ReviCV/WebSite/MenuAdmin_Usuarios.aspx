<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin_Usuarios.aspx.cs" Inherits="MenuAdmin_Usuarios" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />
    <title>PA - Gestión de Usuarios</title>
    <style>
        html, body {
    margin: 0;
    padding: 0;
    height: 100vh; /* Cambiar de 100% a 100vh */
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    overflow-x: hidden; /* Evitar scroll horizontal */
    background-color: #f4f4f4;
}

.contenedor {
    padding: 0; /* Quitar el padding de 30px */
    max-width: 100%;
    margin: 0;
    box-sizing: border-box;
    min-height: 100vh; /* Asegurar altura completa */
    display: flex;
    flex-direction: column;
}

.barra-lateral {
    width: 250px;
    background-color: #2c3e50;
    padding-top: 20px;
}

.boton-icono {
    background: none;
    border: none;
    cursor: pointer;
    color: #2980b9;
    font-size: 16px;
}

    .boton-icono:hover {
        color: #1c598a;
    }

.boton-enlace {
    display: block;
    padding: 15px 20px;
    color: white;
    background: none;
    border: none;
    width: 100%;
    text-align: left;
    font: inherit;
    cursor: pointer;
    text-decoration: none;
}

    .boton-enlace:hover {
        background-color: #34495e;
    }

.contenido-principal {
    flex: 1;
    padding: 20px; /* Reducir padding de 30px a 20px */
    background-color: #f4f4f4;
    display: flex;
    flex-direction: column;
    gap: 30px;
    width: 100%;
    box-sizing: border-box;
    min-height: calc(100vh - 60px); /* Restar altura del navbar */
}

.seccion-tabla {
    height: 100%;
    min-height: 300px;
    overflow-y: auto;
    border: 1px solid #ccc;
    background: white;
    padding: 20px;
    border-radius: 6px;
    box-sizing: border-box;
}

.estilo-tabla {
    width: 100%;
    border-collapse: collapse;
}

    .estilo-tabla th {
        background-color: #2c3e50;
        color: white;
        text-align: left;
        padding: 10px;
    }

    .estilo-tabla td {
        padding: 8px;
        border-bottom: 1px solid #ddd;
    }

.fila-normal {
    background-color: white;
    transition: background-color 0.3s ease;
}

.fila-seleccionada {
    background-color: #d6eaff !important;
    font-weight: bold;
}

.seccion-inferior {
    display: flex;
    gap: 20px;
}

.seccion-abm, .seccion-filtro {
    flex: 1;
    background: white;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 6px;
}

.campo-formulario {
    width: 98%;
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
    margin-right: 10px;
}

.boton-verde {
    background-color: #27ae60;
}

.boton-azul {
    background-color: #2980b9;
}

.boton-rojo {
    background-color: #c0392b;
}

.boton-gris {
    background-color: #7f8c8d;
}

h2, h3 {
    margin-top: 0;
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
}
    </style>
</head>
<body>
    <form runat="server">
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
                <h2>Listado de Usuarios</h2>
                <div class="seccion-tabla">
                    <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False"
                        CssClass="estilo-tabla" Width="100%">
                        <RowStyle CssClass="fila-normal" />
                        <SelectedRowStyle CssClass="fila-seleccionada" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <button type="button" class="boton-icono select-user-btn">
                                        <i class="fa fa-arrow-right"></i>
                                    </button>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="DNI" HeaderText="DNI" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Rol" HeaderText="Rol" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="seccion-inferior">
                    <div class="seccion-abm">
                        <h3>Creación de usuarios</h3>
                        <asp:TextBox ID="txtDni" runat="server" CssClass="campo-formulario" ClientIDMode="Static" placeholder="DNI" />
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="campo-formulario" ClientIDMode="Static" placeholder="Nombre" />
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="campo-formulario" ClientIDMode="Static" placeholder="Apellido" />
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="campo-formulario" ClientIDMode="Static" placeholder="Nombre de Usuario" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="campo-formulario" ClientIDMode="Static" placeholder="Email" />
                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="campo-formulario" ClientIDMode="Static" placeholder="Rol" />
                        <div>
                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="boton boton-verde" OnClick="btnAgregar_Click" />
                            <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="boton boton-azul" OnClick="btnModificar_Click" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="boton boton-rojo" OnClick="btnEliminar_Click" />
                            <asp:Button ID="btnSerializar" runat="server" Text="Serializar" CssClass="boton boton-gris" OnClick="btnSerializar_Click" OnClientClick="return validarSeleccion();"/>
                            <asp:Button ID="btnCancelarEleccion" runat="server" Text="Cancelar" CssClass="boton boton-gris" OnClick="btnCancelarEleccion_Click" />
                        </div>
                    </div>

                    <div class="seccion-filtro">
                        <h3>Filtrar Usuarios</h3>
                        <asp:TextBox ID="txtFiltroDni" runat="server" CssClass="campo-formulario" placeholder="DNI" />
                        <asp:TextBox ID="txtFiltroUsername" runat="server" CssClass="campo-formulario" placeholder="Usuario" />
                        <asp:TextBox ID="txtFiltroEmail" runat="server" CssClass="campo-formulario" placeholder="Email" />
                        <asp:DropDownList ID="ddlFiltroRol" runat="server" CssClass="campo-formulario" />
                        <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="boton boton-gris" OnClick="btnFiltrar_Click" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="boton boton-gris" OnClick="btnLimpiar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        document.addEventListener('DOMContentLoaded', () => {
            const btnAgregar = document.getElementById('<%= btnAgregar.ClientID %>');
            const campos = [
                { id: 'txtDni', tipo: 'texto' },
                { id: 'txtNombre', tipo: 'texto' },
                { id: 'txtApellido', tipo: 'texto' },
                { id: 'txtUsername', tipo: 'texto' },
                { id: 'txtEmail', tipo: 'texto' },
                { id: 'ddlRol', tipo: 'seleccion' }
            ];

            function limpiarBorde(elemento) {
                elemento.style.border = '';
            }

            campos.forEach(campo => {
                const elemento = document.getElementById(campo.id);
                if (!elemento) return;
                elemento.addEventListener('focus', () => limpiarBorde(elemento));
            });

            btnAgregar.addEventListener('click', function (e) {
                let todoOk = true;
                campos.forEach(campo => {
                    const elemento = document.getElementById(campo.id);
                    if (!elemento) return;
                    if (campo.tipo === 'texto' && !elemento.value.trim()) {
                        elemento.style.border = '2px solid red';
                        todoOk = false;
                        setTimeout(() => limpiarBorde(elemento), 2500);
                    }
                    if (campo.tipo === 'seleccion' && !elemento.value) {
                        elemento.style.border = '2px solid red';
                        todoOk = false;
                        setTimeout(() => limpiarBorde(elemento), 2500);
                    }
                });
                if (!todoOk) e.preventDefault();
            });
        });

        document.addEventListener('DOMContentLoaded', () => {
            const botonesSeleccionar = document.querySelectorAll('.select-user-btn');

            botonesSeleccionar.forEach(boton => {
                boton.addEventListener('click', function () {
                    const fila = this.closest('tr');
                    const celdas = fila.querySelectorAll('td');

                    botonesSeleccionar.forEach(btn => {
                        const otraFila = btn.closest('tr');
                        otraFila.classList.remove('fila-seleccionada');
                    });

                    fila.classList.add('fila-seleccionada');

                    const txtDni = document.getElementById('txtDni');
                    txtDni.value = decodeHTMLEntidades(celdas[1].innerHTML.trim());
                    txtDni.readOnly = true;
                    document.getElementById('txtNombre').value = decodeHTMLEntidades(celdas[2].innerHTML.trim());
                    document.getElementById('txtApellido').value = decodeHTMLEntidades(celdas[3].innerHTML.trim());
                    document.getElementById('txtUsername').value = decodeHTMLEntidades(celdas[4].innerHTML.trim());
                    document.getElementById('txtEmail').value = decodeHTMLEntidades(celdas[5].innerHTML.trim());
                    document.getElementById('ddlRol').value = decodeHTMLEntidades(celdas[6].innerHTML.trim());
                    btnAgregar.disabled = true;
                });
            });

            function decodeHTMLEntidades(texto) {
                const area = document.createElement('textarea');
                area.innerHTML = texto;
                return area.value;
            }
        });
    </script>
    <script>
        function validarSeleccion() {
            var txt = document.getElementById('<%= txtUsername.ClientID %>');
            if (!txt.value.trim()) {
                alert('Debe seleccionar un usuario!');
                return false; // evita el postback
            }
            return true; // continúa al servidor
        }
    </script>
</body>
</html>