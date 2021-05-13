using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 

        //Datos del Producto
        public static String grupo;
        public static String marca;
        public static String codigo = "";
        public static String descripcion;
        public static String aplicacion;
        public static String referencia;
        public static String medida;
        public static Double precio1;
        public static Double precio2;
        public static Double costo1;
        public static Double costo2;
        public static int tienda1;
        public static int bodega1;

        public static int cantidad;
        public static int cantidad2;
        public static int id;

        //datos usuario
        public static String usuario = "";

        //cadena de conexion
        public static class GGG
        {
            public static String conectar = "";
            public static String empresa = "";
            public static String direccion = "";
            public static String RTN = "";
            public static String telefono_empresa = "";
            public static String correo = "honduauto@yahoo.com";

        }

    //ISV
    public static double ISV = 0.15;
        

        //datos de facturacion
        public static String rango;
        public static String cai;
        public static DateTime fecha_limite;

        public static int dia = 28;
        public static int mes = 5;
        public static int anio = 3000;

        [STAThread]
        static void Main()
        {
            int n = DateTime.Now.Day;
            int nn = DateTime.Now.Month;
            int nnn = DateTime.Now.Year;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new login());
        }
    }
}
