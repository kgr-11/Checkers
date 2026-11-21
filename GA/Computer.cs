using Checkers;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GA
{
    public class Computer
    {
        Network computer_genetic_alg;  //найденная компьютерная сеть для игры, обученная генетическим алгоритмом
        Network computer;
        public bool IsReady;//будет готов, когда обучится
        public Computer()
        {
            IsReady = false;
        }

        //функция запуска ГА
        public void LearningNetwork()
        {
            List<int> scelet = new List<int> { 32,40,10,1};//получили размер этой сети
            //обучаем сеть генетическим алгоритмом
            int countIndivid = 100;//кол-во особей в поколении
            int countGeneration = 100;//кол-во поколений
            computer_genetic_alg = Genetic_Algorithm.GA(scelet, countIndivid, countGeneration);
            IsReady = true;
        }

        


        public void NextStepComputer(int[,] game_place, int typeElement)
        {
            
        }

        
    }

}