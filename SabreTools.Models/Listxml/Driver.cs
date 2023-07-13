using System.Xml;
using System.Xml.Serialization;

namespace SabreTools.Models.Listxml
{
    [XmlRoot("driver")]
    public class Driver
    {
        /// <remarks>(good|imperfect|preliminary)</remarks>
        [XmlAttribute("status")]
        public string Status { get; set; }

        /// <remarks>(good|imperfect|preliminary)</remarks>
        [XmlAttribute("emulation")]
        public string Emulation { get; set; }

        /// <remarks>(good|imperfect|preliminary)</remarks>
        [XmlAttribute("cocktail")]
        public string Cocktail { get; set; }

        /// <remarks>(supported|unsupported)</remarks>
        [XmlAttribute("savestate")]
        public string SaveState { get; set; }

        /// <remarks>(yes|no) "no"</remarks>
        [XmlAttribute("requiresartwork")]
        public string? RequiresArtwork { get; set; }

        /// <remarks>(yes|no) "no"</remarks>
        [XmlAttribute("unofficial")]
        public string? Unofficial { get; set; }

        /// <remarks>(yes|no) "no"</remarks>
        [XmlAttribute("nosoundhardware")]
        public string? NoSoundHardware { get; set; }

        /// <remarks>(yes|no) "no"</remarks>
        [XmlAttribute("incomplete")]
        public string? Incomplete { get; set; }

        #region DO NOT USE IN PRODUCTION

        /// <remarks>Should be empty</remarks>
        [XmlAnyAttribute]
        public XmlAttribute[]? ADDITIONAL_ATTRIBUTES { get; set; }

        /// <remarks>Should be empty</remarks>
        [XmlAnyElement]
        public object[]? ADDITIONAL_ELEMENTS { get; set; }

        #endregion
    }
}