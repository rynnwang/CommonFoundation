Background of Common 4.x

 

To support .NET core 2.0, common is refined to supported both 4.5 & 4.6.2 first. Meanwhile, regarding unavoidable incompatibility between .NET framework and .NET
core, implementation and scope have to be changed to meet goal.

 

Scope Change (Based on aspects)

 

-   Kept:

    -   Common contracts and entity interfaces.

    -   AOP

    -   Restful API

    -   Reflection

    -   Http enhancements and extension. (System.Web based classes is supported in .NET framework only.)

    -   Sandbox and dynamic compiling.

    -   Data access controller (for Microsoft SQL Server)

-   New:

    -   HttpRequestMessage + HttpResponseMessage supports.

-   Removed:

    -   System.Draw based.

    -   Win Form/WPF UI based.

    -   LDAP extension.

-   As new library:

    -   Common service interfaces: moved to Beyova.FunctionService.Generic (TBD)
