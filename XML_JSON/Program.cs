using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Schema;

namespace XML_JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to json and xml parser for library archive!");
            Console.Write("Enter validation type (DTD/Schema): ");
            var input = Console.ReadLine();
            if (!Enum.TryParse<Validation>(input, out var typeValidation))
            {
                Console.WriteLine("Specified validation type is not valid entry");
                return;
            }
            XmlParser parser = null;
            try
            {
                parser = new XmlParser("books.xml", typeValidation, "books.xsd");
            }
            catch (FileNotFoundException file)
            {
                
            }
            if (parser != null)
            {
                Console.WriteLine("Xml parse result:");
                parser.Read();
                parser.DisplayErrors();
                Console.WriteLine();
            }
            

            try
            {
                JsonParser parserJson = new JsonParser("books.json");
                Console.WriteLine("Xml parse result:");
                parserJson.Read();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Json is not valid library archive file: " + e.Message);
            }
            catch (JsonException jsonException)
            {
                Console.WriteLine("File is not a valid Json " + jsonException.Message);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine(fileNotFoundException.Message);
            }


        }
        
    }
}