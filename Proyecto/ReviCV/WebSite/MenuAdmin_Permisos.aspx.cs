using SERVICIOS;
using SERVICIOS.Permisos;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MenuAdmin_Permisos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarRolesYGrupos();
            CargarArbolPermisos();
            CargarPermisosAsignados();
        }
    }

    private void CargarRolesYGrupos()
    {
        GestorPermisos gestorPermisos = new GestorPermisos();
        List<Permiso> permisos = gestorPermisos.ObtenerPermisos("Compuesto");
        permisos.RemoveAll(x => x.nombre == "Administrador");

        ddlRolesGrupos.DataSource = permisos.Select(p =>
            new {
                nombre = ((p as PermisoCompuesto).EsRol ? "ROL: " : "GRUPO: ") + p.nombre,
                valor = p.nombre
            }
        ).ToList();

        ddlRolesGrupos.DataTextField = "nombre";
        ddlRolesGrupos.DataValueField = "valor";
        ddlRolesGrupos.DataBind();


        ddlRolesGrupos.Items.Insert(0, new ListItem("-- Seleccione --", ""));
    }


    protected void ddlRolesGrupos_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool haySeleccion = ddlRolesGrupos.SelectedIndex > 0;

        btnEliminar.Enabled = haySeleccion;
        btnModificarNombre.Enabled = haySeleccion;
        btnGuardarCambios.Enabled = haySeleccion;

        CargarPermisosAsignados();
    }

    private void CargarPermisosAsignados()
    {
        chkListPermisos.Items.Clear();
        GestorPermisos gestorPermisos = new GestorPermisos();
        var permisosSimples = gestorPermisos.ObtenerPermisos("Todos excepto rol");

        foreach (var permiso in permisosSimples)
        {
            ListItem item = new ListItem(permiso.nombre, permiso.nombre);
            chkListPermisos.Items.Add(item);
        }

        string rolSeleccionado = ddlRolesGrupos.SelectedValue;
        List<Permiso> RootsPermits = gestorPermisos.ObtenerPermisosEnArbol();
        Permiso selected = RootsPermits.Find(x => x.nombre == ddlRolesGrupos.SelectedValue.ToString());

        if (selected is PermisoCompuesto compoundPermit)
        {
            foreach (Permiso p in compoundPermit.PermisosIncluidos) { MarcarPermisoEnLista(p); }
        }
    }

    private void MarcarPermisoEnLista(Permiso permiso)
    {
        for (int i = 0; i < chkListPermisos.Items.Count; i++)
        {
            if (chkListPermisos.Items[i].Value == permiso.nombre)
            {
                chkListPermisos.Items[i].Selected = true;
                break;
            }
        }
    }
    private void CargarArbolPermisos()
    {
        treeViewPermisos.Nodes.Clear();
        GestorPermisos gestor = new GestorPermisos();

        // Obtenemos los permisos raíz compuestos
        var rootPermisos = gestor.ObtenerPermisosEnArbol();

        // Llamada recursiva para construir todo el árbol
        foreach (var permiso in rootPermisos)
        {
            AgregarNodoRecursivo(permiso, treeViewPermisos.Nodes);
        }
    }

    private void AgregarNodoRecursivo(Permiso permiso, TreeNodeCollection parentNodes)
    {
        string label = permiso is PermisoCompuesto c
            ? (c.EsRol ? "ROL: " : "GRUPO: ") + permiso.nombre
            : permiso.nombre;

        TreeNode nodo = new TreeNode(label);
        parentNodes.Add(nodo);

        if (permiso is PermisoCompuesto comp)
            foreach (var sub in comp.PermisosIncluidos)
                AgregarNodoRecursivo(sub, nodo.ChildNodes);
    }






    protected void chkListPermisos_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnGuardarCambios.Enabled = true;
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        string script = $@"
Swal.fire({{
    title: '¿Eliminar?',
    text: 'Esta acción eliminará el rol/grupo y todos sus vínculos.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Eliminar',
    cancelButtonText: 'Cancelar'
}}).then((result) => {{
    if (result.isConfirmed) {{
        __doPostBack('{btnEliminarConfirmar.UniqueID}', '');
    }}
}});
";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmEliminar", script, true);
    }

    protected void btnEliminarConfirmar_Click(object sender, EventArgs e)
    {
        GestorPermisos gestorPermisos = new GestorPermisos();
        gestorPermisos.QuitarPermiso(ddlRolesGrupos.Text);

        CargarRolesYGrupos();
        CargarArbolPermisos();
        CargarPermisosAsignados();
    }



    protected void btnModificarNombre_Click(object sender, EventArgs e)
    {
        string script = @"
Swal.fire({
    title: 'Modificar nombre',
    input: 'text',
    inputLabel: 'Nuevo nombre del permiso',
    inputPlaceholder: 'Escriba el nuevo nombre',
    showCancelButton: true,
    confirmButtonText: 'Guardar',
    cancelButtonText: 'Cancelar'
}).then((result) => {
    if (result.isConfirmed && result.value && result.value.trim() !== '') {
        document.getElementById('" + hfNuevoNombre.ClientID + @"').value = result.value;
        __doPostBack('" + btnConfirmarCambio.UniqueID + @"','');
    }
});
";

        ScriptManager.RegisterStartupScript(
            this.Page,
            this.Page.GetType(),
            "SwalModificarNombre",
            script,
            true
        );
    }


    protected void btnConfirmarCambio_Click(object sender, EventArgs e)
    {
        string nuevoNombre = hfNuevoNombre.Value;

        if (string.IsNullOrWhiteSpace(nuevoNombre)) return;

        GestorPermisos gestorPermisos = new GestorPermisos();
        gestorPermisos.ModificarNombrePermiso(ddlRolesGrupos.SelectedValue, nuevoNombre);

        // refrescamos todo
        CargarRolesYGrupos();
        CargarArbolPermisos();
        CargarPermisosAsignados();
    }


    protected void btnCrearRol_Click(object sender, EventArgs e)
    {
        GeneracionDePermisoCompuesto(true);
    }

    protected void btnCrearGrupo_Click(object sender, EventArgs e)
    {
        GeneracionDePermisoCompuesto(false);
    }

    private void GeneracionDePermisoCompuesto(bool esRol)
    {
        if (txtNuevoNombre.Text == "")
        {
            string script = @"
Swal.fire({
    title: 'Error',
    text: 'El nombre del permiso no puede estar vacío.',
    icon: 'error',
    confirmButtonText: 'Aceptar'
});";


            ScriptManager.RegisterStartupScript(
                this.Page,
                this.Page.GetType(),
                "SwalError",
                script,
                true
            );
            return;   
        }
        GestorPermisos gestorPermisos = new GestorPermisos();
        if (gestorPermisos.ExistePermiso(txtNuevoNombre.Text))
        {
            string script = @"
Swal.fire({
    title: 'Error',
    text: 'Ya existe un permiso con ese nombre.',
    icon: 'error',
    confirmButtonText: 'Aceptar'
});";


            ScriptManager.RegisterStartupScript(
                this.Page,
                this.Page.GetType(),
                "SwalError",
                script,
                true
            );
            return;   
        }
        if (!gestorPermisos.AgregarPermisoCompuesto(txtNuevoNombre.Text, AgregarPermisosCheckeadosAPermisoSeleccionado(txtNuevoNombre.Text), esRol))
        {
            string script = @"
Swal.fire({
    title: 'Error',
    text: 'Hubo un error al guardar el permiso.',
    icon: 'error',
    confirmButtonText: 'Aceptar'
});";


            ScriptManager.RegisterStartupScript(
                this.Page,
                this.Page.GetType(),
                "SwalError",
                script,
                true
            );
            return;
        }
        txtNuevoNombre.Text = "";
        CargarRolesYGrupos();
        CargarArbolPermisos();
        CargarPermisosAsignados();
    }

    public List<string> AgregarPermisosCheckeadosAPermisoSeleccionado(string nombrePermiso)
    {
        List<string> items = new List<string>();
        foreach (ListItem item in chkListPermisos.Items) if (item.Selected) items.Add(item.Text.ToString());

        return items;
    }


    protected void btnGuardarCambios_Click(object sender, EventArgs e)
    {
        GestorPermisos gestorPermisos = new GestorPermisos();
        if(!gestorPermisos.ModificarPermisoCompuesto(ddlRolesGrupos.Text, AgregarPermisosCheckeadosAPermisoSeleccionado(txtNuevoNombre.Text)))
        {
            string script = @"
Swal.fire({
    title: 'Error',
    text: 'Hubo un error al modificar el permiso.',
    icon: 'error',
    confirmButtonText: 'Aceptar'
});";


            ScriptManager.RegisterStartupScript(
                this.Page,
                this.Page.GetType(),
                "SwalError",
                script,
                true
            );
            return;
        }
        CargarRolesYGrupos();
        CargarArbolPermisos();
        CargarPermisosAsignados();
    }

    #region TOPBAR

    protected void btnInicio_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin.aspx");
    }

    protected void btnUsuarios_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin_Usuarios.aspx");
    }

    protected void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        GestorBitacora gestorBitacora = new GestorBitacora();
        gestorBitacora.GuardarLogBitacora("Logout", Session["username"].ToString());
        Session.Clear();
        Response.Redirect("LandingPage.aspx");
    }

    protected void btnVolverALanding_Click(object sender, EventArgs e)
    {
        Response.Redirect("LandingPage.aspx");
    }

    protected void btnBitacora_Click(object sender, EventArgs e)
    {
        Response.Redirect("BitacoraPage.aspx");
    }

    protected void btnRubrosIdiomas_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin_RubrosIdiomas.aspx");
    }

    #endregion
}