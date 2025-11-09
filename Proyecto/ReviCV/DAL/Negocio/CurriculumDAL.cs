using ENTIDADES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CurriculumDAL
    {
        public int GuardarCurriculum(Curriculum cv)
        {
            string query = $@"
        INSERT INTO {TablasBD.Curriculum} (UsernameUsuario, Curriculum, Idioma, Rubro, NombreArchivo)
        VALUES (@UsernameUsuario, @Curriculum, @Idioma, @Rubro, @Nombre);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                

                cmd.Parameters.AddWithValue("@UsernameUsuario", cv.Usuario);
                cmd.Parameters.AddWithValue("@Curriculum", cv.ArchivoCV);
                cmd.Parameters.AddWithValue("@Idioma", cv.Idioma.Item1);
                cmd.Parameters.AddWithValue("@Rubro", cv.Rubro.Item1);
                cmd.Parameters.AddWithValue("@Nombre", cv.Nombre);

                int id = (int)cmd.ExecuteScalar();

                
                return id;
            }
        }

        public int AltaRubro(string NombreRubro)
        {
            string query = $@"
        INSERT INTO {TablasBD.Rubro} (Rubro)
        VALUES (@Rubro);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var conn = Conexion.Instancia.ReturnConexion())
            {
                
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Rubro", NombreRubro);
                    int nuevoId = (int)cmd.ExecuteScalar();
                    
                    return nuevoId;
                }
            }
        }

        public void BajaRubro(int ID)
        {
            string query = $"Delete from {TablasBD.Rubro} where ID_Rubro = @Id";

            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                cmd.Parameters.AddWithValue("@Id", ID);
                cmd.ExecuteNonQuery();
                
            }
        }
        public bool RubroEnUso(int idRubro)
        {
            string query = "SELECT COUNT(*) FROM Curriculum WHERE Rubro = @Rubro";

            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                cmd.Parameters.AddWithValue("@Rubro", idRubro);
                
                int cantidad = (int)cmd.ExecuteScalar();
                

                return cantidad == 0;
            }
        }
        public void ModificarRubro(int ID, string nNombreRubro)
        {
            string query = $"UPDATE {TablasBD.Rubro} SET Rubro = @Rubro WHERE ID_Rubro = @Id";

            using (var conn = Conexion.Instancia.ReturnConexion())
            {
                
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Rubro", nNombreRubro);
                    cmd.Parameters.AddWithValue("@Id", ID);
                    cmd.ExecuteNonQuery();
                }
                
            }
        }

        public int AltaIdioma(string NombreIdioma)
        {
            string query = $@"
        INSERT INTO {TablasBD.Idioma} (Idioma)
        VALUES (@Idioma);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var conn = Conexion.Instancia.ReturnConexion())
            {
                
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Idioma", NombreIdioma);
                    int nuevoId = (int)cmd.ExecuteScalar();
                    
                    return nuevoId;
                }
            }
        }

        public void BajaIdioma(int ID)
        {
            string query = $"Delete from {TablasBD.Idioma} where ID_Idioma = @Id";

            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                cmd.Parameters.AddWithValue("@Id", ID);
                cmd.ExecuteNonQuery();
                
            }
        }
        public bool IdiomaEnUso(int idIdioma)
        {
            string query = "SELECT COUNT(*) FROM Curriculum WHERE Idioma = @Idioma";

            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                cmd.Parameters.AddWithValue("@Idioma", idIdioma);
                
                int cantidad = (int)cmd.ExecuteScalar();
                

                return cantidad == 0;
            }
        }
        public void ModificarIdioma(int ID, string nNombreIdioma)
        {
            string query = $"UPDATE {TablasBD.Idioma} SET Idioma = @Idioma WHERE ID_Idioma = @Id";

            using (var conn = Conexion.Instancia.ReturnConexion())
            {
                
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Idioma", nNombreIdioma);
                    cmd.Parameters.AddWithValue("@Id", ID);
                    cmd.ExecuteNonQuery();
                }
                
            }
        }

        public Curriculum ObtenerCurriculumFiltrado(string rubro, string idioma)
        {
            Curriculum cv = null;
            int idCV = 0;
            string username = null;
            byte[] archivoBytes = null;

            List<string> condiciones = new List<string>();
            List<SqlParameter> parametros = new List<SqlParameter>();

            // Consulta base con TOP 1 y selección aleatoria
            string queryBase = $"SELECT TOP 1 ID_CV, UsernameUsuario, Curriculum FROM {TablasBD.Curriculum} WHERE 1=1";

            // Agregar filtros dinámicamente
            if (!string.IsNullOrEmpty(rubro))
            {
                condiciones.Add("Rubro = @Rubro");
                parametros.Add(new SqlParameter("@Rubro", rubro));
            }

            if (!string.IsNullOrEmpty(idioma))
            {
                condiciones.Add("Idioma = @Idioma");
                parametros.Add(new SqlParameter("@Idioma", idioma));
            }

            // Unir condiciones con AND y agregar ORDER BY NEWID()
            string whereClause = string.Join(" AND ", condiciones);
            string finalQuery = $"{queryBase} {(condiciones.Count > 0 ? " AND " + whereClause : "")} ORDER BY NEWID()";

            using (SqlConnection conn = Conexion.Instancia.ReturnConexion())
            {
                using (SqlCommand cmd = new SqlCommand(finalQuery, conn))
                {
                    cmd.Parameters.AddRange(parametros.ToArray());
                    

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            idCV = dr.GetInt32(dr.GetOrdinal("ID_CV"));
                            username = dr.GetString(dr.GetOrdinal("UsernameUsuario"));

                            if (!dr.IsDBNull(dr.GetOrdinal("Curriculum")))
                            {
                                long length = dr.GetBytes(dr.GetOrdinal("Curriculum"), 0, null, 0, 0);
                                archivoBytes = new byte[length];
                                dr.GetBytes(dr.GetOrdinal("Curriculum"), 0, archivoBytes, 0, (int)length);
                            }
                        }
                    } // El SqlDataReader se cierra automáticamente aquí

                    
                }
            }

            cv = new Curriculum
            {
                ID_CV = idCV,
                Usuario = username,
                ArchivoCV = archivoBytes
            };
            return cv;
        }

        public Curriculum ObtenerCurriculumPorID(int id)
        {
            Curriculum cv = null;
            string username = null;

            string query = $"SELECT ID_CV, UsernameUsuario, Curriculum FROM {TablasBD.Curriculum} WHERE ID_CV = @ID";

            using (SqlConnection conn = Conexion.Instancia.ReturnConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    

                    byte[] archivoBytes = null;
                    int idCV = 0;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            idCV = dr.GetInt32(dr.GetOrdinal("ID_CV"));
                            username = dr.GetString(dr.GetOrdinal("UsernameUsuario"));

                            if (!dr.IsDBNull(dr.GetOrdinal("Curriculum")))
                            {
                                long length = dr.GetBytes(dr.GetOrdinal("Curriculum"), 0, null, 0, 0);
                                archivoBytes = new byte[length];
                                dr.GetBytes(dr.GetOrdinal("Curriculum"), 0, archivoBytes, 0, (int)length);
                            }
                        }
                    }

                    cv = new Curriculum();
                    cv.ID_CV = idCV;
                    cv.Usuario = username;
                    cv.ArchivoCV = archivoBytes;
                }

                return cv;
            }
        }

        public Dictionary<int, string> ObtenerIdiomas()
        {
            Dictionary<int, string> idiomas = new Dictionary<int, string>();
            string query = $"SELECT ID_Idioma, Idioma FROM {TablasBD.Idioma}";

            using (SqlConnection conn = Conexion.Instancia.ReturnConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("ID_Idioma"));
                            string nombre = reader.GetString(reader.GetOrdinal("Idioma"));
                            idiomas.Add(id, nombre);
                        }
                    }
                    
                }
            }

            return idiomas;
        }

        public Dictionary<int, string> ObtenerRubros()
        {
            Dictionary<int, string> rubros = new Dictionary<int, string>();
            string query = $"SELECT ID_Rubro, Rubro FROM {TablasBD.Rubro}";

            using (SqlConnection conn = Conexion.Instancia.ReturnConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("ID_Rubro"));
                            string nombre = reader.GetString(reader.GetOrdinal("Rubro"));
                            rubros.Add(id, nombre);
                        }
                    }
                    
                }
            }

            return rubros;
        }

        public List<Curriculum> ObtenerCurriculumsPorUsuario(string nombreUsuario)
        {
            List<Curriculum> listaCV = new List<Curriculum>();

            string query = $@"
        SELECT ID_CV, Curriculum, Idioma, Rubro, NombreArchivo 
        FROM {TablasBD.Curriculum} 
        WHERE UsernameUsuario = @UsernameUsuario";

            using (SqlConnection conn = Conexion.Instancia.ReturnConexion())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UsernameUsuario", nombreUsuario);

                

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int idCV = dr.GetInt32(dr.GetOrdinal("ID_CV"));
                        int idIdioma = dr.IsDBNull(dr.GetOrdinal("Idioma")) ? -1 : dr.GetInt32(dr.GetOrdinal("Idioma"));
                        int idRubro = dr.IsDBNull(dr.GetOrdinal("Rubro")) ? -1 : dr.GetInt32(dr.GetOrdinal("Rubro"));
                        string nombreArchivo = dr.GetString(dr.GetOrdinal("NombreArchivo"));

                        byte[] archivoBytes = null;
                        if (!dr.IsDBNull(dr.GetOrdinal("Curriculum")))
                        {
                            long length = dr.GetBytes(dr.GetOrdinal("Curriculum"), 0, null, 0, 0);
                            archivoBytes = new byte[length];
                            dr.GetBytes(dr.GetOrdinal("Curriculum"), 0, archivoBytes, 0, (int)length);
                        }

                        // Agregar a la lista
                        Curriculum cv = new Curriculum
                        {
                            ID_CV = idCV,
                            Usuario = nombreUsuario,
                            ArchivoCV = archivoBytes,
                            Idioma = (idIdioma, ""),
                            Rubro = (idRubro, ""),
                            Nombre = nombreArchivo
                        };

                        listaCV.Add(cv);
                    }
                }

                
            }

            var idiomas = ObtenerIdiomas();
            var rubros = ObtenerRubros();

            foreach (var cv in listaCV)
            {
                if (idiomas.TryGetValue(cv.Idioma.Item1, out var nombreIdioma))
                    cv.Idioma = (cv.Idioma.Item1, nombreIdioma);

                if (rubros.TryGetValue(cv.Rubro.Item1, out var nombreRubro))
                    cv.Rubro = (cv.Rubro.Item1, nombreRubro);
            }

            return listaCV;
        }

        public void EliminarCurriculum(int idCV)
        {
            string query = $"DELETE FROM {TablasBD.Curriculum} WHERE ID_CV = @Id";

            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                cmd.Parameters.AddWithValue("@Id", idCV);
                cmd.ExecuteNonQuery();
                
            }
        }

    }
}
