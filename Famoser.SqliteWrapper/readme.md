What is this library
This library serves as Wrapper between your database entities and you models used in your view.

Preconditions:
 - use an IoC implementation (I can recommend the SimpleIoC implementation from the MvvmLight library)
 - have all your models implement the ISqliteModel interface with a parameterless constructor
 - have all your entities inherit from EntityBase with a parameterless constructor
 - put the EntityMapAttribute to all properties in the model you would like to be mapped, you may additionally need the EntityConversionAttribute if types do not match
 - in your presentation projekt install the corresponding Sqlite.Net.Platform.* package

Prepare to use this library:
 - implement the ISqliteServiceSettingsProvider interface
 - register the SqliteService against the ISqliteService interface
 - register the ISQLitePlatform against the platform specific implementation

Use this library
 - register all hand-made converters for the EntityConversionAttribute by EntityConversionAttribute.RegisterConverter();
 - construct the GenericRepository & use it!

Some notes:
 - pass an implemententation of the IExceptionLogger to the GenericRepository so the GenericRepository may not throw exceptions but log them
 - there is already an implementation of an enum (model) to int (entity) converter, so no need to worry about that
