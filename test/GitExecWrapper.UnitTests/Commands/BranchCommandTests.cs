// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using GitExecWrapper.Commands;
using Xunit;

namespace GitExecWrapper.UnitTests.Commands
{
    public class BranchCommandTests
    {
        private const string Repo = "repo";

        private readonly BranchCommand command;


        public BranchCommandTests()
        {
            command = new BranchCommand(Repo);
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
        public void CanToString(bool all)
        {
            // Arrange
            command.All(all);

            // Act
            var result = command.ToString();

            // Assert
            result.Should().Contain("branch");
            result.Should().Contain("--list");
            result.Should().Contain("-vv");
            result.Should().Contain("--no-color");

            if (all)
            {
                result.Should().Contain("--all");
            }
            else
            {
                result.Should().NotContain("--all");
            }
        }
    }
}
