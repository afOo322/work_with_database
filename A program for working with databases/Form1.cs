using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Xml.Linq;


namespace A_program_for_working_with_databases
{
public partial class Form1 : Form
{
    private string connectionString = "Data Source=database.db;Version=3;";

    public Form1()
    {
        InitializeComponent();
        CreateDatabaseAndTable();
    }

    private void CreateDatabaseAndTable()
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS People (
                                       Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                       Name TEXT NOT NULL,
                                       Age INTEGER NOT NULL)";
            SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn);
            cmd.ExecuteNonQuery();
        }
    }

    private void LoadData()
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT * FROM People";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }

    private void btnLoadData_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string insertQuery = "INSERT INTO People (Name, Age) VALUES (@name, @age)";
            SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
            cmd.ExecuteNonQuery();
        }
        LoadData();
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedRows.Count > 0)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE People SET Name = @name, Age = @age WHERE Id = @id";
                SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            LoadData();
        }
        else
        {
            MessageBox.Show("Выберите запись для обновления.");
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedRows.Count > 0)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM People WHERE Id = @id";
                SQLiteCommand cmd = new SQLiteCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            LoadData();
        }
        else
        {
            MessageBox.Show("Выберите запись для удаления.");
        }
    }
}
}

