using Humanizer;

namespace Raiden.SRLC
{
    public sealed class Stage
    {
        private string? _name;

        public Stage(ReleaseCycle cycle)
        {
            Cycle = cycle;
        }

        public ReleaseCycle Cycle
        {
            get;
            init;
        }

        public string Identifier => Name.ToLower();

        public string Name
        {
            get
            {
                if (_name is null)
                    return _name = Cycle.GetName();
                else
                    return _name;
            }
        }

        public override string ToString() => Cycle.Humanize().Transform(To.TitleCase);
    }
}