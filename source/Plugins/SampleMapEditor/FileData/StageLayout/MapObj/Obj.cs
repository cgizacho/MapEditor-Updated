﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    /// <summary>
    /// Represents an Obj placed on the stage.
    /// </summary>
    public class Obj : SpatialObject, IByamlSerializable, IStageReferencable
    {
        [ByamlMember]
        public bool IsLinkDest { get; set; } = false;
        
        [ByamlMember]
        public string LayerConfigName { get; set; }

        [ByamlMember]
        public string ModelName { get; set; } = "";

        [ByamlMember]
        public string UnitConfigName { get; set; }

        [ByamlMember]
        public List<LinkInfo> Links { get; set; } = new List<LinkInfo>();



        [ByamlMember(Optional = true)]
        public int Team { get; set; }






        /*private static int objcounter = 0;
        private static string GetNewObjId => $"obj{objcounter++}";*/
        public Obj()
        {
            this.LayerConfigName = "Cmn";
            this.UnitConfigName = "Obj_GeneralBox_15x15x15_Blitz";

            this.Scale = new ByamlVector3F(1, 1, 1);
        }


        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Reads data from the given <paramref name="dictionary"/> to satisfy members.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{String, Object}"/> to read the data from.</param>
        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            //ModeInclusion = ModeInclusion.FromDictionary(dictionary);
        }

        /// <summary>
        /// Writes instance members into the given <paramref name="dictionary"/> to store them as BYAML data.
        /// </summary>
        /// <param name="dictionary">The <see cref="Dictionary{String, Object}"/> to store the data in.</param>
        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            //ModeInclusion.ToDictionary(dictionary);
        }

        /// <summary>
        /// Allows references of course data objects to be resolved to provide real instances instead of the raw values
        /// in the BYAML.
        /// </summary>
        /// <param name="stageDefinition">The <see cref="StageDefinition"/> providing the objects.</param>
        public void DeserializeReferences(StageDefinition stageDefinition)
        {
            /*// References to paths and their points.
            try
            {
                Path = _pathIndex == null ? null : courseDefinition.Paths[_pathIndex.Value];
                PathPoint = _pathPointIndex == null ? null : Path.Points[_pathPointIndex.Value];
                LapPath = _lapPathIndex == null ? null : courseDefinition.LapPaths[_lapPathIndex.Value];
                LapPathPoint = _lapPathPointIndex == null ? null : LapPath.Points[_lapPathPointIndex.Value];
                ObjPath = _objPathIndex == null ? null : courseDefinition.ObjPaths[_objPathIndex.Value];
                ObjPathPoint = _objPathPointIndex == null ? null : ObjPath.Points[_objPathPointIndex.Value];
                EnemyPath1 = _enemyPath1Index == null ? null : courseDefinition.EnemyPaths[_enemyPath1Index.Value];
                EnemyPath2 = _enemyPath2Index == null ? null : courseDefinition.EnemyPaths[_enemyPath2Index.Value];
                ItemPath1 = _itemPath1Index == null ? null : courseDefinition.ItemPaths[_itemPath1Index.Value];
                ItemPath2 = _itemPath2Index == null ? null : courseDefinition.ItemPaths[_itemPath2Index.Value];
            }
            catch
            {

            }

            // References to other unit objects.
            ParentArea = _areaIndex == null ? null : courseDefinition.Areas[_areaIndex.Value];
            ParentObj = _objIndex == null ? null : courseDefinition.Objs[_objIndex.Value];

            if (Path != null) Path.References.Add(this);*/
        }

        /// <summary>
        /// Allows references between course objects to be serialized into raw values stored in the BYAML.
        /// </summary>
        /// <param name="stageDefinition">The <see cref="StageDefinition"/> providing the objects.</param>
        public void SerializeReferences(StageDefinition stageDefinition)
        {
            /*if (Single == false) Single = null;
            if (NoCol == false) NoCol = null;

            // References to paths and their points.
            _pathIndex = Path == null ? null : (int?)courseDefinition.Paths.IndexOf(Path);
            _pathPointIndex = PathPoint?.Index;
            _lapPathIndex = LapPath == null ? null : (int?)courseDefinition.LapPaths.IndexOf(LapPath);
            _lapPathPointIndex = LapPathPoint?.Index;
            _objPathIndex = ObjPath == null ? null : (int?)courseDefinition.ObjPaths.IndexOf(ObjPath);
            _objPathPointIndex = (ObjPath != null && ObjPathPoint != null) ? ObjPathPoint.Index : null;
            _enemyPath1Index = EnemyPath1 == null ? null : (int?)courseDefinition.EnemyPaths.IndexOf(EnemyPath1);
            _enemyPath2Index = EnemyPath2 == null ? null : (int?)courseDefinition.EnemyPaths.IndexOf(EnemyPath2);
            _itemPath1Index = ItemPath1 == null ? null : (int?)courseDefinition.ItemPaths.IndexOf(ItemPath1);
            _itemPath2Index = ItemPath2 == null ? null : (int?)courseDefinition.ItemPaths.IndexOf(ItemPath2);

            // References to other unit objects.
            _areaIndex = ParentArea == null ? null : (int?)courseDefinition.Areas.IndexOf(ParentArea);
            _objIndex = ParentObj == null ? null : (int?)courseDefinition.Objs.IndexOf(ParentObj);

            if (_objIndex == -1) _objIndex = null;
            if (_areaIndex == -1) _areaIndex = null;
            if (_pathIndex == -1) _pathIndex = null;
            if (_pathPointIndex == -1) _pathPointIndex = null;
            if (_objPathIndex == -1) _objPathIndex = null;
            if (_objPathPointIndex == -1) _objPathPointIndex = null;

            if (_pathIndex != null)
                System.Console.WriteLine($"Obj {ObjId} _pathIndex {_pathIndex}");
            if (_objPathIndex != null)
                System.Console.WriteLine($"Obj {ObjId} _objPathIndex {_pathIndex}");*/

            List<string> UsedObjIds = new List<string>();
            for (int i = 0; i < stageDefinition.Actors.Count; i++)
            {
                if (stageDefinition.Actors[i].InstanceID != null)
                {
                    UsedObjIds.Add(stageDefinition.Actors[i].InstanceID);
                }
            }

            List<int> UsedObjIdNums = new List<int>();
            UsedObjIds.ForEach(i => UsedObjIdNums.Add(int.Parse(i.Replace("obj", "")))); //Convert.ToInt32(i)));

            int objIdxCounter = 0;
            for (int i = 0; i < stageDefinition.Actors.Count; i++)
            {
                if (stageDefinition.Actors[i].InstanceID != null) continue;
                while (UsedObjIdNums.Contains(objIdxCounter))
                    objIdxCounter++;
                stageDefinition.Actors[i].InstanceID = $"obj{objIdxCounter++}";
            }
        }


        public Obj Clone()
        {
            return new Obj()
            {
                // ~ Fill out once Obj is fully defined ~
                UnitConfigName = this.UnitConfigName,
                IsLinkDest = this.IsLinkDest,
                LayerConfigName = this.LayerConfigName,
                Translate = this.Translate,
                Scale = this.Scale,
                Rotate = this.Rotate,
                Links = this.Links,
                Team = this.Team,
            };
        }




        public static string GetResourceName(string actorName)
        {
            var db = GlobalSettings.ActorDatabase;
            if (db.ContainsKey(actorName) && db[actorName].FmdbName.Length > 0)
                return db[actorName].ResName;
            else
                return "";
        }

        public static string FindFilePath(string resName)
        {
            string resFilePath = GlobalSettings.GetContentPath($"Model\\{resName}.bfres.zs");

            if (File.Exists(resFilePath)) return resFilePath;

            return string.Empty;
        }
    }
}
