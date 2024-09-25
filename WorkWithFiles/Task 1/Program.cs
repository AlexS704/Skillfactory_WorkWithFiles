// See https://aka.ms/new-console-template for more information
using System.Text;


internal class Program
{
    private static void Main(string[] args)
    {
        //string directoryPath = @" C:\Users\Александр\Desktop\Test_papka";
        if (args.Length > 0)
        {
            string directoryPath = args[0];
            //CheckFilesDirectory(directoryPath);
            CheckDirectoriesInDirectory(directoryPath);
        }
        else
        {
            Console.WriteLine("Usage: Task 1 run <directory_path>");
        }

        
    }

    
    private static void CheckDirectoriesInDirectory(string directoryPath)
    {
        foreach (string subDirectory in Directory.EnumerateDirectories(directoryPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(subDirectory);
            DateTime lastWriteTime = directoryInfo.LastWriteTime;
            TimeSpan timeSinceLastModified = DateTime.UtcNow - lastWriteTime;

            if (timeSinceLastModified > TimeSpan.FromMinutes(30))
            {
                directoryInfo.Delete();
            }

            else
            {
                static void CheckFilesDirectory(string directoryPath)
                {
                    foreach (string fileName in Directory.EnumerateFiles(directoryPath))
                    {
                        FileInfo fileInfo = new FileInfo(fileName);
                        DateTime lastWrite = fileInfo.LastWriteTime;
                        TimeSpan timeSinceLastModified = DateTime.UtcNow - lastWrite;

                        if (timeSinceLastModified > TimeSpan.FromMinutes(30))
                        {
                            fileInfo.Delete();
                        }
                    }

                }
            }

            CheckDirectoriesInDirectory(subDirectory);
        }
    }
}
