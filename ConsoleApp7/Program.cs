using System;
using System.Collections.Generic;
using System.Linq;

namespace RegistroVentasBoletos
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistroVentasServicio servicio = new RegistroVentasServicio();

            // Solicitar la cantidad de ventas a registrar
            Console.WriteLine("Ingrese el número de ventas a registrar:");
            int numVentas = Convert.ToInt32(Console.ReadLine());

            // Registrar las ventas
            for (int i = 0; i < numVentas; i++)
            {
                Console.WriteLine($"\nRegistro de Venta {i + 1}:");
                servicio.RegistrarVenta();
            }

            // Mostrar la lista de ventas
            servicio.MostrarListaVentas();

            // Mostrar el acumulado del importe pagado por clientes
            servicio.MostrarAcumuladoImportePago();
        }
    }

    // Clase para representar una venta
    class Venta
    {
        public string Ruta { get; set; }
        public string TipoCliente { get; set; }
        public int CantidadPersonas { get; set; }
        public double MontoTotal { get; set; }
    }

    // Clase que contiene la lógica del negocio para el registro de ventas
    class RegistroVentasServicio
    {
        private Dictionary<string, double> _rutas = new Dictionary<string, double>()
        {
            {"Sacsayhuaman – Puka Pukara – Tambomachay", 100},
            {"Tipon -Lucre-Piquillaqta", 120},
            {"Ollantaytambo-Machupicchu", 150}
        };

        private Dictionary<int, double> _descuentosPorCantidad = new Dictionary<int, double>()
        {
            {1, 0},
            {2, 0.08},
            {8, 0.13},
            {17, 0.15}
        };

        private Dictionary<string, double> _descuentosPorTipoCliente = new Dictionary<string, double>()
        {
            {"Promoción de colegios", 0.07},
            {"Adultos mayores de 60 años", 0.05}
        };

        private List<Venta> _ventas = new List<Venta>();

        public void RegistrarVenta()
        {
            Console.WriteLine("Seleccione la ruta (ingrese el número correspondiente):");
            MostrarRutas();

            int opcionRuta = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Ingrese el tipo de cliente (Promoción de colegios, Adultos mayores de 60 años, Adultos menores de 60 y jóvenes):");
            string tipoCliente = Console.ReadLine();

            Console.WriteLine("Ingrese la cantidad de personas:");
            int cantidadPersonas = Convert.ToInt32(Console.ReadLine());

            double descuentoCantidad = CalcularDescuentoPorCantidad(cantidadPersonas);
            double descuentoTipoCliente = CalcularDescuentoPorTipoCliente(tipoCliente);
            double precioRuta = _rutas.Values.ToArray()[opcionRuta - 1];
            double montoTotal = CalcularMontoTotal(precioRuta, cantidadPersonas, descuentoCantidad, descuentoTipoCliente);

            _ventas.Add(new Venta { Ruta = _rutas.Keys.ToArray()[opcionRuta - 1], TipoCliente = tipoCliente, CantidadPersonas = cantidadPersonas, MontoTotal = montoTotal });
        }

        private void MostrarRutas()
        {
            int j = 1;
            foreach (var ruta in _rutas)
            {
                Console.WriteLine($"{j}. {ruta.Key} - S/. {ruta.Value}");
                j++;
            }
        }

        private double CalcularDescuentoPorCantidad(int cantidadPersonas)
        {
            double descuentoCantidad = 0;
            foreach (var descuento in _descuentosPorCantidad)
            {
                if (cantidadPersonas >= descuento.Key)
                {
                    descuentoCantidad = descuento.Value;
                }
            }
            return descuentoCantidad;
        }

        private double CalcularDescuentoPorTipoCliente(string tipoCliente)
        {
            double descuentoTipoCliente = 0;
            if (_descuentosPorTipoCliente.ContainsKey(tipoCliente))
            {
                descuentoTipoCliente = _descuentosPorTipoCliente[tipoCliente];
            }
            return descuentoTipoCliente;
        }

        private double CalcularMontoTotal(double precioRuta, int cantidadPersonas, double descuentoCantidad, double descuentoTipoCliente)
        {
            return precioRuta * cantidadPersonas * (1 - descuentoCantidad) * (1 - descuentoTipoCliente);
        }

        public void MostrarListaVentas()
        {
            Console.WriteLine("\nLista de Ventas:");
            foreach (var venta in _ventas)
            {
                Console.WriteLine($"Ruta: {venta.Ruta}, Tipo de Cliente: {venta.TipoCliente}, Cantidad de Personas: {venta.CantidadPersonas}, Monto Total: S/. {venta.MontoTotal}");
            }
        }

        public void MostrarAcumuladoImportePago()
        {
            double acumuladoImportePago = _ventas.Sum(v => v.MontoTotal);
            Console.WriteLine($"\nAcumulado del Importe Pagado por Clientes: S/. {acumuladoImportePago}");
        }
    }
}