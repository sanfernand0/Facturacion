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
    public partial class reporte_traslado : Form
    {
        public reporte_traslado()
        {
            InitializeComponent();
        }

        private void reporte_traslado_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'BaseDataSet.traslado' Puede moverla o quitarla según sea necesario.
            this.trasladoTableAdapter.Fill(this.BaseDataSet.traslado);

            this.reportViewer1.RefreshReport();
        }
    }
}
