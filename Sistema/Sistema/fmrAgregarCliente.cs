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
    public partial class fmrAgregarCliente : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
       public fmrAgregarCliente()
        {
            InitializeComponent();
            if (checkBox1.Checked)
            {
                txtLimite.Enabled = true;
            }
            else
            {
                txtLimite.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int codigo = 1;
            decimal limite;
            try
            {
                if (txtCliente.Text != "")
                {

                    conexion.Open();
                    OleDbCommand comando22 = new OleDbCommand("SELECT MAX(codigo) FROM clientes", conexion);
                    if (comando22.ExecuteScalar() != DBNull.Value)
                    {
                        codigo = Convert.ToInt32(comando22.ExecuteScalar());
                        codigo = codigo + 1;
                    }


                    string insert = "INSERT INTO clientes VALUES (@codigo, @cliente, @direccion, @pais, @telefono, @fax, @email, @telefono_contacto, @RTN, @credito, @limite, @exento)";
                    OleDbCommand comando21 = new OleDbCommand(insert, conexion);
                    comando21.Parameters.AddWithValue("@codigo", codigo);
                    comando21.Parameters.AddWithValue("@cliente", txtCliente.Text);
                    comando21.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                    comando21.Parameters.AddWithValue("@pais", txtPais.Text);
                    comando21.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                    comando21.Parameters.AddWithValue("@fax", txtFax.Text);
                    comando21.Parameters.AddWithValue("@email", txtEmail.Text);
                    comando21.Parameters.AddWithValue("@telefono_contacto", txtTelefono.Text);
                    comando21.Parameters.AddWithValue("@RTN", txtRTN.Text);
                    comando21.Parameters.AddWithValue("@credito", checkBox1.Checked);

                    if (String.IsNullOrEmpty(txtLimite.Text))
                    {
                        limite = 0.00m;
                    }else
                    {
                        limite = Convert.ToDecimal(txtLimite.Text);
                    }
                    comando21.Parameters.AddWithValue("@limite",limite);
                    comando21.Parameters.AddWithValue("@exento", checkBox2.Checked);

                    comando21.ExecuteNonQuery();
                    MessageBox.Show("Registro Ingresado correctamente", "Bien", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Debe selecionar un Nombre para el Cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtLimite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                toolTip1.IsBalloon = true;
                toolTip1.Show("Solo se permiten numeros", txtLimite, 3000);
                e.Handled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtLimite.Enabled = true;
            }
            else
            {
                txtLimite.Enabled = false;
            }
        }
    }
}
