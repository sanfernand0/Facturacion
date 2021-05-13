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
    public partial class consultas : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Inventario");
        DataTable ventas = new DataTable("Ventas");


        public consultas()
        {
            InitializeComponent();
            textBox1.Focus();
            actualizar();

        }

        public void actualizar()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT id as Id, grupo as Grupo, marca as Marca, codigo as Codigo, descripcion as Descripcion, aplicacion as Aplicacion, referencia as Referencia, medida as Medida, bodega1, tienda1, costo1 as Costo, precio1 as PrecioVenta, precio2 as PrecioMinimo FROM inventario order by id asc", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Costo"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["PrecioMinimo"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["PrecioVenta"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["descripcion"].FillWeight = 300;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
        }


        public void actualizarVentas()
        {
            try
            {
                ventas.Clear();
                dataGridView2.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT id, fecha, numero_factura, grupo, marca, codigo, descripcion, cantidad, precio, cliente, rtn, vendedor, tipo FROM ventas ORDER BY id ASC", conexion);
                data.Fill(ventas);
                dataGridView2.DataSource = ventas;
                dataGridView2.Columns[6].FillWeight = 250;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }

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
            this.Dispose();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarVentas();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
