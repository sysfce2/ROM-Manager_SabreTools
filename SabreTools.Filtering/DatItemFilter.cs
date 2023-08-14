﻿using System.Collections.Generic;
using SabreTools.Core;
using SabreTools.Core.Tools;
using SabreTools.DatItems;
using SabreTools.DatItems.Formats;
using SabreTools.Logging;

namespace SabreTools.Filtering
{
    /// <summary>
    /// Represents the filtering operations that need to be performed on a set of items, usually a DAT
    /// </summary>
    public class DatItemFilter : Filter
    {
        #region Fields

        #region Filters

        public FilterItem<string> AltName { get; private set; } = new FilterItem<string>();
        public FilterItem<string> AltTitle { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Analog_Mask { get; private set; } = new FilterItem<string>();
        public FilterItem<string> ArchiveDotOrgFormat { get; private set; } = new FilterItem<string>();
        public FilterItem<string> ArchiveDotOrgSource { get; private set; } = new FilterItem<string>();
        public FilterItem<Endianness> AreaEndianness { get; private set; } = new FilterItem<Endianness>() { Positive = Endianness.NULL, Negative = Endianness.NULL };
        public FilterItem<string> AreaName { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> AreaSize { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> AreaWidth { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<string> Bios { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Boot { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Categories { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> Channels { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<ChipType> ChipType { get; private set; } = new FilterItem<ChipType>() { Positive = Core.ChipType.NULL, Negative = Core.ChipType.NULL };
        public FilterItem<long?> Clock { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<string> Clone { get; private set; } = new FilterItem<string>();
        public FilterItem<SupportStatus> CocktailStatus { get; private set; } = new FilterItem<SupportStatus>() { Positive = Core.SupportStatus.NULL, Negative = Core.SupportStatus.NULL };
        public FilterItem<long?> Coins { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<string> Complete { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Condition_Mask { get; private set; } = new FilterItem<string>();
        public FilterItem<Relation> Condition_Relation { get; private set; } = new FilterItem<Relation>() { Positive = Core.Relation.NULL, Negative = Core.Relation.NULL };
        public FilterItem<string> Condition_Tag { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Condition_Value { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Content { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> Control_Buttons { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Control_KeyDelta { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Control_Maximum { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Control_Minimum { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Control_Player { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Control_ReqButtons { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<bool?> Control_Reverse { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<long?> Control_Sensitivity { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<ControlType> Control_Type { get; private set; } = new FilterItem<ControlType>() { Positive = ControlType.NULL, Negative = ControlType.NULL };
        public FilterItem<string> Control_Ways { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Control_Ways2 { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Control_Ways3 { get; private set; } = new FilterItem<string>();
        public FilterItem<string> CRC { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Date { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> Default { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Description { get; private set; } = new FilterItem<string>();
        public FilterItem<DeviceType> DeviceType { get; private set; } = new FilterItem<DeviceType>() { Positive = Core.DeviceType.NULL, Negative = Core.DeviceType.NULL };
        public FilterItem<string> DevStatus { get; private set; } = new FilterItem<string>();
        public FilterItem<DisplayType> DisplayType { get; private set; } = new FilterItem<DisplayType>() { Positive = Core.DisplayType.NULL, Negative = Core.DisplayType.NULL };
        public FilterItem<SupportStatus> EmulationStatus { get; private set; } = new FilterItem<SupportStatus>() { Positive = Core.SupportStatus.NULL, Negative = Core.SupportStatus.NULL };
        public FilterItem<string> Extension_Name { get; private set; } = new FilterItem<string>();
        public FilterItem<FeatureStatus> FeatureOverall { get; private set; } = new FilterItem<FeatureStatus>() { Positive = Core.FeatureStatus.NULL, Negative = Core.FeatureStatus.NULL };
        public FilterItem<FeatureStatus> FeatureStatus { get; private set; } = new FilterItem<FeatureStatus>() { Positive = Core.FeatureStatus.NULL, Negative = Core.FeatureStatus.NULL };
        public FilterItem<FeatureType> FeatureType { get; private set; } = new FilterItem<FeatureType>() { Positive = Core.FeatureType.NULL, Negative = Core.FeatureType.NULL };
        public FilterItem<string> Filter { get; private set; } = new FilterItem<string>();
        public FilterItem<string> FixedImage { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> FlipX { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<long?> HBEnd { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> HBStart { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Height { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> HTotal { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<bool?> Incomplete { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Index { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Instance_BriefName { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Instance_Name { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Interface { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> Inverted { get; private set; } = new FilterItem<bool?>();
        public FilterItem<string> Language { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Languages { get; private set; } = new FilterItem<string>();
        public FilterItem<LoadFlag> LoadFlag { get; private set; } = new FilterItem<LoadFlag>() { Positive = Core.LoadFlag.NULL, Negative = Core.LoadFlag.NULL };
        public FilterItem<bool?> Location_Inverted { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Location_Name { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> Location_Number { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Mandatory { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<string> Mask { get; private set; } = new FilterItem<string>();
        public FilterItem<string> MD5 { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Merge { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> MIA { get; private set; } = new FilterItem<bool?>();
        public FilterItem<string> Name { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> NoSoundHardware { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Number { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Offset { get; private set; } = new FilterItem<string>();
        public FilterItem<OpenMSXSubType> OpenMSXSubType { get; private set; } = new FilterItem<OpenMSXSubType>() { Positive = Core.OpenMSXSubType.NULL, Negative = Core.OpenMSXSubType.NULL };
        public FilterItem<string> OpenMSXType { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> Optional { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Original { get; private set; } = new FilterItem<string>();
        public FilterItem<string> OriginalFilename { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Part_Feature_Name { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Part_Feature_Value { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Part_Interface { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Part_Name { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> PixClock { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<string> Physical { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> Players { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<double?> Refresh { get; private set; } = new FilterItem<double?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<string> Region { get; private set; } = new FilterItem<string>();
        public FilterItem<string> RegParent { get; private set; } = new FilterItem<string>();
        public FilterItem<Relation> Relation { get; private set; } = new FilterItem<Relation>() { Positive = Core.Relation.NULL, Negative = Core.Relation.NULL };
        public FilterItem<string> Remark { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> RequiresArtwork { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<long?> Rotate { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<string> Rotation { get; private set; } = new FilterItem<string>();
        public FilterItem<Supported> SaveStateStatus { get; private set; } = new FilterItem<Supported>() { Positive = Supported.NULL, Negative = Supported.NULL };
        public FilterItem<bool?> Service { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<bool?> Setting_Default { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Setting_Name { get; private set; } = new FilterItem<string>();
        public FilterItem<string> Setting_Value { get; private set; } = new FilterItem<string>();
        public FilterItem<string> SHA1 { get; private set; } = new FilterItem<string>();
        public FilterItem<string> SHA256 { get; private set; } = new FilterItem<string>();
        public FilterItem<string> SHA384 { get; private set; } = new FilterItem<string>();
        public FilterItem<string> SHA512 { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> Size { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<bool?> SlotOption_Default { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> SlotOption_DeviceName { get; private set; } = new FilterItem<string>();
        public FilterItem<string> SlotOption_Name { get; private set; } = new FilterItem<string>();
        public FilterItem<SoftwareListStatus> SoftwareListStatus { get; private set; } = new FilterItem<SoftwareListStatus>() { Positive = Core.SoftwareListStatus.None, Negative = Core.SoftwareListStatus.None };
        public FilterItem<string> SpamSum { get; private set; } = new FilterItem<string>();
        public FilterItem<ItemStatus> Status { get; private set; } = new FilterItem<ItemStatus>() { Positive = ItemStatus.NULL, Negative = ItemStatus.NULL };
        public FilterItem<string> Summation { get; private set; } = new FilterItem<string>();
        public FilterItem<SupportStatus> SupportStatus { get; private set; } = new FilterItem<SupportStatus>() { Positive = Core.SupportStatus.NULL, Negative = Core.SupportStatus.NULL };
        public FilterItem<string> Tag { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> Tilt { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Type { get; private set; } = new FilterItem<string>();
        public FilterItem<bool?> Unofficial { get; private set; } = new FilterItem<bool?>() { Neutral = null };
        public FilterItem<string> Value { get; private set; } = new FilterItem<string>();
        public FilterItem<long?> VBEnd { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> VBStart { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> VTotal { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<long?> Width { get; private set; } = new FilterItem<long?>() { Positive = null, Negative = null, Neutral = null };
        public FilterItem<bool?> Writable { get; private set; } = new FilterItem<bool?>() { Neutral = null };

        #endregion

        /// <summary>
        /// Determines if any filters have been set
        /// </summary>
        public bool HasFilters { get; set; } = false;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DatItemFilter()
        {
            logger = new Logger(this);
        }

        #endregion

        #region Population

        /// <summary>
        /// Set multiple filters from key
        /// </summary>
        /// <param name="key">Key for the filter to be set</param>
        /// <param name="values">List of values for the filter</param>
        /// <param name="negate">True if negative filter, false otherwise</param>
        public void SetFilter(DatItemField key, List<string> values, bool negate)
        {
            foreach (string value in values)
            {
                SetFilter(key, value, negate);
            }
        }

        /// <summary>
        /// Set a single filter from key
        /// </summary>
        /// <param name="key">Key for the filter to be set</param>
        /// <param name="value">Value of the filter</param>
        /// <param name="negate">True if negative filter, false otherwise</param>
        public void SetFilter(DatItemField key, string value, bool negate)
        {
            switch (key)
            {
                case DatItemField.AltName:
                    SetStringFilter(AltName, value, negate);
                    break;

                case DatItemField.AltTitle:
                    SetStringFilter(AltTitle, value, negate);
                    break;

                case DatItemField.Analog_Mask:
                    SetStringFilter(Analog_Mask, value, negate);
                    break;

                case DatItemField.ArchiveDotOrgFormat:
                    SetStringFilter(ArchiveDotOrgFormat, value, negate);
                    break;

                case DatItemField.ArchiveDotOrgSource:
                    SetStringFilter(ArchiveDotOrgSource, value, negate);
                    break;

                case DatItemField.AreaEndianness:
                    if (negate)
                        AreaEndianness.Negative |= value.AsEndianness();
                    else
                        AreaEndianness.Positive |= value.AsEndianness();
                    break;

                case DatItemField.AreaName:
                    SetStringFilter(AreaName, value, negate);
                    break;

                case DatItemField.AreaSize:
                    SetLongFilter(AreaSize, value, negate);
                    break;

                case DatItemField.AreaWidth:
                    SetLongFilter(AreaWidth, value, negate);
                    break;

                case DatItemField.Bios:
                    SetStringFilter(Bios, value, negate);
                    break;

                case DatItemField.Boot:
                    SetStringFilter(Boot, value, negate);
                    break;

                case DatItemField.Categories:
                    SetStringFilter(Categories, value, negate);
                    break;

                case DatItemField.Channels:
                    SetLongFilter(Channels, value, negate);
                    break;

                case DatItemField.ChipType:
                    if (negate)
                        ChipType.Negative |= value.AsChipType();
                    else
                        ChipType.Positive |= value.AsChipType();
                    break;

                case DatItemField.Clock:
                    SetLongFilter(Clock, value, negate);
                    break;

                case DatItemField.Clone:
                    SetStringFilter(Clone, value, negate);
                    break;

                case DatItemField.CocktailStatus:
                    if (negate)
                        CocktailStatus.Negative |= value.AsSupportStatus();
                    else
                        CocktailStatus.Positive |= value.AsSupportStatus();
                    break;

                case DatItemField.Coins:
                    SetLongFilter(Coins, value, negate);
                    break;

                case DatItemField.Complete:
                    SetStringFilter(Complete, value, negate);
                    break;

                case DatItemField.Condition_Mask:
                    SetStringFilter(Condition_Mask, value, negate);
                    break;

                case DatItemField.Condition_Relation:
                    if (negate)
                        Condition_Relation.Negative |= value.AsRelation();
                    else
                        Condition_Relation.Positive |= value.AsRelation();
                    break;

                case DatItemField.Condition_Tag:
                    SetStringFilter(Condition_Tag, value, negate);
                    break;

                case DatItemField.Condition_Value:
                    SetStringFilter(Condition_Value, value, negate);
                    break;

                case DatItemField.Content:
                    SetStringFilter(Content, value, negate);
                    break;

                case DatItemField.Control_Buttons:
                    SetLongFilter(Control_Buttons, value, negate);
                    break;

                case DatItemField.Control_KeyDelta:
                    SetLongFilter(Control_KeyDelta, value, negate);
                    break;

                case DatItemField.Control_Player:
                    SetLongFilter(Control_Player, value, negate);
                    break;

                case DatItemField.Control_Maximum:
                    SetLongFilter(Control_Maximum, value, negate);
                    break;

                case DatItemField.Control_Minimum:
                    SetLongFilter(Control_Minimum, value, negate);
                    break;

                case DatItemField.Control_RequiredButtons:
                    SetLongFilter(Control_ReqButtons, value, negate);
                    break;

                case DatItemField.Control_Reverse:
                    SetBooleanFilter(Control_Reverse, value, negate);
                    break;

                case DatItemField.Control_Sensitivity:
                    SetLongFilter(Control_Sensitivity, value, negate);
                    break;

                case DatItemField.Control_Type:

                    if (negate)
                        Control_Type.Negative |= value.AsControlType();
                    else
                        Control_Type.Positive |= value.AsControlType();
                    break;

                case DatItemField.Control_Ways:
                    SetStringFilter(Control_Ways, value, negate);
                    break;

                case DatItemField.Control_Ways2:
                    SetStringFilter(Control_Ways2, value, negate);
                    break;

                case DatItemField.Control_Ways3:
                    SetStringFilter(Control_Ways3, value, negate);
                    break;

                case DatItemField.CRC:
                    SetStringFilter(CRC, value, negate);
                    break;

                case DatItemField.Date:
                    SetStringFilter(Date, value, negate);
                    break;

                case DatItemField.Default:
                    SetBooleanFilter(Default, value, negate);
                    break;

                case DatItemField.Description:
                    SetStringFilter(Description, value, negate);
                    break;

                case DatItemField.DeviceType:
                    if (negate)
                        DeviceType.Negative |= value.AsDeviceType();
                    else
                        DeviceType.Positive |= value.AsDeviceType();
                    break;

                case DatItemField.DevStatus:
                    SetStringFilter(DevStatus, value, negate);
                    break;

                case DatItemField.DisplayType:
                    if (negate)
                        DisplayType.Negative |= value.AsDisplayType();
                    else
                        DisplayType.Positive |= value.AsDisplayType();
                    break;

                case DatItemField.EmulationStatus:
                    if (negate)
                        EmulationStatus.Negative |= value.AsSupportStatus();
                    else
                        EmulationStatus.Positive |= value.AsSupportStatus();
                    break;

                case DatItemField.Extension_Name:
                    SetStringFilter(Extension_Name, value, negate);
                    break;

                case DatItemField.FeatureOverall:
                    if (negate)
                        FeatureOverall.Negative |= value.AsFeatureStatus();
                    else
                        FeatureOverall.Positive |= value.AsFeatureStatus();
                    break;

                case DatItemField.FeatureStatus:
                    if (negate)
                        FeatureStatus.Negative |= value.AsFeatureStatus();
                    else
                        FeatureStatus.Positive |= value.AsFeatureStatus();
                    break;

                case DatItemField.FeatureType:
                    if (negate)
                        FeatureType.Negative |= value.AsFeatureType();
                    else
                        FeatureType.Positive |= value.AsFeatureType();
                    break;

                case DatItemField.Filter:
                    SetStringFilter(Filter, value, negate);
                    break;

                case DatItemField.FixedImage:
                    SetStringFilter(FixedImage, value, negate);
                    break;

                case DatItemField.FlipX:
                    SetBooleanFilter(FlipX, value, negate);
                    break;

                case DatItemField.HBEnd:
                    SetLongFilter(HBEnd, value, negate);
                    break;

                case DatItemField.HBStart:
                    SetLongFilter(HBStart, value, negate);
                    break;

                case DatItemField.Height:
                    SetLongFilter(Height, value, negate);
                    break;

                case DatItemField.HTotal:
                    SetLongFilter(HTotal, value, negate);
                    break;

                case DatItemField.Incomplete:
                    SetBooleanFilter(Incomplete, value, negate);
                    break;

                case DatItemField.Index:
                    SetStringFilter(Index, value, negate);
                    break;

                case DatItemField.Instance_BriefName:
                    SetStringFilter(Instance_BriefName, value, negate);
                    break;

                case DatItemField.Instance_Name:
                    SetStringFilter(Instance_Name, value, negate);
                    break;

                case DatItemField.Interface:
                    SetStringFilter(Interface, value, negate);
                    break;

                case DatItemField.Inverted:
                    SetBooleanFilter(Inverted, value, negate);
                    break;

                case DatItemField.Language:
                    SetStringFilter(Language, value, negate);
                    break;

                case DatItemField.Languages:
                    SetStringFilter(Languages, value, negate);
                    break;

                case DatItemField.LoadFlag:
                    if (negate)
                        LoadFlag.Negative |= value.AsLoadFlag();
                    else
                        LoadFlag.Positive |= value.AsLoadFlag();
                    break;

                case DatItemField.Location_Inverted:
                    SetBooleanFilter(Location_Inverted, value, negate);
                    break;

                case DatItemField.Location_Name:
                    SetStringFilter(Location_Name, value, negate);
                    break;

                case DatItemField.Location_Number:
                    SetLongFilter(Location_Number, value, negate);
                    break;

                case DatItemField.Mandatory:
                    SetLongFilter(Mandatory, value, negate);
                    break;

                case DatItemField.Mask:
                    SetStringFilter(Mask, value, negate);
                    break;

                case DatItemField.MD5:
                    SetStringFilter(MD5, value, negate);
                    break;

                case DatItemField.Merge:
                    SetStringFilter(Merge, value, negate);
                    break;

                case DatItemField.MIA:
                    SetBooleanFilter(MIA, value, negate);
                    break;

                case DatItemField.Name:
                    SetStringFilter(Name, value, negate);
                    break;

                case DatItemField.NoSoundHardware:
                    SetBooleanFilter(NoSoundHardware, value, negate);
                    break;

                case DatItemField.Number:
                    SetStringFilter(Number, value, negate);
                    break;

                case DatItemField.Offset:
                    SetStringFilter(Offset, value, negate);
                    break;

                case DatItemField.OpenMSXSubType:
                    if (negate)
                        OpenMSXSubType.Negative |= value.AsOpenMSXSubType();
                    else
                        OpenMSXSubType.Positive |= value.AsOpenMSXSubType();
                    break;

                case DatItemField.OpenMSXType:
                    SetStringFilter(OpenMSXType, value, negate);
                    break;

                case DatItemField.Optional:
                    SetBooleanFilter(Optional, value, negate);
                    break;

                case DatItemField.Original:
                    SetStringFilter(Original, value, negate);
                    break;

                case DatItemField.OriginalFilename:
                    SetStringFilter(OriginalFilename, value, negate);
                    break;

                case DatItemField.Part_Feature_Name:
                    SetStringFilter(Part_Feature_Name, value, negate);
                    break;

                case DatItemField.Part_Feature_Value:
                    SetStringFilter(Part_Feature_Value, value, negate);
                    break;

                case DatItemField.Part_Interface:
                    SetStringFilter(Part_Interface, value, negate);
                    break;

                case DatItemField.Part_Name:
                    SetStringFilter(Part_Name, value, negate);
                    break;

                case DatItemField.Physical:
                    SetStringFilter(Physical, value, negate);
                    break;

                case DatItemField.PixClock:
                    SetLongFilter(PixClock, value, negate);
                    break;

                case DatItemField.Players:
                    SetLongFilter(Players, value, negate);
                    break;

                case DatItemField.Refresh:
                    SetDoubleFilter(Refresh, value, negate);
                    break;

                case DatItemField.Region:
                    SetStringFilter(Region, value, negate);
                    break;

                case DatItemField.RegParent:
                    SetStringFilter(RegParent, value, negate);
                    break;

                case DatItemField.Relation:
                    if (negate)
                        Relation.Negative |= value.AsRelation();
                    else
                        Relation.Positive |= value.AsRelation();
                    break;

                case DatItemField.Remark:
                    SetStringFilter(Remark, value, negate);
                    break;

                case DatItemField.RequiresArtwork:
                    SetBooleanFilter(RequiresArtwork, value, negate);
                    break;

                case DatItemField.Rotate:
                    SetLongFilter(Rotate, value, negate);
                    break;

                case DatItemField.Rotation:
                    SetStringFilter(Rotation, value, negate);
                    break;

                case DatItemField.SaveStateStatus:
                    if (negate)
                        SaveStateStatus.Negative |= value.AsSupported();
                    else
                        SaveStateStatus.Positive |= value.AsSupported();
                    break;

                case DatItemField.Service:
                    SetBooleanFilter(Service, value, negate);
                    break;

                case DatItemField.Setting_Default:
                    SetBooleanFilter(Setting_Default, value, negate);
                    break;

                case DatItemField.Setting_Name:
                    SetStringFilter(Setting_Name, value, negate);
                    break;

                case DatItemField.Setting_Value:
                    SetStringFilter(Setting_Value, value, negate);
                    break;

                case DatItemField.SHA1:
                    SetStringFilter(SHA1, value, negate);
                    break;

                case DatItemField.SHA256:
                    SetStringFilter(SHA256, value, negate);
                    break;

                case DatItemField.SHA384:
                    SetStringFilter(SHA384, value, negate);
                    break;

                case DatItemField.SHA512:
                    SetStringFilter(SHA512, value, negate);
                    break;

                case DatItemField.Size:
                    SetLongFilter(Size, value, negate);
                    break;

                case DatItemField.SlotOption_Default:
                    SetBooleanFilter(SlotOption_Default, value, negate);
                    break;

                case DatItemField.SlotOption_DeviceName:
                    SetStringFilter(SlotOption_DeviceName, value, negate);
                    break;

                case DatItemField.SlotOption_Name:
                    SetStringFilter(SlotOption_Name, value, negate);
                    break;

                case DatItemField.SoftwareListStatus:
                    if (negate)
                        SoftwareListStatus.Negative |= value.AsSoftwareListStatus();
                    else
                        SoftwareListStatus.Positive |= value.AsSoftwareListStatus();
                    break;

                case DatItemField.SpamSum:
                    SetStringFilter(SpamSum, value, negate);
                    break;

                case DatItemField.Status:
                    if (negate)
                        Status.Negative |= value.AsItemStatus();
                    else
                        Status.Positive |= value.AsItemStatus();
                    break;

                case DatItemField.Summation:
                    SetStringFilter(Summation, value, negate);
                    break;

                case DatItemField.SupportStatus:
                    if (negate)
                        SupportStatus.Negative |= value.AsSupportStatus();
                    else
                        SupportStatus.Positive |= value.AsSupportStatus();
                    break;

                case DatItemField.Tag:
                    SetStringFilter(Tag, value, negate);
                    break;

                case DatItemField.Tilt:
                    SetBooleanFilter(Tilt, value, negate);
                    break;

                case DatItemField.Type:
                    if (value.AsItemType() == ItemType.NULL)
                        return;

                    SetStringFilter(Type, value, negate);
                    break;

                case DatItemField.Unofficial:
                    SetBooleanFilter(Unofficial, value, negate);
                    break;

                case DatItemField.Value:
                    SetStringFilter(Value, value, negate);
                    break;

                case DatItemField.VBEnd:
                    SetLongFilter(VBEnd, value, negate);
                    break;

                case DatItemField.VBStart:
                    SetLongFilter(VBStart, value, negate);
                    break;

                case DatItemField.VTotal:
                    SetLongFilter(VTotal, value, negate);
                    break;

                case DatItemField.Width:
                    SetLongFilter(Width, value, negate);
                    break;

                case DatItemField.Writable:
                    SetBooleanFilter(Writable, value, negate);
                    break;
            }
        }

        #endregion

        #region Running

        /// <summary>
        /// Check to see if a DatItem passes the filters
        /// </summary>
        /// <param name="datItem">DatItem to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        public bool PassesFilters(DatItem datItem)
        {
            if (datItem == null)
                return false;

            #region Common

            // Filter on item type
            if (!PassStringFilter(Type, datItem.ItemType.ToString()))
                return false;

            // Name is common if the name is non-null
            if (datItem.GetName() != null)
            {
                // Filter on item name
                if (!PassStringFilter(Name, datItem.GetName()))
                    return false;
            }

            #endregion

            #region Item-Specific

            return datItem switch
            {
                Adjuster adjuster => PassesFilters(adjuster),
                Analog analog => PassesFilters(analog),
                Archive archive => PassesFilters(archive),
                BiosSet biosSet => PassesFilters(biosSet),
                Chip chip => PassesFilters(chip),
                Condition condition => PassesFilters(condition),
                Configuration configuration => PassesFilters(configuration),
                ConfLocation confLocation => PassesFilters(confLocation),
                ConfSetting confSetting => PassesFilters(confSetting),
                Control control => PassesFilters(control),
                DataArea dataArea => PassesFilters(dataArea),
                Device device => PassesFilters(device),
                DipLocation dipLocation => PassesFilters(dipLocation),
                DipSwitch dipSwitch => PassesFilters(dipSwitch),
                DipValue dipValue => PassesFilters(dipValue),
                Disk disk => PassesFilters(disk),
                DiskArea diskArea => PassesFilters(diskArea),
                Display display => PassesFilters(display),
                Driver driver => PassesFilters(driver),
                Extension extension => PassesFilters(extension),
                Feature feature => PassesFilters(feature),
                Info info => PassesFilters(info),
                Input input => PassesFilters(input),
                Instance instance => PassesFilters(instance),
                Media media => PassesFilters(media),
                Part part => PassesFilters(part),
                PartFeature partFeature => PassesFilters(partFeature),
                Port port => PassesFilters(port),
                RamOption ramOption => PassesFilters(ramOption),
                Release release => PassesFilters(release),
                Rom rom => PassesFilters(rom),
                SharedFeature sharedFeature => PassesFilters(sharedFeature),
                Slot slot => PassesFilters(slot),
                SlotOption slotOption => PassesFilters(slotOption),
                SoftwareList softwareList => PassesFilters(softwareList),
                Sound sound => PassesFilters(sound),
                _ => false,
            };

            #endregion
        }

        /// <summary>
        /// Check to see if an Adjuster passes the filters
        /// </summary>
        /// <param name="adjuster">Adjuster to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Adjuster adjuster)
        {
            // DatItem_Default
            if (!PassBoolFilter(Default, adjuster.Default))
                return false;

            // Filter on individual conditions
            if (adjuster.ConditionsSpecified)
            {
                foreach (Condition condition in adjuster.Conditions)
                {
                    if (!PassesFilters(condition, true))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if an Analog passes the filters
        /// </summary>
        /// <param name="analog">Analog to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Analog analog)
        {
            // DatItem_Analog_Mask
            if (!PassStringFilter(Analog_Mask, analog.Mask))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if an Archive passes the filters
        /// </summary>
        /// <param name="archive">Archive to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Archive archive)
        {
            // DatItem_Categories
            if (!PassStringFilter(Categories, archive.Categories))
                return false;

            // DatItem_Clone
            if (!PassStringFilter(Clone, archive.CloneValue))
                return false;

            // DatItem_Complete
            if (!PassStringFilter(Complete, archive.Complete))
                return false;

            // DatItem_DevStatus
            if (!PassStringFilter(DevStatus, archive.DevStatus))
                return false;

            // DatItem_Languages
            if (!PassStringFilter(Languages, archive.Languages))
                return false;

            // DatItem_Number
            if (!PassStringFilter(Number, archive.Number))
                return false;

            // DatItem_Physical
            if (!PassStringFilter(Physical, archive.Physical))
                return false;

            // DatItem_Region
            if (!PassStringFilter(Region, archive.Region))
                return false;

            // DatItem_RegParent
            if (!PassStringFilter(RegParent, archive.RegParent))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a BiosSet passes the filters
        /// </summary>
        /// <param name="biosSet">BiosSet to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(BiosSet biosSet)
        {
            // DatItem_Default
            if (!PassBoolFilter(Default, biosSet.Default))
                return false;

            // DatItem_Description
            if (!PassStringFilter(Description, biosSet.Description))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Chip passes the filters
        /// </summary>
        /// <param name="chip">Chip to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Chip chip)
        {
            // DatItem_ChipType
            if (ChipType.MatchesPositive(Core.ChipType.NULL, chip.ChipType) == false)
                return false;
            if (ChipType.MatchesNegative(Core.ChipType.NULL, chip.ChipType) == true)
                return false;

            // DatItem_Clock
            if (!PassLongFilter(Clock, chip.Clock))
                return false;

            // DatItem_Tag
            if (!PassStringFilter(Tag, chip.Tag))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Condition passes the filters
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="sub">True if this is a subitem, false otherwise</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Condition condition, bool sub = false)
        {
            if (sub)
            {
                // DatItem_Condition_Mask
                if (!PassStringFilter(Condition_Mask, condition.Mask))
                    return false;

                // DatItem_Condition_Relation
                if (Condition_Relation.MatchesPositive(Core.Relation.NULL, condition.Relation) == false)
                    return false;
                if (Condition_Relation.MatchesNegative(Core.Relation.NULL, condition.Relation) == true)
                    return false;

                // DatItem_Condition_Tag
                if (!PassStringFilter(Condition_Tag, condition.Tag))
                    return false;

                // DatItem_Condition_Value
                if (!PassStringFilter(Condition_Value, condition.Value))
                    return false;
            }
            else
            {
                // DatItem_Mask
                if (!PassStringFilter(Mask, condition.Mask))
                    return false;

                // DatItem_Relation
                if (Relation.MatchesPositive(Core.Relation.NULL, condition.Relation) == false)
                    return false;
                if (Relation.MatchesNegative(Core.Relation.NULL, condition.Relation) == true)
                    return false;

                // DatItem_Tag
                if (!PassStringFilter(Tag, condition.Tag))
                    return false;

                // DatItem_Value
                if (!PassStringFilter(Value, condition.Value))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if a Configuration passes the filters
        /// </summary>
        /// <param name="configuration">Configuration to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Configuration configuration)
        {
            // DatItem_Mask
            if (!PassStringFilter(Mask, configuration.Mask))
                return false;

            // DatItem_Tag
            if (!PassStringFilter(Tag, configuration.Tag))
                return false;

            // Filter on individual conditions
            if (configuration.ConditionsSpecified)
            {
                foreach (Condition subCondition in configuration.Conditions)
                {
                    if (!PassesFilters(subCondition, true))
                        return false;
                }
            }

            // Filter on individual locations
            if (configuration.LocationsSpecified)
            {
                foreach (ConfLocation subLocation in configuration.Locations)
                {
                    if (!PassesFilters(subLocation))
                        return false;
                }
            }

            // Filter on individual settings
            if (configuration.SettingsSpecified)
            {
                foreach (ConfSetting subSetting in configuration.Settings)
                {
                    if (!PassesFilters(subSetting))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a ConfLocation passes the filters
        /// </summary>
        /// <param name="confLocation">ConfLocation to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(ConfLocation confLocation)
        {
            // DatItem_Location_Inverted
            if (!PassBoolFilter(Location_Inverted, confLocation.Inverted))
                return false;

            // DatItem_Location_Name
            if (!PassStringFilter(Location_Name, confLocation.Name))
                return false;

            // DatItem_Location_Number
            if (!PassLongFilter(Location_Number, confLocation.Number))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a ConfSetting passes the filters
        /// </summary>
        /// <param name="confSetting">ConfSetting to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(ConfSetting confSetting)
        {
            // DatItem_Setting_Default
            if (!PassBoolFilter(Setting_Default, confSetting.Default))
                return false;

            // DatItem_Setting_Name
            if (!PassStringFilter(Setting_Name, confSetting.Name))
                return false;

            // DatItem_Setting_Value
            if (!PassStringFilter(Setting_Value, confSetting.Value))
                return false;

            // Filter on individual conditions
            if (confSetting.ConditionsSpecified)
            {
                foreach (Condition subCondition in confSetting.Conditions)
                {
                    if (!PassesFilters(subCondition, true))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a Control passes the filters
        /// </summary>
        /// <param name="control">Control to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Control control)
        {
            // DatItem_Control_Buttons
            if (!PassLongFilter(Control_Buttons, control.Buttons))
                return false;

            // DatItem_Control_KeyDelta
            if (!PassLongFilter(Control_KeyDelta, control.KeyDelta))
                return false;

            // DatItem_Control_Maximum
            if (!PassLongFilter(Control_Maximum, control.Maximum))
                return false;

            // DatItem_Control_Minimum
            if (!PassLongFilter(Control_Minimum, control.Minimum))
                return false;

            // DatItem_Control_Player
            if (!PassLongFilter(Control_Player, control.Player))
                return false;

            // DatItem_Control_ReqButtons
            if (!PassLongFilter(Control_ReqButtons, control.RequiredButtons))
                return false;

            // DatItem_Control_Reverse
            if (!PassBoolFilter(Control_Reverse, control.Reverse))
                return false;

            // DatItem_Control_Sensitivity
            if (!PassLongFilter(Control_Sensitivity, control.Sensitivity))
                return false;

            // DatItem_Control_Type
            if (Control_Type.MatchesPositive(ControlType.NULL, control.ControlType) == false)
                return false;
            if (Control_Type.MatchesNegative(ControlType.NULL, control.ControlType) == true)
                return false;

            // DatItem_Control_Ways
            if (!PassStringFilter(Control_Ways, control.Ways))
                return false;

            // DatItem_Control_Ways2
            if (!PassStringFilter(Control_Ways2, control.Ways2))
                return false;

            // DatItem_Control_Ways3
            if (!PassStringFilter(Control_Ways3, control.Ways3))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a DataArea passes the filters
        /// </summary>
        /// <param name="dataArea">DataArea to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(DataArea dataArea)
        {
            // DatItem_AreaEndianness
            if (AreaEndianness.MatchesPositive(Endianness.NULL, dataArea.Endianness) == false)
                return false;
            if (AreaEndianness.MatchesNegative(Endianness.NULL, dataArea.Endianness) == true)
                return false;

            // DatItem_AreaName
            if (!PassStringFilter(AreaName, dataArea.Name))
                return false;

            // DatItem_AreaSize
            if (!PassLongFilter(AreaSize, dataArea.Size))
                return false;

            // DatItem_AreaWidth
            if (!PassLongFilter(AreaWidth, dataArea.Width))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Device passes the filters
        /// </summary>
        /// <param name="device">Device to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Device device)
        {
            // DatItem_DeviceType
            if (DeviceType.MatchesPositive(Core.DeviceType.NULL, device.DeviceType) == false)
                return false;
            if (DeviceType.MatchesNegative(Core.DeviceType.NULL, device.DeviceType) == true)
                return false;

            // DatItem_FixedImage
            if (!PassStringFilter(FixedImage, device.FixedImage))
                return false;

            // DatItem_Interface
            if (!PassStringFilter(Interface, device.Interface))
                return false;

            // DatItem_Mandatory
            if (!PassLongFilter(Mandatory, device.Mandatory))
                return false;

            // DatItem_Tag
            if (!PassStringFilter(Tag, device.Tag))
                return false;

            // Filter on individual extensions
            if (device.ExtensionsSpecified)
            {
                foreach (Extension subExtension in device.Extensions)
                {
                    if (!PassesFilters(subExtension))
                        return false;
                }
            }

            // Filter on individual instances
            if (device.InstancesSpecified)
            {
                foreach (Instance subInstance in device.Instances)
                {
                    if (!PassesFilters(subInstance))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a DipLocation passes the filters
        /// </summary>
        /// <param name="dipLocation">DipLocation to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(DipLocation dipLocation)
        {
            // DatItem_Location_Inverted
            if (!PassBoolFilter(Location_Inverted, dipLocation.Inverted))
                return false;

            // DatItem_Location_Name
            if (!PassStringFilter(Location_Name, dipLocation.Name))
                return false;

            // DatItem_Location_Number
            if (!PassLongFilter(Location_Number, dipLocation.Number))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a DipSwitch passes the filters
        /// </summary>
        /// <param name="dipSwitch">DipSwitch to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(DipSwitch dipSwitch)
        {
            // DatItem_Mask
            if (!PassStringFilter(Mask, dipSwitch.Mask))
                return false;

            // DatItem_Tag
            if (!PassStringFilter(Tag, dipSwitch.Tag))
                return false;

            // Filter on individual conditions
            if (dipSwitch.ConditionsSpecified)
            {
                foreach (Condition subCondition in dipSwitch.Conditions)
                {
                    if (!PassesFilters(subCondition, true))
                        return false;
                }
            }

            // Filter on individual locations
            if (dipSwitch.LocationsSpecified)
            {
                foreach (DipLocation subLocation in dipSwitch.Locations)
                {
                    if (!PassesFilters(subLocation))
                        return false;
                }
            }

            // Filter on individual values
            if (dipSwitch.ValuesSpecified)
            {
                foreach (DipValue subValue in dipSwitch.Values)
                {
                    if (!PassesFilters(subValue))
                        return false;
                }
            }

            // Filter on Part
            if (dipSwitch.PartSpecified)
            {
                if (!PassesFilters(dipSwitch.Part))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if a DipValue passes the filters
        /// </summary>
        /// <param name="dipValue">DipValue to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(DipValue dipValue)
        {
            // DatItem_Setting_Default
            if (!PassBoolFilter(Setting_Default, dipValue.Default))
                return false;

            // DatItem_Setting_Name
            if (!PassStringFilter(Setting_Name, dipValue.Name))
                return false;

            // DatItem_Setting_Value
            if (!PassStringFilter(Setting_Value, dipValue.Value))
                return false;

            // Filter on individual conditions
            if (dipValue.ConditionsSpecified)
            {
                foreach (Condition subCondition in dipValue.Conditions)
                {
                    if (!PassesFilters(subCondition, true))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a Disk passes the filters
        /// </summary>
        /// <param name="disk">Disk to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Disk disk)
        {
            // DatItem_Index
            if (!PassStringFilter(Index, disk.Index))
                return false;

            // DatItem_MD5
            if (!PassStringFilter(MD5, disk.MD5))
                return false;

            // DatItem_Merge
            if (!PassStringFilter(Merge, disk.MergeTag))
                return false;

            // DatItem_Optional
            if (!PassBoolFilter(Optional, disk.Optional))
                return false;

            // DatItem_Region
            if (!PassStringFilter(Region, disk.Region))
                return false;

            // DatItem_SHA1
            if (!PassStringFilter(SHA1, disk.SHA1))
                return false;

            // DatItem_Status
            if (Status.MatchesPositive(ItemStatus.NULL, disk.ItemStatus) == false)
                return false;
            if (Status.MatchesNegative(ItemStatus.NULL, disk.ItemStatus) == true)
                return false;

            // DatItem_Writable
            if (!PassBoolFilter(Writable, disk.Writable))
                return false;

            // Filter on DiskArea
            if (disk.DiskAreaSpecified)
            {
                if (!PassesFilters(disk.DiskArea))
                    return false;
            }

            // Filter on Part
            if (disk.PartSpecified)
            {
                if (!PassesFilters(disk.Part))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if a DiskArea passes the filters
        /// </summary>
        /// <param name="diskArea">DiskArea to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(DiskArea diskArea)
        {
            // DatItem_AreaName
            if (!PassStringFilter(AreaName, diskArea.Name))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Display passes the filters
        /// </summary>
        /// <param name="display">Display to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Display display)
        {
            // DatItem_DisplayType
            if (DisplayType.MatchesPositive(Core.DisplayType.NULL, display.DisplayType) == false)
                return false;
            if (DisplayType.MatchesNegative(Core.DisplayType.NULL, display.DisplayType) == true)
                return false;

            // DatItem_FlipX
            if (!PassBoolFilter(FlipX, display.FlipX))
                return false;

            // DatItem_HBEnd
            if (!PassLongFilter(HBEnd, display.HBEnd))
                return false;

            // DatItem_HBStart
            if (!PassLongFilter(HBStart, display.HBStart))
                return false;

            // DatItem_Height
            if (!PassLongFilter(Height, display.Height))
                return false;

            // DatItem_HTotal
            if (!PassLongFilter(HTotal, display.HTotal))
                return false;

            // DatItem_PixClock
            if (!PassLongFilter(PixClock, display.PixClock))
                return false;

            // DatItem_Refresh
            if (!PassDoubleFilter(Refresh, display.Refresh))
                return false;

            // DatItem_Rotate
            if (!PassLongFilter(Rotate, display.Rotate))
                return false;

            // DatItem_Tag
            if (!PassStringFilter(Tag, display.Tag))
                return false;

            // DatItem_VBEnd
            if (!PassLongFilter(VBEnd, display.VBEnd))
                return false;

            // DatItem_VBStart
            if (!PassLongFilter(VBStart, display.VBStart))
                return false;

            // DatItem_VTotal
            if (!PassLongFilter(VTotal, display.VTotal))
                return false;

            // DatItem_Width
            if (!PassLongFilter(Width, display.Width))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Driver passes the filters
        /// </summary>
        /// <param name="driver">Driver to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Driver driver)
        {
            // DatItem_CocktailStatus
            if (CocktailStatus.MatchesPositive(Core.SupportStatus.NULL, driver.Cocktail) == false)
                return false;
            if (CocktailStatus.MatchesNegative(Core.SupportStatus.NULL, driver.Cocktail) == true)
                return false;

            // DatItem_EmulationStatus
            if (EmulationStatus.MatchesPositive(Core.SupportStatus.NULL, driver.Emulation) == false)
                return false;
            if (EmulationStatus.MatchesNegative(Core.SupportStatus.NULL, driver.Emulation) == true)
                return false;

            // DatItem_Incomplete
            if (!PassBoolFilter(Incomplete, driver.Incomplete))
                return false;

            // DatItem_NoSoundHardware
            if (!PassBoolFilter(NoSoundHardware, driver.NoSoundHardware))
                return false;

            // DatItem_RequiresArtwork
            if (!PassBoolFilter(RequiresArtwork, driver.RequiresArtwork))
                return false;

            // DatItem_SaveStateStatus
            if (SaveStateStatus.MatchesPositive(Supported.NULL, driver.SaveState) == false)
                return false;
            if (SaveStateStatus.MatchesNegative(Supported.NULL, driver.SaveState) == true)
                return false;

            // DatItem_SupportStatus
            if (SupportStatus.MatchesPositive(Core.SupportStatus.NULL, driver.Status) == false)
                return false;
            if (SupportStatus.MatchesNegative(Core.SupportStatus.NULL, driver.Status) == true)
                return false;

            // DatItem_Unofficial
            if (!PassBoolFilter(Unofficial, driver.Unofficial))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Extension passes the filters
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Extension extension)
        {
            // DatItem_Extension_Name
            if (!PassStringFilter(Extension_Name, extension.Name))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Feature passes the filters
        /// </summary>
        /// <param name="feature">Feature to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Feature feature)
        {
            // DatItem_FeatureOverall
            if (FeatureOverall.MatchesPositive(Core.FeatureStatus.NULL, feature.Overall) == false)
                return false;
            if (FeatureOverall.MatchesNegative(Core.FeatureStatus.NULL, feature.Overall) == true)
                return false;

            // DatItem_FeatureStatus
            if (FeatureStatus.MatchesPositive(Core.FeatureStatus.NULL, feature.Status) == false)
                return false;
            if (FeatureStatus.MatchesNegative(Core.FeatureStatus.NULL, feature.Status) == true)
                return false;

            // DatItem_FeatureType
            if (FeatureType.MatchesPositive(Core.FeatureType.NULL, feature.Type) == false)
                return false;
            if (FeatureType.MatchesNegative(Core.FeatureType.NULL, feature.Type) == true)
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Info passes the filters
        /// </summary>
        /// <param name="info">Info to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Info info)
        {
            // DatItem_Value
            if (!PassStringFilter(Value, info.Value))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Input passes the filters
        /// </summary>
        /// <param name="input">Input to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Input input)
        {
            // DatItem_Coins
            if (!PassLongFilter(Coins, input.Coins))
                return false;

            // DatItem_Players
            if (!PassLongFilter(Players, input.Players))
                return false;

            // DatItem_Service
            if (!PassBoolFilter(Service, input.Service))
                return false;

            // DatItem_Tilt
            if (!PassBoolFilter(Tilt, input.Tilt))
                return false;

            // Filter on individual controls
            if (input.ControlsSpecified)
            {
                foreach (Control subControl in input.Controls)
                {
                    if (!PassesFilters(subControl))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a Instance passes the filters
        /// </summary>
        /// <param name="instance">Instance to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Instance instance)
        {
            // DatItem_Instance_BriefName
            if (!PassStringFilter(Instance_BriefName, instance.BriefName))
                return false;

            // DatItem_Instance_Name
            if (!PassStringFilter(Instance_Name, instance.Name))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Media passes the filters
        /// </summary>
        /// <param name="media">Media to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Media media)
        {
            // DatItem_MD5
            if (!PassStringFilter(MD5, media.MD5))
                return false;

            // DatItem_SHA1
            if (!PassStringFilter(SHA1, media.SHA1))
                return false;

            // DatItem_SHA256
            if (!PassStringFilter(SHA256, media.SHA256))
                return false;

            // DatItem_SpamSum
            if (!PassStringFilter(SpamSum, media.SpamSum))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Part passes the filters
        /// </summary>
        /// <param name="part">Part to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Part part)
        {
            // DatItem_Part_Interface
            if (!PassStringFilter(Part_Interface, part.Interface))
                return false;

            // DatItem_Part_Name
            if (!PassStringFilter(Part_Name, part.Name))
                return false;

            // Filter on features
            if (part.FeaturesSpecified)
            {
                foreach (PartFeature subPartFeature in part.Features)
                {
                    if (!PassesFilters(subPartFeature))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a PartFeature passes the filters
        /// </summary>
        /// <param name="partFeature">PartFeature to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(PartFeature partFeature)
        {
            // DatItem_Part_Feature_Name
            if (!PassStringFilter(Part_Feature_Name, partFeature.Name))
                return false;

            // DatItem_Part_Feature_Value
            if (!PassStringFilter(Part_Feature_Value, partFeature.Value))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Port passes the filters
        /// </summary>
        /// <param name="port">Port to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Port port)
        {
            // DatItem_Tag
            if (!PassStringFilter(Tag, port.Tag))
                return false;

            // Filter on individual analogs
            if (port.AnalogsSpecified)
            {
                foreach (Analog subAnalog in port.Analogs)
                {
                    if (!PassesFilters(subAnalog))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a RamOption passes the filters
        /// </summary>
        /// <param name="ramOption">RamOption to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(RamOption ramOption)
        {
            // DatItem_Content
            if (!PassStringFilter(Content, ramOption.Content))
                return false;

            // DatItem_Default
            if (!PassBoolFilter(Default, ramOption.Default))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Release passes the filters
        /// </summary>
        /// <param name="release">Release to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Release release)
        {
            // DatItem_Date
            if (!PassStringFilter(Date, release.Date))
                return false;

            // DatItem_Default
            if (!PassBoolFilter(Default, release.Default))
                return false;

            // DatItem_Language
            if (!PassStringFilter(Language, release.Language))
                return false;

            // DatItem_Region
            if (!PassStringFilter(Region, release.Region))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Rom passes the filters
        /// </summary>
        /// <param name="rom">Rom to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Rom rom)
        {
            // DatItem_AltName
            if (!PassStringFilter(AltName, rom.AltName))
                return false;

            // DatItem_AltTitle
            if (!PassStringFilter(AltTitle, rom.AltTitle))
                return false;

            // DatItem_ArchiveDotOrgFormat
            if (!PassStringFilter(ArchiveDotOrgFormat, rom.ArchiveDotOrgFormat))
                return false;

            // DatItem_ArchiveDotOrgSource
            if (!PassStringFilter(ArchiveDotOrgSource, rom.ArchiveDotOrgSource))
                return false;

            // DatItem_Bios
            if (!PassStringFilter(Bios, rom.Bios))
                return false;

            // DatItem_Boot
            if (!PassStringFilter(Boot, rom.Boot))
                return false;

            // DatItem_CRC
            if (!PassStringFilter(CRC, rom.CRC))
                return false;

            // DatItem_Date
            if (!PassStringFilter(Date, rom.Date))
                return false;

            // DatItem_Inverted
            if (!PassBoolFilter(Inverted, rom.Inverted))
                return false;

            // DatItem_LoadFlag
            if (LoadFlag.MatchesPositive(Core.LoadFlag.NULL, rom.LoadFlag) == false)
                return false;
            if (LoadFlag.MatchesNegative(Core.LoadFlag.NULL, rom.LoadFlag) == true)
                return false;

            // DatItem_MD5
            if (!PassStringFilter(MD5, rom.MD5))
                return false;

            // DatItem_Merge
            if (!PassStringFilter(Merge, rom.MergeTag))
                return false;

            // DatItem_MIA
            if (!PassBoolFilter(MIA, rom.MIA))
                return false;

            // DatItem_Offset
            if (!PassStringFilter(Offset, rom.Offset))
                return false;

            // DatItem_OpenMSXSubType
            if (OpenMSXSubType.MatchesPositive(Core.OpenMSXSubType.NULL, rom.OpenMSXSubType) == false)
                return false;
            if (OpenMSXSubType.MatchesNegative(Core.OpenMSXSubType.NULL, rom.OpenMSXSubType) == true)
                return false;

            // DatItem_OpenMSXType
            if (!PassStringFilter(OpenMSXType, rom.OpenMSXType))
                return false;

            // DatItem_Optional
            if (!PassBoolFilter(Optional, rom.Optional))
                return false;

            // DatItem_Original
            if (!PassStringFilter(Original, rom.Original?.Content))
                return false;

            // DatItem_OriginalFilename
            if (!PassStringFilter(OriginalFilename, rom.OriginalFilename))
                return false;

            // DatItem_Region
            if (!PassStringFilter(Region, rom.Region))
                return false;

            // DatItem_Remark
            if (!PassStringFilter(Remark, rom.Remark))
                return false;

            // DatItem_Rotation
            if (!PassStringFilter(Rotation, rom.Rotation))
                return false;

            // DatItem_SHA1
            if (!PassStringFilter(SHA1, rom.SHA1))
                return false;

            // DatItem_SHA256
            if (!PassStringFilter(SHA256, rom.SHA256))
                return false;

            // DatItem_SHA384
            if (!PassStringFilter(SHA384, rom.SHA384))
                return false;

            // DatItem_SHA512
            if (!PassStringFilter(SHA512, rom.SHA512))
                return false;

            // DatItem_Size
            if (!PassLongFilter(Size, rom.Size))
                return false;

            // DatItem_SpamSum
            if (!PassStringFilter(SpamSum, rom.SpamSum))
                return false;

            // DatItem_Status
            if (Status.MatchesPositive(ItemStatus.NULL, rom.ItemStatus) == false)
                return false;
            if (Status.MatchesNegative(ItemStatus.NULL, rom.ItemStatus) == true)
                return false;

            // DatItem_Summation
            if (!PassStringFilter(Summation, rom.Summation))
                return false;

            // DatItem_Value
            if (!PassStringFilter(Value, rom.Value))
                return false;

            // Filter on DataArea
            if (rom.DataAreaSpecified)
            {
                if (!PassesFilters(rom.DataArea))
                    return false;
            }

            // Filter on Part
            if (rom.PartSpecified)
            {
                if (!PassesFilters(rom.Part))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if a SharedFeature passes the filters
        /// </summary>
        /// <param name="sharedFeature">SharedFeature to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(SharedFeature sharedFeature)
        {
            // DatItem_Value
            if (!PassStringFilter(Value, sharedFeature.Value))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Slot passes the filters
        /// </summary>
        /// <param name="slot">Slot to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Slot slot)
        {
            // Filter on individual slot options
            if (slot.SlotOptionsSpecified)
            {
                foreach (SlotOption subSlotOption in slot.SlotOptions)
                {
                    if (!PassesFilters(subSlotOption))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check to see if a SlotOption passes the filters
        /// </summary>
        /// <param name="slotOption">SlotOption to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(SlotOption slotOption)
        {
            // DatItem_SlotOption_Default
            if (!PassBoolFilter(SlotOption_Default, slotOption.Default))
                return false;

            // DatItem_SlotOption_DeviceName
            if (!PassStringFilter(SlotOption_DeviceName, slotOption.DeviceName))
                return false;

            // DatItem_SlotOption_Name
            if (!PassStringFilter(SlotOption_Name, slotOption.Name))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a SoftwareList passes the filters
        /// </summary>
        /// <param name="softwareList">SoftwareList to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(SoftwareList softwareList)
        {
            // DatItem_Filter
            if (!PassStringFilter(Filter, softwareList.Filter))
                return false;

            // DatItem_SoftwareListStatus
            if (SoftwareListStatus.MatchesPositive(Core.SoftwareListStatus.None, softwareList.Status) == false)
                return false;
            if (SoftwareListStatus.MatchesNegative(Core.SoftwareListStatus.None, softwareList.Status) == true)
                return false;

            // DatItem_Tag
            if (!PassStringFilter(Tag, softwareList.Tag))
                return false;

            return true;
        }

        /// <summary>
        /// Check to see if a Sound passes the filters
        /// </summary>
        /// <param name="sound">Sound to check</param>
        /// <returns>True if the item passed the filter, false otherwise</returns>
        private bool PassesFilters(Sound sound)
        {
            // DatItem_Channels
            if (!PassLongFilter(Channels, sound.Channels))
                return false;

            return true;
        }

        #endregion
    }
}
