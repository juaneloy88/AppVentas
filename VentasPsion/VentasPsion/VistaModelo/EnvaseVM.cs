using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class EnvaseVM
    {
        EnvaseService envaseService = new EnvaseService();

        /*Método para conectarse al modelo-servicio que regresa la Lista de Envases a los cuales puede abonar un Cliente específico*/
        public StatusService FtnRegresarEnvasesPorClienteVM(int iIdCliente)
        {
            StatusService statusService = new StatusService();
            EnvaseService envaseService = new EnvaseService();

            // Manda llamar a la función que regresa la Lista de Envases a los cuales puede abonar el Cliente enviado como parámetro.
            statusService = envaseService.FtnRegresarEnvasesPorCliente(iIdCliente);

            return statusService;
        }

        /*Método para conectarse al modelo-servicio que regresa el Resumen de Abono de Envase de un Cliente específico*/
        public async Task<List<envase>> FtnRegresarResumenEnvasePorClienteVM(int iIdCliente)
        {
            // Manda llamar a la función que regresa el Resumen de Abono de Envase del Cliente enviado como parámetro.
            List<envase> loListaEnvases = await envaseService.FtnRegresarResumenEnvasePorCliente(iIdCliente);

            return loListaEnvases;
        }

        /*Método para conectarse al modelo-servicio que regresa el Resumen de Abono de Envase de un Cliente específico*/
        public bool FtnInsertaEnvaseTemp(int iCliente)
        {
            bool bRespuesta = false;
            bRespuesta = envaseService.FtnInsertaEnvaseTemp(iCliente);
            return bRespuesta;
        }

        /*Método para borral la tabla temporal envase_temp*/
        
        public bool FtnBorrarEnvaseTemp(int iCliente)
        {
            bool bRespuesta = false;
            bRespuesta = envaseService.FtnBorrarEnvaseTemp(iCliente);
            return bRespuesta;
        }
        
    }
}
