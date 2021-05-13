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
    public partial class fmrEditarProveedor : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);

        public fmrEditarProveedor()
        {
            InitializeComponent();
            txtNombre.Focus();
        }

        public fmrEditarProveedor(int id, string proveedor, string direccion, string pais, 
            string telefono, string fax, string email, string telefono_contacto, string rtn, string nombreContacto)
        {
            InitializeComponent();
            txtId.Text = id.ToString();
            txtNombre.Text = proveedor;
            txtDireccion.Text = direccion;
            txtPais.Text = pais;
            txtTelefonoProveedor.Text = telefono;
            txtFax.Text = fax;
            txtEmail.Text = email;
            txtTelefonoContacto.Text = telefono_contacto;
            txtRTN.Text = rtn;
            txtNombreContacto.Text = nombreContacto.ToString();
     
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                string update = "UPDATE proveedores set proveedor = ?, direccion = ?, pais = ?, telefono = ?, fax = ?, email = ?, telefono_contacto = ?, RTN = ?, nombre_contacto = ? WHERE codigo= ?";
                OleDbCommand comando7 = new OleDbCommand(update, conexion);
                comando7.Parameters.AddWithValue("@proveedor", txtNombre.Text);
                comando7.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                comando7.Parameters.AddWithValue("@pais", txtPais.Text);
                comando7.Parameters.AddWithValue("@telefono", txtTelefonoProveedor.Text);
                comando7.Parameters.AddWithValue("@fax", txtFax.Text);
                comando7.Parameters.AddWithValue("@email", txtEmail.Text);
                comando7.Parameters.AddWithValue("@telefono_contacto", txtTelefonoContacto.Text);
                comando7.Parameters.AddWithValue("@RTN", txtRTN.Text);
                comando7.Parameters.AddWithValue("@nombre_contacto", txtNombreContacto.Text);
                comando7.Parameters.AddWithValue("@codigo", txtId.Text);
                comando7.ExecuteNonQuery();
                if (comando7.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("Proveedor actualizado con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
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

        private void editarInventario_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

    }
}
