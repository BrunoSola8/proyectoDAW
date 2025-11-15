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

        public List<PermisoCompuesto> LeerPermisosEnArbol()
        {
            var map = new Dictionary<string, Permiso>();
            var compuestos = new Dictionary<string, PermisoCompuesto>();

            try
            {
                // 1) Leer permisos
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand("SELECT * FROM Permiso", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nombre = reader["NombrePermiso"].ToString();
                        string tipo = reader["TipoPermiso"].ToString();
                        bool esRol = reader.GetBoolean(reader.GetOrdinal("EsRolPermiso"));

                        Permiso permiso;

                        if (tipo == "Compuesto")
                        {
                            var comp = new PermisoCompuesto(nombre, esRol);
                            compuestos[nombre] = comp;
                            permiso = comp;
                        }
                        else
                        {
                            permiso = new PermisoSimple(nombre);
                        }

                        map[nombre] = permiso;
                    }
                }

                // 2) Leer relaciones y armar el árbol
                using (var conn = Conexion.Instancia.ReturnConexion())
                using (var cmd = new SqlCommand("SELECT * FROM PermisoRelacion", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string padre = reader["CompuestoNombre"].ToString();
                        string hijo = reader["IncluidoNombre"].ToString();

                        // SEGURO: existe en el diccionario
                        var compuesto = (PermisoCompuesto)map[padre];
                        var incluido = map[hijo];

                        compuesto.AgregarPermiso(incluido);
                    }
                }

                return compuestos.Values.ToList();
            }
            catch
            {
                // si falla, devolvés un árbol potencialmente incompleto, pero al menos consistente.
                return compuestos.Values.ToList();
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

            string query = "SELECT NombrePermiso, TipoPermiso, EsRolPermiso FROM Permiso";

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
                            bool rol = reader.GetBoolean(reader.GetOrdinal("EsRolPermiso"));

                            if (tipoBD == "Simple")
                                permisos.Add(new PermisoSimple(nombre));
                            else
                                permisos.Add(new PermisoCompuesto(nombre, rol));
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
