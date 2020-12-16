using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    public class BorraDatosTablasService
    {
        /*Método que consume la Api para borrar los datos de todas las tablas de la BD del dispositivo*/
        public StatusService FtnBorrarDatosTablas()
        {
            try
            {
                var dbConexion = new conexionDB().CadenaConexion();

                dbConexion.DeleteAll<activos_comodatos>();
                dbConexion.DeleteAll<bonificaciones>();
                dbConexion.DeleteAll<candados_productos>();
                dbConexion.DeleteAll<clientes>();
                dbConexion.DeleteAll<clientes_estatus>();
                dbConexion.DeleteAll<clientes_requisitos_surtir>();
                dbConexion.DeleteAll<concepto_devoluciones>();
                dbConexion.DeleteAll<cuota>();
                dbConexion.DeleteAll<departamentos>();
                dbConexion.DeleteAll<devoluciones>();
                //dbConexion.DeleteAll<empleados>();          // No se le borran los datos porque contiene la información de los empleados del perfil que se cargó inicialmente.
                dbConexion.DeleteAll<encuesta>();
                dbConexion.DeleteAll<envase>();
                dbConexion.DeleteAll<envase_temp>();
                dbConexion.DeleteAll<existencia>();
                //dbConexion.DeleteAll<fecha_reparto>();    // No se le borran los datos porque no es una tabla como tal de la BD del dispositivo, es sólo una clase para instanciar y guardar en ella una fecha.
                dbConexion.DeleteAll<gps>();
                dbConexion.DeleteAll<kpis>();
                dbConexion.DeleteAll<leyenda>();
                dbConexion.DeleteAll<lista_maestra>();
                dbConexion.DeleteAll<opciones>();
                dbConexion.DeleteAll<orden_ticket>();
                dbConexion.DeleteAll<password>();
                dbConexion.DeleteAll<pedido>();
                dbConexion.DeleteAll<productos>();
                dbConexion.DeleteAll<promociones>();
                dbConexion.DeleteAll<respuestas>();
                dbConexion.DeleteAll<reto_diario>();
                dbConexion.DeleteAll<ruta>();
                dbConexion.DeleteAll<solicitudes>();
                dbConexion.DeleteAll<tipo_movimiento>();
                dbConexion.DeleteAll<venta_cabecera>();
                dbConexion.DeleteAll<venta_detalle>();
                dbConexion.DeleteAll<volumen_ventas>();
                dbConexion.DeleteAll<conseptos_no_venta>();
                dbConexion.DeleteAll<opciones_app>();
                dbConexion.DeleteAll<version_app>();
                dbConexion.DeleteAll<pagare_clientes>();
                //dbConexion.DeleteAll<complementos>();
                dbConexion.DeleteAll<envase_sugerido>();
                dbConexion.DeleteAll<FacturasVenta>();
                dbConexion.DeleteAll<venta_pagos>();
                dbConexion.DeleteAll<documentos_cabecera>();
                dbConexion.DeleteAll<documentos_detalle>();
                dbConexion.DeleteAll<cat_segmentos>();
                dbConexion.DeleteAll<clientes_competencia>();
                dbConexion.DeleteAll<anticipos>(); 

                return new StatusService
                {
                    status = true,
                    mensaje = "Los datos de todas las tablas de la base de datos fueron borrados correctamente."
                };
            }
            catch (Exception exc)
            {
                return new StatusService
                {
                    status = false,
                    mensaje = "Error: " + exc.Message
                };
            }
        }
    }
}
