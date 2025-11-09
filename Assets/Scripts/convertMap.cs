using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine.Rendering;

public class convertMap : MonoBehaviour
{
    public string filePath;
    public class MapSymbol
    {
        public string id;
        public List<Vector2> coords = new List<Vector2>();
    }
    void Start()
    {
        parseOMAP();
    }
    // ISOM 2017 symbol set (for now)
    List<MapSymbol> parseOMAP()
    {
        List<MapSymbol> omap = new List<MapSymbol>();
        XmlDocument mapFile = new XmlDocument();
        try
        {
            mapFile.Load(filePath);
            print("success");
        }
        catch (System.Exception e)
        {
            print($"Failed {e.Message}");
        }
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(mapFile.NameTable);
        nsmgr.AddNamespace("omap", "http://openorienteering.org/apps/mapper/xml/v2");

        XmlNodeList contentPart = mapFile.SelectNodes("//omap:parts/omap:part", nsmgr);
        foreach (XmlNode contentObject in contentPart)
        {
            XmlNodeList symbolNodes = contentObject.SelectNodes("omap:objects/omap:object", nsmgr);
            foreach (XmlNode symbolNode in symbolNodes)
            {
                MapSymbol symbol = ParseObject(symbolNode); // todo
                if (symbol != null && symbol.coords.Count > 0)
                {
                    omap.Add(symbol);
                }
            }
        }
        return omap;
    }
    MapSymbol ParseObject(XmlNode symbolNode)
    {
        MapSymbol symbol = new MapSymbol();
        string id = symbolNode.Attributes["symbol"].Value;
        symbol.id = id;
        XmlNode coordsNode = symbolNode.SelectSingleNode("coords");
        if(coordsNode != null)
        {
            string coordsString = coordsNode.InnerText.Trim();
            if(!string.IsNullOrEmpty(coordsString))
            {
                string[] coordPairs = coordsString.Split(';');
                foreach(string pair in coordPairs)
                {
                    string trimmedPair = pair.Trim();
                    if (string.IsNullOrEmpty(trimmedPair))
                        continue;
                    string[] coords = trimmedPair.Split(' ');
                    if(coords.Length == 2)
                    {
                        if (float.TryParse(coords[0], out float x) && float.TryParse(coords[1], out float y))
                            symbol.coords.Add(new Vector2(x / 10000f, y / 10000f));
                    }
                }
            }
        }
        return symbol;
    }
    void Update()
    {
        
    }
}
