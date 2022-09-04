// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using GitExecWrapper.Commands;
using Xunit;

namespace GitExecWrapper.UnitTests.Commands
{
    public class FetchCommandTests
    {
        private const string Repo = "repo";

        private readonly FetchCommand command;


        public FetchCommandTests()
        {
            command = new FetchCommand(Repo);
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
        public void CanToString(bool dryRun)
        {
            // Arrange
            command.DryRun(dryRun);

            // Act
            var result = command.ToString();

            // Assert
            result.Should().Contain("fetch");
            result.Should().Contain("--verbose");

            if (dryRun)
            {
                result.Should().Contain("--dry-run");
            }
            else
            {
                result.Should().NotContain("--dry-run");
            }
        }
    }
}
