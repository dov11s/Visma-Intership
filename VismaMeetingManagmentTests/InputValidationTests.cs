namespace VismaMeetingManagmentTests;
internal class InputValidationTests
{
    InputValidation _validation;


    [SetUp]
    public void SetUp()
    {
        _validation = new InputValidation();
    }


    [Test]
    [TestCase("aa", 3, ExpectedResult = false)]
    [TestCase("aaa", 3, ExpectedResult = true)]
    [TestCase("", 3, ExpectedResult = false)]
    [TestCase(null, 3, ExpectedResult = false)]
    public bool ValidateString_WhenStringIsLongerThan_ReturnsTrue(string text, int length)
        => _validation.ValidateString(text, length);


    [Test]
    [TestCase("aa", ExpectedResult = false)]
    [TestCase("125", ExpectedResult = true)]
    [TestCase("", ExpectedResult = false)]
    [TestCase(null, ExpectedResult = false)]
    [TestCase("111abs55", ExpectedResult = false)]
    public bool ValidateInteger_WhenStringIsInteger_ReturnsTrue(string text)
        => _validation.ValidateInteger(text);


    [Test]
    [TestCase("0", ExpectedResult = true)]
    [TestCase("Hub", ExpectedResult = true)]
    [TestCase("3", ExpectedResult = true)]

    [TestCase("", ExpectedResult = false)]
    [TestCase(null, ExpectedResult = false)]
    [TestCase("5", ExpectedResult = false)]
    [TestCase("TEMP", ExpectedResult = false)]
    public bool ValidateCategorie_WhenCategorieExists_ReturnsTrue(string text) 
        => _validation.ValidateCategorie(text);


    [Test]
    [TestCase("0", ExpectedResult = true)]
    [TestCase("Live", ExpectedResult = true)]
    [TestCase("1", ExpectedResult = true)]

    [TestCase("3", ExpectedResult = false)]
    [TestCase("", ExpectedResult = false)]
    [TestCase(null, ExpectedResult = false)]
    [TestCase("TEMP", ExpectedResult = false)]
    public bool ValidateType_WhenCategorieExists_ReturnsTrue(string text)
        => _validation.ValidateType(text);


   

    [Test]
    [TestCase("2022-12-01 19:55", "yyyy-MM-dd HH:mm", ExpectedResult = true)]
    [TestCase("2022-12-01 10:55", "yyyy-MM-dd hh:mm", ExpectedResult = true)]
    [TestCase("2022-12-01", "yyyy-MM-dd", ExpectedResult = true)]

    [TestCase("2022-12-01", "", ExpectedResult = false)]
    [TestCase("2022-12-01", "yyyy-MM-dd HH:mm", ExpectedResult = false)]
    [TestCase("2022-12-01 14:55", "yyyy-MM-dd hh:mm", ExpectedResult = false)]
    [TestCase("2022-12-01 10:00", "yyyy-MM-dd", ExpectedResult = false)]
    [TestCase("", "", ExpectedResult = false)]
    [TestCase(null, "", ExpectedResult = false)]
    public bool ValidateDate_WhenDateIsCorrectFormat_ReturnsTrue(string date, string format)
        => _validation.ValidateDate(date, format);

    [Test]
    [TestCase("2022-12-01 19:55", "yyyy-MM-dd HH:mm", "2022-12-01 19:30", ExpectedResult = true)]
    [TestCase("2022-12-02", "yyyy-MM-dd", "2022-12-01", ExpectedResult = true)]

    [TestCase("2022-12-01 19:55", "yyyy-MM-dd HH:mm", "2022-12-01 20:30", ExpectedResult = false)]
    [TestCase("2022-12-02", "yyyy-MM-dd", "2022-12-08", ExpectedResult = false)]
    [TestCase("2022-12-01", "yyyy-MM-dd HH:mm", "2022-12-01 10:00", ExpectedResult = false)]
    [TestCase("", "yyyy-MM-dd HH:mm", "2022-12-01 10:00", ExpectedResult = false)]
    [TestCase(null, "yyyy-MM-dd HH:mm", "2022-12-01 10:00", ExpectedResult = false)]
    public bool ValidateDate_WhenDateIsAfterGiven_ReturnsTrue(string after, string format, string before)
        => _validation.ValidateDate(after, format, DateTime.ParseExact(before, format, null));

    [Test]
    [TestCase("444", 3, ExpectedResult = true)]
    [TestCase("8689", 4, ExpectedResult = true)]

    [TestCase("111", 3, ExpectedResult = false)]
    [TestCase("22", 3, ExpectedResult = false)]
    public bool ValidateMeetingName_WhenNameIsFreeToUse_ReturnsTrue(string meetingNmae, int minlegth)
    {
        List<Meeting> temp = CreateMeetingsData();

        _validation.UpdateMeetingData(temp, "");

        return _validation.ValidateMeetingName(meetingNmae, minlegth);
    }

    [Test]
    [TestCase("111", 3, ExpectedResult = true)]
    [TestCase("333", 3, ExpectedResult = true)]

    [TestCase("1111", 4, ExpectedResult = false)]
    [TestCase("22", 3, ExpectedResult = false)]
    public bool ValidateMeetingExist_WhenMeetingExist_ReturnsTrue(string meetingNmae, int minlegth)
    {
        List<Meeting> temp = CreateMeetingsData();

        _validation.UpdateMeetingData(temp, "");

        return _validation.ValidateMeetingExist(meetingNmae, minlegth);
    }

    [Test]
    [TestCase("111", 3, "1", ExpectedResult = true)]
    [TestCase("333", 3, "3", ExpectedResult = true)]

    [TestCase("1111", 4, "1", ExpectedResult = false)]
    [TestCase("22", 3, "2", ExpectedResult = false)]
    [TestCase("111", 3, "2", ExpectedResult = false)]
    [TestCase("222", 3, "1", ExpectedResult = false)]
    public bool ValidateMeetingYours_WhenMeetingIsYours_ReturnsTrue(string meetingNmae, int minlegth, string username)
    {
        List<Meeting> temp = CreateMeetingsData();

        _validation.UpdateMeetingData(temp, username);

        return _validation.ValidateMeetingYours(meetingNmae, minlegth);
    }



    List<Meeting> CreateMeetingsData()
    {
        List<Meeting> temp = new();
        for (int i = 1; i <= 3; i++)
        {
            DateTime sDate = DateTime.Now.AddDays(i);
            string name = i.ToString();

            Meeting tempMeeting = new(name + name + name, name, name, sDate, sDate.AddHours(i), Categories.Hub, Types.Live);
            temp.Add(tempMeeting);
        }

        return temp;
    }

}
