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
    public partial class reporte_saldobodega : Form
    {
        public reporte_saldobodega()
        {
            InitializeComponent();

        }

      
        private void reporte_saldobodega_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'BaseDataSet1.pedido' Puede moverla o quitarla según sea necesario.

            this.reportViewer1.RefreshReport();
        }

    
    }
}
