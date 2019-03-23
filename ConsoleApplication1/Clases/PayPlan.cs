using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ConsoleApplication1
{
    public class PayPlan
    {
        BindingList<CashFlow> bl_cashFlow_PayPlan = new BindingList<CashFlow>();


        public PayPlan()
        {

        }

        public PayPlan(CashFlow cashFlow)
        {
            generatePayPlan(cashFlow);
        }

        private void generatePayPlan(CashFlow cashFlow)
        {
            bl_cashFlow_PayPlan.Clear();
            for (int i = 0; i < cashFlow.Cuotas; i++)
            {
                CashFlow cash = new CashFlow(cashFlow.Id, cashFlow.Tipo, cashFlow.Concepto, cashFlow.Descripcion, cashFlow.Fecha.AddMonths(i), cashFlow.Monto);
                bl_cashFlow_PayPlan.Add(cash);
            }
        }

        public BindingList<CashFlow> getBindingListPayPlan()
        {
            if (bl_cashFlow_PayPlan.Count != 0)
                return bl_cashFlow_PayPlan;
            else
                return null;
        }
    }
}
