// See https://aka.ms/new-console-template for more information
using System.Text;


internal class Program
{
    private static void Main(string[] args)
    {       
        Console.Write("Введите путь до папки:");
        string directoryPath = Console.ReadLine();

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

    private static void CheckFilesInDirectoryRecursively(string directoryPath)
    {
        foreach (string fileName in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
        {
            FileInfo fileInfo = new FileInfo(fileName);
            DateTime lastWriteTime = fileInfo.LastWriteTime;
            TimeSpan timeSinceLastModified = DateTime.Now - lastWriteTime;

            /* Отладка
            Console.WriteLine($"Last write time: {lastWriteTime}");
            Console.WriteLine($"Current time: {DateTime.Now}");
            Console.WriteLine($"Time since last modification: {timeSinceLastModified}");
            */

            if (timeSinceLastModified >= TimeSpan.FromMinutes(30))
            {
                fileInfo.Delete();
            }
        }

        foreach (string subDirectory in Directory.EnumerateDirectories(directoryPath))
        {
            CheckFilesInDirectoryRecursively(subDirectory);

            Directory.Delete(subDirectory, true);
        }

        Console.WriteLine($"Папка по пути:{directoryPath} очищена от файлов и папок, не использующихся дольше 30 минут");
    }
}
