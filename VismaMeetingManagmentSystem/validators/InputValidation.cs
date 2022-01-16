namespace VismaMeetingManagmentSystem.validators;
public class InputValidation
{

    List<Meeting>? _meetings;
    string? _user;

    public InputValidation()
    {
        _meetings = new List<Meeting>();
    }

    public void UpdateMeetingData(List<Meeting>? meets, string user)
    {
        _meetings = meets;
        _user = user;
    }

    public bool ValidateString(string? text, int minLenght = 3)
    {
        if ((text != null) && (text.Length >= minLenght))
            return true;

        WriteLine("Entered input must be atleast {0} characters long", minLenght);
        return false;
    }

    public bool ValidateInteger(string? text, int minLenght = 0)
    {
        if (int.TryParse(text, out _))
            return true;

        WriteLine("Entered input is not integer");
        return false;
    }

    public bool ValidateCategorie(string? text, int minLenght = 0)
    {
        if(text == null) 
            return false;

        if (int.TryParse(text, out int value) && Enum.IsDefined(typeof(Categories), value))
            return true;

        if (Enum.IsDefined(typeof(Categories), text))
            return true;

        WriteLine("This category does not exist");

        return false;
    }

    public bool ValidateType(string? text, int minLenght = 0)
    {
        if (text == null)
            return false;

        
        if (int.TryParse(text, out int value) && Enum.IsDefined(typeof(Types), value))
            return true;

        if (Enum.IsDefined(typeof(Types), text))
            return true;

        WriteLine("This type does not exist");

        return false;
    }

    /// <summary>
    /// Validates if given text is correct format of date
    /// </summary>
    /// <param name="text">text for validation</param>
    /// <param name="format">date format</param>
    /// <returns>true if date is valid</returns>
    public bool ValidateDate(string? text, string format)
    {
        if (DateTime.TryParseExact(text, format, null, DateTimeStyles.None, out _))
            return true;

        WriteLine("Time format is incorrect");

        return false;
    }


    /// <summary>
    /// Validates given text for correct date format 
    /// and checks if this date is after the given other date
    /// </summary>
    /// <param name="text">text for date validation</param>
    /// <param name="format">date format</param>
    /// <param name="before">date that needs to be before text(Date)</param>
    /// <returns>true if text is correct form and after other date(before)</returns>

    public bool ValidateDate(string? text, string format, DateTime before)
    {
        if (DateTime.TryParseExact(text, format, null, DateTimeStyles.None, out DateTime date))
        {
            if (date > before)
                return true;
            else
            {
                WriteLine("Entered date must be after {0}", before.ToString(format));
                return false;
            }
        }
         
        WriteLine("Time format is incorrect");

        return false;
    }


    /// <summary>
    /// Check if given name is not present in meetings list
    /// </summary>
    /// <param name="name">meeting's name</param>
    /// <param name="minLenght">name minimal length</param>
    /// <returns>true if name is not present in meetings list</returns>
    public bool ValidateMeetingName(string? name, int minLenght)
    {
        if(ValidateString(name, minLenght))
        {
            if (!_meetings.Any(meeting => meeting.Name == name))
                return true;
            else
                WriteLine("Meeting with name {0} already exist", name);
        }
        return false;
    }

    public bool ValidateMeetingExist(string? text, int minLenght)
    {
        if (ValidateString(text, minLenght))
        {
            if (_meetings.Any(meeting => meeting.Name == text))
                return true;
            else
                WriteLine("Meeting with name {0} does not exist", text);
        }
        return false;
    }

    public bool ValidateMeetingYours(string? text, int minLenght)
    {
        if(ValidateMeetingExist(text, minLenght))
        {
            if (_meetings.Any(meeting => meeting.Name == text && meeting.ResponsiblePerson == _user))
                return true;
            WriteLine("You are not responsible person for meeting: {0}", text);
        }

        return false;
    }

}
