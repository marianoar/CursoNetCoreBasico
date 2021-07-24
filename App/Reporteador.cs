using CoreEscuela.Entidades;
using Etapa1.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //public IEnumerable<Escuela> GetListaEvaluaciones()
        //{
        //    //var lista = _diccionario.GetValueOrDefault(LlaveDiccionario.Escuela);

        //    IEnumerable<Escuela> rta;

        //    if(_diccionario.TryGetValue(LlaveDiccionario.Escuela,
        //                            out IEnumerable<ObjetoEscuelaBase> lista)){
        //        rta=lista.Cast<Escuela>();
        //    }
        //    else
        //    {
        //        rta = null; // y loguear el error;
        //    }
        //    return rta;
        //}
        public IEnumerable<Evaluacion> GetListaEvaluaciones()
        {
            //var lista = _diccionario.GetValueOrDefault(LlaveDiccionario.Escuela);

            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluacion,
                                    out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluacion>();
            }
            else
            {
                return new List<Evaluacion>();
            }

        }
        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }
        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluacion> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from Evaluacion ev in listaEvaluaciones
                    select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluacion>> GetDicEvaluacionesXAsignaturas()
        {
            var diccionario = new Dictionary<string, IEnumerable<Evaluacion>>();

            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;

                diccionario.Add(asig, evalsAsig);

            }
            return diccionario;

        }

        public Dictionary<string, IEnumerable<object>> GetPromedioAlumnoXAsignatura()
        {
            var diccionario = new Dictionary<string, IEnumerable<object>>();

            var diccEvalXAsig = GetDicEvaluacionesXAsignaturas();

            foreach (var asigConEval in diccEvalXAsig)
            {
                var promedioAlumnos = from eval in asigConEval.Value
                            group eval by new
                            {
                                eval.Alumno.UniqueId,
                                eval.Alumno.Nombre
                            }
                            into grupoEvalAlumno
                            select new AlumnoPromedio
                            {
                                AlumnoId = grupoEvalAlumno.Key.UniqueId, //este es el UniqueID
                                AlumnoNombre = grupoEvalAlumno.Key.Nombre,
                                Promedio = grupoEvalAlumno.Average(evaluacion => evaluacion.Nota)
                            };
                diccionario.Add(asigConEval.Key, promedioAlumnos);
            }
            return diccionario;

        }
    }
}
