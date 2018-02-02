using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Beyova;
using Beyova.Gravity;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Beyova.Gravity.TestClient")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("Beyova.Gravity.TestClient")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyDescription(BeyovaPropertyConstants.AssemblyDescription)]
[assembly: AssemblyCompany(BeyovaPropertyConstants.Company)]
[assembly: AssemblyCopyright(BeyovaPropertyConstants.Copyright)]
[assembly: AssemblyTrademark(BeyovaPropertyConstants.AssemblyTrademark)]

[assembly: GravityProtocol()]
[assembly: GravityEventHook(typeof(Beyova.Gravity.TestClient.TestClientEventHook))]


// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ab8b7e8d-8e97-4586-9753-16a3ae43cd51")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0")]
//[assembly: AssemblyFileVersion("1.0.0.0")]
