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
using System.IO;

namespace Zapatería_ElChele.Forms
{
    public partial class GestionProductosForm : Form
    {
        private List<Producto> productos;
        private List<Categoria> categorias;
        public GestionProductosForm()
        {
            InitializeComponent();
            productos = FileManager.LeerDeArchivo<Producto>("Binarios/productos.dat")?? new List<Producto>();
            categorias = FileManager.LeerDeArchivo<Categoria>("categorias.dat") ?? new List<Categoria>();
            CargarProductos();
            ActualizarComboBoxCategorias();
            ActualizarGrafico();
        }

        private void CargarProductos()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productos;
        }
        private void ActualizarComboBoxCategorias()
        {
            // Filtrar las categorías activas
            var categoriasActivas = categorias.Where(c => c.Estado).ToList();

            if (!categoriasActivas.Any())
            {
                MessageBox.Show("No hay categorías activas disponibles. Por favor, agregue categorías primero.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.DataSource = null;
                return;
            }

            // Configurar ComboBox con categorías activas
            comboBox1.DataSource = null;
            comboBox1.DataSource = categoriasActivas;
            comboBox1.DisplayMember = "Nombre";  // Mostrar el nombre en el ComboBox
            comboBox1.ValueMember = "id";       // Usar el ID como valor interno
        }
        private void ActualizarGrafico()
        {
            chartProductosxCategoria.Series.Clear();
            chartProductosxCategoria.Series.Add("Productos");
            chartProductosxCategoria.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            var categorias = productos.GroupBy(p => p.Categoria)
                                       .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
                                       .ToList();

            foreach (var categoria in categorias)
            {
                chartProductosxCategoria.Series[0].Points.AddXY(categoria.Categoria, categoria.Cantidad);
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombreProducto.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecioProducto.Text) ||
                    string.IsNullOrWhiteSpace(txtStock.Text) ||
                    comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Todos los campos son obligatorios.");
                    return;
                }

                var categoriaSeleccionada = (Categoria)comboBox1.SelectedItem;

                var nuevoProducto = new Producto
                {
                    Nombre = txtNombreProducto.Text,
                    PrecioUnitario = decimal.Parse(txtPrecioProducto.Text),
                    Categoria = categoriaSeleccionada.Nombre,
                    Stock = int.Parse(txtStock.Text)
                };

                productos.Add(nuevoProducto);
                FileManager.GuardarEnArchivo("Binarios/productos.dat", productos);
                MessageBox.Show("Producto agregado correctamente.");
                LimpiarCampos();
                CargarProductos();
                ActualizarGrafico();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.SelectedRows.Count > 0)
                {
                    var productoSeleccionado = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;
                    var categoriaSeleccionada = (Categoria)comboBox1.SelectedItem;

                    productoSeleccionado.Nombre = txtNombreProducto.Text;
                    productoSeleccionado.PrecioUnitario = decimal.Parse(txtPrecioProducto.Text);
                    productoSeleccionado.Categoria = categoriaSeleccionada.Nombre;
                    productoSeleccionado.Stock = int.Parse(txtStock.Text);

                    FileManager.GuardarEnArchivo("Binarios/productos.dat", productos);
                    MessageBox.Show("Producto actualizado correctamente.");
                    LimpiarCampos();
                    CargarProductos();
                    ActualizarGrafico();
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para editar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el producto: " + ex.Message);
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.SelectedRows.Count > 0)
                {
                    var productoSeleccionado = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;

                    if (MessageBox.Show($"¿Está seguro de eliminar el producto {productoSeleccionado.Nombre}?",
                        "Confirmar eliminación", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        productos.Remove(productoSeleccionado);
                        FileManager.GuardarEnArchivo("Binarios/productos.dat", productos);
                        MessageBox.Show("Producto eliminado correctamente.");
                        LimpiarCampos();
                        CargarProductos();
                        ActualizarGrafico();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message);
            }
        }
        private void LimpiarCampos()
        {
            txtNombreProducto.Clear();
            txtPrecioProducto.Clear();
            txtStock.Clear();
            comboBox1.SelectedIndex = -1;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void GestionProductosForm_Load(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void btnEliminar_Click_1(object sender, EventArgs e) 
        {
            try
            {
                if (dgvProductos.SelectedRows.Count > 0)
                {
                    var productoSeleccionado = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;

                    if (MessageBox.Show($"¿Está seguro de eliminar el producto {productoSeleccionado.Nombre}?",
                        "Confirmar eliminación", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        productos.Remove(productoSeleccionado);
                        FileManager.GuardarEnArchivo("Binarios/productos.dat", productos);
                        MessageBox.Show("Producto eliminado correctamente.");
                        LimpiarCampos();
                        CargarProductos();
                        ActualizarGrafico();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message);
            }
        }
        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.SelectedRows.Count > 0)
                {
                    var productoSeleccionado = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;
                    var categoriaSeleccionada = (Categoria)comboBox1.SelectedItem;

                    productoSeleccionado.Nombre = txtNombreProducto.Text;
                    productoSeleccionado.PrecioUnitario = decimal.Parse(txtPrecioProducto.Text);
                    productoSeleccionado.Categoria = categoriaSeleccionada.Nombre;
                    productoSeleccionado.Stock = int.Parse(txtStock.Text);

                    FileManager.GuardarEnArchivo("Binarios/productos.dat", productos);
                    MessageBox.Show("Producto actualizado correctamente.");
                    LimpiarCampos();
                    CargarProductos();
                    ActualizarGrafico();
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para editar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el producto: " + ex.Message);
            }
        }

    }
}