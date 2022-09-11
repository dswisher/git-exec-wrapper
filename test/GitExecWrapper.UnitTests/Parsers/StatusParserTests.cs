// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using GitExecWrapper.Models;
using GitExecWrapper.Parsers;
using GitExecWrapper.UnitTests.Parsers.TestHelpers;
using Xunit;

namespace GitExecWrapper.UnitTests.Parsers
{
    public class StatusParserTests
    {
        private const string FilePath = ".gitignore";
        private const string DirPath = "test/obj/project.assets.json";
        private const string Modes = "000000 100644 100644";
        private const string Commits = "0000000000000000000000000000000000000000 73325c8c52f86fc932bbebfda1ad04f424330a79";
        private const string TrackedFile = " " + Modes + " " + Commits + " " + FilePath;

        private readonly StatusParser parser = new ();


        [Fact]
        public void CanParseEmptyResponse()
        {
            // Arrange
            var builder = OutputBuilder.Create()
                .AddLine("# branch.oid (initial)")
                .AddLine("# branch.head main");

            // Act
            var result = parser.ParseOutput(builder.Build());

            // Assert
            result.Items.Should().BeEmpty();
        }


        [Theory]
        [InlineData("1 A. N..." + TrackedFile, FileStatus.Added, FileStatus.Unchanged, FilePath)]
        [InlineData("1 .M N..." + TrackedFile, FileStatus.Unchanged, FileStatus.Modified, FilePath)]
        [InlineData("1 D. N..." + TrackedFile, FileStatus.Deleted, FileStatus.Unchanged, FilePath)]
        [InlineData("? " + FilePath, FileStatus.Unknown, FileStatus.Unknown, FilePath)]
        [InlineData("! " + DirPath, FileStatus.Unknown, FileStatus.Ignored, DirPath)]
        public void CanParseItems(string line, FileStatus expectedStagedStatus, FileStatus expectedUnstagedStatus, string expectedPath)
        {
            // Arrange
            var builder = OutputBuilder.Create().AddLine(line);

            // Act
            var result = parser.ParseOutput(builder.Build());

            // Assert
            result.Items.Should().HaveCount(1);

            result.Items[0].IndexStatus.Should().Be(expectedStagedStatus);
            result.Items[0].WorkDirStatus.Should().Be(expectedUnstagedStatus);

            result.Items[0].Path.Should().Be(expectedPath);
        }


        [Theory]
        [InlineData("# branch.ab +0 -0", 0, 0)]
        [InlineData("# branch.ab +2 -0", 2, 0)]
        [InlineData("# branch.ab +0 -3", 0, 3)]
        [InlineData("# branch.ab +413 -301", 413, 301)]
        public void CanParseAheadBehind(string line, int? ahead, int? behind)
        {
            // Arrange
            var builder = OutputBuilder.Create().AddLine(line);

            // Act
            var result = parser.ParseOutput(builder.Build());

            // Assert
            result.CommitsAhead.Should().Be(ahead);
            result.CommitsBehind.Should().Be(behind);
        }


        [Theory]
        [InlineData("# branch.oid fa6f4d4459f51a593cf443fbce2cbf1f3821d0ab", "fa6f4d4459f51a593cf443fbce2cbf1f3821d0ab")]
        [InlineData("# branch.oid (initial)", null)]
        public void CanParseCurrentCommit(string line, string commit)
        {
            // Arrange
            var builder = OutputBuilder.Create().AddLine(line);

            // Act
            var result = parser.ParseOutput(builder.Build());

            // Assert
            result.CurrentCommit.Should().Be(commit);
        }


        [Theory]
        [InlineData("# branch.head main", "main")]
        [InlineData("# branch.head (detached)", null)]
        public void CanParseCurrentBranch(string line, string branch)
        {
            // Arrange
            var builder = OutputBuilder.Create().AddLine(line);

            // Act
            var result = parser.ParseOutput(builder.Build());

            // Assert
            result.CurrentBranch.Should().Be(branch);
        }


        [Theory]
        [InlineData("# branch.upstream origin/develop", "origin/develop")]
        public void CanParseUpstream(string line, string upstream)
        {
            // Arrange
            var builder = OutputBuilder.Create().AddLine(line);

            // Act
            var result = parser.ParseOutput(builder.Build());

            // Assert
            result.Upstream.Should().Be(upstream);
        }



        [Theory]
        [InlineData("# stash 1", 1)]
        public void CanParseStashed(string line, int stashed)
        {
            // Arrange
            var builder = OutputBuilder.Create().AddLine(line);

            // Act
            var result = parser.ParseOutput(builder.Build());

            // Assert
            result.StashCount.Should().Be(stashed);
        }
    }
}
