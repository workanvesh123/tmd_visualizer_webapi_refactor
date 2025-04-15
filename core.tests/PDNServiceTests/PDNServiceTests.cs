namespace PDNServiceTests;

[TestFixture]
public class PDNServiceTests
{
    private string validPdnPath;
    private string invalidPdnPath;
    private string emptyPdnPath;

    [SetUp]
    public void Setup()
    {
        validPdnPath = $@"D:\TMEIC SVN\data\OneDrive_1_3-11-2024\70e3_240304(Trace Save and TMdN Data Included)\Database\A5CA03A_TM-70e3 - Copy.pdn";
        invalidPdnPath =  $@"D:\TMEIC SVN\data\OneDrive_1_3-11-2024\70e3_240304(Trace Save and TMdN Data Included)\Database\A5CA03A_TM-70e3 - Copy1.pdn";
        emptyPdnPath =  $@"D:\TMEIC SVN\data\OneDrive_1_3-11-2024\70e3_240304(Trace Save and TMdN Data Included)\Database\A5CA03A_TM-70e3 - Copy2.pdn";
    }

    [Test]
    public void ReadPDNData_ShouldReturnSignalsAndFaults_WhenPDNPathIsValid()
    {
        // Arrange
        PDNService.PDNPath = validPdnPath;

        // Act
        PDNService pDNService = new();
        var result = pDNService.ReadPDNData();

        // Assert
        Assert.That(result.Item1, Is.Not.Null); // Signals
        Assert.That(result.Item2, Is.Not.Null); // Faults
        Assert.That(result.Item1, Is.Not.Empty); // Signals
        Assert.That(result.Item2, Is.Not.Empty); // Faults
    }

    [Test]
    public void ReadPDNData_ShouldThrowException_WhenPDNPathIsInvalid()
    {
        // Arrange
        PDNService.PDNPath = invalidPdnPath;

        // Act & Assert
        PDNService pDNService = new();
        Assert.Throws<FileNotFoundException>(() => pDNService.ReadPDNData());
    }
    [Test]
    public void ReadPDNData_ShouldReturnEmptyLists_WhenPDNFileHasNoSignalsOrFaults()
    {
        // Arrange
        PDNService.PDNPath = emptyPdnPath;

        // Act
        PDNService pDNService = new();
        var result = pDNService.ReadPDNData();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Item1, Is.Not.Null); // Signals
            Assert.That(result.Item2, Is.Not.Null); // Faults
            Assert.That(result.Item1, Is.Empty); // Signals
            Assert.That(result.Item2, Is.Empty); // Faults
        });
    }
}
