using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Permisos
{
    public class PermisoDAO
    {

        public List<Permiso> LeerPermisosEnArbol()
        {
            var permisos = new List<Permiso>();
            var permisosCompuestos = new List<Permiso>();

            try
            {
                string queryPermisos = "SELECT * FROM Permiso";

                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(queryPermisos, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nombre = reader["NombrePermiso"].ToString();
                        string tipo = reader["TipoPermiso"].ToString();

                        if (tipo == "Compuesto")
                        {
                            var compuesto = new PermisoCompuesto(nombre);
                            permisosCompuestos.Add(compuesto);
                            permisos.Add(compuesto);
                        }
                        else
                        {
                            permisos.Add(new PermisoSimple(nombre));
                        }
                    }
                }

                string queryRelaciones = "SELECT * FROM PermisoRelacion";

                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(queryRelaciones, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var a = reader["CompuestoNombre"].ToString();
                        var b = reader["IncluidoNombre"].ToString();
                        var compuesto = (PermisoCompuesto)permisosCompuestos.Find(x => x.getNombre() == reader["CompuestoNombre"].ToString());

                        var incluido = permisos.Find(x => x.getNombre() == reader["IncluidoNombre"].ToString());

                        compuesto.AgregarPermiso(incluido);
                    }
                }

                return permisosCompuestos;
            }
            catch
            {
                return permisos;
            }
        }

        public PermisoCompuesto LeerPermisoCompuesto(string nombreBuscado)
        {
            var permisos = LeerPermisosEnArbol();

            return (PermisoCompuesto)permisos.Find(p => p.getNombre() == nombreBuscado);
        }

        public List<Permiso> LeerPermisos(TipoPermiso tipo = TipoPermiso.Todos)
        {
            var permisos = new List<Permiso>();

            var filtros = new Dictionary<TipoPermiso, string>
            {
                { TipoPermiso.Simple,           "TipoPermiso = @tipo" },
                { TipoPermiso.Compuesto,        "TipoPermiso = @tipo" },
                { TipoPermiso.Rol,              "EsRolPermiso = 1" },
                { TipoPermiso.TodosExceptoRol,  "EsRolPermiso = 0" }
            };

            string query = "SELECT NombrePermiso, TipoPermiso FROM Permiso";

            if (tipo != TipoPermiso.Todos)
                query += $" WHERE {filtros[tipo]}";

            try
            {
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (tipo == TipoPermiso.Simple || tipo == TipoPermiso.Compuesto) cmd.Parameters.AddWithValue("@tipo", tipo.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader["NombrePermiso"].ToString();
                            string tipoBD = reader["TipoPermiso"].ToString();

                            if (tipoBD == "Simple")
                                permisos.Add(new PermisoSimple(nombre));
                            else
                                permisos.Add(new PermisoCompuesto(nombre));
                        }
                    }

                }
            }
            catch { }

            return permisos;
        }

        public bool InsertarPermiso(Permiso permiso, bool esRol)
        {
            string query = @"INSERT INTO Permiso (NombrePermiso, TipoPermiso, EsRolPermiso)
                             VALUES (@Nombre, @Tipo, @EsRol)";

            try
            {
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", permiso.getNombre());
                    cmd.Parameters.AddWithValue("@Tipo", permiso.EsCompuesto() ? "Compuesto" : "Simple");
                    cmd.Parameters.AddWithValue("@EsRol", esRol);

                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch { return false; }
        }

        public bool InsertarRelacion(string compuesto, string incluido)
        {
            string query = @"INSERT INTO PermisoRelacion (CompuestoNombre, IncluidoNombre)
                             VALUES (@Compuesto, @Incluido)";

            try
            {
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Compuesto", compuesto);
                    cmd.Parameters.AddWithValue("@Incluido", incluido);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch { return false; }
        }

        public bool EliminarPermiso(string permiso)
        {
            try
            {
                string queryValidar = "SELECT COUNT(*) FROM Usuario WHERE Rol = @Nombre";

                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(queryValidar, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", permiso);

                    if ((int)cmd.ExecuteScalar() > 0)
                        return false;
                }

                EliminarRelaciones(permiso);

                string query = "DELETE FROM Permiso WHERE NombrePermiso = @Nombre";

                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", permiso);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch { return false; }
        }

        public bool EliminarRelaciones(string nombre)
        {
            string query = "DELETE FROM PermisoRelacion WHERE CompuestoNombre = @Nombre OR IncluidoNombre = @Nombre";

            try
            {
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch { return false; }
        }

        public bool ModificarPermiso(string nombre, string nuevoNombre)
        {
            try
            {
                string queryPermiso = "UPDATE Permiso SET NombrePermiso = @Nuevo WHERE NombrePermiso = @Viejo";

                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(queryPermiso, conn))
                {
                    cmd.Parameters.AddWithValue("@Viejo", nombre);
                    cmd.Parameters.AddWithValue("@Nuevo", nuevoNombre);
                    cmd.ExecuteNonQuery();
                }

                string queryRelaciones = @"
                    UPDATE PermisoRelacion
                    SET CompuestoNombre = CASE WHEN CompuestoNombre = @Viejo THEN @Nuevo ELSE CompuestoNombre END,
                        IncluidoNombre = CASE WHEN IncluidoNombre = @Viejo THEN @Nuevo ELSE IncluidoNombre END";

                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(queryRelaciones, conn))
                {
                    cmd.Parameters.AddWithValue("@Viejo", nombre);
                    cmd.Parameters.AddWithValue("@Nuevo", nuevoNombre);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch { return false; }
        }

        public bool EsRol(string nombre)
        {
            string query = "SELECT EsRolPermiso FROM Permiso WHERE NombrePermiso = @Nombre";

            try
            {
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);

                    var resultado = cmd.ExecuteScalar();
                    return resultado != null && resultado.ToString() == "True";
                }
            }
            catch { return false; }
        }

        public bool ExistePermiso(string nombre)
        {
            string query = "SELECT TOP 1 1 FROM Permiso WHERE NombrePermiso = @Nombre";

            try
            {
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    return cmd.ExecuteScalar() != null;
                }
            }
            catch { return false; }
        }

    }
}
