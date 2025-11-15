using BLL;
using ENTIDADES;
using SERVICIOS;
using SERVICIOS.Traducciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page, IObserver
{
    public void Actualizar()
    {
        RecorrerControles(this);
    }
    void RecorrerControles(Control controlPadre)
    {
        TraductorDAL traductor = new TraductorDAL();
        foreach (Control c in controlPadre.Controls)
        {
            if (c is Label lbl && lbl.Attributes["data-key"] != null)
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
                else if (html.TagName.Equals("h2", StringComparison.OrdinalIgnoreCase))
                {
                    html.InnerText = html.InnerText.ToUpper();
                }
            }
            if (c.HasControls())
            {
                RecorrerControles(c);
            }
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["username"] == null)
            {
                GestorUsuario gestorUsuario = new GestorUsuario();
                Usuario u = gestorUsuario.ObtenerUsuario(tbNombreUsuario.Text);
                if (u != null)
                {
                    Validador validador = new Validador();
                    Encriptador encriptador = new Encriptador();
                    if (validador.Verificar(u.NombreUsuario, encriptador.EncriptarIrreversible(tbContraseña.Text)))
                    {
                        GestorIntegridad gestorIntegridad = new GestorIntegridad();
                        string bdErrores = gestorIntegridad.VerificarIntegridadTodasLasTablas();
                        Application["EstadoBD"] = bdErrores == "" ? true : false;
                        Application["ErroresBD"] = "";

                        GuardarSession(u);
                        if (Application["EstadoBD"].Equals(true))
                        {
                            if (Session["Rol"].ToString() != "Usuario")
                            {
                                GestorBitacora gestorBitacora = new GestorBitacora();
                                gestorBitacora.GuardarLogBitacora("Login", Session["username"].ToString());
                            }

                            Response.Redirect("LandingPage.aspx");
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            Application["ErroresBD"] = bdErrores;
                            if (Session["Rol"].Equals("Webmaster"))
                            {
                                Response.Redirect("Verificador.aspx");
                            }
                            else
                            {
                                Response.Redirect("AvisoErrorBD.aspx");
                                Context.ApplicationInstance.CompleteRequest();
                            }
                        }
                    }
                    else { labelErrores.ForeColor = System.Drawing.Color.Red; labelErrores.Text = "Credenciales incorrectas"; }
                }
                else { labelErrores.ForeColor = System.Drawing.Color.Red; labelErrores.Text = "No existe el usuario"; }
            }
            else { labelErrores.ForeColor = System.Drawing.Color.Orange; labelErrores.Text = "Ya hay una sesión iniciada"; }
        }
        catch { labelErrores.ForeColor = System.Drawing.Color.Red; labelErrores.Text = "Tiempo de espera agotado."; }
    }

    protected void btnSignUp_Click(object sender, EventArgs e)
    {
        Response.Redirect("Sign_Up.aspx");
    }

    public void GuardarSession(Usuario u)
    {
        Session["username"] = $"{u.NombreUsuario}";
        Session["Rol"] = $"{u.Rol}";
        Session["Idioma"] = $"{u.Idioma}";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //TraductorDAL traductor = new TraductorDAL();
        //traductor.Suscribe(this);
        //traductor.Notify();
        //traductor.CargarTraduccionesDesdeBD("Español");
        if (Session["Rol"] != null) Response.Redirect("LandingPage.aspx");
        //Actualizar();
    }
}