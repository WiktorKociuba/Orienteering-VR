using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Unity.Mathematics;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Data.Common;
using TMPro;

public class convertMap : MonoBehaviour
{
    List<MapSymbol> omap;
    List<MapSymbol> courseSym;
    float minX = float.MaxValue, minY = float.MaxValue, maxY = float.MinValue, maxX = float.MinValue;
    Terrain terrain;
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
    GameObject contour; //101
    GameObject indexContoure; //102
    GameObject formLine; //103
    GameObject earthBank; //104
    GameObject earthWall; //1051
    GameObject retainingEarthWall; //1052
    GameObject ruinedEarthWall; //106
    GameObject erosionGully; //107
    GameObject smallErosionGully; //108
    GameObject smallKnoll; //109
    GameObject smallElongatedKnoll; //110
    GameObject smallDepression; //111
    GameObject pit; //112
    GameObject brokenGround; //113
    GameObject veryBrokenGround; //114
    GameObject prominentLandformFeature; //115
    GameObject impassableCliff; //201
    GameObject cliff; //202
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
    GameObject sandyGround; //213
    GameObject bareRock; //214
    GameObject trench; //215
    GameObject uncrossableBodyOfWater; //301
    GameObject shallowBodyOfWater; //302
    GameObject waterhole; //303
    GameObject crossableWatercourse; //304
    GameObject smallCrossableWatercourse; //305
    GameObject seasonalWaterChannel; //306
    GameObject uncrossableMarsh; //307
    GameObject marsh; //308
    GameObject narrowMarsh; //309
    GameObject indistinctMarsh; //310
    GameObject well; //311
    GameObject spring; //312
    GameObject prominentWaterFeature; //313
    GameObject openLand; //401
    GameObject openLandWithTrees; //402
    GameObject openLandWithBushes; //402.1
    GameObject roughOpenLand; //403
    GameObject roughOpenLandWithTrees; //404
    GameObject roughOpenLandWithBushes; //404.1
    GameObject forest; //405
    GameObject vegetationSlow; //406
    GameObject vegetationSlowOneDir; //406.1
    GameObject vegetationSlowGoodVis; //407
    GameObject vegetationWalk; //408
    GameObject vegetationWalkOneDir; //408.1;408.2
    GameObject vegetationWalkGoodVis; //409
    GameObject vegetationFight; //410
    GameObject vegetationFightOneDir; //410.1;410.2;410.3
    GameObject cultivatedLand; //412
    GameObject orchard; //413
    GameObject vineyard; //414
    GameObject cultivationBoundary; //415
    GameObject distinctVegetationBoundary; //416
    public GameObject prominentLargeTree; //417
    public GameObject prominentSmallTree; //418
    public GameObject prominentVegetationFeature; //419
    GameObject pavedArea; //501
    GameObject wideRoad; //502
    GameObject road; //503
    GameObject vehicleTrack; //504
    GameObject footpath; //505
    GameObject smallFootpath; //506
    GameObject lessDistinctSmallFootpath; //507
    GameObject linearTrace; //508
    public GameObject railway; //509
    public GameObject powerLine; //510
    public GameObject majorPowerLine; //511
    GameObject bridgeTunnel; //512
    public GameObject wall; //5131
    public GameObject retainingWall; //5132
    public GameObject ruinedWall; //514
    public GameObject impassableWall; //515
    public GameObject fence; //516
    public GameObject ruinedFence; //517
     public GameObject impassableFence; //518
    GameObject crossingPointFence; //519
    GameObject areaNotEnter; //520
    GameObject building; //521
    GameObject canopy; //522
    public GameObject ruin; //523
    public GameObject highTower; //524
    public GameObject smallTower; //525
    public GameObject cairn; //526
    public GameObject fodderRack; //527
    GameObject prominentLineFeature; //528
    GameObject prominentImpassableLineFeature; //529
    public GameObject prominentManMadeFeatureRing; //530
    public GameObject prominentManMadeFeatureX; //531
    GameObject stairway; //532
    GameObject magneticNorthLine; //601
    GameObject registrationMark; //602
    GameObject spotHeigh; //603
    GameObject start; //701
    GameObject mapIssuePoint; //702
    GameObject controlPoint; //703
    GameObject controlNumber; //704
    GameObject courseLine; //705
    GameObject finish; //706
    GameObject markedRoute; //707
    GameObject outOfBoundsBound; //708
    GameObject outOfBoundsArea; //709
    GameObject crossingPoint; //710
    GameObject outOfBoundsRoute; //711
    GameObject firstAidPost; //712
    GameObject refreshmentPoint; //713
    GameObject continuingPoint; //715
    public TerrainLayer grassLayer;
    public TerrainLayer sandLayer;
    public TerrainLayer rockLayer;
    public TerrainLayer waterLayer;
    public TerrainLayer pathLayer;
    public TerrainLayer asphaltLayer;
    public string filePath;
    public string coursePath;
    public class MapSymbol
    {
        public string id;
        public List<Vector2> coords = new List<Vector2>();
        public float rotation;
        public string text;
        public int number;
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
        temp.id = 23; temp.isomId = 2011; temp.type = 1; temp.symbolObject = impassableCliff;
        isomSet[23] = temp;
        temp = new isomSymbol();
        temp.id = 24; temp.isomId = 2012; temp.type = 1; temp.symbolObject = impassableCliff;
        isomSet[24] = temp;
        temp = new isomSymbol();
        temp.id = 25; temp.isomId = 2013; temp.type = 1; temp.symbolObject = impassableCliff;
        isomSet[25] = temp;
        temp = new isomSymbol();
        temp.id = 26; temp.isomId = 202; temp.type = 1; temp.symbolObject = cliff;
        isomSet[26] = temp;
        temp = new isomSymbol();
        temp.id = 27; temp.isomId = 2021; temp.type = 1; temp.symbolObject = cliff;
        isomSet[27] = temp;
        temp = new isomSymbol();
        temp.id = 28; temp.isomId = 2022; temp.type = 1; temp.symbolObject = cliff;
        isomSet[28] = temp;
        temp = new isomSymbol();
        temp.id = 29; temp.isomId = 2023; temp.type = 1; temp.symbolObject = cliff;
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
        StartCoroutine(generateTerrain());
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
        terrainData.SetDetailResolution(1024,16);
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
            if(Vector2.Distance(start, end) > 0.1f){
                path.Add(end);
            }
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
            ccwPath.Add(corners[edge]);
            edge = (edge -1 +4)%4;
        }
        ccwPath.Add(end);
        float cwDist = calculatePathLength(start, cwPath);
        float ccwDist = calculatePathLength(start, ccwPath);
        return cwDist <= ccwDist ? cwPath : ccwPath;
    }
    Vector2 snapToOffsetBoundary(Vector2 point, float offset){
        float left = minX;
        float right = maxX;
        float bottom = minY;
        float top = maxY;
        Vector2 bottomLeft = new Vector2(left,bottom);
        Vector2 bottomRight = new Vector2(right, bottom);
        Vector2 topRight = new Vector2(right,top);
        Vector2 topLeft = new Vector2(left, top);
        float distToBottomLeft = Vector2.Distance(point, bottomLeft);
        float distToBottomRight = Vector2.Distance(point, bottomRight);
        float distToTopRight = Vector2.Distance(point,topRight);
        float distToTopLeft = Vector2.Distance(point,topLeft);
        float minCornerDist = Mathf.Min(distToBottomLeft, distToBottomRight, distToTopLeft, distToTopRight);
        if(minCornerDist < 2f){
            if(minCornerDist == distToBottomLeft)
                return bottomLeft;
            if(minCornerDist == distToBottomRight)
                return bottomRight;
            if(minCornerDist == distToTopRight)
                return topRight;
            return topLeft;
        }
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
        float minXl = float.MaxValue, minYl = float.MaxValue, maxXl = float.MinValue, maxYl = float.MinValue;
        foreach(Vector2 coord in contour.coords){
            if(coord.x < minXl) 
                minXl = coord.x;
            if(coord.x > maxXl)
                maxXl = coord.x;
            if(coord.y < minYl)
                minYl = coord.y;
            if(coord.y > maxYl)
                maxYl = coord.y;
        }
        float width = maxXl - minXl;
        float height = maxYl - minYl;
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
        slopeLine.rotation += Mathf.PI / 2f;
        Vector2 direction = new Vector2(
            Mathf.Cos(slopeLine.rotation),
            Mathf.Sin(slopeLine.rotation)
        );
        Vector2 offset = direction * 7f;
        Vector2 testPoint = slopeLine.coords[0] + offset;
        print(slopeLine.rotation);
        if(isPointInPolygon(testPoint, closeOpenContour(contour.coords,contour.isClosed,0)))
            {print(contour.isClosed);
            return 2;}
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
        terrain.gameObject.layer = 7;
        int resolution = data.heightmapResolution;
        float[,] heights = new float[resolution,resolution];
        float minElev = float.MaxValue;
        float maxElev = float.MinValue;
        float sampleInterval = 5f;
        for(int i = 0; i < contours.Count; i++){
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
        if(pathLayer != null)
            layers.Add(pathLayer);
        if(asphaltLayer != null)
            layers.Add(asphaltLayer);
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
        if(id >= 110 && id <= 119){
            return 3;
        }
        if(id >= 105 && id <= 109 && id !=107){
            return 4;
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
    private List<List<Vector2>> treeCoords = new List<List<Vector2>>();
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
        List<PathData> paths = new List<PathData>();
        foreach(MapSymbol symbol in omap){
            isomSymbol refSym = isomSet[int.Parse(symbol.id)];
            int layerIndex = getTerrainLayerIndex(int.Parse(symbol.id));
            int id = int.Parse(symbol.id);
            if(refSym.type == 2){
                if(layerIndex >= 0){
                    paintArea(alphamap, symbol.coords, layerIndex, alphamapWidth, alphamapHeight);
                }
                if(id == 80){// white forest
                    generateTreePosition(symbol.coords,1.2f, 2);
                    treeCoords.Add(symbol.coords);
                }
                else if(id >= 81 && id <= 83){//vegetationSlow
                    generateTreePosition(symbol.coords,2,2);
                    spawnSlowZones(symbol.coords,0);
                    treeCoords.Add(symbol.coords);
                }
                else if(id >= 84 && id <= 87)//vegetationWalk
                {
                    generateTreePosition(symbol.coords,3, 0);
                    spawnSlowZones(symbol.coords,1);
                    treeCoords.Add(symbol.coords);
                }
                else if(id >= 88  && id <= 92)//vegetationFight
                {
                    generateTreePosition(symbol.coords,6,0);
                    spawnSlowZones(symbol.coords,2);
                    treeCoords.Add(symbol.coords);
                }
            } 
            if(id >= 115 && id <= 119){
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 0.4f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
            else if(id == 114){
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 0.6f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
            else if(id == 113){
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 0.7f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
            else if(id == 112){
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 1.3f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
            else if(id == 111)
            {
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 2.5f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
            else if(id == 110){
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 4f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
            else if(id == 109){
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 10f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
            else if(id >= 105 && id <= 108 && id != 107){
                PathData path = new PathData();
                path.coords = symbol.coords;
                path.width = 5f;
                path.indexLayer = layerIndex;
                paths.Add(path);
            }
        paintPath(paths, alphamap, alphamapWidth, alphamapHeight);
        data.SetAlphamaps(0,0,alphamap);
        }
    }
    /*
        Path painting
    */
    public class PathData
    {
        public List<Vector2> coords;
        public float width;
        public int indexLayer;
    }
    void paintPath(List<PathData> paths, float [,,] alphamap, int awidth, int aheight){
        for(int y = 0; y < aheight; y++){
            for(int x = 0; x < awidth; x++){
                Vector2 worldPos = new Vector2(
                    minX + (float)x / (awidth-1)*(maxX-minX),
                    minY + (float)y / (aheight-1)*(maxY-minY)
                );
                foreach(var path in paths){
                    for(int i = 0; i < path.coords.Count-1; i++){
                        Vector2 start = path.coords[i];
                        Vector2 end = path.coords[i+1];
                        if(path.width == 0.6f){
                            System.Random rand = new System.Random();
                            float rNum = rand.Next(2);
                            if(rNum == 1){
                                if(distancePointToSegment(worldPos,start,end) <= path.width){
                                    for(int j = 0; j < alphamap.GetLength(2); j++){
                                        alphamap[y,x,j] = 0f;
                                    }
                                    alphamap[y,x,path.indexLayer] = 1f;
                                }
                            }
                        }
                        else if(distancePointToSegment(worldPos,start,end) <= path.width){
                            for(int j = 0; j < alphamap.GetLength(2); j++){
                                alphamap[y,x,j] = 0f;
                            }
                            alphamap[y,x,path.indexLayer] = 1f;
                        }
                    }
                }
            }
        }
    }
    /*
        Tree painting
    */
    public GameObject[] treePrefabs;
    TreePrototype[] treePrototypes;
    public float treeBillboardDistance = 50f;
    public float treeCullDistance = 1000f;
    public int maxTreesPerCell = 100;
    public PhysicsMaterial terrainCollider;
    void configureTreeLOD(){ // Setup customizable setting for the user later
        TerrainData data = terrain.terrainData;
        terrain.treeBillboardDistance = treeBillboardDistance;
        terrain.treeDistance = treeCullDistance;
        terrain.treeCrossFadeLength = 5f;
        terrain.treeMaximumFullLODCount = 25;
        terrain.detailObjectDistance = 80f;
        terrain.detailObjectDensity = 0.3f;
        terrain.heightmapPixelError = 5f;
        terrain.basemapDistance = 500f;
        terrain.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        terrain.materialTemplate.enableInstancing = true;
    }
    void setupTreePrototypes(){
        TerrainData data = terrain.terrainData;
        treePrototypes = new TreePrototype[treePrefabs.Length];
        for(int i = 0; i < treePrefabs.Length; i++){
            TreePrototype prototype = new TreePrototype();
            prototype.prefab = treePrefabs[i];
            prototype.bendFactor = 0.1f;
            if(treePrefabs[i]!=null){
                MeshRenderer renderer = treePrefabs[i].GetComponent<MeshRenderer>();
                if(renderer != null && renderer.sharedMaterial != null){
                    renderer.sharedMaterial.enableInstancing = true;
                }
            }
            treePrototypes[i] = prototype;
        }
        if(treePrototypes != null && treePrototypes.Length > 0){
            data.treePrototypes = treePrototypes;
            data.RefreshPrototypes();
        }
        configureTreeLOD();
    }
    void spawnTreesOnTerrain(List<Vector2> coords, int treePrototypeIndex){
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
        terrain.GetComponent<TerrainCollider>().enabled = true;
        terrain.GetComponent<TerrainCollider>().material = terrainCollider;
        terrain.GetComponent<TerrainCollider>().enabled = false;
        terrain.GetComponent<TerrainCollider>().enabled = true;
    }
    void generateTreePosition(List<Vector2> coords, float density, int treePrototypeIndex = -1){
        if(coords.Count == 1){
            spawnTreesOnTerrain(coords, treePrototypeIndex);
            return;
        }
        else if(coords.Count < 3)
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
    void generateBackgroundTrees(){
        TerrainData data = terrain.terrainData;
        Vector3 terrainSize = data.size;
        Vector3 terrainPos = terrain.transform.position;
        float area = terrainSize.x*terrainSize.z;
        float density = 1.2f;
        int treeCount = Mathf.RoundToInt((area/100)*density);
        List<Vector2> treePositions = new List<Vector2>();
        System.Random rand = new System.Random();
        float minTreeDistance = 1f;
        for(int i = 0, j = 0; i < treeCount && j < treeCount*2; j++){
            float x = terrainPos.x + (float)rand.NextDouble()*terrainSize.x;
            float z = terrainPos.z + (float)rand.NextDouble()*terrainSize.z;
            Vector2 randomPos = new Vector2(x,z);
            bool isValid = true;
            foreach(var coords in treeCoords){
                if(isPointInPolygon(randomPos,coords)){
                    isValid = false;
                    break;
                }
            }
            if(!isValid){
                continue;
            }
            bool tooClose = false;
            foreach(Vector2 tree in treePositions){
                if(Vector2.Distance(randomPos, tree) < minTreeDistance){
                    tooClose = true;
                    break;
                }
            }
            if(!tooClose){
                treePositions.Add(randomPos);
                i++;
            }
        }
        spawnTreesOnTerrain(treePositions,2);
    }
    /*
        Spawn point objects on terrain
    */
    void addTerrainDetails(GameObject prefab, Vector2 position){
        TerrainData data = terrain.terrainData;
        int detailIndex = -1;
        DetailPrototype[] details = data.detailPrototypes;
        for(int i = 0; i < details.Length; i++){
            if(details[i].prototype == prefab){
                detailIndex = i;
                break;
            }
        }
        if(detailIndex < 0){
            DetailPrototype detailPrototype = new DetailPrototype();
            detailPrototype.prototype = prefab;
            detailPrototype.renderMode = DetailRenderMode.VertexLit;
            detailPrototype.usePrototypeMesh = true;
            Array.Resize(ref details, details.Length+1);
            details[details.Length-1] = detailPrototype;
            data.detailPrototypes = details;
            detailIndex = details.Length-1;
        }
        Vector3 terrainPos = new Vector3(position.x, 0, position.y)-terrain.transform.position;
        float normalizedX = terrainPos.x/data.size.x;
        float normalizedZ = terrainPos.z/data.size.z;
        int detailX = Mathf.RoundToInt(normalizedX*data.detailWidth);
        int detailZ = Mathf.RoundToInt(normalizedZ*data.detailHeight);
        if(detailX < 0 || detailX > data.detailWidth || detailZ < 0 || detailZ > data.detailWidth)
            return;
        int[,] detailLayer = data.GetDetailLayer(0,0,data.detailWidth,data.detailHeight, detailIndex);
        detailLayer[detailZ, detailX] = 16;
        data.SetDetailLayer(0,0,detailIndex,detailLayer);
    }
    /*
        Setup the grass
    */
    public SO_GrassSettings grassSettings;
    public SO_GrassToolSettings grassToolSettings;
    public bool generateGrass = true;
    public LayerMask grassLayerMask = ~0;
    private GrassComputeScript grassCompute;
    public float grassDensityPerSquareMeter = 0.0001f;
    Camera grassRendererCamera;
    public Material grassShaderMaterial;
    bool isGrassValid(Vector2 coord){
        float colDist = 3f;
        foreach(MapSymbol symbol in delSymbols){
            int type = isomSet[int.Parse(symbol.id)].type;
            int isomId = isomSet[int.Parse(symbol.id)].isomId;
            if((isomId >= 401 && isomId <= 404) || (isomId >= 4010 && isomId < 4050)){
                continue;
            }
            if(type == 0){
                if(Vector2.Distance(symbol.coords[0],coord) < colDist){
                    return false;
                }
            }
            if(type == 1){
                for(int i = 0; i < symbol.coords.Count-1; i++){
                    Vector2 start = symbol.coords[i];
                    Vector2 end = symbol.coords[i+1];
                    if(distancePointToSegment(coord,start,end) < colDist){
                        return false;
                    }
                }
            }
            if(type == 3){
                if(isPointInPolygon(coord,symbol.coords)){
                    return false;
                }
            }
        }
        return true;
    }
    void setupGrassSystem(){     
        if(!generateGrass || grassSettings == null){
            Debug.LogWarning("Grass generation disabled");
            return;
        }
        if(terrain == null){
            Debug.LogError("Terrain is null");
            return;
        }
        GameObject grassCamObj = new GameObject("GrassRendererCamera");
        grassRendererCamera = grassCamObj.AddComponent<Camera>();
        grassRendererCamera.orthographic = true;
        grassRendererCamera.enabled = false;
        grassRendererCamera.clearFlags = CameraClearFlags.SolidColor;
        grassRendererCamera.backgroundColor = Color.black;
        grassRendererCamera.cullingMask = grassLayerMask;
        grassRendererCamera.depth = -100;
        TerrainData data = terrain.terrainData;
        Vector3 terrainCenter = terrain.transform.position + new Vector3(data.size.x / 2f, data.size.y + 50f, data.size.z/2f);
        grassCamObj.transform.position = terrainCenter;
        grassCamObj.transform.rotation = Quaternion.Euler(90f,0f,0f);
        grassRendererCamera.orthographicSize = Mathf.Max(data.size.x, data.size.z)/2f;
        GameObject grassObject = new GameObject("GrassSystem");
        grassCompute = grassObject.AddComponent<GrassComputeScript>();
        grassCompute.currentPresets = grassSettings;
        Type grassComputeType = typeof(GrassComputeScript);
        var instantiatedMaterialVar = grassComputeType.GetField("m_InstantiatedMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if(instantiatedMaterialVar != null){
            instantiatedMaterialVar.SetValue(grassCompute, grassShaderMaterial);
        }
        GameObject renderMapObj = new GameObject("TerrainRenderer");
        RenderTerrainMap renderMap = renderMapObj.gameObject.AddComponent<RenderTerrainMap>();
        renderMap.camToDrawWith = grassRendererCamera;
        renderMap.resolution = 512;
        renderMap.adjustScaling = 2.5f;
        renderMap.repeatRate = 5f;
        Type renderMapType = typeof(RenderTerrainMap);
        var terrainsField  = renderMapType.GetField("terrains", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if(terrainsField != null){
            Terrain[] terrainsArray = new Terrain[]{terrain};
            terrainsField.SetValue(renderMap, terrainsArray);
        }
        var layerField = renderMapType.GetField("layer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if(layerField != null){
            layerField.SetValue(renderMap, grassLayerMask);
        }
        renderMap.enabled = true;
        renderMap.DrawDiffuseMap();
        generateGrassPositions();
    }
    void generateGrassPositions(){
        if(grassCompute == null){
            Debug.LogWarning("Grass compute script null");
            return;
        }
        TerrainData data = terrain.terrainData;
        Vector3 terrainSize = data.size;
        Vector3 terrainPos = terrain.transform.position;
        float totalArea = terrainSize.x*terrainSize.z;
        int totalGrassCount = Mathf.RoundToInt(totalArea*grassDensityPerSquareMeter);
        List<GrassData> grassDataList = new List<GrassData>();
        System.Random rand = new System.Random();
        for(int i = 0; i < totalGrassCount; i++){
            float x = terrainPos.x + (float)rand.NextDouble()*terrainSize.x;
            float z = terrainPos.z + (float)rand.NextDouble()*terrainSize.z;
            if(!isGrassValid(new Vector2(x,z))){
                continue;
            }
            Vector3 worldPos = new Vector3(x,0,z);
            float y = terrain.SampleHeight(worldPos);
            Vector3 normalizedPos = new Vector3(
                (x-terrainPos.x)/terrainSize.x,
                0,
                (z-terrainPos.z)/terrainSize.z
            );
            int alphaX = Mathf.FloorToInt(normalizedPos.x * (data.alphamapWidth-1));
            int alphaZ = Mathf.FloorToInt(normalizedPos.z * (data.alphamapHeight-1));
            float[,,] alphamap = data.GetAlphamaps(alphaX, alphaZ,1,1);
            if(alphamap[0,0,0] > 0.5f){
                Vector3 normal = data.GetInterpolatedNormal(normalizedPos.x, normalizedPos.z);
                GrassData grass = new GrassData();
                grass.position = new Vector3(x,y,z);
                grass.normal = normal;
                grass.length = new Vector2(
                    (float) rand.NextDouble(),
                    (float)rand.NextDouble()
                );
                Color baseColor = new Color(0.1f,0.3f,0.1f);
                grass.color = new Vector3(
                    baseColor.r + ((float)rand.NextDouble()-0.5f)*0.2f,
                    baseColor.g + ((float)rand.NextDouble()-0.5f)*0.2f,
                    baseColor.b + ((float)rand.NextDouble()-0.5f)*0.2f
                );
                grassDataList.Add(grass);
            }
        }
        grassCompute.SetGrassPaintedDataList = grassDataList;
        grassCompute.Reset();
    }
    /*
        Slow zones setup
    */
    public GameObject vegetationSlowSZ;
    public GameObject vegetationWalkSZ;
    public GameObject vegetationFightSZ;
    public MovePlayer PlayerVR;
    public MovePlayerPC PlayerPC;
    public GameObject PlayerVRGO;
    public GameObject PlayerPCGO;
    GameObject slowZonePar;
    void spawnSlowZones(List<Vector2> coords, int vegeType/*0-Slow;1-Walk;2-Fight*/){
        if(slowZonePar == null){
            slowZonePar = new GameObject("SlowZones");
        }
        GameObject slowZonePrefab;
        switch(vegeType){
            case 0: slowZonePrefab = vegetationSlowSZ; break;
            case 1: slowZonePrefab = vegetationWalkSZ; break;
            case 2: slowZonePrefab = vegetationFightSZ; break;
            default: Debug.LogWarning("No vegetype"); return;
        }
        MeshFilter meshFilter = slowZonePrefab.GetComponent<MeshFilter>();
        if(meshFilter == null || meshFilter.sharedMesh == null){
            Debug.LogWarning("Check slow zone prefab");
            return;
        }
        Bounds bounds = meshFilter.sharedMesh.bounds;
        float baseLength = bounds.size.z;
        float baseWidth = bounds.size.x;
        for(int i = 0; i < coords.Count; i++){
            float length = Vector2.Distance(coords[i],coords[(i+1)%coords.Count]);
            Vector2 direction = (coords[(i+1)%coords.Count]-coords[i]).normalized;
            float angle = Mathf.Atan2(direction.x,direction.y)*Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector2 midpoint = (coords[i]+coords[(i+1)%coords.Count])/2f;
            float terrainHeight = terrain.SampleHeight(new Vector3(midpoint.x,0,midpoint.y));
            Vector3 position = new Vector3(midpoint.x, terrainHeight, midpoint.y);
            float scaleZ = length/baseLength;
            float scaleX = 0f;
            float scaleY = 500f;
            GameObject slowZoneObj = Instantiate(slowZonePrefab, position, rotation, slowZonePar.transform);
            slowZoneObj.name = $"SlowZoneSegment_{i}_vegeType_{vegeType}";
            slowZoneObj.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            slowMovement slowScript = slowZoneObj.AddComponent<slowMovement>();
            slowScript.movePlayer1 = PlayerVR;
            slowScript.movePlayerPC1 = PlayerPC;
            slowScript.Player1 = PlayerVRGO;
            slowScript.PlayerPC1 = PlayerPCGO;
            switch(vegeType){
                case 0: 
                    slowScript.slowZone = 1;
                    slowScript.newSpeed = 4;
                    break;
                case 1:
                    slowScript.slowZone = 2;
                    slowScript.newSpeed = 3;
                    break;
                case 2:
                    slowScript.slowZone = 3;
                    slowScript.newSpeed = 2;
                    break;
                default:
                    slowScript.slowZone = 1;
                    slowScript.newSpeed = 4;
                    break;
            }
        }
    }
    /*
        Landform features
    */
    public class pointLfFeature
    {
        public Vector2 coords;
        public float elevChange;
        public int id;
    }
    public class lineLfFeature
    {
        public List<Vector2> coords;
        public float elevChange;
        public int id;
    }
    public class areaLfFeature
    {
        public List<Vector2> coords;
        public float elevChange;
        public int id;
    }
    void processLandformFeatures(){
        TerrainData data = terrain.terrainData;
        int res = data.heightmapResolution;
        float[,] heights = data.GetHeights(0,0,res,res);
        List<pointLfFeature> pointFeatures = new List<pointLfFeature>();
        List<lineLfFeature> lineFeatures = new List<lineLfFeature>();
        List<areaLfFeature> areaFeatures = new List<areaLfFeature>();
        float terrainMaxHeight = data.size.y;
        float widthl;
        float heightl;
        float area;
        int hoesCount;
        System.Random rand = new System.Random();
        foreach(MapSymbol symbol in omap){
            pointLfFeature ptft = new pointLfFeature();
            ptft.id = -1;
            lineLfFeature lift = new  lineLfFeature();
            lift.id = -1;
            areaLfFeature alft = new areaLfFeature();
            alft.id = -1;
            float minXl = float.MaxValue, maxXl = float.MinValue;
            float minYl = float.MaxValue, maxYl = float.MinValue;
            switch(isomSet[int.Parse(symbol.id)].isomId){
                case 104: //earthBank - already hanled by contours
                    continue;
                case 1051: //earhtWall
                    lift.coords = symbol.coords;
                    lift.elevChange = 1.5f/terrainMaxHeight;
                    lineFeatures.Add(lift);
                    continue;
                case 106: //brokenearthWall
                    lift.coords = symbol.coords;
                    lift.elevChange = 1.5f/terrainMaxHeight;
                    lineFeatures.Add(lift);
                    continue;
                case 107: //erosionGully
                    lift.coords = symbol.coords;
                    lift.elevChange = -1.5f/terrainMaxHeight;
                    lineFeatures.Add(lift);
                    continue;
                case 215:
                    lift.coords = symbol.coords;
                    lift.elevChange = -1.5f/terrainMaxHeight;
                    lineFeatures.Add(lift);
                    continue;
                case 108: //smallErosionGully;
                    lift.coords = symbol.coords;
                    lift.elevChange = -1f/terrainMaxHeight;
                    lineFeatures.Add(lift);
                    continue;
                case 109: //smalKnoll
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = 2f/terrainMaxHeight;
                    pointFeatures.Add(ptft);
                    continue;
                case 110: //elongatedKnoll
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = 2f/terrainMaxHeight;
                    pointFeatures.Add(ptft);
                    continue;
                case 111: //smallDepression
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -1.5f/terrainMaxHeight;
                    pointFeatures.Add(ptft);
                    continue;
                case 112: //pit
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -3f/terrainMaxHeight;
                    pointFeatures.Add(ptft);
                    continue;
                case 113: //brokenGround
                    for(int i = 0; i < symbol.coords.Count; i++){
                        minXl = Mathf.Min(minXl,symbol.coords[i].x);
                        maxXl = Mathf.Max(maxXl,symbol.coords[i].x);
                        minYl = Mathf.Min(minYl,symbol.coords[i].y);
                        maxYl = Mathf.Max(maxYl,symbol.coords[i].x);
                    }
                    widthl = maxXl-minXl;
                    heightl = maxYl-minYl;
                    area = heightl*widthl;
                    hoesCount = Mathf.RoundToInt(area*0.2f);
                    for(int i = 0, j = 0; i < hoesCount && j < hoesCount*2; j++){
                        Vector2 rarndomPos = new Vector2(
                            minXl + (float)rand.NextDouble()*widthl,
                            minYl + (float)rand.NextDouble()*heightl
                        );
                        if(isPointInPolygon(rarndomPos, symbol.coords)){
                            pointLfFeature lPtft = new pointLfFeature();
                            lPtft.coords = rarndomPos;
                            lPtft.elevChange = -0.2f/terrainMaxHeight;
                            pointFeatures.Add(lPtft);
                            i++;
                        }
                    }
                    continue;
                case 114: //veryBrokenGround
                    for(int i = 0; i < symbol.coords.Count; i++){
                        minXl = Mathf.Min(minXl,symbol.coords[i].x);
                        maxXl = Mathf.Max(maxXl,symbol.coords[i].x);
                        minYl = Mathf.Min(minYl,symbol.coords[i].y);
                        maxYl = Mathf.Max(maxYl,symbol.coords[i].x);
                    }
                    widthl = maxXl-minXl;
                    heightl = maxYl-minYl;
                    area = heightl*widthl;
                    hoesCount = Mathf.RoundToInt(area*0.3f);
                    for(int i = 0, j = 0; i < hoesCount && j < hoesCount*2; j++){
                        Vector2 rarndomPos = new Vector2(
                            minXl + (float)rand.NextDouble()*widthl,
                            minYl + (float)rand.NextDouble()*heightl
                        );
                        if(isPointInPolygon(rarndomPos, symbol.coords)){
                            pointLfFeature lPtft = new pointLfFeature();
                            lPtft.coords = rarndomPos;
                            lPtft.elevChange = -0.2f/terrainMaxHeight;
                            pointFeatures.Add(lPtft);
                            i++;
                        }
                    }
                    continue;
                case 115: //prominent land feature
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = 0f;
                    pointFeatures.Add(ptft);
                    continue;
                case 303: //waterhole
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -3f/terrainMaxHeight;
                    ptft.id = 303;
                    pointFeatures.Add(ptft);
                    continue;
                case 311: //well
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -1.5f/terrainMaxHeight;
                    ptft.id = 311;
                    pointFeatures.Add(ptft);
                    continue;
                case 312: //spring
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -1.5f/terrainMaxHeight;
                    ptft.id = 312;
                    pointFeatures.Add(ptft);
                    continue;
                case 313: //prominentwaterfeature
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -1.5f/terrainMaxHeight;
                    ptft.id = 313;
                    pointFeatures.Add(ptft);
                    continue;
                case 304:
                    lift.coords = symbol.coords;
                    lift.elevChange = -0.5f/terrainMaxHeight;
                    lift.id = 304;
                    lineFeatures.Add(lift);
                    continue;
                case 305:
                    lift.coords = symbol.coords;
                    lift.elevChange = -1f/terrainMaxHeight;
                    lift.id = 305;
                    lineFeatures.Add(lift);
                    continue;
                case 306:
                    lift.coords = symbol.coords;
                    lift.elevChange = -2f/terrainMaxHeight;
                    lift.id = 306;
                    lineFeatures.Add(lift);
                    continue;
                case 301:
                    alft.coords = symbol.coords;
                    alft.elevChange = -1f/terrainMaxHeight;
                    areaFeatures.Add(alft);
                    generateWaterArea(symbol.coords);
                    continue;
                case 2031:
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -3f/terrainMaxHeight;
                    pointFeatures.Add(ptft);
                    ptft.id = 2031;
                    continue;
                case 2032:
                    ptft.coords = symbol.coords[0];
                    ptft.elevChange = -3f/terrainMaxHeight;
                    pointFeatures.Add(ptft);
                    ptft.id = 2032;
                    continue;
                default:
                    continue; 
            }
        }
        changeHeightOfArea(areaFeatures,heights,res);
        changeHeightOfLine(lineFeatures,heights,res);
        changeHeightOfPoint(pointFeatures,heights,res);
        data.SetHeights(0,0,heights);
    }
    void changeHeightOfPoint(List<pointLfFeature> feat, float[,] heights, int res){
        float radius = 3f;
        for(int y = 0; y < res; y++){
            for(int x = 0; x < res; x++){
                Vector2 worldPos = new Vector2(
                    minX + (float)x / (res-1)*(maxX-minX),
                    minY + (float)y / (res-1)*(maxY-minY)
                );
                foreach(var ft in feat){
                    float dist = Vector2.Distance(worldPos, ft.coords);
                    if(dist < radius){
                        float factor = 1f-(dist/radius);
                        factor = Mathf.SmoothStep(0,1,factor);
                        heights[y,x] += ft.elevChange*factor;
                        heights[y,x] = Mathf.Max(0,heights[y,x]);
                    }
                }
            }
        }
        foreach(var ft in feat){
            if(ft.id == 312 || ft.id == 313 || ft.id == 311 || ft.id == 303){
                generateWaterPlane(ft.coords,radius);
            }
            if(ft.id == 2031 ){
                CreatePointObject(isomSet[30],new List<Vector2>{ft.coords});
            }
            if(ft.id == 2032){
                CreatePointObject(isomSet[31],new List<Vector2>{ft.coords});
            }
        }
    }
    void changeHeightOfLine(List<lineLfFeature> feats, float[,] heights, int res){
        float lineWidth = 2f;
        for(int y = 0; y < res; y++){
            for(int x = 0; x < res; x++){
                Vector2 worldPos = new Vector2(
                    minX + (float)x / (res-1)*(maxX-minX),
                    minY + (float)y / (res-1)*(maxY-minY)
                );
                foreach(var ft in feats){
                    if(ft.id == 306){
                        lineWidth = 1f;
                    }
                    else if(ft.id == 304){
                        lineWidth = 3f;
                    }
                    else{
                        lineWidth = 2f;
                    }
                    float minDist = float.MaxValue;
                    for(int i = 0; i < ft.coords.Count-1; i++){
                        float dist = distancePointToSegment(worldPos,ft.coords[i],ft.coords[i+1]);
                        minDist = Mathf.Min(minDist,dist);
                    }
                    if(minDist<lineWidth){
                        float factor = 1f-(minDist/lineWidth);
                        factor = Mathf.SmoothStep(0,1,factor);
                        heights[y,x] += ft.elevChange*factor;
                        heights[y,x] = Mathf.Max(0,heights[y,x]);
                    }
                }
            }
        }
        foreach(var ft in feats){
            if(ft.id == 306){
                lineWidth = 1f;
            }
            else if(ft.id == 304){
                lineWidth = 3f;
            }
            else{
                lineWidth = 2f;
            }
            if(ft.id == 305 || ft.id == 306 || ft.id == 304)
            {
                generateWaterLine(ft.coords,lineWidth);
            }
        }
    }
    void changeHeightOfArea(List<areaLfFeature> feats, float[,] heights, int res){
        float slopeTolerance = 0f;
        for(int y = 0; y < res; y++){
            for(int x = 0; x < res; x++){
                Vector2 worldPos = new Vector2(
                    minX + (float)x/(res-1)*(maxX-minX),
                    minY + (float)y/(res-1)*(maxY-minY)
                );
                foreach(var ft in feats){
                    if(isPointInPolygon(worldPos, ft.coords)){
                        heights[y,x]+=ft.elevChange;
                        heights[y,x]=Mathf.Max(heights[y,x],0);
                        continue;
                    }
                    float minDist = float.MaxValue;
                    for(int i = 0; i < ft.coords.Count; i++){
                        float dist = distancePointToSegment(worldPos, ft.coords[i],ft.coords[(i+1)%ft.coords.Count]);
                        minDist = Mathf.Min(minDist, dist);
                    }
                    if(minDist < slopeTolerance){
                        float factor = 1f-(minDist/slopeTolerance);
                        factor = Mathf.SmoothStep(0,1,factor);
                        heights[y,x]+=ft.elevChange*factor;
                        heights[y,x]=Mathf.Max(0,heights[y,x]);
                    }
                }
            }
        }
    }
    /*
        Generate water
    */
    public GameObject waterPlane;
    GameObject waterParent;
    void generateWaterPlane(Vector2 coords, float radius){
        if(waterParent == null){
            waterParent = new GameObject("Water Parent");
        }
        MeshFilter meshFilter = waterPlane.GetComponent<MeshFilter>();
        if(meshFilter == null || meshFilter.sharedMesh == null){
            Debug.LogWarning("Check water prefab");
            return;
        }
        float minX = coords.x-radius, maxX = coords.x+radius;
        float minY = coords.y-radius, maxY = coords.y-radius;
        Bounds bounds = meshFilter.sharedMesh.bounds;
        float baseLength = bounds.size.z;
        float baseWidth = bounds.size.x;
        float scaleX = radius*2/baseWidth;
        float scaleY = 1f;
        float scaleZ = radius*2/baseLength;
        float height = Mathf.Min(terrain.SampleHeight(new Vector3(minX, 0, minY)), terrain.SampleHeight(new Vector3(minX, 0, maxY)), terrain.SampleHeight(new Vector3(maxX, 0, minY)), terrain.SampleHeight(new Vector3(maxX, 0, maxY)));
        Vector3 pos = new Vector3(coords.x, height, coords.y);
        GameObject waterObj = Instantiate(waterPlane, pos, Quaternion.Euler(0,0,0), waterParent.transform);
        waterObj.transform.localScale = new Vector3(scaleX,scaleY,scaleZ);
        waterObj.name = "Water Plane";
    }
    
void generateWaterLine(List<Vector2> coords, float width){
    if(waterParent == null){
        waterParent = new GameObject("Water Parent");
    }
    MeshFilter meshFilter = waterPlane.GetComponent<MeshFilter>();
    if(meshFilter == null || meshFilter.sharedMesh == null){
        Debug.LogWarning("Check water prefab");
    }
    Bounds bounds = meshFilter.sharedMesh.bounds;
    float baseWidth = bounds.size.x;
    float baseLength = bounds.size.z;
    for(int i = 0; i < coords.Count-1; i++){
        Vector2 start = coords[i];
        Vector2 end = coords[i+1];
        float segmentLength = Vector2.Distance(start,end);
        float segmentWidth = width*2.5f;
        float objectLength = 2f;
        Vector2 direction = (end-start).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        int objectCount = Mathf.CeilToInt(segmentLength/objectLength)+1;
        for(int j = 0; j < objectCount; j++){
            Vector2 pos2D = start+direction*j*objectLength;
            float terrainHeight = terrain.SampleHeight(new Vector3(pos2D.x,0,pos2D.y));
            Vector3 position = new Vector3(pos2D.x, terrainHeight,pos2D.y);
            Vector3 normal = getTerrainNormal(position);
            Quaternion yRotation = Quaternion.Euler(0, angle, 0);
            Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal);
            Quaternion rotation = normalRotation * yRotation;
            GameObject obj = Instantiate(waterPlane, position, rotation, waterParent.transform);
            obj.transform.localScale = new Vector3(segmentWidth/baseWidth,0.5f,(objectLength+0.1f)/baseLength);
            obj.name = "Water Line";
        }
    }
}
    void generateWaterArea(List<Vector2> coords){
        float minY = coords[0].y, maxY=coords[0].y, minX=coords[0].x, maxX=coords[0].x, avHeight = float.MaxValue;
        foreach(var coord in coords){
            minY = Mathf.Min(minY, coord.y);
            maxY = Mathf.Max(maxY, coord.y);
            minX = Mathf.Min(minX, coord.x);
            maxX = Mathf.Max(maxX, coord.x);
            avHeight = Mathf.Min(avHeight,terrain.SampleHeight(new Vector3(coord.x,0,coord.y)));
        }
        if(waterParent == null){
            waterParent = new GameObject("Water Parent");
        }
        MeshFilter meshFilter = waterPlane.GetComponent<MeshFilter>();
        if(meshFilter == null || meshFilter.sharedMesh == null){
            Debug.LogWarning("Check water prefab");
            return;
        }
        Bounds bounds = meshFilter.sharedMesh.bounds;
        float baseLength = bounds.size.z;
        float baseWidth = bounds.size.x;
        float tileSize = 2f;
        for(float x = minX; x < maxX; x+=tileSize){
            for(float y = minY; y < maxY; y += tileSize){
                Vector2 center = new Vector2(x+tileSize/2f, y+tileSize/2f);
                if(isPointInPolygon(center,coords)){
                    Vector3 pos = new Vector3(center.x,avHeight,center.y);
                    Vector3 normal = getTerrainNormal(pos);
                    Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal);
                    Vector3 scale = new Vector3((tileSize+0.1f)/baseWidth,1f,(tileSize+0.1f)/baseLength);
                    GameObject obj = Instantiate(waterPlane, pos, normalRotation,waterParent.transform);
                    obj.transform.localScale = scale;
                    obj.name = "Water Tile";
                }
                else{
                    Vector2[] corners = new Vector2[]{
                        new Vector2(x,y),
                        new Vector2(x+tileSize,y),
                        new Vector2(x, y + tileSize),
                        new Vector2(x+tileSize,y+tileSize)
                    };
                    bool spawn = false;
                    foreach(var corner in corners){
                        if(isPointInPolygon(corner,coords)){
                            spawn = true;
                            break;
                        }
                    }
                    if(spawn){
                        Vector3 pos = new Vector3(center.x,avHeight,center.y);
                        Vector3 normal = getTerrainNormal(pos);
                        Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal);
                        Vector3 scale = new Vector3((tileSize+0.1f)/baseWidth,1f,(tileSize+0.1f)/baseLength);
                        GameObject obj = Instantiate(waterPlane, pos, normalRotation,waterParent.transform);
                        obj.transform.localScale = scale;
                        obj.name = "Water Tile";
                    }
                }
            }
        }
    }
    Vector3 getTerrainNormal(Vector3 pos){
        TerrainData data = terrain.terrainData;
        Vector3 terrainPos = pos-terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainPos.x/data.size.x,
            terrainPos.y/data.size.y,
            terrainPos.z/data.size.z
        );
        return data.GetInterpolatedNormal(normalizedPos.x,normalizedPos.z);
    }
    /*
        Setup the course
    */
    public GameObject controlPrefab;
    public GameObject PC;
    public GameObject VR;
    private Vector3 startPos;
    public GameObject VRObjects;
    void spawnContorls(){
        MapSymbol start = new MapSymbol(), finish = new MapSymbol();
        List<MapSymbol> controls = new List<MapSymbol>(), controlNumbers = new List<MapSymbol>();
        MapSymbol simpleCourse = new MapSymbol();
        bool isSimple = false;
        foreach(var control in courseSym){
            print(control.coords[0].x);
            int id = int.Parse(control.id);
            if(id == 1){
                start = control;
            }
            else if(id == 2){
                controls.Add(control);
            }
            else if(id == 3){
                controlNumbers.Add(control);
            }
            else if(id == 6){
                finish = control;
            }
            else if(id == 0){
                simpleCourse = control;
                isSimple = true;
                break;
            }
        }
        GameObject coursePar = new GameObject("Course Parent");
        Checkpoints checkpoints = null;
        if(PlayerPCGO.activeInHierarchy){
            checkpoints = PlayerPCGO.GetComponent<Checkpoints>();
        }
        else if(PlayerVRGO.activeInHierarchy){
            checkpoints = PlayerVRGO.GetComponent<Checkpoints>();
        }
        GameObject[] controlsObj;
        if(isSimple){
            controlsObj = new GameObject[simpleCourse.coords.Count];
            GameObject startObj = Instantiate(controlPrefab, new Vector3(simpleCourse.coords[0].x, terrain.SampleHeight(new Vector3(simpleCourse.coords[0].x,0,simpleCourse.coords[0].y)),simpleCourse.coords[0].y),Quaternion.identity,coursePar.transform);
            GameObject finishObj = Instantiate(controlPrefab, new Vector3(simpleCourse.coords[simpleCourse.coords.Count].x, terrain.SampleHeight(new Vector3(simpleCourse.coords[simpleCourse.coords.Count].x,0,simpleCourse.coords[simpleCourse.coords.Count].y)),simpleCourse.coords[simpleCourse.coords.Count].y),Quaternion.identity, coursePar.transform);
            startObj.name = "Start";
            finishObj.name = "Finish";
            startObj.GetComponent<playAudioWhenPunched>().checkpoints = checkpoints;
            startObj.GetComponent<playAudioWhenPunched>().controlName = "Start";
            finishObj.GetComponent<playAudioWhenPunched>().checkpoints = checkpoints;
            finishObj.GetComponent<playAudioWhenPunched>().controlName = "Finish";
            for(int i = 1; i < simpleCourse.coords.Count-1; i++){
                GameObject obj = Instantiate(controlPrefab, new Vector3(simpleCourse.coords[i].x, terrain.SampleHeight(new Vector3(simpleCourse.coords[i].x,0,simpleCourse.coords[i].y)),simpleCourse.coords[i].y),Quaternion.identity, coursePar.transform);
                obj.name = i.ToString();
                obj.GetComponent<playAudioWhenPunched>().checkpoints = checkpoints;
                obj.GetComponent<playAudioWhenPunched>().controlName = i.ToString();
                controlsObj[i] = obj;
            }
            checkpoints.start = startObj;
            checkpoints.finish = finishObj;
            checkpoints.checkpoints = controlsObj;
            startPos = startObj.transform.position;
        }
        else{
            GameObject startObj = Instantiate(controlPrefab, new Vector3(start.coords[0].x, terrain.SampleHeight(new Vector3(start.coords[0].x,0,start.coords[0].y)),start.coords[0].y), Quaternion.identity, coursePar.transform),
            finishObj = Instantiate(controlPrefab, new Vector3(finish.coords[0].x, terrain.SampleHeight(new Vector3(finish.coords[0].x, 0, finish.coords[0].y)),finish.coords[0].y), Quaternion.identity, coursePar.transform);
            startObj.name = "Start";
            finishObj.name = "Finish";
            startObj.GetComponent<playAudioWhenPunched>().checkpoints = checkpoints;
            startObj.GetComponent<playAudioWhenPunched>().controlName = "Start";
            finishObj.GetComponent<playAudioWhenPunched>().checkpoints = checkpoints;
            finishObj.GetComponent<playAudioWhenPunched>().controlName = "Finish";
            foreach(var controlNum in controlNumbers){
                float minDist = float.MaxValue;
                int minId = 0;
                for(int i = 0; i < controls.Count; i++){
                    float dist = Vector2.Distance(controlNum.coords[0], controls[i].coords[0]);
                    if(dist == Mathf.Min(minDist,dist)){
                        minDist = dist;
                        minId = i;
                    }
                }
                controls[minId].number = int.Parse(controlNum.text);
                controls[minId].text = controlNum.text;
            }
            controls.Sort((a,b)=>a.number.CompareTo(b.number));
            controlsObj = new GameObject[controls.Count];
            for(int i = 0; i < controls.Count; i++){
                MapSymbol control = controls[i];
                GameObject obj = Instantiate(controlPrefab, new Vector3(control.coords[0].x, terrain.SampleHeight(new Vector3(control.coords[0].x,0,control.coords[0].y)),control.coords[0].y),Quaternion.identity, coursePar.transform);
                obj.name = control.text;
                playAudioWhenPunched controlPlay = obj.GetComponent<playAudioWhenPunched>();
                controlPlay.checkpoints = checkpoints;
                controlPlay.controlName = control.text;
                controlsObj[i] = obj;
            }
            checkpoints.start = startObj;
            checkpoints.finish = finishObj;
            checkpoints.checkpoints = controlsObj; 
            startPos = startObj.transform.position;
        }
        LayerMask collisionMask = ~((1<<7) | (1<<10));
        List<string> unknownCoords = new List<string>();
        foreach(var obj in controlsObj){
            bool isSpawned = true;
            for(int i = 0; i < 100 && Physics.CheckSphere(obj.transform.position,1f,collisionMask); i++){
                Vector3 randomOffset = UnityEngine.Random.insideUnitCircle*20f;
                Vector3 testPos = new Vector3(obj.transform.position.x+randomOffset.x,0,obj.transform.position.z+randomOffset.z);
                testPos.y = terrain.SampleHeight(testPos);
                if(!Physics.CheckSphere(testPos,1f,collisionMask)){
                    obj.transform.position = testPos;
                    break;
                }
                if(i==99){
                    isSpawned = false;
                }
            }
            if(!isSpawned){
                unknownCoords.Add(obj.name);    
            }
        }
        if(unknownCoords.Count > 0){
            string errorText = $"Control(s) {string.Join(", ",unknownCoords)} might not be accessible. \n Consider changing the position a little bit.";
            StartCoroutine(displayError(errorText));
        }
        Vector3 spawnPos = new Vector3(startPos.x+3f,0,startPos.z);
        spawnPos.y = terrain.SampleHeight(spawnPos);
        Vector3 direction = startPos-spawnPos;
        direction.y = 0;
        float angle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.Euler(0,angle,0);
        PC.GetComponent<MovePlayerPC>().enabled = false;
        PC.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        PC.GetComponent<Rigidbody>().position = spawnPos;
        PC.GetComponent<Rigidbody>().rotation = lookRotation;
        PC.GetComponent<MovePlayerPC>().enabled = true;
        VR.GetComponent<MovePlayer>().enabled = false;
        VR.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        VR.GetComponent<Rigidbody>().position = spawnPos;
        VR.GetComponent<Rigidbody>().rotation = lookRotation;
        VR.GetComponent<MovePlayer>().enabled = true;
        VRObjects.transform.position = spawnPos;
        VRObjects.transform.rotation = lookRotation;
    }
    public GameObject popup;
    public TextMeshProUGUI popupText;
    IEnumerator displayError(string text){
        popup.SetActive(true);
        popupText.text = text;
        yield return null;
    }
    /*
        Remove unnecessary trees
    */
    private List<MapSymbol> delSymbols;
    void removeTrees(){
        TerrainData data = terrain.terrainData;
        List<TreeInstance> validTrees = new List<TreeInstance>();
        delSymbols = new List<MapSymbol>();
        foreach(var symbol in omap){
            int isomId = isomSet[int.Parse(symbol.id)].isomId;
            if((isomId >= 401 && isomId <= 404) || (isomId >= 4010 && isomId < 4050) || (isomId >= 201 && isomId <= 207) ||(isomId >= 2010 && isomId < 2080) || (isomId >= 301 && isomId <= 313) || (isomId >= 3010 && isomId < 3140) || (isomId >= 417 && isomId <= 419) || (isomId >= 501 && isomId <= 511) || (isomId >= 5010 && isomId < 5120) || (isomId >= 513 && isomId <= 518) || (isomId >= 523 && isomId <= 531)){
                delSymbols.Add(symbol);
            }
        }
        float colDist = 1.5f;
        foreach(TreeInstance tree in data.treeInstances){
            Vector3 treeWorldPos = Vector3.Scale(tree.position, data.size)+terrain.transform.position;
            bool delete = false;
            foreach(MapSymbol symbol in delSymbols){
                int type = isomSet[int.Parse(symbol.id)].type;
                Vector2 treePos2D = new Vector2(treeWorldPos.x,treeWorldPos.z);
                if(type == 0){
                    if(Vector2.Distance(treePos2D, symbol.coords[0]) < colDist){
                        delete = true;
                    }
                }
                else if(type == 1){
                    for(int i = 0; i < symbol.coords.Count-1; i++){
                        Vector2 start = symbol.coords[i];
                        Vector2 end  = symbol.coords[i+1];
                        if(distancePointToSegment(treePos2D,start,end) < colDist){
                            delete = true;
                        }
                    }
                }
                else if(type == 2){
                    if(isPointInPolygon(treePos2D, symbol.coords)){
                        delete = true;
                    }
                }
                if(delete)
                    break;
            }
            if(!delete){
                validTrees.Add(tree);
            }
        }
        data.treeInstances = validTrees.ToArray();
        data.RefreshPrototypes();
        foreach(var symbol in omap){
            int isomId = isomSet[int.Parse(symbol.id)].isomId;
            if(isomId == 402 || isomId == 404){
                generateTreePosition(symbol.coords,0.8f,3);
            }
            if(isomId == 4021 || isomId == 4041){
                generateTreePosition(symbol.coords, 1.67f, 4);
            }
        }
    }
    // ISOM 2017 symbol set (for now)
    IEnumerator parseOMAP()
    {
        omap = new List<MapSymbol>();
        XmlDocument mapFile = new XmlDocument();
        if(loadingBar != null)
            loadingBar.fillAmount = 0.05f;
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
        yield return null;
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
        mapFile = new XmlDocument();
        try{
            mapFile.Load(coursePath);
            print("success");
        }
        catch(System.Exception e)
        {
            print($"Failed {e.Message}");
        }
        courseSym = new List<MapSymbol>();
        nsmgr = new XmlNamespaceManager(mapFile.NameTable);
        nsmgr.AddNamespace("omap", "http://openorienteering.org/apps/mapper/xml/v2");
        contentPart = mapFile.SelectNodes("//omap:parts/omap:part", nsmgr);
        foreach(XmlNode contentObject in contentPart){
            XmlNodeList symbolNodes = contentObject.SelectNodes("omap:objects/omap:object", nsmgr);
            foreach(XmlNode symbolNode in symbolNodes){
                MapSymbol symbol = ParseObject(symbolNode, nsmgr);
                if(symbol != null && symbol.coords.Count > 0){
                    courseSym.Add(symbol);
                    print(symbol.coords);
                }
            }
        }
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.1f;
        generateMapBounds();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.2f;
        generateHeightMap();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.4f;
        processLandformFeatures();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.5f;
        setupTerrainLayers();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.6f;
        paintTerrain();
        generateBackgroundTrees();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.7f;
        spawnContorls();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.8f;
        int objcount = 0;
        foreach(MapSymbol symbol in omap)
        {
            isomSymbol refSym = isomSet[int.Parse(symbol.id)];
            int id = refSym.isomId;
            if(refSym.symbolObject == null)
                continue;
            objcount++;
            if(objcount %50 == 0)
                yield return null;
            if (refSym.type == 0)
            {
                CreatePointObject(refSym, symbol.coords);
            }
            if (refSym.type == 1)
            {
                if (refSym.symbolObject == null) continue;
                CreateLineObject(refSym, symbol.coords);
            }
            if (refSym.type == 2)
            {
                CreateAreaObject(refSym, symbol.coords);
            }
        }
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.85f;
        removeTrees();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 0.9f;
        setupGrassSystem();
        yield return null;
        if(loadingBar != null)
            loadingBar.fillAmount = 1f;
        yield return new WaitForSeconds(0.5f);
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
        obj.transform.Rotate(0,UnityEngine.Random.Range(0,360),0);
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
        MeshFilter meshFilter = symbol.symbolObject.GetComponent<MeshFilter>();
        if(meshFilter == null || meshFilter.sharedMesh == null){
            Debug.LogWarning($"No mesh found for {symbol.isomId}");
            return;
        }
        Bounds bounds = meshFilter.sharedMesh.bounds;
        float objectLengthX = bounds.size.x;
        float objectLengthZ = bounds.size.z;
        float objectLength;
        bool longerSide; // 0 - x, 1 - z
        if(objectLengthX > objectLengthZ){
            objectLength = objectLengthX;
            longerSide = false;
        }
        else{
            objectLength = objectLengthZ;
            longerSide = true;
        }
        GameObject parentObj = new GameObject($"LineObject_{symbol.symbolObject.name}_id_{symbol.isomId}");
        for(int i = 0; i < coords.Count-1; i++){
            Vector2 start = coords[i];
            Vector2 end = coords[i+1];
            float segmentLength = Vector2.Distance(start,end);
            Vector2 direction = (end-start).normalized;
            int objectCount = Mathf.CeilToInt(segmentLength/objectLength);
            float spacing = objectLength;
            float angle = -Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
            Quaternion rotation;
            if(!longerSide){
                rotation = Quaternion.Euler(0,angle, 0);
            }
            else{
                rotation = Quaternion.Euler(0,90+angle,0);
            }
            if(symbol.id >= 121 && symbol.id <= 124){
                objectCount = 1;
                rotation.y = rotation.y+90;
            }
            for(int j = 0; j < objectCount; j++){
                float lengthToAdd = j*spacing;
                Vector2 pos2D = start+direction*lengthToAdd;
                float terrainHeight = terrain.SampleHeight(new Vector3(pos2D.x,0,pos2D.y));
                Vector3 position = new Vector3(pos2D.x,terrainHeight,pos2D.y);
                Vector3 normal = getTerrainNormal(position);
                Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up,normal);
                Quaternion finalRotation = normalRotation*rotation;
                GameObject obj= Instantiate(symbol.symbolObject, position, finalRotation, parentObj.transform);
                obj.name = $"{symbol.symbolObject.name}";
            }
        }
        if(symbol.id >= 121 && symbol.id <= 124){
            CreatePointObject(symbol,new List<Vector2>{coords[coords.Count-1]});
        }
    }
    void CreateAreaObject(isomSymbol symbol, List<Vector2> coords)
    {
        print(symbol.id);
        if (coords.Count < 3)
        {
            Debug.LogWarning($"Too few coordinates for {symbol.isomId}");
            return;
        }
        float minXBox = coords[0].x, maxXBox = coords[0].x;
        float minYBox = coords[0].y, maxYBox = coords[0].y;
        foreach(var coord in coords){
            if(coord.x<minXBox) minXBox = coord.x;
            if(coord.x>maxXBox) maxXBox = coord.x;
            if(coord.y<minYBox) minYBox = coord.y;
            if(coord.y>maxYBox) maxYBox = coord.y;
        }
        int density = 1;
        float width = maxXBox-minXBox;
        float height = maxYBox-minYBox;
        float area = Mathf.Abs(maxXBox-minXBox)*Mathf.Abs(maxYBox-minYBox);
        int objCount = Mathf.RoundToInt(area*density);
        System.Random rand = new System.Random();
        float tolerance = 2f;
        List<Vector2> existingObj = new List<Vector2>();
        GameObject objParent = new GameObject($"{symbol.id}_Parent");
        for(int i = 0, j = 0; i < objCount && j < objCount; j++){
            Vector2 randomPos = new Vector2(
                minXBox + (float)rand.NextDouble()*width,
                minYBox + (float)rand.NextDouble()*height
            );
            if(!isPointInPolygon(randomPos, coords))
                continue;
            bool isGood = true;
            foreach(Vector2 otherObj in existingObj){
                if(Vector2.Distance(randomPos, otherObj) < tolerance){
                    isGood = false;
                    break;
                }
            }
            if(isGood){
                Vector3 pos = new Vector3(randomPos.x,terrain.SampleHeight(new Vector3(randomPos.x,0,randomPos.y)),randomPos.y);
                GameObject obj = Instantiate(symbol.symbolObject, pos, Quaternion.Euler((float)rand.NextDouble()*360,(float)rand.NextDouble()*360,(float)rand.NextDouble()*360), objParent.transform);
                obj.name = $"AreaObject{symbol.id}";
                i++;
            }
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
        float minXl = coords[0].x;
        float maxXl = coords[0].x;
        float minYl = coords[0].y;
        float maxYl = coords[0].y;
        for (int i = 1; i < coords.Count; i++)
        {
            if (coords[i].x < minXl)
                minXl = coords[i].x;
            if (coords[i].x > maxXl)
                maxXl = coords[i].x;
            if (coords[i].y < minYl)
                minYl = coords[i].y;
            if (coords[i].y > maxYl)
                minYl = coords[i].y;
        }
        float rangeX = maxXl - minXl;
        float rangeY = maxYl - minYl;
        if (rangeX == 0)
            rangeX = 1f;
        if (rangeY == 0)
            rangeY = 1f;
        for(int i = 0; i < coords.Count; i++)
        {
            uvs[i] = new Vector2(
                (coords[i].x - minXl) / rangeX,
                (coords[i].y - minYl) / rangeY
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
                        if (float.TryParse(coords[0], out float x) && float.TryParse(coords[1], out float y)){
                            y = -y;
                            symbol.coords.Add(new Vector2(x / 100f, y / 100f));
                        }
                    }
                }
            }
        }
        XmlNode textNode = symbolNode.SelectSingleNode("omap:text", nsmgr);
        if(textNode != null){
            symbol.text = textNode.InnerText;
        }
        return symbol;
    }
    /*
        Scene loading
    */
    public GameObject loadingScreen;
    public Image loadingBar;
    public GameObject player;
    public GameObject fadeToBlack;
    IEnumerator generateTerrain()
    {
        if(loadingScreen != null)
            loadingScreen.SetActive(true);
        yield return parseOMAP();
         fadeToBlack.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        if(loadingScreen != null)
            loadingScreen.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        fadeToBlack.SetActive(false);
        player.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        popup.SetActive(false);
    }
    void Update()
    {
        
    }
}