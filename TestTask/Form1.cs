using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TestTask
{
    public partial class Form1 : Form
    {
        static string location = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\TestTask\\TestDB.mdf";
        public string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + location + ";Initial Catalog = TestDB; Integrated Security = True";
        private ContextDataContext linq;

        public Form1()
        {
            InitializeComponent();
            // cream instanta a clasei OleDbConnection
            linq = new ContextDataContext(connection);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var marfaList = linq.Vanzari.ToList();
            foreach (var marfa in marfaList)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Tag = marfa.Id;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = marfa.Id;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = marfa.Denumire;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = marfa.Data;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = marfa.Pret;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = marfa.Cantitate;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = (marfa.Cantitate * marfa.Pret);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var linq = new ContextDataContext(connection);
            var marfaOrdonata = linq.Vanzari.OrderBy(el => el.Denumire).ThenBy(el => el.Data).ToList();
            foreach (var marfa in marfaOrdonata)
            {

                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Tag = marfa.Id;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = marfa.Id;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = marfa.Denumire;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = marfa.Data;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = marfa.Pret;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = marfa.Cantitate;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = (marfa.Cantitate * marfa.Pret);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var linq = new ContextDataContext(connection);
            Vanzari newmarfa = new Vanzari
            {
                Denumire = tbMarfa.Text,
                Data = dtData.Value,
                Pret = Decimal.Parse(tbPret.Text),
                Cantitate = Decimal.Parse(tbCantitate.Text)
            };
            linq.Vanzari.InsertOnSubmit(newmarfa);
            linq.SubmitChanges();
            MessageBox.Show("Datele au fost salvate cu success");
            button7_Click(null, null);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //lbPretMaxim.Text = "Pret maxim: ";
            var linq = new ContextDataContext(connection);
            var pretMaxim = linq.Vanzari.Where(el => el.Denumire == tbMarfa.Text).OrderByDescending(el => el.Pret).ToList();
            lbPretMaxim.Text += $"{pretMaxim[0].Pret} pentru {pretMaxim[0].Denumire}";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tbMarfa.Text = tbPret.Text = tbCantitate.Text =  "";
            lbPretMaxim.Text = "Pret maxim: ";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var linq = new ContextDataContext(connection);
            var pretMaxim = linq.Vanzari.OrderBy(el => el.Data).ToArray();
            decimal? suma = 0;
            decimal? sumaMax = 0;
            DateTime? datamax = new DateTime();
         
            for(int i = 1; i < pretMaxim.Length; i++)
            {
                if(pretMaxim[i].Data == pretMaxim[i - 1].Data)
                {
                    suma += pretMaxim[i].Cantitate * pretMaxim[i].Pret;
                    datamax = pretMaxim[i].Data;
                }
                else
                {
                    sumaMax = suma;
                    suma = 0;
                    datamax = pretMaxim[i].Data;
                }
            }

            lbVenitMaxim.Text = $"Data cu venit maxim : {datamax}";
        }
    }
}
