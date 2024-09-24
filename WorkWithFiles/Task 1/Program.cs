// See https://aka.ms/new-console-template for more information



using System.Security.Cryptography.X509Certificates;

string tempDirectory = @"C:\\Users\pogorelov.aleksandr\Desktop\Новая папка";


if (Directory.Exists(tempDirectory))
    {
        string[] dirs = Directory.GetDirectories(tempDirectory);
        //не работает
        foreach (string d in dirs)
        {                       
            Console.WriteLine(d);

            DateTime dateTime = File.GetLastWriteTime(d);
            Console.WriteLine(dateTime);
            var flieInfo = new FileInfo(d);

            if (dateTime < DateTime.Now)
            {
                flieInfo.Delete();
            }

    }

        
        
                                                                            
        string[] files = Directory.GetFiles(tempDirectory);
        foreach(string f in files)
        {
            Console.WriteLine(f);
        }
}

    
//try 
//{    
//    directoryInfo.Delete(true);
//    Console.WriteLine($"{tempDirectory} удален.");
//}
//catch (Exception e)
//{
//    Console.WriteLine($"Ошибка: {e}");
//}

