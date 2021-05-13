using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema
{
    public partial class menu3 : Form
    {
        public menu3()
        {
            InitializeComponent();
        }

        public menu3(string iniciales)
        {
            InitializeComponent();
            label1.Text = "Bienvenido: " + iniciales;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            factura2 u = new factura2();
            u.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            prestamo u = new prestamo();
            u.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            traslado1 u = new traslado1();
            u.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cotizacion u = new cotizacion();
            u.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            menu3.ActiveForm.Hide();
            login n = new login();
            n.ShowDialog();
           
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            consultas r = new consultas();
            r.Show();

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void menu3_Activated(object sender, EventArgs e)
        {
 
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            actualizador r = new actualizador();
            r.Show();

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
    
    
}
