using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoZapateriaElChele.Forms;
using Zapatería_ElChele.Modelos;


namespace Zapatería_ElChele.Forms
{
    public partial class DashboardForm : Form
    {

        private Usuario _usuario;
        public DashboardForm(Usuario usuario)
        {

            InitializeComponent();
            _usuario = usuario;
        }

        private void categoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionCategoriasForm categoriasForm = new GestionCategoriasForm();
            categoriasForm.ShowDialog();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionProductosForm productosForm = new GestionProductosForm();
            productosForm.ShowDialog();
        }

        private void gestionarVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionVentasForm ventasForm = new GestionVentasForm();
            ventasForm.ShowDialog();

        }

        private void generarReportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void DashboardForm_Load(object sender, EventArgs e) { }

        private void btnGestionUsuarios_Click(object sender, EventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionUsuariosForm usuariosForm = new GestionUsuariosForm();
            usuariosForm.ShowDialog();
        }
    }
}
