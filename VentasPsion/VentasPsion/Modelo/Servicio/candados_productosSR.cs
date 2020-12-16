using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class candados_productosSR
    {

        conexionDB cODBC = new conexionDB();

        //* busca si la clave tiene alguna restriccion y si la tiene valida que la cumpla  *//
        //public int  ValidaCandadosProducto(string sClave,int iCantidad)
        //{
        //    try
        //    {
        //        List<candados_productos> oLista = new List<candados_productos>();
        //        int iRegistros = 0;

        //        using (SQLiteConnection conn = cODBC.CadenaConexion())
        //        {
        //            var sQuery = "Select count(*) from candados_productos WHERE  arc_clave = ? ";
        //            iRegistros = conn.ExecuteScalar<int>(sQuery, sClave);
        //            if (iRegistros > 0)
        //            {
        //                sQuery = "Select * from candados_productos WHERE arc_clave = ? ";
        //                oLista = conn.Query<candados_productos>(sQuery);
        //                return 1;
        //            }
        //            else
        //                return 1;
        //        }

              
        //    }
        //    catch (Exception ex)
        //    {
        //        VarEntorno.sMensajeError = ex.Message;
        //        return -1;
        //    }
        //}

       
        public bool fnProductoconRestriccionV(string sProducto, string sMod, string sSegmento)
        {
            try
            {
                int iResultado = 0;
                List<candados_productos> lsCandados = new List<candados_productos>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //regexp_split_to_table(cpc_segmento,',') 
                    var sQuery = @"Select * from candados_productos   ";
                    lsCandados = conn.Query<candados_productos>(sQuery);

                    sQuery = @"Select * from candados_productos WHERE arc_clave = ?  ";
                    lsCandados = conn.Query<candados_productos>(sQuery, sProducto);
                }

                foreach (var cand in lsCandados)
                {
                    string[] seg = cand.cpc_segmento.Split(',');

                    foreach(var s in seg)
                        if (s.Equals(sSegmento))
                            iResultado = 1;

                    string[] mod = cand.cpc_mod.Split(',');

                    foreach (var m in mod)
                        if (m.Equals(sMod))
                            iResultado = 1;
                }

                if (iResultado == 1)
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


        public bool fnProductoconRestriccion(string sProducto,string sMod,string sSegmento)
        {
            try
            {
                int iResultado = 0;
                string sSegC = "";

                if (sSegmento == "C")
                    sSegC = "";
                else
                    sSegC = "%";

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = @"Select count(*) from candados_productos WHERE arc_clave = ?  and (cpc_mod like '%" + sMod + @"%' or  CASE 
                                        WHEN cpc_segmento LIKE '" + sSegC + sSegmento+ @"%' AND length('" + sSegmento + @"')=1 THEN 'true' 
                                        WHEN cpc_segmento LIKE '%" + sSegmento+ @"%' AND length('" + sSegmento + @"')=2   THEN 'true' 

                                        END ) ";
                    iResultado = conn.ExecuteScalar<int>(sQuery, sProducto);
                }

                if (iResultado > 0)
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

        public string fnValidaRestriccion(string sProducto,int iCantidad, string sMod, string sSegmento)
        {
            try
            {
                bool bResultado= true ;
                string sRespuesta = "";

                List<candados_productos> lCandados = new List<candados_productos>();
                List<candados_productos> lsCandados = null; 

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //regexp_split_to_table(cpc_segmento,',') 
                    var sQuery = @"Select * from candados_productos WHERE arc_clave = ?  ";
                    lsCandados = conn.Query<candados_productos>(sQuery, sProducto);
                }

                foreach (var cand in lsCandados)
                {
                    string[] seg = cand.cpc_segmento.Split(',');

                    foreach (var s in seg)
                        if (s.Equals(sSegmento))
                            lCandados.Add(cand);

                    string[] mod = cand.cpc_mod.Split(',');

                    foreach (var m in mod)
                        if (m.Equals(sMod))
                            lCandados.Add(cand);
                }

                foreach (var Candado in lCandados)
                {
                    switch (Candado.cpn_tipo)
                    {
                        case 1:
                            ////maximo de venta
                            if (iCantidad <= Candado.cpn_cantidad)
                            { }
                            else
                            {
                                bResultado = false;
                                sRespuesta = "Supero la cantidad "+ Candado.cpn_cantidad + " de venta de "+ sProducto + " por cliente:[" + iCantidad + "]; atte:PPM ";
                            }
                            break;
                        case 2:
                            //minimo de venta
                            if (iCantidad >= Candado.cpn_cantidad)
                            { }
                            else
                            {
                                bResultado = false;
                                sRespuesta = "No Cumple con la cantidad minima de " + Candado.cpn_cantidad + "  de venta de " + sProducto + " por cliente:["+ iCantidad+ "] ; atte:PPM ";
                            }
                            break;
                        case 3:
                            // factores de venta 
                            if ((iCantidad % Candado.cpn_cantidad)==0)
                            { }
                            else
                            {
                                bResultado = false;
                                sRespuesta = "la cantidad total ["+ iCantidad + "] de "+ sProducto + " no son multiplo  " + Candado.cpn_cantidad + "; atte:PPM ";
                            }
                            break;
                        default:
                            bResultado = false;
                            sRespuesta = "Error";
                            VarEntorno.sMensajeError = "Tipo de restriccion desconocido";
                            break;
                    }

                    if (bResultado == false)
                        break;
                }

                return sRespuesta;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return "Error";
            }
        }

    }
}
