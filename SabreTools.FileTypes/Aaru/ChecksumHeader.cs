﻿using System.IO;
using SabreTools.IO.Extensions;

namespace SabreTools.FileTypes.Aaru
{
    /// <summary>
    /// Checksum block, contains a checksum of all user data sectors
    /// (except for optical discs that is 2352 bytes raw sector if available
    /// </summary>
    /// <see href="https://github.com/aaru-dps/Aaru/blob/master/Aaru.Images/AaruFormat/Structs.cs" />
    public class ChecksumHeader
    {
        /// <summary>Identifier, <see cref="BlockType.ChecksumBlock" /></summary>
        public AaruBlockType identifier;
        /// <summary>Length in bytes of the block</summary>
        public uint length;
        /// <summary>How many checksums follow</summary>
        public byte entries;

        /// <summary>
        /// Read a stream as an ChecksumHeader
        /// </summary>
        /// <param name="stream">ChecksumHeader as a stream</param>
        /// <returns>Populated ChecksumHeader, null on failure</returns>
        public static ChecksumHeader Deserialize(Stream stream)
        {
            var checksumHeader = new ChecksumHeader();

            checksumHeader.identifier = (AaruBlockType)stream.ReadUInt32();
            checksumHeader.length = stream.ReadUInt32();
            checksumHeader.entries = stream.ReadByteValue();

            return checksumHeader;
        }
    }
}
