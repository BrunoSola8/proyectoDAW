using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using ENTIDADES;

/// <summary>
/// Descripción breve de ServicioWeb
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
// [System.Web.Script.Services.ScriptService]
public class ServicioWeb : System.Web.Services.WebService
{

    public ServicioWeb()
    {

        //Elimine la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    [WebMethod]
    public void SerializarUsuario(Usuario pUsuario)
    {
        XmlSerializer xml = new XmlSerializer(typeof(Usuario));
        string nombreArchivo = $"usuario_{DateTime.Now:ddMMyy_HHmm}.xml";
        string ruta = Server.MapPath($"~/Archivos/{nombreArchivo}");

        string carpeta = Path.GetDirectoryName(ruta);
        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }
        using (StreamWriter writer = new StreamWriter(ruta))
        {
            xml.Serialize(writer, pUsuario);
        }
    }

}
