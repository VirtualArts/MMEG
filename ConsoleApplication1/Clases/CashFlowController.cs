using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleApplication1
{
    public class CashFlowController
    {
        BindingList<CashFlow> bl_cashflow = new BindingList<CashFlow>();

        public enum Tipo
        {
            Entrada = 0,
            Salida = 1
        }

        public CashFlowController()
        {

        }


        public void addToList(CashFlow obj)
        {
            bl_cashflow.Add(obj);
        }
        public void removFromList(CashFlow obj)
        {
            if (bl_cashflow.Where(i => i == obj).Count() > 0)
                bl_cashflow.Remove(obj);
        }
        public void clearList()
        {
            bl_cashflow.Clear();
        }
        public int listCount()
        {
            return bl_cashflow.Count();
        }
        public CashFlow getObj(int i)
        {
            return bl_cashflow[i];
        }

        public BindingList<CashFlow> dataSource()
        {
            return bl_cashflow;
        }

        public void dataGridRowToControls(ref Form1 frm, int rowIndex)
        {
            foreach (System.Windows.Forms.Control item in frm.Controls)
            {
                switch (item.GetType().Name)
                {
                    case "TextBox":
                        PropertyInfo[] pi = getCashFlowByRowIndex(rowIndex).GetType().GetProperties();
                        if (pi.Where(i => i.Name == item.Tag.ToString()).Count() > 0)
                        {

                        }
                        break;
                    case "ComboBox":
                        break;
                    case "DateTimePicker":
                        break;
                    case "NumericUpDown":
                        break;
                }
            }
        }

        public CashFlow getCashFlowByRowIndex(int rowIndex)
        {
            return bl_cashflow[rowIndex];
        }
    }
}
