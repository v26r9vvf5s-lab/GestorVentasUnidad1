# Sistema Gestor de Ventas e Inventario (Mini-POS)

## Información del Estudiante

| Campo | Detalle |
|---|---|
| **Nombre completo** | Jhon Mateo Guette Vera |
| **Módulo** | Unidad 1 — Fundamentos de C# (.NET 8) |
| **Fecha** | Septiembre 2026 |

---

## Descripción del Proyecto

Aplicación de consola desarrollada en **C# (.NET 8)** que simula un **Punto de Venta (POS) y Gestor de Inventario** para una tienda de barrio.

El sistema permite:

- 📦 **Registrar productos** con nombre, precio y stock inicial.
- 📋 **Consultar el inventario** con alertas de bajo stock.
- 🛒 **Procesar ventas** con cálculo automático de IVA (19%) y descuento de cliente frecuente (10%).
- 📊 **Ver estadísticas diarias** de caja: total ingresado, número de ventas, promedio por venta y producto más vendido.

> **Restricción de diseño:** Implementado exclusivamente con los conceptos de la Unidad 1 (variables, `List<T>`, control de flujo, métodos estáticos, `try/catch`, `TryParse`). No se utilizan clases personalizadas (POO).

---



## Estructura del Proyecto

```
GestorVentasUnidad1/
├── Program.cs                  ← Código fuente principal
├── GestorVentasUnidad1.csproj  ← Configuración del proyecto
├── .gitignore                  ← Exclusión de bin/ y obj/
└── README.md                   ← Este archivo
```

---

## Métodos Estáticos Implementados

| Método | Propósito |
|---|---|
| `LeerEntero(mensaje, min, max)` | Lectura segura de enteros con validación de rango |
| `LeerDecimal(mensaje, min)` | Lectura segura de decimales con validación |
| `CalcularFactura(precio, cantidad, descuento, out iva, out desc)` | Cálculo de subtotal, descuento e IVA |
| `ImprimirEncabezado(titulo)` | Formateo visual de encabezados de pantalla |

---

## Ejemplo de Ejecución

### Menú Principal

```
====================================================
     SISTEMA GESTOR DE VENTAS E INVENTARIO (MINI-POS)
====================================================
 1. Registrar nuevo producto en inventario
 2. Consultar inventario completo
 3. Registrar una venta
 4. Ver reporte de caja y estadísticas diarias
 5. Salir
====================================================
Seleccione una opción (1-5):
```

### Ticket de Venta Generado

```
====================================================
              TICKET DE VENTA
====================================================
 Producto:          Café Colombiano 500g (x2)
 Subtotal:              $36.000,00
 Descuento (10%):      -$ 3.600,00
 IVA (19%):            +$ 6.156,00
 ---------------------------------------------------
 TOTAL A PAGAR:         $38.556,00
====================================================
[OK] Venta efectuada con éxito. Stock actualizado: 8 unidades.
```

### Manejo de Errores

```
Seleccione una opción (1-5): abc
[ERROR] Entrada no válida. Debe ingresar un número entero.

Seleccione una opción (1-5): 9
[ERROR] Opción fuera de rango. Ingrese un valor entre 1 y 5.
```

---

## Lógica de Cálculo

$$\text{Subtotal} = \text{Precio} \times \text{Cantidad}$$

$$\text{Descuento} = \text{Subtotal} \times 10\% \quad \text{(si aplica)}$$

$$\text{IVA} = (\text{Subtotal} - \text{Descuento}) \times 19\%$$

$$\text{Total} = \text{Subtotal} - \text{Descuento} + \text{IVA}$$

---

## Licencia

Proyecto académico — Uso educativo.
