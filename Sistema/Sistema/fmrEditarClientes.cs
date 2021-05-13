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
    public partial class fmrEditarClientes : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);

        public fmrEditarClientes()
        {
            InitializeComponent();
            txtCliente.Focus();
        }

        public fmrEditarClientes(int id, string cliente, string direccion, string pais, 
            string telefono, string fax, string email, string contacto, string rtn, bool credito, double limite, bool exento)
        {
            InitializeComponent();
            txtId.Text = id.ToString();
            txtCliente.Text = cliente;
            txtDireccion.Text = direccion;
            txtPais.Text = pais;
            txtLimite.Text = limite.ToString("N2");
            txtEmail.Text = email;
            txtFax.Text = fax;
            txtContacto.Text = contacto;
            txtRTN.Text = rtn;
            checkBox1.Checked = credito;
            txtTelefono.Text = telefono;
            checkBox2.Checked = exento;

            if (checkBox1.Checked)
            {
                txtLimite.Enabled = true;
            }else
            {
                txtLimite.Enabled = false;
            }
     
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                string update = "UPDATE clientes set Cliente = ?, direccion = ?, pais = ?, telefono = ?, fax = ?, email = ?, telefono_contacto = ?, RTN = ? , Credito = ? , Limite = ?, exento = ? WHERE codigo = ?";
                OleDbCommand comando7 = new OleDbCommand(update, conexion);
                comando7.Parameters.AddWithValue("@Cliente", txtCliente.Text);
                comando7.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                comando7.Parameters.AddWithValue("@pais", txtPais.Text);
                comando7.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                comando7.Parameters.AddWithValue("@fax", txtFax.Text);
                comando7.Parameters.AddWithValue("@email", txtEmail.Text);
                comando7.Parameters.AddWithValue("@telefono_contacto", txtContacto.Text);
                comando7.Parameters.AddWithValue("@RTN", txtRTN.Text);
                comando7.Parameters.AddWithValue("@Credito", checkBox1.Checked);
                comando7.Parameters.AddWithValue("@Limite", txtLimite.Text);
                comando7.Parameters.AddWithValue("@exento", checkBox2.Checked);
                comando7.Parameters.AddWithValue("@codigo", txtId.Text);

                comando7.ExecuteNonQuery();
                if (comando7.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("Cliente actualizado con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information); this.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtInciales_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(txtPais, "Las Iniciales serán las que aparecerán en la facturacion");
            toolTip1.IsBalloon = true;

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
    }
}
