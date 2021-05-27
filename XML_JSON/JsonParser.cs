using System;
using System.IO;
using System.Text.Json;

namespace XML_JSON
{
    public class JsonParser
    {

        private JsonDocument document;
        
        public JsonParser(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("File can't be found: " + filename);
            }

            using (var stream = new StreamReader(filename))
            {
                var jsonString = stream.ReadToEnd();
                document = JsonDocument.Parse(jsonString);
            }
            
        }

        public void Read()
        {
            var root = document.RootElement;
            var library = GetElement(root, "library");

            JsonElement[] places = new JsonElement[3];
            places[0] = GetElement(library, "reading_room");
            places[1] = GetElement(library, "library_loan");
            places[2] = GetElement(library, "reserve");

            foreach (var place in places)
            {
                var books = GetElement(place, "books");
                foreach (var book in books.EnumerateArray())
                {
                    var invenory_number = GetElement(book, "inventory_number");
                    Console.WriteLine($"Inventory number: {invenory_number.GetInt32()}");
                    var name = GetElement(book, "name");
                    CheckStringElement("name", name);
                    Console.WriteLine($"Book name: {name.GetString()}");
                    var isbn  = GetElement(book, "isbn");
                    CheckStringElement("isbn", isbn);
                    Console.WriteLine($"ISBN: {isbn.GetString()}");
                    var authors = GetElement(book, "authors");
                    Console.WriteLine("Authors:");
                    if (authors.ValueKind != JsonValueKind.Array && authors.GetArrayLength() == 0)
                    {
                        throw new ArgumentException("Authors is not array or empty");
                    }
                    foreach (var author in authors.EnumerateArray())
                    {
                        CheckStringElement("author", author);
                        Console.WriteLine("  " + author.GetString());
                    }
                    Console.WriteLine();
                }
            }
                
            
        }

        private void CheckStringElement(string elementName, JsonElement element)
        {
            if (String.IsNullOrWhiteSpace(element.GetString()))
            {
                throw new ArgumentException($"Field {elementName} is empty");
            }
        }

        private JsonElement GetElement(JsonElement element, string elementName)
        {
            if (element.TryGetProperty(elementName, out var foundElement))
            {
                return foundElement;
            }
            else
            {
                throw new ArgumentException($"Element {elementName} is not found or not an object");
            }
            
        }

    }
}