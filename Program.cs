using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csv_Creator
{
    class Program
    {
        /// <summary>
        /// 最終列かどうかの判定をします
        /// </summary>
        /// <param name="finalColIndex"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        static bool IsWriteComma(int finalColIndex, int currentIndex)
        {
            return finalColIndex != currentIndex;
        }

        /// <summary>
        /// 最終列以外にカンマを付け足します
        /// </summary>
        /// <param name="col"></param>
        /// <param name="finalColIndex"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        static string AddComma(string col, int finalColIndex, int j)
        {
            string comma = ",";

            if (IsWriteComma(finalColIndex, j))
            {
                col += comma;
            }

            return col;
        }

        static void WriteCsv(StreamWriter writer, CsvFile csvFile)
        {
            try
            {
                for (int i = 0; i < csvFile.RowCount; i++)
                {
                    int j = 0;
                    csvFile.OneLine.ToList().ForEach(col =>
                    {
                        col = csvFile.IsDynamics[j] == "y" ? col + i.ToString() : col;
                        col = AddComma(col, csvFile.OneLine.Length - 1, j);
                        writer.Write(col);
                        j++;
                    });
                    //すべての列に値を入れてたら改行する
                    writer.Write(Environment.NewLine);
                }
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static bool IsCorrectAnswer(string target)
        {
            string[] correctAnswers = { "y", "n" };

            bool judge = correctAnswers.ToList()
                        .Exists(correct => correct.Equals(target));

            return judge;
        }

        static bool CheckAnswer(string target)
        {
            bool judge = IsCorrectAnswer(target);

            if (!judge)
            {
                Console.WriteLine("y or nで答えてください");
            }

            return judge;
        }

        static void Main(string[] args)
        {
            //一時ファイルに保存する設定
            string tmpPath = Path.GetTempPath();
            Console.WriteLine($"保存するパスは{tmpPath}です！");

            Console.WriteLine("ファイル名を入力してください!(拡張子はいりません)");
            string fileName = Console.ReadLine() + ".csv";

            Console.WriteLine("列数を入力してください!");
            int colmunCount = int.Parse(Console.ReadLine());

            Console.WriteLine("行数を入力してください!");
            int rowCount = int.Parse(Console.ReadLine());

            string[] oneLine = new string[colmunCount];
            string[] isDynamics = new string[colmunCount];
            for (int i = 0; i < colmunCount; i++)
            {
                Console.WriteLine($"{i}番目の値を入力してください!:");
                oneLine[i] = Console.ReadLine();

                bool flage = true;
                string target = "";
                while (flage)
                {
                    Console.WriteLine("動的にしますか?(y or n)");
                    target = Console.ReadLine();
                    //正しい答えの際、falseにする
                    flage = !CheckAnswer(target);
                }

                isDynamics[i] = target;
            }

            var csvFile = new CsvFile(fileName, tmpPath, oneLine, isDynamics, rowCount, colmunCount);
            try
            {
                bool judge = csvFile.ExistCsvFile();

                if (judge)
                {
                    throw new Exception("ファイルが存在します");
                }

                using (StreamWriter writer = new StreamWriter(csvFile.GetCsvFilePath()))
                {
                    WriteCsv(writer, csvFile);
                }

                Console.WriteLine("成功です!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("終了します(keyを押したら終了します)");
                Console.ReadLine();
            }
        }
    }
}
