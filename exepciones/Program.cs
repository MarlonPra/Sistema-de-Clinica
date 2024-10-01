using System;
using System.Linq;

namespace exepciones
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Paciente paciente = new Paciente();
            int contadorErrores = 0; // Contador de errores

            Console.WriteLine("¡Bienvenido al sistema de registro de pacientes del hospital!");
            Console.WriteLine("Por favor, ingresa la información solicitada a continuación.\n");

            // 1. Validación del nombre del paciente (solo letras, espacios y guiones permitidos)
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
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
            }

            // 2. Validación de la edad del paciente
            bool edadValida = false;
            while (!edadValida)
            {
                try
                {
                    Console.Write("Ingresa la edad del paciente (entre 1 y 120 años): ");
                    paciente.Edad = Convert.ToInt32(Console.ReadLine());

                    if (paciente.Edad <= 0 || paciente.Edad > 120)
                    {
                        throw new ArgumentOutOfRangeException("Edad", "La edad debe estar entre 1 y 120 años.");
                    }
                    edadValida = true;
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    Console.WriteLine($"Error de formato: {ex.Message}. Asegúrate de ingresar un número válido.");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    contadorErrores++;
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
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
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
            }

            // 4. Validación del diagnóstico (solo letras, espacios y guiones permitidos)
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
                    calculoValido = true;
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    Console.WriteLine($"Error de formato: {ex.Message}. Asegúrate de ingresar un número válido.");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    contadorErrores++;
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
            }

            // 6. Manejo de un índice fuera de rango al seleccionar especialidades
            Console.WriteLine("\nSelecciona la especialidad médica del paciente:");
            Console.WriteLine("1. Cardiología\n2. Neurología\n3. Pediatría");
            bool especialidadValida = false;
            string[] especialidades = { "Cardiología", "Neurología", "Pediatría" };
            while (!especialidadValida)
            {
                try
                {
                    Console.Write("Ingresa el número correspondiente a la especialidad (1-3): ");
                    int opcionEspecialidad = Convert.ToInt32(Console.ReadLine());

                    if (opcionEspecialidad < 1 || opcionEspecialidad > especialidades.Length)
                    {
                        throw new IndexOutOfRangeException("La especialidad seleccionada no es válida.");
                    }

                    paciente.Especialidad = especialidades[opcionEspecialidad - 1];
                    especialidadValida = true;
                }
                catch (IndexOutOfRangeException ex)
                {
                    contadorErrores++;
                    Console.WriteLine($"Error: {ex.Message}. Inténtalo nuevamente.");
                }
                catch (FormatException ex)
                {
                    contadorErrores++;
                    Console.WriteLine($"Error de formato: {ex.Message}. Asegúrate de ingresar un número válido.");
                }
            }

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

            // Finalizando el programa
            Console.WriteLine("\nFinalizando el programa...");
        }
    }

    // Clase Paciente definida dentro del mismo archivo
    public class Paciente
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string NumeroSeguroSocial { get; set; }
        public string Diagnostico { get; set; }
        public string Especialidad { get; set; }
    }
}
