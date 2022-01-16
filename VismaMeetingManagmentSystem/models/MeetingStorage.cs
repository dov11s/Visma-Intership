namespace VismaMeetingManagmentSystem.models;
public class MeetingStorage
{
    string _filePath;

    public MeetingStorage(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Reads file and if it's correct format and not empty return's meeting list
    /// </summary>
    /// <returns>meeting list</returns>
    public List<Meeting>? ReadAllMeetings()
    {
        List<Meeting>? meetingList = new();

        if (!File.Exists(_filePath))
            return meetingList;
       
        string json = File.ReadAllText(_filePath);

        if (json.Length == 0)
            return meetingList;
        


        try
        {
            meetingList = JsonConvert.DeserializeObject<List<Meeting>>(json, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm"
            });
        }
        catch (Exception ex)
        {
            WriteLine("Could not parse meeting's information from " + _filePath);
            WriteLine(ex.Message);
        }
        
        return meetingList;
    }

    /// <summary>
    /// Updates file with new meetings data
    /// </summary>
    /// <param name="meetings">meetings data</param>
    public void UpdateJsonFile(List<Meeting> meetings)
    {
        string json = JsonConvert.SerializeObject(meetings, new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-dd HH:mm",
            Formatting = Formatting.Indented
        });

        File.WriteAllText(_filePath, json);
    }
}