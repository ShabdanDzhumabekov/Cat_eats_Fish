using System;

namespace Cat_and_Fish
{
    class Program
    {
        class Cat // объект кот
        {
            int x_size; // Диапазон куда может пойти кот по x и y координате
            int y_size;

            int x=0, y=0; // координаты кота
            int calories ; // калории кота

            public delegate void Cat_Action_Description(string message); // создаем делегат который описывает действие кота
            Cat_Action_Description cad; // определяем делегат

            public void RegisterCad(Cat_Action_Description cad) // регистрируем делегат
            {
                this.cad = cad;
            }

            public Cat(int m, int n) // В конструкторе мы задаем диапазон хода кота,
            {                        // т.е насколько далеко можжет ходить, чтобы не выходил за границу
                this.x_size = m;
                this.y_size = n;
                calories = m + n - 5; // Изначальное количество калорий
            }

            public void Eat(int cal) // метод: кот ест рыбу
            {
                calories += cal;
                RegisterCad(DisplayGreenMessage);
                cad($"Сat replenished {cal} calories!");
            }

            public bool FindFish(Fish fish) // Метод(Инстинкт), кота, поиска рыбы
            {
                
                if (x == fish.x && y == fish.y) // координаты кота совпали с координатами рыбы
                {
                    Eat(fish.Calories); // кот съедает рыбу
                    fish.new_cordinate(); // назначается новая координата рыбы, т.е появляется новая рыба

                    RegisterCad(DisplayMessage);
                    cad($"calories: {calories}");

                    return true;

                }
                else if (calories==0)
                {
                    RegisterCad(DisplayRedMessage);
                    cad("R.I.P Cat");

                    return false;
                }

                RegisterCad(DisplayMessage);
                cad("");
                cad($"calories: {calories}");

                return true;
            }

            public int X { get { return x; } } // возвращаем x
            public int Y { get { return y; } } // возвращаем y

            public int Calories { get { return calories; } } // возвращаем кол-во калорий кота
            public void new_cordinate() // новая координата для кота
            {
                Random rnd = new Random();
                x = rnd.Next(0, x_size);
                y = rnd.Next(0, y_size);
            }
            public void Move(ConsoleKeyInfo Key)  // метод ходьбы кота по площадке
            {
                if (Key.Key == ConsoleKey.DownArrow)  // т.к базис площадки находится в левом верхнем углу и строится сверху вниз,
                {                                     //  наше движение по y кординате получается инверционным
                    if (y != y_size-1)
                    {
                        y += 1;
                        calories -= 1;
                    }
                }
                else if(Key.Key == ConsoleKey.UpArrow)
                {
                    if (y!=0)
                    {
                        y -= 1;
                        calories -= 1;
                    }
                    }
                else if(Key.Key == ConsoleKey.LeftArrow)
                {
                    if (x!=0)
                    {
                        x -= 1;
                        calories -= 1;
                    }
                }
                else if(Key.Key == ConsoleKey.RightArrow)
                {
                    if (x!= x_size-1)
                    {
                        x += 1;
                        calories -= 1;
                    }
                }
            }

            private static void DisplayMessage(string message)
            {
                Console.WriteLine(message);
            }

            private static void DisplayRedMessage(String message)
            {
                // Устанавливаем красный цвет символов
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                // Сбрасываем настройки цвета
                Console.ResetColor();
            }

            private static void DisplayGreenMessage(String message)
            {
                // Устанавливаем зеленый цвет символов
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                // Сбрасываем настройки цвета
                Console.ResetColor();
            }
        }

        class Fish // объект рыба
        {

            public int x;
            public int y;

            int x_size;
            int y_size;

            public Fish(int x_size, int y_size)
            {
                this.x_size = x_size;
                this.y_size = y_size;
            }
            private int calories = 10;
            public void new_cordinate()
            {
                Random rnd = new Random();

                x = rnd.Next(0, x_size);
                y = rnd.Next(0, y_size);
                calories = rnd.Next(1, 10); // новая рыба с новым кол-вом калорий
            }

            public int Calories { get { return calories; } }
        }

        class Field // объект поле
        {
            int x_size = 0;  // Размерность площадки по x
            int y_size = 0;  // Размерность площадки  по y

            
            public Field(int m, int n) // Определяем в конструкторе размерность поля
            {
                this.x_size = m;
                this.y_size = n;
            }
            
            public void Creat_plot(Cat c, Fish f) // Создается поле
            {

                for (int i=0; i<y_size; i++) // сперва поле строится по y
                {
                    for(int j=0; j<x_size; j++) // строим поле по x
                    {
                        if (j == c.X && i == c.Y)
                            Console.Write("C");
                        else if (j == f.x && i == f.y)
                            Console.Write("F");
                        else
                            Console.Write("*");
                    }

                    Console.WriteLine();
                }
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Enter the x size of plot = "); int x_size = Convert.ToInt32(Console.ReadLine());  // вводим размерность площадки
            Console.Write("Enter the y size of plot = "); int y_size = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            Cat cat = new Cat(x_size,y_size);
            Fish fish = new Fish(x_size,y_size);
            Field field = new Field(x_size,y_size);

            cat.new_cordinate();
            fish.new_cordinate();

            if(cat.X==fish.x && cat.Y==fish.y)
            {
                fish.new_cordinate();
            }

            ConsoleKeyInfo key;
            //field.Creat_plot(cat, fish);
            
            while (cat.FindFish(fish))
            {
                
                field.Creat_plot(cat, fish);
                key = Console.ReadKey();
                Console.Clear();
                cat.Move(key);
            }
        }
    }
}
