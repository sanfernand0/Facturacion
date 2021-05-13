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
    public partial class mantenimientoInventario : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Inventario");


        public mantenimientoInventario()
        {
            InitializeComponent();
            textBox1.Focus();
            actualizar();
            estilos();
        }


        public void estilos()
        {
            dataGridView1.Columns["descripcion"].FillWeight = 300;
            dataGridView1.Columns["PrecioVenta"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["Costo"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["PrecioMinimo"].DefaultCellStyle.Format = "N2";
        }

        public void actualizar()
        {
            try
            {
                textBox1.Focus();
                conexion.Open();
                dataGridView1.DataSource = "";
                dt.Clear();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT id as Id, grupo as Grupo, marca as Marca, codigo as Codigo, descripcion as Descripcion, aplicacion as Aplicacion, referencia as Referencia, medida as Medida, bodega1, tienda1, costo1 as Costo, precio1 as PrecioVenta, precio2 as PrecioMinimo, exento as Exento FROM inventario order by id desc", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            this.Close();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AgregarInventario r = new AgregarInventario();
            r.FormClosed += new FormClosedEventHandler(mantenimientoInventario_FormClosed);
            r.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                    string grupo = Convert.ToString(dataGridView1.CurrentRow.Cells["grupo"].Value);
                    string marca = dataGridView1.CurrentRow.Cells["marca"].Value.ToString();
                    string codigo = dataGridView1.CurrentRow.Cells["codigo"].Value.ToString();
                    string descripcion = dataGridView1.CurrentRow.Cells["descripcion"].Value.ToString();
                    string aplicacion = dataGridView1.CurrentRow.Cells["aplicacion"].Value.ToString();
                    string referencia = dataGridView1.CurrentRow.Cells["referencia"].Value.ToString();
                    string medida = dataGridView1.CurrentRow.Cells["medida"].Value.ToString();
                    int bodega1 = Convert.ToInt32(dataGridView1.CurrentRow.Cells["bodega1"].Value.ToString());
                    int tienda1 = Convert.ToInt32(dataGridView1.CurrentRow.Cells["tienda1"].Value.ToString());
                    decimal costo1 = decimal.Parse(dataGridView1.CurrentRow.Cells["costo"].Value.ToString());
                    decimal precio1 = decimal.Parse(dataGridView1.CurrentRow.Cells["precioventa"].Value.ToString());
                    decimal precio2 = decimal.Parse(dataGridView1.CurrentRow.Cells["preciominimo"].Value.ToString());
                    bool exento = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["exento"].Value);

                    editarInventario edit = new editarInventario(id, grupo, marca, codigo, descripcion, aplicacion, referencia, medida,
                        bodega1, tienda1, costo1,  precio1, precio2, exento);
                    edit.FormClosed += new FormClosedEventHandler(mantenimientoInventario_FormClosed);
                    edit.Show();
                }
                else
                {
                    MessageBox.Show("Seleccione una fila antes de continuar", "Ninguna fila seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (MessageBox.Show("¿Seguro que quiere eliminar este articulo?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conexion.Open();
                        int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        string delete = "DELETE FROM inventario WHERE id = " + id + "";
                        OleDbCommand comando2 = new OleDbCommand(delete, conexion);
                        comando2.ExecuteNonQuery();
                        MessageBox.Show("Articulo Eliminado con Exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conexion.Close();
                        actualizar();
                        estilos();
                    }
                    else
                    {
                        MessageBox.Show("Operación Cancelada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void mantenimientoInventario_FormClosed(object sender, FormClosedEventArgs e)
        {
            actualizar();
            estilos();
        }
    }
}
