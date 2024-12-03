using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zapatería_ElChele.Ayudantes;
using Zapatería_ElChele.Modelos;

namespace Zapatería_ElChele.Forms
{
    public partial class GestionUsuariosForm : Form
    {

        private List<Usuario> usuarios;

        public GestionUsuariosForm()
        {
            InitializeComponent();

            usuarios = FileManager.LeerDeArchivo<Usuario>("Binarios/usuarios.dat") ?? new List<Usuario>();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = usuarios;

            dgvUsuarios.Columns["Contraseña"].Visible = false;
        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuarioID.Text) ||
                string.IsNullOrWhiteSpace(txtContraseña.Text) ||
                string.IsNullOrWhiteSpace(cbRol.Text) ||
                string.IsNullOrWhiteSpace(txtNombreUsuario.Text)
                )
            {
                MessageBox.Show("Por favor, llene todos los campos.");
                return;
            }

            //Verificar si el usuario ya existe
            if (usuarios.Any(u => u.UsuarioID == txtUsuarioID.Text))
            {
                MessageBox.Show("El usuario ya existe.");
                return;
            }

            //Agregar usuario
            var nuevoUsuario = new Usuario
            {
                UsuarioID = txtUsuarioID.Text,
                Nombre = txtNombreUsuario.Text,
                Rol = cbRol.Text,
                Activo = chkActivo.Checked,
                Contraseña = txtContraseña.Text

            };

            //Guardar usuario
            usuarios.Add(nuevoUsuario);
            FileManager.GuardarEnArchivo("Binarios/usuarios.dat", usuarios);

            //Limpiae los campos
            CargarUsuarios();
            LimpiarCampos();
            ActualizarDataGridView();
            MessageBox.Show("Usuario agregado correctamente.");
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario.");
                return;
            }

            var usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as Usuario;

            if (MessageBox.Show($"¿Está seguro de eliminar al usuario {usuarioSeleccionado.UsuarioID}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                usuarios.Remove(usuarioSeleccionado);
                FileManager.GuardarEnArchivo("Binarios/usuarios.dat", usuarios);
                CargarUsuarios();
                MessageBox.Show("Usuario eliminado correctamente.");
            }
            ActualizarDataGridView();
        }


        private void LimpiarCampos()
        {
            txtUsuarioID.Clear();
            txtNombreUsuario.Clear();
            cbRol.SelectedIndex = -1;
            chkActivo.Checked = false;
            txtContraseña.Clear();
        }
        private void GestionUsuariosForm_Load(object sender, EventArgs e) { }

        private void lblUsuarioID_Click(object sender, EventArgs e) { }
        private void LblUsuarioID_Click(object sender, EventArgs e) { }

        private void btnEditarUsuario_Click_1(object sender, EventArgs e)
        {
                if (dgvUsuarios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione un usuario.");
                    return;
                }

                var usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as Usuario;

                usuarioSeleccionado.UsuarioID = txtUsuarioID.Text;
                usuarioSeleccionado.Nombre = txtNombreUsuario.Text;
                usuarioSeleccionado.Rol = cbRol.Text;
                usuarioSeleccionado.Activo = chkActivo.Checked;
                usuarioSeleccionado.Contraseña = txtContraseña.Text;

                FileManager.GuardarEnArchivo("Binarios/usuarios.dat", usuarios);
                CargarUsuarios();
                MessageBox.Show("Usuario actualizado correctamente.");
                ActualizarDataGridView();
        }

        private void ActualizarDataGridView()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = usuarios;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtContraseña_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
