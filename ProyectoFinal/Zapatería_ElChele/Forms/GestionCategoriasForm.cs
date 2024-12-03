using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Zapatería_ElChele.Ayudantes;
using Zapatería_ElChele.Modelos;
using System.IO;

namespace Zapatería_ElChele.Forms
{
    public partial class GestionCategoriasForm : Form
    {
        private List<Categoria> categorias;
        public GestionCategoriasForm()
        {
            InitializeComponent();
        }

        private void GestionCategoriasForm_Load(object sender, EventArgs e)
        {

            // Asegurar que el archivo existe
            if (!File.Exists("categorias.dat"))
            {
                FileManager.GuardarEnArchivo("categorias.dat", new List<Categoria>());
            }
            // Leer categorías existentes
            ActualizarCategorias(); // Asegúrate de cargar las categorías al abrir el formulario
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            // Validar campos
            if (string.IsNullOrWhiteSpace(txtCategoriaID.Text) || string.IsNullOrWhiteSpace(txtDescripcionCategoria.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Leer categorías existentes
            categorias = FileManager.LeerDeArchivo<Categoria>("categorias.dat") ?? new List<Categoria>();

            // Crear nueva categoría
            var nuevaCategoria = new Categoria
            {
                id = categorias.Count > 0 ? categorias.Max(c => c.id) + 1 : 1,
                Nombre = txtCategoriaID.Text,
                Descripcion = txtDescripcionCategoria.Text,
                Estado = chkEstado.Checked
            };

            // Agregar y guardar
            categorias.Add(nuevaCategoria);
            FileManager.GuardarEnArchivo("categorias.dat", categorias);

            MessageBox.Show("Categoría agregada exitosamente.");
            ActualizarCategorias();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            if (dgvCategorias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una categoría para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener categoría seleccionada
            var categoriaSeleccionada = (Categoria)dgvCategorias.SelectedRows[0].DataBoundItem;

            // Validar campos
            if (string.IsNullOrWhiteSpace(txtCategoriaID.Text) || string.IsNullOrWhiteSpace(txtDescripcionCategoria.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            // Actualizar datos
            categoriaSeleccionada.Nombre = txtCategoriaID.Text;
            categoriaSeleccionada.Descripcion = txtDescripcionCategoria.Text;
            categoriaSeleccionada.Estado = chkEstado.Checked;

            // Guardar cambios
            FileManager.GuardarEnArchivo("categorias.dat", categorias);

            MessageBox.Show("Categoría editada exitosamente.");
            ActualizarCategorias();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dgvCategorias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una categoría para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener categoría seleccionada
            var categoriaSeleccionada = (Categoria)dgvCategorias.SelectedRows[0].DataBoundItem;

            // Confirmar eliminación
            var confirmResult = MessageBox.Show($"¿Está seguro de eliminar la categoría '{categoriaSeleccionada.Nombre}'?",
                                                "Confirmar Eliminación",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                // Eliminar categoría
                categorias.Remove(categoriaSeleccionada);
                FileManager.GuardarEnArchivo("categorias.dat", categorias);

                MessageBox.Show("Categoría eliminada exitosamente.");
                ActualizarCategorias();

            }
        }

        private void ActualizarCategorias()
        {
            // Leer categorías y actualizar el DataGridView
            categorias = FileManager.LeerDeArchivo<Categoria>("categorias.dat") ?? new List<Categoria>();
            dgvCategorias.DataSource = null; // Reiniciar fuente de datos
            dgvCategorias.DataSource = categorias;
        }

        private void txtNombre_TextChanged(object sender, EventArgs e) { }
    }
} 

