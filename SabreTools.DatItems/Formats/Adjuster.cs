﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SabreTools.Core;

namespace SabreTools.DatItems.Formats
{
    /// <summary>
    /// Represents which Adjuster(s) is associated with a set
    /// </summary>
    [JsonObject("adjuster"), XmlRoot("adjuster")]
    public class Adjuster : DatItem
    {
        #region Fields

        /// <summary>
        /// Name of the item
        /// </summary>
        [JsonProperty("name"), XmlElement("name")]
        public string? Name
        {
            get => _adjuster.ReadString(Models.Internal.Adjuster.NameKey);
            set => _adjuster[Models.Internal.Adjuster.NameKey] = value;
        }

        /// <summary>
        /// Determine whether the value is default
        /// </summary>
        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore), XmlElement("default")]
        public bool? Default
        {
            get => _adjuster.ReadBool(Models.Internal.Adjuster.DefaultKey);
            set => _adjuster[Models.Internal.Adjuster.DefaultKey] = value;
        }

        [JsonIgnore]
        public bool DefaultSpecified { get { return Default != null; } }

        /// <summary>
        /// Conditions associated with the adjustment
        /// </summary>
        [JsonProperty("conditions", DefaultValueHandling = DefaultValueHandling.Ignore), XmlElement("conditions")]
        public List<Condition>? Conditions
        {
            get => _adjuster.Read<Condition[]>(Models.Internal.Adjuster.ConditionKey)?.ToList();
            set => _adjuster[Models.Internal.Adjuster.ConditionKey] = value?.ToArray();
        }

        [JsonIgnore]
        public bool ConditionsSpecified { get { return Conditions != null && Conditions.Count > 0; } }

        /// <summary>
        /// Internal Adjuster model
        /// </summary>
        [JsonIgnore]
        private Models.Internal.Adjuster _adjuster = new();

        #endregion

        #region Accessors

        /// <inheritdoc/>
        public override string? GetName() => Name;

        /// <inheritdoc/>
        public override void SetName(string? name) => Name = name;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a default, empty Adjuster object
        /// </summary>
        public Adjuster()
        {
            Name = string.Empty;
            ItemType = ItemType.Adjuster;
        }

        #endregion

        #region Cloning Methods

        /// <inheritdoc/>
        public override object Clone()
        {
            return new Adjuster()
            {
                ItemType = this.ItemType,
                DupeType = this.DupeType,

                Machine = this.Machine?.Clone() as Machine,
                Source = this.Source?.Clone() as Source,
                Remove = this.Remove,

                _adjuster = this._adjuster?.Clone() as Models.Internal.Adjuster ?? new Models.Internal.Adjuster(),
            };
        }

        #endregion

        #region Comparision Methods

        /// <inheritdoc/>
        public override bool Equals(DatItem? other)
        {
            // If we don't have a Adjuster, return false
            if (ItemType != other?.ItemType || other is not Adjuster otherInternal)
                return false;

            // Compare the internal models
            return _adjuster.EqualTo(otherInternal._adjuster);
        }

        #endregion
    }
}
