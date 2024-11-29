using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCompte.Model
{
    public interface IObserver
    {
        public void update(Object? data = null);
    }
}
