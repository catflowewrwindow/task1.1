using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;


namespace laba1
{
    public partial class Form1 : Form
    {
        string connectionString = ("Server=localhost;Port=5432;Database=db;Username=postgres;Password=0000;");
        public Form1()
        {
            InitializeComponent();
            FillDataGridView();
        }
        private void FillDataGridView()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем значения из текстовых полей
            string id = textBox4.Text;
            string name = textBox1.Text;
            string course = textBox2.Text;
            string status = textBox3.Text;
            // Проверяю, чтобы все поля были заполнены
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(course) || string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            try
            {
                // Открываю соединение с базой данных
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки данных
                    string insertQuery = "INSERT INTO users (id_users, name, course, status) VALUES (@id, @name, @course, @status)";

                    // Создаю команду с параметрами
                    using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                        cmd.Parameters.AddWithValue("@name", name);

                        // Преобразую значение course в long
                        if (long.TryParse(course, out long courseValue))
                        {
                            cmd.Parameters.AddWithValue("@course", courseValue);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка преобразования значения 'course' в число.");
                            return;
                        }

                        cmd.Parameters.AddWithValue("@status", status);

                        // Выполняем запрос
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Данные успешно добавлены в таблицу.");
                    }
                }
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Получаем значения из текстовых полей
            string id = textBox5.Text;
            string name = textBox8.Text;
            string course = textBox7.Text;
            string status = textBox6.Text;

            // Проверяем, чтобы все поля были заполнены
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(course) || string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE users SET name = @name, course = @course, status = @status WHERE id_users = @id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                        cmd.Parameters.AddWithValue("@name", name);

                        if (long.TryParse(course, out long courseValue))
                        {
                            cmd.Parameters.AddWithValue("@course", courseValue);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка преобразования значения 'course' в число.");
                            return;
                        }

                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Данные успешно обновлены в таблице.");
                    }
                }

                FillDataGridView(); // Обновляем отображение в DataGridView после обновления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}");
            }
        }
    }
}
