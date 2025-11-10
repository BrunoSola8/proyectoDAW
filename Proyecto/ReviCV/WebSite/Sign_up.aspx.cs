using ENTIDADES;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sign_up : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Application["EstadoBD"] == null)
        {
            GestorIntegridad gestor = new GestorIntegridad();
            string bdErrores = gestor.VerificarIntegridadTodasLasTablas();
            Application["EstadoBD"] = bdErrores == "" ? true : false;
        }
       // if (Application["EstadoBD"].Equals(false)) Response.Redirect("AvisoErrorBD.aspx");
        ErrorContraseñasLB.Visible = false;
        Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string dni = TbDNI.Text.Trim();
        string nombre = TbNombre.Text.Trim();
        string apellido = TbApellido.Text.Trim();
        string username = TbUserName.Text.Trim();
        string contraseña = TbPassw1.Text.Trim();
        string idioma = DropDownList1.SelectedValue.ToString();
        string repetirContraseña = TbPassw2.Text.Trim();
        string mail = TbMail.Text.Trim();
        GestorUsuario gestorUsuario = new GestorUsuario();
        List<string> errores = gestorUsuario.ValidarSignUp(dni, nombre, apellido, username,idioma, contraseña, mail);
        if (contraseña != repetirContraseña)
        {
            errores.Add("contraseña");
            ErrorContraseñasLB.Visible = true;
            ErrorContraseñasLB.ForeColor = System.Drawing.Color.Red;
            ErrorContraseñasLB.Text = "Las contraseñas no coinciden";
        }
        if (errores.Count > 0)
        {
            foreach (string campo in errores)
            {
                switch (campo)
                {
                    case "dni":
                        rfvDNI.ErrorMessage = "* DNI inválido o ya registrado";
                        rfvDNI.IsValid = false;
                        break;
                    case "nombre":
                        rfvNombre.ErrorMessage = "* Nombre inválido";
                        rfvNombre.IsValid = false;
                        break;
                    case "apellido":
                        rfvApellido.ErrorMessage = "* Apellido inválido";
                        rfvApellido.IsValid = false;
                        break;
                    case "username":
                        rfvUserName.ErrorMessage = "* Username inválido o ya existe";
                        rfvUserName.IsValid = false;
                        break;
                    case "idioma":
                        rfvIdioma.ErrorMessage = "* Idioma inválido";
                        rfvIdioma.IsValid = false;
                        break;
                    case "contraseña":
                        rfvPass1.ErrorMessage = "* Contraseña inválida";
                        rfvPass1.IsValid = false;
                        break;
                    case "mail":
                        rfvMail.ErrorMessage = "* Mail ya registrado";
                        rfvMail.IsValid = false;
                        break;
                }
            }
            return; 
        }
        Encriptador encriptador = new Encriptador();

        Usuario usuario = new Usuario(
            int.Parse(dni),
            nombre,
            apellido,
            username,
            encriptador.EncriptarIrreversible(contraseña),
            mail
            ,idioma
        );

        gestorUsuario.InsertarUsuario(usuario);

        ErrorContraseñasLB.Visible = true;
        ErrorContraseñasLB.ForeColor = System.Drawing.Color.Green;
        ErrorContraseñasLB.Text = "Usuario registrado correctamente";

        VaciarTB();

    }
    public void VaciarTB()
    {
        TbDNI.Text = string.Empty;
        TbNombre.Text = string.Empty;
        TbApellido.Text = string.Empty;
        TbUserName.Text = string.Empty;
        TbPassw1.Text = string.Empty;
        TbMail.Text = string.Empty;
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }
}