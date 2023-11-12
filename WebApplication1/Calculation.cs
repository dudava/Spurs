using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationsOfSpurs
{
    internal class Calculation
    {
        // ibeam's characteristics

        Dictionary<int, Dictionary<string, string>> ibeams = new Dictionary<int, Dictionary<string, string>>()
        {
            [0] = new Dictionary<string, string>()
            {
                { "Тип", "15K2" },
                { "Ширина полки", "0.15" }
            },
            [1] = new Dictionary<string, string>()
            {
                { "Тип", "20K2" },
                { "Ширина полки", "0.2" }
            },
            [2] = new Dictionary<string, string>()
            {
                { "Тип", "25K2" },
                { "Ширина полки", "0.25" }
            },
            [3] = new Dictionary<string, string>()
            {
                { "Тип", "30K2" },
                { "Ширина полки", "0.3" }
            },
            [4] = new Dictionary<string, string>()
            {
                { "Тип", "35K2" },
                { "Ширина полки", "0.35" }
            },

        };


        // Переменные веденные Вовой
        double DifDistance = 0.025; // Расстояние меняющееся в процессе переборки двутавра

        // Data to output only

        string ibeamtype, concretetype;

        double Qf, Gammab1, a, b, h0;

        const double Gammab3 = 0.85;

        // ConcreateChippingCheck

        double Qult, nu, Hox, Hoy, Ab, Rbt;

        // MinimalSpurDepth

        double Fbult, Rb, Abloc, Abmax, Rloc, fib, c, e;

        //

        bool[] changerequirements;  // доделать

        int Variant;  // доделать


        public Calculation(string concretetype, string ibeamtype, double[] variables, bool[] changerequirements)
        {
            this.concretetype = concretetype;
            this.ibeamtype = ibeamtype;


            this.changerequirements = changerequirements;
            this.changerequirements = changerequirements;

            if (changerequirements[0] == true && changerequirements[1] == true)
            {
                Variant = 2;
            }
        }

        int FindUsersSpur(string SpurMark)
        {
            string value1 = string.Empty;

            int n = 0;

            while (String.Equals(SpurMark, value1))
            {
                ibeams[n].TryGetValue("Тип", out value1);
                n++;
            }
            n--;
            b = Convert.ToDouble(ibeams[n]["Ширина полки"]);
            return n;
        }

        void ChoiceOptions(int Variant)  //Основные опирации по выбору типа подбора через а || b || a & b 
        {
            switch (Variant)
            {
                case 0:     // При желании подбора только по ширине двутавра
                    FindUsersSpur(ibeamtype);
                    ConcreteChippingCheck(FindUsersSpur(ibeamtype));
                    break;
                case 1:     // При желании подбора только по глубине заделки двутавра
                    a += 0.1;
                    ConcreteChippingCheck();
                    break;
                case 2:     // При желании подбора по двум характеристикам
                    FindUsersSpur(ibeamtype);
                    a += 0.2;
                    ConcreteChippingCheck(FindUsersSpur(ibeamtype));
                    break;
            }
        }

        void Outputvalues()    // При этом порядке осуществелния функций, сначало будет изменяться значение заделки шпоры, потом уже ширина полки двутавра
        {
            MinimalSpurDepth();
            ConcreteChippingCheck();
        }

        void ConcreteChippingCheck()
        {
            h0 = 0.5 * (Hox + Hoy);
            nu = 2 * (b + a + (3 * h0) / 2);
            Ab = h0 * nu;
            Fbult = Rbt * Ab * Gammab1 * Gammab3;

            if (Qf < Fbult)
            {
                //Дописать
            }
            else
            {
                ChoiceOptions(Variant); // Решил реализовать метод, который при непрохождении  на выкалывания смог считать выборанный пользователем расчет
            }
        }
        void ConcreteChippingCheck(int n)
        {
            h0 = 0.5 * (Hox + Hoy);
            nu = 2 * (b + a + (3 * h0) / 2);
            Ab = h0 * nu;
            Fbult = Rbt * Ab * Gammab1 * Gammab3;
            if (Qf < Fbult)
            {
                //Дописать
            }
            else
            {
                if (n < 5 || Hox > 0.1 || Hoy > 0.1) // Сделал проверку на элементы ограниченные списком, при ситуации, где двутавр не попадает в список, мы выводим, что двутавра нет
                {
                    b = Convert.ToDouble(ibeams[++n]["Ширина полки"]);
                    if (n > 0)
                    {
                        Hox = h0 - DifDistance;            // При изменении ширины двутавра будет пересчет изначальных расстояний от края элемента до арматуры
                        Hoy = h0 - DifDistance;
                    }

                    ConcreteChippingCheck(n);


                }
                else
                {
                    //Вывод на экран: Необходимого размера двутавра нет ?? Console.WriteLine ("Необходимого размера двутавра нет");
                }
            }
        }
        void MinimalSpurDepth()
        {

            e = (a / 2) + c;
            Abloc = b * (a / 2);
            Abmax = 2 * (a / 2) * (a / 2) + Abloc;
            fib = 0.8 * Math.Sqrt(Abmax / Abloc);
            Rloc = Rb * fib * Gammab1 * Gammab3;
            Qult = (Rloc * a * b) / (6 * (e / a) + 1);
            if (Qf < Qult)
            {
                //Дописать
            }
            else
            {
                MinimalSpurDepth(a + 0.01); // Также при помощи перегрузки сделал цикл по подбору величины заделки
            }

        }
        void MinimalSpurDepth(double a)
        {

            e = (a / 2) + c;
            Abloc = b * (a / 2);
            Abmax = 2 * (a / 2) * (a / 2) + Abloc;
            fib = 0.8 * Math.Sqrt(Abmax / Abloc);
            Rloc = Rb * fib * Gammab1 * Gammab3;
            Qult = (Rloc * a * b) / (6 * (e / a) + 1);
            if (Qf < Qult)
            {
                //Дописать
            }
            else
            {
                MinimalSpurDepth(a + 0.01);
            }

        }
        public string GetibeamType()
        {
            return ibeamtype;
        }
    }
}
