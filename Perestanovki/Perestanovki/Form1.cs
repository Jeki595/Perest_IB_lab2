using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Perestanovki
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Choice()
        {
            if (comboBox1.Text == "Изгородь")
            {
                Hedge(radioButton1, textBox1, textBox3);
            }
            else if (comboBox1.Text == "Ключевой")
            {
                Key(radioButton1, textBox1, textBox2, textBox3);
            }
            else if (comboBox1.Text == "Комбинированный")
                Combo(radioButton1, textBox1, textBox2, textBox3);
            else
                MessageBox.Show("Выберите метод шифрования");
        }

        static private string Nine_Group(string originalText, int leng)
        {
            if (leng % 9 != 0)                              // Проверка того что у всех чисел есть группа из девяти
            {
                for (int i = 0; i < 9 - leng % 9; i++)
                {
                    originalText += " ";                    // Добавление пробелов при недостатке чисел в группе
                }
            }
            return originalText;
        }

        static private string True_key(string key, TextBox textBox2)
        {
            try                                             // Проверка корректности ключа
            {
                key = textBox2.Text;
            }
            catch (Exception e)
            {
                MessageBox.Show("Некорректный ключ");
                return "1";
            }
            if (key.Length == 9)
            {
                for (int i = 1; i <= key.Length; i++)
                {
                    if (key.Contains(char.Parse($"{i}")))
                    {

                    }
                    else
                    {
                        MessageBox.Show("Данные некорректны");
                        return "1";
                    }
                }
            }
            else
            {
                MessageBox.Show("Данные некорректны");
                return "1";
            }
            return key;
        }
        static private string Correct_Text(string originalText, int leng)
        {
            for (int i = 0; i < leng; i++)              // Проверка совпадения алфавита
            {
                if ((originalText[i] >= 'А' && originalText[i] <= 'я') || originalText[i] == 'ё' || originalText[i] == 'Ё' || originalText[i] == ',' || originalText[i] == '.' || originalText[i] == ' ')
                { }
                else
                {
                    MessageBox.Show("Данные некорректны");
                    return "1";
                }
            }
            return "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)                               // Шифрование
            {
                Choice();
            }
            else if (radioButton2.Checked)                          // Дешифрование
            {
                Choice();
            }
            else
                MessageBox.Show("Выберите что желаете делать с текстом: шифрование или дешифрование!");
            textBox4.Text = null;
        }





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///     Изгородь
        static void Hedge(RadioButton radioButton1, TextBox textBox1, TextBox textBox3)
        {
            string originalText = textBox1.Text; // Textbox1
            int leng = originalText.Length;             // Длина текста


            bool prov = true;                           // Проверка на четность
            if (leng % 2 == 0)
            {
                prov = false;
            }

            if (radioButton1.Checked)
            {
                string konez = "";                          // Зашифрованный текст
                konez = Encode_Hedge(originalText, konez, prov, leng);
                textBox3.Text = konez;
            }
            else
            {
                string noviy = "";                          // Расшифрованный текст
                noviy = Decode_Hedge(originalText, noviy, prov, leng);
                textBox3.Text = noviy;
            }
        }




        static string Encode_Hedge(string originalText, string konez, bool prov, int leng)
        {
            if (Correct_Text(originalText, leng) == "1")           // Проверка совпадения алфавита
            {
                return "";
            }

            if (prov)                                   // Нечетные
            {
                for (int i = 0; i <= leng - 1; i += 2)  // Проход по элементам с шагом 2
                {
                    konez += originalText[i];
                    if (i == leng - 1)
                    {
                        i = -1;
                    }
                }
            }
            else                                        // Четные
            {
                for (int i = 0; i <= leng - 1; i += 2)  // Проход по элементам с шагом 2
                {
                    konez += originalText[i];
                    if (i == leng - 2)
                    {
                        i = -1;
                    }
                }
            }
            Console.WriteLine(konez);
            return konez;
        }

        static string Decode_Hedge(string originalText, string noviy, bool prov, int leng)
        {
            if (Correct_Text(originalText, leng) == "1")           // Проверка совпадения алфавита
            {
                return "";
            }

            if (prov)                                   // Не четные
            {
                int s = leng / 2 + 1;
                noviy += originalText[0];
                for (int i = s; i < leng; i += s)
                {
                    noviy += originalText[i];
                    i -= s - 1;
                    noviy += originalText[i];
                }
            }
            else                                        // Четные
            {
                int s = leng / 2;
                noviy += originalText[0];
                for (int i = s; i < leng; i += s)
                {
                    noviy += originalText[i];
                    if (i != leng - 1)
                    {
                        i -= s - 1;
                        noviy += originalText[i];
                    }
                }
            }
            return noviy;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///     Ключевой
        static void Key(RadioButton radioButton1, TextBox textBox1, TextBox textBox2, TextBox textBox3)
        {
            string originalText = textBox1.Text;       // Textbox1
            int leng = originalText.Length;                 // Длина начального текста
            string key = "";                                // Строка с ключем
            key = (True_key(key, textBox2));
            if (key == "1")
            {
                return;
            }

            if (radioButton1.Checked)
            {
                string konez = "";                              // Зашифрованный текст
                konez = Encode_Key(originalText, konez, key, leng);
                textBox3.Text = konez;
            }
            else
            {
                string noviy = "";                              // Расшифрованный текст
                noviy = Decode_Key(originalText, noviy, key, leng);
                textBox3.Text = noviy;
            }


        }
        static string Encode_Key(string originalText, string konez, string key, int leng)
        {

            if (Correct_Text(originalText, leng) == "1")           // Проверка совпадения алфавита
            {
                return "";
            }

            int start = 0;                                  // Стартовая точка для 9 чисел


            originalText = Nine_Group(originalText, leng);

            for (int i = 1; i < 9; i++)                     // Цикл для шифрования
            {
                while (i % 10 != 0)
                {
                    konez += originalText[start + int.Parse($"{key[i - 1]}") - 1];
                    i++;
                }
                start += 9;                                 // Смещение начальной точки на следующую группу из девяти
                if (start >= originalText.Length)           // Условие для выхода из цикла
                {
                    break;
                }
                i = 0;
            }
            return konez;
        }

        static string Decode_Key(string originalText, string noviy, string key, int leng)
        {

            if (Correct_Text(originalText, leng) == "1")           // Проверка совпадения алфавита
            {
                return "";
            }
            int start = 0;                                  // Стартовая точка для 9 чисел

            originalText = Nine_Group(originalText, leng);

            for (int i = 1; i < 9; i++)
            {
                while (i % 10 != 0)
                {
                    noviy += originalText[start + key.IndexOf(char.Parse($"{i}"))];
                    i++;
                }
                start += 9;                                 // Смещение начальной точки на следующую группу из девяти
                if (start >= leng)                          // Условие для выхода из цикла
                {
                    break;
                }
                i = 0;
            }
            return noviy;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///     Комбинированный

        static void Combo(RadioButton radioButton1, TextBox textBox1, TextBox textBox2, TextBox textBox3)
        {
            Console.WriteLine("Введите текст для шифрования:");
            string originalText = textBox1.Text;            // Textbox1
            int leng = originalText.Length;                 // Длина начального текста
            string key = "";                                // Строка с ключем

            key = (True_key(key, textBox2));
            if (key == "1")
            {
                return;
            }

            if (radioButton1.Checked)
            {
                string konez = "";                              // Зашифрованный текст
                konez = Encode_Comb(originalText, konez, key, leng);
                textBox3.Text = konez;
            }
            else
            {
                string noviy = "";                              // Расшифрованный текст
                noviy = Decode_Comb(originalText, key, leng);
                textBox3.Text = noviy;
            }




            string novi = "";
        }
        static string permutation(TextBox textBox1, TextBox textBox3, TextBox textBox4)    // Функция поиска ключа
        {
            string originalText = textBox1.Text;            // Textbox1
            int leng = originalText.Length;                 // Длина начального текста
            string noviy = "";
            string key2 = "123456789";                      // Строка для поиска ключа
            int count = 1;                                  // Счетчик для того чтобы работа циклов начиналась лишь на втором запуске
            noviy = Decode_Comb(originalText, key2, leng);
            textBox3.Text = noviy;
            DialogResult r = MessageBox.Show("Устраивает ли вас полученный текст", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.No)                  // Проверка ключа
            {
                char[] mas = new char[9];
                for (int i = 0; i < 9; i++)
                {
                    mas[i] = key2[i];
                }
                int counttt = 0;                            // counttt, countt, coun, cou, co, c, ci - счетчики для перемещения чисел массива

                for (int j = 0; j <= 40320; j++)            // 40320 это то через сколько заходов поменяется первая цифра
                {
                    if (count > 1)
                    {
                        swap(mas, 0, 8 - counttt);          // смена элементов начиная с самого наименьшего в промежутке массива от 2 до 9 элемента с первым
                        sort_m(mas, 1, 9);                  // сортировка по возрастанию элементов массива со 2 по 9
                        if (counttt == 7)                   // то сколько нужно отнять для вычисления того какой индекс менять следующим
                            counttt = 0;
                        else
                            counttt++;

                    }
                    int countt = 0;

                    count++;
                    for (int k = 0; k <= 5040; k++)         // Далее все циклы аналогичны
                    {
                        if (count > 2)
                        {
                            swap(mas, 1, 8 - countt);
                            sort_m(mas, 2, 9);
                            if (countt == 6)
                                countt = 0;
                            else
                                countt++;

                        }
                        count++;
                        int coun = 0;

                        for (int l = 0; l <= 720; l++)
                        {
                            if (count > 3)
                            {
                                swap(mas, 2, 8 - coun);
                                sort_m(mas, 3, 9);
                                if (coun == 5)
                                    coun = 0;
                                else
                                    coun++;

                            }
                            count++;
                            int cou = 0;

                            for (int m = 0; m <= 120; m++)
                            {
                                if (count > 4)
                                {
                                    swap(mas, 3, 8 - cou);
                                    sort_m(mas, 4, 9);
                                    if (cou == 4)
                                        cou = 0;
                                    else
                                        cou++;

                                }
                                count++;
                                int co = 0;
                                for (int n = 0; n <= 24; n++)
                                {
                                    if (count > 5)
                                    {
                                        swap(mas, 4, 8 - co);
                                        sort_m(mas, 5, 9);
                                        if (co == 3)
                                            co = 0;
                                        else
                                            co++;
                                    }
                                    count++;
                                    int c = 0;
                                    for (int s = 0; s <= 6; s++)
                                    {
                                        if (count > 6)
                                        {

                                            swap(mas, 5, 8 - c);
                                            swap(mas, 6, 8);
                                            if (c == 2)
                                                c = 0;
                                            else
                                                c++;
                                        }
                                        count++;
                                        int ci = 0;

                                        for (int d = 0; d <= 2; d++)
                                        {
                                            if (count > 7)
                                            {
                                                swap(mas, 6, 8 - ci);
                                                swap(mas, 7, 8);
                                                if (ci == 1)
                                                    ci = 0;
                                                else
                                                    ci++;
                                            }
                                            count++;
                                            for (int y = 0; y < 1; y++)
                                            {
                                                if (count > 8)
                                                {
                                                    string st = "";
                                                    for (int h = 0; h < 9; h++)         // Создание строки ключа
                                                    {
                                                        st += mas[h];
                                                    }
                                                    noviy = Decode_Comb(originalText, st, leng);    // Проверка ключа
                                                    textBox3.Text = noviy;
                                                    r = MessageBox.Show("Устраивает ли вас полученный текст", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                                    if (r == DialogResult.Yes)
                                                    {
                                                        MessageBox.Show("Ключ успешно найден!");
                                                        textBox4.Text = st;
                                                        return st;
                                                    }

                                                }
                                                count++;
                                                swap(mas, 7, 8);
                                                string str = "";
                                                for (int h = 0; h < 9; h++)
                                                {
                                                    str += mas[h];
                                                }
                                                noviy = Decode_Comb(originalText, str, leng);
                                                textBox3.Text = noviy;
                                                r = MessageBox.Show("Устраивает ли вас полученный текст", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                                if (r == DialogResult.Yes)
                                                {
                                                    MessageBox.Show("Ключ успешно найден!");
                                                    textBox4.Text = str;
                                                    return str;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (r == DialogResult.Yes)
            {
                MessageBox.Show("Ключ успешно найден!");
                textBox3.Text = noviy;
                textBox4.Text = key2;
                return key2;
            }
            else
            {
                MessageBox.Show("Вы ввели некорректное значение");
                return "1";
            }
            return "1";
        }

        static char[] swap(char[] mas, int i, int j) // Функция перестановки двух элементов
        {
            char temp = mas[j];
            mas[j] = mas[i];
            mas[i] = temp;
            return mas;
        }
        static char[] sort_m(char[] mas, int i, int j)  // Функция сортировки отдельных элементов массива по возрастанию
        {
            int nach = i;
            for (int l = i; l < j; l++)
            {
                for (int k = i + 1; k < j; k++)
                {
                    if (mas[l] > mas[k])
                    {
                        swap(mas, l, k);
                    }
                }
            }
            return mas;
        }
        static string Encode_Comb(string originalText, string konez, string key, int leng)       // Функция шифрования
        {

            if (Correct_Text(originalText, leng) == "1")           // Проверка совпадения алфавита
            {
                return "";
            }

            int start = 0;                                  // Стартовая точка для 9 чисел

            if (leng % 9 != 0)                              // Проверка того что у всех чисел есть группа из девяти
            {
                for (int i = 0; i < 9 - leng % 9; i++)
                {
                    originalText += " ";                    // Добавление пробелов при недостатке чисел в группе
                }
            }

            int str = originalText.Length / 9;
            char[,] mas = new char[str, 9];
            int count = 0;
            for (int i = 0; i < str; i++)                   // Заполнение массива текстом для шифрования
            {
                for (int j = 0; j < 9; j++)
                {
                    mas[i, j] = originalText[count];
                    count++;
                }
            }
            mas = sort(mas, str, key);                      // Сортировка массива по ключу

            for (int i = 0; i < 9; i++)                     // Занесение данных массива в строку
            {
                for (int j = 0; j < str; j++)
                {
                    konez += mas[j, i];
                }
            }
            return konez;
        }




        static char[,] sort(char[,] mas, int str, string key)   // Функция сортировки массива по ключу
        {
            char[,] exit = new char[str, 9];
            for (int k = 0; k < 9; k++)
            {
                int number = int.Parse($"{key[k]}");

                for (int i = 0; i < str; i++)
                {
                    for (int j = number - 1; j <= number - 1; j++)
                    {
                        exit[i, k] = mas[i, j];
                    }
                }

            }
            return exit;
        }

        static char[,] sort_d(char[,] mas, int str, string key)     // Функция сортировки массива по ключу для дешифрования
        {
            char[,] exit = new char[str, 9];
            for (int k = 0; k < 9; k++)
            {
                int number = int.Parse($"{key[k]}");

                for (int i = 0; i < str; i++)
                {
                    for (int j = number - 1; j <= number - 1; j++)
                    {
                        exit[i, j] = mas[i, k];
                    }
                }

            }
            return exit;
        }

        static string Decode_Comb(string originalText, string key, int leng)    // Функция дешифрования
        {
            int str = originalText.Length / 9;
            char[,] mas = new char[str, 9];
            int count = 0;
            string s = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < str; j++)
                {
                    mas[j, i] = originalText[count];
                    count++;
                }
            }
            mas = sort_d(mas, str, key);

            for (int i = 0; i < str; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    s += mas[i, j];
                }
            }
            return s;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            permutation(textBox1, textBox3, textBox4);
        }
    }
}
