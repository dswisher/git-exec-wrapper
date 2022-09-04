// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using GitExecWrapper.Helpers;
using Xunit;

namespace GitExecWrapper.UnitTests.Helpers
{
    public class DebugHelpersTests
    {
        [Fact]
        public void CanHandleNulls()
        {
            // Act
            DebugHelpers.Dump(1, null, null);

            // No exception == success
        }
    }
}
