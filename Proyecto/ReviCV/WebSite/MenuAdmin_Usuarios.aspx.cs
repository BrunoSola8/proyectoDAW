using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using ENTIDADES;
using SERVICIOS;


public partial class MenuAdmin_Usuarios : Page
{


    protected void btnInicio_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin.aspx");
    }

    protected void btnUsuarios_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin_Usuarios.aspx");
    }

    protected void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("LandingPage.aspx");
    }

    protected void btnVolverALanding_Click(object sender, EventArgs e)
    {
        Response.Redirect("LandingPage.aspx");
    }

    private void CargarUsuarios()
    {
        GestorUsuario gestorUsuarios = new GestorUsuario();
        List<Usuario> usuarios = gestorUsuarios.ObtenerTodosUsuarios();
        gvUsuarios.DataSource = usuarios;
        gvUsuarios.DataBind();
    }
    private void CargarUsuariosFiltrados(string dni, string username, string mail, string rol)
    {
        GestorUsuario gestorUsuarios = new GestorUsuario();
        List<Usuario> usuarios = gestorUsuarios.FiltrarUsuarios(dni, username, mail, rol);
        gvUsuarios.DataSource = usuarios;
        gvUsuarios.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Rol"] == null) Response.Redirect("LandingPage.aspx");

        var estadoBD = Application["EstadoBD"];
        var rol = Session["Rol"]?.ToString();

        if (estadoBD is bool bdOk && !bdOk)
        {
            Response.Redirect("AvisoErrorBD.aspx");
        }

        if (rol != "Administrador")
        {
            Response.Redirect("LandingPage.aspx");
        }

        if (!IsPostBack)
        {
            CargarUsuarios();
            CargarRoles();
        }
    }



    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Encriptador encriptador = new Encriptador();
        GestorUsuario gestorUsuarios = new GestorUsuario();
        if (int.TryParse(txtDni.Text, out int x) && !string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtApellido.Text) && !string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(ddlRol.SelectedValue))
        {
            if(!gestorUsuarios.UsuarioExistente(int.Parse(txtDni.Text), txtUsername.Text))            
            {
                Usuario usuario = new Usuario
                    (
                    int.Parse(txtDni.Text),
                    txtNombre.Text,
                    txtApellido.Text,
                    txtUsername.Text,
                    encriptador.EncriptarIrreversible(txtDni.Text),
                    txtEmail.Text,
                    ddlRol.SelectedValue
                    );

                gestorUsuarios.InsertarUsuario(usuario);
                GestorBitacora gestorBitacora = new GestorBitacora();
                gestorBitacora.GuardarLogBitacora($"Se agregó el usuario {usuario.DNI}", Session["username"].ToString());
                CargarUsuarios();
                LimpiarTxt();
            }            
        }
    }


    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        GestorUsuario gestorUsuarios = new GestorUsuario();
        CargarUsuariosFiltrados(txtFiltroDni.Text, txtFiltroUsername.Text, txtFiltroEmail.Text, ddlFiltroRol.SelectedValue.ToString());
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        Encriptador encriptador = new Encriptador();
        GestorUsuario gestorUsuario = new GestorUsuario();
        if (int.TryParse(txtDni.Text, out int x) && !string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtApellido.Text) && !string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(ddlRol.SelectedValue))
        {
            if(!gestorUsuario.UsernameRepetido(txtUsername.Text))
            {
                Usuario usuario = new Usuario
            (
                int.Parse(txtDni.Text),
                txtNombre.Text,
                txtApellido.Text,
                txtUsername.Text,
                "",
                txtEmail.Text,
                ddlRol.SelectedValue
            );


                gestorUsuario.ModificarUsuario(usuario);
                GestorBitacora gestorBitacora = new GestorBitacora();
                gestorBitacora.GuardarLogBitacora($"Se modificó el usuario {usuario.DNI}", Session["username"].ToString());
                CargarUsuarios();
                LimpiarTxt();
            }            
        }
    }
    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        if(txtDni.Text != "")
        {
            GestorUsuario gestorUsuario = new GestorUsuario();
            gestorUsuario.EliminarUsuario(int.Parse(txtDni.Text));
            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.GuardarLogBitacora($"Se eliminó el usuario {txtDni.Text}", Session["username"].ToString());
            CargarUsuarios();
            LimpiarTxt();
      
        }
    }
    protected void btnSerializar_Click(object sender, EventArgs e)
    {
        GestorUsuario gestorUsuario = new GestorUsuario();
        Usuario usuarioSerializado = gestorUsuario.ObtenerUsuario(txtUsername.Text);
        ServicioWeb sw = new ServicioWeb();
        sw.SerializarUsuario(usuarioSerializado);
    }
    protected void btnDeserializar_Click(object sender, EventArgs e)
    {        
        ServicioWeb sw = new ServicioWeb();
        Usuario usuario = sw.DeserializarUsuario(fuArchivo.FileName);
        CargarUsuarioDeserializado(usuario);
    }
    private void CargarUsuarioDeserializado(Usuario usuario)
    {
        if (usuario != null)
        {
            List<Usuario> usuarios = new List<Usuario>();
            usuarios.Add(usuario);
            gvUsuarios.DataSource = usuarios;
            gvUsuarios.DataBind();
        }
    }
    public void LimpiarTxt()
    {
        txtApellido.Text = "";
        txtDni.Text = "";
        txtEmail.Text = "";
        txtNombre.Text = "";
        txtUsername.Text = "";
        ddlRol.SelectedValue = "";
    }
    public void CancelarModificacion()
    {
        btnAgregar.Enabled = true;
        txtApellido.Text = "";
        txtDni.Text = "";
        txtEmail.Text = "";
        txtNombre.Text = "";
        txtUsername.Text = "";
        ddlRol.SelectedValue = "";
    }
    public void LimpiarFiltros()
    {
        txtFiltroEmail.Text = "";
        txtFiltroDni.Text = "";
        txtFiltroUsername.Text = "";
        ddlFiltroRol.SelectedValue = "";
        CargarUsuarios();
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarFiltros();
    }

    private void CargarRoles()
    {
        List<ListItem> roles = new List<ListItem>
    {
        new ListItem("Selecciona un rol", ""),
        new ListItem("Administrador", "Administrador"),
        new ListItem("Webmaster", "Webmaster"),
        new ListItem("Usuario", "Usuario")
    };

        // Desactivar la opción por defecto
        roles[0].Attributes.Add("disabled", "true");
        roles[0].Selected = true;

        ddlRol.Items.Clear();
        ddlFiltroRol.Items.Clear();

        ddlRol.Items.AddRange(roles.ToArray());
        ddlFiltroRol.Items.Add(new ListItem("Todos los roles", ""));
        for (int i = 1; i < roles.Count; i++)
        {
            ddlFiltroRol.Items.Add(roles[i]);
        }
    }


    protected void btnBitacora_Click(object sender, EventArgs e)
    {

    }

    protected void btnRubrosIdiomas_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin_RubrosIdiomas.aspx");
    }

    protected void btnCancelarEleccion_Click(object sender, EventArgs e)
    {
        CancelarModificacion();
    }



}
