using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApplication1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Neurona
    {
        private decimal[] inputs;
        private decimal[,] weights;
        private decimal error;
        private decimal desiredOutput;
        private decimal output;
        private bool runFirstTime = false;
        private int time = 0;
        private int iter = 0;
        private const int maxIter = 1000;
        private const decimal learningRate = 0.2M;
        private Timer timer = new Timer(1000);
        private decimal sigmoide(decimal output) { return Convert.ToDecimal(1 / (1 + Math.Pow(Math.E, Convert.ToDouble(-output)))); }

        public Neurona(decimal[] Inputs, decimal desiredOutput)
        {
            inputs = Inputs;
            this.desiredOutput = desiredOutput;
            setWeights();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time++;
        }

        public void Train()
        {
            time = 0;
            iter = 0;
            timer.Start();

            while (Math.Round(output, 5) != Math.Round(desiredOutput, 5))
            {
                calculateOutput();
                if ((++iter % maxIter) == 0)
                    runFirstTime = false;
                setWeights();
            }
            timer.Stop();
            Console.WriteLine(string.Format("Finished result output: {0} in {1}s  with {2} iterations.", Math.Round(output, 5).ToString(), time.ToString(), iter.ToString()));
        }

        private decimal calculateOutput()
        {
            output = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                    output += sigmoide(weights[i, j] * inputs[i]);
            }
            error = desiredOutput - output;
            return output;
        }

        private void setWeights()
        {
            if (!runFirstTime)
            {
                runFirstTime = true;
                weights = new decimal[inputs.Length, inputs.Length];
                Random ran = new Random();
                for (int i = 0; i < inputs.Length; i++)
                {
                    for (int j = 0; j < inputs.Length; j++)
                        weights[i, j] = Convert.ToDecimal(ran.Next(-1, 1));
                }
            }
            else
                for (int i = 0; i < inputs.Length; i++)
                {
                    for (int j = 0; j < inputs.Length; j++)
                        weights[i, j] = weights[i, j] + (learningRate * error * inputs[i]);
                }
        }
    }
}
