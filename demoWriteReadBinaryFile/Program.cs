using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace demoWriteReadBinaryFile
{
    class Program
    {
        /// <summary>
        /// "Serializable", indica al objeto que se puede convertir en una secuencia de bytes 
        /// para almacenar el objeto o transmitirlo a la memoria, 
        /// a una base de datos o a un archivo.
        /// 
        /// Mas informacion en: https://docs.microsoft.com/es-es/dotnet/csharp/programming-guide/concepts/serialization/
        /// </summary>
        [Serializable]
        struct Persona
        {
            public string nombre;
            public string apellido;
            public int edad;
        }

        static void Main(string[] args)
        {
            try
            {
                String nombreDelArchivoBin = @"data.dat";

                EscribirBinario(nombreDelArchivoBin);
                LeerBinario(nombreDelArchivoBin);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar el archivo binario: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }            
        }
        /// <summary>
        /// Metodo encargado de escribir un archivo en formato binario, este archivo contendra una lista de objetos de tipo "Persona"
        /// </summary>
        /// <param name="nombreDelArchivoBin"></param>
        private static void EscribirBinario(string nombreDelArchivoBin)
        {
            try
            {
                //Se crea la referencia a un archivo nuevo, indicandole "FileMode.Create"
                FileStream fs = new FileStream(nombreDelArchivoBin, FileMode.Create);

                //Obtengo una lista de "Persona"
                List<Persona> listPersonas1 = getList();

                //Objeto encargado de almacenar un stream en memoria
                MemoryStream memoryStream = new MemoryStream();
                //Elemento encargado de serializar y deserializar un objeto, en formato binario.
                BinaryFormatter bf = new BinaryFormatter();
                //Serializa el objeto entregado y vuelca los datos en el MemoryStream.
                bf.Serialize(memoryStream, listPersonas1);


                //Escribre objetos en formato binario, para esto se le pasa un FileStream para que vuelque los datos en dicho fichero cuando sea necesario
                BinaryWriter bw = new BinaryWriter(fs);
                //Se combierte el MemoryStream en byte[] y se envia a la escritura en el objeto BinaryWriter que a su vez lo vuelca a un archivo cargado en "FileStream"
                bw.Write(memoryStream.ToArray());
                
                //Cuando se termian de ocupar los objetos, se deben cerrar, para liberar recursos y posibles fallos en la lectura y escritura del archivo binario
                bw.Close();
                fs.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Metodo encargado de leer un archivo en formato binario, el cual contiene una lista de objetos "Persona"
        /// </summary>
        /// <param name="nombreDelArchivoBin"></param>
        private static void LeerBinario(string nombreDelArchivoBin)
        {
            try
            {
                //Se crea la referencia a un archivo binario existente, indicando que se debe abrir par su lectura
                FileStream fs = new FileStream(nombreDelArchivoBin, FileMode.Open);
                //Elemento encargado de serializar y deserializar un objeto, en formato binario.
                BinaryFormatter bf = new BinaryFormatter();

                //Se obtienen los objetos de tipo "Persona" desde el archivo binario
                //Para esto se indica que BinaryFormatter deserialize el contenido del FileStream, retornando un objeto
                //Finalmente se realiza un "cast" para convertir el objeto en una lista de tipo "Persona"
                List<Persona> lP = (List<Persona>)bf.Deserialize(fs);

                //Se recorre la lista de personas obtenide del archivo binario
                foreach (Persona p in lP)
                {
                    Console.WriteLine(p.nombre + " - " + p.apellido);
                }
                //Se realiza un conteo de todos los objetos que contengan un nombre especifico (para esto se usa Linq)
                Console.WriteLine("Por busqueda: " + lP.Count(x => x.nombre == "edgardo"));

                //Cuando se termian de ocupar los objetos, se deben cerrar, para liberar recursos y posibles fallos en la lectura y escritura del archivo binario
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message,ex);
            }
        }
        /// <summary>
        /// Se genera una lista con objetos "Persona"
        /// </summary>
        /// <returns></returns>
        private static List<Persona> getList()
        {
            List<Persona> lP = new List<Persona>();
            lP.Add(new Persona() { nombre = "edgardo", apellido = "vasquez", edad = 20 });
            lP.Add(new Persona() { nombre = "roberto", apellido = "fajardo", edad = 30 });
            lP.Add(new Persona() { nombre = "ernesto", apellido = "baez", edad = 27 });
            lP.Add(new Persona() { nombre = "pedro", apellido = "toro", edad = 32 });
            lP.Add(new Persona() { nombre = "juan", apellido = "zambrano", edad = 25 });
            lP.Add(new Persona() { nombre = "diego", apellido = "valenzuela", edad = 35 });
            lP.Add(new Persona() { nombre = "edgardo", apellido = "vasquez", edad = 18 });
            return lP;
        }
    }
}
