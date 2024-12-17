using System;
using System.Data;
using MySql.Data.MySqlClient; // Importar el conector MySQL
using System.Windows.Forms;

namespace proyectofinalcruds
{
    public partial class Pacientes : Form
    {
        // String de conexión a la base de datos MySQL
        public string conexionBD = "Server=localhost;Database=clinicadb;Uid=root;Pwd=;";

        public Pacientes()
        {
            InitializeComponent();
            CargarPacientes();
        }

        // Evento para el botón Regreso
        private void btnRegreso_Click(object sender, EventArgs e)
        {
            Menu menuPrincipal = new Menu();
            menuPrincipal.Show();
            this.Hide();
        }

        // Método para cargar los datos de la tabla en el DataGridView
        private void CargarPacientes()
        {
            using (MySqlConnection conexion = new MySqlConnection(conexionBD))
            {
                string query = "SELECT Id, Nombre, Apellido, Sexo, Edad, Fecha_Creacion, Estado FROM pacientes";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPacientes.DataSource = dt;
            }
        }

        // Evento para el botón Insertar
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexion = new MySqlConnection(conexionBD))
            {
                string query = "INSERT INTO pacientes (Nombre, Apellido, Sexo, Edad, Usuario_Creador, Fecha_Creacion, Estado) " +
                               "VALUES (@Nombre, @Apellido, @Sexo, @Edad, @Usuario_Creador, CURRENT_TIMESTAMP(), @Estado)";

                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Nombre", txtNombres.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellidos.Text);
                cmd.Parameters.AddWithValue("@Sexo", cmbxSexo.SelectedItem?.ToString() ?? "N/A");
                cmd.Parameters.AddWithValue("@Edad", Convert.ToInt32(txtEdad.Text));
                cmd.Parameters.AddWithValue("@Usuario_Creador", 1); // Cambiar según usuario actual
                cmd.Parameters.AddWithValue("@Estado", "Activo");

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                MessageBox.Show("Paciente insertado correctamente.");
                CargarPacientes();
                LimpiarCampos();
            }
        }

        // Evento para el botón Actualizar
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvPacientes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvPacientes.CurrentRow.Cells["Id"].Value);

                using (MySqlConnection conexion = new MySqlConnection(conexionBD))
                {
                    string query = "UPDATE pacientes SET Nombre = @Nombre, Apellido = @Apellido, Sexo = @Sexo, Edad = @Edad, " +
                                   "Usuario_Modificador = @Usuario_Modificador, Fecha_Modificacion = CURRENT_TIMESTAMP(), Estado = @Estado " +
                                   "WHERE Id = @Id";

                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombres.Text);
                    cmd.Parameters.AddWithValue("@Apellido", txtApellidos.Text);
                    cmd.Parameters.AddWithValue("@Sexo", cmbxSexo.SelectedItem?.ToString() ?? "N/A");
                    cmd.Parameters.AddWithValue("@Edad", Convert.ToInt32(txtEdad.Text));
                    cmd.Parameters.AddWithValue("@Usuario_Modificador", 1); // Cambiar según usuario actual
                    cmd.Parameters.AddWithValue("@Estado", "Activo");
                    cmd.Parameters.AddWithValue("@Id", id);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    conexion.Close();
                }

                MessageBox.Show("Paciente actualizado correctamente.");
                CargarPacientes();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Seleccione un paciente para actualizar.");
            }
        }

        // Evento para el botón Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPacientes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvPacientes.CurrentRow.Cells["Id"].Value);

                using (MySqlConnection conexion = new MySqlConnection(conexionBD))
                {
                    string query = "DELETE FROM pacientes WHERE Id = @Id";
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    conexion.Close();
                }

                MessageBox.Show("Paciente eliminado correctamente.");
                CargarPacientes();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Seleccione un paciente para eliminar.");
            }
        }

        // Evento para seleccionar una fila del DataGridView
        private void dgvPacientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPacientes.Rows[e.RowIndex];
                txtNombres.Text = row.Cells["Nombre"].Value.ToString();
                txtApellidos.Text = row.Cells["Apellido"].Value.ToString();
                cmbxSexo.SelectedItem = row.Cells["Sexo"].Value.ToString();
                txtEdad.Text = row.Cells["Edad"].Value.ToString();
            }
        }

        // Método para limpiar los campos después de una acción
        private void LimpiarCampos()
        {
            txtNombres.Clear();
            txtApellidos.Clear();
            cmbxSexo.SelectedIndex = -1;
            txtEdad.Clear();
        }
    }
}
