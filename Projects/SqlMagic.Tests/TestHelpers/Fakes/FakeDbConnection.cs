using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlMagic.Tests.TestHelpers.Fakes
{
    public class FakeDbConnection : IDbConnection
    {
        public Func<IDbCommand> CreateCommandFactory = () => new FakeDbCommand();
        public List<IDbCommand> Commands = new List<IDbCommand>();

        public FakeDbConnection(bool openConnection = true)
        {
            if (openConnection)
            {
                this.Open();
            }
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public string ConnectionString
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

        public int ConnectionTimeout
        {
            get { throw new NotImplementedException(); }
        }

        public IDbCommand CreateCommand()
        {
            this.Commands.Add(this.CreateCommandFactory());

            return this.Commands.Last();
        }

        public string Database
        {
            get { throw new NotImplementedException(); }
        }

        public void Open()
        {
            this.State = ConnectionState.Open;
        }

        public ConnectionState State { get; private set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
