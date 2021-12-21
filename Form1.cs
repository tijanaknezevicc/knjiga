using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Knjiga
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string cs = "Data source = KOMPAC\\SQLEXPRESS; Initial catalog = knjiga; Integrated security = true";
        int current = 0;
        DataTable data = new DataTable();
        SqlConnection connection;
        string naslov, autor, povez;
        int brStrana;
        SqlDataAdapter adapter;

        void customRefresh()
        {
            if (data.Rows.Count == 0)
            {
                buttonBeg.Enabled = false;
                buttonEnd.Enabled = false;

                buttonPrev.Enabled = false;
                buttonNext.Enabled = false;

                buttonDelete.Enabled = false;
                buttonUpdate.Enabled = false;

                Clear();
                
            }
            else
            {
                labelId.Text = data.Rows[current]["id"].ToString();
                textBoxNaslov.Text = data.Rows[current]["naslov"].ToString();
                textBoxAutor.Text = data.Rows[current]["autor"].ToString();
                textBoxBrStr.Text = data.Rows[current]["brStrana"].ToString();
                textBoxPovez.Text = data.Rows[current]["povez"].ToString();

                buttonEnd.Enabled = (current != data.Rows.Count - 1);
                buttonNext.Enabled = (current != data.Rows.Count - 1);
                buttonBeg.Enabled = (current != 0);
                buttonPrev.Enabled = (current != 0);
                buttonDelete.Enabled = true;
                buttonUpdate.Enabled = true;
            }
        }

        void Clear()
        {
            labelId.Text = "";
            textBoxNaslov.Text = "";
            textBoxAutor.Text = "";
            textBoxBrStr.Text = "";
            textBoxPovez.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(cs);
            adapter = new SqlDataAdapter("select * from knjiga", connection);
            adapter.Fill(data);
            customRefresh();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(cs);
            naslov = textBoxNaslov.Text;
            autor = textBoxAutor.Text;
            brStrana = Convert.ToInt32(textBoxBrStr.Text);
            povez = textBoxPovez.Text;

            connection.Open();
            SqlCommand doIt = new SqlCommand($"insert into knjiga values('{naslov}','{autor}','{brStrana}','{povez}')", connection);
            doIt.ExecuteNonQuery();
            connection.Close();
            data.Clear();

            adapter = new SqlDataAdapter("select * from knjiga", connection);
            adapter.Fill(data);
            current = data.Rows.Count - 1;

            customRefresh();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(cs);
            naslov = textBoxNaslov.Text;
            autor = textBoxAutor.Text;
            brStrana = Convert.ToInt32(textBoxBrStr.Text);
            povez = textBoxPovez.Text;

            connection.Open();
            SqlCommand doIt = new SqlCommand($"update knjiga set naslov = '{naslov}', autor = '{autor}', brStrana = '{brStrana}', povez = '{povez}' where id = {labelId.Text}", connection);
            doIt.ExecuteNonQuery();
            connection.Close();

            data.Clear();
            adapter = new SqlDataAdapter("select * from knjiga", connection);
            adapter.Fill(data);
            customRefresh();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(cs);
            SqlCommand doIt = new SqlCommand(String.Format($"delete from knjiga where id={labelId.Text}"), connection);
            connection.Open();
            doIt.ExecuteNonQuery();
            connection.Close();

            data.Clear();
            adapter = new SqlDataAdapter("select * from knjiga", connection);
            adapter.Fill(data);
            current = 0;
            customRefresh();
        }

        private void buttonBeg_Click(object sender, EventArgs e)
        {
            current = 0;
            customRefresh();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (current - 1 >= 0)
            {
                current--;
                customRefresh();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (current + 1 <= data.Rows.Count)
            {
                current++;
                customRefresh();
            }
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            current = data.Rows.Count - 1;
            customRefresh();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
