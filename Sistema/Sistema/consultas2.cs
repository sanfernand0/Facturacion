using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace Sistema
{
    public partial class consultas2 : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.conexion);
        DataTable dt = new DataTable("Inventario");
        DataTable ventas = new DataTable("Ventas");


        public consultas2()
        {
            InitializeComponent();
            textBox1.Focus();
            actualizarVentas();
            actualizar();
        }



        private void inventarioBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Focus();
                this.Validate();
                this.inventarioBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.baseDataSet);
            }
            catch
            {

            }
        }

        public void actualizar()
        {
            try
            {
                textBox1.Focus();
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT *FROM inventario ORDER BY id ASC", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[4].Width = 300;

            }
            catch
            {

            }
        }


        public void actualizarVentas()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT id, fecha, numero_factura, grupo, marca, codigo, descripcion, cantidad, precio, cliente, rtn, vendedor, tipo FROM ventas ORDER BY id ASC", conexion);
                data.Fill(ventas);
                dataGridView2.DataSource = ventas;
                conexion.Close();
                dataGridView2.Columns[6].Width = 250;

            }
            catch
            {

            }

        }

        private void inventario_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDataSet.ventas' Puede moverla o quitarla según sea necesario.
            //this.ventasTableAdapter.Fill(this.baseDataSet.ventas);
            textBox1.Focus();
        }

     

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("codigo like '%{0}%' OR grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR aplicacion like'%{0}%' OR referencia like'%{0}%' OR medida like'%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv.ToTable();
                textBox1.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = ventas.DefaultView;
                dv.RowFilter = string.Format("codigo like '%{0}%' OR grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR rtn like'%{0}%' OR cliente like'%{0}%' OR vendedor like'%{0}%'", textBox2.Text);
                dataGridView2.DataSource = dv.ToTable();
                textBox2.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
      
        }
    }
}
