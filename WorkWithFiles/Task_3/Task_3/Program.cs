/// <summary>
/// Задание 3. 
/// Показать, сколько весит папка до очистки. Использовать метод из задания 2
/// Выполнить очистку.
/// Показать сколько файлов удалено и сколько места освобождено.
/// Показать, сколько папка весит после очистки.
/// </summary>

namespace Task_3
{    
    internal class Program
    {
        static void Main(string[] args)
        {
           
            if (args.Length > 0)
            {
                string directoryPath = args[0];
                
                try
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        throw new IOException($"Папка '{directoryPath}' не существует.");
                    }

                    CheckFilesInDirectoryRecursively(directoryPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            else
            {
                Console.WriteLine("Необходимо передать путь к директории в качестве аргумента командной строки.");
            }          

        }
        /// <summary>
        /// Метод для очистке данных, не использующихся >30 минут
        /// </summary>
        /// <param name="directoryPath"></param>
        private static void CheckFilesInDirectoryRecursively(string directoryPath)
        {
            long size = GetDirectorySize(directoryPath);
            Console.WriteLine($"Исходный размер папки: {size} байт");
            
            int numberDeletedFiles = 0;
            foreach (string fileName in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                TimeSpan timeSinceLastModified = DateTime.Now - lastWriteTime;
                

                if (timeSinceLastModified >= TimeSpan.FromMinutes(30))
                {
                    fileInfo.Delete();
                    ++numberDeletedFiles;
                }
            }

            int numberDeletedDirectory = 0;
            foreach (string subDirectory in Directory.EnumerateDirectories(directoryPath))
            {
                CheckFilesInDirectoryRecursively(subDirectory);
                Directory.Delete(subDirectory, true);
                ++numberDeletedDirectory;
            }            
            Console.WriteLine($"Папка по пути:{directoryPath} очищена от {numberDeletedFiles} файлов и {numberDeletedDirectory} папок, не использующихся дольше 30 минут");
            long cleaningSize = GetDirectorySize(directoryPath);
            long resultCleaning = size - cleaningSize;
            
            Console.WriteLine($"Освобождено: {resultCleaning} байт");
            Console.WriteLine($"Текущий размер папки: {cleaningSize} байт");
        }
        /// <summary>
        /// Метод для подсчета размера файлов
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        private static long GetDirectorySize(string directoryPath)
        {
            long size = 0;
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                size += file.Length;
            }

            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                size += GetDirectorySize(subDir.FullName);
            }

            return size;
        }
    }
    
}
