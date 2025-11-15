using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Traducciones
{
    public class TraductorDAL
    {
        private static TraductorDAL Instance;
        public static TraductorDAL TranslatorInstance
        {
            get
            {
                if (Instance == null) Instance = new TraductorDAL();
                return Instance;
            }
        }
        public TraductorDAL()
        {
            traducciones = new Dictionary<string, string>();
        }

        List<IObserver> ObserversList;
        Dictionary<string, string> traducciones;
        string LenguajeActual;

        public string Traducir(string toTranslate)
        {
            try
            {
                string translation = "";
                return translation = traducciones[toTranslate];
            }
            catch (Exception ex) { return toTranslate; }
        }
        public void CargarTraduccionesDesdeBD(string idioma)
        {
           
            LenguajeActual = idioma;
            traducciones.Clear();
            var lista = ObtenerTraduccionesPorIdioma(idioma);

            foreach (var item in lista)
            {
                traducciones[item.Etiqueta] = item.Traduccion;
            }
        }
        public List<(string Etiqueta, string Traduccion)> ObtenerTraduccionesPorIdioma(string idioma)
        {
            List<(string Etiqueta, string Traduccion)> lista = new List<(string, string)>();
            string query = @"
        SELECT Etiqueta, Traduccion
        FROM Traducciones
        WHERE Idioma = @Idioma";

            using (SqlConnection conexion = Conexion.Instancia.ReturnConexion())
            using (SqlCommand cmd = new SqlCommand(query, conexion))
            {
                cmd.Parameters.AddWithValue("@Idioma", idioma);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string etiqueta = reader["Etiqueta"].ToString();
                        string traduccion = reader["Traduccion"].ToString();
                        lista.Add((etiqueta, traduccion));
                    }
                }
            }

            return lista;
        }
        public void Suscribe(IObserver nObserver)
        {
            ObserversList.Add(nObserver);
        }

        public void Unsuscribe(IObserver nObserver)
        {
            if (ObserversList.Contains(nObserver)) ObserversList.Remove(nObserver);
        }

        public void Notify()
        {
            foreach (IObserver obs in ObserversList)
            {
                obs.Actualizar();
            }
        }
    }
}
