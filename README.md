# Git Exec Wrapper for Dotnet

This is a class library that provides a (hopefully) easy-to-use wrapper around command-line `git`, to facilitate scripting.


# Motivation

While working on a project with many git repositories (79 at last count), I wanted to automate the process of keeping each repository up-to-date on my laptop.
I originally started out using [libgit2sharp](https://github.com/libgit2/libgit2sharp), but ran into two stumbling blocks:

1. `libgit2sharp` does not support SSH when doing a fetch.
1. Since `libgit2sharp` uses a native C library, I could not package my application as a single-file executable.

These are somewhat specific to my use case.
In general, I would highly recommend using `libgit2sharp` if possible, as it is battle tested, well maintained, documented, and does not rely on `git` being installed.

The amazing [simple-exec](https://github.com/adamralph/simple-exec) library is used to actually run the `git` commands.
The value-add in this wrapper library is the code that parses the output from commands like `git status`, making it easy to consume in scripts.


# Usage

TBD.

