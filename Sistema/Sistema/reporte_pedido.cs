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
    public partial class reporte_pedido : Form
    {
        public reporte_pedido()
        {
            InitializeComponent();
        }

        private void reporte_pedido_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'BaseDataSet.pedido' Puede moverla o quitarla según sea necesario.
            this.pedidoTableAdapter.Fill(this.BaseDataSet.pedido);

            this.reportViewer1.RefreshReport();
        }
    }
}
