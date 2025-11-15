using ENTIDADES;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GestorUsuario
    {
        public Usuario ObtenerUsuario(string pUsuario)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ObtenerUsuario(pUsuario);
        }

        public void InsertarUsuario(Usuario pUsuario)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            usuarioDAL.InsertarUsuario(pUsuario);

            GestorIntegridad gestorIntegridad = new GestorIntegridad();
            gestorIntegridad.ActualizarDVHRegistro(TablasBD.Usuario, pUsuario.DNI);
        }
        public bool UsuarioExistente(int pDNI, string username)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.UsuarioYaRegistrado(pDNI, username);
        }
        public bool UsernameRepetido(string username)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.UsernameRepetido(username);
        }
        public List<string> ValidarSignUp(string dni, string Nombre, string Apellido, string username,string idioma, string contraseña, string mail)
        {
            HashSet<string> errores = new HashSet<string>();

            if (string.IsNullOrWhiteSpace(dni) || !dni.All(char.IsDigit) || dni.Length < 7 || dni.Length > 8)
                errores.Add("dni");

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("nombre");

            if (string.IsNullOrWhiteSpace(Apellido))
                errores.Add("apellido");

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
                errores.Add("username");

            if (string.IsNullOrWhiteSpace(idioma))
                errores.Add("idioma");

            if (string.IsNullOrWhiteSpace(contraseña))
                errores.Add("contraseña");

            if (string.IsNullOrWhiteSpace(mail))
                errores.Add("mail");
            else
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(mail);
                    if (addr.Address != mail)
                        errores.Add("mail");
                }
                catch
                {
                    errores.Add("mail");
                }
            }
            foreach (var campo in UsuarioYaExiste(dni, username, mail))
                errores.Add(campo);


            return errores.ToList();
        }
        private List<string> UsuarioYaExiste(string dni, string username, string mail)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            List<string> repetidos = new List<string>();
            List<Usuario> usuariosRegistrados = usuarioDAL.ObtenerTodosUsuarios();

            if (usuariosRegistrados.Any(u => u.DNI.ToString() == dni))
                repetidos.Add("dni");

            if (usuariosRegistrados.Any(u => u.NombreUsuario == username))
                repetidos.Add("username");

            if (usuariosRegistrados.Any(u => u.Email == mail))
                repetidos.Add("mail");

            return repetidos;
        }
        public void EliminarUsuario(int dni)
        {
            UsuarioDAL usuarioDAL=new UsuarioDAL();
            usuarioDAL.EliminarUsuario(dni);

            GestorIntegridad gestorIntegridad = new GestorIntegridad();
            gestorIntegridad.GuardarIntegridadTabla(TablasBD.Usuario);
        }
        public void ModificarUsuario(Usuario pUsuario)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            usuarioDAL.ModificarUsuario(pUsuario);

            GestorIntegridad gestorIntegridad = new GestorIntegridad();
            gestorIntegridad.ActualizarDVHRegistro(TablasBD.Usuario, pUsuario.DNI);
        }
        public List<string> ObtenerTodosNombresUsuarios()
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ObtenerTodosNombresUsuarios();
        }
        
        public List<Usuario> ObtenerTodosUsuarios()
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ObtenerTodosUsuarios();
        }
        public List<Usuario> FiltrarUsuarios(string dni, string username, string email, string rol)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.FiltrarUsuarios(dni, username, email, rol);
        }

        public void CambiarPassword(int dni, string pPassword)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            Encriptador encriptador = new Encriptador();
            usuarioDAL.CambiarPassword(dni, encriptador.EncriptarIrreversible(pPassword));

            GestorIntegridad gestorIntegridad = new GestorIntegridad();
            gestorIntegridad.ActualizarDVHRegistro(TablasBD.Usuario, dni);
        }
    }
}
