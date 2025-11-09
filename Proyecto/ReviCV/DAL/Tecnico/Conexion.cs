using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Conexion
    {
        private readonly string _connectionString =
            "Data Source=.;Initial Catalog=Proyecto_DAW;Integrated Security=True;";

        private static readonly Conexion instancia = new Conexion();
        public static Conexion Instancia => instancia;

        private Conexion() { }

        public SqlConnection ReturnConexion()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
