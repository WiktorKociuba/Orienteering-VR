using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine.Rendering;
using NUnit.Framework.Constraints;
using UnityEditor.Rendering;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using Unity.Mathematics;
using UnityEngine.UIElements;
using UnityEngine.Analytics;
using System.Text.RegularExpressions;
using System;
using System.IO.Compression;
using Valve.VR.InteractionSystem;

public class convertMap : MonoBehaviour
{
    List<MapSymbol> omap;
    float minX = float.MaxValue, minY = float.MaxValue, maxY = float.MinValue, maxX = float.MinValue;
    public Terrain terrain;
    public Material defaultMaterial;
    public class isomSymbol
    {
        public int id; // omap ID
        public int isomId; // ISOM 2017-2 symbol set ID
        public int type;
        /*
            0 - point
            1 - line
            2 - area
        */
        public GameObject symbolObject;
    }
    public class ContourData
    {
        public string id;
        public List<Vector2> coords;
        public bool isClosed;
        public int nestLevel;
        public int slopeDir;
    }
    public class ElevationPoint
    {
        public Vector2 coords;
        public float height;
        public int direction;
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
    public GameObject waterhole; //303
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
    public GameObject openLandWithBushes; //402.1
    public GameObject roughOpenLand; //403
    public GameObject roughOpenLandWithTrees; //404
    public GameObject roughOpenLandWithBushes; //404.1
    public GameObject forest; //405
    public GameObject vegetationSlow; //406
    public GameObject vegetationSlowOneDir; //406.1
    public GameObject vegetationSlowGoodVis; //407
    public GameObject vegetationWalk; //408
     public GameObject vegetationWalkOneDir; //408.1;408.2
    public GameObject vegetationWalkGoodVis; //409
    public GameObject vegetationFight; //410
    public GameObject vegetationFightOneDir; //410.1;410.2;410.3
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
    public TerrainLayer grassLayer;
    public TerrainLayer sandLayer;
    public TerrainLayer rockLayer;
    public TerrainLayer waterLayer;
    public string filePath;
    public class MapSymbol
    {
        public string id;
        public List<Vector2> coords = new List<Vector2>();
        public float rotation;
    }
    public Dictionary<int, isomSymbol> isomSet = new Dictionary<int, isomSymbol>();
    void Start()
    {
        /*
            0 - point
            1 - line
            2 - area
        */
        isomSymbol temp = new isomSymbol();
        temp.id = 0; temp.isomId = 101; temp.type = 1; temp.symbolObject = contour;
        isomSet[0] = temp;
        temp = new isomSymbol();
        temp.id = 1; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[1] = temp;
        temp = new isomSymbol();
        temp.id = 2; temp.isomId = 102; temp.type = 1; temp.symbolObject = indexContoure;
        isomSet[2] = temp;
        temp = new isomSymbol();
        temp.id = 3; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[3] = temp;
        temp = new isomSymbol();
        temp.id = 4; temp.isomId = 103; temp.type = 1; temp.symbolObject = formLine;
        isomSet[4] = temp;
        temp = new isomSymbol();
        temp.id = 5; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[5] = temp;
        temp = new isomSymbol();
        temp.id = 6; temp.isomId = 104; temp.type = 1; temp.symbolObject = earthBank;
        isomSet[6] = temp;
        temp = new isomSymbol();
        temp.id = 7; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[7] = temp;
        temp = new isomSymbol();
        temp.id = 8; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[8] = temp;
        temp = new isomSymbol();
        temp.id = 9; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[9] = temp;
        temp = new isomSymbol();
        temp.id = 10; temp.isomId = 1051; temp.type = 1; temp.symbolObject = earthWall;
        isomSet[10] = temp;
        temp = new isomSymbol();
        temp.id = 11; temp.isomId = 106; temp.type = 1; temp.symbolObject = ruinedEarthWall;
        isomSet[11] = temp;
        temp = new isomSymbol();
        temp.id = 12; temp.isomId = 107; temp.type = 1; temp.symbolObject = erosionGully;
        isomSet[12] = temp;
        temp = new isomSymbol();
        temp.id = 13; temp.isomId = 108; temp.type = 1; temp.symbolObject = smallErosionGully;
        isomSet[13] = temp;
        temp = new isomSymbol();
        temp.id = 14; temp.isomId = 109; temp.type = 0; temp.symbolObject = smallKnoll;
        isomSet[14] = temp;
        temp = new isomSymbol();
        temp.id = 15; temp.isomId = 110; temp.type = 0; temp.symbolObject = smallElongatedKnoll;
        isomSet[15] = temp;
        temp = new isomSymbol();
        temp.id = 16; temp.isomId = 111; temp.type = 0; temp.symbolObject = smallDepression;
        isomSet[16] = temp;
        temp = new isomSymbol();
        temp.id = 17; temp.isomId = 112; temp.type = 0; temp.symbolObject = pit;
        isomSet[17] = temp;
        temp = new isomSymbol();
        temp.id = 18; temp.isomId = 113; temp.type = 2; temp.symbolObject = brokenGround;
        isomSet[18] = temp;
        temp = new isomSymbol();
        temp.id = 19; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[19] = temp;
        temp = new isomSymbol();
        temp.id = 20; temp.isomId = 114; temp.type = 2; temp.symbolObject = veryBrokenGround;
        isomSet[20] = temp;
        temp = new isomSymbol();
        temp.id = 21; temp.isomId = 115; temp.type = 0; temp.symbolObject = prominentLandformFeature;
        isomSet[21] = temp;
        temp = new isomSymbol();
        temp.id = 22; temp.isomId = 201; temp.type = 1; temp.symbolObject = impassableCliff;
        isomSet[22] = temp;
        temp = new isomSymbol();
        temp.id = 23; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[23] = temp;
        temp = new isomSymbol();
        temp.id = 24; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[24] = temp;
        temp = new isomSymbol();
        temp.id = 25; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[25] = temp;
        temp = new isomSymbol();
        temp.id = 26; temp.isomId = 202; temp.type = 1; temp.symbolObject = cliff;
        isomSet[26] = temp;
        temp = new isomSymbol();
        temp.id = 27; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[27] = temp;
        temp = new isomSymbol();
        temp.id = 28; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[28] = temp;
        temp = new isomSymbol();
        temp.id = 29; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[29] = temp;
        temp = new isomSymbol();
        temp.id = 30; temp.isomId = 2031; temp.type = 0; temp.symbolObject = rockyPit;
        isomSet[30] = temp;
        temp = new isomSymbol();
        temp.id = 31; temp.isomId = 2032; temp.type = 0; temp.symbolObject = dangerousPit;
        isomSet[31] = temp;
        temp = new isomSymbol();
        temp.id = 32; temp.isomId = 204; temp.type = 0; temp.symbolObject = Boulder;
        isomSet[32] = temp;
        temp = new isomSymbol();
        temp.id = 33; temp.isomId = 2045; temp.type = 0; temp.symbolObject = Boulder;
        isomSet[33] = temp;
        temp = new isomSymbol();
        temp.id = 34; temp.isomId = 205; temp.type = 0; temp.symbolObject = largeBoulder;
        isomSet[34] = temp;
        temp = new isomSymbol();
        temp.id = 35; temp.isomId = 206; temp.type = 0; temp.symbolObject = giganticBoulder;
        isomSet[35] = temp;
        temp = new isomSymbol();
        temp.id = 36; temp.isomId = 207; temp.type = 0; temp.symbolObject = boulderCluster;
        isomSet[36] = temp;
        temp = new isomSymbol();
        temp.id = 37; temp.isomId = 2071; temp.type = 0; temp.symbolObject = boulderCluster;
        isomSet[37] = temp;
        temp = new isomSymbol();
        temp.id = 38; temp.isomId = 208; temp.type = 2; temp.symbolObject = boulderField;
        isomSet[38] = temp;
        temp = new isomSymbol();
        temp.id = 39; temp.isomId = 2081; temp.type = 2; temp.symbolObject = boulderField;
        isomSet[39] = temp;
        temp = new isomSymbol();
        temp.id = 40; temp.isomId = 2082; temp.type = 2; temp.symbolObject = boulderField;
        isomSet[40] = temp;
        temp = new isomSymbol();
        temp.id = 41; temp.isomId = 209; temp.type = 2; temp.symbolObject = denseBoulderField;
        isomSet[41] = temp;
        temp = new isomSymbol();
        temp.id = 42; temp.isomId = 210; temp.type = 2; temp.symbolObject = stonyGroundRun;
        isomSet[42] = temp;
        temp = new isomSymbol();
        temp.id = 43; temp.isomId = 2101; temp.type = 2; temp.symbolObject = stonyGroundRun;
        isomSet[43] = temp;
        temp = new isomSymbol();
        temp.id = 44; temp.isomId = 211; temp.type = 2; temp.symbolObject = stonyGroundWalk;
        isomSet[44] = temp;
        temp = new isomSymbol();
        temp.id = 45; temp.isomId = 212; temp.type = 2; temp.symbolObject = stonyGroundFight;
        isomSet[45] = temp;
        temp = new isomSymbol();
        temp.id = 46; temp.isomId = 213; temp.type = 2; temp.symbolObject = sandyGround;
        isomSet[46] = temp;
        temp = new isomSymbol();
        temp.id = 47; temp.isomId = 214; temp.type = 2; temp.symbolObject = bareRock;
        isomSet[47] = temp;
        temp = new isomSymbol();
        temp.id = 48; temp.isomId = 215; temp.type = 1; temp.symbolObject = trench;
        isomSet[48] = temp;
        temp = new isomSymbol();
        temp.id = 49; temp.isomId = 301; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet[49] = temp;
        temp = new isomSymbol();
        temp.id = 50; temp.isomId = 3011; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet[50] = temp;
        temp = new isomSymbol();
        temp.id = 51; temp.isomId = 3012; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet[51] = temp;
        temp = new isomSymbol();
        temp.id = 52; temp.isomId = 3013; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet[52] = temp;
        temp = new isomSymbol();
        temp.id = 53; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet[53] = temp;
        temp = new isomSymbol();
        temp.id = 54; temp.isomId = 302; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet[54] = temp;
        temp = new isomSymbol();
        temp.id = 55; temp.isomId = 3021; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet[55] = temp;
        temp = new isomSymbol();
        temp.id = 56; temp.isomId = 3022; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet[56] = temp;
        temp = new isomSymbol();
        temp.id = 57; temp.isomId = 3023; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet[57] = temp;
        temp = new isomSymbol();
        temp.id = 58; temp.isomId = 3025; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet[58] = temp;
        temp = new isomSymbol();
        temp.id = 59; temp.isomId = 303; temp.type = 0; temp.symbolObject = waterhole;
        isomSet[59] = temp;
        temp = new isomSymbol();
        temp.id = 60; temp.isomId = 304; temp.type = 1; temp.symbolObject = crossableWatercourse;
        isomSet[60] = temp;
        temp = new isomSymbol();
        temp.id = 61; temp.isomId = 305; temp.type = 1; temp.symbolObject = smallCrossableWatercourse;
        isomSet[61] = temp;
        temp = new isomSymbol();
        temp.id = 62; temp.isomId = 306; temp.type = 1; temp.symbolObject = seasonalWaterChannel;
        isomSet[62] = temp;
        temp = new isomSymbol();
        temp.id = 63; temp.isomId = 307; temp.type = 2; temp.symbolObject = uncrossableMarsh;
        isomSet[63] = temp;
        temp = new isomSymbol();
        temp.id = 64; temp.isomId = 3071; temp.type = 2; temp.symbolObject = uncrossableMarsh;
        isomSet[64] = temp;
        temp = new isomSymbol();
        temp.id = 65; temp.isomId = -3072; temp.type = -1; temp.symbolObject = null;
        isomSet[65] = temp;
        temp = new isomSymbol();
        temp.id = 66; temp.isomId = 308; temp.type = 2; temp.symbolObject = marsh;
        isomSet[66] = temp;
        temp = new isomSymbol();
        temp.id = 67; temp.isomId = 3081; temp.type = 2; temp.symbolObject = marsh;
        isomSet[67] = temp;
        temp = new isomSymbol();
        temp.id = 68; temp.isomId = 309; temp.type = 1; temp.symbolObject = narrowMarsh;
        isomSet[68] = temp;
        temp = new isomSymbol();
        temp.id = 69; temp.isomId = 310; temp.type = 2; temp.symbolObject = indistinctMarsh;
        isomSet[69] = temp;
        temp = new isomSymbol();
        temp.id = 70; temp.isomId = 3101; temp.type = 2; temp.symbolObject = indistinctMarsh;
        isomSet[70] = temp;
        temp = new isomSymbol();
        temp.id = 71; temp.isomId = 311; temp.type = 0; temp.symbolObject = well;
        isomSet[71] = temp;
        temp = new isomSymbol();
        temp.id = 72; temp.isomId = 312; temp.type = 0; temp.symbolObject = spring;
        isomSet[72] = temp;
        temp = new isomSymbol();
        temp.id = 73; temp.isomId = 313; temp.type = 0; temp.symbolObject = prominentWaterFeature;
        isomSet[73] = temp;
        temp = new isomSymbol();
        temp.id = 74; temp.isomId = 401; temp.type = 2; temp.symbolObject = openLand;
        isomSet[74] = temp;
        temp = new isomSymbol();
        temp.id = 75; temp.isomId = 402; temp.type = 2; temp.symbolObject = openLandWithTrees;
        isomSet[75] = temp;
        temp = new isomSymbol();
        temp.id = 76; temp.isomId = 4021; temp.type = 2; temp.symbolObject = openLandWithBushes;
        isomSet[76] = temp;
        temp = new isomSymbol();
        temp.id = 77; temp.isomId = 403; temp.type = 2; temp.symbolObject = roughOpenLand;
        isomSet[77] = temp;
        temp = new isomSymbol();
        temp.id = 78; temp.isomId = 404; temp.type = 2; temp.symbolObject = roughOpenLandWithTrees;
        isomSet[78] = temp;
        temp = new isomSymbol();
        temp.id = 79; temp.isomId = 4041; temp.type = 2; temp.symbolObject = roughOpenLandWithBushes;
        isomSet[79] = temp;
        temp = new isomSymbol();
        temp.id = 80; temp.isomId = 405; temp.type = 2; temp.symbolObject = forest;
        isomSet[80] = temp;
        temp = new isomSymbol();
        temp.id = 81; temp.isomId = 406; temp.type = 2; temp.symbolObject = vegetationSlow;
        isomSet[81] = temp;
        temp = new isomSymbol();
        temp.id = 82; temp.isomId = 4061; temp.type = 2; temp.symbolObject = vegetationSlowOneDir;
        isomSet[82] = temp;
        temp = new isomSymbol();
        temp.id = 83; temp.isomId = 407; temp.type = 2; temp.symbolObject = vegetationSlowGoodVis;
        isomSet[83] = temp;
        temp = new isomSymbol();
        temp.id = 84; temp.isomId = 408; temp.type = 2; temp.symbolObject = vegetationWalk;
        isomSet[84] = temp;
        temp = new isomSymbol();
        temp.id = 85; temp.isomId = 4081; temp.type = 2; temp.symbolObject = vegetationWalkOneDir;
        isomSet[85] = temp;
        temp = new isomSymbol();
        temp.id = 86; temp.isomId = 4082; temp.type = 2; temp.symbolObject = vegetationWalkOneDir;
        isomSet[86] = temp;
        temp = new isomSymbol();
        temp.id = 87; temp.isomId = 409; temp.type = 2; temp.symbolObject = vegetationWalkGoodVis;
        isomSet[87] = temp;
        temp = new isomSymbol();
        temp.id = 88; temp.isomId = 410; temp.type = 2; temp.symbolObject = vegetationFight;
        isomSet[88] = temp;
        temp = new isomSymbol();
        temp.id = 89; temp.isomId = 4101; temp.type = 2; temp.symbolObject = vegetationFightOneDir;
        isomSet[89] = temp;
        temp = new isomSymbol();
        temp.id = 90; temp.isomId = 4102; temp.type = 2; temp.symbolObject = vegetationFightOneDir;
        isomSet[90] = temp;
        temp = new isomSymbol();
        temp.id = 91; temp.isomId = 4103; temp.type = 2; temp.symbolObject = vegetationFightOneDir;
        isomSet[91] = temp;
        temp = new isomSymbol();
        temp.id = 92; temp.isomId = 4104; temp.type = 2; temp.symbolObject = vegetationFight;
        isomSet[92] = temp;
        temp = new isomSymbol();
        temp.id = 93; temp.isomId = 412; temp.type = 2; temp.symbolObject = cultivatedLand;
        isomSet[93] = temp;
        temp = new isomSymbol();
        temp.id = 94; temp.isomId = 4121; temp.type = 2; temp.symbolObject = cultivatedLand;
        isomSet[94] = temp;
        temp = new isomSymbol();
        temp.id = 95; temp.isomId = 413; temp.type = 2; temp.symbolObject = orchard;
        isomSet[95] = temp;
        temp = new isomSymbol();
        temp.id = 96; temp.isomId = 4131; temp.type = 2; temp.symbolObject = orchard;
        isomSet[96] = temp;
        temp = new isomSymbol();
        temp.id = 97; temp.isomId = 414; temp.type = 2; temp.symbolObject = vineyard;
        isomSet[97] = temp;
        temp = new isomSymbol();
        temp.id = 98; temp.isomId = 4141; temp.type = 2; temp.symbolObject = vineyard;
        isomSet[98] = temp;
        temp = new isomSymbol();
        temp.id = 99; temp.isomId = -415; temp.type = -1; temp.symbolObject = null;
        isomSet[99] = temp;
        temp = new isomSymbol();
        temp.id = 100; temp.isomId = 416; temp.type = 1; temp.symbolObject = distinctVegetationBoundary;
        isomSet[100] = temp;
        temp = new isomSymbol();
        temp.id = 101; temp.isomId = 4161; temp.type = 1; temp.symbolObject = distinctVegetationBoundary;
        isomSet[101] = temp;
        temp = new isomSymbol();
        temp.id = 102; temp.isomId = 417; temp.type = 0; temp.symbolObject = prominentLargeTree;
        isomSet[102] = temp;
        temp = new isomSymbol();
        temp.id = 103; temp.isomId = 418; temp.type = 0; temp.symbolObject = prominentSmallTree;
        isomSet[103] = temp;
        temp = new isomSymbol();
        temp.id = 104; temp.isomId = 419; temp.type = 0; temp.symbolObject = prominentVegetationFeature;
        isomSet[104] = temp;
        temp = new isomSymbol();
        temp.id = 105; temp.isomId = 501; temp.type = 2; temp.symbolObject = pavedArea;
        isomSet[105] = temp;
        temp = new isomSymbol();
        temp.id = 106; temp.isomId = 5011; temp.type = 2; temp.symbolObject = pavedArea;
        isomSet[106] = temp;
        temp = new isomSymbol();
        temp.id = 107; temp.isomId = -5012; temp.type = -1; temp.symbolObject = null;
        isomSet[107] = temp;
        temp = new isomSymbol();
        temp.id = 108; temp.isomId = 502; temp.type = 1; temp.symbolObject = wideRoad;
        isomSet[108] = temp;
        temp = new isomSymbol();
        temp.id = 109; temp.isomId = 5022; temp.type = 1; temp.symbolObject = wideRoad;
        isomSet[109] = temp;
        temp = new isomSymbol();
        temp.id = 110; temp.isomId = 503; temp.type = 1; temp.symbolObject = road;
        isomSet[110] = temp;
        temp = new isomSymbol();
        temp.id = 111; temp.isomId = 504; temp.type = 1; temp.symbolObject = vehicleTrack;
        isomSet[111] = temp;
        temp = new isomSymbol();
        temp.id = 112; temp.isomId = 505; temp.type = 1; temp.symbolObject = footpath;
        isomSet[112] = temp;
        temp = new isomSymbol();
        temp.id = 113; temp.isomId = 506; temp.type = 1; temp.symbolObject = smallFootpath;
        isomSet[113] = temp;
        temp = new isomSymbol();
        temp.id = 114; temp.isomId = 507; temp.type = 1; temp.symbolObject = lessDistinctSmallFootpath;
        isomSet[114] = temp;
        temp = new isomSymbol();
        temp.id = 115; temp.isomId = 508; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet[115] = temp;
        temp = new isomSymbol();
        temp.id = 116; temp.isomId = 5081; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet[116] = temp;
        temp = new isomSymbol();
        temp.id = 117; temp.isomId = 5082; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet[117] = temp;
        temp = new isomSymbol();
        temp.id = 118; temp.isomId = 5083; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet[118] = temp;
        temp = new isomSymbol();
        temp.id = 119; temp.isomId = 5084; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet[119] = temp;
        temp = new isomSymbol();
        temp.id = 120; temp.isomId = 509; temp.type = 1; temp.symbolObject = railway;
        isomSet[120] = temp;
        temp = new isomSymbol();
        temp.id = 121; temp.isomId = 510; temp.type = 1; temp.symbolObject = powerLine;
        isomSet[121] = temp;
        temp = new isomSymbol();
        temp.id = 122; temp.isomId = 511; temp.type = 1; temp.symbolObject = majorPowerLine;
        isomSet[122] = temp;
        temp = new isomSymbol();
        temp.id = 123; temp.isomId = 5111; temp.type = 1; temp.symbolObject = majorPowerLine;
        isomSet[123] = temp;
        temp = new isomSymbol();
        temp.id = 124; temp.isomId = 5112; temp.type = 1; temp.symbolObject = majorPowerLine;
        isomSet[124] = temp;
        temp = new isomSymbol();
        temp.id = 125; temp.isomId = 512; temp.type = 0; temp.symbolObject = bridgeTunnel;
        isomSet[125] = temp;
        temp = new isomSymbol();
        temp.id = 126; temp.isomId = 5121; temp.type = 0; temp.symbolObject = bridgeTunnel;
        isomSet[126] = temp;
        temp = new isomSymbol();
        temp.id = 127; temp.isomId = 5122; temp.type = 0; temp.symbolObject = bridgeTunnel;
        isomSet[127] = temp;
        temp = new isomSymbol();
        temp.id = 128; temp.isomId = 513; temp.type = 1; temp.symbolObject = wall;
        isomSet[128] = temp;
        temp = new isomSymbol();
        temp.id = 129; temp.isomId = 514; temp.type = 1; temp.symbolObject = ruinedWall;
        isomSet[129] = temp;
        temp = new isomSymbol();
        temp.id = 130; temp.isomId = 515; temp.type = 1; temp.symbolObject = impassableWall;
        isomSet[130] = temp;
        temp = new isomSymbol();
        temp.id = 131; temp.isomId = 516; temp.type = 1; temp.symbolObject = fence;
        isomSet[131] = temp;
        temp = new isomSymbol();
        temp.id = 132; temp.isomId = 517; temp.type = 1; temp.symbolObject = ruinedFence;
        isomSet[132] = temp;
        temp = new isomSymbol();
        temp.id = 133; temp.isomId = 518; temp.type = 1; temp.symbolObject = impassableFence;
        isomSet[133] = temp;
        temp = new isomSymbol();
        temp.id = 134; temp.isomId = 519; temp.type = 0; temp.symbolObject = crossingPointFence;
        isomSet[134] = temp;
        temp = new isomSymbol();
        temp.id = 135; temp.isomId = 520; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet[135] = temp;
        temp = new isomSymbol();
        temp.id = 136; temp.isomId = 5201; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet[136] = temp;
        temp = new isomSymbol();
        temp.id = 137; temp.isomId = 5202; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet[137] = temp;
        temp = new isomSymbol();
        temp.id = 138; temp.isomId = 5203; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet[138] = temp;
        temp = new isomSymbol();
        temp.id = 139; temp.isomId = 521; temp.type = 2; temp.symbolObject = building;
        isomSet[139] = temp;
        temp = new isomSymbol();
        temp.id = 140; temp.isomId = 5211; temp.type = 2; temp.symbolObject = building;
        isomSet[140] = temp;
        temp = new isomSymbol();
        temp.id = 141; temp.isomId = 5212; temp.type = 2; temp.symbolObject = building;
        isomSet[141] = temp;
        temp = new isomSymbol();
        temp.id = 142; temp.isomId = 5213; temp.type = 2; temp.symbolObject = building;
        isomSet[142] = temp;
        temp = new isomSymbol();
        temp.id = 143; temp.isomId = 5214; temp.type = 2; temp.symbolObject = building;
        isomSet[143] = temp;
        temp = new isomSymbol();
        temp.id = 144; temp.isomId = 522; temp.type = 2; temp.symbolObject = canopy;
        isomSet[144] = temp;
        temp = new isomSymbol();
        temp.id = 145; temp.isomId = 5221; temp.type = 2; temp.symbolObject = canopy;
        isomSet[145] = temp;
        temp = new isomSymbol();
        temp.id = 146; temp.isomId = 5222; temp.type = 2; temp.symbolObject = canopy;
        isomSet[146] = temp;
        temp = new isomSymbol();
        temp.id = 147; temp.isomId = 523; temp.type = 2; temp.symbolObject = ruin;
        isomSet[147] = temp;
        temp = new isomSymbol();
        temp.id = 148; temp.isomId = 5231; temp.type = 2; temp.symbolObject = canopy;
        isomSet[148] = temp;
        temp = new isomSymbol();
        temp.id = 149; temp.isomId = 524; temp.type = 0; temp.symbolObject = highTower;
        isomSet[149] = temp;
        temp = new isomSymbol();
        temp.id = 150; temp.isomId = 525; temp.type = 0; temp.symbolObject = smallTower;
        isomSet[150] = temp;
        temp = new isomSymbol();
        temp.id = 151; temp.isomId = 526; temp.type = 0; temp.symbolObject = cairn;
        isomSet[151] = temp;
        temp = new isomSymbol();
        temp.id = 152; temp.isomId = 527; temp.type = 0; temp.symbolObject = fodderRack;
        isomSet[152] = temp;
        temp = new isomSymbol();
        temp.id = 153; temp.isomId = 528; temp.type = 1; temp.symbolObject = prominentLineFeature;
        isomSet[153] = temp;
        temp = new isomSymbol();
        temp.id = 154; temp.isomId = 529; temp.type = 1; temp.symbolObject = prominentImpassableLineFeature;
        isomSet[154] = temp;
        temp = new isomSymbol();
        temp.id = 155; temp.isomId = 530; temp.type = 0; temp.symbolObject = prominentManMadeFeatureRing;
        isomSet[155] = temp;
        temp = new isomSymbol();
        temp.id = 156; temp.isomId = 531; temp.type = 0; temp.symbolObject = prominentManMadeFeatureX;
        isomSet[156] = temp;
        temp = new isomSymbol();
        temp.id = 157; temp.isomId = 532; temp.type = 1; temp.symbolObject = stairway;
        isomSet[157] = temp;
        temp = new isomSymbol();
        temp.id = 158; temp.isomId = 5321; temp.type = 1; temp.symbolObject = stairway;
        isomSet[158] = temp;
        temp = new isomSymbol();
        temp.id = 159; temp.isomId = -6011; temp.type = -1; temp.symbolObject = null;
        isomSet[159] = temp;
        temp = new isomSymbol();
        temp.id = 160; temp.isomId = -6012; temp.type = -1; temp.symbolObject = null;
        isomSet[160] = temp;
        temp = new isomSymbol();
        temp.id = 161; temp.isomId = -6013; temp.type = -1; temp.symbolObject = null;
        isomSet[161] = temp;
        temp = new isomSymbol();
        temp.id = 162; temp.isomId = -6014; temp.type = -1; temp.symbolObject = null;
        isomSet[162] = temp;
        temp = new isomSymbol();
        temp.id = 163; temp.isomId = -602; temp.type = -1; temp.symbolObject = null;
        isomSet[163] = temp;
        temp = new isomSymbol();
        temp.id = 164; temp.isomId = -603; temp.type = -1; temp.symbolObject = null;
        isomSet[164] = temp;
        temp = new isomSymbol();
        temp.id = 165; temp.isomId = -6031; temp.type = -1; temp.symbolObject = null;
        isomSet[165] = temp;
        temp = new isomSymbol();
        temp.id = 166; temp.isomId = 704; temp.type = 0; temp.symbolObject = null;
        isomSet[166] = temp;
        temp = new isomSymbol();
        temp.id = 167; temp.isomId = 799; temp.type = 0; temp.symbolObject = null;
        isomSet[167] = temp;
        parseOMAP();
    }
    void getMapSize(){
        foreach(MapSymbol symbol in omap){
            foreach (Vector2 coord in symbol.coords){
                if(coord.x > maxX)
                    maxX = coord.x;
                if(coord.x < minX)
                    minX = coord.x;
                if(coord.y > maxY)
                    maxY = coord.y;
                if(coord.y < minY)
                    minY = coord.y;
            }
        }
    }
    void generateMapBounds(){
        getMapSize();
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = 513;
        terrainData.size = new Vector3(math.abs(maxX-minX), 0, math.abs(maxY-minY));
        GameObject terrainObject = Terrain.CreateTerrainGameObject(terrainData);
        terrainObject.transform.position = new Vector3(minX, 0, minY);
        terrain = terrainObject.GetComponent<Terrain>();
    }
    bool isContourClosed(MapSymbol contour, float threshold = 1f){
        if(contour.coords.Count < 3)
            return false;
        float distance = Vector2.Distance(contour.coords[0], contour.coords[contour.coords.Count-1]);
        return distance < threshold;
    }
    bool isPointInPolygon(Vector2 point, List<Vector2> coords){
        int intersections = 0;
        int count = coords.Count;
        for(int i = 0; i < count; i++){
            Vector2 a = coords[i];
            Vector2 b = coords[(i+1)%count];
            if((a.y > point.y) != (b.y > point.y)){
                float xIntersect = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;
                if(point.x < xIntersect)
                    intersections++;
            }
        }
        return intersections % 2 == 1;
    }
    int getOffsetEdge(Vector2 point, float offset){
        float left = minX - offset;
        float right = maxX + offset;
        float bottom = minY - offset;
        float top = maxY + offset;
        if(Mathf.Abs(point.y-bottom) < 0.1f)
            return 0;
        if(Mathf.Abs(point.x-right) < 0.1f)
            return 1;
        if(Math.Abs(point.y - top) < 0.1f)
            return 2;
        return 3;
    }
    float calculatePathLength(Vector2 start, List<Vector2> path){
        float length = Vector2.Distance(start,path[0]);
        for(int i = 0; i < path.Count -1; i++){
            length += Vector2.Distance(path[i],path[i+1]);
        }
        return length;
    }
    List<Vector2> getBoundsPath(Vector2 start, Vector2 end, float offset){
        List<Vector2> path = new List<Vector2>();
        int startEdge = getOffsetEdge(start, offset);
        int endEdge = getOffsetEdge(end, offset);
        if(startEdge == endEdge){
            path.Add(end);
            return path;
        }
        List<Vector2> corners = new List<Vector2>{
            new Vector2(minX -offset,minY-offset),
            new Vector2(maxX+offset,minY-offset),
            new Vector2(maxX+offset,maxY+offset),
            new Vector2(minX-offset,maxY+offset)
        };
        List<Vector2> cwPath = new List<Vector2>();
        int edge = startEdge;
        while(edge != endEdge){
            edge = (edge+1) % 4;
            cwPath.Add(corners[edge]);
        }
        cwPath.Add(end);
        List<Vector2> ccwPath = new List<Vector2>();
        edge = startEdge;
        while(edge != endEdge){
            edge = (edge -1 +4)%4;
            ccwPath.Add(corners[edge]);
        }
        ccwPath.Add(end);
        float cwDist = calculatePathLength(start, cwPath);
        float ccwDist = calculatePathLength(start, ccwPath);
        return cwDist <= ccwDist ? cwPath : ccwPath;
    }
    Vector2 snapToOffsetBoundary(Vector2 point, float offset){
        float distToLeft = Mathf.Abs(point.x-minX);
        float distToRight = Mathf.Abs(point.x-maxX);
        float distToBottom = Mathf.Abs(point.y-minY);
        float distToTop = Math.Abs(point.y-maxY);
        float minDist = Mathf.Min(distToBottom, distToLeft, distToRight, distToTop);
        if(minDist == distToLeft)
            return new Vector2(minX-offset, point.y);
        if(minDist == distToRight)
            return new Vector2(maxX+offset, point.y);
        if(minDist == distToBottom)
            return new Vector2(point.x, minY-offset);
        return new Vector2(point.x, maxY+offset);
    }
    List<Vector2> closeOpenContour(List<Vector2> coords, bool isClosed, int offsetIndex){
        if(isClosed)
            return coords;
        float offset = offsetIndex*2f;
        List<Vector2> closedCoords = new List<Vector2>(coords);
        Vector2 start = coords[0], end = coords[coords.Count-1];
        Vector2 startBoundary = snapToOffsetBoundary(start,offset), endBoundary = snapToOffsetBoundary(end,offset);
        closedCoords.Add(endBoundary);
        List<Vector2> boundsPath = getBoundsPath(endBoundary, startBoundary, offset);
        closedCoords.AddRange(boundsPath);
        return closedCoords;
    }
    float getContourExtent(ContourData contour){
        float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
        foreach(Vector2 coord in contour.coords){
            if(coord.x < minX) 
                minX = coord.x;
            if(coord.x > maxX)
                maxX = coord.x;
            if(coord.y < minY)
                minY = coord.y;
            if(coord.y > maxY)
                maxY = coord.y;
        }
        float width = maxX - minX;
        float height = maxY - minY;
        return Mathf.Sqrt(width*width+height*height);
    }
    int getOffsetIndex(ContourData contour, ContourData testPoint, List<ContourData> allContours){
        if(contour.isClosed)
            return 0;
        float extent = getContourExtent(contour);
        int offsetIndex = 0;
        foreach(ContourData other in allContours){
            if(other == contour)
                continue;
            if(!other.isClosed){
                float otherExtent = getContourExtent(other);
                if(extent > otherExtent){
                    offsetIndex++;
                }
            }
        }
        return offsetIndex;
    }
    int findNestingLevel(ContourData contour, List<ContourData> allContours){
        Vector2 point = contour.coords[0];
        int level = 0;
        foreach(ContourData otherContour in allContours){
            if(contour.coords == otherContour.coords){
                continue;
            }
            int offsetIndex = getOffsetIndex(otherContour, contour, allContours);
            List<Vector2> closedCoords = closeOpenContour(otherContour.coords, otherContour.isClosed, offsetIndex);
            if(isPointInPolygon(point, closedCoords)){
                level++;
            }
        }
        return level;
    }
    float IDWInterpolation(Vector2 worldPos, List<ElevationPoint> heightPoints){
        float power = 2f;
        int kNearest = 20;
        List <(float dist, float height)> distances = new List<(float,float)>();
        foreach(var point in heightPoints){
            float dist = Vector2.Distance(worldPos, point.coords);
            if(dist < 0.01f) return point.height;
            distances.Add((dist, point.height));
        }
        distances.Sort((a,b) => a.dist.CompareTo(b.dist));
        float sumWeights = 0f;
        float sumValues = 0f;
        int count = Mathf.Min(kNearest, distances.Count);
        for(int i = 0; i < count; i++){
            float weight = 1f/Mathf.Pow(distances[i].dist, power);
            sumWeights += weight;
            sumValues += weight * distances[i].height;
        }
        return sumValues/sumWeights;
    }
    // 1 - slope line outwards, 2 - slope line inwards, 0 - not set
    int slopeDirection(MapSymbol slopeLine, ContourData contour){
        Vector2 direction = new Vector2(
            Mathf.Cos(slopeLine.rotation),
            Mathf.Sin(slopeLine.rotation)
        );
        Vector2 offset = direction * 5f;
        slopeLine.coords[0] += offset;
        if(isPointInPolygon(slopeLine.coords[0], contour.coords))
            return 2;
        return 1;
    }
    float distancePointToSegment(Vector2 p, Vector2 a, Vector2 b){
        Vector2 ab = b - a;
        float lenSq = Vector2.Dot(ab,ab);
        if(lenSq < 1e-3f)
            return Vector2.Distance(p,a);
        float t = Vector2.Dot(p-a,ab)/lenSq;
        t = Mathf.Clamp01(t);
        Vector2 closest = a+t*ab;
        return Vector2.Distance(p,closest);
    }
    int findClosestContour(MapSymbol slopeLine, List<ContourData> contours){
        ContourData closestContour = null;
        int index = -1;
        float minDistance = float.MaxValue;
        Vector2 slopePoint = slopeLine.coords[0];
        for(int idx = 0; idx < contours.Count; idx++){
            ContourData contour = contours[idx];
            int segmentCount = contour.isClosed ? contour.coords.Count : contour.coords.Count -1;
            for(int i =0; i < segmentCount; i++){
                Vector2 point1 = contour.coords[i];
                Vector2 point2 = contour.coords[(i+1) % contour.coords.Count];
                float dist = distancePointToSegment(slopePoint, point1, point2);
                if(dist < minDistance){
                    minDistance = dist;
                    closestContour = contour;
                    index = idx;
                }
            }
        }
        return index;
    }
    void assignElevationLevels(List<ContourData> contours){
        Dictionary<ContourData, ContourData> parentOf = new Dictionary<ContourData, ContourData>();
        foreach(ContourData child in contours){
            foreach(ContourData potentialParent in contours){
                if(child == potentialParent) continue;
                if(potentialParent.nestLevel == child.nestLevel -1){
                    int offsetIndex = getOffsetIndex(potentialParent, child, contours);
                    List<Vector2> closedParentCoords = closeOpenContour(potentialParent.coords, potentialParent.isClosed, offsetIndex);
                    if(isPointInPolygon(child.coords[0], closedParentCoords)){
                        parentOf[child] = potentialParent;
                        break;
                    }
                }
            }
        }
        Dictionary<ContourData, bool> isRootDepression = new Dictionary<ContourData, bool>();
        Dictionary<ContourData, int> maxDepth = new Dictionary<ContourData, int>();
        foreach(ContourData root in contours){
            if(!parentOf.ContainsKey(root)){
                bool hasDepression = false;
                int depth = 0;
                int rootOffsetIndex = getOffsetIndex(root,root,contours);
                List<Vector2> closedRootCoords = closeOpenContour(root.coords,root.isClosed,rootOffsetIndex);
                foreach(ContourData other in contours){
                    if(other.nestLevel >= root.nestLevel){
                        if(other == root || (other.nestLevel > root.nestLevel && isPointInPolygon(other.coords[0], closedRootCoords))){
                            int relativeDepth = other.nestLevel - root.nestLevel;
                            if(relativeDepth > depth) depth = relativeDepth;
                            if(other.slopeDir == 2) hasDepression = true;
                        }
                    }
                }
                isRootDepression[root] = hasDepression;
                maxDepth[root] = depth;
            }
        }
        Dictionary<ContourData, int> elevationLevel = new Dictionary<ContourData, int>();
        foreach(ContourData contour in contours){
            if(!parentOf.ContainsKey(contour)){
                elevationLevel[contour] = contour.nestLevel+1;
            }
        }
        int maxNestLevel = 0;
        foreach(ContourData c in contours){
            if(c.nestLevel > maxNestLevel) maxNestLevel = c.nestLevel;
        }
        for(int nest = 1; nest <= maxNestLevel; nest++){
            foreach(ContourData contour in contours){
                if(contour.nestLevel != nest) continue;
                if(!parentOf.ContainsKey(contour)) continue;
                ContourData parent = parentOf[contour];
                int parentElev = elevationLevel[parent];
                ContourData root = parent;
                while(parentOf.ContainsKey(root)){
                    root = parentOf[root];
                }
                bool inDepression = isRootDepression.ContainsKey(root) && isRootDepression[root];
                if(contour.slopeDir == 2){
                        elevationLevel[contour] = parentElev -1;
                }
                else if(contour.slopeDir == 1){
                    elevationLevel[contour] = parentElev +1;
                }
                else if(inDepression){
                    elevationLevel[contour] = parentElev-1;
                }
                else{
                    elevationLevel[contour] = parentElev +1;
                }
            }
        }
        int minLevel = 0;
        foreach(ContourData contour in contours){
            if(elevationLevel.ContainsKey(contour)){
                minLevel = Mathf.Min(minLevel, elevationLevel[contour]);
            }
        }
        minLevel*=-1;
        foreach(ContourData contour in contours){
            if(elevationLevel.ContainsKey(contour)){
                contour.nestLevel = elevationLevel[contour]+minLevel;
            }
        }
    }
    void generateHeightMap(){
        List<ContourData> contours = new List<ContourData>();
        List<MapSymbol> slopeLines = new List<MapSymbol>();
        foreach(MapSymbol symbol in omap){
            if(symbol.id == "0" || symbol.id == "2" || symbol.id == "4"){
                ContourData temp = new ContourData();
                temp.id = symbol.id; 
                temp.coords = symbol.coords;
                temp.isClosed = isContourClosed(symbol);
                temp.nestLevel = 0;
                temp.slopeDir = 0;
                contours.Add(temp);
            }
            if(symbol.id == "1"){
                slopeLines.Add(symbol);
            }
        }
        for(int i = 0; i < slopeLines.Count; i++){
            int closestContour = findClosestContour(slopeLines[i],contours);
            contours[closestContour].slopeDir = slopeDirection(slopeLines[i],contours[closestContour]);
        }
        for(int i = 0; i < contours.Count; i++){
            contours[i].nestLevel = findNestingLevel(contours[i], contours);
        }
        assignElevationLevels(contours);
        List<ElevationPoint> heightPoints = new List<ElevationPoint>();
        TerrainData data = terrain.terrainData;
        int resolution = data.heightmapResolution;
        float[,] heights = new float[resolution,resolution];
        float minElev = float.MaxValue;
        float maxElev = float.MinValue;
        float sampleInterval = 5f;
        for(int i = 0; i < contours.Count; i++){
            /*for(int j = 0; j < contours[i].coords.Count; j++){
                ElevationPoint temp = new ElevationPoint();
                temp.coords.x = contours[i].coords[j].x;
                temp.coords.y = contours[i].coords[j].y;
                temp.height = 5*contours[i].nestLevel;
                if(temp.height > maxElev) maxElev = temp.height;
                if(temp.height < minElev) minElev = temp.height;
                heightPoints.Add(temp);
            }*/
            int offsetIndex = getOffsetIndex(contours[i],contours[i],contours);
            List<Vector2> coords = closeOpenContour(contours[i].coords, contours[i].isClosed,offsetIndex);
            if(coords.Count < 2) continue;
            float elevation = 5f * contours[i].nestLevel;
            float accumulatedDist = 0f;
            ElevationPoint first = new ElevationPoint();
            first.coords = coords[0];
            first.height = elevation;
            heightPoints.Add(first);
            int segmentCount = contours[i].isClosed ? coords.Count : coords.Count -1;
            for(int j = 0; j < segmentCount; j++){
                Vector2 segmentStart = coords[j];
                Vector2 segmentEnd = coords[(j+1)%coords.Count];
                float segmentLength = Vector2.Distance(segmentStart, segmentEnd);
                float distanceAlongSegment = 0f;
                while(accumulatedDist + distanceAlongSegment < segmentLength){
                    float remainingDist = sampleInterval - accumulatedDist;
                    distanceAlongSegment += remainingDist;
                    float t = distanceAlongSegment / segmentLength;
                    Vector2 interpolatedPos = Vector2.Lerp(segmentStart, segmentEnd, t);
                    ElevationPoint temp = new ElevationPoint();
                    temp.coords = interpolatedPos;
                    temp.height = elevation;
                    temp.direction = 0;
                    heightPoints.Add(temp);
                    accumulatedDist = 0f;
                }
                accumulatedDist += segmentLength - distanceAlongSegment;
            }
            if(elevation > maxElev) maxElev = elevation;
            if(elevation < minElev) minElev = elevation;
        }
        minElev = 0;
        if(heightPoints.Count == 0){
            return;
        }
        float elevRange = maxElev-minElev;
        if(elevRange == 0) elevRange = 1;
        for(int y = 0; y < resolution; y++){
            for(int x = 0; x < resolution; x++){
                Vector2 worldPos = new Vector2(
                    minX+(float)x/(resolution-1)*(maxX-minX),
                    minY+(float)y /(resolution-1)*(maxY-minY)
                );
                bool insideAnyContour = false;
                foreach(ContourData contour in contours){
                    List<Vector2> testCoords;
                    if(contour.isClosed){
                        testCoords = contour.coords;
                    }
                    else{
                        int offsetIndex = getOffsetIndex(contour, contour, contours);
                        testCoords = closeOpenContour(contour.coords, contour.isClosed, offsetIndex);
                    }
                    if(isPointInPolygon(worldPos, testCoords)){
                        insideAnyContour = true;
                        break;
                    }
                }
                float elevation;
                if(insideAnyContour){
                    elevation = IDWInterpolation(worldPos, heightPoints);
                }
                else{
                    float nearestDist = float.MaxValue;
                    float nearestElev = 0;
                    Vector2 nearestContourPoint = Vector2.zero;
                    foreach(ContourData contour in contours){
                        for(int j = 0; j < contour.coords.Count - 1; j++){
                            float dist = distancePointToSegment(worldPos, contour.coords[j], contour.coords[j+1]);
                            if(dist < nearestDist){
                                nearestDist = dist;
                                nearestElev = 5f * contour.nestLevel;
                                Vector2 ab = contour.coords[j+1]-contour.coords[j];
                                float lenSq = Vector2.Dot(ab,ab);
                                float t = Mathf.Clamp01(Vector2.Dot(worldPos-contour.coords[j], ab)/lenSq);
                                nearestContourPoint = contour.coords[j]+t*ab;
                            }
                        }
                    }
                    float distToEdge = Mathf.Min(
                        Mathf.Abs(worldPos.x - minX),
                        Mathf.Abs(worldPos.x - maxX),
                        Mathf.Abs(worldPos.y - minY),
                        Mathf.Abs(worldPos.y - maxY)
                    );
                    float contourDistToEdge = Mathf.Min(
                        Mathf.Abs(nearestContourPoint.x - minX),
                        Mathf.Abs(nearestContourPoint.x-maxX),
                        Mathf.Abs(nearestContourPoint.y-minY),
                        Mathf.Abs(nearestContourPoint.y-maxY)
                    );
                    float edgeThreshold = 20f;
                    float edgeFalloff = nearestDist/edgeThreshold;
                    float edgeElev = nearestElev * Mathf.Max(0.3f,1f-edgeFalloff*0.7f);
                    float totalDist = nearestDist+distToEdge;
                    float tNormal =nearestDist/totalDist;
                    float normalElev;
                    if(tNormal <= 0.5f){
                        normalElev = nearestElev*(1f-2f*tNormal);
                    }
                    else{
                        normalElev = 0;
                    }
                    if(contourDistToEdge < edgeThreshold ){
                        float blendFactor = contourDistToEdge/edgeThreshold;
                        elevation = Mathf.Lerp(edgeElev,normalElev,blendFactor);
                    }
                    else{
                        elevation = normalElev;
                    }
                }
                heights[y,x] = (elevation-minElev)/elevRange;
            }
            if(y % 50 == 0) print($"Heightmap progress: {y}/{resolution}");
        }
        data.SetHeights(0,0,heights);
        Vector3 size = data.size;
        size.y = maxElev;
        data.size = size;
    }
    /*
        Painting the terrain
    */
    void setupTerrainLayers(){
        TerrainData data = terrain.terrainData;
        List<TerrainLayer> layers = new List<TerrainLayer>();
        if(grassLayer != null)
            layers.Add(grassLayer);
        if(sandLayer != null)
            layers.Add(sandLayer);
        if(rockLayer != null)
            layers.Add(rockLayer);
        data.terrainLayers = layers.ToArray();
    }
    int getTerrainLayerIndex(int id){
        if((id == 74) || (id >= 80 && id <= 92)){
            return 0;
        }
        if(id == 46){
            return 1;
        }
        if(id == 47){
            return 2;
        }
        return -1;
    }
    void paintArea(float[,,] alphamap, List<Vector2> coords, int layerIndex, int width, int height){
        TerrainData data = terrain.terrainData;
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                Vector2 worldPos = new Vector2(
                    minX + (float)x / (width-1)*(maxX-minX),
                    minY + (float)y / (height-1)*(maxY-minY)
                );
                if(isPointInPolygon(worldPos, coords)){
                    for(int i = 0; i < alphamap.GetLength(2); i++){
                        alphamap[y,x,i] = 0f;
                    }
                alphamap[y,x,layerIndex] = 1f;
                }
            }
        }
    }
    void paintTerrain(){
        TerrainData data = terrain.terrainData;
        int alphamapWidth = data.alphamapWidth;
        int alphamapHeight = data.alphamapHeight;
        int numLayers = data.terrainLayers.Length;
        float[,,] alphamap = new float[alphamapHeight, alphamapWidth, numLayers];
        for(int y = 0; y < alphamapHeight; y++){
            for(int x = 0; x < alphamapWidth; x++){
                alphamap[y,x,0] = 1f;
            }
        }
        foreach(MapSymbol symbol in omap){
            isomSymbol refSym = isomSet[int.Parse(symbol.id)];
            if(refSym.type == 2){
                int layerIndex = getTerrainLayerIndex(int.Parse(symbol.id));
                if(layerIndex >= 0){
                    paintArea(alphamap, symbol.coords, layerIndex, alphamapWidth, alphamapHeight);
                }
                if(layerIndex == 0){
                    generateTreePosition(symbol.coords,5, 0);
                }
            }
        data.SetAlphamaps(0,0,alphamap);
        }
    }
    /*
        Tree painting
    */
    public GameObject[] treePrefabs;
    TreePrototype[] treePrototypes;
    void setupTreePrototypes(){
        TerrainData data = terrain.terrainData;
        treePrototypes = new TreePrototype[treePrefabs.Length];
        for(int i = 0; i < treePrefabs.Length; i++){
            TreePrototype prototype = new TreePrototype();
            prototype.prefab = treePrefabs[i];
            treePrototypes[i] = prototype;
        }
        if(treePrototypes != null && treePrototypes.Length > 0){
            data.treePrototypes = treePrototypes;
            data.RefreshPrototypes();
        }
    }
    void spawnTreesOnTerrain(List<Vector2> coords, int treePrototypeIndex){
        print(coords.Count);
        TerrainData data = terrain.terrainData;
        setupTreePrototypes();
        if(data.treePrototypes.Length == 0){
            Debug.LogWarning("No tree prototypes!");
            return;
        }
        if(treePrototypeIndex >= data.treePrototypes.Length){
            Debug.LogWarning("Tree prototype index out of range!");
            return;
        }
        List<TreeInstance> newTrees = new List<TreeInstance>(data.treeInstances);
        foreach(Vector2 coord in coords){
            Vector3 worldPos = new Vector3(coord.x, 0, coord.y);
            Vector3 terrainPos = worldPos - terrain.transform.position;
            float terrainHeight = terrain.SampleHeight(worldPos);
            Vector3 normalizedPos = new Vector3(
                terrainPos.x/data.size.x,
                terrainHeight/data.size.y,
                terrainPos.z/data.size.z
            );
            if(normalizedPos.x >= 0 && normalizedPos.x <= 1 && normalizedPos.z >= 0 && normalizedPos.z <= 1 && normalizedPos.y >= 0 && normalizedPos.y <= 1){
                TreeInstance tree = new TreeInstance();
                tree.position = normalizedPos;
                tree.prototypeIndex = treePrototypeIndex;
                tree.widthScale = 1f;
                tree.heightScale = 1f;
                tree.color = Color.white;
                tree.lightmapColor = Color.white;
                newTrees.Add(tree);
            }
        }
        data.treeInstances = newTrees.ToArray();
        data.RefreshPrototypes();
    }
    void generateTreePosition(List<Vector2> coords, float density, int treePrototypeIndex = -1){
        if(coords.Count < 3)
            return;
        float minXBox = coords[0].x, maxXBox = coords[0].x;
        float minYBox = coords[0].y, maxYBox = coords[0].y;
        foreach(Vector2 coord in coords){
            if(coord.x < minXBox) minXBox = coord.x;
            if(coord.x > maxXBox) maxXBox = coord.x;
            if(coord.y < minYBox) minYBox = coord.y;
            if(coord.y > maxYBox) maxYBox = coord.y;
        }
        float width = maxXBox-minXBox;
        float height = maxYBox-minYBox;
        float area = Mathf.Abs(maxXBox-minXBox)*Mathf.Abs(maxYBox-minYBox);
        int treeCount = Mathf.RoundToInt((area/100)*density);
        List<Vector2> treePositions = new List<Vector2>();
        System.Random rand = new System.Random();
        float tolerance = 1f;
        print(treeCount);
        for(int i = 0, j = 0; i < treeCount && j < treeCount*2;j++){
            Vector2 randomPos = new Vector2(
                minXBox + (float)rand.NextDouble()*width,
                minYBox + (float)rand.NextDouble()*height
            );
            if(!isPointInPolygon(randomPos, coords))
                continue;
            bool tooClose = false;
            foreach(Vector2 existingTree in treePositions){
                if(Vector2.Distance(randomPos, existingTree) < tolerance)
                {
                    tooClose = true;
                    break;
                }
            }
            if(tooClose)
                continue;
            treePositions.Add(randomPos);
            i++;
        }
        spawnTreesOnTerrain(treePositions, treePrototypeIndex);
    }
    // ISOM 2017 symbol set (for now)
    List<MapSymbol> parseOMAP()
    {
        omap = new List<MapSymbol>();
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
                MapSymbol symbol = ParseObject(symbolNode, nsmgr); // todo
                if (symbol != null && symbol.coords.Count > 0)
                {
                    omap.Add(symbol);
                }
            }
        }
        generateMapBounds();
        generateHeightMap();
        setupTerrainLayers();
        paintTerrain();
        foreach(MapSymbol symbol in omap)
        {
            isomSymbol refSym = isomSet[int.Parse(symbol.id)];
            int id = int.Parse(symbol.id);
            if((id == 74) || (id >= 80 && id <= 92)){
                continue;
            }
            if(id == 46){
                continue;
            }
            if(id == 47){
                continue;
            }
            if (refSym.type == 0)
            {
                CreatePointObject(refSym, symbol.coords);
            }
            if (refSym.type == 1)
            {
                CreateLineObject(refSym, symbol.coords);
            }
            if (refSym.type == 2)
            {
                CreateAreaObject(refSym, symbol.coords);
            }
        }
        return omap;
    }
    void CreatePointObject(isomSymbol symbol, List<Vector2> coords)
    {
        if (coords.Count == 0 || symbol.symbolObject == null)
        {
            Debug.LogWarning($"Too few coordinates for {symbol.isomId}");
            return;
        }
        Vector2 pos = coords[0];
        float terrainHeight = terrain.SampleHeight(new Vector3(pos.x, 0, pos.y));
        GameObject obj = Instantiate(symbol.symbolObject, new Vector3(pos.x, terrainHeight, pos.y), Quaternion.identity);
        obj.name = $"{symbol.symbolObject.name}_{symbol.isomId}";
    }
    void CreateLineObject(isomSymbol symbol, List<Vector2> coords)
    {
        if(symbol.isomId == 101 || symbol.isomId == 102 || symbol.isomId == 103)
        {
            return;
        }
        if (coords.Count < 2)
        {
            Debug.LogWarning($"Too few coordinates for {symbol.isomId}");
            return;
        }
        GameObject obj = new GameObject($"{symbol.symbolObject.name}_{symbol.isomId}");
        Vector3 sizes = symbol.symbolObject.GetComponent<MeshFilter>().mesh.bounds.size;
        foreach(Vector2 coord in coords)
        {
            
            GameObject newObj = Instantiate(obj, new Vector3(coord.x,0,coord.y), Quaternion.identity);
        }
    }
    void CreateAreaObject(isomSymbol symbol, List<Vector2> coords)
    {
        if (coords.Count < 3)
        {
            Debug.LogWarning($"Too few coordinates for {symbol.isomId}");
            return;
        }
        GameObject obj = new GameObject($"{symbol.symbolObject.name}_{symbol.isomId}");
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        Material objMaterial = null;
        if (symbol.symbolObject != null)
        {
            MeshRenderer prefabRenderer = symbol.symbolObject.GetComponent<MeshRenderer>();
            if (prefabRenderer != null && prefabRenderer.sharedMaterial != null)
            {
                objMaterial = prefabRenderer.sharedMaterial;
            }
        }
        else if (defaultMaterial != null)
        {
            objMaterial = defaultMaterial;
        }
        mr.sharedMaterial = objMaterial;
        Mesh areaMesh = CreateMesh(coords);
        areaMesh.name = $"Mesh_{symbol.isomId}";
        mf.mesh = areaMesh;
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        collider.sharedMesh = areaMesh;
        if(symbol.id == 80) // white forest
        {
            
        }
    }
    Mesh CreateMesh(List<Vector2> coords)
    {
        Mesh nMesh = new Mesh();
        Vector3[] vertices = new Vector3[coords.Count];
        float terrainHeight = 0f; // todo: process the contours
        for (int i = 0; i < coords.Count; i++)
        {
            vertices[i] = new Vector3(coords[i].x, terrainHeight+0.05f, coords[i].y);
        }
        List<int> triangles = new List<int>();
        for (int i = 1; i < coords.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }
        Vector2[] uvs = GenerateUVs(coords);
        nMesh.vertices = vertices;
        nMesh.triangles = triangles.ToArray();
        nMesh.uv = uvs;
        nMesh.RecalculateNormals();
        nMesh.RecalculateBounds();
        return nMesh;
    }
    Vector2[] GenerateUVs(List<Vector2> coords)
    {
        Vector2[] uvs = new Vector2[coords.Count];
        float minX = coords[0].x;
        float maxX = coords[0].x;
        float minY = coords[0].y;
        float maxY = coords[0].y;
        for (int i = 1; i < coords.Count; i++)
        {
            if (coords[i].x < minX)
                minX = coords[i].x;
            if (coords[i].x > maxX)
                maxX = coords[i].x;
            if (coords[i].y < minY)
                minY = coords[i].y;
            if (coords[i].y > maxY)
                minY = coords[i].y;
        }
        float rangeX = maxX - minX;
        float rangeY = maxY - minY;
        if (rangeX == 0)
            rangeX = 1f;
        if (rangeY == 0)
            rangeY = 1f;
        for(int i = 0; i < coords.Count; i++)
        {
            uvs[i] = new Vector2(
                (coords[i].x - minX) / rangeX,
                (coords[i].y - minY) / rangeY
            );
        }
        return uvs;
    }
    MapSymbol ParseObject(XmlNode symbolNode, XmlNamespaceManager nsmgr)
    {
        MapSymbol symbol = new MapSymbol();
        string id = symbolNode.Attributes["symbol"].Value;
        symbol.id = id;
        float rotation = 0;
        if(symbolNode.Attributes["rotation"] != null)
            rotation = float.Parse(symbolNode.Attributes["rotation"].Value);
        symbol.rotation = rotation;
        XmlNode coordsNode = symbolNode.SelectSingleNode("omap:coords", nsmgr);
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
                    if (coords.Length >= 2)
                    {
                        if (float.TryParse(coords[0], out float x) && float.TryParse(coords[1], out float y))
                            symbol.coords.Add(new Vector2(x / 100f, y / 100f));
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
