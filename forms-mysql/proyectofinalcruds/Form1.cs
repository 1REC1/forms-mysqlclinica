using System;
using MySql.Data.MySqlClient; // Importar el conector MySQL
using System.Windows.Forms;

namespace proyectofinalcruds
{
    public partial class Form1 : Form
    {
        // Cadena de conexión a la base de datos MySQL
        private string connectionString = "Server=localhost;Database=clinicadb;Uid=root;Pwd=;";

        public Form1()
        {
            InitializeComponent();
        }

        private void lblRegistrarse_Click(object sender, EventArgs e)
        {
            // Crear una instancia del formulario de registro
            RegistrarUsuario usuario = new RegistrarUsuario();

            // Mostrar el formulario de registro
            usuario.Show();

            // Ocultar el formulario de login
            this.Hide();
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;  // TextBox para el usuario
            string contrasena = txtPassword.Text;  // TextBox para la contraseña

            if (ValidarLogin(usuario, contrasena))
            {
                // Si el login es exitoso, muestra el formulario principal (Menu)
                Menu menuPrincipal = new Menu();
                menuPrincipal.Show();

                // Ocultar el formulario de login
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }

        // Método para validar el login en la base de datos MySQL
        private bool ValidarLogin(string usuario, string contrasena)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Consulta SQL para verificar el usuario y la contraseña
                    string query = "SELECT COUNT(*) FROM usuarios WHERE nombre = @usuario AND contrasena = @contrasena";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@usuario", usuario);
                    command.Parameters.AddWithValue("@contrasena", contrasena);

                    int count = Convert.ToInt32(command.ExecuteScalar());  // Devuelve el número de registros que coinciden

                    return count > 0;  // Si count > 0, el login es válido
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
