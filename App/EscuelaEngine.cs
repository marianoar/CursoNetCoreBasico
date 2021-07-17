using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using Etapa1.Entidades;

namespace CoreEscuela
{
    public class EscuelaEngine
    {
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academay", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();

        }

        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> GetDiccionarioObjetos()
        {
           

            var diccionario = new Dictionary<LlaveDiccionario, IEnumerable< ObjetoEscuelaBase>>();

            diccionario.Add(LlaveDiccionario.Escuela, new[] { Escuela });

            diccionario.Add(LlaveDiccionario.Curso, Escuela.Cursos.Cast<ObjetoEscuelaBase>());

            var listaTemporal = new List<Evaluacion>();
            var listaTemporalAsignaturas = new List<Asignatura>();
            var listaTemporalAlumnos = new List<Alumno>();

            foreach (var cur in Escuela.Cursos)
            {
                listaTemporalAlumnos.AddRange(cur.Alumnos);
                listaTemporalAsignaturas.AddRange(cur.Asignaturas);



                foreach (var alumno in cur.Alumnos)
                {
                    listaTemporal.AddRange(alumno.Evaluaciones);
                }
            }
            diccionario.Add(LlaveDiccionario.Alumno, listaTemporalAlumnos.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Asignatura, listaTemporalAsignaturas.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Evaluacion, listaTemporal.Cast<ObjetoEscuelaBase>());

            return diccionario;
        }

        public void ImprimirDicc(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicc,
            bool printEval=false)
        {
            foreach (var obj in dicc)
            {
                //Console.WriteLine(obj.Value);
                foreach (var val in obj.Value)
                {
                    switch (obj.Key)
                    {
                        case LlaveDiccionario.Asignatura:
                            Console.WriteLine("Asignatura: " + val.Nombre);
                            break;
                        case LlaveDiccionario.Evaluacion:
                            if (printEval)
                            {
                                Console.WriteLine(val);
                            }
                            break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine("Escuela: "+val);
                            break;
                        case LlaveDiccionario.Alumno:
                            Console.WriteLine("Alumno: "+val.Nombre);
                            break;
                        case LlaveDiccionario.Curso:
                            Console.WriteLine("Curso: " + val.Nombre+"Cantidad: "+((Curso)val).Alumnos.Count());
                            break;
                        default:
                            Console.WriteLine(val);
                            break;
                    }
                }
            }
        }
        #region Metodos carga
        private void CargarEvaluaciones()
        {
            var lista = new List<Evaluacion>();

            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        // var rnd = new Random(System.Environment.TickCount); miliseg desde que se encendio la mqauina
                        var rnd = new Random();
                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluacion
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#1{i + 1}",
                                Nota = (float)Math.Round(5 * rnd.NextDouble(),1),
                                Alumno = alumno
                            };
                            alumno.Evaluaciones.Add(ev);
                        }

                    }
                }
            }
        }

        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }

        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }


        #endregion

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(

          bool traeEvaluaciones = true, // asi son Parametros opcionales
          bool traeAlumnos = true,
          bool traeAsignaturas = true,
          bool traeCursos = true
         )
        {
            return GetObjetosEscuela(out int dummy, out dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            bool traeEvaluaciones = true, // asi son Parametros opcionales
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true
            )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out int dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            bool traeEvaluaciones = true, // asi son Parametros opcionales
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true
            )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoCursos, out int dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAsignaturas,
            bool traeEvaluaciones = true, // asi son Parametros opcionales
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true
            )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoCursos, out conteoAsignaturas, out int dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoAl,
            out int conteoAs,
            out int conteoCur,
            bool traeEvaluaciones = true, // asi son Parametros opcionales
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true
           )
        {
            conteoEvaluaciones = 0;
            conteoAs = 0;
            conteoAl = 0;
            conteoCur = 0;
            var listaObj = new List<ObjetoEscuelaBase>();

            listaObj.Add(Escuela);

            if (traeCursos)
            {
                listaObj.AddRange(Escuela.Cursos);
                conteoCur = Escuela.Cursos.Count();
            }

            foreach (var curso in Escuela.Cursos)
            {
                if (traeAsignaturas)
                {
                    listaObj.AddRange(curso.Asignaturas);
                    conteoAs += curso.Asignaturas.Count();
                }

                if (traeAlumnos)
                {
                    listaObj.AddRange(curso.Alumnos);
                    conteoAl += curso.Alumnos.Count();
                }
                if (traeEvaluaciones)
                {

                    foreach (var alumno in curso.Alumnos)
                    {
                        listaObj.AddRange(alumno.Evaluaciones);
                        conteoEvaluaciones += alumno.Evaluaciones.Count();
                    }
                }
            }
            return listaObj.AsReadOnly();
        }
    }
}