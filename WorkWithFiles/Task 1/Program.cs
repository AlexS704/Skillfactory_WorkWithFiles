// See https://aka.ms/new-console-template for more information


string tempFile = @"C:\\Users\Александр\Desktop\Test_papka\dada.txt"; // Получаем путь до папки.
var fileInfo = new FileInfo(tempFile); // Создаем объект класса FileInfo.


try
{
    //Удаляем ранее созданный файл.
    fileInfo.Delete();
    Console.WriteLine($"{tempFile} удален.");
}
catch (Exception e)
{
    Console.WriteLine($"Ошибка: {e}");
}
