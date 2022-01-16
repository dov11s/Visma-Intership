namespace VismaMeetingManagmentSystem.models;
public class Meeting
{
    public string Name { get; private set; }
    public string ResponsiblePerson { get; private set; }
    public string Description { get; private set; }

    public Categories Category { get; private set; }
    public Types Type { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public List<Person> Persons { get; private set; }



    public Meeting(string name, string responsiblePerson, string description, DateTime startDate, DateTime endDate, Categories category, Types type)
    {
        Name = name;
        ResponsiblePerson = responsiblePerson;
        Description = description;

        StartDate = startDate;
        EndDate = endDate;

        Category = category;
        Type = type;

        Persons = new List<Person>();
    }


    /// <summary>
    /// Adds person to Persons list if not already in it
    /// </summary>
    /// <param name="name">persons name</param>
    /// <param name="date">date when added</param>
    /// <returns>true if succeeded</returns>
    public bool AddPerson(string name, DateTime date)
    {
        if (!Persons.Any(person => person.Name == name))
        {
            Persons.Add(new Person(name, date));
            PrintLine();
            WriteLine("{2}: {0} successfully added to meeting: {1}", name, Name, date.ToString("yyyy-MM-dd HH:mm"));
            PrintLine();
            return true;
        }

        WriteLine("{0} is already in this meeting", name);
        return false;
    }


    /// <summary>
    /// Removes person from Persons list if he is not responsible for it
    /// and he exists in it
    /// </summary>
    /// <param name="name">person's name</param>
    /// <returns>true if succeeded</returns>
    public bool RemovePerson(string name)
    {
        if (Persons.Any(person => person.Name == name))
        {
            if (ResponsiblePerson != name)
            {
                Persons.RemoveAll(person => person.Name == name);

                PrintLine();
                WriteLine("{0} was successfully removed from meeting: {1}", name, Name);
                PrintLine();
                return true;
            }

            else
            {
                WriteLine("{0} cannot be removed. He is responsible person", name);
                return false;
            }
        }

        WriteLine("{0} is not present in this meeting", name);
        return false;
    }


    public static string TableName()
    {
        return String.Format("|{0,64} {1, 62}", "Meeting", "|");
    }

    public static string TableHeader()
    {
        return String.Format("|{0,-10}|{1,-20}|{2,-30}|{3,-10}|{4,-10}|{5,-20}|{6,-20}|", "Name",
        "ResponsiblePerson", "Descriptiopn", "Category", "Type", "StartDate", "EndDate");
    }

    public string TableEntry()
    {
        if(Description.Length > 28)
        {
            string entry = "";

            string[] chunks = ChunksUpto(Description, 28).ToArray();

            entry+= String.Format("|{0,-10}|{1,-20}|{2,-30}|{3,-10}|{4,-10}|{5,-20}|{6,-20}|", Name,
            ResponsiblePerson, chunks[0], Category, Type, StartDate.ToString("yyyy-MM-dd HH:mm"),
            EndDate.ToString("yyyy-MM-dd HH:mm")) + "\n";

            for (int i = 1; i < chunks.Length; i++)
                entry += String.Format("|{0,-10}|{1,-20}|{2,-30}|{3,-10}|{4,-10}|{5,-20}|{6,-20}|", "",
                "", chunks[i], "", "", "", "") + "\n";


            return entry.Remove(entry.Length-2);
        }

        return String.Format("|{0,-10}|{1,-20}|{2,-30}|{3,-10}|{4,-10}|{5,-20}|{6,-20}|", Name,
            ResponsiblePerson, Description, Category, Type, StartDate.ToString("yyyy-MM-dd HH:mm"),
            EndDate.ToString("yyyy-MM-dd HH:mm"));
    }


    void PrintLine(int lenght = 70)
    {
        string line = new('-', lenght);
        WriteLine(line);
    }


    /// <summary>
    /// create an array of chunked text
    /// </summary>
    /// <param name="str">text that need to bee chunked</param>
    /// <param name="maxChunkSize">chunk size</param>
    /// <returns>array of chunked text</returns>
    public IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
    {
        for (int i = 0; i < str.Length; i += maxChunkSize)
            yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
    }
}

