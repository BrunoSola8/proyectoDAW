using BLL;
using ENTIDADES;
using SERVICIOS.Traducciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PaginaPerfilUsuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Rol"] == null) Response.Redirect("LandingPage.aspx");
        if (Session["username"].ToString() == "") Response.Redirect("LandingPage.aspx");
        if (!IsPostBack)
        {
            SettearHiddenFields();
            CargarIdiomas();
            CargarRubros();
            CargarIdiomas2();
            string idiomaUsuario = Session["Idioma"].ToString();
            ddlIdioma.SelectedValue = idiomaUsuario;
        }
    }

    private void CargarIdiomas2()
    {
        ddlIdioma.Items.Clear();
        ddlIdioma.Items.Add(new ListItem("Español", "Español"));
        ddlIdioma.Items.Add(new ListItem("Inglés", "Ingles"));
        ddlIdioma.Items.Add(new ListItem("Portugués", "Portugues"));
    }

    //Esto se ejecuta antes que Page_Load y sirve para crear controles dinamicos como los de los cvs que tiene cada usuario - Matt
    protected void Page_Init(object sender, EventArgs e)
    {
        CargarCurriculums();
    }


   
    private void SettearHiddenFields()
    {
        string nombreUsuario = Session["username"] as string;

        if (!string.IsNullOrEmpty(nombreUsuario))
        {
            GestorUsuario gestorUsuario = new GestorUsuario();
            Usuario usuario = gestorUsuario.ObtenerUsuario(nombreUsuario);

            if (usuario != null)
            {
                username.Text = usuario.NombreUsuario;
                firstName.Text = usuario.Nombre;
                lastName.Text = usuario.Apellido;
                email.Text = usuario.Email;

                hfOriginalUsername.Value = usuario.NombreUsuario;
                hfOriginalFirstName.Value = usuario.Nombre;
                hfOriginalLastName.Value = usuario.Apellido;
                hfOriginalEmail.Value = usuario.Email;
                hfOriginalDNI.Value = usuario.DNI.ToString();
                hfOriginalRol.Value = usuario.Rol;

                lblNombrePerfil.Text = usuario.Nombre + " " + usuario.Apellido;
                lblUsuarioPerfil.Text = usuario.NombreUsuario;
            }
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (username.Text == "" || firstName.Text == "" || lastName.Text == "" || email.Text == "")
        {
            //

            string script = @"
            document.addEventListener('DOMContentLoaded', function() {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: 'Oops...',
                    text: 'Necesitas completar todos los campos.',
                    icon: 'error',
                    confirmButtonText: 'Ok',
                    backdrop: true,
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {
                        container: 'swal-container-fix'
                    }
                }).then(() => {
                    window.location.href = 'PaginaPerfilUsuario.aspx';
                });
            } else {
                window.location.href = 'PaginaPerfilUsuario.aspx';
            }
        });";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "SwalSuccess",
                script,
                true
            );

            //
        }
        else
        {


            GestorUsuario gestorUsuario = new GestorUsuario();
            Usuario usuario = new Usuario
            {
                NombreUsuario = username.Text,
                Nombre = firstName.Text,
                Password = "",
                Apellido = lastName.Text,
                Email = email.Text,
                Rol = hfOriginalRol.Value,
                DNI = int.Parse(hfOriginalDNI.Value),
                Idioma = ddlIdioma.Text
            };
            gestorUsuario.ModificarUsuario(usuario);
            Session["username"] = username.Text;
            Session["Idioma"] = ddlIdioma.Text;
            SettearHiddenFields();

            string script = @"
        document.addEventListener('DOMContentLoaded', function() {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: '¡Actualizaste tus datos!',
                    text: 'Cambios realizados con éxito.',
                    icon: 'success',
                    confirmButtonText: 'Ok',
                    backdrop: true,
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {
                        container: 'swal-container-fix'
                    }
                }).then(() => {
                    window.location.href = 'PaginaPerfilUsuario.aspx';
                });
            } else {
                window.location.href = 'PaginaPerfilUsuario.aspx';
            }
        });";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "SwalSuccess",
                script,
                true
            );
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        username.Text = hfOriginalUsername.Value;
        firstName.Text = hfOriginalFirstName.Value;
        lastName.Text = hfOriginalLastName.Value;
        email.Text = hfOriginalEmail.Value;
    }


    protected void btnCambiarPassword_Click(object sender, EventArgs e)
    {
        if (newPassword.Text == confirmPassword.Text)
        {
            GestorUsuario gestorUsuario = new GestorUsuario();
            gestorUsuario.CambiarPassword(int.Parse(hfOriginalDNI.Value), newPassword.Text);

            string script = @"
            document.addEventListener('DOMContentLoaded', function() {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: '¡Actualizaste tus contraseña!',
                    text: 'Contraseña cambiada con éxito.',
                    icon: 'success',
                    confirmButtonText: 'Ok',
                    backdrop: true,
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {
                        container: 'swal-container-fix'
                    }
                }).then(() => {
                    window.location.href = 'PaginaPerfilUsuario.aspx';
                });
            } else {
                window.location.href = 'PaginaPerfilUsuario.aspx';
            }
        });";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "SwalSuccess",
                script,
                true
            );
        }
        else
        {
            string script = @"
            document.addEventListener('DOMContentLoaded', function() {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: 'Oops...',
                    text: 'Las contraseñas no coinciden.',
                    icon: 'error',
                    confirmButtonText: 'Ok',
                    backdrop: true,
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {
                        container: 'swal-container-fix'
                    }
                }).then(() => {
                    window.location.href = 'PaginaPerfilUsuario.aspx';
                });
            } else {
                window.location.href = 'PaginaPerfilUsuario.aspx';
            }
        });";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "SwalSuccess",
                script,
                true
            );
        }
    }

    protected void btnSubirArchivo_Click(object sender, EventArgs e)
    {
        if (!fileUpload.HasFile)
            return;

        string extension = Path.GetExtension(fileUpload.FileName).ToLower();

        if (extension != ".pdf" && extension != ".jpg" && extension != ".jpeg" && extension != ".png")
        {
            string script = @"
            document.addEventListener('DOMContentLoaded', function() {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: 'Oops...',
                    text: 'Tu CV debe ser un PDF o una imagen.',
                    icon: 'error',
                    confirmButtonText: 'Ok',
                    backdrop: true,
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {
                        container: 'swal-container-fix'
                    }
                }).then(() => {
                    window.location.href = 'PaginaPerfilUsuario.aspx';
                });
            } else {
                window.location.href = 'PaginaPerfilUsuario.aspx';
            }
        });";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "SwalSuccess",
                script,
                true
            );
        }
        else if (string.IsNullOrEmpty(ddlIdiomas.SelectedValue) || string.IsNullOrEmpty(ddlRubros.SelectedValue))
        {
            string script = @"
        document.addEventListener('DOMContentLoaded', function() {
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                title: 'Oops...',
                text: 'Debes seleccionar un idioma y un rubro para tu CV.',
                icon: 'warning',
                confirmButtonText: 'Ok',
                backdrop: true,
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {
                    container: 'swal-container-fix'
                }
            }).then(() => {
                window.location.href = 'PaginaPerfilUsuario.aspx';
            });
        } else {
            window.location.href = 'PaginaPerfilUsuario.aspx';
        }
    });";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "SwalError",
                script,
                true
            );
        }
        else
        {
            string nombreUsuario = Session["username"].ToString();
            GestorUsuario gUsuarios = new GestorUsuario();

            Curriculum cv = new Curriculum();
            cv.Usuario = nombreUsuario;

            using (var ms = new MemoryStream())
            {
                fileUpload.PostedFile.InputStream.CopyTo(ms);
                cv.ArchivoCV = ms.ToArray();
            }

            cv.Nombre = hfNombreArchivo.Value;
            cv.Idioma = (int.Parse(ddlIdiomas.SelectedValue), ddlIdiomas.SelectedItem.Text);
            cv.Rubro = (int.Parse(ddlRubros.SelectedValue), ddlRubros.SelectedItem.Text);

            GestorCurriculum gestor = new GestorCurriculum();
            gestor.GuardarCurriculum(cv);

            string script = @"
            document.addEventListener('DOMContentLoaded', function() {
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: '¡CV Subido!',
                    text: 'Tu curriculum se ha guardado.',
                    icon: 'success',
                    confirmButtonText: 'Ok',
                    backdrop: true,
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {
                        container: 'swal-container-fix'
                    }
                }).then(() => {
                    window.location.href = 'PaginaPerfilUsuario.aspx';
                });
            } else {
                window.location.href = 'PaginaPerfilUsuario.aspx';
            }
        });";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "SwalSuccess",
                script,
                true
            );
        }

    }

    private void CargarIdiomas()
    {
        GestorCurriculum gCurriculums = new GestorCurriculum();
        var idiomas = gCurriculums.ObtenerIdiomas();

        ddlIdiomas.DataSource = idiomas;
        ddlIdiomas.DataTextField = "Value";
        ddlIdiomas.DataValueField = "Key";
        ddlIdiomas.DataBind();

        ddlIdiomas.Items.Insert(0, new ListItem("Idioma del CV", ""));
        ddlIdiomas.Items[0].Attributes.Add("disabled", "true");
        ddlIdiomas.Items[0].Selected = true;
    }

    private void CargarRubros()
    {
        GestorCurriculum gCurriculums = new GestorCurriculum();
        var rubros = gCurriculums.ObtenerRubros();

        ddlRubros.DataSource = rubros;
        ddlRubros.DataTextField = "Value";
        ddlRubros.DataValueField = "Key";
        ddlRubros.DataBind();

        ddlRubros.Items.Insert(0, new ListItem("Rubro del CV", ""));
        ddlRubros.Items[0].Attributes.Add("disabled", "true");
        ddlRubros.Items[0].Selected = true;
    }


    private void CargarCurriculums()
    {
        string nombreUsuario = Session["username"]?.ToString();
        if (string.IsNullOrEmpty(nombreUsuario))
            return;

        GestorCurriculum gestor = new GestorCurriculum();
        var cvs = gestor.ObtenerCurriculumsPorUsuario(nombreUsuario);

        phCurriculums.Controls.Clear();

        foreach (var cv in cvs)
        {
            var contenedor = new Panel { CssClass = "curriculum-item" };

            var lbl = new Label
            {
                CssClass = "curriculum-titulo",
                Text = $"{cv.Nombre} ({cv.Idioma.Item2} - {cv.Rubro.Item2})"
            };
            contenedor.Controls.Add(lbl);


            var btnEliminar = new LinkButton
            {
                Text = "X",
                CommandArgument = cv.ID_CV.ToString(),
                OnClientClick = "return confirm('¿Eliminar este CV?');",
                Style =
                    {
                        ["margin-right"] = "10px",
                        ["font-size"] = "20px",
                        ["color"] = "red",
                        ["text-decoration"] = "none",
                        ["font-weight"] = "bold"
                    }
                };

            btnEliminar.Click += btnEliminar_Click;
            contenedor.Controls.Add(btnEliminar);

            // Botón Ver Reseñas
            var btnVerResenias = new Button
            {
                Text = "Ver reseñas",
                CssClass = "btn btn-guardar",
                PostBackUrl = $"VerResenias.aspx?id={cv.ID_CV}",
                Style = { ["margin-left"] = "auto" },
                Width = Unit.Pixel(130),
                CommandArgument = cv.ID_CV.ToString()
            };
            contenedor.Controls.Add(btnVerResenias);

            phCurriculums.Controls.Add(contenedor);
        }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        var btnEliminar = (LinkButton)sender;
        int idCV;

        if (int.TryParse(btnEliminar.CommandArgument, out idCV))
        {
            GestorCurriculum gestor = new GestorCurriculum();
            gestor.EliminarCurriculum(idCV); 

            CargarCurriculums();
        }
    }





    protected void btnVolverPrincipal_Click(object sender, EventArgs e)
    {
        Response.Redirect("LandingPage.aspx");
    }

    protected void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("LandingPage.aspx");
    }
}