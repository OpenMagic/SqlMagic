using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

namespace SqlMagic.Common.Tests.TestHelpers.Fakes
{
    public class FakeDataParameterCollection : ArrayList, IDataParameterCollection
    {
        public bool Contains(string parameterName)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        public object this[string parameterName]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
