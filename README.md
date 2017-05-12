# IncrementVersion
Small utility to increment the version number of a project based upon a set of rules.

Usage: IncrementVersion (filename) (type)
* (filename) should be the full path to the Assembly Information file.
* (type) should be either "Debug" or "Release" without the quotes.
* (type) is optional and defaults to "Debug".
                    
To use this utility, copy the IncrementVersion executable to your project directory, and add the following line to the Pre-Build event command line in the Build Events tab of your project's properties page.

"$(ProjectDir)IncrementVersion.exe" "$(ProjectDir)Properties\AssemblyInfo.cs" "$(ConfigurationName)"

There are four version numbers stored in the Assembly Information file they are structured:

major.minor.build.revision

The "Debug" type will only increment the revision number, it does not affect the other parts of the version.

The "Release" type will increment the build, incrementing the minor and resetting to 0 when the build reaches 10.  Likewise, when the minor reaches 10 the major is incremented and the minor is reset to 0.  The "Release" type always resets the revision number to 0.

If your version number was 2.6.3.452, then by reading that you can know that there have been 263 Release compiles, and 452 debug compiles since the last release was compiled.

The Increment Version utility does not maintain its own value for the version number and works solely with what is in the Assembly Information file.  For this reason, you are still free to change your applications version numbers through the properties screen or by editing the Assembly Information file directly.
