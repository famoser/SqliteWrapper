﻿namespace Famoser.SqliteWrapper.Attributes.Interface
{
    public interface IEntityMappingConverter
    {
        object Convert(object val);

        object ConvertBack(object val);
    }
}