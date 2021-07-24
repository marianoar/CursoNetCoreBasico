using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using Etapa1.App;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");
            Printer.Beep(10000, cantidad:1);
            // ImpimirCursosEscuela(engine.Escuela);

            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            var evalLista= reporteador.GetListaEvaluaciones();
            var asigLista = reporteador.GetListaAsignaturas();

            var listaEvalXAsig = reporteador.GetDicEvaluacionesXAsignaturas();

            var listaPromedioXAsig = reporteador.GetPromedioAlumnoXAsignatura();

            Printer.WriteTitle("Diccionario");


            var dictmp = engine.GetDiccionarioObjetos();
           //  engine.ImprimirDicc(dictmp, true);
        }

        private static void ImpimirCursosEscuela(Escuela escuela)
        {
            
            Printer.WriteTitle("Cursos de la Escuela");
            
            
            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre  }, Id  {curso.UniqueId}");
                }
            }
        }
    }
}
