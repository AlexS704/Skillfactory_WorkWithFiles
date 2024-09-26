/// <summary>
/// Напишите программу, которая считает размер папки на диске (вместе со всеми вложенными папками и файлами). 
/// На вход метод принимает URL директории, в ответ — размер в байтах.
/// </summary>

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Введите путь до папки: ");
        string directoryPath = Console.ReadLine();

        try
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new IOException($"Директория '{directoryPath}' не существует.");
            }

            long size = GetDirectorySize(directoryPath);

            Console.WriteLine($"Общий размер директории: {size} байт");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        
    }
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