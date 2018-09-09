using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.CryptographyServiceTests
{
    public class CryptographyServiceTestBase
    {
        protected ICryptographyService CryptographyService;

        public void Setup()
        {
            CryptographyService = new CryptographyService();
        }
    }
}