using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class EncuestaVM
    {
        encuestasSR oEncuestas = new encuestasSR();

        public ObservableCollection<encuesta> oListaFormularioEncuesta { get; set; }
        public List<opciones> oListaOpciones = null;

        public EncuestaVM()
        {
            // CONSTRUCTOR
            encuestasSR oEncuesta = new encuestasSR();
            var ListaEncuetas = oEncuesta.obtenerEncuestas();
            oListaFormularioEncuesta = new ObservableCollection<encuesta>();
            for(int i=0;i< ListaEncuetas.Count; i++)
            {
                oListaFormularioEncuesta.Add(new encuesta
                {
                    enn_id = ListaEncuetas[i].enn_id,
                    enc_pregunta = i+1+". "+ListaEncuetas[i].enc_pregunta,
                    enn_tipo_respuesta = ListaEncuetas[i].enn_tipo_respuesta,
                    end_fecha_alta = ListaEncuetas[i].end_fecha_alta,
                    end_fecha_baja = ListaEncuetas[i].end_fecha_baja,
                    enb_estatus = ListaEncuetas[i].enb_estatus
                });
            }
        }

        public void OpcinesPreguntas(int enn_id)
        {
            oListaOpciones = oEncuestas.obtenerOpciones(enn_id);
        }

    }
}
