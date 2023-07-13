using System;
using System.Xml.Serialization;
using Xunit;

namespace SabreTools.Test.Parser
{
    public class SerializationTests
    {
        [Fact]
        public void ArchiveDotOrgDeserializeTest()
        {
            // Open the file for reading
            string filename = System.IO.Path.Combine(Environment.CurrentDirectory, "TestData", "test-archivedotorg-files.xml");
            using var fs = System.IO.File.OpenRead(filename);

            // Setup the serializer
            var serializer = new XmlSerializer(typeof(Models.ArchiveDotOrg.Files));

            // Deserialize the file
            var dat = serializer.Deserialize(fs) as Models.ArchiveDotOrg.Files;

            // Validate the values
            Assert.NotNull(dat);
            Assert.NotNull(dat.File);
            Assert.Equal(22, dat.File.Length);

            // Validate we're not missing any attributes or elements
            Assert.Null(dat.ADDITIONAL_ATTRIBUTES);
            Assert.Null(dat.ADDITIONAL_ELEMENTS);
            foreach (var file in dat.File)
            {
                Assert.Null(file.ADDITIONAL_ATTRIBUTES);
                Assert.Null(file.ADDITIONAL_ELEMENTS);
            }
        }

        [Fact]
        public void ListxmlDeserializeTest()
        {
            // Open the file for reading
            string filename = System.IO.Path.Combine(Environment.CurrentDirectory, "TestData", "test-listxml-files.xml.gz");
            using var fs = System.IO.File.OpenRead(filename);
            using var gz = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Decompress);
            using var xr = System.Xml.XmlReader.Create(gz, new System.Xml.XmlReaderSettings { DtdProcessing = System.Xml.DtdProcessing.Parse });

            // Setup the serializer
            var serializer = new XmlSerializer(typeof(Models.Listxml.Mame));

            // Deserialize the file
            var dat = serializer.Deserialize(xr) as Models.Listxml.Mame;

            // Validate the values
            Assert.NotNull(dat);
            Assert.NotNull(dat.Machine);
            Assert.Equal(45861, dat.Machine.Length);

            // Validate we're not missing any attributes or elements
            Assert.Null(dat.ADDITIONAL_ATTRIBUTES);
            Assert.Null(dat.ADDITIONAL_ELEMENTS);
            foreach (var machine in dat.Machine)
            {
                Assert.Null(machine.ADDITIONAL_ATTRIBUTES);
                Assert.Null(machine.ADDITIONAL_ELEMENTS);

                foreach (var biosset in machine.BiosSet ?? Array.Empty<Models.Listxml.BiosSet>())
                {
                    Assert.Null(biosset.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(biosset.ADDITIONAL_ELEMENTS);
                }

                foreach (var rom in machine.Rom ?? Array.Empty<Models.Listxml.Rom>())
                {
                    Assert.Null(rom.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(rom.ADDITIONAL_ELEMENTS);
                }

                foreach (var disk in machine.Disk ?? Array.Empty<Models.Listxml.Disk>())
                {
                    Assert.Null(disk.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(disk.ADDITIONAL_ELEMENTS);
                }

                foreach (var deviceRef in machine.DeviceRef ?? Array.Empty<Models.Listxml.DeviceRef>())
                {
                    Assert.Null(deviceRef.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(deviceRef.ADDITIONAL_ELEMENTS);
                }

                foreach (var sample in machine.Sample ?? Array.Empty<Models.Listxml.Sample>())
                {
                    Assert.Null(sample.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(sample.ADDITIONAL_ELEMENTS);
                }

                foreach (var chip in machine.Chip ?? Array.Empty<Models.Listxml.Chip>())
                {
                    Assert.Null(chip.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(chip.ADDITIONAL_ELEMENTS);
                }

                foreach (var display in machine.Display ?? Array.Empty<Models.Listxml.Display>())
                {
                    Assert.Null(display.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(display.ADDITIONAL_ELEMENTS);
                }

                if (machine.Sound != null)
                {
                    Assert.Null(machine.Sound.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(machine.Sound.ADDITIONAL_ELEMENTS);
                }

                if (machine.Input != null)
                {
                    Assert.Null(machine.Input.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(machine.Input.ADDITIONAL_ELEMENTS);

                    foreach (var control in machine.Input.Control ?? Array.Empty<Models.Listxml.Control>())
                    {
                        Assert.Null(control.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(control.ADDITIONAL_ELEMENTS);
                    }
                }

                foreach (var dipswitch in machine.DipSwitch ?? Array.Empty<Models.Listxml.DipSwitch>())
                {
                    Assert.Null(dipswitch.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(dipswitch.ADDITIONAL_ELEMENTS);

                    if (dipswitch.Condition != null)
                    {
                        Assert.Null(dipswitch.Condition.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(dipswitch.Condition.ADDITIONAL_ELEMENTS);
                    }

                    foreach (var diplocation in dipswitch.DipLocation ?? Array.Empty<Models.Listxml.DipLocation>())
                    {
                        Assert.Null(diplocation.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(diplocation.ADDITIONAL_ELEMENTS);
                    }

                    foreach (var dipvalue in dipswitch.DipValue ?? Array.Empty<Models.Listxml.DipValue>())
                    {
                        Assert.Null(dipvalue.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(dipvalue.ADDITIONAL_ELEMENTS);

                        if (dipvalue.Condition != null)
                        {
                            Assert.Null(dipvalue.Condition.ADDITIONAL_ATTRIBUTES);
                            Assert.Null(dipvalue.Condition.ADDITIONAL_ELEMENTS);
                        }
                    }
                }

                foreach (var configuration in machine.Configuration ?? Array.Empty<Models.Listxml.Configuration>())
                {
                    Assert.Null(configuration.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(configuration.ADDITIONAL_ELEMENTS);

                    if (configuration.Condition != null)
                    {
                        Assert.Null(configuration.Condition.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(configuration.Condition.ADDITIONAL_ELEMENTS);
                    }

                    foreach (var conflocation in configuration.ConfLocation ?? Array.Empty<Models.Listxml.ConfLocation>())
                    {
                        Assert.Null(conflocation.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(conflocation.ADDITIONAL_ELEMENTS);
                    }

                    foreach (var confsetting in configuration.ConfSetting ?? Array.Empty<Models.Listxml.ConfSetting>())
                    {
                        Assert.Null(confsetting.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(confsetting.ADDITIONAL_ELEMENTS);

                        if (confsetting.Condition != null)
                        {
                            Assert.Null(confsetting.Condition.ADDITIONAL_ATTRIBUTES);
                            Assert.Null(confsetting.Condition.ADDITIONAL_ELEMENTS);
                        }
                    }
                }

                foreach (var port in machine.Port ?? Array.Empty<Models.Listxml.Port>())
                {
                    Assert.Null(port.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(port.ADDITIONAL_ELEMENTS);

                    foreach (var analog in port.Analog ?? Array.Empty<Models.Listxml.Analog>())
                    {
                        Assert.Null(analog.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(analog.ADDITIONAL_ELEMENTS);
                    }
                }

                foreach (var adjuster in machine.Adjuster ?? Array.Empty<Models.Listxml.Adjuster>())
                {
                    Assert.Null(adjuster.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(adjuster.ADDITIONAL_ELEMENTS);

                    if (adjuster.Condition != null)
                    {
                        Assert.Null(adjuster.Condition.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(adjuster.Condition.ADDITIONAL_ELEMENTS);
                    }
                }

                if (machine.Driver != null)
                {
                    Assert.Null(machine.Driver.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(machine.Driver.ADDITIONAL_ELEMENTS);
                }

                foreach (var feature in machine.Feature ?? Array.Empty<Models.Listxml.Feature>())
                {
                    Assert.Null(feature.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(feature.ADDITIONAL_ELEMENTS);
                }

                foreach (var device in machine.Device ?? Array.Empty<Models.Listxml.Device>())
                {
                    Assert.Null(device.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(device.ADDITIONAL_ELEMENTS);

                    if (device.Instance != null)
                    {
                        Assert.Null(device.Instance.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(device.Instance.ADDITIONAL_ELEMENTS);
                    }

                    foreach (var extension in device.Extension ?? Array.Empty<Models.Listxml.Extension>())
                    {
                        Assert.Null(extension.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(extension.ADDITIONAL_ELEMENTS);
                    }
                }

                foreach (var slot in machine.Slot ?? Array.Empty<Models.Listxml.Slot>())
                {
                    Assert.Null(slot.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(slot.ADDITIONAL_ELEMENTS);

                    foreach (var slotoption in slot.SlotOption ?? Array.Empty<Models.Listxml.SlotOption>())
                    {
                        Assert.Null(slotoption.ADDITIONAL_ATTRIBUTES);
                        Assert.Null(slotoption.ADDITIONAL_ELEMENTS);
                    }
                }

                foreach (var softwarelist in machine.SoftwareList ?? Array.Empty<Models.Listxml.SoftwareList>())
                {
                    Assert.Null(softwarelist.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(softwarelist.ADDITIONAL_ELEMENTS);
                }

                foreach (var ramoption in machine.RamOption ?? Array.Empty<Models.Listxml.RamOption>())
                {
                    Assert.Null(ramoption.ADDITIONAL_ATTRIBUTES);
                    Assert.Null(ramoption.ADDITIONAL_ELEMENTS);
                }
            }
        }
    }
}