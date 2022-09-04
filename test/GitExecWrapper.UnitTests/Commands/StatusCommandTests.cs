// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using GitExecWrapper.Commands;
using Xunit;

namespace GitExecWrapper.UnitTests.Commands
{
    public class StatusCommandTests
    {
        private const string Repo = "repo";

        private readonly StatusCommand command;


        public StatusCommandTests()
        {
            command = new StatusCommand(Repo);
        }


        [Fact]
        public void CanGetRepoPath()
        {
            // Assert
            command.RepoPath.Should().Be(Repo);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanToString(bool ignored)
        {
            // Arrange
            command.IncludeIgnored(ignored);

            // Act
            var result = command.ToString();

            // Assert
            result.Should().Contain("status");
            result.Should().Contain("--porcelain=2");

            if (ignored)
            {
                result.Should().Contain("--ignored");
            }
            else
            {
                result.Should().NotContain("--ignored");
            }
        }
    }
}
