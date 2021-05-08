using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace AllMyMusic
{   
    public class CountryCollection : List<CountryItem>
    {
        // Date: 26 January 2011
        // Name: Erwin Kryszat
        // code review session

        private String configFilename = String.Empty;
        public CountryCollection()
        {
        }
        public CountryCollection(String ConfigFilename)
        {
            configFilename = ConfigFilename;
            XmlDocument doc = new XmlDocument();

            if (File.Exists(configFilename) == true)
            {
                doc.Load(configFilename);
                XmlElement nodeCountries = doc.DocumentElement;

                foreach (XmlNode nodeCountry in nodeCountries.ChildNodes)
                {
                    CountryItem Country = new CountryItem();

                    Country.Country = nodeCountry.Attributes["Name"].InnerText;
                    Country.Abbreviation = nodeCountry.Attributes["Abbreviation"].InnerText;
                    Country.FlagPath = nodeCountry.Attributes["FlagPath"].InnerText;

                    this.Add(Country);
                }
            }
        }
        public void UpdateFileFromDatabase(CountryCollection countriesCollectionDatabase)
        {
            foreach (CountryItem country in countriesCollectionDatabase)
            {
                this.UpdateCountry(country);
            }
            Save();
        }
        public void UpdateCountry( CountryItem country)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Country == country.Country)
                {
                    this[i] = country;
                    return;
                }
            }

            // Add Node
            this.Add(country);
        }
        public void Save()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement nodeCountries = doc.CreateElement("Countries");
            doc.AppendChild(nodeCountries);

            for (int i = 0; i < this.Count; i++)
            {
                // Create XML Attributes
                XmlAttribute attName = doc.CreateAttribute("Name");
                XmlAttribute attAbbreviation = doc.CreateAttribute("Abbreviation");
                XmlAttribute attFlagPath = doc.CreateAttribute("FlagPath");

                // Assign Data Values to the Attributes
                attName.InnerText = this[i].Country;
                attAbbreviation.InnerText = this[i].Abbreviation;
                attFlagPath.InnerText = this[i].FlagPath;

                XmlElement nodeCountry = doc.CreateElement("Country");
                nodeCountries.AppendChild(nodeCountry);

                nodeCountry.Attributes.Append(attName);
                nodeCountry.Attributes.Append(attAbbreviation);
                nodeCountry.Attributes.Append(attFlagPath);
            }
            doc.Save(configFilename);
        }

        public String GetAbbreviation(String countryName)
        {
            foreach (CountryItem country in this)
            {
                if (country.Country == countryName)
                {
                    return country.Abbreviation;
                }
            }
            return String.Empty;
        }
    }


}
