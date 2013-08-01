using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
namespace stdChartered_test
{   
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入input文件的位置");
            string filepath = Console.ReadLine();
            FileInfo thesource = new FileInfo(filepath);
            while (!thesource.Exists)
            {
                Console.WriteLine("不存在此文件，请检查输入路径并重新输入");
                filepath = Console.ReadLine();
                thesource = new FileInfo(filepath);
            }
            StreamReader stream = thesource.OpenText();
            Console.WriteLine("请输入output文件的目的地址");

            string tofilepath = Console.ReadLine();
            textop test = new textop(stream);
            test.output(tofilepath);
            Console.WriteLine("执行完成!!!!");
            Console.ReadLine();
        }
    }
    class textop
    {
        public textop(StreamReader stream)
        {
            this.stream = stream;
        }
        private void getInfo() //分析输入文件
        {
            string text = stream.ReadLine();
            double value = 0.0;
            string[] words;
            int lines = 1;
            while (text != null && text != "")
            {
                words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length != 5) //words的长度应该为5个
                    Console.WriteLine("文件第{0}行格式错误",lines);
                value = Convert.ToDouble(words[3]);
                unitDic.Add(words[1], value);

                lines++;
                text = stream.ReadLine();
            }
        }
        public void output(string tofile)  //输出文件
        {
            getInfo();
            StreamWriter writer = new StreamWriter(tofile, false);
            writer.WriteLine("xuemeng1993@126.com");
            writer.WriteLine(""); //第二行空出
            string text;
            string[] words;

            double value = 0.0;//数值
            string unit; //单位
            string op;   //运算符

            for (int i = 0; i < 10; i++) //只有10行
            {
                double result, temp;
                text = stream.ReadLine();
                words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //首先读取两个
                if (words.Length < 2)
                {
                    Console.WriteLine("input文件格式有误");
                    return;
                }
                value = Convert.ToDouble(words[0]);
                unit = getSameStr(words[1]);
                result = value * unitDic[unit]; //算出结果
                int wordIndex = 2;
                if (words.Length > 2)
                {
                    //表示是一个算式
                    while (wordIndex < words.Length)
                    {
                        string[] tempwords = getsubstr(words, wordIndex);

                        op = tempwords[0]; //0一定是运算符
                        value = Convert.ToDouble(tempwords[1]); //1一定是数字
                        unit = getSameStr(tempwords[2]); //2一定是单位

                        temp = value * unitDic[unit]; //算出结果
                        result = compute(result, temp, op);
                        wordIndex = wordIndex + 3;
                    }
                }
                string rst = (Math.Round(result,2,MidpointRounding.AwayFromZero)).ToString(); //保留两位小数
                int k = 0;
                for (; k < rst.Length; k++)
                {
                    if (rst[k] == '.')
                        break;
                }
                if (rst[k] != '.')
                {
                    //表示不存在小数点
                    rst = rst + ".00";
                }
                else
                {
                    k = k + 1;
                    if (k == rst.Length - 1)
                    {
                        //只有1位小数
                        rst = rst + "0";
                    }
                }
                writer.Write(rst);
                writer.Write(" m");
                writer.WriteLine("");
            }
            writer.Close();
            stream.Close();
        }
        private string[] getsubstr(string[] words, int start)
        {
            string[] result = new string[3];
            result[0] = words[start];
            result[1] = words[start + 1];
            result[2] = words[start + 2];
            return result;
        }
        private double compute(double lhs, double rhs, string op)
        { 
            //只可能是加减运算
            if (op == "+")
            {
                return lhs + rhs;
            }
            else if (op == "-")
            {
                return lhs - rhs;
            }
            else
                return 0.0;
        }
        private string getSameStr(string data)
        {

            if (data == "feet" || data == "foot")
                return "foot";
            if (isEuqual("mile", data))
                return "mile";
            else if (isEuqual("yard", data))
                return "yard";
            else if (isEuqual("inch", data))
                return "inch";
            else if (isEuqual("fath", data))
                return "fath";
            else if (isEuqual("furlong", data))
                return "furlong";
            else
                return "";
        }
        private bool isEuqual(string lhs, string rhs)
        {
            int index = 0;
            while (index < lhs.Length && index < rhs.Length)
            {
                if (lhs[index] != rhs[index])
                    return false;
                index++;
            }
            //到达这里，则为true
            return true;
        }
        private StreamReader stream;
        private Dictionary<string ,double > unitDic = new Dictionary<string ,double>(); //字典
    }
}
