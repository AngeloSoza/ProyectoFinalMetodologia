using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Zapatería_ElChele.Modelos;
using Zapatería_ElChele.Ayudantes;
using System.IO;

namespace Zapatería_ElChele.Forms
{
    public partial class LoginForm : Form
    {

        private void VerificarEstructuraDeArchivos()
        {
            string rutaCarpeta = "Binarios";
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta); // Crear carpeta si no existe
            }

            // Crear archivos si no existen
            CrearArchivoSiNoExiste<Usuario>("Binarios/usuarios.dat");
            CrearArchivoSiNoExiste<Categoria>("Binarios/categorias.dat");
            CrearArchivoSiNoExiste<Producto>("Binarios/productos.dat");
            CrearArchivoSiNoExiste<Venta>("Binarios/ventas.dat");
        }

        private void CrearArchivoSiNoExiste<T>(string ruta)
        {
            if (!File.Exists(ruta))
            {
                FileManager.GuardarEnArchivo(ruta, new List<T>()); // Crear archivo vacío
            }
        }
        private void VerificarUsuarioAdmin()
        {
            string rutaUsuarios = "Binarios/usuarios.dat";

            // Leer los usuarios existentes
            var usuarios = FileManager.LeerDeArchivo<Usuario>(rutaUsuarios) ?? new List<Usuario>();

            // Si no hay usuarios en el archivo, agregar un usuario administrador predeterminado
            if (usuarios.Count == 0)
            {
                var usuarioAdmin = new Usuario
                {
                    UsuarioID = "admin",
                    Contraseña = "admin123",
                    Rol = "Administrador",
                    Activo = true
                };

                usuarios.Add(usuarioAdmin);

                // Guardar el usuario administrador en el archivo binario
                FileManager.GuardarEnArchivo(rutaUsuarios, usuarios);
                MessageBox.Show("Se ha creado un usuario administrador predeterminado: \nUsuario: admin\nContraseña: admin123", "Información");
            }
        }
        public LoginForm()
        {
            try
            {
                InitializeComponent();
                VerificarUsuarioAdmin();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar la aplicación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

            string rutaArchivo = "usuarios.dat";

            if (!File.Exists(rutaArchivo))
            {
                var usuarios = new List<Usuario>
                {
                    new Usuario 
                    {
                        UsuarioID = "admin",
                        Contraseña = "123",
                        Nombre = "Administrador",
                        Rol = "Administrador",
                        Activo = true
                    }
                };

                FileManager.GuardarEnArchivo(rutaArchivo, usuarios);
                MessageBox.Show("Se creó el archivo de usuarios. Usuario: admin, Contraseña: 123");
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuarioId = txtUsuario.Text.Trim();
            string contraseña = txtContraseña.Text;

            if (string.IsNullOrEmpty(usuarioId) || string.IsNullOrEmpty(contraseña))
            {
                MessageBox.Show("Por favor, ingrese su usuario y contraseña.");
                return;
            }

            var usuarios = FileManager.LeerDeArchivo<Usuario>("usuarios.dat");
            if (usuarios == null || usuarios.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios en el sistema. Verifique el archivo.");
                return;
            }

            var usuario = usuarios.FirstOrDefault(u => u.UsuarioID == usuarioId && u.Contraseña == contraseña);
            if (usuario == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
                return;
            }

            if (!usuario.Activo)
            {
                MessageBox.Show("El usuario está inactivo. Contacte al administrador.");
                return;
            }



            // Usuario válido: abre el Dashboard.
            DashboardForm dashboardForm = new DashboardForm(usuario);
            dashboardForm.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var usuarios = FileManager.LeerDeArchivo<Usuario>("Binarios/usuarios.dat");
            if (usuarios == null || usuarios.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios en el archivo.", "Error");
                return;
            }

            var usuario = usuarios.FirstOrDefault(u => u.UsuarioID == txtUsuario.Text && u.Contraseña == txtContraseña.Text);
            if (usuario == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error");
                return;
            }

            MessageBox.Show("Inicio de sesión exitoso.");
            this.Hide();
            DashboardForm dashboard = new DashboardForm(usuario);
            dashboard.ShowDialog();
            this.Close();
        }

        private void gpbInicioSesion_Enter(object sender, EventArgs e)
        {
            // Evento opcional al entrar al grupo de inicio de sesión.
        }

        private void txtContraseña_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
