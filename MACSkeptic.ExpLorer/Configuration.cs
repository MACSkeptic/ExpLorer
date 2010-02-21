﻿using System.Collections.Generic;
using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer
{
    public class Configuration
    {
        private readonly string _name;
        private readonly string _value;

        private IDictionary<string, Configuration> _underlings;

        public Configuration(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public Configuration(string name, params Configuration[] underlings)
        {
            _name = name;
            _value = null;
            underlings.ExecuteForEach(Add);
        }

        public Configuration(string name, string value, Configuration @base)
        {
            _name = name;
            _value = value;
            if (@base != null)
            {
                @base.Add(this);
            }
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

        protected virtual IDictionary<string, Configuration> Underlings
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
        }

        public virtual string ToString(bool shallow)
        {
            var shallowRepresentation = "{MACSkeptic.ExpLorer.Configuration: {#{Name}: #{Value}}}".ApplyArguments(this);

            if (shallow)
            {
                return shallowRepresentation;
            }

            return null;
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