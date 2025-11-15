using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Permisos
{
    public class GestorPermisos
    {
        private readonly PermisoDAO dao = new PermisoDAO();

        public bool ExistePermiso(string nombre)
        {
            return dao.ExistePermiso(nombre);
        }

        public bool AgregarPermisoCompuesto(string nombre, List<string> permisos, bool esRol)
        {
            if (dao.ExistePermiso(nombre)) return false;

            var arbolPermisos = dao.LeerPermisosEnArbol();

            foreach (var permiso in permisos)
            {
                var compuesto = arbolPermisos
                    .OfType<PermisoCompuesto>()
                    .FirstOrDefault(x => x.getNombre() == permiso);

                if (compuesto != null && compuesto.ContienePermiso(nombre)) return false;
            }


            var nuevoPermiso = new PermisoCompuesto(nombre, esRol);
            dao.InsertarPermiso(nuevoPermiso, esRol);

            foreach (var permiso in permisos) dao.InsertarRelacion(nombre, permiso);

            return true;
        }

        public bool QuitarPermiso(string nombre) => dao.EliminarPermiso(nombre);

        public void ModificarNombrePermiso(string nombre, string nuevoNombre)
        {
            dao.ModificarPermiso(nombre, nuevoNombre);
        }

        public bool ModificarPermisoCompuesto(string nombre, List<string> permisos)
        {
            var arbolPermisos = dao.LeerPermisosEnArbol();

            if (permisos.Contains(nombre))
                return false;

            foreach (var permiso in permisos)
            {
                var compuesto = arbolPermisos
                    .OfType<PermisoCompuesto>()
                    .FirstOrDefault(x => x.getNombre() == permiso);

                if (compuesto != null && compuesto.ContienePermiso(nombre))
                    return false;
            }

            dao.EliminarRelaciones(nombre);

            foreach (var permiso in permisos)
                dao.InsertarRelacion(nombre, permiso);

            return true;
        }

        public List<Permiso> ObtenerPermisos(string tipo = "")
        {
            TipoPermiso tipoConvertido;

            switch (tipo.Trim().ToLower())
            {
                case "simple":
                    tipoConvertido = TipoPermiso.Simple;
                    break;

                case "compuesto":
                    tipoConvertido = TipoPermiso.Compuesto;
                    break;

                case "rol":
                    tipoConvertido = TipoPermiso.Rol;
                    break;

                case "todosexceptorol":
                case "todos excepto rol":
                    tipoConvertido = TipoPermiso.TodosExceptoRol;
                    break;

                default:
                    tipoConvertido = TipoPermiso.Todos;
                    break;
            }

            return dao.LeerPermisos(tipoConvertido);
        }

        public List<Permiso> ObtenerPermisosEnArbol() => dao.LeerPermisosEnArbol();

        public bool EsRol(string nombrePermiso) => dao.EsRol(nombrePermiso);

        public PermisoCompuesto ObtenerPermisoCompuesto(string nombre) => dao.LeerPermisoCompuesto(nombre);
    }
}
