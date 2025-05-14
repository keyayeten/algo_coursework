using System;
using System.IO;
using System.Text.Json;

namespace algo_coursework
{
    public static class DataStorage
    {
        public static void Save(UserData data, string filename)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filename, json);
        }

        public static UserData Load(string filename)
        {
            if (!File.Exists(filename))
                return null;

            var json = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<UserData>(json);
        }

        public static bool Exists(string filename)
        {
            return File.Exists(filename);
        }
    }
}