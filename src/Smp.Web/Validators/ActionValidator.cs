using System;
using System.Collections.Generic;
using Smp.Web.Models;

namespace Smp.Web.Validators
{
    public interface IActionValidator
    {
        IList<Error> ValidateAction(Models.Action action, ActionType actionType);
    }

    public class ActionValidator : IActionValidator
    {
        public IList<Error> ValidateAction(Models.Action action, ActionType actionType)
        {
            var errors = new List<Error>();

            if (action == null)
            {
                errors.Add(new Error("invalid_action", "Action must exist."));
                return errors;
            }
            if (!IsActionCorrectType(action.ActionType, actionType)) errors.Add(new Error("invalid_action", "Action must be of correct type."));
            if (!IsActionIncomplete(action.Completed)) errors.Add(new Error("invalid_action", "Action must not have been completed already."));
            if (!IsActionAlive(action.ExpiresAt)) errors.Add(new Error("invalid_expiry", "Action must not have expired."));

            return errors;
        }

        private static bool IsActionAlive(DateTime expiresAt)
            => DateTime.UtcNow < expiresAt;

        private static bool IsActionCorrectType(ActionType actionTypeOne, ActionType actionTypeTwo)
            => actionTypeOne == actionTypeTwo;

        private static bool IsActionIncomplete(bool complete)
            => !complete;
    }
}