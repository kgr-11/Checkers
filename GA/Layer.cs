using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GA
{
    public class Layer
    {
        Neuron[] neurons;//нейроны слоя 
        public double[] phi;//пороги этих нейронов
        public double[] delta;//дельта для каждого нейрона слоя
        public double[,] W;// размеры: кол-во нейронов на текущем слое*кол-во нейронов на прошлом слое
        public Layer(int N_curr, int N_prev)
        {
            //заполняем слой нейронами
            neurons = new Neuron[N_curr];
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = new Neuron();

            //создание матрицы весов + её инициализация 
            W = new double[N_curr, N_prev];
            for (int i = 0; i < W.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < W.GetUpperBound(1) + 1; j++)
                {
                    W[i, j] = (Random.Shared.NextDouble() * 2 - 1) / 10;    // [-0.2,0.2]
                }
            }
            //генерация порогов для нейронов
            phi = new double[N_curr];
            for (int i = 0; i < phi.Length; i++)
                phi[i] = (Random.Shared.NextDouble() * 2 - 1) / 10;     // [-0.2,0.2]

            delta = new double[N_curr];
        }
        public Layer(int N_curr, int N_prev, double[,] W, double[] phi)
        {
            //заполняем слой нейронами
            neurons = new Neuron[N_curr];
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = new Neuron();
            this.W = W;
            this.phi = phi;
        }
        public List<double> ActivateLayer(List<double> InputData)
        {
            //активация нейронов слоя 
            List<double> outputData = new List<double>();
            for (int i = 0; i < W.GetUpperBound(0) + 1; i++)
            {
                double S = 0;
                for (int j = 0; j < InputData.Count(); j++)
                {
                    S += W[i, j] * InputData[j];
                }
                neurons[i].Activate(S - phi[i]);//активация нейрона
                outputData.Add(neurons[i].Y);
            }
            return outputData;
        }
    }
}