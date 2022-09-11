// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GitExecWrapper.Commands;
using GitExecWrapper.Models;
using GitExecWrapper.Parsers;
using GitExecWrapper.Services;
using Moq;
using Xunit;

namespace GitExecWrapper.UnitTests.Commands
{
    public class RemoteCommandTests
    {
        private const string Repo = "repo";
        private const string RemoteName1 = "origin";

        private readonly Mock<IGitRunner> runner = new ();
        private readonly Mock<IParser<List<string>>> listParser = new ();
        private readonly Mock<IParser<RemoteItem>> itemParser = new ();
        private readonly CancellationToken token = CancellationToken.None;

        private readonly RemoteCommand command;

        public RemoteCommandTests()
        {
            command = new RemoteCommand(Repo, runner.Object, listParser.Object, itemParser.Object);
        }


        [Fact]
        public async Task CanRun()
        {
            // Arrange
            var names = new List<string> { RemoteName1 };
            var item1 = new RemoteItem
            {
                Name = RemoteName1
            };

            runner.Setup(x => x.ExecAsync(Repo, "remote", listParser.Object, token))
                .ReturnsAsync(names)
                .Verifiable();

            runner.Setup(x => x.ExecAsync(Repo, $"remote show {RemoteName1}", itemParser.Object, token))
                .ReturnsAsync(item1)
                .Verifiable();

            // Act
            var result = await command.ExecAsync(token);

            // Assert
            result.Items.Should().HaveCount(1);
            result.Items[0].Name.Should().Be(RemoteName1);

            runner.VerifyAll();
        }
    }
}
