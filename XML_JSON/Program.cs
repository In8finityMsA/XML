using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XML_JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlParser parser = new XmlParser("books.xml", Validation.Schema, "books.xsd");
            parser.Read();
            parser.DisplayErrors();




        }
        
    }
}