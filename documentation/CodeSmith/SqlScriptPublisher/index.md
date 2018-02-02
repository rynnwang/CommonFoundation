This page is to introduce how to use *Beyova.Common* + *Beyova.CodeSmith* to
publish/merge batch of SQL scripts.

Scripts should be categorized by type:

-   Table

-   StoredProcedure

-   Function

-   View

-   Data

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
var publisher = new SqlScriptPublisher({SqlRootDirectory}, {OutputDirectory});
int count = publisher.Publish(
    SqlScriptType.All, 
    createDropStatement: true, 
    addDefaultScripts: true, 
    defaultSchema: "dbo"
);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

 

 
