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
        Network computer;
        public bool IsReady;//будет готов, когда обучится
        public Computer()
        {
            IsReady = false;
            computer = new Network(new List<int> { 32, 40, 10, 1 });
        }

        //функция запуска ГА
        public void LearningNetwork()
        {
            List<int> scelet = new List<int> { 32,40,10,1};//получили размер этой сети
            //обучаем сеть генетическим алгоритмом
            int countIndivid = 35;//кол-во особей в поколении
            int countGeneration = 100;//кол-во поколений
            computer = Genetic_Algorithm.GA(scelet, countIndivid, countGeneration);
            //computer= new Network(scelet);//очень хуевая сеть, сейчас главное, чтобы она играла
            IsReady = true;
        }

        public void NextStepComputer(Service service)
        {
            // 1. получаем все возможные ходы
            var possibleStep = service.All_Possible_Move();//шашки, которыми сейчас можно сходить
            
            // 2. выбираем лучший ход из возможных
            //key - пара: key- индекс шашки которой можно сходить, value - индекс хода из массива ходов этой шашкой
            //value - оценка этого хода
            Dictionary<KeyValuePair<Node,int>, double> Scores = new Dictionary<KeyValuePair<Node, int>, double>();//оценки каждого хода из possibleStep
                                                                                                                 //перебор возможных ходов
            foreach(var Checker in possibleStep)
            {
                for (int j = 0; j < Checker.Value.Count; j++)
                {
                    var path=Checker.Value[j];
                    //создаем копию доски - изменяя ее, никак не изменим исходную доску
                    var copy_game_place = Service.CopyGamePlace(service.game_place);
                    //делаем ход на копированной доске
                    Service.MakeMove(copy_game_place, path, service.current_step);
                    //преобразуем доску в список 
                    var cond = Genetic_Algorithm.Condition(copy_game_place);
                    //делаем прямой проход
                    var sc = computer.Forward(cond);//получаем оценку этого хода
                                                       //запоминаем оценку
                    Scores[new KeyValuePair<Node, int>(Checker.Key,j)] = sc;
                }
            }
            //находим лучший ход
            var minKey = Scores
                         .OrderBy(pair => pair.Value)      // Сортируем по значению
                         .Select(pair => pair.Key)         // Берем только ключи
                         .FirstOrDefault();                // Берем первый (самый минимальный)
                                                           //на этом этапе знаем пару индексов = какой шашкой ходить и какой именно ход ей делать
                                                           // 3. делаем ход
                                                           //делаем окончательный ход на игровой доске
            service.MakeMove(possibleStep[minKey.Key][minKey.Value]);
            // 4. конце смена хода
            service.ChangePace();
        }
    }

}