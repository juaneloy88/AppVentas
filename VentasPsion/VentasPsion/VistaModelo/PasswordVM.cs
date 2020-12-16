using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    static class PasswordVM
    {

        public static async Task< bool> PassAjustes(string sClave)
        {
            passwordSR cPass = new passwordSR();

            return await cPass.PassAjustes(sClave);

        }
    }
}
