using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine.Rendering;

public class convertMap : MonoBehaviour
{
    public class isomSymbol
    {
        public int id;
        public int type;
        public GameObject symbolObject;
    }
    // https://omapwiki.orienteering.sport/specifications/isom/
    public GameObject contour; //101
    public GameObject indexContoure; //102
    public GameObject formLine; //103
    public GameObject earthBank; //104
    public GameObject earthWall; //1051
    public GameObject retainingEarthWall; //1052
    public GameObject ruinedEarthWall; //106
    public GameObject erosionGully; //107
    public GameObject smallErosionGully; //108
    public GameObject smallKnoll; //109
    public GameObject smallElongatedKnoll; //110
    public GameObject smallDepression; //111
    public GameObject pit; //112
    public GameObject brokenGround; //113
    public GameObject veryBrokenGround; //114
    public GameObject prominentLandformFeature; //115
    public GameObject impassableCliff; //201
    public GameObject cliff; //202
    public GameObject rockyPit; //2031
    public GameObject dangerousPit; //2032
    public GameObject Boulder; //204
    public GameObject largeBoulder; //205
    public GameObject giganticBoulder; //206
    public GameObject boulderCluster; //207
    public GameObject boulderField; //208
    public GameObject denseBoulderField; //209
    public GameObject stonyGroundRun; //210
    public GameObject stonyGroundWalk; //211
    public GameObject stonyGroundFight; //212
    public GameObject sandyGround; //213
    public GameObject bareRock; //214
    public GameObject trench; //215
    public GameObject uncrossableBodyOfWater; //301
    public GameObject shallowBodyOfWater; //302
    public GameObject watterhole; //303
    public GameObject crossableWatercourse; //304
    public GameObject smallCrossableWatercourse; //305
    public GameObject seasonalWaterChannel; //306
    public GameObject uncrossableMarsh; //307
    public GameObject marsh; //308
    public GameObject narrowMarsh; //309
    public GameObject indistinctMarsh; //310
    public GameObject well; //311
    public GameObject spring; //312
    public GameObject prominentWaterFeature; //313
    public GameObject openLand; //401
    public GameObject openLandWithTrees; //402
    public GameObject roughOpenLand; //403
    public GameObject roughOpenLandWithTrees; //404
    public GameObject forest; //405
    public GameObject vegetationSlow; //406
    public GameObject vegetationSlowGoodVis; //407
    public GameObject vegetationWalk; //408
    public GameObject vegetationWalkGoodVis; //409
    public GameObject vegetationFight; //410
    public GameObject cultivatedLand; //412
    public GameObject orchard; //413
    public GameObject vineyard; //414
    public GameObject cultivationBoundary; //415
    public GameObject distinctVegetationBoundary; //416
    public GameObject prominentLargeTree; //417
    public GameObject prominentSmallTree; //418
    public GameObject prominentVegetationFeature; //419
    public GameObject pavedArea; //501
    public GameObject wideRoad; //502
    public GameObject road; //503
    public GameObject vehicleTrack; //504
    public GameObject footpath; //505
    public GameObject smallFootpath; //506
    public GameObject lessDistinctSmallFootpath; //507
    public GameObject linearTrace; //508
    public GameObject railway; //509
    public GameObject powerLine; //510
    public GameObject majorPowerLine; //511
    public GameObject bridgeTunnel; //512
    public GameObject wall; //5131
    public GameObject retainingWall; //5132
    public GameObject ruinedWall; //514
    public GameObject impassableWall; //515
    public GameObject fence; //516
    public GameObject ruinedFence; //517
    public GameObject impassableFence; //518
    public GameObject crossingPointFence; //519
    public GameObject areaNotEnter; //520
    public GameObject building; //521
    public GameObject canopy; //522
    public GameObject ruin; //523
    public GameObject highTower; //524
    public GameObject smallTower; //525
    public GameObject cairn; //526
    public GameObject fodderRack; //527
    public GameObject prominentLineFeature; //528
    public GameObject prominentImpassableLineFeature; //529
    public GameObject prominentManMadeFeatureRing; //530
    public GameObject prominentManMadeFeatureX; //531
    public GameObject stairway; //532
    public GameObject magneticNorthLine; //601
    public GameObject registrationMark; //602
    public GameObject spotHeigh; //603
    public GameObject start; //701
    public GameObject mapIssuePoint; //702
    public GameObject controlPoint; //703
    public GameObject controlNumber; //704
    public GameObject courseLine; //705
    public GameObject finish; //706
    public GameObject markedRoute; //707
    public GameObject outOfBoundsBound; //708
    public GameObject outOfBoundsArea; //709
    public GameObject crossingPoint; //710
    public GameObject outOfBoundsRoute; //711
    public GameObject firstAidPost; //712
    public GameObject refreshmentPoint; //713
    public GameObject continuingPoint; //715
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
        if (coordsNode != null)
        {
            string coordsString = coordsNode.InnerText.Trim();
            if (!string.IsNullOrEmpty(coordsString))
            {
                string[] coordPairs = coordsString.Split(';');
                foreach (string pair in coordPairs)
                {
                    string trimmedPair = pair.Trim();
                    if (string.IsNullOrEmpty(trimmedPair))
                        continue;
                    string[] coords = trimmedPair.Split(' ');
                    if (coords.Length == 2)
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
