using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class CashFlow
    {
        private int id;
        private int cuotas;
        private string tipo;
        private string concepto;
        private string descripcion;
        private DateTime fecha;
        private decimal monto;

        public int Id { get { return id; } set { id = value; } }
        public string Tipo { get { return tipo; } set { tipo = value; } }
        public string Concepto { get { return concepto; } set { concepto = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value; } }
        public DateTime Fecha { get { return fecha; } set { fecha = value; } }
        public decimal Monto { get { return monto; } set { monto = value; } }
        public int Cuotas { get { return cuotas; } set { cuotas = value; } }



        public CashFlow()
        {
            Cuotas = 1;
        }

        public CashFlow(int _id, string _tipo, string _concepto, string _descripcion, DateTime _fecha, decimal _monto, int cuotas = 1)
        {
            Id = _id;
            Tipo = _tipo;
            Concepto = _concepto;
            Descripcion = _descripcion;
            Fecha = _fecha;
            Monto = _monto;
            Cuotas = cuotas;
        }


    }
}
