using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class facturasventasSR
    {
        conexionDB cODBC = new conexionDB();

        public List<FacturasVenta> ListaDoctos;

        public string fnFacturasDevolucion(int iCliente)
        {
            try
            {
                string sResultado = "";
                
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from FacturasVenta where vcn_cliente = ? and length(vcn_folio)= 6";
                    List<FacturasVenta>  vLista = conn.Query<FacturasVenta>(sQuery, iCliente);

                    int i=0;
                    foreach (var Factura in vLista)
                    {
                        if (i != 0)
                            sResultado = sResultado + ",";

                        sResultado = sResultado + Factura.vcc_cadena_original;
                        i++;
                    }

                }                

                return sResultado;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return "";
            }
        }

        
        public List<FacturasVenta> fnListaFacturas(int iCliente)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from FacturasVenta where vcn_cliente = ? and length(vcn_folio)= 6 order by vcf_movimiento desc ";
                    List<FacturasVenta> vLista = conn.Query<FacturasVenta>(sQuery, iCliente);

                    return vLista;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        /* Se consulta el documento del dia anterior para poder generar detalle pago si se llega a pagar */
        /* en caso de multiples tickest de pedido se trae uno de ellos y se bloquea el pago por la pantalla de pedido  */
        public bool UltimaFactura()
        {
            try
            {
                ListaDoctos = fnListaFacturas(VarEntorno.vCliente.cln_clave);
                FacturasVenta LMax = new FacturasVenta();
                if (ListaDoctos.Count > 1)
                    LMax = ListaDoctos[0].vcn_folio != "0" ? ListaDoctos[0] : ListaDoctos[1];
                else
                    LMax = ListaDoctos[0];

                foreach (var fac in ListaDoctos)
                    if (fac.vcf_movimiento > LMax.vcf_movimiento && fac.vcn_folio!="0")
                        LMax = fac;

                ListaDoctos.Clear();
                ListaDoctos.Add(LMax);

                return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public decimal AbonoPreventa(int iCliente)
        {
            try
            {
                decimal nCantidad = 0M;
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"select sum(fvn_pagos) from FacturasVenta 
                                where vcn_cliente = ?  and length(vcn_folio)= 6 ";

                    nCantidad = conn.ExecuteScalar<decimal>(sQuery, iCliente);
                 }
                 return nCantidad;
            }
            catch (Exception e)
            {
                    VarEntorno.sMensajeError = e.Message;
                    return -1M;
            }
        }
    }
}
