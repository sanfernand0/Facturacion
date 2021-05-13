using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Globalization;


namespace Sistema
{
    public partial class fmrAgregarProveedor : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
       public fmrAgregarProveedor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int codigo = 1;
            try
            {
                if (txtNombre.Text != "")
                {
                    conexion.Open();
                    OleDbCommand comando2 = new OleDbCommand("SELECT MAX(codigo) FROM proveedores", conexion);
                    if (comando2.ExecuteScalar() != DBNull.Value)
                    {
                        codigo = Convert.ToInt32(comando2.ExecuteScalar());
                        codigo = codigo + 1;
                    }
             

                    string insert = "INSERT INTO proveedores VALUES (@codigo, @proveedor, @direccion, @pais, @telefono, @fax, @email, @telefono_contacto, @RTN, @nombre_contacto)";
                    OleDbCommand comando = new OleDbCommand(insert, conexion);
                    comando.Parameters.AddWithValue("@codigo", codigo);
                    comando.Parameters.AddWithValue("@proveedor", txtNombre.Text);
                    comando.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                    comando.Parameters.AddWithValue("@pais", txtPais.Text);
                    comando.Parameters.AddWithValue("@telefono", txtTelefonoProveedor.Text);
                    comando.Parameters.AddWithValue("@fax", txtFax.Text);
                    comando.Parameters.AddWithValue("@email", txtEmail.Text);
                    comando.Parameters.AddWithValue("@telefono_contacto", txtTelefonoContacto.Text);
                    comando.Parameters.AddWithValue("@RTN", txtRTN.Text);
                    comando.Parameters.AddWithValue("@nombre_contacto", txtNombreContacto.Text);
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Registro Ingresado correctamente", "Bien", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Debe selecionar un nombre para el proveedor", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
