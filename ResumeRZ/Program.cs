using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace RZ
{
    struct RZPersonalInfo
    {
        public string rz_name;
        public string rz_surname;
        public int rz_age;

        public string rz_login;
        public string rz_password;
    };
    class Program
    {
        public static int userCount = 0;
        private const int password_len = 6;
        public static void Main(string[] args)
        {
            var info = new RZPersonalInfo[userCount];
            RZMenyu(ref info);
        }
        private static void SetColorBlue()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void SetTextCoord(string text, int x, int y, int lenstr)
        {
            int i = 0, temp_len_str = lenstr;
            int Y = y;
            Console.SetCursorPosition(x, y);
            do
            {
                Console.Write(text[i]);
                i += 1;
                temp_len_str -= 1;
                if (temp_len_str == 0)
                {
                    Console.WriteLine();
                    temp_len_str = lenstr;
                    Y += 1;
                    Console.SetCursorPosition(x, Y);
                }
            } while (i < text.Length);
        }

        private static void RZSignUp(ref RZPersonalInfo[] oldInfo, int userCount)
        {
            Console.SetWindowSize(35, 19);
            Console.SetBufferSize(35, 19);
            var info = new RZPersonalInfo[userCount];
            if (userCount > 1)
            {
                for (int i = 0; i < oldInfo.Length; i++)
                {
                    info[i].rz_name = oldInfo[i].rz_name;
                    info[i].rz_surname = oldInfo[i].rz_surname;
                    info[i].rz_age = oldInfo[i].rz_age;
                    info[i].rz_login = oldInfo[i].rz_login;
                    info[i].rz_password = oldInfo[i].rz_password;
                }
            }

            userCount -= 1;
            do
            {
                Console.WriteLine(userCount);
                do
                {
                    Console.SetCursorPosition(2, 6);
                    Console.Write("Name:    ");
                    info[userCount].rz_name = Console.ReadLine();
                } while (info[userCount].rz_name.Length == 0);
                do
                {
                    Console.SetCursorPosition(2, 7);
                    Console.Write("Surname: ");
                    info[userCount].rz_surname = Console.ReadLine();
                } while (info[userCount].rz_surname.Length == 0);
                do
                {
                    Console.SetCursorPosition(2, 8);
                    Console.Write("Age:     ");
                    int.TryParse(Console.ReadLine(), out info[userCount].rz_age);
                } while (info[userCount].rz_age == 0);

                Console.WriteLine($"Name:    {info[userCount].rz_name}");
                Console.WriteLine($"Surname: {info[userCount].rz_surname}");
                Console.WriteLine($"Age:     {info[userCount].rz_age}");
                Console.WriteLine();

                if (info[userCount].rz_name.Length > 0)
                {
                    info[userCount].rz_login = info[userCount].rz_name.ToUpper();
                    info[userCount].rz_login += "_";
                }
                if (info[userCount].rz_surname.Length > 0)
                {
                    string temp_name = info[userCount].rz_surname.ToLower();
                    info[userCount].rz_login += temp_name[0];
                    if (info[userCount].rz_surname.Length > 1)
                        info[userCount].rz_login += temp_name[1];
                }
                if (info[userCount].rz_age > 0)
                {
                    if (info[userCount].rz_surname.Length > 0)
                        info[userCount].rz_login += "_";
                    info[userCount].rz_login += info[userCount].rz_age.ToString();
                }

                info[userCount].rz_password = RandomString(password_len);

                Console.WriteLine($"User Login:    {info[userCount].rz_login}");
                Console.WriteLine($"User Password: {info[userCount].rz_password}");

                Console.ReadKey();

            } while (info[userCount].rz_login.Length == 0);
            RZWriteBin(ref info);
            oldInfo = info;
        }
        private static void RZLogIn(ref RZPersonalInfo[] oldInfo)
        {
            if (oldInfo.Length == 0)
            {
                Console.WriteLine("Users is not exist!");
                Console.ReadKey();
                return;
            }
            Console.SetBufferSize(75, 20);
            Console.SetWindowSize(75, 20);

            int u_ind = 0;

            Console.SetCursorPosition(1, 6);
            Console.WriteLine("Press Up or Down Key");
            Console.SetCursorPosition(1, 7);
            Console.Write($"User Name: {oldInfo[u_ind].rz_login}");
            do
            {
                Console.SetCursorPosition(12, 7);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                int login_len = 0;
                foreach (var item in oldInfo)
                {
                    if (login_len < item.rz_login.Length)
                        login_len = item.rz_login.Length;
                }
                for (int i = 0; i <= login_len; i++)
                {
                    Console.Write(" ");
                }
                Console.SetCursorPosition(12, 7);
                Console.WriteLine(oldInfo[u_ind].rz_login);
                Console.ResetColor();
                Console.SetCursorPosition(12, 7);
                var cki = Console.ReadKey();
//                Console.WriteLine(cki.Key);
                if (cki.Key == ConsoleKey.Escape)
                {
                    RZMenyu(ref oldInfo);
                }
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    u_ind += 1;
                    if (u_ind >= oldInfo.Length)
                    {
                        u_ind = oldInfo.Length - 1;
                    }
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    u_ind -= 1;
                    if (u_ind <= 0)
                    {
                        u_ind = 0;
                    }
                }
                if (cki.Key == ConsoleKey.Enter)
                {
                    string temp_password;
                    Console.SetCursorPosition(1, 8);
                    Console.WriteLine("User Pass: ");
                    Console.SetCursorPosition(1, 6);
                    Console.WriteLine("                    ");
                    Console.SetCursorPosition(12, 8);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int i = 0; i <= login_len; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.SetCursorPosition(12, 8);
                    temp_password = Console.ReadLine();
                    Console.ResetColor();

                    if (temp_password == oldInfo[u_ind].rz_password)
                    {
                        Console.SetCursorPosition(20, 0);
                        for (int i = 0; i < password_len; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(1, 9);
                        Console.WriteLine("                      ");
                        Console.SetCursorPosition(1, 9);
                        Console.WriteLine("Correct Password");
                        Console.SetCursorPosition(50, 0);
                        Console.WriteLine($"Welcome, { oldInfo[u_ind].rz_name }");
                    } else {
                        Console.SetCursorPosition(1, 9);
                        Console.WriteLine("Don't correct password");
                        Console.SetCursorPosition(1, 6);
                        Console.WriteLine("Press Up or Down Key");
                        Console.SetCursorPosition(1, 8);
                        Console.WriteLine("                                  ");
                        Console.SetCursorPosition(12, 7);
                    } 


//                    Console.WriteLine(oldInfo[u_ind].rz_password);
                }
                if (cki.Key == ConsoleKey.F1)
                {
                    Console.SetCursorPosition(20, 0);
                    Console.WriteLine(oldInfo[u_ind].rz_password);
                }
                } while (true);
            
        }
        private static void RZShowDatabase(ref RZPersonalInfo[] oldInfo)
        {
//            Console.SetWindowSize(78, 78);
            Console.SetBufferSize(78, 78);
            Console.WriteLine();
            for (int i = 0; i < oldInfo.Length; i++)
            {
                Console.WriteLine(oldInfo[i].rz_name);
                Console.WriteLine(oldInfo[i].rz_surname);
                Console.WriteLine(oldInfo[i].rz_age);
                Console.WriteLine(oldInfo[i].rz_login);
                Console.WriteLine(oldInfo[i].rz_password);
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        private static byte[] ConvertObjectToByteArray(object ob)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, ob);
            return ms.ToArray();
        }
        private static object ConvertByteArrayToObject(byte[] ba)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new MemoryStream(ba);
            return bf.Deserialize(stream);
        }
        private static void RZWriteBin(ref RZPersonalInfo[] info)
        {
            BinaryWriter bw;
            int il = info.Length;
            //create the file
            try
            {
                bw = new BinaryWriter(new FileStream("mydata.bin", FileMode.Create));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot create file.");
                return;
            }
            //writing into the file
            try
            {
                bw.Write(il);
                for (int i = 0; i < il; i++)
                {
                    bw.Write(info[i].rz_name);
                    bw.Write(info[i].rz_surname);
                    bw.Write(info[i].rz_age);
                    bw.Write(info[i].rz_login);
                    bw.Write(info[i].rz_password);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                return;
            }
            bw.Close();
        }
        private static void RZReadBin(ref RZPersonalInfo[] oldInfo)
        {
            BinaryReader br;

            int il = 1;
            var info = new RZPersonalInfo[il];

            //reading from the file
            try
            {
                br = new BinaryReader(new FileStream("mydata.bin", FileMode.Open));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot open file.");
                return;
            }
            try
            {
                il = br.ReadInt32();
                Console.WriteLine("Integer data: {0}", il);
                if (il > 0)
                {
                    info = new RZPersonalInfo[il];
                }
                for (int i = 0; i < il; i++)
                {
                    info[i].rz_name = br.ReadString();
                    info[i].rz_surname = br.ReadString();
                    info[i].rz_age = br.ReadInt32();
                    info[i].rz_login = br.ReadString();
                    info[i].rz_password = br.ReadString();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot read from file.");
                return;
            }
            br.Close();
            oldInfo = info;
        }
        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = //"ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
//                                 "abcdefghijklmnopqrstuvwxyz" +
                                 "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            //int rz_version = 2;
            //var rz_pass = new string[length];
            //if (rz_version == 2)
            //{
            //    for (int i = 0; i < length; i++)
            //    {
            //    }
            //}
        }
        private static void RZMenyu(ref RZPersonalInfo[] info)
        {
            Console.SetWindowSize(20, 11);
            Console.SetBufferSize(20, 11);
            RZReadBin(ref info);
            userCount = info.Length;

            int m_ind = 0;
            int m_count = 5;
            var m_list = new string[m_count];
            m_list[0] = "  Sign up     ";
            m_list[1] = "  Log in      ";
            m_list[2] = "  My resume   ";
            m_list[3] = "  Parameters  ";
            m_list[4] = "  Exit        ";

            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                for (short i = 0; i < m_list.Length; i++)
                {
                    if (m_ind == i)
                    {
                        SetColorBlue();
                        Console.WriteLine(m_list[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(m_list[i]);
                    }
                }
                cki = Console.ReadKey();
//                Console.WriteLine(cki.Key);
                if (cki.Key == ConsoleKey.Escape)
                {
                    System.Environment.Exit(1);
                }
                if (cki.KeyChar == 'a')
                {
                    Console.WriteLine("You pressed a");
                }
                if (cki.Key == ConsoleKey.X &&
                cki.Modifiers == ConsoleModifiers.Control)
                {
                    Console.WriteLine("You pressed control X");
                }
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    m_ind += 1;
                    if (m_ind >= m_count)
                    {
                        m_ind = m_count - 1;
                    }
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    m_ind -= 1;
                    if (m_ind <= 0)
                    {
                        m_ind = 0;
                    }
                }
                if (cki.Key == ConsoleKey.Enter)
                {
                    switch (m_ind)
                    {
                        case 0:
                            userCount += 1;
                            RZSignUp(ref info, userCount);
                            break;
                        case 1:
                            RZLogIn(ref info);
                            break;
                        case 2:
                            RZPrintMyResume(ref info);
                            break;
                        case 3:
                            RZParameters(ref info);
                            break;
                        case 4:
                            System.Environment.Exit(1);
                            break;
                        default:
                            break;
                    }
                }
            } while (true);
        }
        private static void RZParameters(ref RZPersonalInfo[] info)
        {
            Console.SetWindowSize(23, 9);
            Console.SetBufferSize(23, 9);
            int p_ind = 0;
            int p_count = 3;
            var p_list = new string[p_count];
//            p_list[0] = "  Menyu color change  ";
            p_list[0] = "  Reset Database      ";
            p_list[1] = "  Show Database info  ";
            p_list[2] = "  Back                ";

            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                for (short i = 0; i < p_list.Length; i++)
                {
                    if (p_ind == i)
                    {
                        SetColorBlue();
                        Console.WriteLine(p_list[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(p_list[i]);
                    }
                }
                cki = Console.ReadKey();
//                Console.WriteLine(cki.Key);
                if (cki.Key == ConsoleKey.Escape)
                {
                    System.Environment.Exit(1);
                }
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    p_ind += 1;
                    if (p_ind >= p_count)
                    {
                        p_ind = p_count - 1;
                    }
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    p_ind -= 1;
                    if (p_ind <= 0)
                    {
                        p_ind = 0;
                    }
                }
                if (cki.Key == ConsoleKey.Enter)
                {
                    switch (p_ind)
                    {
                        case 0:
                            RZResetDatabase(ref info);
                            break;
                        case 1:
                            RZShowDatabase(ref info);
                            break;
                        case 2:
                            RZMenyu(ref info);
                            break;
                        default:
                            break;
                    }
                }
            } while (true);
        }
        public static void RZDeleteFile(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            bool isDeleted = false;
            while (!isDeleted)
            {
                try
                {
                    File.Delete(path);
                    isDeleted = true;
                }
                catch (Exception)
                {
                }
            }
        }
        private static void RZResetDatabase(ref RZPersonalInfo[] info)
        {
            RZDeleteFile("mydata.bin");

            var temp_info = new RZPersonalInfo[0];
            info = temp_info;

            //BinaryWriter bw;
            ////create the file
            //try
            //{
            //    bw = new BinaryWriter(new FileStream("mydata.bin", FileMode.Create));
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine(e.Message + "\n Cannot create file.");
            //    return;
            //}
            //writing into the file
            //try
            //{
            //    bw.Write("");
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine(e.Message + "\n Cannot reset Database.");
            //    return;
            //}
            //bw.Close();
        }
        private static void RZPrintMyResume(ref RZPersonalInfo[] info)
        {
            Console.SetWindowSize(78, 33);
            Console.SetBufferSize(78, 33);
            string rz_name = "Rustam";
            string rz_surname = "Zak";
            string rz_patronymic = "As";
            int center_x = 23, center_y = 0;

            Console.Clear();

            Console.WriteLine($"Name: {rz_name}");
            Console.WriteLine($"Surname: {rz_surname}");
            Console.WriteLine($"Patronymic: {rz_patronymic}");

            Console.WriteLine("----------------");
            SetColorBlue();
            Console.WriteLine("Personal Info");
            Console.ResetColor();

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Address:");
            Console.ResetColor();
            Console.WriteLine("1123 Khudu Mammadov");
            Console.WriteLine("Xatai dis. Baku city");

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Phone:");
            Console.ResetColor();
            Console.WriteLine("+994 55 615-51-80");
            Console.WriteLine("+994 77 270-99-23");

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("E-Mail:");
            Console.ResetColor();
            Console.WriteLine("rostik055@gmail.com");
            Console.WriteLine("rostik055@mail.ru");

            Console.WriteLine("----------------");
            SetColorBlue();
            Console.WriteLine("Skills");
            Console.ResetColor();

            Console.WriteLine("----------------");
            SetColorBlue();
            Console.WriteLine("Software");
            Console.ResetColor();

            Console.WriteLine("MS Office Word");
            Console.Write("|");
            SetColorBlue();
            Console.Write("      ");
            Console.ResetColor();
            Console.WriteLine("    | 60%");

            Console.WriteLine("MS Office Excel");
            Console.Write("|");
            SetColorBlue();
            Console.Write("       ");
            Console.ResetColor();
            Console.WriteLine("   | 70%");

            Console.WriteLine("MS Office Power Point");
            Console.Write("|");
            SetColorBlue();
            Console.Write("      ");
            Console.ResetColor();
            Console.WriteLine("    | 60%");

            Console.WriteLine("My SQL");
            Console.Write("|");
            SetColorBlue();
            Console.Write("     ");
            Console.ResetColor();
            Console.WriteLine("     | 50%");

            Console.WriteLine("MS SQL");
            Console.Write("|");
            SetColorBlue();
            Console.Write("     ");
            Console.ResetColor();
            Console.WriteLine("     | 50%");

            Console.SetCursorPosition(center_x, center_y);
            Console.Write("   IT Professional with 1 year of experience");
            Console.SetCursorPosition(center_x, center_y + 1);
            Console.Write("   specializing in IT department management");
            Console.SetCursorPosition(center_x, center_y + 2);
            Console.Write("   for international trading companies.");

            SetColorBlue();
            Console.Write("EXPERIENCE");
            Console.ResetColor();

            Console.WriteLine();

            for (int i = 0; i < 28; i++)
            {
                SetTextCoord("|", center_x, center_y + i, 1);
            }

            SetTextCoord("Программист — специалист, " +
               "занимающийся непосредственной разработкой программного " +
               "обеспечения для различного рода вычислительно-операционных " +
               "систем.", center_x + 3, center_y + 7, 50);

            SetTextCoord("Программирование, как род занятий, " +
                "может являться основной профессиональной " +
                "деятельностью специалиста, либо использоваться " +
                "в качестве вспомогательной деятельности для " +
                "решения иных профессиональных задач, либо же " +
                "использоваться в непрофессиональной сфере" +
                "(как инструмент решения задач или ради получения " +
                "удовольствия от процесса программирования)." +
                "Термин «программист» не обязательно подразумевает " +
                "профессиональное образование или профессиональную " +
                "деятельность.", center_x + 3, center_y + 11, 50);
            /*
                        01.03.02 «Прикладная математика и информатика»
                        02.03.02 «Фундаментальная информатика и информационные технологии»
                        02.03.03 «Математическое обеспечение и администрирование информационных систем»
                        09.02.03 «Программирование в компьютерных системах»
                        09.03.01 «Информатика и вычислительная техника»[1]
                        09.03.02 «Информационные системы и технологии»
                        09.03.03 «Прикладная информатика»
                        09.03.04 «Программная инженерия»[3]
                        10.03.01 «Информационная безопасность»
                        10.05.03 «Информационная безопасность автоматизированных систем»
                        38.03.05 «Бизнес-информатика»
            */
            Console.SetCursorPosition(center_x, center_y + 27);
            ConsoleKeyInfo cki;
            cki = Console.ReadKey();
            if (cki.Key == ConsoleKey.Escape)
            {
                RZMenyu(ref info);
            }
            else RZPrintMyResume(ref info);
        }
    }
}
