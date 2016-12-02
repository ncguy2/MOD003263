using System;
using System.Collections.Generic;
using System.Linq;

/**
 *  Author: Nick Guy
 *  Date: 02/12/2016
 *  Contains: SmartSearch, SmartSearchEntity
 */
namespace Mod003263.utils {

    /// <summary>
    /// A standalone search helper, allowing for keys to be used to specify which method to use to search
    /// </summary>
    public class SmartSearch {

        /// <summary>
        /// Searches through the provided targets and uses the correct entity to return a list of conforming targets
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="targets">The collection of target objects</param>
        /// <param name="entities">The <see cref="SmartSearchEntity{T}"/>s used to define which method to use</param>
        /// <typeparam name="T">The type of target to use</typeparam>
        /// <returns>A list of conforming targets</returns>
        public static List<T> Search<T>(String query, List<T> targets, params SmartSearchEntity<T>[] entities) {
            query = query.ToLower();
            SmartSearchEntity<T> entity = GetChecker(query, entities);
            if (entity == null) return targets;
            query = query.Replace(entity.Key, "");
            Func<string, T, bool> checker = entity.Method;
            if (checker == null) return targets;

            List<T> valids = targets.Where(target => checker(query, target)).ToList();
            return valids;
        }

        /// <summary>
        /// Gets the correct entity from the provided entities based upon what is searched
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="entities">The search entities</param>
        /// <typeparam name="T">The type of target to use</typeparam>
        /// <returns>The entity that matches the provided query</returns>
        private static SmartSearchEntity<T> GetChecker<T>(String q, params SmartSearchEntity<T>[] entities) {
            SmartSearchEntity<T> def = null;
            foreach (SmartSearchEntity<T> entity in entities) {
                if (q.StartsWith(entity.Key.ToLower()))
                    return entity;
                if (def == null && entity.CanDefault)
                    def = entity;
            }
            return def;
        }

    }

    /// <summary>
    /// An entity to be used with <see cref="SmartSearch"/> to define the search methods
    /// </summary>
    /// <typeparam name="T">The object type to use</typeparam>
    public class SmartSearchEntity<T> {
        public SmartSearchEntity(string key, Func<string, T, bool> method) : this(key, method, false) {}

        public SmartSearchEntity(string key, Func<string, T, bool> method, bool canDefault) {
            Key = key;
            Method = method;
            CanDefault = canDefault;
        }

        public Func<string, T, bool> Method { get; }
        public string Key { get; }
        public bool CanDefault { get; }

    }

}