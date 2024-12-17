using System;
using System.Data;
using MySql.Data.MySqlClient; // Importar el conector MySQL
using System.Windows.Forms;

namespace proyectofinalcruds
{
    public partial class Medicos : Form
    {
        // Cadena de conexión a la base de datos MySQL
        public string conexionBD = "Server=localhost;Database=clinicadb;Uid=root;Pwd=;";

        public Medicos()
        {
            InitializeComponent();
            LoadMedicos();
            dgvMedicos.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selección de fila completa
            dgvMedicos.MultiSelect = false; // Permitir selección de una sola fila
        }

        // Método para cargar los médicos en el DataGridView
        private void LoadMedicos()
        {
            using (MySqlConnection connection = new MySqlConnection(conexionBD))
            {
                string query = "SELECT Id, Nombre, Apellido, Especialidad, Telefono, Estado FROM medicos";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvMedicos.DataSource = dataTable;
            }
        }

        // Evento para insertar un nuevo médico
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombres.Text;
            string apellido = txtApellidos.Text;
            string especialidad = txtEspecialidad.Text;
            string telefono = txtTelefono.Text;

            using (MySqlConnection connection = new MySqlConnection(conexionBD))
            {
                string query = "INSERT INTO medicos (Nombre, Apellido, Especialidad, Telefono, Fecha_Creacion, Estado) " +
                               "VALUES (@Nombre, @Apellido, @Especialidad, @Telefono, CURRENT_TIMESTAMP(), 'Activo')";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@Apellido", apellido);
                command.Parameters.AddWithValue("@Especialidad", especialidad);
                command.Parameters.AddWithValue("@Telefono", telefono);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            LoadMedicos();
            LimpiarCampos();
        }

        // Evento para actualizar un médico seleccionado
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvMedicos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvMedicos.SelectedRows[0].Cells["Id"].Value);
                string nombre = txtNombres.Text;
                string apellido = txtApellidos.Text;
                string especialidad = txtEspecialidad.Text;
                string telefono = txtTelefono.Text;

                using (MySqlConnection connection = new MySqlConnection(conexionBD))
                {
                    string query = "UPDATE medicos SET Nombre = @Nombre, Apellido = @Apellido, Especialidad = @Especialidad, " +
                                   "Telefono = @Telefono, Fecha_Modificacion = CURRENT_TIMESTAMP() WHERE Id = @Id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Apellido", apellido);
                    command.Parameters.AddWithValue("@Especialidad", especialidad);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                LoadMedicos();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un médico para actualizar.");
            }
        }

        // Evento para eliminar un médico seleccionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMedicos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvMedicos.SelectedRows[0].Cells["Id"].Value);

                using (MySqlConnection connection = new MySqlConnection(conexionBD))
                {
                    string query = "DELETE FROM medicos WHERE Id = @Id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                LoadMedicos();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un médico para eliminar.");
            }
        }

        // Evento para capturar la selección del DataGridView y mostrar los datos en los campos de texto
        private void dgvMedicos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMedicos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvMedicos.SelectedRows[0];
                txtNombres.Text = row.Cells["Nombre"].Value.ToString();
                txtApellidos.Text = row.Cells["Apellido"].Value.ToString();
                txtEspecialidad.Text = row.Cells["Especialidad"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
            }
        }

        // Método para limpiar los campos de texto
        private void LimpiarCampos()
        {
            txtNombres.Clear();
            txtApellidos.Clear();
            txtEspecialidad.Clear();
            txtTelefono.Clear();
        }

        // Botón para regresar al menú principal
        private void btnRegreso_Click(object sender, EventArgs e)
        {
            Menu menuPrincipal = new Menu();
            menuPrincipal.Show();
            this.Hide();
        }
    }
}
