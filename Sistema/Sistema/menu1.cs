using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sistema
{
    public partial class menu1 : Form
    {
        public menu1()
        {
            InitializeComponent();
        }

        public menu1(string iniciales)
        {
            InitializeComponent();
            label1.Text = "Bienvenido: " + iniciales;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmrUsuarios u = new fmrUsuarios();
            this.Hide();
            u.Show();
            this.Show();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProveedores u = new frmProveedores();
            this.Hide();
            u.Show();
            this.Show();
        }

        private void actualizarTasaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actualizar_tasa u = new actualizar_tasa();
            this.Hide();
            u.Show();
            this.Show();
        }

        private void tiendaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void comprasLocalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crear_pedido_local u = new crear_pedido_local();
            u.Show();
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            crear_pedido_local u = new crear_pedido_local();
            u.Show();
            this.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            fmrUsuarios u = new fmrUsuarios();
            u.Show();
            this.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            frmProveedores u = new frmProveedores();
            u.Show();
            this.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            actualizar_tasa u = new actualizar_tasa();
            u.Show();
            this.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            menu1.ActiveForm.Hide();
            login n = new login();
            n.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            consultas r = new consultas();
            r.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            fmrClientes r = new fmrClientes();
            r.Show();
        }


        private void button10_Click(object sender, EventArgs e)
        {
            Sistema.reporte_compras r = new reporte_compras();
           // Reportes.compras_menu r = new Reportes.compras_menu();
            r.Show();
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            rptventas r = new rptventas();
            r.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            excel r = new excel();
            r.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Datos r = new Datos();
            r.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            arqueov2 r = new arqueov2();
            r.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void anularFacturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            anular_factura r = new anular_factura();
            r.Show();
        }

        private void anularPrestamoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            anular_prestamo r = new anular_prestamo();
            r.Show();
        }

        private void anularTrasladoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            anular_traslado r = new anular_traslado();
            r.Show();
        }

        private void menu1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Down)
            {
                MessageBox.Show("");
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            mantenimientoInventario r = new mantenimientoInventario();
            r.Show();
        }


        private void button17_Click(object sender, EventArgs e)
        {
            verTodo u = new verTodo();
            u.Show();
        }


      

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            rptComprasConsolidado r = new rptComprasConsolidado();
            r.Show();
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            rptventasVendedor r = new rptventasVendedor();
            r.Show();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            crear_pedido_inter j = new crear_pedido_inter();
            j.Show();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            productoMasVendido r = new productoMasVendido();
            r.Show();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            fmrNotaCredito t = new fmrNotaCredito();
            t.Show();
        }

        private void Button22_Click(object sender, EventArgs e)
        {
            fmrCreditos r = new fmrCreditos();
            r.Show();
        }

        private void Button23_Click(object sender, EventArgs e)
        {
            fmrNotaDebito r = new fmrNotaDebito();
            r.Show();
        }
    }
}
