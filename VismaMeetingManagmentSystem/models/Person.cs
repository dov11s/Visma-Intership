namespace VismaMeetingManagmentSystem.models;
public class Person
{
    public string Name { get; private set; }
    public DateTime AddTime { get; private set; }
 
    public Person(string name, DateTime addTime)
    {
        Name = name;
        AddTime = addTime;
    }


    public static string TableName()
    {
        return String.Format("|{0,64} {1, 62}", "Attendees", "|");
    }

    public static string TableHeader()
    {
        return String.Format("|{0,35}{1, 28}{2,35}{1, 29}", "Attendees name",
        "|", "Time added");
    }

    public string TableEntry()
    {
        return String.Format("|{0,35}{1, 28}{2,35}{1, 29}", Name,
            "|", AddTime.ToString("yyyy-MM-dd HH:mm"));
    }



}