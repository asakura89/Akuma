using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Finisar.SQLite;

namespace asmTimex
{
    public class DBConnection
    {
        private static DBConnection _instance;

        private DBConnection() { }

        public static DBConnection GetInstance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DBConnection();
                }

                return _instance;
            }
        }

        public SQLiteConnection OpenConnection(String strCon)
        {
            SQLiteConnection sqlcon = new SQLiteConnection(strCon);

            return sqlcon;
        }
    }
}
