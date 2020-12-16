using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.VistaModelo;

namespace VentasPsion.VistaModelo
{

    public class MenuPrincipalVM
    {
        //
        public class MenuPrincipal
        {
            public string Opcion { get; set; }
            public bool Habilitado { get; set; }
            public string bindHabilidado { get; set; }
            public string Icon { get; set; }
            public MenuPrincipal()
            {

            }
        }
        //

        /**********************************************************/
        /*****************SE AGREGA OTRA LOGICA MVC****************/
        /**********************************************************/
        public class ItemMenuPrincipal : INotifyPropertyChanged
        {
            private string _Opcion;
            private bool _Habilitado;
            private bool _isExtraControlsVisible;
            private string _bindHabilidado;
            private string _Icon;

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            public string bindHabilidado
            {
                get { return _bindHabilidado; }
                set
                {
                    _bindHabilidado = value;
                    OnPropertyChanged();
                }
            }

            public string Icon
            {
                get { return _Icon; }
                set
                {
                    _Icon = value;
                    OnPropertyChanged();
                }
            }

            public string Opcion
            {
                get { return _Opcion; }
                set
                {
                    _Opcion = value;
                    OnPropertyChanged();
                }
            }

            public bool Habilitado
            {
                get { return _Habilitado; }
                set
                {
                    _Habilitado = value;
                    OnPropertyChanged();
                }
            }

            public bool IsExtraControlsVisible
            {
                get { return _isExtraControlsVisible; }
                set
                {
                    _isExtraControlsVisible = value;
                    OnPropertyChanged();
                }
            }
        } // fin clase


        //

        public class DynamicListViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<ItemMenuPrincipal> _allItems;
            private ItemMenuPrincipal _mySelectedItem;

            public event PropertyChangedEventHandler PropertyChanged;

           
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            public ObservableCollection<ItemMenuPrincipal> ObtenerMenuObjeto()
            {
                ObservableCollection<ItemMenuPrincipal> oMenuPrincipal = new ObservableCollection<ItemMenuPrincipal>();
                if (VarEntorno.iNoRuta != 0)
                {
                    //  sRuta = VarEntorno.iNoRuta.ToString() + " - ";
                    oMenuPrincipal.Add(new ItemMenuPrincipal
                    {
                        Opcion = "RECEPCION",
                        Habilitado = false,
                        bindHabilidado = "False",
                        Icon = "ic_cloud_computing"
                    });

                    if (!VarEntorno.bInicioTurno)
                    {

                        oMenuPrincipal.Add(new ItemMenuPrincipal
                        {
                            Opcion = "INICIO DE TURNO",
                            Habilitado = true,
                            bindHabilidado = "True",
                            Icon = "ic_route.png"
                        });
                        oMenuPrincipal.Add(new ItemMenuPrincipal
                        {
                            Opcion = "OPERACIONES",
                            Habilitado = false,
                            bindHabilidado = "False",
                            Icon = "ic_stroller.png"
                        });
                        oMenuPrincipal.Add(new ItemMenuPrincipal
                        {
                            Opcion = "FIN DE TURNO",
                            Habilitado = false,
                            bindHabilidado = "False",
                            Icon = "ic_route.png"
                        });
                        oMenuPrincipal.Add(new ItemMenuPrincipal
                        {
                            Opcion = "TRANSMISION",
                            Habilitado = false,
                            bindHabilidado = "True",
                            Icon = "ic_upload.png"
                        });

                    }
                    else
                    {
                        oMenuPrincipal.Add(new ItemMenuPrincipal
                        {
                            Opcion = "INICIO DE TURNO",
                            Habilitado = false,
                            bindHabilidado = "False",
                            Icon = "ic_route.png"
                        });


                        if (!VarEntorno.bFinTurno)
                        {
                            oMenuPrincipal.Add(new ItemMenuPrincipal
                            {
                                Opcion = "OPERACIONES",
                                Habilitado = true,
                                bindHabilidado = "True",
                                Icon = "ic_stroller.png"
                            });
                            oMenuPrincipal.Add(new ItemMenuPrincipal
                            {
                                Opcion = "FIN DE TURNO",
                                Habilitado = true,
                                bindHabilidado = "True",
                                Icon = "ic_route.png"
                            });
                            oMenuPrincipal.Add(new ItemMenuPrincipal
                            {
                                Opcion = "TRANSMISION",
                                Habilitado = false,
                                bindHabilidado = "True",
                                Icon = "ic_upload.png"
                            });

                        }
                        else
                        {
                            oMenuPrincipal.Add(new ItemMenuPrincipal
                            {
                                Opcion = "OPERACIONES",
                                Habilitado = false,
                                bindHabilidado = "False",
                                Icon = "ic_stroller.png"
                            });
                            oMenuPrincipal.Add(new ItemMenuPrincipal
                            {
                                Opcion = "FIN DE TURNO",
                                Habilitado = false,
                                bindHabilidado = "False",
                                Icon = "ic_route.png"
                            });
                            oMenuPrincipal.Add(new ItemMenuPrincipal
                            {
                                Opcion = "TRANSMISION",
                                Habilitado = false,
                                bindHabilidado = "True",
                                Icon = "ic_upload.png"
                            });
                        }
                    }
                }
                else
                {
                    oMenuPrincipal.Add(new ItemMenuPrincipal
                    {
                        Opcion = "RECEPCION",
                        Habilitado = true,
                        bindHabilidado = "True",
                        Icon = "ic_cloud_computing"
                    });
                    oMenuPrincipal.Add(new ItemMenuPrincipal
                    {
                        Opcion = "INICIO DE TURNO",
                        Habilitado = false,
                        bindHabilidado = "False",
                        Icon = "ic_route.png"
                    });
                    oMenuPrincipal.Add(new ItemMenuPrincipal
                    {
                        Opcion = "OPERACIONES",
                        Habilitado = false,
                        bindHabilidado = "False",
                        Icon = "ic_stroller.png"
                    });
                    oMenuPrincipal.Add(new ItemMenuPrincipal
                    {
                        Opcion = "FIN DE TURNO",
                        Habilitado = false,
                        bindHabilidado = "False",
                        Icon = "ic_route.png"
                    });
                    oMenuPrincipal.Add(new ItemMenuPrincipal
                    {
                        Opcion = "TRANSMISION",
                        Habilitado = false,
                        bindHabilidado = "True",
                        Icon = "ic_upload.png"
                    });
                }


                oMenuPrincipal.Add(new ItemMenuPrincipal
                {
                    Opcion = "UTILERIAS",
                    Habilitado = true,
                    bindHabilidado = "True",
                    Icon = "ic_settings.png"

                });
                oMenuPrincipal.Add(new ItemMenuPrincipal
                {
                    Opcion = "SALIR",
                    Habilitado = true,
                    bindHabilidado = "True",
                    Icon = "ic_logout.png"
                });

                return oMenuPrincipal;


            } // Fin del método ObtenerMenu


            public DynamicListViewModel()
            {
                //AllItems =  new ObservableCollection<ItemMenuPrincipal>(new List<Item> { new Item { MyText = "1" }, new Item { MyText = "2" }, new Item { MyText = "3" } });
                AllItems = ObtenerMenuObjeto();
            }

            public ObservableCollection<ItemMenuPrincipal> AllItems
            {
                get { return _allItems; }
                set
                {
                    _allItems = value;
                    OnPropertyChanged();
                }
            }

            public ItemMenuPrincipal MySelectedItem
            {
                get { return _mySelectedItem; }
                set
                {
                    _mySelectedItem = value;
                    OnPropertyChanged();

                    foreach (var item in AllItems)
                    {
                        item.IsExtraControlsVisible = false;
                    }
                    var selectedItem = AllItems.FirstOrDefault(x => x.Equals(value));
                    if (selectedItem != null)
                    {
                        selectedItem.IsExtraControlsVisible = true;
                    }
                }
            }
        } // fin de clase

        /**********************************************************/
        /*****************SE AGREGA OTRA LOGICA MVC****************/
        /**********************************************************/



        //  public ObservableCollection<MenuPrincipal> oMenuPrincipal { get; set; }
        public MenuPrincipalVM()
        {
            // constructor
        }


        public async Task<List<MenuPrincipal>> ObtieneListaMenu()
        {
            List<MenuPrincipal> lObtieneMenu = new List<MenuPrincipal>();
            var ObjObtenerMenu = ObtenerMenu();
            var list = new List<MenuPrincipal>();
            for (int i=0; i< ObjObtenerMenu.Count; i++)
            {
                list.Add(ObjObtenerMenu[i]);
            }
            return list;
        } 

        public ObservableCollection<MenuPrincipal> ObtenerMenu()
        {
            ObservableCollection<MenuPrincipal> oMenuPrincipal = new ObservableCollection<MenuPrincipal>();
            if (VarEntorno.iNoRuta != 0)
            {
                //  sRuta = VarEntorno.iNoRuta.ToString() + " - ";
                oMenuPrincipal.Add(new MenuPrincipal
                {
                    Opcion = "RECEPCION",
                    Habilitado = false,
                    bindHabilidado = "False",
                    Icon = "ic_cloud_computing"
                });

                if (!VarEntorno.bInicioTurno)
                {

                    oMenuPrincipal.Add(new MenuPrincipal
                    {
                        Opcion = "INICIO DE TURNO",
                        Habilitado = true,
                        bindHabilidado = "True",
                        Icon = "ic_route.png"
                    });
                    oMenuPrincipal.Add(new MenuPrincipal
                    {
                        Opcion = "OPERACIONES",
                        Habilitado = false,
                        bindHabilidado = "False",
                        Icon = "ic_stroller.png"
                    });
                    oMenuPrincipal.Add(new MenuPrincipal
                    {
                        Opcion = "FIN DE TURNO",
                        Habilitado = false,
                        bindHabilidado = "False",
                        Icon = "ic_route.png"
                    });
                    oMenuPrincipal.Add(new MenuPrincipal
                    {
                        Opcion = "TRANSMISION",
                        Habilitado = false,
                        bindHabilidado = "True",
                        Icon = "ic_upload.png"
                    });

                }
                else
                {
                    oMenuPrincipal.Add(new MenuPrincipal
                    {
                        Opcion = "INICIO DE TURNO",
                        Habilitado = false,
                        bindHabilidado = "False",
                        Icon = "ic_route.png"
                    });


                    if (!VarEntorno.bFinTurno)
                    {
                        oMenuPrincipal.Add(new MenuPrincipal
                        {
                            Opcion = "OPERACIONES",
                            Habilitado = true,
                            bindHabilidado = "True",
                            Icon = "ic_stroller.png"
                        });
                        oMenuPrincipal.Add(new MenuPrincipal
                        {
                            Opcion = "FIN DE TURNO",
                            Habilitado = true,
                            bindHabilidado = "True",
                            Icon = "ic_route.png"
                        });
                        oMenuPrincipal.Add(new MenuPrincipal
                        {
                            Opcion = "TRANSMISION",
                            Habilitado = false,
                            bindHabilidado = "True",
                            Icon = "ic_upload.png"
                        });

                    }
                    else
                    {
                        oMenuPrincipal.Add(new MenuPrincipal
                        {
                            Opcion = "OPERACIONES",
                            Habilitado = false,
                            bindHabilidado = "False",
                            Icon = "ic_stroller.png"
                        });
                        oMenuPrincipal.Add(new MenuPrincipal
                        {
                            Opcion = "FIN DE TURNO",
                            Habilitado = false,
                            bindHabilidado = "False",
                            Icon = "ic_route.png"
                        });
                        oMenuPrincipal.Add(new MenuPrincipal
                        {
                            Opcion = "TRANSMISION",
                            Habilitado = true,
                            bindHabilidado = "True",
                            Icon = "ic_upload.png"
                        });
                    }
                }
            }
            else
            {
                oMenuPrincipal.Add(new MenuPrincipal
                {
                    Opcion = "RECEPCION",
                    Habilitado = true,
                    bindHabilidado = "True",
                    Icon = "ic_cloud_computing"
                });
                oMenuPrincipal.Add(new MenuPrincipal
                {
                    Opcion = "INICIO DE TURNO",
                    Habilitado = false,
                    bindHabilidado = "False",
                    Icon = "ic_route.png"
                });
                oMenuPrincipal.Add(new MenuPrincipal
                {
                    Opcion = "OPERACIONES",
                    Habilitado = false,
                    bindHabilidado = "False",
                    Icon = "ic_stroller.png"
                });
                oMenuPrincipal.Add(new MenuPrincipal
                {
                    Opcion = "FIN DE TURNO",
                    Habilitado = false,
                    bindHabilidado = "False",
                    Icon = "ic_route.png"
                });
                oMenuPrincipal.Add(new MenuPrincipal
                {
                    Opcion = "TRANSMISION",
                    Habilitado = false,
                    bindHabilidado = "True",
                    Icon = "ic_upload.png"
                });
            }


            oMenuPrincipal.Add(new MenuPrincipal
            {
                Opcion = "UTILERIAS",
                Habilitado = true,
                bindHabilidado = "True",
                Icon = "ic_settings.png"

            });
            if (VarEntorno.bOperaciones)
            {
                oMenuPrincipal.Add(new MenuPrincipal
                {
                    Opcion = "OPERACIONES",
                    Habilitado = true,
                    bindHabilidado = "True",
                    Icon = "ic_stroller.png"
                });
   
            }

            oMenuPrincipal.Add(new MenuPrincipal
            {
                Opcion = "SALIR",
                Habilitado = true,
                bindHabilidado = "True",
                Icon = "ic_logout.png"
            });

            

            return oMenuPrincipal;


        } // Fin del método ObtenerMenu



       

    

    } // cierre clase


}
