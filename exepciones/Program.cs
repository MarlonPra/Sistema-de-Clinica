using System;
using System.Collections.Generic;
using System.Linq;

namespace exepciones
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Paciente paciente = new Paciente();
            List<string> errores = new List<string>(); // Lista para errores
            Queue<Paciente> pacientesQueue = new Queue<Paciente>(); // Cola para pacientes
            Stack<int> revisionesStack = new Stack<int>(); // Pila para revisiones
            Dictionary<int, string> especialidadesDict = new Dictionary<int, string>
            {
                { 1, "Cardiología" },
                { 2, "Neurología" },
                { 3, "Pediatría" }
            };

            int contadorErrores = 0;

            Console.WriteLine("¡Bienvenido al sistema de registro de pacientes del hospital!");
            Console.WriteLine("Por favor, ingresa la información solicitada a continuación.\n");

            // 1. Validación del nombre del paciente
            bool nombreValido = false;
            while (!nombreValido)
            {
                try
                {
                    Console.Write("Ingresa el nombre completo del paciente (solo letras, espacios y guiones): ");
                    paciente.Nombre = Console.ReadLine();

                    // Validamos que el nombre solo contenga letras, espacios y guiones
                    if (string.IsNullOrEmpty(paciente.Nombre) || !paciente.Nombre.All(c => char.IsLetter(c) || c == ' ' || c == '-'))
                    {
                        throw new FormatException("El nombre solo puede contener letras, espacios y guiones.");
                    }

                    nombreValido = true;
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    errores.Add(ex.Message);
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
            }

            // 2. Validación de la edad del paciente
            bool edadValida = false;
            while (!edadValida)
            {
                Console.Write("Ingresa la edad del paciente (entre 1 y 120 años): ");
                if (!int.TryParse(Console.ReadLine(), out paciente.Edad) || paciente.Edad <= 0 || paciente.Edad > 120)
                {
                    contadorErrores++;
                    errores.Add("La edad debe estar entre 1 y 120 años.");
                    Console.WriteLine($"Error: La edad debe estar entre 1 y 120 años.");
                }
                else
                {
                    edadValida = true;
                }
            }

            // 3. Validación del número de seguro social
            Console.WriteLine("\nEl número de seguro social debe contener exactamente 9 dígitos numéricos y no puede ser 0.");
            bool seguroSocialValido = false;
            while (!seguroSocialValido)
            {
                try
                {
                    Console.Write("Ingresa el número de seguro social del paciente: ");
                    paciente.NumeroSeguroSocial = Console.ReadLine();

                    if (paciente.NumeroSeguroSocial.Length != 9 || !paciente.NumeroSeguroSocial.All(char.IsDigit) || paciente.NumeroSeguroSocial == "000000000")
                    {
                        throw new FormatException("El número de seguro social debe contener exactamente 9 dígitos numéricos y no puede ser 0.");
                    }
                    seguroSocialValido = true;
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    errores.Add(ex.Message);
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
            }

            // 4. Validación del diagnóstico
            bool diagnosticoValido = false;
            while (!diagnosticoValido)
            {
                try
                {
                    Console.Write("Ingresa el diagnóstico del paciente (solo letras, espacios y guiones): ");
                    paciente.Diagnostico = Console.ReadLine();

                    // Validamos que el diagnóstico solo contenga letras, espacios y guiones
                    if (string.IsNullOrEmpty(paciente.Diagnostico) || !paciente.Diagnostico.All(c => char.IsLetter(c) || c == ' ' || c == '-'))
                    {
                        throw new FormatException("El diagnóstico solo puede contener letras, espacios y guiones.");
                    }

                    diagnosticoValido = true;
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    errores.Add(ex.Message);
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
            }

            // 5. Manejo de posibles divisiones por cero en cálculos relacionados con revisiones del paciente
            bool calculoValido = false;
            while (!calculoValido)
            {
                try
                {
                    Console.Write("Ingresa el número de días que el paciente ha estado en el hospital (mayor a 0): ");
                    int diasEnHospital = Convert.ToInt32(Console.ReadLine());

                    if (diasEnHospital <= 0)
                    {
                        throw new ArgumentOutOfRangeException("Días en el hospital", "El número de días debe ser mayor a 0.");
                    }

                    Console.Write("Ingresa el número de revisiones realizadas hoy (mayor a 0): ");
                    int revisiones = Convert.ToInt32(Console.ReadLine());

                    if (revisiones <= 0)
                    {
                        throw new ArgumentOutOfRangeException("Revisiones", "El número de revisiones debe ser mayor a 0.");
                    }

                    int promedioRevisiones = diasEnHospital / revisiones;
                    revisionesStack.Push(revisiones); // Agregar revisiones a la pila
                    calculoValido = true;
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    errores.Add(ex.Message);
                    Console.WriteLine($"Error de formato: {ex.Message}. Asegúrate de ingresar un número válido.");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    contadorErrores++;
                    errores.Add(ex.Message);
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
            }

            // 6. Selección de especialidades
            Console.WriteLine("\nSelecciona la especialidad médica del paciente:");
            foreach (var especialidad in especialidadesDict)
            {
                Console.WriteLine($"{especialidad.Key}. {especialidad.Value}");
            }
            bool especialidadValida = false;
            while (!especialidadValida)
            {
                try
                {
                    Console.Write("Ingresa el número correspondiente a la especialidad (1-3): ");
                    int opcionEspecialidad = int.Parse(Console.ReadLine());

                    switch (opcionEspecialidad)
                    {
                        case 1:
                            paciente.Especialidad = "Cardiología";
                            break;
                        case 2:
                            paciente.Especialidad = "Neurología";
                            break;
                        case 3:
                            paciente.Especialidad = "Pediatría";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Opción", "La opción seleccionada no es válida.");
                    }

                    especialidadValida = true;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    contadorErrores++;
                    errores.Add(ex.Message);
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    errores.Add(ex.Message);
                    Console.WriteLine($"Error de formato: {ex.Message}. Asegúrate de ingresar un número válido.");
                }
            }

            // Agregar paciente a la cola
            pacientesQueue.Enqueue(paciente);

            // Operación completada con éxito
            Console.WriteLine("\n¡Operación completada exitosamente!");

            // Mostrando los datos completos del paciente
            Console.WriteLine("\n--- Resumen de los datos del paciente ---");
            Console.WriteLine($"Nombre: {paciente.Nombre}");
            Console.WriteLine($"Edad: {paciente.Edad}");
            Console.WriteLine($"Número de Seguro Social: {paciente.NumeroSeguroSocial}");
            Console.WriteLine($"Diagnóstico: {paciente.Diagnostico}");
            Console.WriteLine($"Especialidad: {paciente.Especialidad}");

            // Mostrando el número de errores cometidos
            Console.WriteLine($"\nNúmero de errores cometidos durante la ejecución: {contadorErrores}");

            // Imprimiendo los errores
            if (errores.Count > 0)
            {
                Console.WriteLine("\nDetalles de los errores:");
                foreach (var error in errores)
                {
                    Console.WriteLine($"- {error}");
                }
            }

            // Finalizando el programa
            Console.WriteLine("\nFinalizando el programa...");
        }
    }

    // Clase Paciente
    public class Paciente
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string NumeroSeguroSocial { get; set; }
        public string Diagnostico { get; set; }
        public string Especialidad { get; set; }
    }
}
