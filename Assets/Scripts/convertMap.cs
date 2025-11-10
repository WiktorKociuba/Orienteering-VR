using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine.Rendering;
using NUnit.Framework.Constraints;

public class convertMap : MonoBehaviour
{
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
    public string filePath;
    public class MapSymbol
    {
        public string id;
        public List<Vector2> coords = new List<Vector2>();
    }
    public List<isomSymbol> isomSet = new List<isomSymbol>();
    void Start()
    {
        /*
            0 - point
            1 - line
            2 - area
        */
        isomSymbol temp = new isomSymbol();
        temp.id = 0; temp.isomId = 101; temp.type = 1; temp.symbolObject = contour;
        isomSet.Add(temp);
        temp.id = 1; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 2; temp.isomId = 102; temp.type = 1; temp.symbolObject = indexContoure;
        isomSet.Add(temp);
        temp.id = 3; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 4; temp.isomId = 103; temp.type = 1; temp.symbolObject = formLine;
        isomSet.Add(temp);
        temp.id = 5; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 6; temp.isomId = 104; temp.type = 1; temp.symbolObject = earthBank;
        isomSet.Add(temp);
        temp.id = 7; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 8; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 9; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 10; temp.isomId = 1051; temp.type = 1; temp.symbolObject = earthWall;
        isomSet.Add(temp);
        temp.id = 11; temp.isomId = 106; temp.type = 1; temp.symbolObject = ruinedEarthWall;
        isomSet.Add(temp);
        temp.id = 12; temp.isomId = 107; temp.type = 1; temp.symbolObject = erosionGully;
        isomSet.Add(temp);
        temp.id = 13; temp.isomId = 108; temp.type = 1; temp.symbolObject = smallErosionGully;
        isomSet.Add(temp);
        temp.id = 14; temp.isomId = 109; temp.type = 0; temp.symbolObject = smallKnoll;
        isomSet.Add(temp);
        temp.id = 15; temp.isomId = 110; temp.type = 0; temp.symbolObject = smallElongatedKnoll;
        isomSet.Add(temp);
        temp.id = 16; temp.isomId = 111; temp.type = 0; temp.symbolObject = smallDepression;
        isomSet.Add(temp);
        temp.id = 17; temp.isomId = 112; temp.type = 0; temp.symbolObject = pit;
        isomSet.Add(temp);
        temp.id = 18; temp.isomId = 113; temp.type = 2; temp.symbolObject = brokenGround;
        isomSet.Add(temp);
        temp.id = 19; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 20; temp.isomId = 114; temp.type = 2; temp.symbolObject = veryBrokenGround;
        isomSet.Add(temp);
        temp.id = 21; temp.isomId = 115; temp.type = 0; temp.symbolObject = prominentLandformFeature;
        isomSet.Add(temp);
        temp.id = 22; temp.isomId = 201; temp.type = 1; temp.symbolObject = impassableCliff;
        isomSet.Add(temp);
        temp.id = 23; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 24; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 25; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 26; temp.isomId = 202; temp.type = 1; temp.symbolObject = cliff;
        isomSet.Add(temp);
        temp.id = 27; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 28; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 29; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 30; temp.isomId = 2031; temp.type = 0; temp.symbolObject = rockyPit;
        isomSet.Add(temp);
        temp.id = 31; temp.isomId = 2032; temp.type = 0; temp.symbolObject = dangerousPit;
        isomSet.Add(temp);
        temp.id = 32; temp.isomId = 204; temp.type = 0; temp.symbolObject = Boulder;
        isomSet.Add(temp);
        temp.id = 33; temp.isomId = 2045; temp.type = 0; temp.symbolObject = Boulder;
        isomSet.Add(temp);
        temp.id = 34; temp.isomId = 205; temp.type = 0; temp.symbolObject = largeBoulder;
        isomSet.Add(temp);
        temp.id = 35; temp.isomId = 206; temp.type = 0; temp.symbolObject = giganticBoulder;
        isomSet.Add(temp);
        temp.id = 36; temp.isomId = 207; temp.type = 0; temp.symbolObject = boulderCluster;
        isomSet.Add(temp);
        temp.id = 37; temp.isomId = 2071; temp.type = 0; temp.symbolObject = boulderCluster;
        isomSet.Add(temp);
        temp.id = 38; temp.isomId = 208; temp.type = 2; temp.symbolObject = boulderField;
        isomSet.Add(temp);
        temp.id = 39; temp.isomId = 2081; temp.type = 2; temp.symbolObject = boulderField;
        isomSet.Add(temp);
        temp.id = 40; temp.isomId = 2082; temp.type = 2; temp.symbolObject = boulderField;
        isomSet.Add(temp);
        temp.id = 41; temp.isomId = 209; temp.type = 2; temp.symbolObject = denseBoulderField;
        isomSet.Add(temp);
        temp.id = 42; temp.isomId = 210; temp.type = 2; temp.symbolObject = stonyGroundRun;
        isomSet.Add(temp);
        temp.id = 43; temp.isomId = 2101; temp.type = 2; temp.symbolObject = stonyGroundRun;
        isomSet.Add(temp);
        temp.id = 44; temp.isomId = 211; temp.type = 2; temp.symbolObject = stonyGroundWalk;
        isomSet.Add(temp);
        temp.id = 45; temp.isomId = 212; temp.type = 2; temp.symbolObject = stonyGroundFight;
        isomSet.Add(temp);
        temp.id = 46; temp.isomId = 213; temp.type = 2; temp.symbolObject = sandyGround;
        isomSet.Add(temp);
        temp.id = 47; temp.isomId = 214; temp.type = 2; temp.symbolObject = bareRock;
        isomSet.Add(temp);
        temp.id = 48; temp.isomId = 215; temp.type = 1; temp.symbolObject = trench;
        isomSet.Add(temp);
        temp.id = 49; temp.isomId = 301; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet.Add(temp);
        temp.id = 50; temp.isomId = 3011; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet.Add(temp);
        temp.id = 51; temp.isomId = 3012; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet.Add(temp);
        temp.id = 52; temp.isomId = 3013; temp.type = 2; temp.symbolObject = uncrossableBodyOfWater;
        isomSet.Add(temp);
        temp.id = 53; temp.isomId = -1; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 54; temp.isomId = 302; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet.Add(temp);
        temp.id = 55; temp.isomId = 3021; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet.Add(temp);
        temp.id = 56; temp.isomId = 3022; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet.Add(temp);
        temp.id = 57; temp.isomId = 3023; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet.Add(temp);
        temp.id = 58; temp.isomId = 3025; temp.type = 2; temp.symbolObject = shallowBodyOfWater;
        isomSet.Add(temp);
        temp.id = 59; temp.isomId = 303; temp.type = 0; temp.symbolObject = waterhole;
        isomSet.Add(temp);
        temp.id = 60; temp.isomId = 304; temp.type = 1; temp.symbolObject = crossableWatercourse;
        isomSet.Add(temp);
        temp.id = 61; temp.isomId = 305; temp.type = 1; temp.symbolObject = smallCrossableWatercourse;
        isomSet.Add(temp);
        temp.id = 62; temp.isomId = 306; temp.type = 1; temp.symbolObject = seasonalWaterChannel;
        isomSet.Add(temp);
        temp.id = 63; temp.isomId = 307; temp.type = 2; temp.symbolObject = uncrossableMarsh;
        isomSet.Add(temp);
        temp.id = 64; temp.isomId = 3071; temp.type = 2; temp.symbolObject = uncrossableMarsh;
        isomSet.Add(temp);
        temp.id = 65; temp.isomId = -3072; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 66; temp.isomId = 308; temp.type = 2; temp.symbolObject = marsh;
        isomSet.Add(temp);
        temp.id = 67; temp.isomId = 3081; temp.type = 2; temp.symbolObject = marsh;
        isomSet.Add(temp);
        temp.id = 68; temp.isomId = 309; temp.type = 1; temp.symbolObject = narrowMarsh;
        isomSet.Add(temp);
        temp.id = 69; temp.isomId = 310; temp.type = 2; temp.symbolObject = indistinctMarsh;
        isomSet.Add(temp);
        temp.id = 70; temp.isomId = 3101; temp.type = 2; temp.symbolObject = indistinctMarsh;
        isomSet.Add(temp);
        temp.id = 71; temp.isomId = 311; temp.type = 0; temp.symbolObject = well;
        isomSet.Add(temp);
        temp.id = 72; temp.isomId = 312; temp.type = 0; temp.symbolObject = spring;
        isomSet.Add(temp);
        temp.id = 73; temp.isomId = 313; temp.type = 0; temp.symbolObject = prominentWaterFeature;
        isomSet.Add(temp);
        temp.id = 74; temp.isomId = 401; temp.type = 2; temp.symbolObject = openLand;
        isomSet.Add(temp);
        temp.id = 75; temp.isomId = 402; temp.type = 2; temp.symbolObject = openLandWithTrees;
        isomSet.Add(temp);
        temp.id = 76; temp.isomId = 4021; temp.type = 2; temp.symbolObject = openLandWithBushes;
        isomSet.Add(temp);
        temp.id = 77; temp.isomId = 403; temp.type = 2; temp.symbolObject = roughOpenLand;
        isomSet.Add(temp);
        temp.id = 78; temp.isomId = 404; temp.type = 2; temp.symbolObject = roughOpenLandWithTrees;
        isomSet.Add(temp);
        temp.id = 79; temp.isomId = 4041; temp.type = 2; temp.symbolObject = roughOpenLandWithBushes;
        isomSet.Add(temp);
        temp.id = 80; temp.isomId = 405; temp.type = 2; temp.symbolObject = forest;
        isomSet.Add(temp);
        temp.id = 81; temp.isomId = 406; temp.type = 2; temp.symbolObject = vegetationSlow;
        isomSet.Add(temp);
        temp.id = 82; temp.isomId = 4061; temp.type = 2; temp.symbolObject = vegetationSlowOneDir;
        isomSet.Add(temp);
        temp.id = 83; temp.isomId = 407; temp.type = 2; temp.symbolObject = vegetationSlowGoodVis;
        isomSet.Add(temp);
        temp.id = 84; temp.isomId = 408; temp.type = 2; temp.symbolObject = vegetationWalk;
        isomSet.Add(temp);
        temp.id = 85; temp.isomId = 4081; temp.type = 2; temp.symbolObject = vegetationWalkOneDir;
        isomSet.Add(temp);
        temp.id = 86; temp.isomId = 4082; temp.type = 2; temp.symbolObject = vegetationWalkOneDir;
        isomSet.Add(temp);
        temp.id = 87; temp.isomId = 409; temp.type = 2; temp.symbolObject = vegetationWalkGoodVis;
        isomSet.Add(temp);
        temp.id = 88; temp.isomId = 410; temp.type = 2; temp.symbolObject = vegetationFight;
        isomSet.Add(temp);
        temp.id = 89; temp.isomId = 4101; temp.type = 2; temp.symbolObject = vegetationFightOneDir;
        isomSet.Add(temp);
        temp.id = 90; temp.isomId = 4102; temp.type = 2; temp.symbolObject = vegetationFightOneDir;
        isomSet.Add(temp);
        temp.id = 91; temp.isomId = 4103; temp.type = 2; temp.symbolObject = vegetationFightOneDir;
        isomSet.Add(temp);
        temp.id = 92; temp.isomId = 4104; temp.type = 2; temp.symbolObject = vegetationFight;
        isomSet.Add(temp);
        temp.id = 93; temp.isomId = 412; temp.type = 2; temp.symbolObject = cultivatedLand;
        isomSet.Add(temp);
        temp.id = 94; temp.isomId = 4121; temp.type = 2; temp.symbolObject = cultivatedLand;
        isomSet.Add(temp);
        temp.id = 95; temp.isomId = 413; temp.type = 2; temp.symbolObject = orchard;
        isomSet.Add(temp);
        temp.id = 96; temp.isomId = 4131; temp.type = 2; temp.symbolObject = orchard;
        isomSet.Add(temp);
        temp.id = 97; temp.isomId = 414; temp.type = 2; temp.symbolObject = vineyard;
        isomSet.Add(temp);
        temp.id = 98; temp.isomId = 4141; temp.type = 2; temp.symbolObject = vineyard;
        isomSet.Add(temp);
        temp.id = 99; temp.isomId = -415; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 100; temp.isomId = 416; temp.type = 1; temp.symbolObject = distinctVegetationBoundary;
        isomSet.Add(temp);
        temp.id = 101; temp.isomId = 4161; temp.type = 1; temp.symbolObject = distinctVegetationBoundary;
        isomSet.Add(temp);
        temp.id = 102; temp.isomId = 417; temp.type = 0; temp.symbolObject = prominentLargeTree;
        isomSet.Add(temp);
        temp.id = 103; temp.isomId = 418; temp.type = 0; temp.symbolObject = prominentSmallTree;
        isomSet.Add(temp);
        temp.id = 104; temp.isomId = 419; temp.type = 0; temp.symbolObject = prominentVegetationFeature;
        isomSet.Add(temp);
        temp.id = 105; temp.isomId = 501; temp.type = 2; temp.symbolObject = pavedArea;
        isomSet.Add(temp);
        temp.id = 106; temp.isomId = 5011; temp.type = 2; temp.symbolObject = pavedArea;
        isomSet.Add(temp);
        temp.id = 107; temp.isomId = -5012; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 108; temp.isomId = 502; temp.type = 1; temp.symbolObject = wideRoad;
        isomSet.Add(temp);
        temp.id = 109; temp.isomId = 5022; temp.type = 1; temp.symbolObject = wideRoad;
        isomSet.Add(temp);
        temp.id = 110; temp.isomId = 503; temp.type = 1; temp.symbolObject = road;
        isomSet.Add(temp);
        temp.id = 111; temp.isomId = 504; temp.type = 1; temp.symbolObject = vehicleTrack;
        isomSet.Add(temp);
        temp.id = 112; temp.isomId = 505; temp.type = 1; temp.symbolObject = footpath;
        isomSet.Add(temp);
        temp.id = 113; temp.isomId = 506; temp.type = 1; temp.symbolObject = smallFootpath;
        isomSet.Add(temp);
        temp.id = 114; temp.isomId = 507; temp.type = 1; temp.symbolObject = lessDistinctSmallFootpath;
        isomSet.Add(temp);
        temp.id = 115; temp.isomId = 508; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet.Add(temp);
        temp.id = 116; temp.isomId = 5081; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet.Add(temp);
        temp.id = 117; temp.isomId = 5082; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet.Add(temp);
        temp.id = 118; temp.isomId = 5083; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet.Add(temp);
        temp.id = 119; temp.isomId = 5084; temp.type = 1; temp.symbolObject = linearTrace;
        isomSet.Add(temp);
        temp.id = 120; temp.isomId = 509; temp.type = 1; temp.symbolObject = railway;
        isomSet.Add(temp);
        temp.id = 121; temp.isomId = 510; temp.type = 1; temp.symbolObject = powerLine;
        isomSet.Add(temp);
        temp.id = 122; temp.isomId = 511; temp.type = 1; temp.symbolObject = majorPowerLine;
        isomSet.Add(temp);
        temp.id = 123; temp.isomId = 5111; temp.type = 1; temp.symbolObject = majorPowerLine;
        isomSet.Add(temp);
        temp.id = 124; temp.isomId = 5112; temp.type = 1; temp.symbolObject = majorPowerLine;
        isomSet.Add(temp);
        temp.id = 125; temp.isomId = 512; temp.type = 0; temp.symbolObject = bridgeTunnel;
        isomSet.Add(temp);
        temp.id = 126; temp.isomId = 5121; temp.type = 0; temp.symbolObject = bridgeTunnel;
        isomSet.Add(temp);
        temp.id = 127; temp.isomId = 5122; temp.type = 0; temp.symbolObject = bridgeTunnel;
        isomSet.Add(temp);
        temp.id = 128; temp.isomId = 513; temp.type = 1; temp.symbolObject = wall;
        isomSet.Add(temp);
        temp.id = 129; temp.isomId = 514; temp.type = 1; temp.symbolObject = ruinedWall;
        isomSet.Add(temp);
        temp.id = 130; temp.isomId = 515; temp.type = 1; temp.symbolObject = impassableWall;
        isomSet.Add(temp);
        temp.id = 131; temp.isomId = 516; temp.type = 1; temp.symbolObject = fence;
        isomSet.Add(temp);
        temp.id = 132; temp.isomId = 517; temp.type = 1; temp.symbolObject = ruinedFence;
        isomSet.Add(temp);
        temp.id = 133; temp.isomId = 518; temp.type = 1; temp.symbolObject = impassableFence;
        isomSet.Add(temp);
        temp.id = 134; temp.isomId = 519; temp.type = 0; temp.symbolObject = crossingPointFence;
        isomSet.Add(temp);
        temp.id = 135; temp.isomId = 520; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet.Add(temp);
        temp.id = 136; temp.isomId = 5201; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet.Add(temp);
        temp.id = 137; temp.isomId = 5202; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet.Add(temp);
        temp.id = 138; temp.isomId = 5203; temp.type = 2; temp.symbolObject = areaNotEnter;
        isomSet.Add(temp);
        temp.id = 139; temp.isomId = 521; temp.type = 2; temp.symbolObject = building;
        isomSet.Add(temp);
        temp.id = 140; temp.isomId = 5211; temp.type = 2; temp.symbolObject = building;
        isomSet.Add(temp);
        temp.id = 141; temp.isomId = 5212; temp.type = 2; temp.symbolObject = building;
        isomSet.Add(temp);
        temp.id = 142; temp.isomId = 5213; temp.type = 2; temp.symbolObject = building;
        isomSet.Add(temp);
        temp.id = 143; temp.isomId = 5214; temp.type = 2; temp.symbolObject = building;
        isomSet.Add(temp);
        temp.id = 144; temp.isomId = 522; temp.type = 2; temp.symbolObject = canopy;
        isomSet.Add(temp);
        temp.id = 145; temp.isomId = 5221; temp.type = 2; temp.symbolObject = canopy;
        isomSet.Add(temp);
        temp.id = 146; temp.isomId = 5222; temp.type = 2; temp.symbolObject = canopy;
        isomSet.Add(temp);
        temp.id = 147; temp.isomId = 523; temp.type = 2; temp.symbolObject = ruin;
        isomSet.Add(temp);
        temp.id = 148; temp.isomId = 5231; temp.type = 2; temp.symbolObject = canopy;
        isomSet.Add(temp);
        temp.id = 149; temp.isomId = 524; temp.type = 0; temp.symbolObject = highTower;
        isomSet.Add(temp);
        temp.id = 150; temp.isomId = 525; temp.type = 0; temp.symbolObject = smallTower;
        isomSet.Add(temp);
        temp.id = 151; temp.isomId = 526; temp.type = 0; temp.symbolObject = cairn;
        isomSet.Add(temp);
        temp.id = 152; temp.isomId = 527; temp.type = 0; temp.symbolObject = fodderRack;
        isomSet.Add(temp);
        temp.id = 153; temp.isomId = 528; temp.type = 1; temp.symbolObject = prominentLineFeature;
        isomSet.Add(temp);
        temp.id = 154; temp.isomId = 529; temp.type = 1; temp.symbolObject = prominentImpassableLineFeature;
        isomSet.Add(temp);
        temp.id = 155; temp.isomId = 530; temp.type = 0; temp.symbolObject = prominentManMadeFeatureRing;
        isomSet.Add(temp);
        temp.id = 156; temp.isomId = 531; temp.type = 0; temp.symbolObject = prominentManMadeFeatureX;
        isomSet.Add(temp);
        temp.id = 157; temp.isomId = 532; temp.type = 1; temp.symbolObject = stairway;
        isomSet.Add(temp);
        temp.id = 158; temp.isomId = 5321; temp.type = 1; temp.symbolObject = stairway;
        isomSet.Add(temp);
        temp.id = 159; temp.isomId = -6011; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 160; temp.isomId = -6012; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 161; temp.isomId = -6013; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 162; temp.isomId = -6014; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 163; temp.isomId = -602; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 164; temp.isomId = -603; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 165; temp.isomId = -6031; temp.type = -1; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 166; temp.isomId = 704; temp.type = 0; temp.symbolObject = null;
        isomSet.Add(temp);
        temp.id = 167; temp.isomId = 799; temp.type = 0; temp.symbolObject = null;
        isomSet.Add(temp);
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
