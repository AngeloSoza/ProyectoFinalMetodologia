using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Zapatería_ElChele.Ayudantes
{
    public static class FileManager
    {

        public static void GuardarEnArchivo<T>(string ruta, List<T> datos)
        {
            using (FileStream fs = new FileStream(ruta, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, datos);
            }
        }

        
        public static List<T> LeerDeArchivo<T>(string ruta)
        {
            if (!File.Exists(ruta))
            {
                Console.WriteLine($"Archivo no encontrado: {ruta}");
                return new List<T>();
            }

            
        using (FileStream fs = new FileStream(ruta, FileMode.Open))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (List<T>)formatter.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al leer archivo: {ex.Message}");
                    return new List<T>();
                }

            }
        }
    }
}
