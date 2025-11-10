using ENTIDADES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ResenaDAL
    {
        public int GuardarResena(Resena r)
        {
            string query = @"INSERT INTO Resena (Diseno, Comentarios, ID_CV, username_resenador, Contenido, Claridad, Relevancia) 
                     VALUES (@Diseno, @Comentarios, @ID_CV, @UsernameResenador, @Contenido, @Claridad, @Relevancia);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand cmd = new SqlCommand(query, Conexion.Instancia.ReturnConexion()))
            {
                

                cmd.Parameters.AddWithValue("@Diseno", r.Diseno);
                cmd.Parameters.AddWithValue("@Comentarios", r.Comentarios);
                cmd.Parameters.AddWithValue("@ID_CV", r.ID_CV);
                cmd.Parameters.AddWithValue("@UsernameResenador", r.UsuarioReseñador);
                cmd.Parameters.AddWithValue("@Contenido", r.Contenido);
                cmd.Parameters.AddWithValue("@Claridad", r.Claridad);
                cmd.Parameters.AddWithValue("@Relevancia", r.Relevancia);

                int nuevoId = (int)cmd.ExecuteScalar();

                

                return nuevoId;
            }
        }

    }
}
