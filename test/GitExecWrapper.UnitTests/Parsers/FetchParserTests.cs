// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.InteropServices;
using FluentAssertions;
using GitExecWrapper.Models;
using GitExecWrapper.Parsers;
using GitExecWrapper.UnitTests.Parsers.TestHelpers;
using Xunit;

namespace GitExecWrapper.UnitTests.Parsers
{
    public class FetchParserTests
    {
        private readonly FetchParser parser = new ();

        [Theory]
        [InlineData("   c6990d4e..7135a954  develop -> origin/develop", RefStatus.Fetched, "c6990d4e..7135a954", "develop", "origin/develop")]
        [InlineData(" * [new branch]        develop -> origin/develop", RefStatus.NewRef, "[new branch]", "develop", "origin/develop")]
        [InlineData(" = [up to date]        develop -> origin/develop", RefStatus.UpToDate, "[up to date]", "develop", "origin/develop")]
        public void CanParseOneBranch(string line, RefStatus status, string summary, string from, string to)
        {
            // Arrange
            const string fromRepo = "github.com:Example/fun-repo";
            var stderr = OutputBuilder.Create()
                .AddLine($"From {fromRepo}")
                .AddLine(line);

            // Act
            var result = parser.ParseOutput(string.Empty, stderr.Build());

            // Assert
            result.FromRepo.Should().Be(fromRepo);
            result.Items.Should().HaveCount(1);

            var item = result.Items[0];

            item.Status.Should().Be(status);
            item.Summary.Should().Be(summary);
            item.From.Should().Be(from);
            item.To.Should().Be(to);
        }


        [Fact]
        public void CanParseMultiple()
        {
            // Arrange
            var stderr = OutputBuilder.Create()
                .AddLine("From github.com:Example/fun-repo")
                .AddLine("   7b2d11b5..8f5acfa8  develop                 -> origin/develop")
                .AddLine(" = [up to date]        bugfix/stuff-is-really-broken -> origin/bugfix/stuff-is-really-broken")
                .AddLine(" = [up to date]        master                  -> origin/master")
                .AddLine("   326a8817..215b5531  release                 -> origin/release");

            // Act
            var result = parser.ParseOutput(string.Empty, stderr.Build());

            // Assert
            result.FromRepo.Should().Be("github.com:Example/fun-repo");
            result.Items.Should().HaveCount(4);

            // spot-check a few things
            result.Items[0].Status.Should().Be(RefStatus.Fetched);
            result.Items[0].Summary.Should().Be("7b2d11b5..8f5acfa8");

            result.Items[1].Status.Should().Be(RefStatus.UpToDate);
            result.Items[1].From.Should().Be("bugfix/stuff-is-really-broken");
            result.Items[1].To.Should().Be("origin/bugfix/stuff-is-really-broken");

            result.Items[2].Status.Should().Be(RefStatus.UpToDate);

            result.Items[3].Status.Should().Be(RefStatus.Fetched);
        }


        [Fact]
        public void CanParseHttps()
        {
            // Arrange
            var stderr = OutputBuilder.Create()
                .AddLine("POST git-upload-pack (155 bytes)")
                .AddLine("From https://github.com/Example/fun-repo")
                .AddLine(" = [up to date]      master        -> origin/master")
                .AddLine(" = [up to date]      mac-packaging -> origin/mac-packaging");

            // Act
            var result = parser.ParseOutput(string.Empty, stderr.Build());

            // Assert
            result.FromRepo.Should().Be("https://github.com/Example/fun-repo");
            result.Items.Should().HaveCount(2);
        }


        [Theory]
        [InlineData("POST git-upload-pack (155 bytes)")]
        [InlineData("POST git-upload-pack (gzip 1473 to 795 bytes)")]
        [InlineData("Auto packing the repository in background for optimum performance.")]
        [InlineData("See \"git help gc\" for manual housekeeping.")]
        public void CanIgnoreStuff(string line)
        {
            // Arrange
            var stderr = OutputBuilder.Create().AddLine(line);

            // Act
            var result = parser.ParseOutput(string.Empty, stderr.Build());

            // Assert
            result.Items.Should().HaveCount(0);
        }
    }
}
