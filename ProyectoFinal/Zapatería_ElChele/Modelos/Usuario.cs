using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapatería_ElChele.Modelos
{

    [Serializable]

    public class Usuario
    {
        public string UsuarioID { get; set; }
        public string Contraseña { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; } // Administrador, Almacenista, Ventas
        public bool Activo { get; set; }

    }

}

