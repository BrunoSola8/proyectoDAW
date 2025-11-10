using ENTIDADES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Tecnico
{
    public class BitacoraDAL
    {
        public int GuardarLog(string pOperacion, string pUsuario)
        {
            string query = @"
        INSERT INTO Bitacora (Fecha, Operacion, Usuario) 
        VALUES (@Fecha, @Operacion, @Usuario);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand CM = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                CM.Parameters.AddWithValue("@Fecha", DateTime.Now);
                CM.Parameters.AddWithValue("@Operacion", pOperacion);
                CM.Parameters.AddWithValue("@Usuario", pUsuario);

                int nuevoId = (int)CM.ExecuteScalar();

                

                return nuevoId;
            }
        }


        public List<Bitacora> ObtenerLogs()
        {
            List<Bitacora> listaLogs = new List<Bitacora>();
            string query = "SELECT * FROM Bitacora ORDER BY ID DESC";

            using (SqlCommand CM = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                
                using (SqlDataReader DR = CM.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        Bitacora log = new Bitacora(int.Parse(DR[0].ToString()), DateTime.Parse(DR[1].ToString()), DR[2].ToString(), DR[3].ToString());
                        listaLogs.Add(log);
                    }
                }
            }
            return listaLogs;
        }


        public List<Bitacora> FiltrosBitacora(DateTime? desde, DateTime? hasta, string usuario, string operacion)
        {
            List<Bitacora> listaLogs = new List<Bitacora>();

            string query = "SELECT * FROM Bitacora WHERE 1=1";

            if (desde.HasValue)
                query += " AND Fecha >= @Desde";

            if (hasta.HasValue)
                query += " AND Fecha <= @Hasta";

            if (!string.IsNullOrEmpty(usuario))
                query += " AND Usuario LIKE @Usuario";

            if (!string.IsNullOrEmpty(operacion))
                query += " AND Operacion LIKE @Operacion";
            query += " ORDER BY ID DESC";
            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                if (desde.HasValue)
                    cmd.Parameters.AddWithValue("@Desde", desde.Value);

                if (hasta.HasValue)
                    cmd.Parameters.AddWithValue("@Hasta", hasta.Value);

                if (!string.IsNullOrEmpty(usuario))
                    cmd.Parameters.AddWithValue("@Usuario", "%" + usuario + "%");

                if (!string.IsNullOrEmpty(operacion))
                    cmd.Parameters.AddWithValue("@Operacion", "%" + operacion + "%");

                
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Bitacora log = new Bitacora(int.Parse(dr["ID"].ToString()), DateTime.Parse(dr["Fecha"].ToString()), dr["Operacion"].ToString(), dr["Usuario"].ToString());
                        listaLogs.Add(log);
                    }
                }
            }
            return listaLogs;
        }
    }
}
