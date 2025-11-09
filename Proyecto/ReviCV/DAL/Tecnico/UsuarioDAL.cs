using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;

namespace DAL
{
    public class UsuarioDAL
    {
        public bool UsuarioYaRegistrado(int pDNI, string username)
        {
            string query = $"SELECT COUNT(*) FROM {TablasBD.Usuario} WHERE DNI = @DNI OR username = @username";
            using(SqlCommand CM = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                CM.Parameters.AddWithValue("@DNI", pDNI);
                CM.Parameters.AddWithValue("@username", username);
                
                int cantidad = (int)CM.ExecuteScalar();
                
                return cantidad > 0;
            }
        }
        public bool UsernameRepetido(string username)
        {
            string query = $"SELECT COUNT(*) FROM {TablasBD.Usuario} WHERE username = @username";
            using (SqlCommand CM = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                CM.Parameters.AddWithValue("@username", username);
                
                int cantidad = (int)CM.ExecuteScalar();
                
                return cantidad > 0;
            }
        }
        public bool ValidarUsuario(string pUsuario, string Contra)
        {
            bool x = false;
            string query = $"SELECT * FROM {TablasBD.Usuario} WHERE username = @username AND password = @password";
            using (SqlCommand CM = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                CM.Parameters.AddWithValue("@username", pUsuario);
                CM.Parameters.AddWithValue("@password", Contra);
                
                using (SqlDataReader DR = CM.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        if (DR != null)
                        {
                            x = true;
                            break;
                        }
                    }
                }
            }
            
            return x;
        }
        public List<Usuario> FiltrarUsuarios(string dni, string username, string email, string rol)
        {
            List<Usuario> lista = new List<Usuario>();
            List<string> condiciones = new List<string>();
            SqlCommand cmd = new SqlCommand();

            string query = $"SELECT * FROM {TablasBD.Usuario}";

            if (!string.IsNullOrWhiteSpace(dni))
            {
                condiciones.Add("DNI LIKE @DNI");
                cmd.Parameters.AddWithValue("@DNI", $"%{dni}%");
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                condiciones.Add("username LIKE @Username");
                cmd.Parameters.AddWithValue("@Username", $"%{username}%");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                condiciones.Add("Mail LIKE @Email");
                cmd.Parameters.AddWithValue("@Email", $"%{email}%");
            }

            if (!string.IsNullOrWhiteSpace(rol))
            {
                condiciones.Add("Rol = @Rol");
                cmd.Parameters.AddWithValue("@Rol", rol);
            }

            if (condiciones.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", condiciones);
            }

            cmd.CommandText = query;
            cmd.Connection = Conexion.Instancia.ReturnConexion();

            
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Usuario u = new Usuario()
                    {
                        DNI = int.Parse(reader["DNI"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        NombreUsuario = reader["username"].ToString(),
                        Password = reader["password"].ToString(),
                        Email = reader["Mail"].ToString(),
                        Rol = reader["Rol"].ToString()
                    };
                    lista.Add(u);
                }
            }
            

            return lista;
        }
        public Usuario ObtenerUsuario(string pUsuario)
        {
            Usuario U = null;
            string Query = $"SELECT * FROM {TablasBD.Usuario} WHERE username = @username";
            using (SqlCommand CM = new SqlCommand(Query, Conexion.Instancia.ReturnConexion()))
            {
                
                CM.Parameters.AddWithValue("@username", pUsuario);
                using (SqlDataReader DR = CM.ExecuteReader())
                {
                    if (DR.Read())
                    {
                        U = new Usuario(int.Parse(DR[0].ToString()), DR[1].ToString(), DR[2].ToString(), DR[3].ToString(), "", DR[5].ToString(), DR[6].ToString());
                    }
                }
            }
            
            return U;
        }

        public void InsertarUsuario(Usuario U)
        {
            string Query = $"INSERT INTO {TablasBD.Usuario} (DNI, Nombre, Apellido, username, password, Mail, Rol) VALUES (@DNI, @Nombre, @Apellido, @Username, @Pass, @Mail, @Rol)";

            using (SqlCommand CM = new SqlCommand(Query, Conexion.Instancia.ReturnConexion()))
            {
                
                CM.Parameters.AddWithValue("@DNI", U.DNI);
                CM.Parameters.AddWithValue("@Nombre", U.Nombre);
                CM.Parameters.AddWithValue("@Apellido", U.Apellido);
                CM.Parameters.AddWithValue("@Username", U.NombreUsuario);
                CM.Parameters.AddWithValue("@Pass", U.Password);
                CM.Parameters.AddWithValue("@Mail", U.Email);
                CM.Parameters.AddWithValue("@Rol", U.Rol);
                CM.ExecuteNonQuery();
            }
            
        }
        public void EliminarUsuario(int dni)
        {
            string query = $"DELETE FROM {TablasBD.Usuario} WHERE DNI = @DNI";

            using (SqlCommand cm = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                cm.Parameters.AddWithValue("@DNI", dni);
                cm.ExecuteNonQuery();
                
            }
        }
        public List<string> ObtenerTodosNombresUsuarios()
        {
            List<string> U = new List<string>();
            string Query = $"SELECT * FROM {TablasBD.Usuario}";
            using (SqlCommand CM = new SqlCommand(Query, Conexion.Instancia.ReturnConexion()))
            {
                
                using (SqlDataReader DR = CM.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        U.Add(DR[3].ToString());
                    }
                }
            }
            
            return U;
        }
        public void ModificarUsuario(Usuario U)
        {
            string query = $@"UPDATE {TablasBD.Usuario} SET Nombre = @Nombre,Apellido = @Apellido, username = @Username, Mail = @Mail, Rol = @Rol WHERE DNI = @DNI";

            using (SqlCommand cm = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                cm.Parameters.AddWithValue("@Nombre", U.Nombre);
                cm.Parameters.AddWithValue("@Apellido", U.Apellido);
                cm.Parameters.AddWithValue("@Username", U.NombreUsuario);
                cm.Parameters.AddWithValue("@Mail", U.Email);
                cm.Parameters.AddWithValue("@Rol", U.Rol);
                cm.Parameters.AddWithValue("@DNI", U.DNI);
                cm.ExecuteNonQuery();
                
            }
        }
        public List<Usuario> ObtenerTodosUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            string query = $"SELECT * FROM {TablasBD.Usuario}";

            using (SqlCommand cm = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Usuario u = new Usuario
                        (
                            int.Parse(dr["DNI"].ToString()),
                            dr["Nombre"].ToString(),
                            dr["Apellido"].ToString(),
                            dr["username"].ToString(),
                            "",
                            dr["Mail"].ToString(),
                            dr["Rol"].ToString()
                        );

                        usuarios.Add(u);
                    }
                }
            }
            
            return usuarios;
        }

        public void CambiarPassword(int dni, string pPassword)
        {
            string query = $"UPDATE {TablasBD.Usuario} SET password = @Password WHERE DNI = @DNI";

            using (SqlCommand cm = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                cm.Parameters.AddWithValue("@Password", pPassword);
                cm.Parameters.AddWithValue("@DNI", dni);
                cm.ExecuteNonQuery();
                
            }
        }

    }
}
