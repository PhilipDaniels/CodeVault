using System;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.SqlServer.Server;

namespace SQLCLR
{
    /// <summary>
    /// From: http://www.mssqltips.com/sqlservertip/2022/concat-aggregates-sql-server-clr-function/
    /// Implements string concatenation. Null values within the list of things to
    /// be concatenated are ignored, they do not affect the output.
    ///    select dbo.Concat(s, ',') from (values ('a'), (null), ('b')) x(s)
    ///      --> 'a,b'
    /// </summary>
    /// <remarks>
    /// There is also http://groupconcat.codeplex.com/ which the author claims is "better"
    /// but that looks highly dubious to me, it uses a staging dictionary to store the values
    /// before it concatenates them.
    /// </remarks>
    [Serializable]
    [Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
        Format.UserDefined,
        IsInvariantToOrder = false,         // order changes the result
        IsInvariantToNulls = false,         // nulls change the result
        IsInvariantToDuplicates = false,    // duplicates change the result
        MaxByteSize = -1)]
    public struct Concat : IBinarySerialize, INullable
    {
        public Boolean IsNull { get; private set; }

        StringBuilder _accumulator;
        string _delimiter;

        public void Init()
        {
            _accumulator = new StringBuilder();
            _delimiter = String.Empty;
            IsNull = true;
        }

        public void Accumulate(SqlString value, SqlString delimiter)
        {
            if (!delimiter.IsNull & delimiter.Value.Length > 0 && !value.IsNull)
            {
                _delimiter = delimiter.Value; // save for Merge
                if (_accumulator.Length > 0)
                    _accumulator.Append(delimiter.Value);
            }

            if (!value.IsNull)
            {
                _accumulator.Append(value.Value);
                IsNull = false;
            }
        }

        public void Merge(Concat group)
        {
            if (_accumulator.Length > 0 & group._accumulator.Length > 0)
                _accumulator.Append(_delimiter);

            _accumulator.Append(group._accumulator.ToString());
        }

        public SqlString Terminate()
        {
            return IsNull ? SqlString.Null : new SqlString(_accumulator.ToString());
        }



        void IBinarySerialize.Read(System.IO.BinaryReader r)
        {
            IsNull = r.ReadBoolean();
            _delimiter = r.ReadString();
            _accumulator = new StringBuilder(r.ReadString());
            if (_accumulator.Length != 0)
                IsNull = false;
        }

        void IBinarySerialize.Write(System.IO.BinaryWriter w)
        {
            w.Write(IsNull);
            w.Write(_delimiter);
            w.Write(_accumulator.ToString());
        }
    }
}
