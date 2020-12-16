using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class vwBonificaciones
    {
        bonificacionesSR fBonis = new bonificacionesSR();

        List<MostrarBonis> lMuestraBoni = new List<MostrarBonis>();

        #region Método que busca la bonificación del cliente
        public async Task<List<bonificaciones>> BonificacionesCliente(string sFolio, string sCliente)
        {
            var vLista = await fBonis.buscaBonificaciones(sFolio, sCliente);

            return vLista;
        }
        #endregion Método que busca la bonificación del cliente 

        #region Se inserta en la lista la Boni para ser mostrada
        public async Task<List<MostrarBonis>> listaBonificaciones(string sfolio, string sTipo, decimal dImporte)
        {
            MostrarBonis mostrarBonis = new MostrarBonis();
            mostrarBonis.boc_folio = sfolio;
            mostrarBonis.boi_documento = dImporte;
            mostrarBonis.boc_tipo = sTipo;
            this.lMuestraBoni.Add(mostrarBonis);

            return  this.lMuestraBoni.OrderBy(x => x.boc_folio).ToList();
        }
        #endregion Se inserta en la lista la Boni para ser mostrada        

        #region Valida si ya se encuentra aplicada la Bonificación
        public async Task<string> validaSiEstaAplicadaBoni(string sfolio)
        {
            return await fBonis.validaSiEstaAplicadaBoni(sfolio);
        }
        #endregion Valida si ya se encuentra aplicada la Bonificación
    }

    public class MostrarBonis
    {
        public string boc_folio { get; set; }        
        public decimal boi_documento { get; set; }        
        public string boc_tipo { get; set; }

        public MostrarBonis()
        {
            boc_folio = "";
            boi_documento = 0;
            boc_tipo = "";
        }
    }
}