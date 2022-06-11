// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace GitExecWrapper.Models
{
    public class StatusItem
    {
        public FileStatus IndexStatus { get; set; }
        public FileStatus WorkDirStatus { get; set; }
        public string Path { get; set; }
    }
}
