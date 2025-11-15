using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Permisos
{
    public class PermisoCompuesto : Permiso
    {
        private readonly List<Permiso> permisos = new List<Permiso>();

        public PermisoCompuesto(string nombre, bool esRol) : base(nombre) 
        {
            EsRol = esRol;
        }

        public override void AgregarPermiso(Permiso permiso) => permisos.Add(permiso);

        public override void RemoverPermiso(Permiso permiso) => permisos.Remove(permiso);

        public override bool EsCompuesto() => true;

        public bool EsRol;

        public IReadOnlyCollection<Permiso> PermisosIncluidos => permisos.AsReadOnly();


        public Permiso BuscarPermiso(Permiso permisoBuscado)
        {
            if (this == permisoBuscado) return this;

            foreach (var permiso in permisos.Where(p => p.EsCompuesto()))
            {
                var encontrado = (permiso as PermisoCompuesto).BuscarPermiso(permisoBuscado);
                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        public bool ContienePermiso(string nombrePermiso)
        {
            if (getNombre() == nombrePermiso)
                return true;

            return permisos
                .Where(p => p.EsCompuesto())
                .Cast<PermisoCompuesto>()
                .Any(permiso => permiso.ContienePermiso(nombrePermiso));
        }
    }
}
