using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    class clientes_requisitos_surtirVM
    {

        private clientes_requisitos_surtirSR cFunciones = new clientes_requisitos_surtirSR();
        public clientes_requisitos_surtir cDatos;

        public bool TraeDatosSurtir(int nCliente)
        {
           
            try
            {
                cDatos = cFunciones.TraeDatos(nCliente);
            }
            catch
            {
                cDatos = null;
            }

            if (cDatos == null)
                return false;
            else
                return true;
        }

        public bool GuardaDatosSurtir(int nCliente)
        {
            bool bResultado = false;
            try
            {
                bResultado = cFunciones.ActualizaDatos(nCliente,cDatos);
            }
            catch
            {
                bResultado = false;
            }

            return bResultado;
        }

        public clientes_requisitos ClientesReqs()
        {
            clientes_requisitos ObjClientes = null;

            ObjClientes= cFunciones.RequisitosClientes();

            return ObjClientes;
        }
    }
}
