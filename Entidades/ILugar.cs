using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEscuela.Entidades
{
    public interface ILugar
    {
        string Direccion { get; set; }

        void LimpiarLugar();
    }
}
