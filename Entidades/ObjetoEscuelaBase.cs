using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEscuela.Entidades
{
    public class ObjetoEscuelaBase
    {
        public string UniqueId { get; private set;}
        public string Nombre { get; set; }

        public ObjetoEscuelaBase()
        {
            this.UniqueId = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return $"{Nombre}.{UniqueId}";
        }
    }
}
