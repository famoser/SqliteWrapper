#What is this library#
This library serves as Wrapper between your database entities and the models used in your view.

#Preconditions#
##prepare sqlite##
 - install "Famoser.FrameworkEssentials", "SQLite.Net.Async-PCL" and "SQLite.Net-PCL" from nuget in the Presentation project (the one which is deployed)
 - you may need addtionally an extension in your presentation project for sqlite to work, in UWP this extension can be installed within Visual Studio -> Tools -> Extensions -> install "Sqlite for Universal Windows Platform". Then do a right click in your Presentation project on References, Select "Add Reference", select "Universal Windows" -> Extensions -> mark Sqlite

##prepare your project##
 - have all your models implement the `ISqliteModel` interface with a parameterless constructor
 - have all your entities inherit from `EntityBase` with a parameterless constructor
 - put the `EntityMapAttribute` to all properties in the model you would like to be mapped, you may additionally need the `EntityConversionAttribute` if types of the property in Model & Entity do not match
 - implement the `ISqliteServiceSettingsProvider` in your presentation project
 - implement the `ISqliteService`, or use the one already provided by this library (called `SqliteService`)
 - implement the `ISQLitePlatform`, or use the one already provided inside SQLite.Net-PCL (inside `Sqlite.Net.Platform.*`)
 - create any additional converters you need with the `EntityConversionAttribute` by `EntityConversionAttribute.RegisterConverter()`
 
#Use this library#
 - construct the GenericRepository & use it!

#Examples#
 - There is an example project appended to this library, take a look at the `ModelRepository` inside the folder Repositores for a recommended way to use this library

#Some notes#
 - pass an implemententation of the IExceptionLogger to the GenericRepository so the GenericRepository may not throw exceptions but log them
 - there is already an implementation of an enum (model) to int (entity) converter, so no need to worry about that
 - there are some other commonly used converters already implemented in the namespace Converters.*, you still need to register them if you want to use them
 - the `SqliteService` creates tables automatically
 - usage of an IoC implementation is strongly encuraged (I can recommend the `SimpleIoC` implementation from the MvvmLight library, available on nuget)
