namespace MACSkeptic.ExpLorer.Parsers
{
    /// <summary>
    /// Coffee stands for [C][O]n[F]iguration [F]il[E] [E]xpert
    /// </summary>
    public class CoffeeConfigurationParser : GenericConfigurationParser
    {
        protected override string MainConfigurationExtension { get { return ".coffee"; } }
        protected override string UnderlingConfigurationExtension { get { return ".coffee"; } }
    }
}