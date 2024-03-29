﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.SqliteClient;
using System.Data;
using System.IO;
using Assets.Classes;

namespace Assets.Classes
{
    class DatabaseConnector
    {
        static string path = Directory.GetCurrentDirectory();
        static string conn = "URI=file:" + path + "\\Assets\\Plugins\\Database.db";

        public List<string> getUsernames()
        {
            IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            List<string> usernames = new List<string> { };
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT username from Players ";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read()){
                string username = reader.GetString(0);
                usernames.Add(username);
            }
            queryClose(dbconn, reader, dbcmd);
            return usernames;
        }
        public string getLoginAndPassword(string username) {
            IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            List<string> passwords = new List<string> { };

            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT password FROM Players WHERE username='" + username +"';";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                string password = reader.GetString(0);
                passwords.Add(password);
            }
            queryClose(dbconn, reader, dbcmd);
            if(passwords.Count == 1)
            {
                return passwords[0];
            } else
            {
                return "";
            }
        }
        public void createUser(Player player)
        {
            IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            List<string> passwords = new List<string> { };

            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "INSERT INTO Players VALUES ('"+ player.username+"\',\'"+ player.password + "\',\'" + player.ageGroup + "');";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();
            queryClose(dbconn, dbcmd);
        }

        public void saveScore(string player, float points, String game)
        {
            IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "INSERT INTO Scores VALUES ('" + player + "\',\'" + DateTime.Now.ToString() + "\',\'" + points + "\',\'" + game + "');";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();
            queryClose(dbconn, dbcmd);
        }

        public List<string> loadScores(string username, String game)
        {
            IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
            dbconn.Open();
            List<string> scores = new List<string> { };

            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT score FROM Scores WHERE player = '" + username + "' AND game = '" + game + "' ORDER BY score desc";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                string score = reader.GetString(0);
                scores.Add(score);
            }
            queryClose(dbconn, reader, dbcmd);
            return scores;
        }

        private void queryClose(IDbConnection dbconn, IDataReader reader, IDbCommand dbcmd)
        {
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
        private void queryClose(IDbConnection dbconn, IDbCommand dbcmd)
        {
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

    }

}
