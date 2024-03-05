﻿using System.Xml.Serialization;
using Newtonsoft.Json;
using SabreTools.Core;
using SabreTools.Filter;

namespace SabreTools.DatItems.Formats
{
    /// <summary>
    /// Represents release information about a set
    /// </summary>
    [JsonObject("release"), XmlRoot("release")]
    public class Release : DatItem
    {
        #region Fields

        /// <summary>
        /// Name of the item
        /// </summary>
        [JsonProperty("name"), XmlElement("name")]
        public string? Name
        {
            get => _internal.ReadString(Models.Metadata.Release.NameKey);
            set => _internal[Models.Metadata.Release.NameKey] = value;
        }

        /// <summary>
        /// Release region(s)
        /// </summary>
        [JsonProperty("region", DefaultValueHandling = DefaultValueHandling.Ignore), XmlElement("region")]
        public string? Region
        {
            get => _internal.ReadString(Models.Metadata.Release.RegionKey);
            set => _internal[Models.Metadata.Release.RegionKey] = value;
        }

        /// <summary>
        /// Release language(s)
        /// </summary>
        [JsonProperty("language", DefaultValueHandling = DefaultValueHandling.Ignore), XmlElement("language")]
        public string? Language
        {
            get => _internal.ReadString(Models.Metadata.Release.LanguageKey);
            set => _internal[Models.Metadata.Release.LanguageKey] = value;
        }

        /// <summary>
        /// Date of release
        /// </summary>
        [JsonProperty("date", DefaultValueHandling = DefaultValueHandling.Ignore), XmlElement("date")]
        public string? Date
        {
            get => _internal.ReadString(Models.Metadata.Release.DateKey);
            set => _internal[Models.Metadata.Release.DateKey] = value;
        }

        /// <summary>
        /// Default release, if applicable
        /// </summary>
        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore), XmlElement("default")]
        public bool? Default
        {
            get => _internal.ReadBool(Models.Metadata.Release.DefaultKey);
            set => _internal[Models.Metadata.Release.DefaultKey] = value;
        }

        [JsonIgnore]
        public bool DefaultSpecified { get { return Default != null; } }

        #endregion

        #region Accessors

        /// <inheritdoc/>
        public override string? GetName() => Name;

        /// <inheritdoc/>
        public override void SetName(string? name) => Name = name;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a default, empty Release object
        /// </summary>
        public Release()
        {
            _internal = new Models.Metadata.Release();
            Machine = new Machine();

            Name = string.Empty;
            ItemType = ItemType.Release;
            Region = string.Empty;
            Language = string.Empty;
            Date = string.Empty;
            Default = null;
        }

        #endregion

        #region Cloning Methods

        /// <inheritdoc/>
        public override object Clone()
        {
            return new Release()
            {
                ItemType = this.ItemType,
                DupeType = this.DupeType,

                Machine = this.Machine.Clone() as Machine ?? new Machine(),
                Source = this.Source?.Clone() as Source,
                Remove = this.Remove,

                _internal = this._internal?.Clone() as Models.Metadata.Release ?? [],
            };
        }

        #endregion
    
        #region Manipulation

        /// <inheritdoc/>
        public override bool RemoveField(DatItemField datItemField)
        {
            // Get the correct internal field name
            string? fieldName = datItemField switch
            {
                DatItemField.Date => Models.Metadata.Release.DateKey,
                DatItemField.Default => Models.Metadata.Release.DefaultKey,
                DatItemField.Language => Models.Metadata.Release.LanguageKey,
                DatItemField.Region => Models.Metadata.Release.RegionKey,
                _ => null,
            };

            // Remove the field and return
            return FieldManipulator.RemoveField(_internal, fieldName);
        }

        #endregion
    }
}
