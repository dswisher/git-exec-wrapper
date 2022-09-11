// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using GitExecWrapper.Parsers;
using GitExecWrapper.UnitTests.Parsers.TestHelpers;
using Xunit;

namespace GitExecWrapper.UnitTests.Parsers
{
    public class RemoteDetailParserTests
    {
        private readonly RemoteDetailParser parser = new ();


        [Fact]
        public void CanParseTypicalRemote()
        {
            // Arrange
            var builder = OutputBuilder.Create()
                .AddLine("* remote github")
                .AddLine("  Fetch URL: git@github.com:Example/fun-repo.git")
                .AddLine("  Push  URL: git@github.com:Example/fun-repo.git")
                .AddLine("  HEAD branch: develop")
                .AddLine("  Remote branches:")
                .AddLine("    develop                  tracked")
                .AddLine("    main                     tracked")
                .AddLine("    refs/remotes/origin/nuke stale (use 'git remote prune' to remove)")
                .AddLine("  Local branches configured for 'git pull':")
                .AddLine("    develop merges with remote develop")
                .AddLine("    main    merges with remote main")
                .AddLine("  Local refs configured for 'git push':")
                .AddLine("    develop pushes to develop (fast-forwardable)")
                .AddLine("    main    pushes to main    (up to date)");

            // Act
            var result = parser.ParseOutput(builder.Build(), null);

            // Assert
            result.Name.Should().Be("github");
            result.HeadBranch.Should().Be("develop");
        }


#if false
* remote origin
  Fetch URL: https://github.com/carina-studio/PixelViewer.git
  Push  URL: https://github.com/carina-studio/PixelViewer.git
  HEAD branch: master
  Remote branches:
    1.0    tracked
    2.0    tracked
    2.6    tracked
    2.7    tracked
    master tracked
  Local branches configured for 'git pull':
    2.0    merges with remote 2.0
    master merges with remote master
  Local refs configured for 'git push':
    2.0    pushes to 2.0    (up to date)
    master pushes to master (local out of date)
#endif

#if false
* remote origin
  Fetch URL: git@github.com:Example/fun-repo.git
  Push  URL: git@github.com:Example/fun-repo.git
  HEAD branch: develop
  Remote branches:
    BUG-1243                                      tracked
    BUG-1981                                      tracked
    BUG-2075                                      tracked
    BUG-2117                                      new (next fetch will store in remotes/origin)
    develop                                       tracked
    downgrade_sqlclient                           tracked
    featture/BUG-2004                             tracked
    feature-Sql-SNI                               tracked
    feature/BUG-2050                              new (next fetch will store in remotes/origin)
    master                                        tracked
  Local branches configured for 'git pull':
    develop merges with remote develop
    master  merges with remote master
  Local refs configured for 'git push':
    develop pushes to develop (local out of date)
    master  pushes to master  (local out of date)
#endif
    }
}
