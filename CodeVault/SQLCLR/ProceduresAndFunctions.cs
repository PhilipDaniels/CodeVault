using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace SQLCLR
{
    public static class ProceduresAndFunctions
    {
        /// <summary>
        /// An example of how to log a message. This will be done on a separate connection, so that
        /// if the main transaction is rolled back the message will remain.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="numRows">Extra information: Number of rows affected by something.</param>
        [SqlProcedure]
        public static void LogMsg(string message, int? numRows)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.MyLogProc";

                if (String.IsNullOrWhiteSpace(message))
                    cmd.Parameters.AddWithValue("@message", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@message", message);

                if (numRows == null)
                    cmd.Parameters.AddWithValue("@NumRows", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@NumRows", numRows);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [Microsoft.SqlServer.Server.SqlFunction(FillRowMethodName = "FillRow_Multi", TableDefinition = "item nvarchar(4000)")]
        public static IEnumerator SplitString_Multi
            (
            [SqlFacet(MaxSize = -1)]  SqlChars Input,
            [SqlFacet(MaxSize = 255)] SqlChars Delimiter
            )
        {
            return (
                (Input.IsNull || Delimiter.IsNull) ?
                new SplitStringMulti(new char[0], new char[0]) :
                new SplitStringMulti(Input.Value, Delimiter.Value));
        }

        public static void FillRow_Multi(object obj, out SqlString item)
        {
            item = new SqlString((string)obj);
        }

        public class SplitStringMulti : IEnumerator
        {
            public SplitStringMulti(char[] TheString, char[] Delimiter)
            {
                theString = TheString;
                stringLen = TheString.Length;
                delimiter = Delimiter;
                delimiterLen = (byte)(Delimiter.Length);
                isSingleCharDelim = (delimiterLen == 1);

                lastPos = 0;
                nextPos = delimiterLen * -1;
            }

            #region IEnumerator Members

            public object Current
            {
                get
                {
                    return new string(theString, lastPos, nextPos - lastPos);
                }
            }

            public bool MoveNext()
            {
                if (nextPos >= stringLen)
                    return false;
                else
                {
                    lastPos = nextPos + delimiterLen;

                    for (int i = lastPos; i < stringLen; i++)
                    {
                        bool matches = true;

                        //Optimize for single-character delimiters
                        if (isSingleCharDelim)
                        {
                            if (theString[i] != delimiter[0])
                                matches = false;
                        }
                        else
                        {
                            for (byte j = 0; j < delimiterLen; j++)
                            {
                                if (((i + j) >= stringLen) || (theString[i + j] != delimiter[j]))
                                {
                                    matches = false;
                                    break;
                                }
                            }
                        }

                        if (matches)
                        {
                            nextPos = i;

                            //Deal with consecutive delimiters
                            if ((nextPos - lastPos) > 0)
                                return true;
                            else
                            {
                                i += (delimiterLen - 1);
                                lastPos += delimiterLen;
                            }
                        }
                    }

                    lastPos = nextPos + delimiterLen;
                    nextPos = stringLen;

                    if ((nextPos - lastPos) > 0)
                        return true;
                    else
                        return false;
                }
            }

            public void Reset()
            {
                lastPos = 0;
                nextPos = delimiterLen * -1;
            }

            #endregion

            private int lastPos;
            private int nextPos;

            private readonly char[] theString;
            private readonly char[] delimiter;
            private readonly int stringLen;
            private readonly byte delimiterLen;
            private readonly bool isSingleCharDelim;
        }







        static string GetConnectionString()
        {
            // You need Enlist=False otherwise new connections inherit the transaction environment
            // of the context connection.
            // Note that this cannot be cached in a static field or you get this error message when you
            // try to install it into SQL server:
            // CREATE ASSEMBLY failed because method 'GetConnectionString' on type 'CLRLogging.Logger'  in
            // external_access assembly 'CLRLogging' is storing to a static field. Storing to a static field
            // is not allowed in external_access assemblies.
            return String.Format
                (
                "Server=localhost;Database={0};Integrated Security=SSPI;Enlist=False",
                GetCurrentDatabaseName()
                );
        }

        static string GetCurrentDatabaseName()
        {
            using (var conn = new SqlConnection("Context Connection=true"))
            {
                conn.Open();
                return conn.Database;
            }
        }
    }
}
