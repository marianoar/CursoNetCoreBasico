using CoreEscuela.Entidades;
using Etapa1.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etapa1.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc == null)
            {
                throw new ArgumentNullException(nameof(dicObsEsc));
            }
            _diccionario = dicObsEsc;
        }

        //public IEnumerable<Evaluacion> GetListaEvaluaciones()
        //{
        //    _diccionario[LlaveDiccionario.Evaluacion]
        //}
    }
}
