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
    public partial class frmProveedores : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Inventario");


        public frmProveedores()
        {
            InitializeComponent();
            textBox1.Focus();
            actualizar();
        }



        private void inventarioBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            
        }

        public void actualizar()
        {
            try
            {
                textBox1.Focus();
                conexion.Open();
                dataGridView1.DataSource = "";
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT * FROM proveedores ORDER BY proveedor", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                         
            }
            catch(Exception ex)
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
                dv.RowFilter = string.Format("proveedor like '%{0}%' OR direccion '%{0}%' OR pais like'%{0}%' OR telefono like'%{0}%' OR fax like'%{0}%' OR email like'%{0}%' OR telefono_contacto like'%{0}%' OR rtn like'%{0}%' OR nombre_contacto like'%{0}%'", textBox1.Text);
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



        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
      
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fmrAgregarProveedor r = new fmrAgregarProveedor();
            r.FormClosed += new FormClosedEventHandler(fmrUsuarios_FormClosed);
            r.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
     

            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    string nombre = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    string tipo = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    string iniciales = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    string clave = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    string identidad = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    string direccion = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    string telefono = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    string celular = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    string correo = dataGridView1.CurrentRow.Cells[9].Value.ToString();                    

                    fmrEditarProveedor r = new fmrEditarProveedor(id, nombre, tipo, iniciales, identidad, direccion, telefono, celular, correo, clave);
                    r.FormClosed += new FormClosedEventHandler(fmrUsuarios_FormClosed);
                    r.Show();

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
                    if (MessageBox.Show("¿Seguro que quiere eliminar este proveedor?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conexion.Open();
                        int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        string delete = "DELETE FROM proveedores WHERE codigo = " + id + "";
                        OleDbCommand comando2 = new OleDbCommand(delete, conexion);
                        comando2.ExecuteNonQuery();
                        MessageBox.Show("Proveedor Eliminado con Exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = "";
                        dt.Clear();

                        textBox1.Focus();
                        OleDbDataAdapter data = new OleDbDataAdapter("SELECT * FROM proveedores order by proveedor", conexion);
                        data.Fill(dt);
                        dataGridView1.DataSource = dt;

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
            dataGridView1.DataSource = "";
            dt.Clear();
            try
            {
                textBox1.Focus();
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT * FROM proveedores order by proveedor", conexion);
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
    }
}
