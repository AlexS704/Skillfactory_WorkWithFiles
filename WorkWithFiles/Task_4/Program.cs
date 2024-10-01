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

                while (fs.Position < fs.Length)
                {
                    try
                    {
                        string name = br.ReadString();
                        string group = br.ReadString();
                        long dateBinary = br.ReadInt64();
                        DateTime dateOfBirth = DateTime.FromBinary(dateBinary);
                        decimal averageScore = br.ReadDecimal();

                        //Добавляем студента в список
                        students.Add(new Student
                        {
                            Name = name,
                            Group = group,
                            DateOfBirth = dateOfBirth,
                            AverageGrade = averageScore
                        });
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                }
                br.Close();
                fs.Close();
                return students;
            }
        }
        /// <summary>
        /// Cоздаем директорию Students на рабочем столе,
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
        /// Записываем данные студентов в текстовые файлы по группам.
        /// Группирует студентов по группам, создает отдельные текстовые файлы для каждой группы
        /// и записывает данные студентов в эти файлы.
        /// </summary>
        /// <param name="students"></param>
        /// <param name="studentsDirectoryName"></param>
        private static void WriteStudentsToTextFiles(List<Student> students, string studentsDirectoryName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fullPath = Path.Combine(path, studentsDirectoryName);
            
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
        }
    }
}
