using System;
using System.Collections.Generic;
using MACSkeptic.ExpLorer.Parameters;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer
{
    public class Configuration
    {
        private readonly string _name;
        private readonly string _value;

        private IDictionary<string, Configuration> _underlings;

        public Configuration(string name)
            : this(name, c => { })
        {
        }

        public Configuration(string name, string value)
            : this(name, value, c => { })
        {
        }

        public Configuration(string name, Action<ConfigurationParameters> parameters)
            : this(name, string.Empty, parameters)
        {
        }

        public Configuration(string name, string value, Action<ConfigurationParameters> parameters)
        {
            _name = name;
            _value = value;
            parameters.Invoke(new ConfigurationParameters(this));
        }

        public virtual string Value { get { return _value; } }
        public virtual string Name { get { return _name; } }
        public virtual Configuration Base { get; internal set; }

        public virtual string FullName
        {
            get
            {
                return Base == null
                           ? Name
                           : "#{parent}.#{current}".ApplyArguments(new {parent = Base.FullName, current = Name});
            }
        }

        public virtual IDictionary<string, Configuration> Underlings
        {
            get
            {
                if (_underlings == null)
                {
                    _underlings = new Dictionary<string, Configuration>();
                }
                return _underlings;
            }
        }

        public virtual void Add(Configuration configuration)
        {
            if (configuration == null)
            {
                return;
            }

            configuration.Base = this;
            Underlings[configuration.Name] = configuration;
        }

        public virtual void Remove(Configuration configuration)
        {
            throw new NotImplementedException();
        }

        public virtual string ToString(bool shallow)
        {
            var shallowRepresentation = "{MACSkeptic.ExpLorer.Configuration: {#{Name}: #{Value}}}".ApplyArguments(this);

            return shallow
                       ? shallowRepresentation
                       : null;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public Configuration Get(string name)
        {
            var query = name;
            if (!Underlings.Keys.Contains(query))
            {
                query = name.ToLower();
                if (!Underlings.Keys.Contains(query))
                {
                    throw new InvalidOrMissingConfigurationException(this, name);
                }
            }
            return Underlings[query];
        }

        public override bool Equals(object something)
        {
            var other = something as Configuration;
            if (other == null)
            {
                return false;
            }
            return other.FullName == FullName && other.Value == Value;
        }
    }
}