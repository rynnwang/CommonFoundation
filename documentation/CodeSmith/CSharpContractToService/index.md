This page is to introduce how to use *Beyova.Common* + *Beyova.CodeSmith* to
generate

-   SQL

    -   Table

    -   StoredProcedure

-   CSharp

    -   DataAccessController

    -   ServiceCore

by {EntityModel} + {CriteriaModel} under common patterned scenarios.

 

Generate DataAccessController:
------------------------------

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
var dataAccessControllerOptions = new List<DataAccessControllerGenerationOptions>()
{
    new DataAccessControllerGenerationOptions{
        EntityType=typeof({EntityType1}), 
        CriteriaType=typeof({CriteriaType1})
    },
    new DataAccessControllerGenerationOptions{
        EntityType=typeof({EntityType2}), 
        CriteriaType=typeof({CriteriaType2})
    },
    ...
};

DataAccessControllerGenerator.GenerateDataAccessControllers(
    {OutputDirectoryInfo}, 
    dataAccessControllerOptions, 
    {namespace}
);
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

 

 
