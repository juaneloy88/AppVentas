using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VentasPsion.VistaModelo
{
    public class MenuUsuarioVM
    {

        public class MenuPrincipal
        {
            public string Opcion { get; set; }
            public bool Habilitado { get; set; }
            public string Icon { get; set; }
            public MenuPrincipal()
            {

            }
        }
   
        private string TipoMenu;

        public ObservableCollection<MenuPrincipal> oMenuUsuario { get; set; }

        public MenuUsuarioVM(string TipoMenu)
        {
            this.TipoMenu = TipoMenu;
            // constructor
            oMenuUsuario = new ObservableCollection<MenuPrincipal>();
            switch (this.TipoMenu)
            {
                #region PREVENTA
                case "PREVENTA":
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "PREVENTA",
                        Habilitado = true,
                        Icon = "ic_deal.png"
                    });

                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "VISITA",
                        Habilitado = true,
                        Icon = "ic_no_venta.png"
                    });
                    //oMenuUsuario.Add(new MenuPrincipal
                    //{
                    //    Opcion = "COBRANZA Saldo",
                    //    Habilitado = true,
                    //    Icon = "ic_transaction.png"
                    //});
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "COBRANZA Documentos",
                        Habilitado = true,
                        Icon = "ic_transaction.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "ENVASE A RECOGER",
                        Habilitado = true,
                        Icon = "ic_stroller.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CONSULTA",
                        Habilitado = true,
                        Icon = "ic_info.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CONSULTA DOCUMENTOS",
                        Habilitado = true,
                        Icon = "ic_doc.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RELACION ANTICIPO",
                        Habilitado = true,
                        Icon = "ic_doc.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "REGISTRO TELEFONICO",
                        Habilitado = true,
                        Icon = "ic_phone_call.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RESUMEN",
                        Habilitado = true,
                        Icon = "ic_resume.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CENSO COMPETENCIA",
                        Habilitado = true,
                        Icon = "ic_censo_compact.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RETO DEL DIA",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "VOLUMEN DE VENTA",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "ACTIVOS COMODATADOS",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "REPORTE KPIs",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "QUEJAS Y SUGERENCIAS",
                        Habilitado = true,
                        Icon = "ic_like.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "PEDIDO SUGERIDO",
                        Habilitado = true,
                        Icon = "ic_pedido_sugerido.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RECEPCION PARCIAL",
                        Habilitado = true,
                        Icon = "ic_cloud_computing.png"
                    });
                    break;
                #endregion
                #region AUTOVENTA
                case "AUTOVENTA":
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "VENTA",
                        Habilitado = true,
                        Icon = "ic_coins.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "DEVOLUCIONES",
                        Habilitado = true,
                        Icon = "ic_return.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "VISITA",
                        Habilitado = true,
                        Icon = "ic_no_venta.png"
                    });
                    //oMenuUsuario.Add(new MenuPrincipal
                    //{
                    //    Opcion = "COBRANZA Saldo",
                    //    Habilitado = true,
                    //    Icon = "ic_transaction.png"
                    //});
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "COBRANZA Documentos",
                        Habilitado = true,
                        Icon = "ic_transaction.png"
                    });                    
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "ABONO ENVASE",
                        Habilitado = true,
                        Icon = "ic_stroller.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CONSULTA",
                        Habilitado = true,
                        Icon = "ic_info.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CONSULTA DOCUMENTOS",
                        Habilitado = true,
                        Icon = "ic_doc.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RELACION ANTICIPO",
                        Habilitado = true,
                        Icon = "ic_doc.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "REGISTRO TELEFONICO",
                        Habilitado = true,
                        Icon = "ic_phone_call.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RESUMEN",
                        Habilitado = true,
                        Icon = "ic_resume.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CENSO COMPETENCIA",
                        Habilitado = true,
                        Icon = "ic_censo_compact.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RETO DEL DIA",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "VOLUMEN DE VENTA",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "ACTIVOS COMODATADOS",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "REPORTE KPIs",
                        Habilitado = true,
                        Icon = "ic_report.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "QUEJAS Y SUGERENCIAS",
                        Habilitado = true,
                        Icon = "ic_like.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "PEDIDO SUGERIDO",
                        Habilitado = true,
                        Icon = "ic_pedido_sugerido.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RECEPCION PARCIAL",
                        Habilitado = true,
                        Icon = "ic_cloud_computing.png"
                    });
                    break;
                #endregion
                #region REPARTO
                case "REPARTO":
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "REPARTO",
                        Habilitado = true,
                        Icon = "ic_truck.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "DEVOLUCIONES",
                        Habilitado = true,
                        Icon = "ic_return.png"
                    });
                    //oMenuUsuario.Add(new MenuPrincipal
                    //{
                    //    Opcion = "COBRANZA Saldo",
                    //    Habilitado = true,
                    //    Icon = "ic_transaction.png"
                    //});
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "COBRANZA Documentos",
                        Habilitado = true,
                        Icon = "ic_transaction.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "PREVIO",
                        Habilitado = true,
                        Icon = "ic_preview.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CONSULTA",
                        Habilitado = true,
                        Icon = "ic_info.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CONSULTA DOCUMENTOS",
                        Habilitado = true,
                        Icon = "ic_doc.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RESUMEN",
                        Habilitado = true,
                        Icon = "ic_resume.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "CLIENTES REQUISITOS",
                        Habilitado = true,
                        Icon = "ic_checklist.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "REGISTRO TELEFONICO",
                        Habilitado = true,
                        Icon = "ic_phone_call.png"
                    });
                    oMenuUsuario.Add(new MenuPrincipal
                    {
                        Opcion = "RECEPCION PARCIAL",
                        Habilitado = true,
                        Icon = "ic_cloud_computing.png"
                    });
                    break;
                    #endregion
            } // fin switch


            // **********************************************************
            // **********************************************************
            // OPCIONES DEL MENU QUE VAN EN PREVENTA,REPARTO Y AUTOVENTA.
            // **********************************************************
            // **********************************************************
            //oMenuUsuario.Add(new MenuPrincipal
            //{
            //    Opcion = "MAPA",
            //    Habilitado = true,
            //    Icon = "ic_google_maps.png"
            //});
            oMenuUsuario.Add(new MenuPrincipal
            {
                Opcion = "ENCUESTA",
                Habilitado = true,
                Icon = "ic_evaluation.png"
            });

        }
    }


}
