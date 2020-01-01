using System;
using System.Collections.Generic;
using System.Text;
using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.ActionValidatorTests
{
    public class ActionValidatorTestBase
    {
        protected IActionValidator ActionValidator { get; set; }

        public void Setup()
        {
            ActionValidator = new ActionValidator();
        }
    }
}
