using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO.Compression;
using System.Xml.Linq;

namespace OC_Lab_1;

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Company { get; set; }
}

public static class Functions
{
    public static void ShowFunctions()
    {
        string[] functions = { 
            "1. Вывести информацию о дисках.",
            "2. Работа с файлами.",
            "3. Работа с форматом JSON",
            "4. Работа с форматом XML",
            "5. Работа с zip архивом.",
            "6. Выход из программы."
        };

        for (int i = 0; i < 6; i++)
        {
            Console.WriteLine(functions[i]);
        }
    }

    public static void FinishOfTheFunction()
    {
        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadLine();
    }

    public static void ShowDriveInfo()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();

        foreach (DriveInfo drive in drives)
        {
            Console.WriteLine($"Название: {drive.Name}");
            Console.WriteLine($"Тип: {drive.DriveType}");
            if (drive.IsReady)
            {
                Console.WriteLine($"Объем диска: {drive.TotalSize}");
                Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                Console.WriteLine($"Метка: {drive.VolumeLabel}");
                Console.WriteLine($"Файловая система: {drive.DriveFormat}");
            }
            Console.WriteLine();
        }
    }

    public static async Task WorkWithFile()
    {
        string path = @"C:\Intel\note.txt";

        Console.WriteLine("Введите строку для записи в файл: ");
        string text = Console.ReadLine();
        try
        {
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                await sw.WriteLineAsync(text);
                await sw.WriteLineAsync("Асинхронная дозапись");
                await sw.WriteAsync("1,5");
            }
            Console.WriteLine("Запись выполнена.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("Текст, прочитанный из файла: ");
        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(await sr.ReadToEndAsync());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        File.Delete(path);
    }

    public static async Task WorkWithJSON()
    {
        string path = @"D:\Repos\OC_Lab_1\OC_Lab_1\bin\Debug\net6.0\user.json";

        using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
        {
            Console.WriteLine("Введите имя пользователя: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введите возраст пользователя: ");
            int age = int.Parse(Console.ReadLine());

            Person alex = new Person() { Name = name, Age = age };
            await JsonSerializer.SerializeAsync<Person>(fs, alex);
            Console.WriteLine("Данные были сохранены в файл.");
        }

        using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
        {
            Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
            Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
        }

        File.Delete(path);
    }

    public static void WorkWithXML()
    {
        string path = @"D:\Repos\OC_Lab_1\OC_Lab_1\bin\Debug\net6.0\phones.xml";

        XDocument xdoc = new XDocument();
        XElement iphone6 = new XElement("phone");
        XAttribute iphoneNameAttr = new XAttribute("name", "iPhone 13 mini");
        XElement iphoneCompanyElem = new XElement("company", "Apple");
        XElement iphonePriceElem = new XElement("price", "80000");
        iphone6.Add(iphoneNameAttr);
        iphone6.Add(iphoneCompanyElem);
        iphone6.Add(iphonePriceElem);

        Console.Write("Введите имя новой записи: ");
        string name = Console.ReadLine();
        Console.Write("Введите имя компании: ");
        string company = Console.ReadLine();
        Console.Write("Введите цену: ");
        string price = Console.ReadLine();

        XElement newPhone = new XElement("phone");
        XAttribute nameAttr = new XAttribute("name", name);
        XElement companyElem = new XElement("company", company);
        XElement priceElem = new XElement("price", price);
        newPhone.Add(nameAttr);
        newPhone.Add(companyElem);
        newPhone.Add(priceElem);

        XElement phones = new XElement("phones");
        phones.Add(iphone6);
        phones.Add(newPhone);
        xdoc.Add(phones);
        xdoc.Save("phones.xml");

        Console.WriteLine();
        Console.WriteLine("Информация, содержащаяся в файле phones.xml: ");



        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("name");
                if (attr != null)
                    Console.WriteLine(attr.Value);
            }
            foreach (XmlNode childnode in xnode.ChildNodes)
            {
                if (childnode.Name == "company")
                {
                    Console.WriteLine($"Компания: {childnode.InnerText}");
                }
                if (childnode.Name == "price")
                {
                    Console.WriteLine($"Цена: {childnode.InnerText}");
                }
            }
            Console.WriteLine();
        }
        File.Delete(path);
    }

    public static void WorkWithZIP()
    {
        string sourceFolder = @"D:\Repos\OC_Lab_1\Archive";
        string zipFile = @"D:\Repos\OC_Lab_1\Archive.zip";
        string targetFolder = @"D:\Repos\OC_Lab_1\NewArchive";
        string path = @"D:\Repos\OC_Lab_1\FileToZip.txt";
        string pathToFile = @"D:\Repos\OC_Lab_1\NewArchive\FileToZip.txt";

        ZipFile.CreateFromDirectory(sourceFolder, zipFile);
        Console.WriteLine($"Папка {sourceFolder} архивирована в файл {zipFile}");

        using (FileStream zipToOpen = new FileStream(@"D:\Repos\OC_Lab_1\Archive.zip", FileMode.Open))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                ZipArchiveEntry readmeEntry = archive.CreateEntryFromFile(path, "FileToZip.txt");
                Console.WriteLine($"Файл {path} добавлен в архив {zipFile}");
            }
        }

        ZipFile.ExtractToDirectory(zipFile, targetFolder);
        Console.WriteLine($"Файл {zipFile} распакован в папку {targetFolder}");

        var file = new FileInfo(pathToFile);
        Console.WriteLine("Полное имя файла: " + file.FullName);
        Console.WriteLine("Имя файла: " + file.Name);
        Console.WriteLine("Расширение файла: " + file.Extension);
        Console.WriteLine("Размер файла: " + file.Length);

        File.Delete(pathToFile);
        Console.WriteLine($"Файл {pathToFile} был удалён.");
        File.Delete(zipFile);
        Console.WriteLine($"Файл {zipFile} был удалён.");
    }
}
