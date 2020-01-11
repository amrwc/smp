using System.Threading.Tasks;
using NUnit.Framework;
using Smp.Web.Wrappers;

namespace Smp.Web.Tests.Unit.Tests.WrapperTests
{
    public class FileWrapperTests
    {
        public class ReadAllTextTests
        {
            private string _fileContents = string.Empty;

            [OneTimeSetUp]
            public void GivenAValidFile_WhenReadAllTextGetsCalled()
            {
                _fileContents = new FileWrapper().ReadAllText("Resources/Email/email-template.html");
            }

            [Test]
            public void ThenTheFileContentsShouldBeAsExpected()
                => Assert.That(_fileContents.StartsWith("<!DOCTYPE html"), Is.True);
        }

        public class ReadAllTextAsyncTests
        {
            private string _fileContents = string.Empty;

            [OneTimeSetUp]
            public async Task GivenAValidFile_WhenReadAllTextAsyncGetsCalled()
            {
                _fileContents = await new FileWrapper().ReadAllTextAsync("Resources/Email/email-template.html");
            }

            [Test]
            public void ThenTheFileContentsShouldBeAsExpected()
                => Assert.That(_fileContents.StartsWith("<!DOCTYPE html"), Is.True);
        }
    }
}
