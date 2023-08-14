﻿using System.Xml.Serialization;
using Newtonsoft.Json;

/// <summary>
/// This holds all of the auxiliary types needed for proper parsing
/// </summary>
namespace SabreTools.DatItems.Formats
{
    #region DatItem

    #region OpenMSX

    /// <summary>
    /// Represents the OpenMSX original value
    /// </summary>
    [JsonObject("original"), XmlRoot("original")]
    public class Original
    {
        [JsonProperty("value"), XmlElement("value")]
        public bool? Value
        {
            get => _original.ReadBool(Models.Internal.Original.ValueKey);
            set => _original[Models.Internal.Original.ValueKey] = value;
        }

        [JsonProperty("content"), XmlElement("content")]
        public string? Content
        {
            get => _original.ReadString(Models.Internal.Original.ContentKey);
            set => _original[Models.Internal.Original.ContentKey] = value;
        }

        /// <summary>
        /// Internal Original model
        /// </summary>
        [JsonIgnore]
        private readonly Models.Internal.Original _original = new();
    }

    #endregion

    #endregion //DatItem
}
