using ENTIDADES;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Rol"] != null) Response.Redirect("LandingPage.aspx");
    }
}