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
using System.Globalization;

namespace Sistema
{
    public partial class actualizar_tasa : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        double tasa, precio1, precio2;
        public actualizar_tasa()
        {
            InitializeComponent();

            textBox1.Focus();
            conexion.Open();
            OleDbCommand comando = new OleDbCommand("SELECT  *FROM tasa", conexion);
            OleDbDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                tasa = reader.GetDouble(1);
                precio1 = reader.GetDouble(2);
                precio2 = reader.GetDouble(3);
            }

            conexion.Close();

            textBox1.Text = tasa.ToString("N2");
            textBox2.Text = precio1.ToString("N2");
            textBox3.Text = precio2.ToString("N2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox2.Text))
                {

                    MessageBox.Show("NO SE PERMITEN CAMPOS VACIOS", "ERRROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    conexion.Open();
                    string update = "UPDATE tasa set tasa = '" + textBox1.Text + "' ,precio1 = '" + textBox2.Text + "' ,precio2 = '" + textBox3.Text + "' WHERE id=" + 1 + "";
                    OleDbCommand comando2 = new OleDbCommand(update, conexion);
                    comando2.ExecuteNonQuery();
                    MessageBox.Show("Tasa Actualizada con Exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conexion.Close();
                    this.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Actualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conexion.Close();
            }


        }

    private void actualizar_tasa_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox2.Focus();
            }

            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox3.Focus();
            }
            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.')
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                button1.Focus();
            }
            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.')
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
