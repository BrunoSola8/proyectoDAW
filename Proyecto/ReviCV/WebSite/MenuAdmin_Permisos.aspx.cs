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
        ddlRolesGrupos.DataSource = gestorPermisos.ObtenerPermisos("Compuesto");
        ddlRolesGrupos.DataTextField = "nombre";
        ddlRolesGrupos.DataValueField = "nombre";
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
        var permisosSimples = gestorPermisos.ObtenerPermisos("Todo excepto rol");

        foreach (var permiso in permisosSimples)
        {
            ListItem item = new ListItem(permiso.nombre, permiso.nombre);
            chkListPermisos.Items.Add(item);
        }

        // Marcar los permisos que ya tiene el rol/grupo seleccionado
        string rolSeleccionado = ddlRolesGrupos.SelectedValue;
        List<Permiso> RootsPermits = gestorPermisos.ObtenerPermisosEnArbol();
        Permiso selected = RootsPermits.Find(x => x.nombre == ddlRolesGrupos.SelectedItem.ToString());

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
        TreeNode nodo = new TreeNode(permiso.nombre);
        parentNodes.Add(nodo);

        // Si es un permiso compuesto, agregamos sus hijos
        if (permiso is PermisoCompuesto compuesto)
        {
            foreach (var subPermiso in compuesto.PermisosIncluidos)
            {
                AgregarNodoRecursivo(subPermiso, nodo.ChildNodes);
            }
        }
    }




    protected void chkListPermisos_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnGuardarCambios.Enabled = true;
    }

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

    protected void btnEliminar_Click(object sender, EventArgs e)
    {

    }

    protected void btnModificarNombre_Click(object sender, EventArgs e)
    {

    }

    protected void btnCrearRol_Click(object sender, EventArgs e)
    {
        GestorPermisos gestorPermisos = new GestorPermisos();
        List<string> items = new List<string>();
        foreach (ListItem item in chkListPermisos.Items) if (item.Selected) items.Add(item.Text.ToString());
        gestorPermisos.AgregarPermisoCompuesto(txtNuevoNombre.Text, items, true);
        CargarRolesYGrupos();
        CargarArbolPermisos();
    }

    protected void btnCrearGrupo_Click(object sender, EventArgs e)
    {
        CargarRolesYGrupos();
        CargarArbolPermisos();
    }

    protected void btnGuardarCambios_Click(object sender, EventArgs e)
    {
        CargarRolesYGrupos();
        CargarArbolPermisos();
    }
}