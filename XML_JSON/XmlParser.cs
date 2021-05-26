using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XML_JSON
{
    
    public enum Validation { DTD, Schema }
    public class XmlParser
    {
        private XmlReader reader;
        private List<string> errors;
        
        public XmlParser(string filename, Validation validation, string schemaFilename = null)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("File can't be found: " + filename);
            }
            
            var stream = new StreamReader(filename);
            XmlReaderSettings settings = new XmlReaderSettings();

            if (validation == Validation.Schema)
            {
                if (schemaFilename != null)
                {
                    settings = SetScheme(schemaFilename);
                }
                else
                {
                    Console.WriteLine("Schema filename is null");
                }
                
            }
            else if (validation == Validation.DTD)
            {
                settings = EnableDTD(true);
            }

            settings.ValidationEventHandler += OnError;
            reader = XmlReader.Create(stream, settings);
        }

        public void Read()
        {
            if (reader == null)
            {
                Console.WriteLine("An error occured");
            }

            errors = new List<string>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        Console.WriteLine($"<{reader.Name}> contains {reader.AttributeCount} attribute(s)");
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToNextAttribute();
                            Console.WriteLine($"Attribute name: {reader.Name}, attribute value: {reader.Value}");
                        }
                        reader.MoveToElement();
                        break;
                    }
                    case XmlNodeType.Text:
                    {
                        Console.WriteLine($"Text: {reader.Value}");
                        break;
                    }
                    case XmlNodeType.EndElement:
                    {
                        Console.WriteLine($"</{reader.Name}>");
                        break;
                    }
                    case XmlNodeType.XmlDeclaration:
                    {
                        Console.WriteLine($"This document contains XML declaration: {reader.Value}");
                        break;
                    }
                }
            }
        }

        public void DisplayErrors()
        {
            if (errors.Count == 0)
            {
                Console.WriteLine("There is no errors!");
            }
            else
            {
                Console.WriteLine("Errors in supplied file: ");
            }
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }

        public XmlReaderSettings SetScheme(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File can't be found: " + filename);
            }
            
            List<string> errorsSchema = new List<string>();
            var stream = new StreamReader(filename);
            var schema = XmlSchema.Read(stream, (sender, validationEventArgs) =>
                errorsSchema.Add(validationEventArgs.Message)
            );

            if (errorsSchema.Count > 0)
            {
                Console.WriteLine("Your Scheme contains errors!");
                foreach (var error in errorsSchema)
                {
                    Console.WriteLine(error);
                }

                return null;
            }
            
            var settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(schema);

            return settings;
        }

        public XmlReaderSettings EnableDTD(bool isUseDTD)
        {
            var settings = new XmlReaderSettings();
            if (isUseDTD)
            {
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.ValidationType = ValidationType.DTD;
            }
            else
            {
                settings.DtdProcessing = DtdProcessing.Ignore;
            }

            return settings;
        }

        private void OnError(object sender, ValidationEventArgs e)
        {
            errors.Add(e.Message);
        }
    }
}