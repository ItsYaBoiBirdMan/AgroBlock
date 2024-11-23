using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class CSVConverter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string inputFilePath = "Crop necessities.csv";
        string outputFilePath = "Crops.json";
        string csvFilePath = Path.Combine(Application.dataPath, "CSV", inputFilePath);
        string jsonOutputPath = Path.Combine(Application.dataPath, "JSON", outputFilePath);
        var crops = ParseCsvToJson(csvFilePath);
        Debug.Log(crops);
 
        string jsonOutput = JsonConvert.SerializeObject(crops, Formatting.Indented);
        File.WriteAllText(jsonOutputPath, jsonOutput);
    }

    static List<Crop> ParseCsvToJson(string filePath)
    {
        var crops = new Dictionary<string, Crop>();

        using (var reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                // Extract the data
                string cropType = values[0];
                string growthStage = values[1];
                int timeMin = int.Parse(values[2]);
                int timeMax = int.Parse(values[3]);
                string soilType = values[4];
                int soilHumidityMin = int.Parse(values[5]);
                int soilHumidityMax = int.Parse(values[6]);
                int dayTempMin = int.Parse(values[7]);
                int dayTempMax = int.Parse(values[8]);
                int nightTempMin = int.Parse(values[9]);
                int nightTempMax = int.Parse(values[10]);
                int lightPeriodMin = int.Parse(values[11]);
                int lightPeriodMax = int.Parse(values[12]);
                int nitrogenMin = int.Parse(values[13]);
                int nitrogenMax = int.Parse(values[14]);
                int phosphorusMin = int.Parse(values[15]);
                int phosphorusMax = int.Parse(values[16]);
                int potassiumMin = int.Parse(values[17]);
                int potassiumMax = int.Parse(values[18]);

                // Check if the crop already exists
                if (!crops.ContainsKey(cropType))
                {
                    crops[cropType] = new Crop { Name = cropType, Soils = new List<Soil>() };
                }

                // Get the crop object
                var crop = crops[cropType];

                // Check if the soil type already exists
                var soil = crop.Soils.Find(s => s.Type == soilType);
                if (soil == null)
                {
                    soil = new Soil { Type = soilType, GrowthStages = new List<GrowthStage>() };
                    crop.Soils.Add(soil);
                }

                // Add the growth stage to the soil
                soil.GrowthStages.Add(new GrowthStage
                {
                    Stage = growthStage,
                    Time = new TimeRange { Min = timeMin, Max = timeMax },
                    Humidity = new Range { Min = soilHumidityMin, Max = soilHumidityMax },
                    Temperature = new Temperature
                    {
                        Day = new Range { Min = dayTempMin, Max = dayTempMax },
                        Night = new Range { Min = nightTempMin, Max = nightTempMax }
                    },
                    Light = new LightPeriod { Period = new Range { Min = lightPeriodMin, Max = lightPeriodMax } },
                    Nutrients = new Nutrients
                    {
                        Nitrogen = new Range { Min = nitrogenMin, Max = nitrogenMax },
                        Phosphorus = new Range { Min = phosphorusMin, Max = phosphorusMax },
                        Potassium = new Range { Min = potassiumMin, Max = potassiumMax }
                    }
                });
            }
        }

        // Convert the dictionary to a list of crops
        return new List<Crop>(crops.Values);
    }


// Classes representing the structure
public class Crop
{
    public string Name { get; set; }
    public List<Soil> Soils { get; set; }
}

public class Soil
{
    public string Type { get; set; }
    public List<GrowthStage> GrowthStages { get; set; }
}

public class GrowthStage
{
    public string Stage { get; set; }
    public TimeRange Time { get; set; }
    public Range Humidity { get; set; }
    public Temperature Temperature { get; set; }
    public LightPeriod Light { get; set; }
    public Nutrients Nutrients { get; set; }
}

public class TimeRange
{
    public int Min { get; set; }
    public int Max { get; set; }
}

public class Temperature
{
    public Range Day { get; set; }
    public Range Night { get; set; }
}

public class LightPeriod
{
    public Range Period { get; set; }
}

public class Nutrients
{
    public Range Nitrogen { get; set; }
    public Range Phosphorus { get; set; }
    public Range Potassium { get; set; }
}

public class Range {
    public int Min { get; set; }
    public int Max { get; set; }
}
}
