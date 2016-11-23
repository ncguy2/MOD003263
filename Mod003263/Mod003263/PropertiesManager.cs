

/**
 * Author: Nick Guy
 * Date: 07/11/2016
 * Contains: PropertiesManager
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mod003263 {
    /// <summary>
    /// Singleton to load and store the properties
    /// </summary>
    public class PropertiesManager {

        private static PropertiesManager instance;
        public static PropertiesManager GetInstance() {
            return instance ?? (instance = new PropertiesManager());
        }

        private readonly Dictionary<string, string> properties;

        private PropertiesManager() : this("properties.dat") {}

        public PropertiesManager(string propPath) {
            properties = new Dictionary<string, string>();
            LoadProperties(propPath);
        }

        /// <summary>
        /// Loads the properties from the specified file
        /// </summary>
        /// <param name="file">The file to check for properties</param>
        private void LoadProperties(string file) {
            if (!File.Exists(file)) return;
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines)
                ProcessPropertyLine(line);
        }

        /// <summary>
        /// Checks if the provided line is a valid property line, then adds it to the property cache
        /// </summary>
        /// <param name="line">The potential property line</param>
        private void ProcessPropertyLine(string line) {
            string[] keyVal = line.Split('=');
            if (keyVal.Length == 2)
                properties.Add(keyVal[0], keyVal[1]);
        }

        /// <summary>
        /// Gets the property with the specified key
        /// </summary>
        /// <param name="key">The key to find and return the associated value</param>
        /// <returns>The property if it was found, an empty string if it was not</returns>
        public string GetProperty(string key) {
            return GetPropertyOrDefault(key, "");
        }

        public string GetPropertyOrDefault(string key, string def) {
            return HasProperty(key) ? properties[key] : def;
        }

        /// <summary>
        /// Checks if the provided property key is cached within memory
        /// </summary>
        /// <param name="key">The property key to check</param>
        /// <returns>If the property cache contains the provided key</returns>
        public bool HasProperty(string key) {
            return properties.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys from the property cache
        /// </summary>
        /// <returns>A <see cref="List{String}"/> of all keys found in the property cache </returns>
        public List<string> GetKeys() {
            return properties.Keys.ToList();
        }

    }
}