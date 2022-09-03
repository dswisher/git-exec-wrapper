// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text;

namespace GitExecWrapper.UnitTests.Parsers.TestHelpers
{
    public class OutputBuilder
    {
        private readonly StringBuilder builder = new StringBuilder();

        public static OutputBuilder Create()
        {
            return new OutputBuilder();
        }


        public OutputBuilder AddLine(string line)
        {
            builder.AppendLine(line);

            return this;
        }


        public string Build()
        {
            return builder.ToString();
        }
    }
}
