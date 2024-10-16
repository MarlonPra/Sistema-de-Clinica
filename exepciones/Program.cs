using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace SistemaClinicaAvanzado
{
    class Programa
    {
        static void Main(string[] args)
        {
            var clinica = new Clinica();
            bool salir = false;

            while (!salir)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Sistema de Gestión de Pacientes - Clínica Médica Avanzada ===");
                    Console.WriteLine("1. Registrar nuevo paciente");
                    Console.WriteLine("2. Ver todos los pacientes");
                    Console.WriteLine("3. Ver pacientes por especialidad");
                    Console.WriteLine("4. Buscar paciente por NSS");
                    Console.WriteLine("5. Ver estadísticas");
                    Console.WriteLine("6. Actualizar información de paciente");
                    Console.WriteLine("7. Registrar visita médica");
                    Console.WriteLine("8. Ver historial de visitas de un paciente");
                    Console.WriteLine("9. Formulario para pacientes con depresión");
                    Console.WriteLine("10. Salir");
                    Console.WriteLine("Elija una opción:");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            clinica.RegistrarPaciente();
                            break;
                        case "2":
                            clinica.VerTodosPacientes();
                            break;
                        case "3":
                            clinica.VerPacientesPorEspecialidad();
                            break;
                        case "4":
                            clinica.BuscarPacientePorNSS();
                            break;
                        case "5":
                            clinica.VerEstadisticas();
                            break;
                        case "6":
                            clinica.ActualizarInformacionPaciente();
                            break;
                        case "7":
                            clinica.RegistrarVisitaMedica();
                            break;
                        case "8":
                            clinica.VerHistorialVisitas();
                            break;
                        case "9":
                            clinica.FormularioDepresion();
                            break;
                        case "10":
                            Console.WriteLine("Saliendo...");
                            Thread.Sleep(1000);
                            salir = true;
                            break;
                        default:
                            throw new ArgumentException("Opción inválida. Por favor, ingrese un número del 1 al 9.");
                    }

                    if (!salir)
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Gracias por utilizar el Sistema de Gestión de Pacientes de la Clínica Médica Avanzada.");
        }
    }

    public class Clinica
    {
        private List<Paciente> pacientes = new List<Paciente>();
        private Queue<Paciente> listaEspera = new Queue<Paciente>();
        private Stack<string> accionesRecientes = new Stack<string>();
        private Dictionary<Especialidad, List<Paciente>> pacientesPorEspecialidad = new Dictionary<Especialidad, List<Paciente>>();

        public Clinica()
        {
            foreach (Especialidad esp in Enum.GetValues(typeof(Especialidad)))
            {
                pacientesPorEspecialidad[esp] = new List<Paciente>();
            }
        }

        public void RegistrarPaciente()
        {
            Console.Clear();
            Console.WriteLine("=== Registro de Nuevo Paciente ===\n");

            var paciente = new Paciente
            {
                Nombre = ObtenerEntradaValida("Nombre completo: ", ValidarNombre),
                FechaNacimiento = ObtenerEntradaValida("Fecha de nacimiento (DD/MM/AAAA): ", ValidarFechaNacimiento),
                NumeroSeguroSocial = ObtenerEntradaValida("Número de Seguro Social (9 dígitos): ", ValidarNSS),
                Genero = ObtenerEntradaValida("Género (M/F/O): ", ValidarGenero),
                GrupoSanguineo = ObtenerEntradaValida("Grupo sanguíneo (A+, A-, B+, B-, AB+, AB-, O+, O-): ", ValidarGrupoSanguineo),
                Altura = ObtenerEntradaValida("Altura en cm: ", ValidarAltura),
                Peso = ObtenerEntradaValida("Peso en kg: ", ValidarPeso),
                Diagnostico = ObtenerEntradaValida("Diagnóstico preliminar: ", ValidarDiagnostico),
                Especialidad = ObtenerEntradaValida("Especialidad (1. Cardiología, 2. Neurología, 3. Pediatría): ", ValidarEspecialidad)
            };

            pacientes.Add(paciente);
            listaEspera.Enqueue(paciente);
            pacientesPorEspecialidad[paciente.Especialidad].Add(paciente);
            accionesRecientes.Push($"Paciente registrado: {paciente.Nombre} - {DateTime.Now}");

            Console.WriteLine("\n¡Paciente registrado exitosamente!");
        }

        public void VerTodosPacientes()
        {
            Console.Clear();
            Console.WriteLine("=== Lista de Todos los Pacientes ===\n");

            if (pacientes.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados en el sistema.");
                return;
            }

            foreach (var paciente in pacientes)
            {
                Console.WriteLine(paciente);
                Console.WriteLine(new string('-', 70));
            }
        }

        public void VerPacientesPorEspecialidad()
        {
            Console.Clear();
            Console.WriteLine("=== Pacientes por Especialidad ===\n");

            Especialidad especialidad = ObtenerEntradaValida(
                "Seleccione la especialidad (1. Cardiología, 2. Neurología, 3. Pediatría): ",
                ValidarEspecialidad);

            var pacientesEspecialidad = pacientesPorEspecialidad[especialidad];

            if (pacientesEspecialidad.Count == 0)
            {
                Console.WriteLine($"No hay pacientes registrados en {especialidad}.");
                return;
            }

            Console.WriteLine($"\nPacientes en {especialidad}:");
            foreach (var paciente in pacientesEspecialidad)
            {
                Console.WriteLine(paciente);
                Console.WriteLine(new string('-', 70));
            }
        }

        public void BuscarPacientePorNSS()
        {
            Console.Clear();
            Console.WriteLine("=== Búsqueda de Paciente por NSS ===\n");

            string nss = ObtenerEntradaValida("Ingrese el Número de Seguro Social (9 dígitos): ", ValidarNSS);

            var paciente = pacientes.FirstOrDefault(p => p.NumeroSeguroSocial == nss);

            if (paciente != null)
            {
                Console.WriteLine("\nPaciente encontrado:");
                Console.WriteLine(paciente);
            }
            else
            {
                Console.WriteLine("\nNo se encontró ningún paciente con ese Número de Seguro Social.");
            }
        }

        public void VerEstadisticas()
        {
            Console.Clear();
            Console.WriteLine("=== Estadísticas de la Clínica ===\n");

            Console.WriteLine($"Total de pacientes: {pacientes.Count}");
            Console.WriteLine($"Pacientes en lista de espera: {listaEspera.Count}");

            foreach (Especialidad esp in Enum.GetValues(typeof(Especialidad)))
            {
                Console.WriteLine($"Pacientes en {esp}: {pacientesPorEspecialidad[esp].Count}");
            }

            if (pacientes.Any())
            {
                Console.WriteLine($"\nEdad promedio de pacientes: {pacientes.Average(p => p.Edad):F2} años");
                Console.WriteLine($"IMC promedio de pacientes: {pacientes.Average(p => p.IMC):F2}");
            }

            Console.WriteLine("\nÚltimas 5 acciones realizadas:");
            foreach (var accion in accionesRecientes.Take(5))
            {
                Console.WriteLine(accion);
            }
        }

        public void ActualizarInformacionPaciente()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Información de Paciente ===\n");

            string nss = ObtenerEntradaValida("Ingrese el Número de Seguro Social del paciente a actualizar: ", ValidarNSS);
            var paciente = pacientes.FirstOrDefault(p => p.NumeroSeguroSocial == nss);

            if (paciente == null)
            {
                Console.WriteLine("No se encontró ningún paciente con ese Número de Seguro Social.");
                return;
            }

            Console.WriteLine("Ingrese la nueva información (deje en blanco para mantener el valor actual):");

            string nuevoNombre = ObtenerEntradaValidaOpcional("Nuevo nombre: ", ValidarNombre, paciente.Nombre);
            if (!string.IsNullOrWhiteSpace(nuevoNombre)) paciente.Nombre = nuevoNombre;

            DateTime? nuevaFechaNacimiento = ObtenerEntradaValidaOpcional("Nueva fecha de nacimiento (DD/MM/AAAA): ", ValidarFechaNacimiento, paciente.FechaNacimiento);
            if (nuevaFechaNacimiento.HasValue) paciente.FechaNacimiento = nuevaFechaNacimiento.Value;

            string nuevoGenero = ObtenerEntradaValidaOpcional("Nuevo género (M/F/O): ", ValidarGenero, paciente.Genero);
            if (!string.IsNullOrWhiteSpace(nuevoGenero)) paciente.Genero = nuevoGenero;

            string nuevoGrupoSanguineo = ObtenerEntradaValidaOpcional("Nuevo grupo sanguíneo: ", ValidarGrupoSanguineo, paciente.GrupoSanguineo);
            if (!string.IsNullOrWhiteSpace(nuevoGrupoSanguineo)) paciente.GrupoSanguineo = nuevoGrupoSanguineo;

            int? nuevaAltura = ObtenerEntradaValidaOpcional("Nueva altura en cm: ", ValidarAltura, paciente.Altura);
            if (nuevaAltura.HasValue) paciente.Altura = nuevaAltura.Value;

            double? nuevoPeso = ObtenerEntradaValidaOpcional("Nuevo peso en kg: ", ValidarPeso, paciente.Peso);
            if (nuevoPeso.HasValue) paciente.Peso = nuevoPeso.Value;

            string nuevoDiagnostico = ObtenerEntradaValidaOpcional("Nuevo diagnóstico: ", ValidarDiagnostico, paciente.Diagnostico);
            if (!string.IsNullOrWhiteSpace(nuevoDiagnostico)) paciente.Diagnostico = nuevoDiagnostico;

            Especialidad? nuevaEspecialidad = ObtenerEntradaValidaOpcional("Nueva especialidad (1. Cardiología, 2. Neurología, 3. Pediatría): ", ValidarEspecialidad, paciente.Especialidad);
            if (nuevaEspecialidad.HasValue)
            {
                pacientesPorEspecialidad[paciente.Especialidad].Remove(paciente);
                paciente.Especialidad = nuevaEspecialidad.Value;
                pacientesPorEspecialidad[paciente.Especialidad].Add(paciente);
            }

            accionesRecientes.Push($"Información actualizada para el paciente: {paciente.Nombre} - {DateTime.Now}");
            Console.WriteLine("\n¡Información del paciente actualizada exitosamente!");
        }

        public void RegistrarVisitaMedica()
        {
            Console.Clear();
            Console.WriteLine("=== Registrar Visita Médica ===\n");

            string nss = ObtenerEntradaValida("Ingrese el Número de Seguro Social del paciente: ", ValidarNSS);
            var paciente = pacientes.FirstOrDefault(p => p.NumeroSeguroSocial == nss);

            if (paciente == null)
            {
                Console.WriteLine("No se encontró ningún paciente con ese Número de Seguro Social.");
                return;
            }

            var visita = new VisitaMedica
            {
                Fecha = DateTime.Now,
                Motivo = ObtenerEntradaValida("Motivo de la visita: ", ValidarTexto),
                Diagnostico = ObtenerEntradaValida("Diagnóstico: ", ValidarTexto),
                Tratamiento = ObtenerEntradaValida("Tratamiento: ", ValidarTexto)
            };

            paciente.HistorialVisitas.Add(visita);
            accionesRecientes.Push($"Visita médica registrada para el paciente: {paciente.Nombre} - {DateTime.Now}");
            Console.WriteLine("\n¡Visita médica registrada exitosamente!");
        }

        public void VerHistorialVisitas()
        {
            Console.Clear();
            Console.WriteLine("=== Historial de Visitas Médicas ===\n");

            string nss = ObtenerEntradaValida("Ingrese el Número de Seguro Social del paciente: ", ValidarNSS);
            var paciente = pacientes.FirstOrDefault(p => p.NumeroSeguroSocial == nss);

            if (paciente == null)
            {
                Console.WriteLine("No se encontró ningún paciente con ese Número de Seguro Social.");
                return;
            }

            if (paciente.HistorialVisitas.Count == 0)
            {
                Console.WriteLine($"El paciente {paciente.Nombre} no tiene visitas médicas registradas.");
                return;
            }

            Console.WriteLine($"Historial de visitas médicas para {paciente.Nombre}:");
            foreach (var visita in paciente.HistorialVisitas)
            {
                Console.WriteLine(visita);
                Console.WriteLine(new string('-', 70));
            }
        }
        public void FormularioDepresion()
        {
            Console.WriteLine("Formulario para pacientes con síntomas de depresión");

            Console.WriteLine("¿Con qué frecuencia te sientes triste o decaído? (1: Nunca, 2: A veces, 3: Frecuentemente, 4: Siempre)");
            int respuesta1 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("¿Tienes problemas para disfrutar las cosas que antes te hacían feliz? (1: Nunca, 2: A veces, 3: Frecuentemente, 4: Siempre)");
            int respuesta2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("¿Te sientes fatigado o sin energía a menudo? (1: Nunca, 2: A veces, 3: Frecuentemente, 4: Siempre)");
            int respuesta3 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nRecomendaciones:");

            if (respuesta1 >= 3 && respuesta2 >= 3 && respuesta3 >= 3)
            {
                Console.WriteLine("- Parece que estás pasando por un momento muy difícil emocionalmente.");
                Console.WriteLine("- Te recomendamos hablar con un profesional de la salud mental lo antes posible.");
                Console.WriteLine("- Trata de buscar apoyo en personas de confianza, ya sea amigos o familiares.");
                Console.WriteLine("- Considera dedicar tiempo a actividades que te ayuden a desconectarte, como leer o escuchar música.");
            }
            else if ((respuesta1 >= 3 && respuesta2 >= 3) || (respuesta1 >= 3 && respuesta3 >= 3) || (respuesta2 >= 3 && respuesta3 >= 3))
            {
                Console.WriteLine("- Es posible que estés enfrentando algunos desafíos emocionales importantes.");
                Console.WriteLine("- Aunque no parezca grave, hablar con un terapeuta puede ayudarte a gestionar tus emociones.");
                Console.WriteLine("- Realiza actividades que te relajen, como practicar yoga, hacer ejercicio moderado o escribir tus pensamientos.");
                Console.WriteLine("- Mantén una rutina que te permita descansar y desconectarte de tus preocupaciones diarias.");
            }
            else if (respuesta1 >= 3 || respuesta2 >= 3 || respuesta3 >= 3)
            {
                Console.WriteLine("- Parece que hay algunos signos de que podrías estar lidiando con emociones difíciles.");
                Console.WriteLine("- Te sugerimos que busques algún apoyo, ya sea un terapeuta o consejero.");
                Console.WriteLine("- Intenta implementar hábitos saludables, como mantener una buena alimentación y descansar adecuadamente.");
                Console.WriteLine("- Hablar con un amigo o familiar también puede ser útil para compartir tus sentimientos.");
            }
            else
            {
                Console.WriteLine("- Parece que los síntomas no son tan graves, pero aún es importante cuidar de tu bienestar emocional.");
                Console.WriteLine("- Prueba realizar actividades que te relajen y te hagan sentir mejor, como salir a caminar o practicar un hobby que disfrutes.");
                Console.WriteLine("- Recuerda que siempre es válido buscar apoyo si sientes que las emociones te abruman.");
                Console.WriteLine("- Estar en contacto con tus seres queridos puede ayudarte a mantener una buena salud emocional.");
            }

            Console.WriteLine("- Recuerda que hablar con un profesional siempre es una opción.");
        }

        private T ObtenerEntradaValida<T>(string prompt, Func<string, (bool, string, T)> validador)
        {
            while (true)
            {
                Console.Write(prompt);
                string entrada = Console.ReadLine();
                var (esValido, mensajeError,

 valor) = validador(entrada);

                if (esValido)
                {
                    return valor;
                }

                Console.WriteLine($"Error: {mensajeError}");
            }
        }

        private T? ObtenerEntradaValidaOpcional<T>(string prompt, Func<string, (bool, string, T)> validador, T valorActual) where T : struct
        {
            Console.Write(prompt);
            string entrada = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(entrada))
            {
                return null;
            }
            var (esValido, mensajeError, valor) = validador(entrada);
            if (esValido)
            {
                return valor;
            }
            Console.WriteLine($"Error: {mensajeError}. Se mantendrá el valor actual.");
            return null;
        }

        private string ObtenerEntradaValidaOpcional(string prompt, Func<string, (bool, string, string)> validador, string valorActual)
        {
            Console.Write(prompt);
            string entrada = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(entrada))
            {
                return valorActual;
            }
            var (esValido, mensajeError, valor) = validador(entrada);
            if (esValido)
            {
                return valor;
            }
            Console.WriteLine($"Error: {mensajeError}. Se mantendrá el valor actual.");
            return valorActual;
        }

        private (bool, string, string) ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return (false, "El nombre no puede estar vacío.", null);
            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return (false, "El nombre solo puede contener letras y espacios.", null);
            return (true, "", nombre.Trim());
        }

        private (bool, string, DateTime) ValidarFechaNacimiento(string fecha)
        {
            if (DateTime.TryParseExact(fecha, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaNacimiento))
            {
                if (fechaNacimiento > DateTime.Now)
                    return (false, "La fecha de nacimiento no puede ser en el futuro.", DateTime.MinValue);
                if (fechaNacimiento < DateTime.Now.AddYears(-120))
                    return (false, "La fecha de nacimiento no puede ser hace más de 120 años.", DateTime.MinValue);
                return (true, "", fechaNacimiento);
            }
            return (false, "Formato de fecha inválido. Use DD/MM/AAAA.", DateTime.MinValue);
        }

        private (bool, string, string) ValidarNSS(string nss)
        {
            if (!Regex.IsMatch(nss, @"^\d{9}$"))
                return (false, "El NSS debe contener exactamente 9 dígitos numéricos.", null);
            if (nss == "000000000")
                return (false, "El NSS no puede ser 000000000.", null);
            return (true, "", nss);
        }

        private (bool, string, string) ValidarGenero(string genero)
        {
            if (string.IsNullOrWhiteSpace(genero) || !new[] { "M", "F", "O" }.Contains(genero.ToUpper()))
                return (false, "El género debe ser M, F u O.", null);
            return (true, "", genero.ToUpper());
        }

        private (bool, string, string) ValidarGrupoSanguineo(string grupo)
        {
            if (string.IsNullOrWhiteSpace(grupo) || !new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }.Contains(grupo.ToUpper()))
                return (false, "Grupo sanguíneo inválido. Debe ser A+, A-, B+, B-, AB+, AB-, O+ u O-.", null);
            return (true, "", grupo.ToUpper());
        }

        private (bool, string, int) ValidarAltura(string altura)
        {
            if (int.TryParse(altura, out int alturaNum) && alturaNum > 0 && alturaNum < 300)
                return (true, "", alturaNum);
            return (false, "La altura debe ser un número entero entre 1 y 300 cm.", 0);
        }

        private (bool, string, double) ValidarPeso(string peso)
        {
            if (double.TryParse(peso, out double pesoNum) && pesoNum > 0 && pesoNum < 500)
                return (true, "", pesoNum);
            return (false, "El peso debe ser un número entre 0 y 500 kg.", 0);
        }

        private (bool, string, string) ValidarDiagnostico(string diagnostico)
        {
            if (string.IsNullOrWhiteSpace(diagnostico))
                return (false, "El diagnóstico no puede estar vacío.", null);
            return (true, "", diagnostico.Trim());
        }

        private (bool, string, Especialidad) ValidarEspecialidad(string entrada)
        {
            if (int.TryParse(entrada, out int opcion) && Enum.IsDefined(typeof(Especialidad), opcion))
            {
                return (true, "", (Especialidad)opcion);
            }
            return (false, "Seleccione una opción válida (1, 2 o 3).", Especialidad.Cardiologia);
        }

        private (bool, string, string) ValidarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return (false, "El texto no puede estar vacío.", null);
            return (true, "", texto.Trim());
        }
    }

    public class Paciente
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string NumeroSeguroSocial { get; set; }
        public string Genero { get; set; }
        public string GrupoSanguineo { get; set; }
        public int Altura { get; set; }
        public double Peso { get; set; }
        public string Diagnostico { get; set; }
        public Especialidad Especialidad { get; set; }
        public List<VisitaMedica> HistorialVisitas { get; set; } = new List<VisitaMedica>();

        public int Edad => CalcularEdad();
        public double IMC => Math.Round(Peso / Math.Pow(Altura / 100.0, 2), 2);

        private int CalcularEdad()
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - FechaNacimiento.Year;
            if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }

        public string EdadConDias
        {
            get
            {
                var hoy = DateTime.Today;
                var edad = CalcularEdad();
                var diasAdicionales = (hoy - FechaNacimiento.AddYears(edad)).Days;
                return $"{edad} años y {diasAdicionales} días";
            }
        }

        public override string ToString() =>
            $"Nombre: {Nombre}\n" +
            $"Edad: {EdadConDias}\n" +
            $"NSS: {NumeroSeguroSocial}\n" +
            $"Género: {Genero}\n" +
            $"Grupo Sanguíneo: {GrupoSanguineo}\n" +
            $"Altura: {Altura} cm\n" +
            $"Peso: {Peso} kg\n" +
            $"IMC: {IMC}\n" +
            $"Diagnóstico: {Diagnostico}\n" +
            $"Especialidad: {Especialidad}";
    }

    public class VisitaMedica
    {
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }

        public override string ToString() =>
            $"Fecha: {Fecha}\n" +
            $"Motivo: {Motivo}\n" +
            $"Diagnóstico: {Diagnostico}\n" +
            $"Tratamiento: {Tratamiento}";
    }

    public enum Especialidad
    {
        Cardiologia = 1,
        Neurologia = 2,
        Pediatria = 3
    }
}