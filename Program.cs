using System;
using System.Collections.Generic;

class Program
{
    static List<string>  nombres  = new List<string>();
    static List<decimal> precios  = new List<decimal>();
    static List<int>     stocks   = new List<int>();
    static List<int>     vendidos = new List<int>();

    static decimal cajaDiaria  = 0m;
    static int     totalVentas = 0;

    const decimal IVA_PORCENTAJE       = 0.19m;
    const decimal DESCUENTO_PORCENTAJE = 0.10m;
    const int     UMBRAL_BAJO_STOCK    = 5;

    static void Main(string[] args)
    {
        int opcion;
        do
        {
            ImprimirEncabezado("SISTEMA GESTOR DE VENTAS E INVENTARIO (MINI-POS)");
            Console.WriteLine(" 1. Registrar nuevo producto en inventario");
            Console.WriteLine(" 2. Consultar inventario completo");
            Console.WriteLine(" 3. Registrar una venta");
            Console.WriteLine(" 4. Ver reporte de caja y estadisticas diarias");
            Console.WriteLine(" 5. Salir");
            Console.WriteLine("====================================================");

            opcion = LeerEntero("Seleccione una opcion (1-5): ", 1, 5);

            switch (opcion)
            {
                case 1: RegistrarProducto();   break;
                case 2: ConsultarInventario(); break;
                case 3: RegistrarVenta();      break;
                case 4: ReporteDeCaja();       break;
                case 5: MensajeSalida();       break;
            }

        } while (opcion != 5);
    }

    static int LeerEntero(string mensaje, int min, int max)
    {
        int resultado;
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(entrada, out resultado))
            {
                Console.WriteLine("[ERROR] Entrada no valida. Debe ingresar un numero entero.");
                continue;
            }

            if (resultado < min || resultado > max)
            {
                Console.WriteLine($"[ERROR] Opcion fuera de rango. Ingrese un valor entre {min} y {max}.");
                continue;
            }

            return resultado;
        }
    }

    static decimal LeerDecimal(string mensaje, decimal min)
    {
        decimal resultado;
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine() ?? string.Empty;

            bool ok = decimal.TryParse(entrada,
                          System.Globalization.NumberStyles.Any,
                          System.Globalization.CultureInfo.CurrentCulture,
                          out resultado);

            if (!ok)
            {
                ok = decimal.TryParse(entrada,
                         System.Globalization.NumberStyles.Any,
                         System.Globalization.CultureInfo.InvariantCulture,
                         out resultado);
            }

            if (!ok)
            {
                Console.WriteLine("[ERROR] Valor no valido. Ingrese un numero decimal (ej: 12500 o 12500.50).");
                continue;
            }

            if (resultado < min)
            {
                Console.WriteLine($"[ERROR] El valor debe ser mayor o igual a {min:C}.");
                continue;
            }

            return resultado;
        }
    }

    static decimal CalcularFactura(decimal precio, int cantidad, bool tieneDescuento,
                                   out decimal montoIva, out decimal montoDescuento)
    {
        decimal subtotal      = precio * cantidad;
        montoDescuento        = tieneDescuento ? subtotal * DESCUENTO_PORCENTAJE : 0m;
        decimal baseImponible = subtotal - montoDescuento;
        montoIva              = baseImponible * IVA_PORCENTAJE;
        return baseImponible + montoIva;
    }

    static void ImprimirEncabezado(string titulo)
    {
        Console.WriteLine();
        Console.WriteLine("====================================================");
        int anchoLinea = 50;
        int espacios   = (anchoLinea - titulo.Length) / 2;
        if (espacios < 0) espacios = 0;
        string centrado = titulo.PadLeft(titulo.Length + espacios).PadRight(anchoLinea);
        Console.WriteLine($"  {centrado}");
        Console.WriteLine("====================================================");
    }

    static void RegistrarProducto()
    {
        ImprimirEncabezado("REGISTRAR NUEVO PRODUCTO");

        string nombre;
        while (true)
        {
            Console.Write("Nombre del producto: ");
            nombre = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("[ERROR] El nombre no puede estar vacio.");
                continue;
            }

            bool duplicado = false;
            foreach (string n in nombres)
            {
                if (n.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    duplicado = true;
                    break;
                }
            }

            if (duplicado)
            {
                Console.WriteLine($"[ERROR] Ya existe un producto con el nombre \"{nombre}\". Use un nombre diferente.");
                continue;
            }

            break;
        }

        decimal precio = LeerDecimal("Precio unitario ($): ", 0.01m);
        int stock      = LeerEntero("Stock inicial (unidades): ", 0, int.MaxValue);

        nombres.Add(nombre);
        precios.Add(precio);
        stocks.Add(stock);
        vendidos.Add(0);

        Console.WriteLine($"\n[OK] Producto \"{nombre}\" registrado exitosamente.");
        Console.WriteLine($"     Precio: {precio:C} | Stock inicial: {stock} unidades");
        Pausa();
    }

    static void ConsultarInventario()
    {
        ImprimirEncabezado("INVENTARIO COMPLETO");

        if (nombres.Count == 0)
        {
            Console.WriteLine(" No hay productos registrados en el inventario.");
            Pausa();
            return;
        }

        Console.WriteLine($" {"ID",-4} {"Producto",-25} {"Precio",-15} {"Stock",-8} Estado");
        Console.WriteLine(new string('-', 72));

        for (int i = 0; i < nombres.Count; i++)
        {
            string alerta = stocks[i] < UMBRAL_BAJO_STOCK ? "[ALERTA: BAJO STOCK]" : "";
            Console.WriteLine($" {i + 1,-4} {nombres[i],-25} {precios[i],-15:C} {stocks[i],-8} {alerta}");
        }

        Console.WriteLine(new string('-', 72));
        Console.WriteLine($" Total de productos registrados: {nombres.Count}");
        Pausa();
    }

    static void RegistrarVenta()
    {
        ImprimirEncabezado("REGISTRAR VENTA");

        if (nombres.Count == 0)
        {
            Console.WriteLine(" No hay productos disponibles. Registre productos primero.");
            Pausa();
            return;
        }

        Console.WriteLine(" Productos disponibles:");
        Console.WriteLine(new string('-', 62));
        for (int i = 0; i < nombres.Count; i++)
        {
            string alerta = stocks[i] < UMBRAL_BAJO_STOCK ? " [ALERTA: BAJO STOCK]" : "";
            Console.WriteLine($" {i + 1}. {nombres[i],-28} | Precio: {precios[i],10:C} | Stock: {stocks[i]}{alerta}");
        }
        Console.WriteLine(new string('-', 62));

        int idProducto = LeerEntero($"Seleccione el numero del producto a vender (1-{nombres.Count}): ", 1, nombres.Count);
        int idx        = idProducto - 1;

        int cantidad;
        while (true)
        {
            cantidad = LeerEntero("Ingrese la cantidad a comprar: ", 1, int.MaxValue);

            if (cantidad > stocks[idx])
            {
                Console.WriteLine($"[ERROR] Stock insuficiente. Solo quedan {stocks[idx]} unidades en inventario.");
                continue;
            }

            break;
        }

        bool tieneDescuento = LeerSiNo("Aplica descuento de cliente frecuente (10%)? (S/N): ");

        decimal montoIva, montoDescuento;
        decimal totalPagar = CalcularFactura(precios[idx], cantidad, tieneDescuento, out montoIva, out montoDescuento);
        decimal subtotal   = precios[idx] * cantidad;

        stocks[idx]   -= cantidad;
        vendidos[idx] += cantidad;
        cajaDiaria    += totalPagar;
        totalVentas++;

        ImprimirEncabezado("TICKET DE VENTA");
        Console.WriteLine($" Producto:          {nombres[idx]} (x{cantidad})");
        Console.WriteLine($" Subtotal:          {subtotal,14:C}");

        if (tieneDescuento)
            Console.WriteLine($" Descuento (10%):  -{montoDescuento,13:C}");

        Console.WriteLine($" IVA (19%):        +{montoIva,13:C}");
        Console.WriteLine(new string('-', 51));
        Console.WriteLine($" TOTAL A PAGAR:     {totalPagar,14:C}");
        Console.WriteLine("====================================================");
        Console.WriteLine($"[OK] Venta efectuada con exito. Stock actualizado: {stocks[idx]} unidades.");
        Pausa();
    }

    static void ReporteDeCaja()
    {
        ImprimirEncabezado("REPORTE DE CAJA Y ESTADISTICAS DIARIAS");

        if (totalVentas == 0)
        {
            Console.WriteLine(" No se han registrado ventas en esta sesion.");
            Pausa();
            return;
        }

        decimal promedio  = cajaDiaria / totalVentas;
        int maxVendido    = 0;
        int idxMasVendido = -1;

        for (int i = 0; i < vendidos.Count; i++)
        {
            if (vendidos[i] > maxVendido)
            {
                maxVendido    = vendidos[i];
                idxMasVendido = i;
            }
        }

        Console.WriteLine($" Total de ventas realizadas:      {totalVentas} venta(s)");
        Console.WriteLine($" Total acumulado en caja:         {cajaDiaria,14:C}");
        Console.WriteLine($" Promedio de dinero por venta:    {promedio,14:C}");

        if (idxMasVendido >= 0)
            Console.WriteLine($" Producto mas vendido:            {nombres[idxMasVendido]} ({maxVendido} uds.)");

        Console.WriteLine();
        Console.WriteLine(new string('-', 62));
        Console.WriteLine(" Detalle de unidades vendidas por producto:");
        Console.WriteLine(new string('-', 62));

        for (int i = 0; i < nombres.Count; i++)
        {
            if (vendidos[i] > 0)
                Console.WriteLine($"   {nombres[i],-30}: {vendidos[i],4} unidades vendidas");
        }

        Console.WriteLine(new string('-', 62));
        Pausa();
    }

    static void MensajeSalida()
    {
        Console.WriteLine();
        Console.WriteLine("====================================================");
        Console.WriteLine("  Gracias por usar el Sistema Mini-POS.");
        Console.WriteLine($"  Sesion finalizada. Total en caja: {cajaDiaria:C}");
        Console.WriteLine("  Hasta pronto!");
        Console.WriteLine("====================================================");
        Console.WriteLine();
    }

    static bool LeerSiNo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string entrada = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();

            if (entrada == "S") return true;
            if (entrada == "N") return false;

            Console.WriteLine("[ERROR] Respuesta no valida. Ingrese S o N.");
        }
    }

    static void Pausa()
    {
        Console.WriteLine("\nPresione ENTER para continuar...");
        Console.ReadLine();
    }
}
