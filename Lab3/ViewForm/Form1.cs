using Calculate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewForm
{
    public partial class Form1 : Form
    {
        private readonly int ROWS = 2;
        private readonly int COLOUMNS = 2;

        public Form1()
        {
            InitializeComponent();

            dataGridViewP.AllowUserToAddRows = false;
            dataGridViewP.AllowUserToDeleteRows = false;

            dataGridViewP.ColumnCount = COLOUMNS+1;
            dataGridViewP.RowCount = ROWS;

            dataGridViewP.ColumnHeadersVisible = true;

            dataGridViewP.Columns[1].Name = "To 1 SMO";
            dataGridViewP.Columns[2].Name = "To 2 SMO";

            dataGridViewP.Rows[0].Cells[0].Value = "From 1 SMO";
            dataGridViewP.Rows[1].Cells[0].Value = "From 2 SMO";

            dataGridViewP.Columns[0].DefaultCellStyle = new DataGridViewCellStyle() { BackColor =  Color.FromArgb(200, 200, 200) };
            dataGridViewP.Columns[0].ReadOnly = true;

            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            numericUpDownN.Value = Settings.N;
            textBoxMu1.Text = Settings.Mu[0].ToString();
            textBoxMu2.Text = Settings.Mu[1].ToString();

            dataGridViewP.Rows[0].Cells[1].Value = Settings.P[0, 0].ToString();
            dataGridViewP.Rows[0].Cells[2].Value = Settings.P[0, 1].ToString();
            dataGridViewP.Rows[1].Cells[1].Value = Settings.P[1, 0].ToString();
            dataGridViewP.Rows[1].Cells[2].Value = Settings.P[1, 1].ToString();

            UpdateForms();
        }

        private void buttonSaveValues_Click(object sender, EventArgs e)
        {
            try
            {
                TryToSetValues();

                UpdateForms();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TryToSetValues()
        {
            if(
               double.TryParse(textBoxMu1.Text, out double mu1)
            && double.TryParse(textBoxMu2.Text, out double mu2)
            && double.TryParse(dataGridViewP.Rows[0].Cells[1].Value.ToString(), out double p11)
            && double.TryParse(dataGridViewP.Rows[0].Cells[2].Value.ToString(), out double p12)
            && double.TryParse(dataGridViewP.Rows[1].Cells[1].Value.ToString(), out double p21)
            && double.TryParse(dataGridViewP.Rows[1].Cells[2].Value.ToString(), out double p22)
            )
            {
                if(
                   mu1 > 0
                && mu2 > 0
                && ((p11 + p12) == 1)
                && ((p21 + p22) == 1)
                )
                {
                    Settings.N = (int)numericUpDownN.Value;
                    Settings.Mu[0] = mu1;
                    Settings.Mu[1] = mu2;
                    Settings.P[0, 0] = p11;
                    Settings.P[0, 1] = p12;
                    Settings.P[1, 0] = p21;
                    Settings.P[1, 1] = p22;
                }
                else
                {
                    throw new ArgumentException("Incorrect values!");
                }                
            }
            else
            {
                throw new ArgumentException("Incorrect format of data!");
            }
        }

        private void UpdateForms()
        {
            var calc = new Calculate.Calculate();


            textBoxLi1.Text = calc.Li(0).ToString();
            textBoxRi1.Text = calc.Ri(0).ToString();
            textBoxMi1.Text = calc.Mi(0).ToString();
            textBoxLambda1.Text = calc.Lambda_i(0).ToString();
            textBoxTi1.Text = calc.Ti(0).ToString();
            textBoxQi1.Text = calc.Qi(0).ToString();

            textBoxLi2.Text = calc.Li(1).ToString();
            textBoxRi2.Text = calc.Ri(1).ToString();
            textBoxMi2.Text = calc.Mi(1).ToString();
            textBoxLambda2.Text = calc.Lambda_i(1).ToString();
            textBoxTi2.Text = calc.Ti(1).ToString();
            textBoxQi2.Text = calc.Qi(1).ToString();
        }
    }
}
