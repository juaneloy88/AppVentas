using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class vmPromociones
    {
        cargaPromociones cargaPromo = new cargaPromociones();

        public List<MostrarPromociones> lMuestraPromociones = new List<MostrarPromociones>();
        public List<MostrarPrevioPromocion> lMuestraPrevioPromo = new List<MostrarPrevioPromocion>();        

        #region Se Obtiene la lista de las promociones
        public async Task<List<promociones>> buscaPromociones()
        {
            var promos = await cargaPromo.buscaPromociones();

            return promos;
        }
        #endregion Se Obtiene la lista de las promociones

        #region Se obtiene la lista de Venta y Regalo de acuerdo al Id de la promoción seleccionada multiplicada por el número de promociones
        public async Task<List<MostrarPromociones>> listaVentaRegalo(string sID, int iNumeroPromociones)
        {
            int iVenta, iRegalo;

            this.lMuestraPromociones.Clear();

            var vClaves = await cargaPromo.listaVentaRegalo(sID);
            
            foreach (MostrarPromociones promo in vClaves)
            {
                #region Validación de la promoción si es dinámica o no y multiplicarla por el número de promociones
                if (promo.ptc_tipo.ToString() == "S")
                {
                    iVenta = 0;
                    iRegalo = 0;
                }
                else
                {                    
                    iVenta = iNumeroPromociones * (Convert.ToInt32(promo.arn_cantidad_venta.ToString()));
                    iRegalo = iNumeroPromociones * (Convert.ToInt32(promo.arn_cantidad_regalo.ToString()));
                }
                #endregion Validación de la promoción si es dinámica o no y multiplicarla por el número de promociones

                #region Se insertan los datos en la lista MostrarPromociones
                MostrarPromociones promo1 = new MostrarPromociones();
                promo1.ppn_numero_promocion = Convert.ToInt32(promo.ppn_numero_promocion.ToString());
                promo1.arc_clave_venta = promo.arc_clave_venta.ToString().PadRight(4);
                promo1.ard_corta_vta = promo.ard_corta_vta.ToString().PadRight(10);
                promo1.arn_venta = iVenta;
                promo1.arn_cantidad_venta = iNumeroPromociones * (Convert.ToInt32(promo.arn_cantidad_venta.ToString()));
                promo1.arc_clave_regalo = promo.arc_clave_regalo.ToString().PadRight(4);
                promo1.ard_corta_regalo = promo.ard_corta_regalo.ToString().PadRight(10);
                promo1.arn_regalo = iRegalo;
                promo1.arn_cantidad_regalo = iNumeroPromociones * (Convert.ToInt32(promo.arn_cantidad_regalo.ToString()));
                promo1.ptc_tipo = promo.ptc_tipo.ToString();

                lMuestraPromociones.Add(promo1);
                #endregion Se insertan los datos en la lista MostrarPromociones
            }

            return this.lMuestraPromociones.OrderBy(x => x.arc_clave_venta).ToList();
        }
        #endregion Se obtiene la lista de Venta y Regalo de acuerdo al Id de la promoción seleccionada multiplicada por el número de promociones

        #region Método para realizar el update con la cantidad capturada para el campo de venta
        public async Task<List<MostrarPromociones>> detallePromocionesVentaUpdate(string sClaveVenta, string sCantidad)
        {
            var vValida = this.lMuestraPromociones.Find(x => x.arc_clave_venta.Contains(sClaveVenta));            

            if (vValida != null)
            {
                this.lMuestraPromociones.Remove(vValida);

                MostrarPromociones promo1 = new MostrarPromociones();
                promo1.ppn_numero_promocion = Convert.ToInt32(vValida.ppn_numero_promocion.ToString());
                promo1.arc_clave_venta = vValida.arc_clave_venta.ToString().PadRight(4);
                promo1.arn_venta = Convert.ToInt32(sCantidad);
                promo1.arn_regalo = vValida.arn_regalo;
                promo1.arn_cantidad_venta = vValida.arn_cantidad_venta;
                promo1.arc_clave_regalo = vValida.arc_clave_regalo.ToString().PadRight(4);
                promo1.arn_cantidad_regalo = vValida.arn_cantidad_regalo;
                promo1.ard_corta_vta = vValida.ard_corta_vta.ToString().PadRight(10);
                promo1.ard_corta_regalo = vValida.ard_corta_regalo.ToString().PadRight(10);
                promo1.ptc_tipo = vValida.ptc_tipo.ToString();

                 lMuestraPromociones.Add(promo1);

            }

            return this.lMuestraPromociones.OrderBy(x => x.arc_clave_venta).ToList();
        }
        #endregion Método para realizar el update con la cantidad capturada para el campo de venta

        #region Método para realizar el update con la cantidad capturada para el campo de regalo
        public async Task<List<MostrarPromociones>> detallePromocionesRegaloUpdate(string sClaveRegalo, string sCantidad)
        {
            var vValida = this.lMuestraPromociones.Find(x => x.arc_clave_regalo.Contains(sClaveRegalo));

            if (vValida != null)
            {
                this.lMuestraPromociones.Remove(vValida);

                MostrarPromociones promo1 = new MostrarPromociones();
                promo1.ppn_numero_promocion = Convert.ToInt32(vValida.ppn_numero_promocion.ToString());
                promo1.arc_clave_venta = vValida.arc_clave_venta.ToString().PadRight(4);
                promo1.arn_venta = vValida.arn_venta;
                promo1.arn_regalo = Convert.ToInt32(sCantidad);
                promo1.arn_cantidad_venta = vValida.arn_cantidad_venta;
                promo1.arc_clave_regalo = vValida.arc_clave_regalo.ToString().PadRight(4);
                promo1.arn_cantidad_regalo = vValida.arn_cantidad_regalo;
                promo1.ard_corta_vta = vValida.ard_corta_vta.ToString().PadRight(10);
                promo1.ard_corta_regalo = vValida.ard_corta_regalo.ToString().PadRight(10);
                promo1.ptc_tipo = vValida.ptc_tipo.ToString();

                 lMuestraPromociones.Add(promo1);

            }

            return this.lMuestraPromociones.OrderBy(x => x.arc_clave_venta).ToList();
        }
        #endregion Método para realizar el update con la cantidad capturada para el campo de regalo        

        #region Validación para conocer si la tabla las promociones se encuentran capturadas correctamente
        public async Task<string> validacionPromo(string sID, int iNumeroPromociones)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            int iCantidadVta = 0;
            int iCantidadRegalo = 0;
            int iVenta = 0;
            int iRegalo = 0;
            #endregion Declaración de Variables

            #region Se obtienen las cantidades de venta y de obsequio
            var vCantidades = await cargaPromo.validacionPromo(sID);

            foreach (promociones promo in vCantidades)
            {
                iCantidadVta = iNumeroPromociones * Convert.ToInt32(promo.ppn_cantidad_venta.ToString());
                iCantidadRegalo = iNumeroPromociones * Convert.ToInt32(promo.ppn_cantidad_regalo.ToString());
            }

            var vLista = this.lMuestraPromociones;

            foreach (MostrarPromociones promo in vLista)
            {
                iVenta = iVenta + Convert.ToInt32(promo.arn_venta.ToString());
                iRegalo = iRegalo + Convert.ToInt32(promo.arn_regalo.ToString());
            }
            #endregion Se obtienen las cantidades de venta y de obsequio

            #region Validación de Cantidades de Venta y de Regalo
            if (iVenta == iCantidadVta)
            {
                if (iRegalo == iCantidadRegalo)
                {
                    sRespuesta = "Ok";
                }
                else
                {
                    sRespuesta = "Las Cantidades de Obsequio NO son correctas, deben de ser " + iCantidadRegalo.ToString() + " y estan capturadas " + iRegalo.ToString();
                }
            }
            else
            {
                sRespuesta = "Las Cantidades de Venta NO son correctas, deben de ser " + iCantidadVta.ToString() + " y estan capturadas " + iVenta.ToString();
            }
            #endregion Validación de Cantidades de Venta y de Regalo

            return sRespuesta;
        }
        #endregion Validación para conocer si la tabla las promociones se encuentran capturadas correctamente

        #region Carga el previo de la promoción
        public async Task<List<MostrarPrevioPromocion>> previoPromociones()
        {
            #region Declaración de Variables
            string sMarca = string.Empty;
            string sDesc = string.Empty;
            int iCantidad = 0;
            string sTipo = string.Empty;
            this.lMuestraPrevioPromo.Clear();
            #endregion Declaración de Variables

            #region Método para obtener los productos de venta
            var vListaVenta = this.lMuestraPromociones;

            foreach (MostrarPromociones promo in vListaVenta)
            {
                iCantidad = promo.arn_venta;

                if (iCantidad > 0)
                {                
                    MostrarPrevioPromocion previo = new MostrarPrevioPromocion();
                    previo.arc_clave = promo.arc_clave_venta;
                    previo.ard_corta = promo.ard_corta_vta;
                    previo.ppn_cantidad = iCantidad;
                    previo.ppc_tipo = "Venta";

                    lMuestraPrevioPromo.Add(previo);
                }
            }
            #endregion Método para obtener los productos de venta

            #region Método para obtener los productos de regalo
            var vListaRegalo = this.lMuestraPromociones;

            foreach (MostrarPromociones promo in vListaRegalo)
            {
                iCantidad = promo.arn_regalo;

                if (iCantidad > 0)
                {
                    MostrarPrevioPromocion previo = new MostrarPrevioPromocion();
                    previo.arc_clave = promo.arc_clave_regalo;
                    previo.ard_corta = promo.ard_corta_regalo;
                    previo.ppn_cantidad = iCantidad;
                    previo.ppc_tipo = "Obsequio";

                     lMuestraPrevioPromo.Add(previo);
                }
                    
            }
            #endregion Método para obtener los productos de regalo

            return lMuestraPrevioPromo.OrderBy(x => x.arc_clave).ToList(); 
        }
        #endregion Carga el previo de la promoción

        #region Método que Guarda los registros tanto de Venta como de Regalo en la tabla de venta_detalle
        public async Task<string> GuardaVenta(List<MostrarPromociones> lMuestraPromociones)
        {
            string sRespuesta = string.Empty;

            var vLista = lMuestraPromociones;

            sRespuesta = await cargaPromo.GuardaVenta(vLista);

            return sRespuesta;
        }
        #endregion Método que Guarda los registros tanto de Venta como de Regalo en la tabla de venta_detalle
    }

    public class MostrarPromociones
    {
        public int ppn_numero_promocion { get; set; }        
        public string arc_clave_venta { get; set; }
        public int arn_cantidad_venta { get; set; }
        public int arn_venta { get; set; }        
        public string ard_corta_vta { get; set; }        
        public string arc_clave_regalo { get; set; }
        public int arn_cantidad_regalo { get; set; }
        public int arn_regalo { get; set; }
        public string ard_corta_regalo { get; set; }
        public string ptc_tipo { get; set; }

        public MostrarPromociones()
        {
            ppn_numero_promocion = 0;
            arc_clave_venta = "";
            arn_cantidad_venta = 0;
            arn_venta = 0;
            ard_corta_vta = "";
            arc_clave_regalo = "";
            arn_cantidad_regalo = 0;
            arn_regalo = 0;
            ard_corta_regalo = "";
            ptc_tipo = "";
        }
    }

    public class MostrarPrevioPromocion
    {
        public string arc_clave { get; set; }
        public string ard_corta { get; set; }
        public int ppn_cantidad { get; set; }
        public string ppc_tipo { get; set; }

        public MostrarPrevioPromocion()
        {
            arc_clave = "";
            ard_corta = "";
            ppn_cantidad = 0;
            ppc_tipo = "";
        }
    }
}
