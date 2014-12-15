How to make a release of the RLToolkit
=========
<br>
This is a quick checklist to ensure i've done all the steps properly when releasing a new version.<br>
I won't keep track of the actual checks in these lists but it can be helpful if somebody wants to recreate their own package of this toolkit.<br>
<br>
<br>
1- Compile the platform you want to release
2- Fix any compilation errors/warning and ensure the XMLDocumentation is done for everything
3- In nUnit, run the unit tests and ensure they all pass
4- In Doxygen, compile the htmlDoc for the project
5- Make a package (zip/tar/etc) named RLToolkit-vX.Y.Z-PlatformPackage
6- Include the following folders: bin, html
7- Include the documentation files (Readme, licence, how to, structure, etc)
8- Make a new release on Git and add the proper description
9- Add the package to the release.
<br>
<br>
Note: We should release Linux & Windows, both at the same time. Ideally prepare both windows and linux packages before doing the Git release.<br>
<br>