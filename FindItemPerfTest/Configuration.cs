using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FindItemPerfTest
{
    public class Configuration
    {
        private Configuration() { }

        public int Repetitions { get; set; }
        public int SearchCount { get; set; }
        public string[] ToSearch { get; set; }
        public string[] ToFind { get; set; }

        public static Configuration LoadConfiguration(string path)
        {
            XmlReaderSettings xrs = new XmlReaderSettings()
            {
                CloseInput = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };

            int repetitions = 3, searchCount = 1000;
            List<string> searchList = new List<string>();
            List<string> searchTerms = new List<string>();

            bool inConfig = false, inRepetition = false, inSearchCount = false;
            bool inSearchList = false, inSearchTerms = false, inItem = false;


            using (FileStream fs = File.OpenRead(path))
            using (XmlReader xr = XmlReader.Create(fs, xrs))
            {
                while (xr.Read())
                {
                    switch (xr.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xr.Name)
                            {
                                case "repetitions":
                                    inRepetition = inConfig;
                                    break;

                                case "searchCount":
                                    inSearchCount = inConfig;
                                    break;

                                case "settings":
                                    inConfig = true;
                                    break;

                                case "searchList":
                                    inSearchList = true;
                                    break;

                                case "searchTerms":
                                    inSearchTerms = true;
                                    break;

                                case "item":
                                    inItem = inSearchList || inSearchTerms;
                                    break;
                            }

                            break;

                        case XmlNodeType.EndElement:
                            switch (xr.Name)
                            {
                                case "repetitions":
                                    inRepetition = false;
                                    break;

                                case "searchCount":
                                    inSearchCount = false;
                                    break;

                                case "settings":
                                    inRepetition = false;
                                    inSearchCount = false;
                                    inConfig = false;
                                    break;

                                case "searchList":
                                    inItem = false;
                                    inSearchList = false;
                                    break;

                                case "searchTerms":
                                    inItem = false;
                                    inSearchTerms = false;
                                    break;

                                case "item":
                                    inItem = false;
                                    break;
                            }

                            break;

                        case XmlNodeType.Text:
                            if (inConfig)
                            {
                                if (inRepetition)
                                {
                                    int.TryParse(xr.Value, out repetitions);
                                }
                                else if (inSearchCount)
                                {
                                    int.TryParse(xr.Value, out searchCount);
                                }
                            }
                            else if (inSearchTerms && inItem)
                            {
                                searchTerms.Add(xr.Value);
                            }
                            else if (inSearchList && inItem)
                            {
                                searchList.Add(xr.Value);
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            return new Configuration()
            {
                Repetitions = repetitions,
                SearchCount = searchCount,
                ToFind = searchTerms.ToArray(),
                ToSearch = searchList.ToArray()
            };
        }

    }
}
