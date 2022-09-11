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
        [InlineData("* main                0f231f5 [origin/main: ahead 1] Yet another", true, "main", "0f231f5", "origin/main", "Yet another")]
        [InlineData("* 2.0    6c17d51 [origin/2.0] Update version to 2.0.3.325.", true, "2.0", "6c17d51", "origin/2.0", "Update version to 2.0.3.325.")]
        [InlineData("  master d6d20bb [origin/master: behind 74] Update stuff", false, "master", "d6d20bb", "origin/master", "Update stuff")]
        [InlineData("  feature/add-stuff   e5c48b9 [origin/feature/add-stuff] - Adding new relationship", false, "feature/add-stuff", "e5c48b9", "origin/feature/add-stuff", "- Adding new relationship")]
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
