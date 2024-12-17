using System;
using System.Data;
using MySql.Data.MySqlClient; // Importar el conector MySQL
using System.Windows.Forms;

namespace proyectofinalcruds
{
    public partial class RegistrarUsuario : Form
    {
        // Cadena de conexión a la base de datos MySQL
        private string connectionString = "Server=localhost;Database=clinicadb;Uid=root;Pwd=;";

        public RegistrarUsuario()
        {
            InitializeComponent();
            CargarDatos();  // Cargar los datos al iniciar el formulario
        }

        // Método para cargar los datos en el DataGridView
        private void CargarDatos()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Id, nombre, rol, contrasena FROM usuarios";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Asignar los datos al DataGridView
                    dgvUsuarios.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message);
                }
            }
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsuarios.Rows[e.RowIndex];
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtRol.Text = row.Cells["rol"].Value.ToString();
                txtContrasena.Text = row.Cells["contrasena"].Value.ToString();
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombre.Text;
            string rolUsuario = txtRol.Text;
            string contrasenaUsuario = txtContrasena.Text;

            if (RegistrarNuevoUsuario(nombreUsuario, rolUsuario, contrasenaUsuario))
            {
                MessageBox.Show("Usuario registrado correctamente.");
                CargarDatos();
            }
            else
            {
                MessageBox.Show("Hubo un error al registrar el usuario.");
            }
        }

        private bool RegistrarNuevoUsuario(string nombre, string rol, string contrasena)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO usuarios (nombre, rol, contrasena, fecha_creacion, fecha_modificacion) " +
                                   "VALUES (@nombre, @rol, @contrasena, CURRENT_TIMESTAMP(), CURRENT_TIMESTAMP())";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@rol", rol);
                    command.Parameters.AddWithValue("@contrasena", contrasena);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar el usuario: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombre.Text;
            string rolUsuario = txtRol.Text;
            string contrasenaUsuario = txtContrasena.Text;
            int usuarioId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);

            if (ActualizarUsuario(usuarioId, nombreUsuario, rolUsuario, contrasenaUsuario))
            {
                MessageBox.Show("Usuario actualizado correctamente.");
                CargarDatos();
            }
            else
            {
                MessageBox.Show("Hubo un error al actualizar el usuario.");
            }
        }

        private bool ActualizarUsuario(int id, string nombre, string rol, string contrasena)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE usuarios SET nombre = @nombre, rol = @rol, contrasena = @contrasena, " +
                                   "fecha_modificacion = CURRENT_TIMESTAMP() WHERE Id = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@rol", rol);
                    command.Parameters.AddWithValue("@contrasena", contrasena);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el usuario: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int usuarioId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);

            if (EliminarUsuario(usuarioId))
            {
                MessageBox.Show("Usuario eliminado correctamente.");
                CargarDatos();
            }
            else
            {
                MessageBox.Show("Hubo un error al eliminar el usuario.");
            }
        }

        private bool EliminarUsuario(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM usuarios WHERE Id = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el usuario: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnRegreso_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
    }
}
