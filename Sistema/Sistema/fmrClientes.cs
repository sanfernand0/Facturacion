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
    public partial class fmrClientes : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Inventario");


        public fmrClientes()
        {
            InitializeComponent();
            textBox1.Focus();
            actualizar();
            config();
        }

        public void config()
        {
            dataGridView1.Columns[10].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns[1].FillWeight = 250;
        }

        public void actualizar()
        {
            try
            {
                textBox1.Focus();
                conexion.Open();
                dataGridView1.DataSource = "";
                dt.Clear();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT codigo, Cliente, direccion, pais, telefono, fax, email, telefono_contacto, RTN, Credito, Limite, exento FROM clientes order by codigo", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
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

        private void inventario_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("Cliente like '%{0}%' OR direccion like'%{0}%' OR pais like'%{0}%' OR telefono like'%{0}%' OR fax like'%{0}%' OR email like'%{0}%' OR telefono_contacto like'%{0}%' OR RTN like'%{0}%'", textBox1.Text);
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
            fmrAgregarCliente r = new fmrAgregarCliente();
            r.FormClosed += new FormClosedEventHandler(fmrUsuarios_FormClosed);
            r.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double n = 0.00;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    string cliente = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    string direccion = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    string pais = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    string telefono = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    string fax = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    string email = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    string contacto = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    string rtn = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    bool credito = Convert.ToBoolean(dataGridView1.CurrentRow.Cells[9].Value);
                    double limite =Convert.ToDouble(dataGridView1.CurrentRow.Cells[10].Value);
                    bool exento = Convert.ToBoolean(dataGridView1.CurrentRow.Cells[11].Value);

                    fmrEditarClientes r = new fmrEditarClientes(id, cliente, direccion, pais, telefono, fax, email, contacto, rtn, credito, limite, exento);
                    r.FormClosed += new FormClosedEventHandler(fmrUsuarios_FormClosed);
                    r.Show();

                }
                else
                {
                    MessageBox.Show("Seleccione una fila antes de continuar", "Ninguna fila seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
            catch (Exception ex)
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
                    if (MessageBox.Show("¿Seguro que quiere eliminar este cliente?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conexion.Open();
                        int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        string delete = "DELETE FROM clientes WHERE codigo = " + id + "";
                        OleDbCommand comando2 = new OleDbCommand(delete, conexion);
                        comando2.ExecuteNonQuery();
                        MessageBox.Show("Cliente Eliminado con Exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conexion.Close();
                        actualizar();
                        config();

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
            finally
            {
                conexion.Close();

            }
        }
   
        private void fmrUsuarios_FormClosed(object sender, FormClosedEventArgs e)
        {
            actualizar();
            config();
        }

    }
}
