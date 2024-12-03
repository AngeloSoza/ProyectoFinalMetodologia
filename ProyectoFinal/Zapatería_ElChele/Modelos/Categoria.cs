using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapatería_ElChele.Modelos
{

    [Serializable]
    internal class Categoria
    {
        public int id { get; set;  }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }




    }
}
