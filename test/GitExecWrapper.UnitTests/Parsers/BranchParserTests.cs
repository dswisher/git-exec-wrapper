// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using GitExecWrapper.Parsers;
using GitExecWrapper.UnitTests.Parsers.TestHelpers;
using Xunit;

namespace GitExecWrapper.UnitTests.Parsers
{
    public class BranchParserTests
    {
        private readonly BranchParser parser = new ();

        [Theory]
        [InlineData("* main 826d737 Initial", true, "main", "826d737", null, "Initial")]
        [InlineData("  main    981421d [origin/main] Merge pull request #1", false, "main", "981421d", "origin/main", "Merge pull request #1")]
        public void CanParseOneBranch(string line, bool isCurrent, string branchName, string commitSha, string upstreamBranch, string message)
        {
            // Arrange
            var stdout = OutputBuilder.Create()
                .AddLine(line);

            // Act
            var result = parser.ParseOutput(stdout.Build());

            // Assert
            result.Items.Should().HaveCount(1);

            var item = result.Items[0];

            item.IsCurrent.Should().Be(isCurrent);
            item.BranchName.Should().Be(branchName);
            item.CommitSha.Should().Be(commitSha);
            item.UpstreamBranch.Should().Be(upstreamBranch);
            item.Message.Should().Be(message);
        }
    }
}
