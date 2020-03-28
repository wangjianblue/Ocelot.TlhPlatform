using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Cache
{
    public sealed class KeyEntity
    {
        private string name;
        /// <summary>
        /// Cache Name（Use for search cache key）
        /// /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value.Trim().ToLower(); }
        }
        private string key;
        /// <summary>
        /// Cache Key
        /// /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value.Trim().ToLower(); }
        }
        /// <summary>
        /// Valid Time (Unit:minute)
        /// /// </summary>
        public int ValidTime { get; set; } 
        /// <summary>
        /// Enaled /// </summary>
        public bool Enabled { get; set; }
    } 
}