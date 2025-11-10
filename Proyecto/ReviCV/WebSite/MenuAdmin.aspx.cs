using SERVICIOS;
using SERVICIOS.Traducciones;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class MenuAdmin : Page, IObserver
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
        Actualizar();
    }

    protected void btnVerPerfilUsuario_Click(object sender, EventArgs e)
    {
        Response.Redirect("PaginaPerfilUsuario.aspx");
    }
    public void Actualizar()
    {
        RecorrerControles(this);
    }
    void RecorrerControles(Control controlPadre)
    {
        TraductorDAL traductor = new TraductorDAL();
        foreach (Control c in controlPadre.Controls)
        {
            if (c is LinkButton lbl && lbl.Attributes["data-key"] != null)
            {
                string clave = lbl.Attributes["data-key"];
                lbl.Text = traductor.Traducir(clave);
            }
            else if (c is Button btn && btn.Attributes["data-key"] != null)
            {
                string clave = btn.Attributes["data-key"];
                btn.Text = traductor.Traducir(clave);
            }
            else if (c is HtmlGenericControl html)
            {
                if (html.Attributes["data-key"] != null)
                {
                    string clave = html.Attributes["data-key"];
                    html.InnerText = traductor.Traducir(clave);
                }
                else if (html.TagName.Equals("p", StringComparison.OrdinalIgnoreCase))
                {
                    string clave = html.Attributes["data-key"];
                    html.InnerText = traductor.Traducir(clave);
                }
                else if (html.TagName.Equals("h1", StringComparison.OrdinalIgnoreCase))
                {
                    string clave = html.Attributes["data-key"];
                    html.InnerText = traductor.Traducir(clave);
                }
                else if (html.TagName.Equals("h2", StringComparison.OrdinalIgnoreCase))
                {
                    string clave = html.Attributes["data-key"];
                    html.InnerText = traductor.Traducir(clave);
                }
            }
            if (c.HasControls())
            {
                RecorrerControles(c);
            }
        }
    }

}
