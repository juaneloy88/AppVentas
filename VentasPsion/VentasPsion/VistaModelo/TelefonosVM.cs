using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    class TelefonosVM
    {
        public telefonos_clientes Tel = new telefonos_clientes();
        public List<telefonos_clientes> Telefonos = new List<telefonos_clientes>();

        private telefonos_clientesSR TelSR = new telefonos_clientesSR();

        public bool GuardaRegistro()
        {
            return TelSR.GuardaTelefono(Tel)<=0?false:true;
        }

        public bool ListaTelefonos()
        {
            try
            {
                Telefonos = TelSR.ListaRegTel();

                if (Telefonos == null)
                {
                    VarEntorno.sMensajeError = "No Hay Registros Telefonicos";
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

        public void limpia()
        {
            Tel = new telefonos_clientes();
        }

    }
}
