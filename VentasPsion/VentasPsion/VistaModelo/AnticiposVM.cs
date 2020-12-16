using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class AnticiposVM
    {
        public List<anticipos> lAnticipos = new List<anticipos>();       

        private anticipoSR FnAnticipo = new anticipoSR();

        public bool lAnt()
        {
            try
            {
                lAnticipos = FnAnticipo.ListaAnticipos();
                if (lAnticipos is null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public bool RelacionaAnticipo(string sFolioAnticipo,string sFolioCabecera)
        {
            try
            {
                if (FnAnticipo.UpdateRelacionado(sFolioAnticipo, sFolioCabecera))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public string vcn_folio { get; set; }

        public DateTime anfMovimiento { get; set; }

        public decimal dMontoPago { get; set; }

        public void LimpiaVariables()
        {
            lAnticipos = null;
            vcn_folio = "";
            dMontoPago = 0M;
        }
    }
}