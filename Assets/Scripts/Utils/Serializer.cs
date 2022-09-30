using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace ScavengerWorld
{
    public static class Serializer
    {
        public static T Load<T>(string filename) where T : class
        {
            if (File.Exists(filename))
            {
                try
                {
                    using (Stream stream = File.OpenRead(filename))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return formatter.Deserialize(stream) as T;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            return default(T);
        }

        public static List<T> LoadAll<T>(string directory) where T : class
        {
            List<T> items = new();
            string[] files = Directory.GetFiles(directory, "*_UtilityAction.dat");
            foreach (string f in files)
            {
                items.Add(Load<T>(f));
            }
            return items;
        }

        public static void Save<T>(string filename, T data) where T : class
        {
            using (Stream stream = File.OpenWrite(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }
    }
}