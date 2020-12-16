using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Servicio;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.VistaModelo
{
    public class CensoVM
    {
        public clientes_competenciaSR CompetenciaFn = new clientes_competenciaSR();
        public clientes_competencia ClienteComp = new clientes_competencia();

        public bool GuardarComp()
        {            
            try
            {
                return CompetenciaFn.Guardar(ClienteComp);
            }
            catch
            {
                return false;
            }
        }
    }
}
