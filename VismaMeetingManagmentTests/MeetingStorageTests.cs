
namespace VismaMeetingManagmentTests;
internal class MeetingStorageTests
{
    [Test]
    public void ReadAllMeetings_WhenFileDoesntExist_ReturnsEmptyList()
    {
        MeetingStorage storage = new("");
        List<Meeting>? meetings = storage.ReadAllMeetings();

        Assert.IsEmpty(meetings);
    }

    [Test]
    public void ReadAllMeetings_WhenFileIsEmpty_ReturnsEmptyList()
    {
        string tempfile = Path.GetTempFileName();
        MeetingStorage storage = new(tempfile);

        List<Meeting>? meetings = storage.ReadAllMeetings();


        File.Delete(tempfile);
        Assert.IsEmpty(meetings);
    }

    [Test]
    public void ReadAllMeetings_WhenFileIsIncorect_ReturnsEmptyLists()
    {
        string tempFile = "temp.json";
        File.WriteAllText(tempFile, "incorect format");

        MeetingStorage storage = new(tempFile);
        List<Meeting>? meetings = storage.ReadAllMeetings();

        File.Delete(tempFile);
        Assert.IsEmpty(meetings);
    }



    [Test]
    public void UpdateJsonFile_WhenListIsEmpty_WritesEmptyList()
    {
        string tempFile = "temp.json";
        MeetingStorage storage = new(tempFile);

        storage.UpdateJsonFile(new List<Meeting>());

        List<Meeting>? meetings = storage.ReadAllMeetings();
        File.Delete(tempFile);
        Assert.IsEmpty(meetings);
    }

    [Test]
    public void UpdateJsonFileANDReadAllMeetings_WhenListIsNormal_ShouldWork()
    {
        string tempFile = "temp.json";
        MeetingStorage storage = new(tempFile);

        List<Meeting> meetings = new();

        for (int i = 1; i <= 3; i++)
        {
            DateTime sDate = DateTime.Now.AddDays(i);
            string name = i.ToString();

            Meeting tempMeeting = new(name, name, name, sDate, sDate.AddHours(i), Categories.Hub, Types.Live);
            meetings.Add(tempMeeting);
        }

        storage.UpdateJsonFile(meetings);

        List<Meeting> readMeetings = storage.ReadAllMeetings();

        bool exist = true;

        for (int i = 0; i < 3; i++)
        {
            if (!meetings.Any(meeting => meeting.Name == readMeetings.ElementAt(i).Name))
                exist = false;
        }

        File.Delete(tempFile);
        Assert.IsTrue(exist);
    }



}