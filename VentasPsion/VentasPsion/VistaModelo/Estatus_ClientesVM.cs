using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class Estatus_ClientesVM
    {
        fnStatusClientes fnStatusCtes = new fnStatusClientes();
        conseptoDevolucionesSR SRde = new conseptoDevolucionesSR();
        fnVentaCabecera VenCab = new fnVentaCabecera();

        #region ANT
        /*
        clientes_estatusSR oEstatus = new clientes_estatusSR();        

        public bool traePagare(int iCliente)
        {
            return oEstatus.fnPagareCliente(iCliente);
        }


        public bool traeClienteFoco(int iCliente)
        {
            return oEstatus.fnFocoCliente(iCliente);
        }

        public bool traeCoolerCliente(int iCliente)
        {
            return oEstatus.fnCoolerCliente(iCliente);
        }

        public bool ActualizaPagare(int iCliente, bool bEstatus)
        {
            return oEstatus.fnActualizaPagare(iCliente, bEstatus);
        }

        public bool ActualizaCooler(int iCliente, bool bEstatus)
        {
            return oEstatus.fnActualizaCooler(iCliente, bEstatus);
        }

        public bool ActualizaFoco(int iCliente, bool bEstatus)
        {
            return oEstatus.fnActualizaFoco(iCliente, bEstatus);
        }
        */
        #endregion
    }
}
