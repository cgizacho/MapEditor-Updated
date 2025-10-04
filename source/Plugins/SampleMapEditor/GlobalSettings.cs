﻿using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class GlobalSettings
    {
        //public static Dictionary<int, ActorDefinition> ActorDatabase = new Dictionary<int, ActorDefinition>();    // use name instead
        public static Dictionary<string, ActorDefinition> ActorDatabase = new Dictionary<string, ActorDefinition>();

        public static string S3GameVersion1 = "a";
        public static string S3GameVersion2 = "1";
        public static string S3GameVersion3 = "0";

        public static string GamePath { get; set; }

        public static string IPPath { get; set; }

        public static string SDORPath { get; set; }

        public static string ModOutputPath { get; set; }

        public static bool IsLinkingComboBoxActive { get; set; } = true;

        //public static bool IsMK8D => File.Exists($"{GamePath}\\RaceCommon\\TS_PolicePackun\\TS_PolicePackun.bfres"); // no.

        public static PathSettings PathDrawer = new PathSettings();

        /// <summary>
        /// Loads the actor database
        /// </summary>
        public static void LoadDataBase()
        {
            Console.WriteLine("~ Called GlobalSettings.LoadDataBase() ~");
            if (ActorDatabase.Count > 0)
                return;

            LoadActorDb();

            Console.WriteLine("~ Called GlobalSettings.LoadDataBase() ~");
        }

        /// <summary>
        /// Gets content path from either the update, game, or aoc directories based on what is present.
        /// </summary>
        public static string GetContentPath(string relativePath)
        {
            //Update first then base package.
            if (File.Exists($"{ModOutputPath}\\{relativePath}")) return $"{ModOutputPath}\\{relativePath}";
            if (File.Exists($"{IPPath}\\{relativePath}")) return $"{IPPath}\\{relativePath}";
            if (File.Exists($"{SDORPath}\\{relativePath}")) return $"{SDORPath}\\{relativePath}";
            if (File.Exists($"{GamePath}\\{relativePath}")) return $"{GamePath}\\{relativePath}";

            return relativePath;
        }

        static void LoadActorDb()
        {
            Console.WriteLine("~ Called GlobalSettings.LoadActorDb() ~");
            // Find the Mush pack and load it
            string path = GetContentPath($"RSDB/ActorInfo.Product.{S3GameVersion1}{S3GameVersion2}{S3GameVersion3}.rstbl.byml.zs");
            /*if (!File.Exists(path))
                return;*/

            if (!File.Exists(path))
            {
                Console.WriteLine($"File \"{path}\" could not be found!");
                return;
            }

            var actorDb = new ActorDefinitionDb(path);
            foreach (var actor in actorDb.Definitions)
            {
                //Console.WriteLine($"~ Adding Actor: {actor.Name}");
                ActorDatabase.Add(actor.Name, actor);
            }
        }



        public class PathSettings
        {
            public PathColor RailColor0 = new PathColor(new Vector3(170, 0, 160), new Vector3(255, 64, 255), new Vector3(255, 64, 255));
            public PathColor RailColor1 = new PathColor(new Vector3(170, 160, 0), new Vector3(255, 0, 0), new Vector3(255, 0, 0));
        }

        public class PathColor
        {
            public Vector3 PointColor = new Vector3(0, 0, 0);
            public Vector3 LineColor = new Vector3(0, 0, 0);
            public Vector3 ArrowColor = new Vector3(0, 0, 0);

            [JsonIgnore]
            public EventHandler OnColorChanged;

            public PathColor(Vector3 point, Vector3 line, Vector3 arrow)
            {
                PointColor = new Vector3(point.X / 255f, point.Y / 255f, point.Z / 255f);
                LineColor = new Vector3(line.X / 255f, line.Y / 255f, line.Z / 255f);
                ArrowColor = new Vector3(arrow.X / 255f, arrow.Y / 255f, arrow.Z / 255f);
            }
        }
    }
}
