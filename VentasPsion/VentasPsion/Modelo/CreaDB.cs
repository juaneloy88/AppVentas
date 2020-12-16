using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo
{
    class CreaDB
    {
        public CreaDB()
        {
            MtdGenerarBD();
        }

        private void MtdGenerarBD()
        {
            conexionDB cODBC = new conexionDB();
            SQLiteConnection conn;

            conn = cODBC.CadenaConexion();

            conn.CreateTable<bonificaciones>();
            conn.CreateTable<clientes>();
            conn.CreateTable<clientes_estatus>();
            conn.CreateTable<concepto_devoluciones>();
            conn.CreateTable<empleados>();

            conn.CreateTable<envase>();
            conn.CreateTable<existencia>();
            conn.CreateTable<gps>();
            conn.CreateTable<orden_ticket>();
            conn.CreateTable<password>();

            conn.CreateTable<pedido>();
            conn.CreateTable<productos>();
            conn.CreateTable<ruta>();
            conn.CreateTable<candados_productos>();
            conn.CreateTable<cuota>();

            conn.CreateTable<devoluciones>();
            conn.CreateTable<encuesta>();
            conn.CreateTable<leyenda>();
            conn.CreateTable<promociones>();
            conn.CreateTable<respuestas>();

            conn.CreateTable<venta_cabecera>();
            conn.CreateTable<venta_detalle>();
            conn.CreateTable<lista_maestra>();

            conn.CreateTable<reto_diario>();
            conn.CreateTable<volumen_ventas>();
            conn.CreateTable<activos_comodatos>();
            conn.CreateTable<kpis>();
            conn.CreateTable<opciones>();
            conn.CreateTable<departamentos>();

            conn.CreateTable<solicitudes>();
            conn.CreateTable<tipo_movimiento>();
            conn.CreateTable<conseptos_no_venta>();
            conn.CreateTable<envase_temp>();

            conn.CreateTable<version_app>();
            conn.CreateTable<opciones_app>();

            conn.CreateTable<pagare_clientes>();
            //conn.CreateTable<complementos>();
            conn.CreateTable<envase_sugerido>();
            conn.CreateTable<FacturasVenta>();
            conn.CreateTable<venta_pagos>();
            conn.CreateTable<documentos_cabecera>();
            conn.CreateTable<documentos_detalle>();
            conn.CreateTable<clientes_requisitos_surtir>();
            conn.CreateTable<cat_segmentos>();

            conn.CreateTable<clientes_competencia>();
            conn.CreateTable<anticipos>();
            conn.CreateTable<telefonos_clientes>();

            string name = Plugin.Settings.CrossSettings.Current
            .GetValueOrDefault<string>("Estatus", "false");

            // if (name == "" || name == "false")
            {
                //MtdCargaDatosIniciales(conn);
                Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Estatus", "OK");
            }

        }

    }
}
