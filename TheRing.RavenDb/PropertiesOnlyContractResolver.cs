namespace TheRing.RavenDb
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Raven.Imports.Newtonsoft.Json.Serialization;

    #endregion

    public class PropertiesOnlyContractResolver : DefaultContractResolver
    {
        #region Constructors and Destructors

        public PropertiesOnlyContractResolver()
        {
            this.DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }

        #endregion

        #region Methods

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var result = base.GetSerializableMembers(objectType);

            return result.Where(x => x.MemberType == MemberTypes.Property).ToList();
        }

        #endregion
    }
}