using System.IO;
using System.Linq;
using System.Text;
using SabreTools.IO.Writers;
using SabreTools.Models.RomCenter;

namespace SabreTools.Serialization
{
    /// <summary>
    /// Serializer for RomCenter INI files
    /// </summary>
    public partial class RomCenter
    {
        /// <summary>
        /// Serializes the defined type to a RomCenter INI file
        /// </summary>
        /// <param name="metadataFile">Data to serialize</param>
        /// <param name="path">Path to the file to serialize to</param>
        /// <returns>True on successful serialization, false otherwise</returns>
        public static bool SerializeToFile(MetadataFile? metadataFile, string path)
        {
            using var stream = SerializeToStream(metadataFile);
            if (stream == null)
                return false;

            using var fs = File.OpenWrite(path);
            stream.CopyTo(fs);
            return true;
        }

        /// <summary>
        /// Serializes the defined type to a stream
        /// </summary>
        /// <param name="metadataFile">Data to serialize</param>
        /// <returns>Stream containing serialized data on success, null otherwise</returns>
        public static Stream? SerializeToStream(MetadataFile? metadataFile)
        {
            // If the metadata file is null
            if (metadataFile == null)
                return null;

            // Setup the writer and output
            var stream = new MemoryStream();
            var writer = new IniWriter(stream, Encoding.UTF8);

            // Write out the credits section
            WriteCredits(metadataFile.Credits, writer);

            // Write out the dat section
            WriteDat(metadataFile.Dat, writer);

            // Write out the emulator section
            WriteEmulator(metadataFile.Emulator, writer);

            // Write out the games
            WriteGames(metadataFile.Games, writer);

            // Return the stream
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// Write credits information to the current writer
        /// </summary>
        /// <param name="credits">Credits object representing the credits information</param>
        /// <param name="writer">IniWriter representing the output</param>
        private static void WriteCredits(Credits? credits, IniWriter writer)
        {
            // If the credits information is missing, we can't do anything
            if (credits == null)
                return;

            writer.WriteSection("credits");

            writer.WriteKeyValuePair("author", credits.Author);
            writer.WriteKeyValuePair("version", credits.Version);
            writer.WriteKeyValuePair("email", credits.Email);
            writer.WriteKeyValuePair("homepage", credits.Homepage);
            writer.WriteKeyValuePair("url", credits.Url);
            writer.WriteKeyValuePair("date", credits.Date);
            writer.WriteKeyValuePair("comment", credits.Comment);
            writer.WriteLine();

            writer.Flush();
        }

        /// <summary>
        /// Write dat information to the current writer
        /// </summary>
        /// <param name="dat">Dat object representing the dat information</param>
        /// <param name="writer">IniWriter representing the output</param>
        private static void WriteDat(Dat? dat, IniWriter writer)
        {
            // If the dat information is missing, we can't do anything
            if (dat == null)
                return;

            writer.WriteSection("dat");

            writer.WriteKeyValuePair("version", dat.Version);
            writer.WriteKeyValuePair("plugin", dat.Plugin);
            writer.WriteKeyValuePair("split", dat.Split);
            writer.WriteKeyValuePair("merge", dat.Merge);
            writer.WriteLine();

            writer.Flush();
        }

        /// <summary>
        /// Write emulator information to the current writer
        /// </summary>
        /// <param name="emulator">Emulator object representing the emulator information</param>
        /// <param name="writer">IniWriter representing the output</param>
        private static void WriteEmulator(Emulator? emulator, IniWriter writer)
        {
            // If the emulator information is missing, we can't do anything
            if (emulator == null)
                return;

            writer.WriteSection("emulator");

            writer.WriteKeyValuePair("refname", emulator.RefName);
            writer.WriteKeyValuePair("version", emulator.Version);
            writer.WriteLine();

            writer.Flush();
        }

        /// <summary>
        /// Write games information to the current writer
        /// </summary>
        /// <param name="games">Games object representing the games information</param>
        /// <param name="writer">IniWriter representing the output</param>
        private static void WriteGames(Games? games, IniWriter writer)
        {
            // If the games information is missing, we can't do anything
            if (games?.Rom == null || !games.Rom.Any())
                return;

            writer.WriteSection("games");

            foreach (var rom in games.Rom)
            {
                var romBuilder = new StringBuilder();

                romBuilder.Append('¬');
                romBuilder.Append(rom.ParentName);
                romBuilder.Append('¬');
                romBuilder.Append(rom.ParentDescription);
                romBuilder.Append('¬');
                romBuilder.Append(rom.GameName);
                romBuilder.Append('¬');
                romBuilder.Append(rom.GameDescription);
                romBuilder.Append('¬');
                romBuilder.Append(rom.RomName);
                romBuilder.Append('¬');
                romBuilder.Append(rom.RomCRC);
                romBuilder.Append('¬');
                romBuilder.Append(rom.RomSize);
                romBuilder.Append('¬');
                romBuilder.Append(rom.RomOf);
                romBuilder.Append('¬');
                romBuilder.Append(rom.MergeName);
                romBuilder.Append('¬');
                romBuilder.Append('\n');

                writer.WriteString(romBuilder.ToString());
                writer.Flush();
            }

            writer.WriteLine();
            writer.Flush();
        }

        #region Internal

        /// <summary>
        /// Convert from <cref="Models.RomCenter.MetadataFile"/> to <cref="Models.Internal.MetadataFile"/>
        /// </summary>
        public static Models.Internal.MetadataFile? ConvertToInternalModel(MetadataFile? item)
        {
            if (item == null)
                return null;
            
            var metadataFile = new Models.Internal.MetadataFile
            {
                [Models.Internal.MetadataFile.HeaderKey] = ConvertHeaderToInternalModel(item),
            };

            if (item?.Games?.Rom != null && item.Games.Rom.Any())
            {
                metadataFile[Models.Internal.MetadataFile.MachineKey] = item.Games.Rom
                    .Where(r => r != null)
                    .Select(ConvertMachineToInternalModel).ToArray();
            }

            return metadataFile;
        }

        /// <summary>
        /// Convert from <cref="Models.RomCenter.MetadataFile"/> to <cref="Models.Internal.Header"/>
        /// </summary>
        private static Models.Internal.Header ConvertHeaderToInternalModel(MetadataFile item)
        {
            var header = new Models.Internal.Header();

            if (item.Credits != null)
            {
                header[Models.Internal.Header.AuthorKey] = item.Credits.Author;
                header[Models.Internal.Header.VersionKey] = item.Credits.Version;
                header[Models.Internal.Header.EmailKey] = item.Credits.Email;
                header[Models.Internal.Header.HomepageKey] = item.Credits.Homepage;
                header[Models.Internal.Header.UrlKey] = item.Credits.Url;
                header[Models.Internal.Header.DateKey] = item.Credits.Date;
                header[Models.Internal.Header.CommentKey] = item.Credits.Comment;
            }

            if (item.Dat != null)
            {
                header[Models.Internal.Header.DatVersionKey] = item.Dat.Version;
                header[Models.Internal.Header.PluginKey] = item.Dat.Plugin;

                if (item.Dat.Split == "yes" || item.Dat.Split == "1")
                    header[Models.Internal.Header.ForceMergingKey] = "split";
                else if (item.Dat.Merge == "yes" || item.Dat.Merge == "1")
                    header[Models.Internal.Header.ForceMergingKey] = "merge";
            }

            if (item.Emulator != null)
            {
                header[Models.Internal.Header.RefNameKey] = item.Emulator.RefName;
                header[Models.Internal.Header.EmulatorVersionKey] = item.Emulator.Version;
            }

            return header;
        }

        /// <summary>
        /// Convert from <cref="Models.RomCenter.Game"/> to <cref="Models.Internal.Machine"/>
        /// </summary>
        private static Models.Internal.Machine ConvertMachineToInternalModel(Rom item)
        {
            var machine = new Models.Internal.Machine
            {
                [Models.Internal.Machine.RomOfKey] = item.ParentName,
                //[Models.Internal.Machine.ParentDescriptionKey] = item.ParentDescription, // This is unmappable
                [Models.Internal.Machine.NameKey] = item.GameName,
                [Models.Internal.Machine.DescriptionKey] = item.GameDescription,
                [Models.Internal.Machine.RomKey] = new Models.Internal.Rom[] { ConvertToInternalModel(item) },
            };

            return machine;
        }

        /// <summary>
        /// Convert from <cref="Models.RomCenter.Rom"/> to <cref="Models.Internal.Rom"/>
        /// </summary>
        private static Models.Internal.Rom ConvertToInternalModel(Rom item)
        {
            var rom = new Models.Internal.Rom
            {
                [Models.Internal.Rom.NameKey] = item.RomName,
                [Models.Internal.Rom.CRCKey] = item.RomCRC,
                [Models.Internal.Rom.SizeKey] = item.RomSize,
                [Models.Internal.Rom.MergeKey] = item.MergeName,
            };
            return rom;
        }

        #endregion
    }
}