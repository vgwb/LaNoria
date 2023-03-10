using vgwb.framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace vgwb.lanoria
{
    public interface IDefinition
    {
        int Id { get; }
    }

    public class DataManager : SingletonMonoBehaviour<DataManager>
    {
        public GameData Data;

        #region Profile

        private ProfileData ProfileData;
        public ProfileData Profile => ProfileData;
        private static string profileName = "default";

        public void CreateNewProfile(AppSettings settings)
        {
            ProfileData = new ProfileData {
                Version = AppManager.I.ApplicationConfig.Version,
                AppSettings = settings
            };

            SaveProfile();
        }

        public void CreateDefaultNewProfile()
        {
            CreateNewProfile(new AppSettings {
                NativeLocale = "es",
                TutorialCompleted = false,
                SfxDisabled = false
            });
        }

        public void SaveProfile()
        {
            SaveSerialized(ProfileData, Application.persistentDataPath, $"profile_{profileName}");
        }

        public bool LoadProfile()
        {
            return LoadSerialized(out ProfileData, Application.persistentDataPath, $"profile_{profileName}");
        }

        private bool SaveSerialized<T>(T data, string folderPath, string filename)
        {
            string extension = "json";
            string path = $"{folderPath}/{filename}.{extension}";

            try {
                var settings = new JsonSerializerSettings();
                var json = JsonConvert.SerializeObject(data, settings);
                var bytes = Encoding.UTF8.GetBytes(json);
                var bf = new BinaryFormatter();
                using (FileStream stream = File.Create(path)) {
                    stream.Write(bytes);
                    return true;
                }
            }
            catch (Exception e) {
                Debug.LogError($"Could not save data at path {path}\nException {e.Message}");
                return false;
            }
        }

        private bool LoadSerialized<T>(out T data, string folderPath, string key)
        {
            string extension = "json";

            string path = $"{folderPath}/{key}.{extension}";
            if (!File.Exists(path)) {
                Debug.LogError($"No file could be found at path {path}");
                data = default;
                return false;
            }

            try {
                using (StreamReader r = new StreamReader(path)) {
                    var jsonString = r.ReadToEnd();
                    var jobj = JObject.Parse(jsonString);
                    var serializer = JsonSerializer.Create();
                    data = jobj.ToObject<T>(serializer);
                    return true;
                }

            }
            catch (Exception e) {
                Debug.LogError($"Could not load data at path {path}\nException {e.Message}");
                data = default;
                return false;
            }
        }

        #endregion
    }
}
