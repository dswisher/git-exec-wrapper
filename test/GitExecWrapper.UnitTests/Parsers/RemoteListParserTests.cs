// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using GitExecWrapper.Parsers;
using GitExecWrapper.UnitTests.Parsers.TestHelpers;
using Xunit;

namespace GitExecWrapper.UnitTests.Parsers
{
    public class RemoteListParserTests
    {
        private readonly RemoteListParser parser = new ();


        [Fact]
        public void CanParseRemotePair()
        {
            // Arrange
            var builder = OutputBuilder.Create()
                .AddLine("origin")
                .AddLine("extra");

            // Act
            var result = parser.ParseOutput(builder.Build(), null);

            // Assert
            result.Should().BeEquivalentTo("origin", "extra");
        }
    }
}
