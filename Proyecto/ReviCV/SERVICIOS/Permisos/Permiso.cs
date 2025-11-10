using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Permisos
{
    public abstract class Permiso
    {
        public string nombre { get; private set; }

        protected Permiso(string nNombre) { nombre = nNombre; }

        public virtual void AgregarPermiso(Permiso nPermiso) { }

        public virtual void RemoverPermiso(Permiso nPermiso) { }

        public virtual bool EsCompuesto() { return false; }

        public string getNombre() {  return nombre; }
    }
}
