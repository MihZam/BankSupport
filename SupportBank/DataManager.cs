using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace SupportBank
{
    class DataManager
    {
        private List<string> listDate = new List<string>(),
                             listFrom = new List<string>(),
                             listTo = new List<string>(),
                             listNarrative = new List<string>(),
                             listAmount = new List<string>();

        public List<string> getDate()
        {
            return listDate;
        }

        public List<string> getFrom()
        {
            return listFrom;
        }

        public List<string> getTo()
        {
            return listTo;
        }

        public List<string> getNarrative()
        {
            return listNarrative;
        }

        public List<string> getAmount()
        {
            return listAmount;
        }

        public void readDataCSV(string path)
        {
            using (var reader = new StreamReader(path))
            {
                bool isFirstEntry = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var format = "dd/MM/yyyy";
                    DateTime dateTime;
                    if (DateTime.TryParseExact(values[0], format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out dateTime))
                    {
                        listDate.Add(values[0]);
                    }
                    else if (!isFirstEntry)
                    {
                        throw new Exception($"Date {values[0]} is not in right format");
                    }

                    if (!isFirstEntry)
                    {
                        listFrom.Add(values[1]);
                    }

                    if (!isFirstEntry)
                    {
                        listTo.Add(values[2]);
                    }

                    if (!isFirstEntry)
                    {
                        listNarrative.Add(values[3]);
                    }

                    try
                    {
                        double test = Convert.ToDouble(values[4]);
                        listAmount.Add(values[4]);
                    }
                    catch
                    {
                        if (!isFirstEntry)
                        {
                            throw new Exception($"Amount {values[4]} is not in double format");
                        }
                    }

                    isFirstEntry = false;
                }
            }
        }

        public void readDataJSON(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var jsonString = reader.ReadToEnd();
                List<JsonData> jsonDataList = JsonConvert.DeserializeObject<List<JsonData>>(jsonString);

                bool isFirstEntry = true;
                foreach (var jsonData in jsonDataList) {
                    DateTime dateTime = new DateTime();
                    if (DateTime.TryParse(jsonData.Date, out dateTime))
                    {
                        listDate.Add(jsonData.Date);
                    }
                    else if (!isFirstEntry)
                    {
                        throw new Exception($"Date {jsonData.Date} is not in right format");
                    }

                    if (!isFirstEntry)
                    {
                        listFrom.Add(jsonData.FromAccount);
                    }

                    if (!isFirstEntry)
                    {
                        listTo.Add(jsonData.ToAccount);
                    }

                    if (!isFirstEntry)
                    {
                        listNarrative.Add(jsonData.Narrative);
                    }

                    try
                    {
                        double test = Convert.ToDouble(jsonData.Amount);
                        listAmount.Add(jsonData.Amount);
                    }
                    catch
                    {
                        if (!isFirstEntry)
                        {
                            throw new Exception($"Amount {jsonData.Amount} is not in double format");
                        }
                    }

                    isFirstEntry = false;
                }
            }
        }

        public void readDataXML(string path)
        {
            var xml = new XmlDocument();
            xml.Load(path);
            foreach (XmlNode date in xml.DocumentElement.ChildNodes)
            {
                var dateTicksString = date.Attributes["Date"].InnerText;
                try
                {
                    var dateTicks = Convert.ToDouble(dateTicksString);
                    DateTime dateTime = DateTime.FromOADate(dateTicks);
                    listDate.Add(dateTime.ToString());
                }
                catch
                {
                    throw new Exception($"Date {date.Attributes["Date"].InnerText} is not in right format");
                }

                foreach (XmlNode node in date.ChildNodes)
                {
                    if (node.Name == "Description")
                    {
                        listNarrative.Add(node.InnerText);
                    }
                    else if (node.Name == "Value")
                    {
                        try
                        {
                            double test = Convert.ToDouble(node.InnerText);
                            listAmount.Add(node.InnerText);
                        }
                        catch
                        { 
                            throw new Exception($"Amount {node.InnerText} is not in double format");
                        }
                    }
                    else if (node.Name == "Parties")
                    {
                        foreach (XmlNode nameNode in node)
                        {
                            if (nameNode.Name == "From")
                            {
                                listFrom.Add(nameNode.InnerText);
                            }
                            else if (nameNode.Name == "To")
                            {
                                listTo.Add(nameNode.InnerText);
                            }
                        }
                    }
                }
            }
        }
    }
}