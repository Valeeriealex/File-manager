using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_manager
{
    internal class Program
    {
        private const int PageSize = 2;
        static void Main(string[] args)
        {
            bool isWorking = true;

            while (isWorking)
            {
                ShowMenu();

                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        ShowDirectoryTree();
                        break;

                    case 2:
                        CopyDitectory();
                        break;

                    case 3:
                        CopyFile();
                        break;

                    case 4:
                        DeleteDirectory();
                        break;

                    case 5:
                        DeleteFile();
                        break;

                    case 6:
                        ShowDirectoryInfo();
                        break;

                    case7:
                        ShowFileInfo();
                        break;

                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("1. Вывод древа файловой системы\n2. Копирование каталога" +
                "\n3. Копирование файла\n4. Удаление каталога\n5. Удаление файла" +
                "\n6. Вывод информации о каталоге\n7. Вывод информации о файле");
        }

        private static int GetUserChoice()
        {
            Console.WriteLine("Введите номер выбранного пункта: ");
            int choice;

            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Неверный ввод. Повторите попутку.");
                Console.WriteLine("Введите номер выбранного пункта: ");
            }

            return choice;
        }

        private static void ShowDirectoryTree()
        {
            Console.WriteLine("Введите путь к каталогу: ");
            var path = Console.ReadLine();

            if (Directory.Exists(path))
            {
                var directories = Directory.GetDirectories(path);
                var files = Directory.GetFiles(path);
                var pageCount = (int)Math.Ceiling((double)directories.Length / PageSize);

                for (var i = 0; i < pageCount; i++)
                {
                    Console.WriteLine($"Страница {i + 1}/{pageCount}: ");

                    for (var j = i * PageSize; j < Math.Min((i + 1) * PageSize, directories.Length); j++)
                    {
                        Console.WriteLine(directories[j]);
                    }

                    for (var k = i * PageSize; k < Math.Min((i + 1) * PageSize, files.Length); k++)
                    {
                        Console.WriteLine(files[k]);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Для продолжения нажмите любую клавишу");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Указанный каталог не существует");
            }
        }

        private static void CopyDitectory()
        {
            Console.WriteLine("Введите исходный каталог: ");
            var sourcePath = Console.ReadLine();

            if (Directory.Exists(sourcePath))
            {
                Console.WriteLine("Введите путь каталога назначения: ");
                var destinationPath = Console.ReadLine();

                try
                {
                    Directory.CreateDirectory(destinationPath);
                    var directories = Directory.GetDirectories(sourcePath);
                    var files = Directory.GetFiles(sourcePath);

                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        var destinationFile = Path.Combine(destinationPath, fileName);
                        File.Copy(file, destinationFile);
                    }

                    foreach (var directory in directories)
                    {
                        var directoryName = Path.GetFileName(directory);
                        var destinationDirectory = Path.Combine(destinationPath, directoryName);
                        CopyDirectoryRecursively(directory, destinationDirectory);
                    }

                    Console.WriteLine("Каталог скопирован");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка при копировании: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Исходный каталог не существует");
            }
        }

        private static void CopyDirectoryRecursively(string sourceDirectory, string destinationDirectory)
        {
            var files = Directory.GetFiles(sourceDirectory);
            var directories = Directory.GetDirectories(sourceDirectory);
            Directory.CreateDirectory(destinationDirectory);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destinationFile = Path.Combine(destinationDirectory, fileName);
                File.Copy(file, destinationFile);
            }

            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory);
                var destinationSubdirectory = Path.Combine(destinationDirectory, directoryName);
                CopyDirectoryRecursively(directory, destinationSubdirectory);
            }
        }

        private static void CopyFile()
        {
            Console.WriteLine("Введите путь к исходному файлу: ");
            var courceFile = Console.ReadLine();

            if (File.Exists(courceFile))
            {
                Console.WriteLine("Введите путь назначения файла: ");
                var destinationFile = Console.ReadLine();

                try
                {
                    File.Copy(courceFile, destinationFile);
                    Console.WriteLine("Файл успешно скопирован");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка копирования: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Исходный файл не существует");
            }
        }

        private static void DeleteDirectory()
        {
            Console.WriteLine("Введите путь к удаляемому каталогу: ");
            var directoryPath = Console.ReadLine();

            if (Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.Delete(directoryPath, true);
                    Console.WriteLine("Каталог успешно удален");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка удаления: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Каталог не существует");
            }
        }

        private static void DeleteFile()
        {
            Console.WriteLine("Введите путь к удаляемому файлу: ");
            var filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine("Файл успешно скопирован");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка при удалении файла: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Указанный файл не существует");
            }
        }

        private static void ShowDirectoryInfo()
        {
            Console.WriteLine("Введите путь к каталогу: ");
            var directoryPath = Console.ReadLine();

            if (Directory.Exists(directoryPath))
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                Console.WriteLine($"Имя каталога: {directoryInfo.Name}");
                Console.WriteLine($"Путь к каталогу:{directoryInfo.FullName}");
                Console.WriteLine($"Дата создания: {directoryInfo.CreationTime}");
                Console.WriteLine($"Дата последнего обновления: {directoryInfo.LastWriteTime}");
                Console.WriteLine($"Количество файлов: {Directory.GetFiles(directoryPath).Length}");
                Console.WriteLine($"Количество каталогов: {Directory.GetDirectories(directoryPath).Length}");
            }
            else
            {
                Console.WriteLine("Каталог не существует");
            }
        }

        private static void ShowFileInfo()
        {
            Console.WriteLine("Введите путь к файлу: ");
            var filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                Console.WriteLine($"Имя файлв: {fileInfo.Name}");
                Console.WriteLine($"Полный путь к файлу: {fileInfo.FullName}");
                Console.WriteLine($"Размер файла: {fileInfo.Length} байт");
                Console.WriteLine($"Дата создания: {fileInfo.CreationTime}");
                Console.WriteLine($"Дата последнего обновления: {fileInfo.LastWriteTime}");
            }
            else
            {
                Console.WriteLine("Указанный файл не существует");
            }
        }
    }
}
