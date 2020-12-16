using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    class VisitaVM
    {
        public List<conseptos_no_venta> ListaConseptos = new List<conseptos_no_venta>();
        public clientes_estatusSR NoVenta = new clientes_estatusSR();


        public  bool ValidaEstatusCliente()
        {
            //string sCliente = VarEntorno.vCliente.cln_clave.ToString();

            int sRespuesta = NoVenta.ObtieneClientesSinVenta(VarEntorno.vCliente.cln_clave);

            if (sRespuesta == 0 )
                return true;
            else
                return false;
        }

        //******  se realizan todos los procesos      ***//
        public bool GuardaMotivo(int iMotivo)
        {
            try
            {
                bool bResultado = false;
                //fnClientes_Estatus cEstatusClientes = new fnClientes_Estatus();

                bResultado = NoVenta.fnActualizaEstatus_Noventa(VarEntorno.vCliente.cln_clave, iMotivo, 4);
                //if (bResultado == true)
                //{
                //    bResultado = NoVenta.fnActualizaEstatus(VarEntorno.vCliente.cln_clave, 4);                    
                //}

                return bResultado;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //*****  carga los conseptos de devoluciones para mostrarse en pantalla   *****//
        public bool lConseptosNoventa()
        {
            try
            {
                ListaConseptos = new conseptoSinVentaSR().ListaConseptos();

                if (ListaConseptos == null)
                {
                    VarEntorno.sMensajeError = "No Hay Conseptos";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }

        }

    }
}
