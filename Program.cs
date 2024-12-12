using System;
namespace ConsoleApp1
{
    public class Time
    {
        private int hours;
        private int minutes;
        public int Hours
        {

            get
            {
                return hours;
            }

            set
            {
                if (hours < 0 || hours > 24)
                {
                    Console.WriteLine("Error");
                }
                hours = value;
            }

        }
        public int Minutes
        {
            get
            {
                return minutes;
            }
            set
            {
                if (minutes < 0 || minutes > 60)
                {
                    Console.WriteLine("Error");
                }
                minutes = value;
            }
        }
        // унарная операция операнда -- (вычитание минут)
        public static Time operator --(Time time)
        {
            time.minutes--;
            if (time.minutes < 0)
            {
                time.minutes = 59;
                time.hours--;
            }
            if (time.hours < 0)
            {
                time.hours = 0;
                time.minutes = 0;
            }
            return time;
        }
        // бинарная операция операнда + (Time, int)
        public static Time operator +(Time time, int minutes)
        {
            time.minutes += minutes;
            while (time.minutes >= 60)
            {
                time.minutes -= 60;
                time.hours++;
            }
            return time;
        }
        // для право- и левостороннего операнда
        public static Time operator +(int minutes, Time time)
        {
            time.minutes += minutes;
            while (time.minutes >= 60)
            {
                time.minutes -= 60;
                time.hours++;
            }
            return time;
        }

        // бинарная операция операнда + (Time, Time)
        public static Time operator +(Time t1, Time t2)
        {
            int totalMinutes = t1.hours * 60 + t1.minutes + t2.hours * 60 + t2.minutes;
            return new Time(totalMinutes / 60, totalMinutes % 60);
        }

        // явное преобразование в int
        public static explicit operator int(Time time)
        {
            return time.hours;
        }

        // неявное преобразование в bool
        public static implicit operator bool(Time time)
        {
            return time.hours != 0; // && time.minutes != 0; если минуты нужны
        }

        public static bool operator >(Time t1, Time t2) // перегрузка сравнения для поиска максимума
        {
            if (t1.hours > t2.hours) return true;
            if (t1.hours == t2.hours && t1.minutes > t2.minutes) return true;
            return false;
        }

        public static bool operator <(Time t1, Time t2) // перегрузка сравнения для поиска минимума (пришлось сделать и оператор < тоже)
        {
            if (t1.hours < t2.hours) return true;
            if (t1.hours == t2.hours && t1.minutes > t2.minutes) return true;
            return false;
        }

        public Time(int hours, int minutes)
        {
            this.Hours = hours; // указатели this на поля объекта Time (иначе будет по нулям)
            this.Minutes = minutes;
        }
        // метод вычитания минут
        public Time DiffMinutes(int minutesToDiff)
        {
            int totalMinutes = hours * 60 + minutes - minutesToDiff;

            // если < 0 то устанавливается 0
            if (totalMinutes < 0)
            {
                totalMinutes = 0;
            }

            return new Time(totalMinutes / 60, totalMinutes % 60); // возврат значения нового времени
        }

        // метод для вывода информации о времени
        public void PrintTime()
        {
            Console.WriteLine($"{hours:D2}:{minutes:D2}"); // $"{}" - вставка значений часов и минут прямо в строчку
                                                           // D - формат десятка, 2 - кол-во символов, чтобы добавить ноль
        }

        class TimeArray
        {
            Time[] arr = null;
            static Random rnd = new Random();
            public static int count = 0;

            public int Length
            {
                get
                {
                    return arr.Length;
                }
            }

            public int Max()
            {

                int max = 0;
                for (int i = 1; i < arr.Length; i++)
                {
                    if (arr[i] > arr[max])
                    {
                        max = i;
                    }
                }

                return max;
            }

            public TimeArray() // конструктор без параметров
            {
                arr = new Time[1];
                arr[0] = new Time(12, 30);
                count++;
            }
            public TimeArray(int size) // конструктор с параметром
            {
                arr = new Time[size];
                for (int i = 0; i < size; i++)
                {
                    Time t = new Time(rnd.Next(0, 24), rnd.Next(0, 60));
                    arr[i] = t;
                    count++;
                }
            }
            public TimeArray(params Time[] list) // конструктор с перемен.
            {
                arr = new Time[list.Length]; // вытаскиваем длину массива
                for (int i = 0; i < list.Length; i++)
                {
                    arr[i] = list[i];
                    count++;
                }
            }

            // индексатор (особый случай перегрузки операции доступа к элементам) позволяют обращаться по индексу к объектам классов (как с массивами)
            public Time this[int index]
            {
                get
                {
                    if (index >= 0 && index < arr.Length)
                        return arr[index];
                    else
                    {
                        Console.WriteLine("Ошибка в индексаторе по get");
                        return new Time(0, 0);
                    }
                }
                set
                {
                    if (index >= 0 && index < arr.Length)
                        arr[index] = value;
                    else
                    {
                        Console.WriteLine("Ошибка в индексаторе по get");
                    }
                }
            }

            // просмотр элементов
            public void PrintTime()
            {
                if (arr == null || arr.Length == 0)
                {
                    Console.WriteLine("Массив пустой");
                }
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i].PrintTime();
                }
            }
        }
        static void Main()
        {
            // объект типа Time
            Time time1 = new Time(10, 30);
            time1.PrintTime();

            // вычитание и результат в newTime
            Time newTime = time1.DiffMinutes(90);
            newTime = 5 + newTime;
            newTime--;
            newTime.PrintTime();


            Time newnewTime = time1 + newTime; // пример сложения временных диапазонов
            newnewTime.PrintTime();

            int x = (int)newnewTime;
            Console.WriteLine(x); // пример преобразования в int

            bool ok = newnewTime;
            Console.WriteLine(ok); // пример преобразования в bool


            Console.WriteLine("\nМассив первый");

            TimeArray tar1 = new TimeArray(3); // первый массив сгенерирован случайно
            tar1.PrintTime();

            Console.WriteLine("\nМассив второй");

            Time t1 = new Time(16, 48); // второй массив с заданными переменными
            Time t2 = new Time(12, 42);
            Time t3 = new Time(07, 11);
            TimeArray tar2 = new TimeArray(t1, t2, t3);
            tar2[1] = new Time(12, 34); // заменили 2 элемент 2 массива на 12-34
            tar2.PrintTime();

            Console.WriteLine("\nМассив третий");

            TimeArray tar3 = new TimeArray();
            tar3.PrintTime();

            Console.WriteLine("\nКоличество созданных в программе объектов: " + TimeArray.count);

            Console.WriteLine("Второй элемент первого массива: ");
            tar1[1].PrintTime(); // вывели через индексатор 2 элемент 1 массива

            int maximum1 = tar1.Max();
            Console.WriteLine("Номер максимального элемента в первом массиве: " + maximum1);

            int maximum2 = tar2.Max();
            Console.WriteLine("Номер максимального элемента во втором массиве: " + maximum2);

        }

    }
}