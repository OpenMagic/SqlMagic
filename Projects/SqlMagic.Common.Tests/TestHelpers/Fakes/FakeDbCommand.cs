using System;
using System.Data;

namespace SqlMagic.Common.Tests.TestHelpers.Fakes
{
    public class FakeDbCommand : IDbCommand
    {
        public bool IsDisposed;

        /// <summary>
        /// Get/Set the value that <see cref="ExecuteNonQuery"/> returns.
        /// </summary>
        public int ReturnValueForExecuteNonQuery;

        /// <summary>
        /// Get/Set the value that <see cref="ExecuteScalar"/> returns.
        /// </summary>
        public int ReturnValueForExecuteScalar;

        public string CommandText { get; set; }

        public FakeDbCommand()
        {
            this.Parameters = new FakeDataParameterCollection();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public int CommandTimeout
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

        public CommandType CommandType
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

        public IDbConnection Connection
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

        public IDbDataParameter CreateParameter()
        {
            return new FakeDbParameter();
        }

        public int ExecuteNonQuery()
        {
            return this.ReturnValueForExecuteNonQuery;
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar()
        {
            return this.ReturnValueForExecuteScalar;
        }

        public IDataParameterCollection Parameters { get; private set; }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction Transaction
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

        public UpdateRowSource UpdatedRowSource
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

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}
