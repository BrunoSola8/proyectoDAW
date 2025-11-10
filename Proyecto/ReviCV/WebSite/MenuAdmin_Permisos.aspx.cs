using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MenuAdmin_Permisos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ddlRolesGrupos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void chkListPermisos_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

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
        GestorBitacora gestorBitacora = new GestorBitacora();
        gestorBitacora.GuardarLogBitacora("Logout", Session["username"].ToString());
        Session.Clear();
        Response.Redirect("LandingPage.aspx");
    }

    protected void btnVolverALanding_Click(object sender, EventArgs e)
    {
        Response.Redirect("LandingPage.aspx");
    }

    protected void btnBitacora_Click(object sender, EventArgs e)
    {
        Response.Redirect("BitacoraPage.aspx");
    }

    protected void btnRubrosIdiomas_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin_RubrosIdiomas.aspx");
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {

    }

    protected void btnModificarNombre_Click(object sender, EventArgs e)
    {

    }

    protected void btnCrearRol_Click(object sender, EventArgs e)
    {

    }

    protected void btnCrearGrupo_Click(object sender, EventArgs e)
    {

    }

    protected void btnGuardarCambios_Click(object sender, EventArgs e)
    {

    }
}