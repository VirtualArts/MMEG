using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public partial class Form1 : Form
    {
        private CashFlowController cashController = new CashFlowController();
        private CashFlow cashFlow;

        public enum Tipo
        {
            Entrada = 0,
            Salida = 1
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            double value;
            if (double.TryParse(txtMonto.Text, out value))
                txtDescripcion.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", value);
            else
                txtDescripcion.Text = string.Empty;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidateControlDataToObject())
            {
                decimal monto = 0;
                CashFlow cashflow = new CashFlow();
                cashflow.Concepto = cbConcepto.SelectedItem.ToString();
                cashflow.Tipo = cbTipo.SelectedItem.ToString();
                cashflow.Fecha = dtFecha.Value;
                cashflow.Id = cashController.listCount() + 1;
                decimal.TryParse(txtMonto.Text, out monto);
                cashflow.Monto = monto;
                cashflow.Cuotas = Convert.ToInt32(nudCuotas.Value);
                cashflow.Descripcion = txtDescripcion.Text;
                cashController.addToList(cashflow);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = cashController.dataSource();
                clearControls();
            }
        }

        private void clearControls()
        {
            cbConcepto.Text = "";
            cbTipo.Text = "";
            txtDescripcion.Text = string.Empty;
            txtMonto.Text = "0";
            nudCuotas.Value = 1;
            dtFecha.Value = DateTime.Now;
        }
        private bool ValidateControlDataToObject()
        {
            bool result = true;
            decimal monto = 0;
            decimal.TryParse(txtMonto.Text, out monto);

            cbTipo.BackColor = Color.White;
            cbConcepto.BackColor = Color.White;
            dtFecha.CalendarForeColor = Color.Black;
            txtMonto.BackColor = Color.White;

            if (cbTipo.SelectedIndex < 0)
            {
                cbTipo.BackColor = Color.Orange;
                result = false;
            }
            else if (cbConcepto.SelectedIndex < 0)
            {
                cbConcepto.BackColor = Color.Orange;
                result = false;
            }
            else if (dtFecha.Value.Year < 1990 || dtFecha.Value.Year > DateTime.Now.AddYears(10).Year)
            {
                dtFecha.CalendarForeColor = Color.Red;
                result = false;
            }
            else if (monto < 0 || string.IsNullOrEmpty(txtMonto.Text))
            {
                txtMonto.BackColor = Color.Orange;
                result = false;
            }

            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dtFecha.Value = DateTime.Now;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            clearControls();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                object row = dataGridView1.SelectedRows;

            }
        }
    }
}
