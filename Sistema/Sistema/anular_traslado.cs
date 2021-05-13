using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Sistema
{
    public partial class anular_traslado : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        AutoCompleteStringCollection facturas = new AutoCompleteStringCollection();
        DataTable dt = new DataTable("Ventas");
        DataTable dtin = new DataTable("Inventario");
        int cantidadFactura;
        int id;
        int cantidadinventario;

        public anular_traslado()
        {
            InitializeComponent();
            obtenerfacturas();
        }
        public void obtenerfacturas()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT DISTINCT numero_traslado FROM traslado where fecha_anulacion = '00' ", conexion);
                OleDbDataReader reader2 = comando.ExecuteReader();

                while (reader2.Read())
                {
                    this.comboBox1.Items.Add(reader2.GetString(0));
                    facturas.Add(reader2.GetString(0));               
                }
                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboBox1.AutoCompleteCustomSource = facturas;
                conexion.Close();

            }
            catch
            {
                conexion.Close();
            }
        }

        public void actualizar()
        {
            conexion.Open();
            dt.Clear();
            dtin.Clear();
            OleDbDataAdapter data = new OleDbDataAdapter("SELECT *FROM traslado WHERE fecha_anulacion = '00' and numero_traslado='" + comboBox1.Text+"'", conexion);
            data.Fill(dt);
            dataGridView1.DataSource = dt;
            conexion.Close();
        }

        public void buscar(string codigo, string marca, string grupo)
        {
            conexion.Open();
            OleDbDataAdapter data = new OleDbDataAdapter("SELECT id, bodega1 FROM inventario WHERE codigo='" + codigo+ "' AND marca= '"+marca+"' AND grupo ='"+grupo+"'", conexion);
            data.Fill(dtin);
            dataGridView2.DataSource = dtin;
            conexion.Close();
            dataGridView1.Columns["fecha_anulacion"].Visible = false;
            dataGridView1.Columns["descripcion"].Width = 300;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizar();
            foreach (DataGridViewRow factura in dataGridView1.Rows)
            {
                string cod, mar, gru;
                cod = Convert.ToString(factura.Cells[5].Value);
                mar = Convert.ToString(factura.Cells[4].Value);
                gru = Convert.ToString(factura.Cells[3].Value);
                buscar(cod, mar, gru);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Elija un numero de traslado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                cantidadinventario = 0;
                cantidadFactura = 0;
                try
                {
                    conexion.Open();
                    foreach (DataGridViewRow factura in dataGridView1.Rows)
                    {
                        string fecha = DateTime.Now.ToString();
                        string update = "UPDATE traslado set fecha_anulacion = '" + fecha + "' WHERE numero_traslado= '" + comboBox1.Text + "'";
                        OleDbCommand comando55 = new OleDbCommand(update, conexion);
                        comando55.ExecuteNonQuery();
                    }

                    int contador = 0;
                    foreach (DataGridViewRow inven in dataGridView2.Rows)
                    {
                        cantidadFactura = Convert.ToInt32(dataGridView1.Rows[contador].Cells[7].Value);
                        id = Convert.ToInt32(inven.Cells[0].Value);
                        cantidadinventario = Convert.ToInt32(inven.Cells[1].Value);
                        int suma = cantidadFactura + cantidadinventario;
                        string update = "UPDATE inventario set bodega1 = '" + suma + "' WHERE id= " + id + "";
                        OleDbCommand comando55 = new OleDbCommand(update, conexion);
                        comando55.ExecuteNonQuery();
                        contador++;
                    }
                    conexion.Close();
                    MessageBox.Show("Ha anulado el traslado con exito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
