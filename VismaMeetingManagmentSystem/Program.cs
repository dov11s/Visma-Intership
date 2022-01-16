
MeetingStorage storage = new("data.json");

List<Meeting>? meetings = storage.ReadAllMeetings();

InputValidation validation = new();

PrintLine();
WriteLine("\nWelcome to Visma's meeting managment system\n");

WriteLine("For best experience use console in fullscreen\n");
PrintLine();

string? user = null;
string? userInput = null;
string[] words;


bool toContinue = true;
while (toContinue)
{
    if (user == null)
    {
        WriteLine("In order to continue please login (Type login)");
        userInput = ReadLine();

        if (validation.ValidateString(userInput))
        {
            
            if (userInput.ToLower() == "login")
                LoginPerson();

            else
                WriteLine("Unknown command: {0}", userInput);
        }

        else
            WriteLine("Entered command must be atleast 2 character long");
    }
    else
    {
        PrintLine();
        WriteLine("Enter a command to continue");
        userInput = ReadLine();

        if (validation.ValidateString(userInput))
        {
            words = userInput.Split(' ');
            switch (words[0].ToLower())
            {
                case "help":
                    ShowHelp();
                    break;

                case "logout":
                    user = null;
                    break;

                case "create":
                    PrintLine();
                    WriteLine("You can create new meeting");
                    CreateMeeting();
                    break;

                case "add":
                    PrintLine();
                    AddPerson();
                    break;

                case "remove":
                    PrintLine();
                    RemovePerson();
                    break;

                case "delete":
                    PrintLine();
                    DeleteMeeting();
                    break;

                case "show":
                    FilterMeetings();
                    break;
                case "exit":
                    toContinue = false;
                    break;

                default:
                    WriteLine("Unknown command: {0}", words[0].ToLower());
                    break;
            }

        }
        else
            WriteLine("Entered command must be atleast 2 character long");
    }
}


void LoginPerson()
{
    while (true)
    {
        Write("Username: ");
        user = ReadLine();

        if (validation.ValidateString(user, 3))
        {
            PrintLine();
            WriteLine("Welcome {0}", user);
            ShowHelp();
            break;
        }
        else
            WriteLine("Username should contain atleast 3 characters");
    }
}

void ShowHelp()
{
    PrintLine();
    WriteLine("Commands that you can use:");
    WriteLine("help - to see this list");
    WriteLine("logout - to logout");
    WriteLine("create - to create new meeting");
    WriteLine("delete - to delete a meeting");
    WriteLine("add - to add a person in a selected meeting");
    WriteLine("remove - to remove a person from a selected meeting");
    WriteLine("show - to show all meetings");
    WriteLine("exit - to exit the program");
    PrintLine();
}

void FilterMeetings()
{
    if (meetings.Count == 0)
    {
        WriteLine("Meeting list is empty.");
        return;
    }


    WriteLine("To show all meetings pres enter");
    WriteLine("In order to filter the data use these commands:");
    WriteLine("0 - description - filter by description");
    WriteLine("1 - resPernson - filter by responsible person");
    WriteLine("2 - category - filter by category");
    WriteLine("3 - type - filter by type");
    WriteLine("4 - dates - filter by dates");
    WriteLine("5 - number - filter by number of attendees");

    userInput = ReadLine();

    if (userInput == null)
        return;

    List<Meeting> fileredMeeting;
    string? input;
    int value;

    switch (userInput)
    {
        case "":
            DisplayMeetingsData(meetings);
            break;

        case "0":
        case "description":
            WriteLine("if the description is “Jono .NET meetas”, searching for .NET will return this entry");
            input = RepeatUntilInputPasses(validation.ValidateString, "Description: ", 3);
            fileredMeeting = meetings.FindAll(meeting => meeting.Description.Contains(input));
            DisplayMeetingsData(fileredMeeting);
            break;

        case "1":
        case "resPernson":        
            input = RepeatUntilInputPasses(validation.ValidateString, "Responsible person: ", 3);
            fileredMeeting = meetings.FindAll(meeting => meeting.ResponsiblePerson == input);
            DisplayMeetingsData(fileredMeeting);
            break;

        case "2":
        case "category":
            PrintCategories();
            input = RepeatUntilInputPasses(validation.ValidateCategorie, "Category: ", 0);

            if(int.TryParse(input, out value))
                fileredMeeting = meetings.FindAll(meeting => (int)meeting.Category == value);
            else
                fileredMeeting = meetings.FindAll(meeting => meeting.Category.ToString() == input);

            DisplayMeetingsData(fileredMeeting);
            break;

        case "3":
        case "types":
            PrintTypes();

            input = RepeatUntilInputPasses(validation.ValidateType, "Type: ", 0);

            if (int.TryParse(input, out value))
                fileredMeeting = meetings.FindAll(meeting => (int)meeting.Type == value);
            else
                fileredMeeting = meetings.FindAll(meeting => meeting.Type.ToString() == input);

            DisplayMeetingsData(fileredMeeting);
            break;

        case "4":
        case "dates":
            WriteLine("To show meetings that will happen after, type first date and leave second date empty");
            WriteLine("To show meetings that will happen between type both dates");
            WriteLine("Time format example 2022-01-01");

            string format = "yyyy-MM-dd";
            DateTime firstDate, secondDate;

            input = RepeatUntilEnteredDatePasses(validation.ValidateDate, "First date: ", format);
            firstDate = DateTime.ParseExact(input, format, null);

            Write("Second date: ");
            input = ReadLine();

            if(input != "")
            {
                if(validation.ValidateDate(input, format, firstDate))
                {
                    secondDate = DateTime.ParseExact(input, format, null);
                    fileredMeeting = meetings.FindAll(meeting => meeting.StartDate > firstDate && meeting.StartDate < secondDate);
                }
                else
                {
                    input = RepeatUntilDateIsAfterGiven(validation.ValidateDate, "Second date: ", format, firstDate);
                    secondDate = DateTime.ParseExact(input, format, null);

                    fileredMeeting = meetings.FindAll(meeting => meeting.StartDate > firstDate && meeting.StartDate < secondDate);
                }

            }
            else
                fileredMeeting = meetings.FindAll(meeting => meeting.StartDate > firstDate);
                
            DisplayMeetingsData(fileredMeeting);
            break;

        case "5":
        case "number":
            WriteLine("Shows meetings where more or equal amount of persons are in it");

            input = RepeatUntilInputPasses(validation.ValidateInteger, "Attendees count: ", 0);
            fileredMeeting = meetings.FindAll(meeting => meeting.Persons.Count >= int.Parse(input));
            DisplayMeetingsData(fileredMeeting);
            break;

        default:
            WriteLine("Unknown filter");
            break;
    }
}

void CreateMeeting()
{
    validation.UpdateMeetingData(meetings, user);

    WriteLine("In order to create new meeting enter this data:");
    string name;
    string desc;
    string category;
    string type;
    string sDate;
    string eDate;
    DateTime stDate;
    DateTime enDate;

    name = RepeatUntilInputPasses(validation.ValidateMeetingName, "Name: ", 3);
    desc = RepeatUntilInputPasses(validation.ValidateString, "Description: ", 3);

    PrintCategories();
    category = RepeatUntilInputPasses(validation.ValidateCategorie, "Category: ", 0);
    if(!int.TryParse(category, out _))
    {
        Categories cat = (Categories)Enum.Parse(typeof(Categories), category);
        category = ((int)cat).ToString();
    }

    PrintTypes();
    type = RepeatUntilInputPasses(validation.ValidateType, "Type: ", 0);
    if (!int.TryParse(type, out _))
    {
        Types typ = (Types)Enum.Parse(typeof(Types), type);
        type = ((int)typ).ToString();
    }

    string format = "yyyy-MM-dd HH:mm";
    sDate = RepeatUntilEnteredDatePasses(validation.ValidateDate, "Start date (e.g. 2022-01-01 10:00): ", format);
    stDate = DateTime.ParseExact(sDate, format, null);

    eDate = RepeatUntilDateIsAfterGiven(validation.ValidateDate, "End date (e.g. 2022-01-01 10:00): ", format, stDate);
    enDate = DateTime.ParseExact(eDate, format, null);

    Meeting newMeeting = new(name, user, desc, stDate, enDate,
             (Categories)int.Parse(category), (Types)int.Parse(type));

    meetings.Add(newMeeting);

    PrintLine();
    WriteLine("Meeting {0} created succsessfully", name);

    newMeeting.AddPerson(user, DateTime.Now);
    storage.UpdateJsonFile(meetings);
}


void AddPerson()
{
    if (meetings.Count == 0)
    {
        WriteLine("Meeting list is empty.");
        return;
    }

    validation.UpdateMeetingData(meetings, user);

    string meetingName;
    string personName;

    WriteLine("To add new person in meeting enter this data: ");

    meetingName = RepeatUntilInputPasses(validation.ValidateMeetingExist, "Meeting's name: ", 3);

    Meeting current = meetings.Find(meeting => meeting.Name == meetingName);

    while (true)
    {
        personName = RepeatUntilInputPasses(validation.ValidateString, "Persons name: ", 3);

        if (personName == "exit")
            break;

        if (current.AddPerson(personName, DateTime.Now))
            break;
    }

    storage.UpdateJsonFile(meetings);
}

void RemovePerson()
{
    if (meetings.Count == 0)
    {
        WriteLine("Meeting list is empty.");
        return;
    }

    validation.UpdateMeetingData(meetings, user);

    string meetingName;
    string personName;

    WriteLine("To remove person from meeting enter this data: ");

    meetingName = RepeatUntilInputPasses(validation.ValidateMeetingExist, "Meeting's name: ", 3);

    Meeting current = meetings.Find(meeting => meeting.Name == meetingName);

    while (true)
    {
        personName = RepeatUntilInputPasses(validation.ValidateString, "Persons name: ", 3);

        if (current.RemovePerson(personName))
            break;
    }

    storage.UpdateJsonFile(meetings);
}

void DeleteMeeting()
{
    if(meetings.Count == 0)
    {
        WriteLine("Meeting list is empty.");
        return;
    }     

    validation.UpdateMeetingData(meetings, user);

    string meetingName;
    WriteLine("To delete meeting enter this data: ");

    meetingName = RepeatUntilInputPasses(validation.ValidateMeetingYours, "Meeting's name: ", 3);

    meetings.RemoveAll(meeting => meeting.Name == meetingName);

    PrintLine();
    WriteLine("Meeting {0} was successfully deleted", meetingName);
    PrintLine();

    storage.UpdateJsonFile(meetings);
}



/// <summary>
///  Loops until user correctly enters required information
/// </summary>
/// <param name="validate">Validation that input needs to pass</param>
/// <param name="toShow">What information needs to be entereded</param>
/// <param name="minLenght">minimal input length</param>
/// <returns>user input</returns>
string RepeatUntilInputPasses(Func<string?, int, bool> validate, string toShow, int minLenght)
{
    while (true)
    {
        Write(toShow);
        string input = ReadLine();

        if (validate(input, minLenght))
            return input;
    }
}

/// <summary>
///  Loops until user correctly enters required information
/// </summary>
/// <param name="validate">Validation that input needs to pass</param>
/// <param name="toShow">What information needs to be entereded</param>
/// <param name="format">date format</param>
/// <returns>user input</returns>
string RepeatUntilEnteredDatePasses(Func<string?, string, bool> validate, string toShow, string format)
{
    while (true)
    {
        Write(toShow);
        string input = ReadLine();

        if (validate(input, format))
            return input;
    }
}

/// <summary>
///  Loops until user correctly enters required information
/// </summary>
/// <param name="validate">Validation that input needs to pass</param>
/// <param name="toShow">What information needs to be entereded</param>
/// <param name="format">date format</param>
/// <param name="after">date that should be before entered date</param>
/// <returns>user input</returns>
string RepeatUntilDateIsAfterGiven(Func<string?, string, DateTime, bool> validate, string toShow, string format, DateTime after)
{
    while (true)
    {
        Write(toShow);
        string input = ReadLine();

        if (validate(input, format, after))
            return input;
    }
}


void DisplayMeetingsData(List<Meeting> meets)
{
    WriteLine("\n\n\n");

    if (meets.Count < 1)
    {
        WriteLine("Nothing to show");
        return;
    }

    int len = Meeting.TableName().Length;

    foreach (Meeting meeting in meets)
    {
        

        PrintLine(len);
        WriteLine(Meeting.TableName());
        PrintLine(len);

        WriteLine(Meeting.TableHeader());
        PrintLine(len);

        WriteLine(meeting.TableEntry());
        PrintLine(len);
        WriteLine();

        PrintLine(len);
        WriteLine(Person.TableName());
        PrintLine(len);

        WriteLine(Person.TableHeader());
        PrintLine(len);

        foreach (Person person in meeting.Persons)
        {
            WriteLine(person.TableEntry());
            PrintLine(len);
        }

        WriteLine("\n\n\n");
    }
}

static void PrintLine(int lenght = 70)
{
    string line = new('-', lenght);
    WriteLine(line);
}

void PrintCategories()
{
    PrintLine();
    WriteLine("Available categories: ");
    foreach (Categories cat in Enum.GetValues(typeof(Categories)))
        WriteLine("{0}- {1}", (int)cat, cat);
    PrintLine();
    WriteLine("To select category enter category name or id");
}

void PrintTypes()
{
    PrintLine();
    WriteLine("Available types: ");
    foreach (Types typ in Enum.GetValues(typeof(Types)))
        WriteLine("{0}- {1}", (int)typ, typ);
    PrintLine();
    WriteLine("To select type enter type name or id: ");
}