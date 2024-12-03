using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapatería_ElChele.Modelos
{
    [Serializable]
    public class Producto
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public double Cantidad { get; internal set; }
        public object IdProducto { get; internal set; }
    }
}
