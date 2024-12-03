using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Zapatería_ElChele.Ayudantes;
using Zapatería_ElChele.Modelos;

namespace ProyectoZapateriaElChele.Forms
{
    public partial class GestionVentasForm : Form
    {
        private List<Producto> productos; // Lista de productos cargados
        private List<Venta> ventas; // Lista de ventas realizadas
        private List<Usuario> usuarios; // Lista de usuarios

        public GestionVentasForm()
        {
            InitializeComponent();

            try
            {
                // Leer los datos desde los archivos binarios
                productos = FileManager.LeerDeArchivo<Producto>("Binarios/productos.dat") ?? new List<Producto>();
                ventas = FileManager.LeerDeArchivo<Venta>("Binarios/ventas.dat") ?? new List<Venta>();
                usuarios = FileManager.LeerDeArchivo<Usuario>("Binarios/usuarios.dat") ?? new List<Usuario>();

                // Cargar datos en los controles
                CargarProductos();
                CargarVendedores();
                CargarVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }
        }

        private void CargarProductos()
        {
            try
            {
                if (productos == null || !productos.Any())
                {
                    MessageBox.Show("No hay productos disponibles.");
                    cmbProducto.DataSource = null;
                    return;
                }

                cmbProducto.DataSource = null;
                cmbProducto.DataSource = productos;
                cmbProducto.DisplayMember = "Nombre"; // Nombre visible
                cmbProducto.ValueMember = "Nombre"; // Valor interno
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

            private void CargarVendedores()
            {
                try
                {
                var vendedores = usuarios?.Where(u => u.Rol == "Ventas").ToList();

                if (vendedores == null || !vendedores.Any())
                {
                    MessageBox.Show("No hay vendedores disponibles.");
                    cmbVendedor.DataSource = null;
                    return;
                }

                cmbVendedor.DataSource = null;
                cmbVendedor.DataSource = vendedores;
                cmbVendedor.DisplayMember = "Nombre";
                cmbVendedor.ValueMember = "UsuarioID";
                }
                catch (Exception ex)
                {
                MessageBox.Show($"Error al cargar vendedores: {ex.Message}");
                }
            }

        private void CargarVentas()
        {
            try
            {
                if (ventas == null || !ventas.Any())
                {
                    MessageBox.Show("No hay ventas registradas.");
                    dgvVentas.DataSource = null;
                    return;
                }

                var datos = ventas.Select(v => new
                {
                    Producto = v.NombreProducto ?? "Sin nombre",
                    Cantidad = v.CantidadVendida,
                    Total = v.Total
                }).ToList();

                dgvVentas.DataSource = datos;

                // Actualizar gráfico
                ActualizarGrafico();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas: {ex.Message}");
            }
        }

        private void ActualizarGrafico()
        {
            try
            {
                if (chartVentas.Series.Count == 0)
                {
                    chartVentas.Series.Add("Ventas");
                }

                chartVentas.Series[0].Points.Clear();
                foreach (var venta in ventas)
                {
                    if (venta != null && !string.IsNullOrEmpty(venta.NombreProducto))
                    {
                        chartVentas.Series[0].Points.AddXY(venta.NombreProducto, venta.Total);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el gráfico: {ex.Message}");
            }
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedValue == null)
                {
                    txtPrecioUnitario.Clear();
                    return;
                }

                var productoSeleccionado = productos.FirstOrDefault(p => p.Nombre == cmbProducto.SelectedValue.ToString());
                if (productoSeleccionado != null)
                {
                    txtPrecioUnitario.Text = productoSeleccionado.PrecioUnitario.ToString();
                    ActualizarTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar producto: {ex.Message}");
            }
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            // Calcular el total al cambiar la cantidad
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            try
            {
                if (decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) &&
                    int.TryParse(txtCantidad.Text, out int cantidad))
                {
                    txtTotal.Text = (precioUnitario * cantidad).ToString();
                }
                else
                {
                    txtTotal.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular el total: {ex.Message}");
            }
        }

        private void btnAgregarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedItem == null || cmbVendedor.SelectedItem == null || string.IsNullOrEmpty(txtCantidad.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                var productoSeleccionado = productos.FirstOrDefault(p => p.Nombre == cmbProducto.SelectedValue.ToString());
                if (productoSeleccionado == null)
                {
                    MessageBox.Show("El producto seleccionado no es válido.");
                    return;
                }

                var nuevaVenta = new Venta(string.Empty, string.Empty, 0, 0, DateTime.Now)
                {
                    NombreProducto = productoSeleccionado.Nombre,
                    CantidadVendida = int.Parse(txtCantidad.Text),
                    PrecioUnitario = productoSeleccionado.PrecioUnitario,
                    Usuario = cmbVendedor.SelectedValue.ToString(),
                    FechaVenta = DateTime.Now,
                    Total = productoSeleccionado.PrecioUnitario * int.Parse(txtCantidad.Text)

                };

                ventas.Add(nuevaVenta);
                FileManager.GuardarEnArchivo("Binarios/ventas.dat", ventas);

                CargarVentas();

                MessageBox.Show("Venta registrada correctamente.");
                txtCantidad.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar venta: {ex.Message}");
            }
        }

        private void btnEditarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar que se haya seleccionado una venta
                if (dgvVentas.SelectedRows.Count > 0)
                {
                    // Obtener el índice de la fila seleccionada
                    int indiceSeleccionado = dgvVentas.SelectedRows[0].Index;

                    // Validar que el índice sea válido
                    if (indiceSeleccionado >= 0 && indiceSeleccionado < ventas.Count)
                    {
                        // Obtener la venta seleccionada en base al índice
                        var ventaSeleccionada = ventas[indiceSeleccionado];

                        // Validar que se haya ingresado una nueva cantidad válida
                        if (int.TryParse(txtCantidad.Text, out int nuevaCantidad) && nuevaCantidad > 0)
                        {
                            // Actualizar la venta con los nuevos valores
                            ventaSeleccionada.CantidadVendida = nuevaCantidad;
                            ventaSeleccionada.Total = nuevaCantidad * ventaSeleccionada.PrecioUnitario;

                            // Guardar los cambios en el archivo binario
                            FileManager.GuardarEnArchivo("Binarios/ventas.dat", ventas);

                            // Actualizar la rejilla del DataGridView
                            CargarVentas();

                            MessageBox.Show("Venta editada correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, ingrese una cantidad válida.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la venta seleccionada.");
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una venta para editar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar la venta: {ex.Message}");
            }
        }

        private void btnEliminarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar que se haya seleccionado una venta
                if (dgvVentas.SelectedRows.Count > 0)
                {
                    // Obtener el índice de la fila seleccionada
                    int indiceSeleccionado = dgvVentas.SelectedRows[0].Index;

                    // Validar que el índice sea válido
                    if (indiceSeleccionado >= 0 && indiceSeleccionado < ventas.Count)
                    {
                        // Confirmar eliminación
                        var confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar esta venta?",
                                                           "Confirmación",
                                                           MessageBoxButtons.YesNo,
                                                           MessageBoxIcon.Question);

                        if (confirmacion == DialogResult.Yes)
                        {
                            // Eliminar la venta de la lista
                            ventas.RemoveAt(indiceSeleccionado);

                            // Guardar los cambios en el archivo binario
                            FileManager.GuardarEnArchivo("Binarios/ventas.dat", ventas);

                            // Actualizar la rejilla del DataGridView
                            CargarVentas();

                            MessageBox.Show("Venta eliminada correctamente.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la venta seleccionada.");
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una venta para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la venta: {ex.Message}");
            }
        }

        private void dgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void gpbGestionVentas_Enter(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void lblCantidadVendida_Click(object sender, EventArgs e) { }

        private void dgvVentas_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void btnEliminarVenta_Click_1(object sender, EventArgs e)
        {
             try
            {
                // Verificar que se haya seleccionado una venta
                if (dgvVentas.SelectedRows.Count > 0)
                {
                    // Obtener el índice de la fila seleccionada
                    int indiceSeleccionado = dgvVentas.SelectedRows[0].Index;

                    // Validar que el índice sea válido
                    if (indiceSeleccionado >= 0 && indiceSeleccionado < ventas.Count)
                    {
                        // Confirmar eliminación
                        var confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar esta venta?",
                                                           "Confirmación",
                                                           MessageBoxButtons.YesNo,
                                                           MessageBoxIcon.Question);

                        if (confirmacion == DialogResult.Yes)
                        {
                            // Eliminar la venta de la lista
                            ventas.RemoveAt(indiceSeleccionado);

                            // Guardar los cambios en el archivo binario
                            FileManager.GuardarEnArchivo("Binarios/ventas.dat", ventas);

                            // Actualizar la rejilla del DataGridView
                            CargarVentas();

                            MessageBox.Show("Venta eliminada correctamente.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la venta seleccionada.");
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una venta para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la venta: {ex.Message}");
            }
        }
    }
}