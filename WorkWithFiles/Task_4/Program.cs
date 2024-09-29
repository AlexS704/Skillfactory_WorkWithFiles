using System.Text;

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
        public int Group { get; set; }
        public DateTime Birthdate { get; set; }
        public double AverageGrade { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            const string inputFileName = @"C:\\Users\Александр\Desktop\BinWrite\students.dat";
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
                        int birthdateLength = br.Read7BitEncodedInt();
                        byte[] birthdateBytes = br.ReadBytes(birthdateLength);
                        long ticks = BitConverter.ToInt64(birthdateBytes, 0);
                        if (DateTime.MinValue.Ticks <= ticks && ticks <= DateTime.MaxValue.Ticks)
                        {
                            DateTime birthdate = new DateTime(ticks);

                            Student student = new Student
                            {
                                Name = br.ReadString(),
                                Group = br.ReadInt32(),
                                Birthdate = birthdate,
                                AverageGrade = br.ReadDouble()
                            };
                            students.Add(student);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("ticks", "Value out of range for DateTime.");
                        }
                        
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                }
                return students;
            }
        }
        private static void CreatesStudentsDirectory(string studentsDirectoryName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fullPath = Path.Combine(path, studentsDirectoryName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }            
        }

        private static void WriteStudentsToTextFiles(List<Student> students, string studentsDirectoryName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fullPath = Path.Combine(path, studentsDirectoryName);

            foreach (var group in students.GroupBy(s => s.Group))
            {
                string groupFileName = $"Group_{group.Key}.txt";
                string fullGroupPath = Path.Combine(fullPath, groupFileName);
                using (var sw = new StreamWriter(fullGroupPath))
                {
                    foreach (var student in group)
                    {
                        sw.WriteLine($"{student.Name}, {student.Birthdate:d}, {student.AverageGrade:F2}");
                    }
                }
            }
        }

    }
}
