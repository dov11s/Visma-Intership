
namespace VismaMeetingManagmentTests;
internal class MeetingTests
{
    Meeting _meet;

    [SetUp]
    public void SetUp()
    {
        _meet = new("ex", "responsible", "ex", DateTime.Now, DateTime.Now.AddHours(1), Categories.Hub, Types.Inperson);
        _meet.AddPerson("tomas", DateTime.Now);
        _meet.AddPerson("jonas", DateTime.Now);
        _meet.AddPerson("petas", DateTime.Now);
        _meet.AddPerson("responsible", DateTime.Now);
    }


    [Test]
    [TestCase("aaabbbccc", 3, ExpectedResult = 3)]
    [TestCase("aaabbbcccd", 3, ExpectedResult = 4)]
    [TestCase("aaabbbcccd", 10, ExpectedResult = 1)]
    [TestCase("aaa", 1, ExpectedResult = 3)]
    public int ChunksUpto_WhenStringIsDevidedIntoChunks_ReturnsTrue(string text, int chunkSize)
        => _meet.ChunksUpto(text, chunkSize).ToArray().Length;

    [Test]
    [TestCase("erika", ExpectedResult = true)]
    [TestCase("toma", ExpectedResult = true)]
    [TestCase("tomas", ExpectedResult = false)]
    [TestCase("responsible", ExpectedResult = false)]
   
    public bool AddPerson_WhenAdded_ReturnsTrue(string name)
        => _meet.AddPerson(name, DateTime.Now);

    [Test]
    [TestCase("tomas", ExpectedResult = true)]
    [TestCase("jonas", ExpectedResult = true)]
    [TestCase("kebabas", ExpectedResult = false)]
    [TestCase("responsible", ExpectedResult = false)]

    public bool RemovePerson_WhenRemoved_ReturnsTrue(string name)
       => _meet.RemovePerson(name);


}
