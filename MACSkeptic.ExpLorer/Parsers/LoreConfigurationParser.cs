namespace MACSkeptic.ExpLorer.Parsers
{
    public class LoreConfigurationParser : GenericConfigurationParser
    {
        protected override string MainConfigurationExtension { get { return ".lore"; } }
        protected override string UnderlingConfigurationExtension { get { return ".tale"; } }
    }
}