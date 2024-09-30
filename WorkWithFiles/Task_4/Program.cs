using System.Text;
using System.Globalization;

namespace Task_4
{
    /// <summary>
    /// Написать программу-загрузчик данных из бинарного формата в текст.
    /// На вход программа получает бинарный файл, предположительно, это база данных студентов.
    /// Свойства сущности Student:
    /// *   Имя — Name (string);
    /// *   Группа — Group (string);
    /// *   Дата рождения — DateOfBirth (DateTime).
    /// *   Средний балл — (decimal).
    /// Программа должна:
    /// *   Cчитать данные о студентах из файла;
    /// *   Создать на рабочем столе директорию Students.
    /// *   Внутри раскидать всех студентов из файла по группам (каждая группа-отдельный текстовый файл), 
    /// в файле группы студенты перечислены построчно в формате "Имя, дата рождения, средний балл".
    /// </summary>

    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageGrade { get; set; }

        //public void ParseGroup(string groupString)
        //{
        //    int parsedGroup;
        //    if (int.TryParse(groupString, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedGroup))
        //    {     
        //       this.Group = parsedGroup.ToString();
        //    }
        //    else
        //    {
        //        throw new FormatException($"Invalid format for group value: {groupString}");
        //    }
        //}                 
    }   
    internal class Program
    {
        static void Main(string[] args)
        {
            const string inputFileName = @"C:\\Users\Александр\Desktop\Write\students.dat";
            const string studentsDirectoryName = "Students";

            List<Student> students = ReadStudentsFromBinFile(inputFileName);

            CreatesStudentsDirectory(studentsDirectoryName);
            WriteStudentsToTextFiles(students, studentsDirectoryName);

        }
        private static List<Student> ReadStudentsFromBinFile(string inputFileName)
        {
            using (var fs = new FileStream(inputFileName, FileMode.Open))
            using (var br = new BinaryReader(fs))
            {
                List<Student> students = new List<Student>();
                while (true)
                {
                    try
                    {
                        long ticks = br.ReadInt64();
                        DateTime date = DateTime.FromBinary(ticks);

                        string groupString = br.ReadString();
                        string name = br.ReadString();
                        decimal grade = br.ReadDecimal();

                        Student student = new Student
                        {
                            Name = name,
                            DateOfBirth = date,
                            AverageGrade = grade
                        };
                        //student.ParseGroup(groupString);
                        students.Add(student);
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                }
                return students;
            }
        }
        /// <summary>
        /// Cоздает директорию Students на рабочем столе,
        /// если она еще не существует
        /// </summary>
        /// <param name="studentsDirectoryName"></param>
        private static void CreatesStudentsDirectory(string studentsDirectoryName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fullPath = Path.Combine(path, studentsDirectoryName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }            
        }
        /// <summary>
        /// Записывает данные студентов в текстовые файлы по группам.
        /// Группирует студентов по группам, создает отдельные текстовые файлы для каждой группы
        /// и записывает данные студентов в эти файлы.
        /// </summary>
        /// <param name="students"></param>
        /// <param name="studentsDirectoryName"></param>
        private static void WriteStudentsToTextFiles(List<Student> students, string studentsDirectoryName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fullPath = Path.Combine(path, studentsDirectoryName);

            //групперуем студентов по группам
            Dictionary<string, List<Student>> groupedStudents = students.GroupBy(s => s.Group).Select(g => g.ToList()).ToDictionary(list => list.First().Group, List => List);

            foreach (var group in groupedStudents)
            {
                string groupFileName = $"Group_{group.Key}.txt";
                string fullGroupPath = Path.Combine(fullPath, groupFileName);
                using (var sw = new StreamWriter(fullGroupPath))
                {
                    foreach (var student in group.Value)
                    {
                        sw.WriteLine($"{student.Name}, {student.DateOfBirth:d}, {student.AverageGrade:F2}");
                    }
                }
            }

            //foreach (var group in students.GroupBy(s => s.Group))
            //{
            //    string groupFilename = $"Group_{group.Key}.txt";
            //    string fullGroupPath = Path.Combine(path, groupFilename);
            //    using (var sw = new StreamWriter(fullGroupPath))
            //    {
            //        foreach (var student in group)
            //        {
            //            sw.WriteLine($"{student.Name}, {student.DateOfBirth:d}, {student.AverageGrade:F2}");
            //        }
            //    }
            //} 
        }
    }
}
