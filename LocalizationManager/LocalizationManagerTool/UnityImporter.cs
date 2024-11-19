using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

public class LocalizationManager
{
    private Dictionary<string, Dictionary<string, string>> localizationData = new Dictionary<string, Dictionary<string, string>>();
    private string currentLanguage = "en"; // Langue par défaut

    /// <summary>
    /// Charge un fichier de localisation (CSV, XML ou JSON).
    /// </summary>
    /// <param name="filePath">Chemin relatif ou absolu du fichier.</param>
    public void LoadLocalizationFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
#if UNITY
            Debug.LogError($"Localization file not found: {filePath}");
#endif
            return;
        }

        string extension = Path.GetExtension(filePath).ToLower();

        switch (extension)
        {
            case ".csv":
                LoadFromCSV(filePath);
                break;
            case ".xml":
                LoadFromXML(filePath);
                break;
            case ".json":
                LoadFromJSON(filePath);
                break;
            default:
#if UNITY
                Debug.LogError($"Unsupported file format: {extension}");
#endif
                break;
        }
    }

    private void LoadFromCSV(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string headerLine = sr.ReadLine();

            if (headerLine == null)
            {
#if UNITY
                         Debug.LogError("CSV file is empty.");
#endif
                return;
            }

            string[] headers = headerLine.Split(',');

            if (headers.Length < 2)
            {
#if UNITY
                Debug.LogError("CSV file must have at least two columns: Key, Language1, Language2...");
#endif
                return;
            }

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] values = line.Split(',');

                if (values.Length != headers.Length)
                {
#if UNITY
                    Debug.LogWarning("Mismatch between headers and row values. Skipping row.");
#endif
                    continue;
                }

                string key = values[0];
                if (!localizationData.ContainsKey(key))
                {
                    localizationData[key] = new Dictionary<string, string>();
                }

                for (int i = 1; i < headers.Length; i++) // Colonnes de langues
                {
                    string language = headers[i];
                    localizationData[key][language] = values[i];
                }
            }
        }
#if UNITY
        Debug.Log("Localization data loaded from CSV.");
#endif
    }

    private void LoadFromXML(string filePath)
    {
        XDocument xmlDoc = XDocument.Load(filePath);

        foreach (XElement element in xmlDoc.Root.Elements("Entry"))
        {
            string key = element.Attribute("Key")?.Value;
            if (string.IsNullOrEmpty(key)) continue;

            if (!localizationData.ContainsKey(key))
            {
                localizationData[key] = new Dictionary<string, string>();
            }

            foreach (XElement translation in element.Elements("Translation"))
            {
                string language = translation.Attribute("Language")?.Value;
                string text = translation.Value;

                if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(text))
                {
                    localizationData[key][language] = text;
                }
            }
        }
#if UNITY
        Debug.Log("Localization data loaded from XML.");
#endif
    }

    private void LoadFromJSON(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonContent);

        foreach (var keyEntry in jsonData)
        {
            string key = keyEntry.Key;
            if (!localizationData.ContainsKey(key))
            {
                localizationData[key] = new Dictionary<string, string>();
            }

            foreach (var langEntry in keyEntry.Value)
            {
                localizationData[key][langEntry.Key] = langEntry.Value;
            }
        }
#if UNITY
        Debug.Log("Localization data loaded from JSON.");
#endif
    }

    /// <summary>
    /// Définit la langue actuelle.
    /// </summary>
    /// <param name="language">Code de la langue (par exemple, "en", "fr").</param>
    public void SetLanguage(string language)
    {
        currentLanguage = language;
#if UNITY
        Debug.Log($"Language set to: {language}");
#endif
    }

    /// <summary>
    /// Récupère la traduction d'une clé.
    /// </summary>
    /// <param name="key">Clé de la traduction.</param>
    /// <returns>Traduction pour la langue actuelle.</returns>
    public string GetTranslation(string key)
    {
        if (localizationData.ContainsKey(key) && localizationData[key].ContainsKey(currentLanguage))
        {
            return localizationData[key][currentLanguage];
        }
        else
        {
#if UNITY
            Debug.LogWarning($"Translation not found for key: {key}, language: {currentLanguage}");
#endif
            return $"[{key}]"; // Retourne la clé comme valeur par défaut
        }
    }

    /// <summary>
    /// Récupère toutes les langues disponibles.
    /// </summary>
    /// <returns>Liste des codes de langues.</returns>
    public List<string> GetAvailableLanguages()
    {
        if (localizationData.Count == 0) return new List<string>();

        return new List<string>(localizationData[localizationData.Keys.First()].Keys);
    }
}
