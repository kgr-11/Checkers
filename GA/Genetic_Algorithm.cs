using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.GA
{
    public static class Genetic_Algorithm
    {
        public static Network GA(List<int> scelet, int countIndivid, int countGeneration)
        {
            Random random = new Random();

            //генерация начальной популяции
            List<KeyValuePair<Network, int>> Population = new List<KeyValuePair<Network, int>>();
            for (int i = 0; i < countIndivid; i++)
            {
                Population.Add(new KeyValuePair<Network, int>(new Network(scelet), 0));//генерация особей популяции c нулевым рейтингом
            }

            //контейнер для элитных особей
            List<KeyValuePair<Network, int>> elites = new List<KeyValuePair<Network, int>>();
            
            //идем по поколениям
            for (int generation = 0; generation <= countGeneration; generation++)
            {
                // каждое поколение к популяции добавляются 15 новых сетей
                for (int _ = 0; _ < 15; _++)
                {
                    Population.Add(new KeyValuePair<Network, int>(new Network(scelet), 0));
                }

                //турнирный отбор - селекция
                for(int i=0;i<Population.Count;i++)
                {
                    Network net = Population[i].Key;
                    //сеть играет 5 игр подряд со случайными противниками одним цветом
                    HashSet<Network> opponents=new HashSet<Network>();
                    for (int _ = 0; _ < 5; _++)
                    { 
                        int index_opp=random.Next(0, Population.Count);
                        if (opponents.Contains(Population[index_opp].Key) || Population[index_opp].Key == net)
                        {
                            _--;
                            continue;
                        }
                        Network opponent = Population[index_opp].Key;
                        //игра
                        var result = Competition(net,opponent);
                        if (result == net)  //  победила первая сеть
                        {
                            Population[i] = new KeyValuePair<Network, int>(net, Population[i].Value + 1);
                            Population[index_opp] = new KeyValuePair<Network, int>(opponent, Population[index_opp].Value - 2);
                        }
                        else if (result == opponent)  // победила вторая сеть
                        {
                            Population[i] = new KeyValuePair<Network, int>(net, Population[i].Value -2);
                            Population[index_opp] = new KeyValuePair<Network, int>(opponent, Population[index_opp].Value +1);
                        }

                        //запоминаем с кем играли
                        opponents.Add(opponent);
                    }
                }

                //применение элитизма
                //  сортируем популяцию по рейтингу и 15 особей с наибольшим рейтингом добавляем в контейнер
                elites = Population.OrderByDescending(x => x.Value).Take(15).ToList();
                if (generation == countGeneration)
                    break;
                //скрещивание
                // создаем особей после мутации - сейчас сохраняем их в виде набора хромосом
                List<List<double>> newPopulation = new List<List<double>>();
                double chance = 1; //скрещиваются все между собой, никто не проталкивается дальше, только элита
                for (int k = 0; k < Population.Count(); k += 2)
                {
                    //два рандомных родителя, т.к. селекция была рандомной
                    int index1 = k;
                    int index2 = k + 1;
                    //их наборы хромосом
                    var coll1 = Population[index1].Key.chromosomes;
                    var coll2 = Population[index2].Key.chromosomes;
                    if (random.NextDouble() > chance)//клонирование двух родителей
                    {
                        newPopulation.Add(new List<double>(coll1));
                        newPopulation.Add(new List<double>(coll2));
                        continue;
                    }
                    //скрещивание 
                    List<double> newColl1 = new List<double>();
                    List<double> newColl2 = new List<double>();
                    //идем по генам - выбираем кому из потомков отдать ген первого предка, кому второго
                    for (int i = 0; i < coll1.Count(); i++)
                    {
                        if (random.Next(0, 2) == 0)
                        {
                            newColl1.Add(coll1[i]);
                            newColl2.Add(coll2[i]);
                        }
                        else
                        {
                            newColl1.Add(coll2[i]);
                            newColl2.Add(coll1[i]);
                        }
                    }
                    //формируем потомков
                    newPopulation.Add(newColl1);
                    newPopulation.Add(newColl2);
                }


                //мутации
                double persectageMutations = 0.30;//30% особей мутируют
                chance = 0.1;//10% генов мутируют
                for (int k = 0; k < newPopulation.Count(); k++)
                {
                    if (random.NextDouble() > persectageMutations)//особь не мутирует
                        continue;
                    //если мутирует, то изменяем ее гены, ! но не все, а какой-то процент
                    for (int j = 0; j < newPopulation[k].Count(); j++)
                    {
                        if (random.NextDouble() < chance)//ген мутируем
                        {
                            if (random.Next(0, 2) == 1)//+0.5
                            {
                                newPopulation[k][j] += 0.5;
                            }
                            else//-0.5
                            {
                                newPopulation[k][j] -= 0.5;
                            }
                        }
                    }
                }

                //сохранение новой популяции
                Population.Clear();//очищаем текущую популяцию
                //Замена популяции на countIndivid - 15 потомков и 15 элитных
                HashSet<int>indexInPopulation= new HashSet<int>();  
                for (int _ = 0; _ < countIndivid - elites.Count(); _++)
                {
                    int index = random.Next(0, Population.Count);
                    if (indexInPopulation.Contains(index))
                    {
                        _--;
                        continue;
                    }
                    Population.Add(new KeyValuePair<Network, int>(new Network(scelet, newPopulation[index]), 0));
                    indexInPopulation.Add(index);
                }
                //рандомно в популяцию вставляем элитных особей
                for (int i = 0; i < elites.Count(); i++)
                {
                    int indexInsert = random.Next(0, Population.Count());
                    Population.Insert(indexInsert, new KeyValuePair<Network, int>(elites[i].Key, 0));
                }
            }
            //возвращаем самую элитную особь
            return Population.OrderByDescending(x => x.Value).Take(1).ToList()[0].Key;
        }

        private static Network? Competition(Network WhiteNet, Network BlackNet)
        {
            //создаем сервис игры
            Service service = new Service();

            var currNetwork = WhiteNet;
            //сети начинают играть
            while (true)
            {
                // 1. получаем все возможные ходы
                var possibleStep = service.All_Possible_Move();//шашки, которыми сейчас можно сходить
                if (possibleStep.Count == 0)//победил соперник
                {
                    if(currNetwork==WhiteNet)//сейчас ходят белые, а ходов нет - победили черные
                        return BlackNet;
                    else  //сейчас ходят черные, а ходов нет -> победили белые
                        return WhiteNet;
                }

                // 2. выбираем лучший ход из возможных
                //key - пара: key- индекс шашки которой можно сходить, value - индекс хода из массива ходов этой шашкой
                //value - оценка этого хода
                Dictionary<KeyValuePair<Node, int>, double> Scores = new Dictionary<KeyValuePair<Node, int>, double>();//оценки каждого хода из possibleStep
                                                                                                                       //перебор возможных ходов
                foreach (var Checker in possibleStep)
                {
                    for (int j = 0; j < Checker.Value.Count; j++)
                    {
                        var path = Checker.Value[j];
                        //создаем копию доски - изменяя ее, никак не изменим исходную доску
                        var copy_game_place = Service.CopyGamePlace(service.game_place);
                        //делаем ход на копированной доске
                        Service.MakeMove(copy_game_place, path, service.current_step);
                        //преобразуем доску в список 
                        var cond = Genetic_Algorithm.Condition(copy_game_place);
                        //делаем прямой проход
                        var sc = currNetwork.Forward(cond);//получаем оценку этого хода
                                                        //запоминаем оценку
                        Scores[new KeyValuePair<Node, int>(Checker.Key, j)] = sc;
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
                if (service.movesWithoutCapture >= 30)// если 15 ходов без взятия -> ничья
                {
                    return null;
                }
                currNetwork=currNetwork==WhiteNet?BlackNet:WhiteNet;
            }
            return null;
        }
        public static List<double> Condition(Node[,] game_place)
        {
            List<double> currentCondition = new List<double>();//текущее состояние
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if ((j + 1) % 2 == 0 && (i + 1) % 2 != 0 || (j + 1) % 2 != 0 && (i + 1) % 2 == 0)
                    {
                        currentCondition.Add(game_place[i, j].Value);
                    }
                }
            }
            return currentCondition;
        }
        
    }
}