using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Bom.Utils
{
    public static class Dev
    {
        public enum ImproveArea
        {
            Undefined = 0,
            Performance = 1,
            CodeQuality = 2,
            Other = 3,
            ToCheck = 4,
        }

        public enum Urgency
        {
            Undefined = 0,
            Low = 1,
            Middle = 2,
            High = 3
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Todo(string? msg = null, Urgency urgency = Urgency.Undefined)
        {
            var toWriteMsg = "Todo called";
            if (string.IsNullOrWhiteSpace(msg))
            {
                toWriteMsg = $"Todo called: '{msg}'";
            }
            if(urgency != Urgency.Undefined)
            {
                toWriteMsg += $" [Urgency: {urgency}]";
            }
            System.Diagnostics.Debug.WriteLine(toWriteMsg);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void PossibleImprovment(string? msg = null, ImproveArea improveArea = ImproveArea.Undefined, Urgency urgency = Urgency.Undefined)
        {
            var toWriteMsg = "Improvment!";
            if (string.IsNullOrWhiteSpace(msg))
            {
                toWriteMsg = $"Improvment: '{msg}'";
            }
            if (improveArea != ImproveArea.Undefined)
            {
                toWriteMsg += $" [Type: {improveArea}]";
            }
            if (urgency != Urgency.Undefined)
            {
                toWriteMsg += $" [Urgency: {urgency}]";
            }
            System.Diagnostics.Debug.WriteLine(toWriteMsg);
        }
    }
}
